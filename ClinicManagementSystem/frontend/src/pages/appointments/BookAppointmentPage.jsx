import { useNavigate } from "react-router-dom";
import { appointmentsApi } from "../../api/appointmentsApi";
import { unwrapError } from "../../api/axiosClient";
import { Card } from "../../components/common/Card";
import { useAuth } from "../../context/AuthContext";
import { useToast } from "../../context/ToastContext";
import { addCachedAppointmentId } from "../../utils/appointmentCache";
import { AppointmentForm } from "./AppointmentForm";

export function BookAppointmentPage() {
  const { user } = useAuth();
  const toast = useToast();
  const navigate = useNavigate();

  async function handleSubmit(payload) {
    try {
      const created = await appointmentsApi.create(payload);
      addCachedAppointmentId(user?.id, created.id);
      toast.success("Appointment booked");
      navigate(`/appointments/${created.id}`);
    } catch (err) {
      throw new Error(unwrapError(err).message);
    }
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="page-eyebrow">Scheduling</p>
          <h1 className="page-title">Book an appointment</h1>
          <p className="page-subtitle">
            Pick a doctor, a date and time, and let us know why you're coming in.
          </p>
        </div>
      </div>

      <Card>
        <AppointmentForm
          onSubmit={handleSubmit}
          onCancel={() => navigate(-1)}
          submitLabel="Book appointment"
        />
      </Card>
    </div>
  );
}
