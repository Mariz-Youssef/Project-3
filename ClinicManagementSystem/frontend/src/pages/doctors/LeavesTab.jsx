import { useState } from "react";
import { doctorLeavesApi } from "../../api/doctorLeavesApi";
import { usePaginatedList } from "../../hooks/usePaginatedList";
import { Table } from "../../components/common/Table";
import { Pagination } from "../../components/common/Pagination";
import { Button } from "../../components/common/Button";
import { Badge } from "../../components/common/Badge";
import { ConfirmDialog } from "../../components/common/ConfirmDialog";
import { LeaveFormModal } from "./LeaveFormModal";
import { useToast } from "../../context/ToastContext";

const STATUS_TONE = {
  Approved: "mint",
  Pending: "warning",
  Rejected: "danger",
};

export function LeavesTab({ doctorId, canManage }) {
  const toast = useToast();
  const fetcher = (params) => doctorLeavesApi.getByDoctor(doctorId, params);

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
    toast.success(editing ? "Leave updated" : "Leave requested");
    reload();
  }

  async function confirmDelete() {
    setDeleting(true);
    try {
      await doctorLeavesApi.remove(doctorId, pendingDelete.id);
      toast.success("Leave deleted");
      setPendingDelete(null);
      reload();
    } catch {
      toast.error("Could not delete this leave.");
    } finally {
      setDeleting(false);
    }
  }

  const columns = [
    { key: "startDate", header: "Start", render: (r) => r.startDate?.slice(0, 10) },
    { key: "endDate", header: "End", render: (r) => r.endDate?.slice(0, 10) },
    { key: "reason", header: "Reason", render: (r) => r.reason || "—" },
    {
      key: "status",
      header: "Status",
      render: (r) => <Badge tone={STATUS_TONE[r.status] ?? "grey"}>{r.status ?? "Pending"}</Badge>,
    },
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
                  Delete
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
            + Request leave
          </Button>
        )}
      </div>

      <Table
        columns={columns}
        rows={items}
        loading={loading}
        emptyTitle="No leaves recorded"
        emptyMessage="Time off requests for this doctor will show up here."
      />

      <Pagination pagination={pagination} onPageChange={setPageNumber} />

      {showForm && (
        <LeaveFormModal
          doctorId={doctorId}
          leave={editing}
          onClose={() => setShowForm(false)}
          onSaved={handleSaved}
        />
      )}

      {pendingDelete && (
        <ConfirmDialog
          title="Delete leave"
          message="This leave record will be permanently deleted."
          confirmLabel="Delete"
          loading={deleting}
          onConfirm={confirmDelete}
          onCancel={() => setPendingDelete(null)}
        />
      )}
    </div>
  );
}
