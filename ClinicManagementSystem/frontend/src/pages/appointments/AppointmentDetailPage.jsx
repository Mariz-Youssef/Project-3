import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { appointmentsApi } from "../../api/appointmentsApi";
import { unwrapError } from "../../api/axiosClient";
import { Loader } from "../../components/common/Loader";
import { Card } from "../../components/common/Card";
import { Badge } from "../../components/common/Badge";
import { Button } from "../../components/common/Button";
import { ConfirmDialog } from "../../components/common/ConfirmDialog";
import { useAuth } from "../../context/AuthContext";
import { useToast } from "../../context/ToastContext";
import { ROLES } from "../../utils/roles";
import { removeCachedAppointmentId } from "../../utils/appointmentCache";
import { STATUS_TONE, personDisplayName } from "./statusTone";

const TERMINAL_STATUSES = ["Completed", "Cancelled", "Canceled"];

export function AppointmentDetailPage() {
  const { id } = useParams();
  const { user } = useAuth();
  const toast = useToast();
  const navigate = useNavigate();

  const [appointment, setAppointment] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [actionLoading, setActionLoading] = useState(null);
  const [confirmCancel, setConfirmCancel] = useState(false);
  const [confirmDelete, setConfirmDelete] = useState(false);

  function load() {
    setLoading(true);
    appointmentsApi
      .getById(id)
      .then((data) => setAppointment(data))
      .catch((err) => setError(unwrapError(err).message))
      .finally(() => setLoading(false));
  }

  useEffect(load, [id]);

  async function runAction(action, key, successMessage) {
    setActionLoading(key);
    try {
      const updated = await action();
      setAppointment(updated);
      toast.success(successMessage);
    } catch (err) {
      toast.error(unwrapError(err).message);
    } finally {
      setActionLoading(null);
    }
  }

  async function handleDelete() {
    setActionLoading("delete");
    try {
      await appointmentsApi.remove(id);
      removeCachedAppointmentId(user?.id, Number(id));
      toast.success("Appointment deleted");
      navigate(-1);
    } catch (err) {
      toast.error(unwrapError(err).message);
      setConfirmDelete(false);
    } finally {
      setActionLoading(null);
    }
  }

  if (loading) {
    return (
      <div className="page">
        <Loader label="Loading appointment" />
      </div>
    );
  }

  if (error || !appointment) {
    return (
      <div className="page">
        <div className="form-error-banner">
          {error ?? "This appointment could not be found."}
        </div>
      </div>
    );
  }

  const status = appointment.status ?? "Pending";
  const isTerminal = TERMINAL_STATUSES.includes(status);
  const isAdmin = user?.role === ROLES.ADMIN;
  const isDoctor = user?.role === ROLES.DOCTOR;
  const isPatient = user?.role === ROLES.PATIENT;

  const canConfirm = (isAdmin || isDoctor) && status === "Pending";
  const canComplete = isDoctor && status === "Confirmed";
  const canCancel = (isAdmin || isDoctor) && !isTerminal;
  const canDelete = isAdmin;
  const canEdit = isPatient && !isTerminal;

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="page-eyebrow">
            <Link to="/appointments" style={{ color: "inherit" }}>
              ← Appointments
            </Link>
          </p>
          <h1 className="page-title">Appointment #{appointment.id}</h1>
        </div>
        <Badge tone={STATUS_TONE[status] ?? "grey"}>{status}</Badge>
      </div>

      <Card title="Details" className="stack-vertical">
        <div className="form-grid">
          <div>
            <p className="page-eyebrow">Patient</p>
            <p>
              {personDisplayName(
                appointment.patientName,
                appointment.patientFirstName,
                appointment.patientLastName,
                appointment.patientId
              )}
            </p>
          </div>
          <div>
            <p className="page-eyebrow">Doctor</p>
            <p>
              {personDisplayName(
                appointment.doctorName,
                appointment.doctorFirstName,
                appointment.doctorLastName,
                appointment.doctorId
              )}
            </p>
          </div>
          <div>
            <p className="page-eyebrow">Date &amp; time</p>
            <p>
              {appointment.appointmentDate
                ? new Date(appointment.appointmentDate).toLocaleString()
                : "—"}
            </p>
          </div>
          <div>
            <p className="page-eyebrow">Status</p>
            <p>{status}</p>
          </div>
          <div className="form-grid--full">
            <p className="page-eyebrow">Reason for visit</p>
            <p>{appointment.reasonForVisit || appointment.notes || "—"}</p>
          </div>
        </div>
      </Card>

      <div className="page-toolbar" style={{ marginTop: "var(--space-5)" }}>
        {canEdit && (
          <Button
            variant="ghost"
            onClick={() => navigate(`/appointments/${id}/edit`)}
          >
            Edit
          </Button>
        )}
        {canConfirm && (
          <Button
            loading={actionLoading === "confirm"}
            onClick={() =>
              runAction(
                () => appointmentsApi.confirm(id),
                "confirm",
                "Appointment confirmed"
              )
            }
          >
            Confirm
          </Button>
        )}
        {canComplete && (
          <Button
            loading={actionLoading === "complete"}
            onClick={() =>
              runAction(
                () => appointmentsApi.complete(id),
                "complete",
                "Appointment marked as completed"
              )
            }
          >
            Mark completed
          </Button>
        )}
        {canCancel && (
          <Button
            variant="danger"
            onClick={() => setConfirmCancel(true)}
            disabled={actionLoading === "cancel"}
          >
            Cancel appointment
          </Button>
        )}
        {canDelete && (
          <Button variant="danger" onClick={() => setConfirmDelete(true)}>
            Delete
          </Button>
        )}
      </div>

      {confirmCancel && (
        <ConfirmDialog
          title="Cancel appointment"
          message="This will mark the appointment as cancelled. This can't be undone."
          confirmLabel="Cancel appointment"
          loading={actionLoading === "cancel"}
          onConfirm={async () => {
            await runAction(
              () => appointmentsApi.cancel(id),
              "cancel",
              "Appointment cancelled"
            );
            setConfirmCancel(false);
          }}
          onCancel={() => setConfirmCancel(false)}
        />
      )}

      {confirmDelete && (
        <ConfirmDialog
          title="Delete appointment"
          message="This appointment will be permanently deleted."
          confirmLabel="Delete"
          loading={actionLoading === "delete"}
          onConfirm={handleDelete}
          onCancel={() => setConfirmDelete(false)}
        />
      )}
    </div>
  );
}
