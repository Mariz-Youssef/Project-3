import { useEffect, useState } from "react";
import { doctorsApi } from "../../api/doctorsApi";
import { FormField, TextInput, TextArea, Select } from "../../components/common/FormField";
import { Button } from "../../components/common/Button";

const EMPTY_FORM = {
  doctorId: "",
  appointmentDate: "",
  appointmentTime: "",
  reasonForVisit: "",
};

/**
 * initialValues: { doctorId, appointmentDate (ISO), reasonForVisit } for edit mode
 */
export function AppointmentForm({ initialValues, onSubmit, onCancel, submitLabel }) {
  const [form, setForm] = useState(() => {
    if (!initialValues) return EMPTY_FORM;
    const dt = initialValues.appointmentDate
      ? new Date(initialValues.appointmentDate)
      : null;
    return {
      doctorId: initialValues.doctorId ?? "",
      appointmentDate: dt ? dt.toISOString().slice(0, 10) : "",
      appointmentTime: dt ? dt.toISOString().slice(11, 16) : "",
      reasonForVisit: initialValues.reasonForVisit ?? "",
    };
  });
  const [doctors, setDoctors] = useState([]);
  const [error, setError] = useState(null);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    doctorsApi
      .getAll({ pageNumber: 1, pageSize: 100 })
      .then((res) => setDoctors(res.items))
      .catch(() => setDoctors([]));
  }, []);

  function update(field, value) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError(null);

    if (!form.doctorId || !form.appointmentDate || !form.appointmentTime) {
      setError("Please choose a doctor, date, and time.");
      return;
    }

    const payload = {
      doctorId: Number(form.doctorId),
      appointmentDate: `${form.appointmentDate}T${form.appointmentTime}:00`,
      reasonForVisit: form.reasonForVisit,
    };

    setSaving(true);
    try {
      await onSubmit(payload);
    } catch (err) {
      setError(err?.message ?? "Could not save this appointment.");
    } finally {
      setSaving(false);
    }
  }

  return (
    <form className="stack-vertical" onSubmit={handleSubmit}>
      {error && <div className="form-error-banner">{error}</div>}

      <div className="form-grid">
        <FormField label="Doctor" htmlFor="appt-doctor" full>
          <Select
            id="appt-doctor"
            required
            value={form.doctorId}
            onChange={(e) => update("doctorId", e.target.value)}
          >
            <option value="" disabled>
              Select a doctor
            </option>
            {doctors.map((d) => (
              <option key={d.id} value={d.id}>
                Dr. {d.firstName} {d.lastName} — {d.specialization}
              </option>
            ))}
          </Select>
        </FormField>

        <FormField label="Date" htmlFor="appt-date">
          <TextInput
            id="appt-date"
            type="date"
            required
            value={form.appointmentDate}
            onChange={(e) => update("appointmentDate", e.target.value)}
          />
        </FormField>

        <FormField label="Time" htmlFor="appt-time">
          <TextInput
            id="appt-time"
            type="time"
            required
            value={form.appointmentTime}
            onChange={(e) => update("appointmentTime", e.target.value)}
          />
        </FormField>

        <FormField label="Reason for visit" htmlFor="appt-reason" full>
          <TextArea
            id="appt-reason"
            value={form.reasonForVisit}
            onChange={(e) => update("reasonForVisit", e.target.value)}
            placeholder="Briefly describe why you're booking this visit"
          />
        </FormField>
      </div>

      <div className="form-actions">
        {onCancel && (
          <Button type="button" variant="ghost" onClick={onCancel} disabled={saving}>
            Cancel
          </Button>
        )}
        <Button type="submit" loading={saving}>
          {submitLabel}
        </Button>
      </div>
    </form>
  );
}
