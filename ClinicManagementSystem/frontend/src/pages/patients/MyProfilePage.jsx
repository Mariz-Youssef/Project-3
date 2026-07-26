import { useEffect, useState } from "react";
import { patientsApi } from "../../api/patientsApi";
import { unwrapError } from "../../api/axiosClient";
import { Loader } from "../../components/common/Loader";
import { Card } from "../../components/common/Card";
import { Button } from "../../components/common/Button";
import { FormField, TextInput, Select, TextArea } from "../../components/common/FormField";
import { useToast } from "../../context/ToastContext";

// Matches CreatePatientDto exactly.
const EMPTY_FORM = {
  dateOfBirth: "",
  gender: "",
  bloodGroup: "",
  address: "",
  allergies: "",
  medicalNotes: "",
  emergencyContactName: "",
  emergencyContactPhone: "",
};

const GENDERS = ["Female", "Male", "Other"];

export function MyProfilePage() {
  const toast = useToast();
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [notFound, setNotFound] = useState(false);
  const [editing, setEditing] = useState(false);
  const [form, setForm] = useState(EMPTY_FORM);
  const [error, setError] = useState(null);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    patientsApi
      .getMyProfile()
      .then((data) => {
        if (cancelled) return;
        setProfile(data);
        setForm({ ...EMPTY_FORM, ...data, dateOfBirth: data.dateOfBirth?.slice(0, 10) ?? "" });
      })
      .catch((err) => {
        if (cancelled) return;
        if (err.response?.status === 404) {
          setNotFound(true);
          setEditing(true);
        } else {
          setError(unwrapError(err).message);
        }
      })
      .finally(() => !cancelled && setLoading(false));
    return () => {
      cancelled = true;
    };
  }, []);

  function update(field, value) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError(null);
    setSaving(true);
    try {
      const saved = notFound
        ? await patientsApi.createMyProfile(form)
        : await patientsApi.updateMyProfile(form);
      setProfile(saved);
      setNotFound(false);
      setEditing(false);
      toast.success(notFound ? "Profile created" : "Profile updated");
    } catch (err) {
      setError(unwrapError(err).message);
    } finally {
      setSaving(false);
    }
  }

  if (loading) {
    return (
      <div className="page">
        <Loader label="Loading your profile" />
      </div>
    );
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="page-eyebrow">Your profile</p>
          <h1 className="page-title">My profile</h1>
          <p className="page-subtitle">
            {notFound
              ? "Finish your patient profile so your care team is ready for your visit."
              : "Keep your health and emergency contact details up to date."}
          </p>
        </div>
        {!editing && profile && <Button onClick={() => setEditing(true)}>Edit profile</Button>}
      </div>

      {error && <div className="form-error-banner" style={{ marginBottom: "var(--space-4)" }}>{error}</div>}

      {editing ? (
        <Card title={notFound ? "Complete your profile" : "Edit profile"}>
          <form className="stack-vertical" onSubmit={handleSubmit}>
            <div className="form-grid">
              <FormField label="Date of birth" htmlFor="p-dob">
                <TextInput
                  id="p-dob"
                  type="date"
                  required
                  value={form.dateOfBirth}
                  onChange={(e) => update("dateOfBirth", e.target.value)}
                />
              </FormField>

              <FormField label="Gender" htmlFor="p-gender">
                <Select
                  id="p-gender"
                  required
                  value={form.gender}
                  onChange={(e) => update("gender", e.target.value)}
                >
                  <option value="" disabled>
                    Select gender
                  </option>
                  {GENDERS.map((g) => (
                    <option key={g} value={g}>
                      {g}
                    </option>
                  ))}
                </Select>
              </FormField>

              <FormField label="Blood group" htmlFor="p-blood">
                <TextInput
                  id="p-blood"
                  required
                  maxLength={3}
                  value={form.bloodGroup}
                  onChange={(e) => update("bloodGroup", e.target.value.toUpperCase())}
                  placeholder="e.g. O+, A-, AB+"
                />
              </FormField>

              <FormField label="Address" htmlFor="p-address">
                <TextInput
                  id="p-address"
                  required
                  maxLength={200}
                  value={form.address}
                  onChange={(e) => update("address", e.target.value)}
                  placeholder="e.g. Nasr City, Cairo"
                />
              </FormField>

              <FormField label="Allergies" htmlFor="p-allergies" full>
                <TextArea
                  id="p-allergies"
                  value={form.allergies}
                  onChange={(e) => update("allergies", e.target.value)}
                  placeholder="e.g. Penicillin, seafood — leave blank if none"
                />
              </FormField>

              <FormField label="Medical notes" htmlFor="p-notes" full>
                <TextArea
                  id="p-notes"
                  value={form.medicalNotes}
                  onChange={(e) => update("medicalNotes", e.target.value)}
                  placeholder="Anything else your care team should know"
                />
              </FormField>

              <FormField label="Emergency contact name" htmlFor="p-ec-name">
                <TextInput
                  id="p-ec-name"
                  required
                  maxLength={100}
                  value={form.emergencyContactName}
                  onChange={(e) => update("emergencyContactName", e.target.value)}
                />
              </FormField>

              <FormField label="Emergency contact phone" htmlFor="p-ec-phone">
                <TextInput
                  id="p-ec-phone"
                  required
                  pattern="^\+?[1-9]\d{1,14}$"
                  title="e.g. 01090000001 or +201090000001"
                  value={form.emergencyContactPhone}
                  onChange={(e) => update("emergencyContactPhone", e.target.value)}
                  placeholder="e.g. 01090000001"
                />
              </FormField>
            </div>

            <div className="form-actions">
              {!notFound && (
                <Button
                  type="button"
                  variant="ghost"
                  onClick={() => setEditing(false)}
                  disabled={saving}
                >
                  Cancel
                </Button>
              )}
              <Button type="submit" loading={saving}>
                {notFound ? "Create profile" : "Save changes"}
              </Button>
            </div>
          </form>
        </Card>
      ) : (
        <Card title="Profile details">
          <div className="form-grid">
            <div>
              <p className="page-eyebrow">Date of birth</p>
              <p>{profile.dateOfBirth?.slice(0, 10) || "—"}</p>
            </div>
            <div>
              <p className="page-eyebrow">Gender</p>
              <p>{profile.gender || "—"}</p>
            </div>
            <div>
              <p className="page-eyebrow">Blood group</p>
              <p>{profile.bloodGroup || "—"}</p>
            </div>
            <div>
              <p className="page-eyebrow">Address</p>
              <p>{profile.address || "—"}</p>
            </div>
            <div className="form-grid--full">
              <p className="page-eyebrow">Allergies</p>
              <p>{profile.allergies || "None recorded"}</p>
            </div>
            <div className="form-grid--full">
              <p className="page-eyebrow">Medical notes</p>
              <p>{profile.medicalNotes || "—"}</p>
            </div>
            <div>
              <p className="page-eyebrow">Emergency contact</p>
              <p>{profile.emergencyContactName || "—"}</p>
            </div>
            <div>
              <p className="page-eyebrow">Emergency contact phone</p>
              <p>{profile.emergencyContactPhone || "—"}</p>
            </div>
          </div>
        </Card>
      )}
    </div>
  );
}
