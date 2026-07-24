import { useEffect, useState } from "react";
import { patientsApi } from "../../api/patientsApi";
import { unwrapError } from "../../api/axiosClient";
import { Loader } from "../../components/common/Loader";
import { Card } from "../../components/common/Card";
import { Button } from "../../components/common/Button";
import { FormField, TextInput, Select, TextArea } from "../../components/common/FormField";
import { useToast } from "../../context/ToastContext";

const EMPTY_FORM = {
  firstName: "",
  lastName: "",
  phone: "",
  dateOfBirth: "",
  gender: "",
  bloodType: "",
  address: "",
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
              : "Keep your contact and health details up to date."}
          </p>
        </div>
        {!editing && profile && <Button onClick={() => setEditing(true)}>Edit profile</Button>}
      </div>

      {error && <div className="form-error-banner" style={{ marginBottom: "var(--space-4)" }}>{error}</div>}

      {editing ? (
        <Card title={notFound ? "Complete your profile" : "Edit profile"}>
          <form className="stack-vertical" onSubmit={handleSubmit}>
            <div className="form-grid">
              <FormField label="First name" htmlFor="p-first">
                <TextInput
                  id="p-first"
                  required
                  value={form.firstName}
                  onChange={(e) => update("firstName", e.target.value)}
                />
              </FormField>
              <FormField label="Last name" htmlFor="p-last">
                <TextInput
                  id="p-last"
                  required
                  value={form.lastName}
                  onChange={(e) => update("lastName", e.target.value)}
                />
              </FormField>
              <FormField label="Phone" htmlFor="p-phone">
                <TextInput
                  id="p-phone"
                  value={form.phone}
                  onChange={(e) => update("phone", e.target.value)}
                />
              </FormField>
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
              <FormField label="Blood type" htmlFor="p-blood">
                <TextInput
                  id="p-blood"
                  value={form.bloodType}
                  onChange={(e) => update("bloodType", e.target.value)}
                  placeholder="e.g. O+"
                />
              </FormField>
              <FormField label="Address" htmlFor="p-address" full>
                <TextArea
                  id="p-address"
                  value={form.address}
                  onChange={(e) => update("address", e.target.value)}
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
              <p className="page-eyebrow">Name</p>
              <p>
                {profile.firstName} {profile.lastName}
              </p>
            </div>
            <div>
              <p className="page-eyebrow">Phone</p>
              <p>{profile.phone || "—"}</p>
            </div>
            <div>
              <p className="page-eyebrow">Date of birth</p>
              <p>{profile.dateOfBirth?.slice(0, 10) || "—"}</p>
            </div>
            <div>
              <p className="page-eyebrow">Gender</p>
              <p>{profile.gender || "—"}</p>
            </div>
            <div>
              <p className="page-eyebrow">Blood type</p>
              <p>{profile.bloodType || "—"}</p>
            </div>
            <div className="form-grid--full">
              <p className="page-eyebrow">Address</p>
              <p>{profile.address || "—"}</p>
            </div>
          </div>
        </Card>
      )}
    </div>
  );
}
