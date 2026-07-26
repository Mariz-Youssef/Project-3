import { axiosClient, unwrap } from "./axiosClient";

export const authApi = {
  register: (payload) =>
    axiosClient.post("/auth/register", payload).then(unwrap),

  createDoctorAccount: (payload) =>
    axiosClient.post("/auth/create-doctor-account", payload).then(unwrap),

  login: (payload) => axiosClient.post("/auth/login", payload).then(unwrap),

  refreshToken: (refreshToken) =>
    axiosClient.post("/auth/refresh-token", { refreshToken }).then(unwrap),

  revokeToken: (refreshToken) =>
    axiosClient.post("/auth/revoke-token", { refreshToken }).then(unwrap),
};
