import { axiosClient, unwrap, unwrapList } from "./axiosClient";

export const doctorsApi = {
  getAll: (params) => axiosClient.get("/doctors", { params }).then(unwrapList),

  getById: (id) => axiosClient.get(`/doctors/${id}`).then(unwrap),

  create: (payload) => axiosClient.post("/doctors", payload).then(unwrap),

  update: (id, payload) =>
    axiosClient.put(`/doctors/${id}`, payload).then(unwrap),

  remove: (id) => axiosClient.delete(`/doctors/${id}`).then(unwrap),

  getByDepartment: (departmentId, params) =>
    axiosClient
      .get(`/doctors/department/${departmentId}`, { params })
      .then(unwrapList),

  getBySpecialization: (specialization, params) =>
    axiosClient
      .get(`/doctors/specialization/${encodeURIComponent(specialization)}`, {
        params,
      })
      .then(unwrapList),
};
