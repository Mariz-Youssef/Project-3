import axios from "axios";
import { getTokens, setTokens, clearTokens } from "../utils/storage";

const BASE_URL = import.meta.env.VITE_API_BASE_URL || "/api";

export const axiosClient = axios.create({
  baseURL: BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Attach the access token to every outgoing request.
axiosClient.interceptors.request.use((config) => {
  const { accessToken } = getTokens();
  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`;
  }
  return config;
});

let isRefreshing = false;
let pendingQueue = [];

function resolveQueue(error, token) {
  pendingQueue.forEach(({ resolve, reject }) => {
    if (error) reject(error);
    else resolve(token);
  });
  pendingQueue = [];
}

// On a 401, try exchanging the refresh token once, then replay the request.
axiosClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    const status = error.response?.status;
    const isAuthRoute = originalRequest?.url?.includes("/auth/");

    if (status !== 401 || isAuthRoute || originalRequest._retry) {
      return Promise.reject(error);
    }

    const { refreshToken } = getTokens();
    if (!refreshToken) {
      clearTokens();
      return Promise.reject(error);
    }

    if (isRefreshing) {
      return new Promise((resolve, reject) => {
        pendingQueue.push({ resolve, reject });
      }).then((token) => {
        originalRequest.headers.Authorization = `Bearer ${token}`;
        return axiosClient(originalRequest);
      });
    }

    originalRequest._retry = true;
    isRefreshing = true;

    try {
      const { data } = await axios.post(`${BASE_URL}/auth/refresh-token`, {
        refreshToken,
      });
      const payload = data?.data ?? data;
      setTokens({
        accessToken: payload.accessToken ?? payload.token,
        refreshToken: payload.refreshToken,
      });
      resolveQueue(null, payload.accessToken ?? payload.token);
      originalRequest.headers.Authorization = `Bearer ${payload.accessToken ?? payload.token}`;
      return axiosClient(originalRequest);
    } catch (refreshError) {
      resolveQueue(refreshError, null);
      clearTokens();
      window.location.href = "/login";
      return Promise.reject(refreshError);
    } finally {
      isRefreshing = false;
    }
  }
);

/**
 * Normalizes the ASP.NET ApiResponse<T> / ApiResponseFactory envelope so callers
 * can just consume the payload, regardless of which wrapper shape a controller used.
 */
export function unwrap(response) {
  const body = response.data;
  if (body && typeof body === "object" && "data" in body) {
    return body.data;
  }
  return body;
}

/**
 * Normalizes a paginated list response into { items, pagination }.
 * Handles both shapes seen in the backend: a "pagination" property sitting
 * next to "data", or a "data.items" / "data.pagination" nesting.
 */
export function unwrapList(response) {
  const body = response.data ?? {};
  const data = body.data ?? body;

  const items = Array.isArray(data) ? data : data?.items ?? [];
  const pagination =
    body.pagination ?? data?.pagination ?? data?.paginationMetadata ?? null;

  return { items, pagination };
}

export function unwrapError(error) {
  const body = error.response?.data;
  const message =
    body?.message ||
    body?.errors?.[0] ||
    (Array.isArray(body?.errors) ? body.errors.join(", ") : null) ||
    error.message ||
    "Something went wrong. Please try again.";
  return { message, status: error.response?.status, raw: body };
}
