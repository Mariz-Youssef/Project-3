import "./FormField.css";

export function FormField({
  label,
  htmlFor,
  error,
  full = false,
  children,
}) {
  return (
    <div className={`form-field ${full ? "form-grid--full" : ""}`}>
      {label && (
        <label className="form-field__label" htmlFor={htmlFor}>
          {label}
        </label>
      )}
      {children}
      {error && <span className="form-field__error">{error}</span>}
    </div>
  );
}

export function TextInput({ id, error, ...rest }) {
  return (
    <input id={id} className={`text-input ${error ? "is-invalid" : ""}`} {...rest} />
  );
}

export function TextArea({ id, error, ...rest }) {
  return (
    <textarea
      id={id}
      className={`text-input text-area ${error ? "is-invalid" : ""}`}
      {...rest}
    />
  );
}

export function Select({ id, error, children, ...rest }) {
  return (
    <select id={id} className={`text-input select-input ${error ? "is-invalid" : ""}`} {...rest}>
      {children}
    </select>
  );
}
