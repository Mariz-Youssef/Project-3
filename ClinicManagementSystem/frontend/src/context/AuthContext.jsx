import { createContext, useContext, useEffect, useState } from "react";
import { authApi } from "../api/authApi";
import { unwrapError } from "../api/axiosClient";
import { getTokens, setTokens, clearTokens } from "../utils/storage";
import { userFromToken } from "../utils/jwt";

const AuthContext = createContext(null);

function extractTokens(payload) {
  return {
    accessToken: payload.accessToken ?? payload.token ?? payload.jwt,
    refreshToken: payload.refreshToken,
  };
}

export function AuthProvider({ children }) {
  const [user, setUser] = useState(() => {
    const { accessToken } = getTokens();
    return accessToken ? userFromToken(accessToken) : null;
  });
  const [initializing, setInitializing] = useState(false);

  useEffect(() => {
    const { accessToken } = getTokens();
    if (accessToken) setUser(userFromToken(accessToken));
  }, []);

  async function login(credentials) {
    try {
      const payload = await authApi.login(credentials);
      const tokens = extractTokens(payload);
      setTokens(tokens);
      const nextUser = userFromToken(tokens.accessToken);
      setUser(nextUser);
      return { success: true, user: nextUser };
    } catch (error) {
      return { success: false, error: unwrapError(error) };
    }
  }

  async function register(payload) {
    try {
      const data = await authApi.register(payload);
      return { success: true, data };
    } catch (error) {
      return { success: false, error: unwrapError(error) };
    }
  }

  async function logout() {
    const { refreshToken } = getTokens();
    try {
      if (refreshToken) await authApi.revokeToken(refreshToken);
    } catch {
      // Best-effort revoke; proceed with local logout regardless.
    } finally {
      clearTokens();
      setUser(null);
    }
  }

  const value = {
    user,
    isAuthenticated: Boolean(user),
    initializing,
    login,
    register,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within an AuthProvider");
  return ctx;
}
