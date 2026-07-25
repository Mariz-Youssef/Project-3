import { NavLink } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { ROLES } from "../../utils/roles";
import "./Sidebar.css";

const NAV_ITEMS = [
  { to: "/", label: "Dashboard", icon: "◆", roles: null, end: true },
  { to: "/departments", label: "Departments", icon: "▤", roles: null },
  { to: "/doctors", label: "Doctors", icon: "✦", roles: null },
  {
    to: "/appointments",
    label: "Appointments",
    icon: "▦",
    roles: [ROLES.ADMIN],
  },
  {
    to: "/patients",
    label: "Patients",
    icon: "◍",
    roles: [ROLES.ADMIN, ROLES.DOCTOR],
  },
  {
    to: "/my-profile",
    label: "My Profile",
    icon: "●",
    roles: [ROLES.PATIENT],
  },
  {
    to: "/my-appointments",
    label: "My Appointments",
    icon: "▦",
    roles: [ROLES.PATIENT],
  },
  {
    to: "/appointments/find",
    label: "Find Appointment",
    icon: "⌕",
    roles: [ROLES.DOCTOR],
  },
];

export function Sidebar() {
  const { user } = useAuth();

  return (
    <aside className="sidebar">
      <div className="sidebar__brand">
        <span className="sidebar__brand-mark">CM</span>
        <div>
          <div className="sidebar__brand-name">Clinicare</div>
          <div className="sidebar__brand-sub">Management System</div>
        </div>
      </div>

      <nav className="sidebar__nav">
        {NAV_ITEMS.filter(
          (item) => !item.roles || item.roles.includes(user?.role)
        ).map((item) => (
          <NavLink
            key={item.to}
            to={item.to}
            end={item.end}
            className={({ isActive }) =>
              `sidebar__link ${isActive ? "is-active" : ""}`
            }
          >
            <span className="sidebar__link-icon">{item.icon}</span>
            {item.label}
          </NavLink>
        ))}
      </nav>

      <div className="sidebar__footer">
        <span className="sidebar__footer-label">Signed in as</span>
        <span className="sidebar__footer-role">{user?.role ?? "User"}</span>
      </div>
    </aside>
  );
}
