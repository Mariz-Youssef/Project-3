import { axiosClient, unwrap, unwrapList } from "./axiosClient";

export const departmentsApi = {
  getAll: (params) =>
    axiosClient.get("/departments", { params }).then(unwrapList),

  getById: (id) => axiosClient.get(`/departments/${id}`).then(unwrap),

  search: (searchTerm, params) =>
    axiosClient
      .get("/departments/search", { params: { searchTerm, ...params } })
      .then(unwrapList),

  create: (payload) => axiosClient.post("/departments", payload).then(unwrap),

  update: (id, payload) =>
    axiosClient.put(`/departments/${id}`, payload).then(unwrap),

  remove: (id) => axiosClient.delete(`/departments/${id}`).then(unwrap),
};
