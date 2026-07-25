import { useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { FormField, TextInput } from "../../components/common/FormField";
import { Button } from "../../components/common/Button";
import "./AuthLayout.css";

export function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const [form, setForm] = useState({ email: "", password: "" });
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  function update(field, value) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError(null);
    setLoading(true);
    const result = await login(form);
    setLoading(false);

    if (!result.success) {
      setError(result.error.message);
      return;
    }

    const redirectTo = location.state?.from?.pathname ?? "/";
    navigate(redirectTo, { replace: true });
  }

  return (
    <div className="auth-screen">
      <div className="auth-screen__side">
        <div className="auth-screen__mark">CM</div>
        <h1 className="auth-screen__title">
          Run the clinic day from one calm, well-organized place.
        </h1>
        <p className="auth-screen__blurb">
          Departments, doctors, schedules, and patient records — all in one
          system built for the whole care team.
        </p>
      </div>
      <div className="auth-screen__form-panel">
        <div className="auth-card">
          <p className="auth-card__eyebrow">Welcome back</p>
          <h2 className="auth-card__title">Log in to Clinicare</h2>

          <form className="stack-vertical" onSubmit={handleSubmit}>
            {error && <div className="form-error-banner">{error}</div>}

            <FormField label="Email" htmlFor="email">
              <TextInput
                id="email"
                type="email"
                required
                autoComplete="email"
                value={form.email}
                onChange={(e) => update("email", e.target.value)}
              />
            </FormField>

            <FormField label="Password" htmlFor="password">
              <TextInput
                id="password"
                type="password"
                required
                autoComplete="current-password"
                value={form.password}
                onChange={(e) => update("password", e.target.value)}
              />
            </FormField>

            <Button type="submit" loading={loading} style={{ width: "100%" }}>
              Log in
            </Button>
          </form>

          <p className="auth-card__footer">
            New patient?{" "}
            <Link to="/register" style={{ color: "var(--color-mint-dark)" }}>
              Create an account
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
}
