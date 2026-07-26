import axios from "axios";
import { getTokens, setTokens, clearTokens } from "../utils/storage";

const BASE_URL = import.meta.env.VITE_API_BASE_URL || "/api";
console.log("BASE_URL =", BASE_URL);

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

const TOTAL_COUNT_KEYS = [
  "totalCount",
  "TotalCount",
  "total_count",
  "totalItems",
  "TotalItems",
  "totalRecords",
  "TotalRecords",
  "total",
  "Total",
  "count",
  "Count",
];

const PAGE_NUMBER_KEYS = ["pageNumber", "PageNumber", "page", "Page", "currentPage", "CurrentPage"];
const PAGE_SIZE_KEYS = ["pageSize", "PageSize", "size", "Size"];
const TOTAL_PAGES_KEYS = ["totalPages", "TotalPages", "pageCount", "PageCount"];

function firstDefined(obj, keys) {
  if (!obj) return undefined;
  for (const key of keys) {
    if (obj[key] != null) return obj[key];
  }
  return undefined;
}

/**
 * Normalizes a paginated list response into { items, pagination }.
 * Handles both shapes seen in the backend: a "pagination" property sitting
 * next to "data", or a "data.items" / "data.pagination" nesting — and is
 * tolerant of different casing/naming for the total-count field itself,
 * since that's the one detail every controller happens to name differently.
 */
export function unwrapList(response) {
  const body = response.data ?? {};
  const data = body.data ?? body;

  const items = Array.isArray(data) ? data : data?.items ?? data?.Items ?? [];

  const paginationRaw =
    body.pagination ??
    body.Pagination ??
    data?.pagination ??
    data?.Pagination ??
    data?.paginationMetadata ??
    data?.PaginationMetadata ??
    null;

  // Some controllers put the total count at the top level instead of inside
  // a nested pagination object — check both places.
  const totalCount =
    firstDefined(paginationRaw, TOTAL_COUNT_KEYS) ?? firstDefined(body, TOTAL_COUNT_KEYS);

  if (totalCount == null && import.meta.env.DEV) {
    // eslint-disable-next-line no-console
    console.warn(
      "[unwrapList] Couldn't find a recognizable total-count field on this response — falling back to items.length, which is wrong once pageSize < total records. Inspect the raw shape below and add the real field name to TOTAL_COUNT_KEYS in src/api/axiosClient.js.",
      { url: response.config?.url, paginationRaw, body }
    );
  }

  const pagination =
    paginationRaw || totalCount != null
      ? {
          ...paginationRaw,
          pageNumber: firstDefined(paginationRaw, PAGE_NUMBER_KEYS) ?? 1,
          pageSize: firstDefined(paginationRaw, PAGE_SIZE_KEYS) ?? items.length,
          totalCount: totalCount ?? items.length,
          totalPages: firstDefined(paginationRaw, TOTAL_PAGES_KEYS) ?? undefined,
        }
      : null;

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
