import { useEffect, useRef, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { FormField, TextInput } from "../../components/common/FormField";
import { Button } from "../../components/common/Button";
import { axiosClient, unwrap, unwrapError } from "../../api/axiosClient";
import { setTokens } from "../../utils/storage";
import "./AuthLayout.css";

const GOOGLE_CLIENT_ID = import.meta.env.VITE_GOOGLE_CLIENT_ID;

function extractTokens(payload) {
  return {
    accessToken: payload.accessToken ?? payload.token ?? payload.jwt,
    refreshToken: payload.refreshToken,
  };
}

export function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const [form, setForm] = useState({ email: "", password: "" });
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
  const [googleError, setGoogleError] = useState(null);
  const [googleReady, setGoogleReady] = useState(false);
  const googleButtonRef = useRef(null);

  const redirectTo = location.state?.from?.pathname ?? "/";

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

    navigate(redirectTo, { replace: true });
  }

  // Handles the ID token Google hands back after the person picks an account.
  async function handleGoogleCredential(response) {
    setGoogleError(null);
    try {
      const result = await axiosClient
        .post("/auth/google", { idToken: response.credential })
        .then(unwrap);

      setTokens(extractTokens(result));
      // Full reload so AuthProvider re-reads the freshly stored tokens.
      window.location.href = redirectTo;
    } catch (err) {
      setGoogleError(unwrapError(err).message);
    }
  }

  // Loads Google's Identity Services script once, then renders its button
  // into googleButtonRef once the script is ready.
  useEffect(() => {
    if (!GOOGLE_CLIENT_ID) return;

    const existing = document.getElementById("google-identity-script");
    if (existing) {
      setGoogleReady(true);
      return;
    }

    const script = document.createElement("script");
    script.id = "google-identity-script";
    script.src = "https://accounts.google.com/gsi/client";
    script.async = true;
    script.defer = true;
    script.onload = () => setGoogleReady(true);
    script.onerror = () =>
      setGoogleError("Couldn't load Google Sign-In. Check your connection.");
    document.body.appendChild(script);
  }, []);

  useEffect(() => {
    if (!googleReady || !GOOGLE_CLIENT_ID || !googleButtonRef.current) return;

    window.google.accounts.id.initialize({
      client_id: GOOGLE_CLIENT_ID,
      callback: handleGoogleCredential,
    });
    window.google.accounts.id.renderButton(googleButtonRef.current, {
      type: "standard",
      theme: "outline",
      size: "large",
      shape: "rectangular",
      width: 320,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [googleReady]);

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

          {GOOGLE_CLIENT_ID && (
            <>
              <div
                style={{
                  display: "flex",
                  alignItems: "center",
                  gap: "var(--space-3)",
                  margin: "var(--space-4) 0",
                  color: "var(--color-slate-light)",
                  fontSize: 12,
                  textTransform: "uppercase",
                  letterSpacing: "0.06em",
                }}
              >
                <span style={{ flex: 1, height: 1, background: "var(--color-grey-200)" }} />
                or
                <span style={{ flex: 1, height: 1, background: "var(--color-grey-200)" }} />
              </div>

              {googleError && (
                <div className="form-error-banner" style={{ marginBottom: "var(--space-3)" }}>
                  {googleError}
                </div>
              )}

              <div style={{ display: "flex", justifyContent: "center" }} ref={googleButtonRef} />
            </>
          )}

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