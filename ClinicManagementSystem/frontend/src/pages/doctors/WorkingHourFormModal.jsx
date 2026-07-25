import { useState } from "react";
import { Modal } from "../../components/common/Modal";
import { FormField, TextInput, Select } from "../../components/common/FormField";
import { Button } from "../../components/common/Button";
import { doctorWorkingHoursApi } from "../../api/doctorWorkingHoursApi";
import { unwrapError } from "../../api/axiosClient";

const DAYS = [
  "Monday",
  "Tuesday",
  "Wednesday",
  "Thursday",
  "Friday",
  "Saturday",
  "Sunday",
];

export function WorkingHourFormModal({ doctorId, workingHour, onClose, onSaved }) {
  const isEdit = Boolean(workingHour);
  const [form, setForm] = useState({
    dayOfWeek: workingHour?.dayOfWeek ?? "Monday",
    startTime: workingHour?.startTime ?? "09:00",
    endTime: workingHour?.endTime ?? "17:00",
  });
  const [error, setError] = useState(null);
  const [saving, setSaving] = useState(false);

  function update(field, value) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError(null);
    setSaving(true);
    try {
      if (isEdit) {
        await doctorWorkingHoursApi.update(doctorId, workingHour.id, form);
      } else {
        await doctorWorkingHoursApi.create(doctorId, form);
      }
      onSaved();
    } catch (err) {
      setError(unwrapError(err).message);
    } finally {
      setSaving(false);
    }
  }

  return (
    <Modal
      title={isEdit ? "Edit working hours" : "Add working hours"}
      onClose={onClose}
    >
      <form className="stack-vertical" onSubmit={handleSubmit}>
        {error && <div className="form-error-banner">{error}</div>}

        <FormField label="Day of week" htmlFor="wh-day">
          <Select
            id="wh-day"
            value={form.dayOfWeek}
            onChange={(e) => update("dayOfWeek", e.target.value)}
          >
            {DAYS.map((day) => (
              <option key={day} value={day}>
                {day}
              </option>
            ))}
          </Select>
        </FormField>

        <div className="form-grid">
          <FormField label="Start time" htmlFor="wh-start">
            <TextInput
              id="wh-start"
              type="time"
              required
              value={form.startTime}
              onChange={(e) => update("startTime", e.target.value)}
            />
          </FormField>
          <FormField label="End time" htmlFor="wh-end">
            <TextInput
              id="wh-end"
              type="time"
              required
              value={form.endTime}
              onChange={(e) => update("endTime", e.target.value)}
            />
          </FormField>
        </div>

        <div className="form-actions">
          <Button type="button" variant="ghost" onClick={onClose} disabled={saving}>
            Cancel
          </Button>
          <Button type="submit" loading={saving}>
            {isEdit ? "Save changes" : "Add"}
          </Button>
        </div>
      </form>
    </Modal>
  );
}
