# Project-3
# 🏥 Clinic Management System API

A production-style **ASP.NET Core Web API** for managing a modern clinic, built using **Clean Architecture principles**, **Entity Framework Core**, **JWT Authentication**, **Role-Based Authorization**, **OAuth 2.0 (Google Login)**, and comprehensive REST API best practices.

The system provides secure management of patients, doctors, appointments, medical records, prescriptions, departments, authentication, and user accounts while following scalable enterprise-level architecture.

---

# 🚀 Features

## 🔐 Authentication & Authorization

The application provides a complete authentication and authorization system.

### Authentication

- JWT Access Tokens
- Refresh Tokens
- Secure Password Hashing
- Login
- Register
- Logout
- Refresh Token
- Google OAuth Login
- Google OAuth Registration

### Authorization

Role-Based Authorization Policies are implemented instead of directly checking roles.

Supported Roles:

- Administrator
- Doctor
- Patient

Authorization Policies

| Policy | Allowed Roles |
|---------|---------------|
| AdminOnly | Admin |
| DoctorOnly | Doctor |
| PatientOnly | Patient |
| AdminOrDoctor | Admin, Doctor |
| AdminDoctorPatient | Admin, Doctor, Patient |

---

# 👥 User Management

The system supports managing system users.

### Administrator

Can:

- Manage all users
- Manage departments
- View all appointments
- View all medical records
- View all prescriptions
- Delete medical records
- Delete prescriptions

### Doctor

Can:

- View assigned appointments
- Create medical records
- Update own medical records
- Create prescriptions
- Update own prescriptions
- View only patients assigned to appointments

### Patient

Can:

- Register
- Login
- Login with Google
- View own appointments
- View own medical records
- View own prescriptions

---

# 🏥 Department Management

Departments organize doctors inside the clinic.

Features

- Get All Departments
- Get Department By Id
- Search Departments
- Create Department
- Update Department
- Soft Delete Department

Authorization

| Endpoint | Access |
|-----------|---------|
| Get | Admin / Doctor / Patient |
| Search | Admin / Doctor / Patient |
| Create | Admin |
| Update | Admin |
| Delete | Admin |

---

# 👨‍⚕️ Doctor Management

Features

- View doctors
- View doctor details
- Search doctors
- Create doctor
- Update doctor
- Soft delete doctor

Doctors belong to departments.

---

# 🧑‍🤝‍🧑 Patient Management

Features

- View patients
- Patient details
- Search patients
- Update patient profile
- Soft delete patient

---

# 📅 Appointment Management

Appointments connect doctors with patients.

Features

- Create Appointment
- Update Appointment
- Cancel Appointment
- Complete Appointment
- View Appointments
- Appointment Filtering
- Appointment Pagination

Business Rules

- Appointment cannot overlap with another appointment.
- Doctor availability is validated.
- Appointment status is managed automatically.
- Only completed appointments can have medical records.

Appointment Status

- Pending
- Confirmed
- Completed
- Cancelled

---

# 📋 Medical Records

Medical records are created only after a completed appointment.

Features

- Get All Medical Records
- Get Medical Record By Id
- Create Medical Record
- Update Medical Record
- Delete Medical Record

Business Rules

- One Medical Record per Appointment
- Appointment must be Completed
- Doctor can only manage medical records created from appointments assigned to them.
- Patients can only view their own records.
- Admin can access all records.

---

# 💊 Prescriptions

Each Medical Record can contain multiple prescriptions.

Features

- Get All Prescriptions
- Get Prescription By Id
- Create Prescription
- Update Prescription
- Delete Prescription

Business Rules

- Prescription belongs to one Medical Record.
- Doctor can only manage prescriptions belonging to their own medical records.
- Patients can only view their own prescriptions.
- Admin can manage all prescriptions.

---

# 🔍 Searching

Implemented search functionality for:

- Departments
- Doctors
- Patients

---

# 📄 Pagination

Implemented generic pagination for every listing endpoint.

Supports

- Page Number
- Page Size
- Pagination Metadata

---

# ❌ Global Exception Handling

The project uses a centralized **Global Exception Handler** to provide consistent and meaningful error responses across the entire application.

Instead of returning framework-generated exceptions, every exception is transformed into a standardized API response.

### Supported Custom Exceptions

| Exception | HTTP Status Code | Purpose |
|------------|-----------------|----------|
| BadRequestException | 400 | Invalid request or business validation failure |
| ValidationException | 400 | Input validation errors |
| UnauthorizedException | 401 | Authentication required |
| ForbiddenException | 403 | User is authenticated but lacks permission |
| NotFoundException | 404 | Requested resource does not exist |
| ConflictException | 409 | Business conflict (duplicate data, invalid state, etc.) |

Example Error Response

```json
{
    "success": false,
    "message": "Medical record with ID '5' was not found.",
    "data": null,
    "errors": null,
    "timestamp": "2026-07-25T18:45:21Z",
    "traceId": "..."
}
```

---

# 📦 Standard Response Wrapper

Every endpoint in the application returns a unified response format.

