# Clinic Management System — Frontend

A React (Vite) frontend for the Clinic Management System backend, covering
every feature exposed by the controllers you provided: Auth, Departments,
Doctors (+ working hours + leaves), and Patients.

## Stack

- React 19 + Vite
- React Router v7 (client-side routing, role-protected routes)
- Axios (JWT attached automatically, silent refresh-token retry on 401)
- Plain CSS with a small design-token system (no UI framework) — palette is
  ink black / charcoal / grey / mint green, per your brief

## Getting started

```bash
cd frontend
npm install
npm run dev
```

Set your API's base URL in `.env`:

```
VITE_API_BASE_URL=https://localhost:7000/api
```

(point it at wherever your ASP.NET backend is actually listening — check
`launchSettings.json` in your backend project for the exact port)

If your backend uses a self-signed HTTPS dev cert, you may need to trust it
first (`dotnet dev-certs https --trust`) or the browser will block requests.

## Folder structure

```
src/
  api/            one file per controller (authApi, departmentsApi, doctorsApi,
                   doctorLeavesApi, doctorWorkingHoursApi, patientsApi)
  components/
    common/       Button, Card, Table, Modal, ConfirmDialog, Badge, Pagination,
                   SearchInput, FormField/TextInput/TextArea/Select, Loader, EmptyState
    layout/       Sidebar, Topbar, AppLayout
    ProtectedRoute.jsx
  context/        AuthContext (login/register/logout, JWT decode), ToastContext
  hooks/          usePaginatedList (drives every list+search+pagination page)
  pages/
    auth/         LoginPage, RegisterPage
    dashboard/    DashboardPage
    departments/  DepartmentsPage + DepartmentFormModal
    doctors/      DoctorsPage, DoctorDetailPage (tabs), DoctorFormModal,
                   WorkingHoursTab/WorkingHourFormModal, LeavesTab/LeaveFormModal
    patients/     PatientsPage (admin/doctor), PatientDetailPage,
                   MyProfilePage (patient's own profile, view/create/edit)
    appointments/ AppointmentsPage (admin list), AppointmentDetailPage
                   (role-gated confirm/complete/cancel/delete/edit actions),
                   BookAppointmentPage + EditAppointmentPage (patient),
                   MyAppointmentsPage (patient), FindAppointmentPage
                   (look up by ID), shared AppointmentForm + statusTone.js
  styles/         theme.css (design tokens), global.css (base + shared page classes)
  utils/          storage.js (token storage), jwt.js (decode claims), roles.js
```

## How auth/roles work

- `POST /auth/login` is expected to return an access token (and ideally a
  refresh token) in the response body. The frontend decodes the JWT on the
  client to read `role` / `nameid` / `email` claims — it doesn't call a
  separate "me" endpoint, since none was in the controllers you shared.
- Route protection mirrors your `[Authorize(Roles = ...)]` / policy attributes:
  - `/patients` and `/patients/:id` — Admin or Doctor only
  - `/my-profile` — Patient only
  - Create/Edit/Delete buttons in the Doctors and Departments UI are hidden
    outside of Admin (matching the commented-out `[Authorize]` attributes in
    `DoctorsController` / `DepartmentsController` — **flip those back on in
    the backend**, they're currently commented out, so right now those
    endpoints are open to anyone).
- On a 401, the client tries `/auth/refresh-token` once and retries the
  original request; if that also fails, it clears tokens and redirects to
  `/login`.

## Appointments — routes and a backend gap worth knowing about

Routes added for `AppointmentsController`:

| Route | Who can access it | Backend policy it matches |
|---|---|---|
| `/appointments` | Admin | `GetAll` — `AdminOnly` |
| `/appointments/:id` | Any authenticated user | `GetById` — `[Authorize]` |
| `/appointments/:id/edit` | Patient | `Update` — `PatientOnly` |
| `/appointments/book` | Patient | `Create` — `PatientOnly` |
| `/appointments/find` | Any authenticated user | (client-side only, jumps to `/appointments/:id`) |
| `/my-appointments` | Patient | (client-side cache — see below) |

