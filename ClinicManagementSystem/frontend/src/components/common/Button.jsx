import "./Button.css";

export function Button({
  variant = "primary",
  size = "md",
  loading = false,
  disabled = false,
  type = "button",
  children,
  ...rest
}) {
  return (
    <button
      type={type}
      className={`btn btn--${variant} btn--${size}`}
      disabled={disabled || loading}
      {...rest}
    >
      {loading ? <span className="btn__spinner" aria-hidden="true" /> : null}
      <span>{children}</span>
    </button>
  );
}
