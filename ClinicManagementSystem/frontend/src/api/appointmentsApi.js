import { axiosClient, unwrap, unwrapList } from "./axiosClient";

export const appointmentsApi = {
  // AdminOnly
  getAll: (params) =>
    axiosClient.get("/Appointments", { params }).then(unwrapList),

  // Authenticated (service enforces ownership for non-admins)
  getById: (id) => axiosClient.get(`/Appointments/${id}`).then(unwrap),

  // PatientOnly
  create: (payload) =>
    axiosClient.post("/Appointments", payload).then(unwrap),

  // PatientOnly
  update: (id, payload) =>
    axiosClient.put(`/Appointments/${id}`, payload).then(unwrap),

  // AdminOrDoctor
  confirm: (id) =>
    axiosClient.patch(`/Appointments/${id}/confirm`).then(unwrap),

  // DoctorOnly
  complete: (id) =>
    axiosClient.patch(`/Appointments/${id}/complete`).then(unwrap),

  // AdminOrDoctor
  cancel: (id) =>
    axiosClient.patch(`/Appointments/${id}/cancel`).then(unwrap),

  // AdminOnly
    remove: (id) => axiosClient.delete(`/Appointments/${id}`).then(unwrap),

    getWorkingHours: (doctorId) =>
        axiosClient
            .get(`/doctors/${doctorId}/working-hours`)
            .then(res => res.data.data.items),

    getAvailableSlots: (doctorId, date) =>
        axiosClient
            .get("/appointments/available-slots", {
                params: {
                    doctorId,
                    date
                }
            })
            .then(res => res.data.data)
};
