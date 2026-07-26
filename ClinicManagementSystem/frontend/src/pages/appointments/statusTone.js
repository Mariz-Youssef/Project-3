export const STATUS_TONE = {
  Pending: "warning",
  Confirmed: "info",
  Completed: "mint",
  Cancelled: "danger",
  Canceled: "danger",
};

/** Best-effort display name from whatever shape the appointment DTO uses. */
export function personDisplayName(fullNameField, firstName, lastName, id) {
  if (fullNameField) return fullNameField;
  const combined = `${firstName ?? ""} ${lastName ?? ""}`.trim();
  return combined || `#${id}`;
}
