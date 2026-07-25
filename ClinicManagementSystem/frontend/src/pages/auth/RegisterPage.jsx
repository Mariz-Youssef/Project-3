import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { authApi } from "../../api/authApi";
import { unwrapError } from "../../api/axiosClient";
import { FormField, TextInput } from "../../components/common/FormField";
import { Button } from "../../components/common/Button";
import { useToast } from "../../context/ToastContext";
import "./AuthLayout.css";
import { useAuth } from "../../context/AuthContext";

export function RegisterPage() {
  const navigate = useNavigate();
    const toast = useToast();
    const { login } = useAuth();

    const [form, setForm] = useState({
    fullName: "",
    email: "",
    password: "",
    confirmPassword: "",
  });
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  function update(field, value) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }

    async function handleSubmit(e) {
        e.preventDefault();
        setError(null);

        if (form.password !== form.confirmPassword) {
            setError("Passwords do not match.");
            return;
        }

        setLoading(true);
        try {
            await authApi.register(form);

            const result = await login({
                email: form.email,
                password: form.password,
            });

            if (!result.success) {
                throw new Error(result.error?.message ?? "Login failed.");
            }

            toast.success("Account created successfully.");

            navigate("/my-profile", {
                replace: true,
            });
        }
        catch (err) {
            setError(unwrapError(err).message ?? err.message);
        }
        finally {
            setLoading(false);
        }
    }

  return (
    <div className="auth-screen">
      <div className="auth-screen__side">
        <div className="auth-screen__mark">CM</div>
        <h1 className="auth-screen__title">
          Book faster, track your care, all in one place.
        </h1>
        <p className="auth-screen__blurb">
          Create your account, then finish your patient profile so your care
          team has what they need before your first visit.
        </p>
      </div>
      <div className="auth-screen__form-panel">
        <div className="auth-card">
          <p className="auth-card__eyebrow">New patient</p>
          <h2 className="auth-card__title">Create your account</h2>

          <form className="stack-vertical" onSubmit={handleSubmit}>
            {error && <div className="form-error-banner">{error}</div>}
            <FormField label="Full name" htmlFor="full-name">
                <TextInput
                    id="full-name"
                    type="text"
                    required
                    value={form.fullName}
                    onChange={(e) => update("fullName", e.target.value)}
                    placeholder="Enter your full name"
                />
            </FormField>
            <FormField label="Email" htmlFor="reg-email">
              <TextInput
                id="reg-email"
                type="email"
                required
                autoComplete="email"
                value={form.email}
                onChange={(e) => update("email", e.target.value)}
                placeholder="Enter your a valid email"

              />
            </FormField>

            <FormField label="Password" htmlFor="reg-password">
              <TextInput
                id="reg-password"
                type="password"
                required
                autoComplete="new-password"
                value={form.password}
                onChange={(e) => update("password", e.target.value)}
                placeholder="Enter a password"

              />
            </FormField>

            <FormField label="Confirm password" htmlFor="reg-confirm">
              <TextInput
                id="reg-confirm"
                type="password"
                required
                autoComplete="new-password"
                value={form.confirmPassword}
                onChange={(e) => update("confirmPassword", e.target.value)}
                placeholder="Confirm password"
              />
            </FormField>

            <Button type="submit" loading={loading} style={{ width: "100%" }}>
              Create account
            </Button>
          </form>

          <p className="auth-card__footer">
            Already have an account?{" "}
            <Link to="/login" style={{ color: "var(--color-mint-dark)" }}>
              Log in
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
}
