import { axiosClient, unwrap, unwrapList } from "./axiosClient";

export const patientsApi = {
  getAll: (params) =>
    axiosClient.get("/patients", { params }).then(unwrapList),

  search: (searchTerm, params) =>
    axiosClient
      .get("/patients/search", { params: { searchTerm, ...params } })
      .then(unwrapList),

  getById: (id) => axiosClient.get(`/patients/${id}`).then(unwrap),

  remove: (id) => axiosClient.delete(`/patients/${id}`).then(unwrap),

  // "My profile" endpoints, used by the logged-in patient.
  getMyProfile: () => axiosClient.get("/patients/profile").then(unwrap),

  createMyProfile: (payload) =>
    axiosClient.post("/patients/profile", payload).then(unwrap),

  updateMyProfile: (payload) =>
    axiosClient.put("/patients/profile", payload).then(unwrap),
};
