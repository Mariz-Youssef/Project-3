import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { ROLES } from "../../utils/roles";
import { departmentsApi } from "../../api/departmentsApi";
import { doctorsApi } from "../../api/doctorsApi";
import { patientsApi } from "../../api/patientsApi";
import "./Dashboard.css";


const PAGE_ONE = { pageNumber: 1, pageSize: 200 };

export function DashboardPage() {
  const { user } = useAuth();
  const canSeePatients = user?.role === ROLES.ADMIN || user?.role === ROLES.DOCTOR;

  const [counts, setCounts] = useState({
    departments: null,
    doctors: null,
    patients: null,
  });

  useEffect(() => {
    let cancelled = false;

    async function loadCounts() {
      const requests = [
        departmentsApi.getAll(PAGE_ONE).then((r) => r.pagination?.totalCount ?? r.items.length),
        doctorsApi.getAll(PAGE_ONE).then((r) => r.pagination?.totalCount ?? r.items.length),
        canSeePatients
          ? patientsApi.getAll(PAGE_ONE).then((r) => r.pagination?.totalCount ?? r.items.length)
          : Promise.resolve(null),
      ];

      const [departments, doctors, patients] = await Promise.allSettled(requests);

      if (cancelled) return;
      setCounts({
        departments: departments.status === "fulfilled" ? departments.value : "—",
        doctors: doctors.status === "fulfilled" ? doctors.value : "—",
        patients: patients.status === "fulfilled" ? patients.value : "—",
      });
    }

    loadCounts();
    return () => {
      cancelled = true;
    };
  }, [canSeePatients]);

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="page-eyebrow">Overview</p>
          <h1 className="page-title">
                      Good to see you
                      {user?.fullName
                          ? `, ${user.fullName}`
                          : user?.email
                              ? `, ${user.email}`
                              : ""} </h1>
          <p className="page-subtitle">
            A quick snapshot of what's happening across the clinic.
          </p>
        </div>
      </div>

      <div className="dashboard-grid">
        <div className="stat-card">
          <p className="stat-card__label">Departments</p>
          <p className="stat-card__value">{counts.departments ?? "…"}</p>
          <p className="stat-card__hint">Active clinical departments</p>
        </div>
        <div className="stat-card">
          <p className="stat-card__label">Doctors</p>
          <p className="stat-card__value">{counts.doctors ?? "…"}</p>
          <p className="stat-card__hint">On the care team</p>
        </div>
        {canSeePatients && (
          <div className="stat-card">
            <p className="stat-card__label">Patients</p>
            <p className="stat-card__value">{counts.patients ?? "…"}</p>
            <p className="stat-card__hint">Registered patient profiles</p>
          </div>
        )}
      </div>

      <div className="quick-links">
        <Link to="/departments" className="quick-link-card">
          <div className="quick-link-card__title">Departments</div>
          <div className="quick-link-card__desc">
            Browse, search, and manage clinic departments.
          </div>
        </Link>
        <Link to="/doctors" className="quick-link-card">
          <div className="quick-link-card__title">Doctors</div>
          <div className="quick-link-card__desc">
            Manage profiles, working hours, and leaves.
          </div>
        </Link>
        {canSeePatients ? (
          <Link to="/patients" className="quick-link-card">
            <div className="quick-link-card__title">Patients</div>
            <div className="quick-link-card__desc">
              Search patient profiles and records.
            </div>
          </Link>
        ) : (
          <Link to="/my-profile" className="quick-link-card">
            <div className="quick-link-card__title">My Profile</div>
            <div className="quick-link-card__desc">
              View or complete your patient profile.
            </div>
          </Link>
        )}
        {user?.role === ROLES.ADMIN && (
          <Link to="/appointments" className="quick-link-card">
            <div className="quick-link-card__title">Appointments</div>
            <div className="quick-link-card__desc">
              Review, confirm, and manage clinic appointments.
            </div>
          </Link>
        )}
        {user?.role === ROLES.PATIENT && (
          <Link to="/appointments/book" className="quick-link-card">
            <div className="quick-link-card__title">Book an appointment</div>
            <div className="quick-link-card__desc">
              Pick a doctor and a time that works for you.
            </div>
          </Link>
        )}
        {user?.role === ROLES.DOCTOR && (
          <Link to="/appointments/find" className="quick-link-card">
            <div className="quick-link-card__title">Find an appointment</div>
            <div className="quick-link-card__desc">
              Look up an appointment by ID to confirm or complete it.
            </div>
          </Link>
        )}
      </div>
    </div>
  );
}
