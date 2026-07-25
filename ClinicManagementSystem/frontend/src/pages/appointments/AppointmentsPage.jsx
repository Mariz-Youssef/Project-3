import { useNavigate } from "react-router-dom";
import { appointmentsApi } from "../../api/appointmentsApi";
import { usePaginatedList } from "../../hooks/usePaginatedList";
import { Table } from "../../components/common/Table";
import { Pagination } from "../../components/common/Pagination";
import { Badge } from "../../components/common/Badge";
import { Button } from "../../components/common/Button";
import { STATUS_TONE, personDisplayName } from "./statusTone";

export function AppointmentsPage() {
  const navigate = useNavigate();

  const { items, pagination, pageNumber, setPageNumber, loading } =
    usePaginatedList(appointmentsApi.getAll, { pageSize: 10 });

  const columns = [
    { key: "id", header: "ID" },
    {
      key: "patient",
      header: "Patient",
      render: (r) =>
        personDisplayName(r.patientName, r.patientFirstName, r.patientLastName, r.patientId),
    },
    {
      key: "doctor",
      header: "Doctor",
      render: (r) =>
        personDisplayName(r.doctorName, r.doctorFirstName, r.doctorLastName, r.doctorId),
    },
    {
      key: "appointmentDate",
      header: "Date & time",
      render: (r) =>
        r.appointmentDate
          ? new Date(r.appointmentDate).toLocaleString()
          : "—",
    },
    {
      key: "status",
      header: "Status",
      render: (r) => (
        <Badge tone={STATUS_TONE[r.status] ?? "grey"}>{r.status ?? "—"}</Badge>
      ),
    },
  ];

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="page-eyebrow">Scheduling</p>
          <h1 className="page-title">Appointments</h1>
          <p className="page-subtitle">
            All appointments across the clinic. Open one to confirm, complete,
            cancel, or delete it.
          </p>
        </div>
        <Button variant="ghost" onClick={() => navigate("/appointments/find")}>
          Look up by ID
        </Button>
      </div>

      <Table
        columns={columns}
        rows={items}
        loading={loading}
        emptyTitle="No appointments yet"
        emptyMessage="Appointments booked by patients will show up here."
        onRowClick={(row) => navigate(`/appointments/${row.id}`)}
      />

      <Pagination pagination={pagination} onPageChange={setPageNumber} />
    </div>
  );
}
