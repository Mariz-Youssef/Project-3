import { Link } from "react-router-dom";

export function NotFoundPage() {
  return (
    <div
      style={{
        minHeight: "100vh",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        gap: "var(--space-3)",
        background: "var(--color-ink)",
        color: "var(--color-white)",
        textAlign: "center",
        padding: "var(--space-6)",
      }}
    >
      <p style={{ fontFamily: "var(--font-mono)", color: "var(--color-mint)" }}>404</p>
      <h1 style={{ fontFamily: "var(--font-display)", fontSize: 28 }}>
        This page took a wrong turn.
      </h1>
      <p style={{ color: "rgba(255,255,255,0.6)", maxWidth: 380 }}>
        The page you're looking for doesn't exist or has moved.
      </p>
      <Link
        to="/"
        style={{
          marginTop: "var(--space-3)",
          background: "var(--color-mint)",
          color: "var(--color-ink)",
          padding: "10px 20px",
          borderRadius: "var(--radius-sm)",
          fontWeight: 600,
          textDecoration: "none",
        }}
      >
        Back to dashboard
      </Link>
    </div>
  );
}
