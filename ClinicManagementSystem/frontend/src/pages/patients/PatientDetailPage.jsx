import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { patientsApi } from "../../api/patientsApi";
import { unwrapError } from "../../api/axiosClient";
import { Loader } from "../../components/common/Loader";
import { Card } from "../../components/common/Card";
import { personDisplayName, personContact } from "../../utils/personDisplay";

export function PatientDetailPage() {
  const { id } = useParams();
  const [patient, setPatient] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    patientsApi
      .getById(id)
      .then((data) => !cancelled && setPatient(data))
      .catch((err) => !cancelled && setError(unwrapError(err).message))
      .finally(() => !cancelled && setLoading(false));
    return () => {
      cancelled = true;
    };
  }, [id]);

  if (loading) {
    return (
      <div className="page">
        <Loader label="Loading patient" />
      </div>
    );
  }

  if (error || !patient) {
    return (
      <div className="page">
        <div className="form-error-banner">
          {error ?? "This patient profile could not be found."}
        </div>
      </div>
    );
  }

  const contact = personContact(patient);

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="page-eyebrow">
            <Link to="/patients" style={{ color: "inherit" }}>
              ← Patients
            </Link>
          </p>
          <h1 className="page-title">{personDisplayName(patient, "Patient")}</h1>
          {contact && <p className="page-subtitle">{contact}</p>}
        </div>
      </div>

      <Card title="Patient details">
        <div className="form-grid">
          <div>
            <p className="page-eyebrow">Date of birth</p>
            <p>{patient.dateOfBirth?.slice(0, 10) || "—"}</p>
          </div>
          <div>
            <p className="page-eyebrow">Gender</p>
            <p>{patient.gender || "—"}</p>
          </div>
          <div>
            <p className="page-eyebrow">Blood group</p>
            <p>{patient.bloodGroup || "—"}</p>
          </div>
          <div>
            <p className="page-eyebrow">Address</p>
            <p>{patient.address || "—"}</p>
          </div>
          <div className="form-grid--full">
            <p className="page-eyebrow">Allergies</p>
            <p>{patient.allergies || "None recorded"}</p>
          </div>
          <div className="form-grid--full">
            <p className="page-eyebrow">Medical notes</p>
            <p>{patient.medicalNotes || "—"}</p>
          </div>
          <div>
            <p className="page-eyebrow">Emergency contact</p>
            <p>{patient.emergencyContactName || "—"}</p>
          </div>
          <div>
            <p className="page-eyebrow">Emergency contact phone</p>
            <p>{patient.emergencyContactPhone || "—"}</p>
          </div>
        </div>
      </Card>
    </div>
  );
}
