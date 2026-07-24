import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { doctorsApi } from "../../api/doctorsApi";
import { unwrapError } from "../../api/axiosClient";
import { Loader } from "../../components/common/Loader";
import { Badge } from "../../components/common/Badge";
import { Card } from "../../components/common/Card";
import { useAuth } from "../../context/AuthContext";
import { ROLES } from "../../utils/roles";
import { WorkingHoursTab } from "./WorkingHoursTab";
import { LeavesTab } from "./LeavesTab";

export function DoctorDetailPage() {
  const { id } = useParams();
  const { user } = useAuth();
  const canManage = user?.role === ROLES.ADMIN || user?.role === ROLES.DOCTOR;

  const [doctor, setDoctor] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [tab, setTab] = useState("hours");

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    doctorsApi
      .getById(id)
      .then((data) => !cancelled && setDoctor(data))
      .catch((err) => !cancelled && setError(unwrapError(err).message))
      .finally(() => !cancelled && setLoading(false));
    return () => {
      cancelled = true;
    };
  }, [id]);

  if (loading) {
    return (
      <div className="page">
        <Loader label="Loading doctor profile" />
      </div>
    );
  }

  if (error || !doctor) {
    return (
      <div className="page">
        <div className="form-error-banner">
          {error ?? "This doctor profile could not be found."}
        </div>
      </div>
    );
  }

  const fullName =
    `${doctor.firstName ?? ""} ${doctor.lastName ?? ""}`.trim() || doctor.name;

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="page-eyebrow">
            <Link to="/doctors" style={{ color: "inherit" }}>
              ← Doctors
            </Link>
          </p>
          <h1 className="page-title">Dr. {fullName}</h1>
          <p className="page-subtitle">{doctor.email}</p>
        </div>
        <Badge tone="mint">{doctor.specialization}</Badge>
      </div>

      <Card className="stack-vertical" style={{ marginBottom: "var(--space-5)" }}>
        <div className="form-grid">
          <div>
            <p className="page-eyebrow">Phone</p>
            <p>{doctor.phone || "—"}</p>
          </div>
          <div>
            <p className="page-eyebrow">License number</p>
            <p>{doctor.licenseNumber || "—"}</p>
          </div>
          {doctor.bio && (
            <div className="form-grid--full">
              <p className="page-eyebrow">Bio</p>
              <p>{doctor.bio}</p>
            </div>
          )}
        </div>
      </Card>

      <div className="tabs">
        <button
          className={`tab-btn ${tab === "hours" ? "active" : ""}`}
          onClick={() => setTab("hours")}
        >
          Working hours
        </button>
        <button
          className={`tab-btn ${tab === "leaves" ? "active" : ""}`}
          onClick={() => setTab("leaves")}
        >
          Leaves
        </button>
      </div>

      {tab === "hours" ? (
        <WorkingHoursTab doctorId={doctor.id} canManage={canManage} />
      ) : (
        <LeavesTab doctorId={doctor.id} canManage={canManage} />
      )}
    </div>
  );
}
