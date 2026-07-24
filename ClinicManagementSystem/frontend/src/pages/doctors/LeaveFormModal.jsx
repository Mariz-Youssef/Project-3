import { useState } from "react";
import { Modal } from "../../components/common/Modal";
import { FormField, TextInput, TextArea, Select } from "../../components/common/FormField";
import { Button } from "../../components/common/Button";
import { doctorLeavesApi } from "../../api/doctorLeavesApi";
import { unwrapError } from "../../api/axiosClient";

const STATUS_OPTIONS = ["Pending", "Approved", "Rejected"];

export function LeaveFormModal({ doctorId, leave, onClose, onSaved }) {
  const isEdit = Boolean(leave);
  const [form, setForm] = useState({
    startDate: leave?.startDate?.slice(0, 10) ?? "",
    endDate: leave?.endDate?.slice(0, 10) ?? "",
    reason: leave?.reason ?? "",
    status: leave?.status ?? "Pending",
  });
  const [error, setError] = useState(null);
  const [saving, setSaving] = useState(false);

  function update(field, value) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError(null);

    if (form.endDate < form.startDate) {
      setError("End date can't be before the start date.");
      return;
    }

    setSaving(true);
    try {
      if (isEdit) {
        await doctorLeavesApi.update(doctorId, leave.id, form);
      } else {
        await doctorLeavesApi.create(doctorId, form);
      }
      onSaved();
    } catch (err) {
      setError(unwrapError(err).message);
    } finally {
      setSaving(false);
    }
  }

  return (
    <Modal title={isEdit ? "Edit leave" : "Request leave"} onClose={onClose}>
      <form className="stack-vertical" onSubmit={handleSubmit}>
        {error && <div className="form-error-banner">{error}</div>}

        <div className="form-grid">
          <FormField label="Start date" htmlFor="leave-start">
            <TextInput
              id="leave-start"
              type="date"
              required
              value={form.startDate}
              onChange={(e) => update("startDate", e.target.value)}
            />
          </FormField>
          <FormField label="End date" htmlFor="leave-end">
            <TextInput
              id="leave-end"
              type="date"
              required
              value={form.endDate}
              onChange={(e) => update("endDate", e.target.value)}
            />
          </FormField>
        </div>

        <FormField label="Reason" htmlFor="leave-reason">
          <TextArea
            id="leave-reason"
            value={form.reason}
            onChange={(e) => update("reason", e.target.value)}
            placeholder="e.g. Annual leave, conference, medical"
          />
        </FormField>

        {isEdit && (
          <FormField label="Status" htmlFor="leave-status">
            <Select
              id="leave-status"
              value={form.status}
              onChange={(e) => update("status", e.target.value)}
            >
              {STATUS_OPTIONS.map((s) => (
                <option key={s} value={s}>
                  {s}
                </option>
              ))}
            </Select>
          </FormField>
        )}

        <div className="form-actions">
          <Button type="button" variant="ghost" onClick={onClose} disabled={saving}>
            Cancel
          </Button>
          <Button type="submit" loading={saving}>
            {isEdit ? "Save changes" : "Submit request"}
          </Button>
        </div>
      </form>
    </Modal>
  );
}
