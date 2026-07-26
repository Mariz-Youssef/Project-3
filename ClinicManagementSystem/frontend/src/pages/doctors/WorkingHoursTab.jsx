import { useState } from "react";
import { doctorWorkingHoursApi } from "../../api/doctorWorkingHoursApi";
import { usePaginatedList } from "../../hooks/usePaginatedList";
import { Table } from "../../components/common/Table";
import { Pagination } from "../../components/common/Pagination";
import { Button } from "../../components/common/Button";
import { ConfirmDialog } from "../../components/common/ConfirmDialog";
import { WorkingHourFormModal } from "./WorkingHourFormModal";
import { useToast } from "../../context/ToastContext";

export function WorkingHoursTab({ doctorId, canManage }) {
  const toast = useToast();
  const fetcher = (params) =>
    doctorWorkingHoursApi.getByDoctor(doctorId, params);

  const { items, pagination, pageNumber, setPageNumber, loading, reload } =
    usePaginatedList(fetcher, { pageSize: 10 });

  const [editing, setEditing] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [pendingDelete, setPendingDelete] = useState(null);
  const [deleting, setDeleting] = useState(false);

  function openCreate() {
    setEditing(null);
    setShowForm(true);
  }

  function openEdit(row) {
    setEditing(row);
    setShowForm(true);
  }

  function handleSaved() {
    setShowForm(false);
    toast.success(editing ? "Working hours updated" : "Working hours added");
    reload();
  }

  async function confirmDelete() {
    setDeleting(true);
    try {
      await doctorWorkingHoursApi.remove(doctorId, pendingDelete.id);
      toast.success("Working hours removed");
      setPendingDelete(null);
      reload();
    } catch {
      toast.error("Could not remove this entry.");
    } finally {
      setDeleting(false);
    }
  }

  const columns = [
    { key: "dayOfWeek", header: "Day" },
    { key: "startTime", header: "Start" },
    { key: "endTime", header: "End" },
    ...(canManage
      ? [
          {
            key: "actions",
            header: "",
            render: (row) => (
              <div className="table-row-actions">
                <Button size="sm" variant="ghost" onClick={() => openEdit(row)}>
                  Edit
                </Button>
                <Button
                  size="sm"
                  variant="danger"
                  onClick={() => setPendingDelete(row)}
                >
                  Remove
                </Button>
              </div>
            ),
          },
        ]
      : []),
  ];

  return (
    <div className="stack-vertical">
      <div className="page-toolbar" style={{ justifyContent: "flex-end" }}>
        {canManage && (
          <Button size="sm" onClick={openCreate}>
            + Add working hours
          </Button>
        )}
      </div>

      <Table
        columns={columns}
        rows={items}
        loading={loading}
        emptyTitle="No working hours set"
        emptyMessage="Add this doctor's weekly availability."
      />

      <Pagination pagination={pagination} onPageChange={setPageNumber} />

      {showForm && (
        <WorkingHourFormModal
          doctorId={doctorId}
          workingHour={editing}
          onClose={() => setShowForm(false)}
          onSaved={handleSaved}
        />
      )}

      {pendingDelete && (
        <ConfirmDialog
          title="Remove working hours"
          message={`Remove ${pendingDelete.dayOfWeek} ${pendingDelete.startTime}–${pendingDelete.endTime}?`}
          confirmLabel="Remove"
          loading={deleting}
          onConfirm={confirmDelete}
          onCancel={() => setPendingDelete(null)}
        />
      )}
    </div>
  );
}
