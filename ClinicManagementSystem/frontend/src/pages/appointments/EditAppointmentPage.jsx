import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { appointmentsApi } from "../../api/appointmentsApi";
import { unwrapError } from "../../api/axiosClient";
import { Card } from "../../components/common/Card";
import { Loader } from "../../components/common/Loader";
import { useToast } from "../../context/ToastContext";
import { AppointmentForm } from "./AppointmentForm";

export function EditAppointmentPage() {
  const { id } = useParams();
  const toast = useToast();
  const navigate = useNavigate();

  const [appointment, setAppointment] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let cancelled = false;
    appointmentsApi
      .getById(id)
      .then((data) => !cancelled && setAppointment(data))
      .catch((err) => !cancelled && setError(unwrapError(err).message))
      .finally(() => !cancelled && setLoading(false));
    return () => {
      cancelled = true;
    };
  }, [id]);

  async function handleSubmit(payload) {
    try {
      await appointmentsApi.update(id, payload);
      toast.success("Appointment updated");
      navigate(`/appointments/${id}`);
    } catch (err) {
      throw new Error(unwrapError(err).message);
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

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="page-eyebrow">Appointment #{appointment.id}</p>
          <h1 className="page-title">Edit appointment</h1>
        </div>
      </div>

      <Card>
        <AppointmentForm
          initialValues={appointment}
          onSubmit={handleSubmit}
          onCancel={() => navigate(-1)}
          submitLabel="Save changes"
        />
      </Card>
    </div>
  );
}
