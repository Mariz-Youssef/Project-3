import "./Loader.css";

export function Loader({ label = "Loading" }) {
  return (
    <div className="loader-row">
      <span className="loader-dot" />
      <span className="loader-dot" />
      <span className="loader-dot" />
      <span className="loader-label">{label}</span>
    </div>
  );
}
