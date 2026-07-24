import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { appointmentsApi } from "../../api/appointmentsApi";
import { Table } from "../../components/common/Table";
import { Badge } from "../../components/common/Badge";
import { Button } from "../../components/common/Button";
import { FormField, TextInput } from "../../components/common/FormField";
import { useAuth } from "../../context/AuthContext";
import { getCachedAppointmentIds, addCachedAppointmentId } from "../../utils/appointmentCache";
import { STATUS_TONE, personDisplayName } from "./statusTone";

export function MyAppointmentsPage() {
  const { user } = useAuth();
  const navigate = useNavigate();

  const [appointments, setAppointments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [lookupId, setLookupId] = useState("");
  const [lookupError, setLookupError] = useState(null);

  function loadCached() {
    const ids = getCachedAppointmentIds(user?.id);
    if (ids.length === 0) {
      setAppointments([]);
      setLoading(false);
      return;
    }
    setLoading(true);
    Promise.allSettled(ids.map((id) => appointmentsApi.getById(id))).then(
      (results) => {
        setAppointments(
          results
            .filter((r) => r.status === "fulfilled")
            .map((r) => r.value)
        );
        setLoading(false);
      }
    );
  }

  useEffect(loadCached, [user?.id]);

  function handleAddById(e) {
    e.preventDefault();
    setLookupError(null);
    const id = Number(lookupId);
    if (!id) {
      setLookupError("Enter a valid appointment ID.");
      return;
    }
    appointmentsApi
      .getById(id)
      .then(() => {
        addCachedAppointmentId(user?.id, id);
        setLookupId("");
        loadCached();
      })
      .catch(() => setLookupError("No appointment found with that ID."));
  }

  const columns = [
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
        r.appointmentDate ? new Date(r.appointmentDate).toLocaleString() : "—",
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
          <h1 className="page-title">My appointments</h1>
          <p className="page-subtitle">
            Appointments you've booked from this browser.
          </p>
        </div>
        <Button onClick={() => navigate("/appointments/book")}>
          + Book appointment
        </Button>
      </div>

      <div
        className="form-error-banner"
        style={{
          background: "var(--color-mint-tint)",
          color: "var(--color-mint-darker)",
          border: "1px solid var(--color-mint-tint-strong)",
          marginBottom: "var(--space-4)",
        }}
      >
        This list is remembered on this device only. If you booked from
        another browser, look it up below by its appointment ID.
      </div>

      <form
        onSubmit={handleAddById}
        className="page-toolbar"
        style={{ alignItems: "flex-end" }}
      >
        <FormField label="Add an appointment by ID" htmlFor="lookup-id" error={lookupError}>
          <TextInput
            id="lookup-id"
            type="number"
            min="1"
            value={lookupId}
            onChange={(e) => setLookupId(e.target.value)}
            placeholder="e.g. 42"
          />
        </FormField>
        <Button type="submit" variant="ghost">
          Add
        </Button>
      </form>

      <Table
        columns={columns}
        rows={appointments}
        loading={loading}
        emptyTitle="No appointments yet"
        emptyMessage="Book your first appointment to see it here."
        onRowClick={(row) => navigate(`/appointments/${row.id}`)}
      />

      <p style={{ fontSize: 12, color: "var(--color-slate)", marginTop: "var(--space-4)" }}>
        Looking for something else?{" "}
        <Link to="/appointments/find" style={{ color: "var(--color-mint-dark)" }}>
          Look up any appointment by ID
        </Link>
        .
      </p>
    </div>
  );
}
