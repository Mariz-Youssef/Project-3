import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { Button } from "../common/Button";
import "./Topbar.css";

export function Topbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  async function handleLogout() {
    await logout();
    navigate("/login", { replace: true });
  }

  const initials = (user?.email || user?.id || "U")
    .toString()
    .slice(0, 2)
    .toUpperCase();

  return (
    <header className="topbar">
      <div className="topbar__spacer" />
      <div className="topbar__user">
        <div className="topbar__avatar">{initials}</div>
        <div className="topbar__user-info">
          <span className="topbar__user-email">
            {user?.email ?? `User #${user?.id ?? ""}`}
          </span>
          <span className="topbar__user-role">{user?.role}</span>
        </div>
        <Button variant="ghost" size="sm" onClick={handleLogout}>
          Log out
        </Button>
      </div>
    </header>
  );
}
