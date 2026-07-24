import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { AuthProvider, useAuth } from "./context/AuthContext";
import { ToastProvider } from "./context/ToastContext";
import { ProtectedRoute } from "./components/ProtectedRoute";
import { AppLayout } from "./components/layout/AppLayout";
import { ROLES } from "./utils/roles";

import { LoginPage } from "./pages/auth/LoginPage";
import { RegisterPage } from "./pages/auth/RegisterPage";
import { DashboardPage } from "./pages/dashboard/Dashboard";
import { DepartmentsPage } from "./pages/departments/DepartmentsPage";
import { DoctorsPage } from "./pages/doctors/DoctorsPage";
import { DoctorDetailPage } from "./pages/doctors/DoctorDetailPage";
import { PatientsPage } from "./pages/patients/PatientsPage";
import { PatientDetailPage } from "./pages/patients/PatientDetailPage";
import { MyProfilePage } from "./pages/patients/MyProfilePage";
import { AppointmentsPage } from "./pages/appointments/AppointmentsPage";
import { AppointmentDetailPage } from "./pages/appointments/AppointmentDetailPage";
import { BookAppointmentPage } from "./pages/appointments/BookAppointmentPage";
import { EditAppointmentPage } from "./pages/appointments/EditAppointmentPage";
import { MyAppointmentsPage } from "./pages/appointments/MyAppointmentsPage";
import { FindAppointmentPage } from "./pages/appointments/FindAppointmentPage";
import { NotFoundPage } from "./pages/NotFoundPage";

function PublicOnlyRoute({ children }) {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? <Navigate to="/" replace /> : children;
}

function AppRoutes() {
  return (
    <Routes>
      <Route
        path="/login"
        element={
          <PublicOnlyRoute>
            <LoginPage />
          </PublicOnlyRoute>
        }
      />
      <Route
        path="/register"
        element={
          <PublicOnlyRoute>
            <RegisterPage />
          </PublicOnlyRoute>
        }
      />

      <Route element={<ProtectedRoute />}>
        <Route element={<AppLayout />}>
          <Route path="/" element={<DashboardPage />} />
          <Route path="/departments" element={<DepartmentsPage />} />
          <Route path="/doctors" element={<DoctorsPage />} />
          <Route path="/doctors/:id" element={<DoctorDetailPage />} />

          <Route element={<ProtectedRoute roles={[ROLES.ADMIN, ROLES.DOCTOR]} />}>
            <Route path="/patients" element={<PatientsPage />} />
            <Route path="/patients/:id" element={<PatientDetailPage />} />
          </Route>

          <Route element={<ProtectedRoute roles={[ROLES.PATIENT]} />}>
            <Route path="/my-profile" element={<MyProfilePage />} />
            <Route path="/my-appointments" element={<MyAppointmentsPage />} />
            <Route path="/appointments/book" element={<BookAppointmentPage />} />
            <Route path="/appointments/:id/edit" element={<EditAppointmentPage />} />
          </Route>

          <Route element={<ProtectedRoute roles={[ROLES.ADMIN]} />}>
            <Route path="/appointments" element={<AppointmentsPage />} />
          </Route>

          <Route path="/appointments/find" element={<FindAppointmentPage />} />
          <Route path="/appointments/:id" element={<AppointmentDetailPage />} />
        </Route>
      </Route>

      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <ToastProvider>
        <AuthProvider>
          <AppRoutes />
        </AuthProvider>
      </ToastProvider>
    </BrowserRouter>
  );
}
