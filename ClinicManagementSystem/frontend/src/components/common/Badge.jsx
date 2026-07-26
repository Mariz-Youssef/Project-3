import "./Badge.css";

// tone: mint | grey | danger | warning | info
export function Badge({ tone = "grey", children }) {
  return <span className={`badge badge--${tone}`}>{children}</span>;
}
