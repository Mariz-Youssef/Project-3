// The Appointments API only exposes GetAll (Admin-only) and GetById — there's
// no "list my appointments" endpoint for patients yet. As a stopgap, we
// remember the IDs a patient has booked (in this browser) so "My Appointments"
// has something to show. This is a convenience cache, not a source of truth:
// it won't show appointments booked from another device/browser, and it's
// cleared if the user clears site data. Once the backend adds a real
// per-patient list endpoint, swap MyAppointmentsPage over to that instead.

const KEY_PREFIX = "cms.myAppointmentIds.";

function keyFor(userId) {
  return `${KEY_PREFIX}${userId ?? "anonymous"}`;
}

export function getCachedAppointmentIds(userId) {
  try {
    const raw = localStorage.getItem(keyFor(userId));
    return raw ? JSON.parse(raw) : [];
  } catch {
    return [];
  }
}

export function addCachedAppointmentId(userId, id) {
  const ids = getCachedAppointmentIds(userId);
  if (!ids.includes(id)) {
    ids.unshift(id);
    localStorage.setItem(keyFor(userId), JSON.stringify(ids.slice(0, 50)));
  }
}

export function removeCachedAppointmentId(userId, id) {
  const ids = getCachedAppointmentIds(userId).filter((i) => i !== id);
  localStorage.setItem(keyFor(userId), JSON.stringify(ids));
}