On the appointment detail page, the Confirm / Complete / Cancel / Delete
buttons are shown based on role, matching the controller's policies exactly
(`Confirm`/`Cancel` → AdminOrDoctor, `Complete` → DoctorOnly, `Delete` →
AdminOnly, `Update` → PatientOnly).

**Gap to flag:** the controller has no "list appointments for the current
doctor" or "list appointments for the current patient" endpoint — only
`GetAll` (Admin-only) and `GetById`. That means, as written today:

- A **patient** has no API-backed way to see their own appointment history.
  `MyAppointmentsPage` works around this by remembering, in that browser's
  `localStorage`, the IDs of appointments the patient has booked or looked
  up (see `src/utils/appointmentCache.js`). It's a client-side convenience,
  clearly labeled as such in the UI — it won't show bookings made from a
  different browser/device, and it's lost if site data is cleared.
- A **doctor** has no API-backed way to see their assigned appointments at
  all. `FindAppointmentPage` lets a doctor jump straight to `/appointments/:id`
  if they already know the ID (e.g. shared by an admin or a patient), but
  there's no doctor-facing list today.

The clean fix is a backend addition — something like
`GET /api/doctors/{doctorId}/appointments` and `GET /api/patients/{patientId}/appointments`
(or a `mine=true` query param on the existing `GetAll`), following the same
pattern as `DoctorLeavesController` / `DoctorWorkingHoursController`. Once
either exists, swap `MyAppointmentsPage`'s data source over to it and this
whole cache workaround can be deleted.

## Assumptions you'll likely need to adjust

I don't have your actual DTOs, so field names in the forms are best guesses
based on the entities mentioned in your architecture (Doctors, Patients,
Appointments, MedicalRecords, AvailabilitySlots):

- **Doctor**: `firstName`, `lastName`, `email`, `phone`, `specialization`,
  `departmentId`, `licenseNumber`, `bio`
- **Patient**: `firstName`, `lastName`, `phone`, `dateOfBirth`, `gender`,
  `bloodType`, `address`
- **Department**: `name`, `description`
- **Doctor leave**: `startDate`, `endDate`, `reason`, `status`
- **Working hours**: `dayOfWeek`, `startTime`, `endTime`
- **Appointment**: `doctorId`, `appointmentDate` (ISO datetime), `reasonForVisit`,
  plus read-only `status`, `patientName`/`patientFirstName`/`patientLastName`,
  `doctorName`/`doctorFirstName`/`doctorLastName` on the response DTO (the
  frontend falls back gracefully between the "one full name field" shape and
  the "first/last name fields" shape — see `personDisplayName()` in
  `src/pages/appointments/statusTone.js`)

If your DTOs use different property names, update the `form` state objects
in the matching `*FormModal.jsx` file — the API calls themselves don't need
to change.

The response envelope is normalized in `src/api/axiosClient.js` via `unwrap`
(single items) and `unwrapList` (paginated lists → `{ items, pagination }`).
It handles a couple of likely shapes for `ApiResponseFactory.Success(...)` /
`ApiResponse<T>.SuccessResponse(...)`, but if your actual JSON shape differs,
that's the one place to adjust.

## Not included

- **Medical records** — the `MedicalRecordsController.cs` you shared is
  entirely commented out on the backend, so there's no live endpoint to
  build against yet. Once you uncomment/finish it, the same patterns used
  in `doctorLeavesApi.js` / `LeavesTab.jsx` will drop in easily.
- **Appointments / availability slots** — mentioned in your architecture
  notes but no controller was provided, so there's nothing to wire up yet.

## Notes

- `npm run build` has been verified to compile cleanly.
- Global fonts (Space Grotesk / Inter / JetBrains Mono) load from Google
  Fonts via `index.html` — swap for self-hosted fonts if you need to work
  offline or avoid the external request.
