// Decodes a JWT payload without verifying the signature. Verification always
// happens server-side; this is purely so the UI can read role/name claims.
export function decodeJwt(token) {
  if (!token) return null;
  try {
    const payload = token.split(".")[1];
    const base64 = payload.replace(/-/g, "+").replace(/_/g, "/");
    const json = decodeURIComponent(
      atob(base64)
        .split("")
        .map((c) => "%" + c.charCodeAt(0).toString(16).padStart(2, "0"))
        .join("")
    );
    return JSON.parse(json);
  } catch {
    return null;
  }
}

const ROLE_CLAIM_KEYS = [
  "role",
  "roles",
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
];
const NAME_ID_CLAIM_KEYS = [
  "nameid",
  "sub",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
];
const EMAIL_CLAIM_KEYS = [
  "email",
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
];

function firstClaim(claims, keys) {
  for (const key of keys) {
    if (claims?.[key] != null) return claims[key];
  }
  return null;
}

export function userFromToken(token) {
  const claims = decodeJwt(token);
  if (!claims) return null;

  const role = firstClaim(claims, ROLE_CLAIM_KEYS);

  return {
    id: firstClaim(claims, NAME_ID_CLAIM_KEYS),
    email: firstClaim(claims, EMAIL_CLAIM_KEYS),
    role: Array.isArray(role) ? role[0] : role,
    raw: claims,
  };
}
