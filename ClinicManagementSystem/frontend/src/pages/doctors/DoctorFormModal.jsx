import { useEffect, useState } from "react";
import { Modal } from "../../components/common/Modal";
import { FormField, TextInput, Select } from "../../components/common/FormField";
import { Button } from "../../components/common/Button";
import { doctorsApi } from "../../api/doctorsApi";
import { departmentsApi } from "../../api/departmentsApi";
import { unwrapError } from "../../api/axiosClient";
import { personDisplayName } from "../../utils/personDisplay";

// Matches CreateDoctorRequest exactly — the Doctor entity itself has no
// name/email/phone/bio, only a link (UserId) to an existing user account.
const EMPTY_FORM = {
  userId: "",
  departmentId: "",
  specialization: "",
  licenseNumber: "",
  yearsOfExperience: "",
  consultationFee: "",
};

export function DoctorFormModal({ doctor, onClose, onSaved }) {
  const isEdit = Boolean(doctor);
  const [form, setForm] = useState({
    ...EMPTY_FORM,
    ...doctor,
    userId: doctor?.userId ?? "",
    departmentId: doctor?.departmentId ?? "",
    yearsOfExperience: doctor?.yearsOfExperience ?? "",
    consultationFee: doctor?.consultationFee ?? "",
  });
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

    const payload = {
      departmentId: Number(form.departmentId),
      specialization: form.specialization,
      licenseNumber: form.licenseNumber,
      yearsOfExperience: Number(form.yearsOfExperience),
      consultationFee: Number(form.consultationFee),
    };
    // UserId links the profile to an existing account and is only meaningful
    // when creating a new doctor profile, not when editing one.
    if (!isEdit) {
      payload.userId = Number(form.userId);
    }

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
      subtitle={
        isEdit ? personDisplayName(doctor, "Doctor") : "Link an existing user account to a doctor profile."
      }
      onClose={onClose}
      width={640}
    >
      <form className="stack-vertical" onSubmit={handleSubmit}>
        {error && <div className="form-error-banner">{error}</div>}

        <div className="form-grid">
          {!isEdit && (
            <FormField
              label="User ID"
              htmlFor="doc-userid"
              full
            >
              <TextInput
                id="doc-userid"
                type="number"
                min="1"
                required
                value={form.userId}
                onChange={(e) => update("userId", e.target.value)}
                placeholder="ID of the existing doctor login account"
              />
              <p style={{ fontSize: 12, color: "var(--color-slate)", marginTop: 4 }}>
                This is the ID of an account already created via "Create doctor
                account" — this form only adds their doctor profile
                (specialization, license, etc.), it doesn't create a login.
              </p>
            </FormField>
          )}

          <FormField label="Department" htmlFor="doc-dept" full>
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

          <FormField label="Specialization" htmlFor="doc-spec">
            <TextInput
              id="doc-spec"
              required
              value={form.specialization}
              onChange={(e) => update("specialization", e.target.value)}
              placeholder="e.g. Neurology"
            />
          </FormField>

          <FormField label="License number" htmlFor="doc-license">
            <TextInput
              id="doc-license"
              required
              value={form.licenseNumber}
              onChange={(e) => update("licenseNumber", e.target.value)}
              placeholder="e.g. NEUR-1005"
            />
          </FormField>

          <FormField label="Years of experience" htmlFor="doc-years">
            <TextInput
              id="doc-years"
              type="number"
              min="0"
              required
              value={form.yearsOfExperience}
              onChange={(e) => update("yearsOfExperience", e.target.value)}
            />
          </FormField>

          <FormField label="Consultation fee" htmlFor="doc-fee">
            <TextInput
              id="doc-fee"
              type="number"
              min="0"
              step="0.01"
              required
              value={form.consultationFee}
              onChange={(e) => update("consultationFee", e.target.value)}
              placeholder="e.g. 500.00"
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
