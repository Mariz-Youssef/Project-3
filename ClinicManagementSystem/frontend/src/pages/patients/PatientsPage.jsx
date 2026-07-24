import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { patientsApi } from "../../api/patientsApi";
import { usePaginatedList } from "../../hooks/usePaginatedList";
import { Table } from "../../components/common/Table";
import { Pagination } from "../../components/common/Pagination";
import { SearchInput } from "../../components/common/SearchInput";
import { Button } from "../../components/common/Button";
import { ConfirmDialog } from "../../components/common/ConfirmDialog";
import { useToast } from "../../context/ToastContext";
import { useAuth } from "../../context/AuthContext";
import { ROLES } from "../../utils/roles";

export function PatientsPage() {
  const toast = useToast();
  const navigate = useNavigate();
  const { user } = useAuth();
  const isAdmin = user?.role === ROLES.ADMIN;

  const {
    items,
    pagination,
    pageNumber,
    setPageNumber,
    searchTerm,
    setSearchTerm,
    loading,
    reload,
  } = usePaginatedList(patientsApi.getAll, { searchFetcher: patientsApi.search });

  const [pendingDelete, setPendingDelete] = useState(null);
  const [deleting, setDeleting] = useState(false);

  async function confirmDelete() {
    setDeleting(true);
    try {
      await patientsApi.remove(pendingDelete.id);
      toast.success("Patient profile deleted");
      setPendingDelete(null);
      reload();
    } catch {
      toast.error("Could not delete this patient.");
    } finally {
      setDeleting(false);
    }
  }

  const columns = [
    {
      key: "name",
      header: "Name",
      render: (row) =>
        `${row.firstName ?? ""} ${row.lastName ?? ""}`.trim() || row.name,
    },
    { key: "email", header: "Email", render: (row) => row.email || "—" },
    { key: "phone", header: "Phone", render: (row) => row.phone || "—" },
    {
      key: "dateOfBirth",
      header: "Date of birth",
      render: (row) => row.dateOfBirth?.slice(0, 10) || "—",
    },
    ...(isAdmin
      ? [
          {
            key: "actions",
            header: "",
            render: (row) => (
              <div className="table-row-actions">
                <Button
                  size="sm"
                  variant="danger"
                  onClick={(e) => {
                    e.stopPropagation();
                    setPendingDelete(row);
                  }}
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
    <div className="page">
      <div className="page-header">
        <div>
          <p className="page-eyebrow">Records</p>
          <h1 className="page-title">Patients</h1>
          <p className="page-subtitle">
            Search registered patient profiles across the clinic.
          </p>
        </div>
      </div>

      <div className="page-toolbar">
        <SearchInput
          value={searchTerm}
          onChange={setSearchTerm}
          placeholder="Search patients by name or email..."
        />
      </div>

      <Table
        columns={columns}
        rows={items}
        loading={loading}
        emptyTitle="No patients found"
        emptyMessage={
          searchTerm ? "Try a different search term." : "No patients registered yet."
        }
        onRowClick={(row) => navigate(`/patients/${row.id}`)}
      />

      <Pagination pagination={pagination} onPageChange={setPageNumber} />

      {pendingDelete && (
        <ConfirmDialog
          title="Delete patient"
          message={`Delete ${pendingDelete.firstName ?? ""} ${pendingDelete.lastName ?? ""}'s profile? This can't be undone.`}
          confirmLabel="Delete"
          loading={deleting}
          onConfirm={confirmDelete}
          onCancel={() => setPendingDelete(null)}
        />
      )}
    </div>
  );
}
