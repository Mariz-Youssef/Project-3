import "./EmptyState.css";

export function EmptyState({ title, message, action }) {
  return (
    <div className="empty-state">
      <div className="empty-state__mark" aria-hidden="true" />
      <h4 className="empty-state__title">{title}</h4>
      {message && <p className="empty-state__message">{message}</p>}
      {action && <div className="empty-state__action">{action}</div>}
    </div>
  );
}
