import { useEffect, useState } from "react";
import { doctorsApi } from "../../api/doctorsApi";
import { FormField, TextInput, TextArea, Select } from "../../components/common/FormField";
import { Button } from "../../components/common/Button";
import { personDisplayName } from "../../utils/personDisplay";

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
const [workingHours, setWorkingHours] = useState([]);
const [availableSlots, setAvailableSlots] = useState([]);

  useEffect(() => {
    doctorsApi
      .getAll({ pageNumber: 1, pageSize: 100 })
      .then((res) => setDoctors(res.items))
      .catch(() => setDoctors([]));
  }, []);

  function update(field, value) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }
    async function handleDoctorChange(doctorId) {
        update("doctorId", doctorId);

        update("appointmentDate", "");
        update("appointmentTime", "");

        setWorkingHours([]);
        setAvailableSlots([]);
        setError(null);

        if (!doctorId) return;

        try {
            const hours = await doctorsApi.getWorkingHours(doctorId);
            setWorkingHours(hours);
        } catch {
            setError("Couldn't load doctor's working hours.");
        }
    }

    async function handleDateChange(date) {
        update("appointmentDate", date);
        update("appointmentTime", "");

        if (!form.doctorId) return;

        const selectedDay = new Date(date).toLocaleDateString("en-US", {
            weekday: "long",
        });

        const worksThatDay = workingHours.some(
            (w) => w.dayOfWeek === selectedDay
        );

        if (!worksThatDay) {
            setAvailableSlots([]);
            setError("Doctor doesn't work on this day.");
            return;
        }

        setError(null);

        try {
            const slots = await doctorsApi.getAvailableSlots(
                form.doctorId,
                date
            );

            setAvailableSlots(slots);
        } catch {
            setAvailableSlots([]);
            setError("Couldn't load available slots.");
        }
    }

  async function handleSubmit(e) {
    e.preventDefault();
    setError(null);

    if (!form.doctorId || !form.appointmentDate || !form.appointmentTime) {
      setError("Please choose a doctor, date, and time.");
      return;
    }

      const [hours, minutes] = form.appointmentTime.split(":");

      const endDate = new Date();
      endDate.setHours(Number(hours));
      endDate.setMinutes(Number(minutes));
      endDate.setSeconds(0);

      // Add the appointment duration (40 minutes)
      endDate.setMinutes(endDate.getMinutes() + 40);

      const endTime = `${String(endDate.getHours()).padStart(2, "0")}:${String(
          endDate.getMinutes()
      ).padStart(2, "0")}:00`;

      const startTime =
          form.appointmentTime.length === 5
              ? `${form.appointmentTime}:00`
              : form.appointmentTime;

      const payload = {
          doctorId: Number(form.doctorId),
          appointmentDate: form.appointmentDate,
          startTime,
          endTime,
          reason: form.reasonForVisit,
          notes: "",
      };

      console.log(payload);

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
            onChange={(e) => handleDoctorChange(e.target.value)}          >
            <option value="" disabled>
              Select a doctor
            </option>
            {doctors.map((d) => (
              <option key={d.id} value={d.id}>
                {personDisplayName(d, "Doctor")} — {d.specialization}
              </option>
            ))}
          </Select>
        </FormField>

        <FormField label="Date" htmlFor="appt-date">
            <TextInput
                id="appt-date"
                type="date"
                required
                min={new Date().toISOString().split("T")[0]}
                disabled={!form.doctorId}
                value={form.appointmentDate}
                onChange={(e) => handleDateChange(e.target.value)}
            />
        </FormField>

        <FormField label="Available Time" htmlFor="appt-time">
            <Select
                id="appt-time"
                required
                disabled={!availableSlots.length}
                value={form.appointmentTime}
                onChange={(e) => update("appointmentTime", e.target.value)}
            >
                <option value="">Select a time</option>

                {availableSlots.map((slot) => (
                    <option key={slot.time} value={slot.time}>
                        {slot.time}
                    </option>
                ))}
            </Select>
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
