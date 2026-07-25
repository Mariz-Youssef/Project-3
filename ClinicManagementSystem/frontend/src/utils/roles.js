// Mirrors the role names used by the backend's [Authorize(Roles = ...)] attributes.
export const ROLES = {
  ADMIN: "Admin",
  DOCTOR: "Doctor",
  PATIENT: "Patient",
};

export function hasRole(user, ...roles) {
  if (!user?.role) return false;
  return roles.includes(user.role);
}
