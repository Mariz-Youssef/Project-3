import { useState } from "react";
import { Modal } from "../../components/common/Modal";
import { FormField, TextInput, TextArea } from "../../components/common/FormField";
import { Button } from "../../components/common/Button";
import { departmentsApi } from "../../api/departmentsApi";
import { unwrapError } from "../../api/axiosClient";

export function DepartmentFormModal({ department, onClose, onSaved }) {
  const isEdit = Boolean(department);

  const [form, setForm] = useState({
    name: department?.name ?? "",
    description: department?.description ?? "",
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
        await departmentsApi.update(department.id, form);
      } else {
        await departmentsApi.create(form);
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
      title={isEdit ? "Edit department" : "Add department"}
      subtitle={isEdit ? department.name : "Create a new clinical department."}
      onClose={onClose}
    >
      <form className="stack-vertical" onSubmit={handleSubmit}>
        {error && <div className="form-error-banner">{error}</div>}

        <FormField label="Department name" htmlFor="dept-name">
          <TextInput
            id="dept-name"
            required
            value={form.name}
            onChange={(e) => update("name", e.target.value)}
            placeholder="e.g. Cardiology"
          />
        </FormField>

        <FormField label="Description" htmlFor="dept-desc">
          <TextArea
            id="dept-desc"
            value={form.description}
            onChange={(e) => update("description", e.target.value)}
            placeholder="What this department covers"
          />
        </FormField>

        <div className="form-actions">
          <Button type="button" variant="ghost" onClick={onClose} disabled={saving}>
            Cancel
          </Button>
          <Button type="submit" loading={saving}>
            {isEdit ? "Save changes" : "Create department"}
          </Button>
        </div>
      </form>
    </Modal>
  );
}
