import { axiosClient, unwrap, unwrapList } from "./axiosClient";

export const doctorLeavesApi = {
  getByDoctor: (doctorId, params) =>
    axiosClient
      .get(`/doctors/${doctorId}/leaves`, { params })
      .then(unwrapList),

  getById: (doctorId, id) =>
    axiosClient.get(`/doctors/${doctorId}/leaves/${id}`).then(unwrap),

  create: (doctorId, payload) =>
    axiosClient.post(`/doctors/${doctorId}/leaves`, payload).then(unwrap),

  update: (doctorId, id, payload) =>
    axiosClient
      .put(`/doctors/${doctorId}/leaves/${id}`, payload)
      .then(unwrap),

  remove: (doctorId, id) =>
    axiosClient.delete(`/doctors/${doctorId}/leaves/${id}`).then(unwrap),
};
