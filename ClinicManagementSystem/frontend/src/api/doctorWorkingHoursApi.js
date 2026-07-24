import { axiosClient, unwrap, unwrapList } from "./axiosClient";

export const doctorWorkingHoursApi = {
  getByDoctor: (doctorId, params) =>
    axiosClient
      .get(`/doctors/${doctorId}/working-hours`, { params })
      .then(unwrapList),

  getById: (doctorId, id) =>
    axiosClient.get(`/doctors/${doctorId}/working-hours/${id}`).then(unwrap),

  create: (doctorId, payload) =>
    axiosClient
      .post(`/doctors/${doctorId}/working-hours`, payload)
      .then(unwrap),

  update: (doctorId, id, payload) =>
    axiosClient
      .put(`/doctors/${doctorId}/working-hours/${id}`, payload)
      .then(unwrap),

  remove: (doctorId, id) =>
    axiosClient
      .delete(`/doctors/${doctorId}/working-hours/${id}`)
      .then(unwrap),
};
