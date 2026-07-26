/**
 * Neither the Doctor nor Patient table stores a name/email/phone — those
 * live on the User account the profile is linked to (UserId). If your
 * DoctorResponse/PatientResponse DTOs join that in under some field, this
 * picks it up automatically; otherwise it falls back to a stable label.
 */
export function personDisplayName(row, fallbackPrefix = "User") {
  if (!row) return "—";

  const candidates = [
    row.fullName,
    row.name,
    row.userFullName,
    row.user?.fullName,
    [row.firstName, row.lastName].filter(Boolean).join(" ").trim(),
    row.user ? [row.user.firstName, row.user.lastName].filter(Boolean).join(" ").trim() : "",
  ];

  const found = candidates.find((c) => c && c.trim().length > 0);
  if (found) return found;

  const id = row.userId ?? row.user?.id ?? row.id;
  return `${fallbackPrefix} #${id ?? "?"}`;
}

export function personContact(row) {
  return row?.email ?? row?.user?.email ?? row?.phone ?? row?.user?.phone ?? null;
}
