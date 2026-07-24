import { useEffect, useState } from "react";
import { Modal } from "../../components/common/Modal";
import { FormField, TextInput, TextArea, Select } from "../../components/common/FormField";
import { Button } from "../../components/common/Button";
import { doctorsApi } from "../../api/doctorsApi";
import { departmentsApi } from "../../api/departmentsApi";
import { unwrapError } from "../../api/axiosClient";

const EMPTY_FORM = {
  firstName: "",
  lastName: "",
  email: "",
  phone: "",
  specialization: "",
  departmentId: "",
  licenseNumber: "",
  bio: "",
};

export function DoctorFormModal({ doctor, onClose, onSaved }) {
  const isEdit = Boolean(doctor);
  const [form, setForm] = useState({ ...EMPTY_FORM, ...doctor });
  const [departments, setDepartments] = useState([]);
  const [error, setError] = useState(null);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    departmentsApi
      .getAll({ pageNumber: 1, pageSize: 100 })
      .then((res) => setDepartments(res.items))
      .catch(() => setDepartments([]));
  }, []);

  function update(field, value) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError(null);
    setSaving(true);

    const payload = { ...form, departmentId: Number(form.departmentId) || null };

    try {
      if (isEdit) {
        await doctorsApi.update(doctor.id, payload);
      } else {
        await doctorsApi.create(payload);
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
      title={isEdit ? "Edit doctor" : "Add doctor"}
      subtitle={isEdit ? `${doctor.firstName} ${doctor.lastName}` : "Add a doctor profile."}
      onClose={onClose}
      width={640}
    >
      <form className="stack-vertical" onSubmit={handleSubmit}>
        {error && <div className="form-error-banner">{error}</div>}

        <div className="form-grid">
          <FormField label="First name" htmlFor="doc-first">
            <TextInput
              id="doc-first"
              required
              value={form.firstName}
              onChange={(e) => update("firstName", e.target.value)}
            />
          </FormField>

          <FormField label="Last name" htmlFor="doc-last">
            <TextInput
              id="doc-last"
              required
              value={form.lastName}
              onChange={(e) => update("lastName", e.target.value)}
            />
          </FormField>

          <FormField label="Email" htmlFor="doc-email">
            <TextInput
              id="doc-email"
              type="email"
              required
              value={form.email}
              onChange={(e) => update("email", e.target.value)}
            />
          </FormField>

          <FormField label="Phone" htmlFor="doc-phone">
            <TextInput
              id="doc-phone"
              value={form.phone}
              onChange={(e) => update("phone", e.target.value)}
            />
          </FormField>

          <FormField label="Specialization" htmlFor="doc-spec">
            <TextInput
              id="doc-spec"
              required
              value={form.specialization}
              onChange={(e) => update("specialization", e.target.value)}
              placeholder="e.g. Pediatrics"
            />
          </FormField>

          <FormField label="Department" htmlFor="doc-dept">
            <Select
              id="doc-dept"
              required
              value={form.departmentId ?? ""}
              onChange={(e) => update("departmentId", e.target.value)}
            >
              <option value="" disabled>
                Select a department
              </option>
              {departments.map((d) => (
                <option key={d.id} value={d.id}>
                  {d.name}
                </option>
              ))}
            </Select>
          </FormField>

          <FormField label="License number" htmlFor="doc-license">
            <TextInput
              id="doc-license"
              value={form.licenseNumber}
              onChange={(e) => update("licenseNumber", e.target.value)}
            />
          </FormField>

          <FormField label="Bio" htmlFor="doc-bio" full>
            <TextArea
              id="doc-bio"
              value={form.bio}
              onChange={(e) => update("bio", e.target.value)}
              placeholder="Short professional background"
            />
          </FormField>
        </div>

        <div className="form-actions">
          <Button type="button" variant="ghost" onClick={onClose} disabled={saving}>
            Cancel
          </Button>
          <Button type="submit" loading={saving}>
            {isEdit ? "Save changes" : "Create doctor"}
          </Button>
        </div>
      </form>
    </Modal>
  );
}