This makes the API predictable and easy to consume by frontend applications.

### Success Response

```json
{
    "success": true,
    "message": "MedicalRecords retrieved successfully.",
    "data": {},
    "errors": null,
    "timestamp": "2026-07-25T18:45:21Z",
    "traceId": "...",
    "pagination": {}
}
```

### Error Response

```json
{
    "success": false,
    "message": "Department already exists.",
    "data": null,
    "errors": null,
    "timestamp": "...",
    "traceId": "..."
}
```

---

# 📄 Pagination

The API supports generic pagination across all list endpoints.

### Pagination Parameters

| Parameter | Description |
|------------|-------------|
| pageNumber | Current page number |
| pageSize | Number of records per page |

### Pagination Metadata

```json
{
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 5,
    "totalRecords": 43,
    "hasPreviousPage": false,
    "hasNextPage": true
}
```

Pagination is implemented for:

- Departments
- Doctors
- Patients
- Appointments
- Medical Records
- Prescriptions

---

# 🔍 Searching

The system provides searching capabilities for business entities.

Supported searches include:

- Departments by name
- Doctors by name
- Patients by name

Search results also support pagination.

---

# 🚦 Rate Limiting

Rate Limiting middleware is implemented to protect the API from excessive requests and abuse.

### Benefits

- Prevents brute-force attacks
- Protects authentication endpoints
- Prevents API abuse
- Improves application stability
- Enhances overall security

---

# 🗑️ Soft Delete

The application implements **Soft Delete** for business entities.

Instead of permanently removing records from the database:

- Records are marked as deleted.
- Deleted entities are automatically excluded from queries.
- Data can be restored if needed.
- Database integrity is preserved.

---

# 🌐 Google OAuth Integration

The system integrates with **Google OAuth 2.0** for secure authentication.

Users can:

- Register using a Google account.
- Login using a Google account.
- Skip manual password creation.

### Benefits

- Faster registration
- Secure authentication
- Trusted identity provider
- Better user experience

---

# 🎨 Frontend Integration

A frontend application was integrated with the backend to verify all API endpoints in real-world scenarios.

The frontend communicates with the API through REST endpoints and validates:

- Authentication
- Authorization
- CRUD Operations
- Business Rules
- Pagination
- Search
- Response Wrapper
- Error Handling

This ensured that the backend was tested beyond Swagger and Postman.

---

# 📚 Swagger Documentation

The project includes full Swagger/OpenAPI documentation.

Swagger allows developers to:

- Explore all endpoints.
- Authenticate using JWT Bearer Token.
- Test every endpoint directly.
- View request models.
- View response models.
- Understand API contracts.

---

# 🛠️ Technologies Used

### Backend

- ASP.NET Core 8 Web API
- C#
- Entity Framework Core
- SQL Server
- AutoMapper

### Authentication & Security

- JWT Authentication
- Refresh Tokens
- Google OAuth 2.0
- Authorization Policies
- Password Hashing

### Architecture

- Repository Pattern
- Generic Repository
- Service Layer
- Dependency Injection
- Clean Layered Architecture

### API Features

- Swagger/OpenAPI
- Pagination
- Response Wrapper
- Global Exception Handling
- Rate Limiting
- Soft Delete

---

# 🧪 Testing

The project was thoroughly tested using multiple approaches.

### Swagger UI

Used for:

- Endpoint testing
- JWT authentication
- Request validation
- Response verification

### Postman

Used for:

- Authentication flow
- Authorization testing
- CRUD operations
- Business rule validation
- Pagination
- Search
- Error handling

### Frontend Integration

Used to verify:

- End-to-end workflows
- UI integration
- API consistency
- Real-world usage scenarios

---

# 📁 Project Structure

```text
backend
│
├── Common
│   ├── Constants
│   ├── Exceptions
│   ├── Extensions
│   ├── Middleware
│   ├── Pagination
│   ├── Responses
│   └── Services
│
├── Data
│   ├── Configurations
│   ├── Migrations
│   └── Seed
│
├── Features
│   ├── Authentication
│   ├── Departments
│   ├── Doctors
│   ├── Patients
│   ├── Appointments
│   ├── MedicalRecords
│   ├── Prescriptions
│   └── Users
│
├── Models
│
├── Persistence
│   ├── Interfaces
│   └── Repositories
│
└── Program.cs
```

---

# 🚀 Future Improvements

Potential future enhancements include:

- Email Notifications
- SMS Notifications
- Online Payment Integration
- Medical File Uploads
- Dashboard & Analytics
- Reporting Module
- SignalR Real-Time Notifications
- Audit Logging
- Redis Caching
- Docker & Kubernetes Deployment
- CI/CD Pipeline
- Unit Testing
- Integration Testing

---
# Project demo:  
https://drive.google.com/file/d/13rGs7uB5-7ztMrZpynymU1SIR5vMNac9/view?usp=drive_link

# 👨‍💻 Developers

## Mohamed Ahmed Abdelrhman
## Mariz Youssef
## Omar Mounes
## Ester Emad


