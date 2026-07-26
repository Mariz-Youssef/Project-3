import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { departmentsApi } from "../../api/departmentsApi";
import { usePaginatedList } from "../../hooks/usePaginatedList";
import { Table } from "../../components/common/Table";
import { Pagination } from "../../components/common/Pagination";
import { SearchInput } from "../../components/common/SearchInput";
import { Button } from "../../components/common/Button";
import { ConfirmDialog } from "../../components/common/ConfirmDialog";
import { DepartmentFormModal } from "./DepartmentFormModal";
import { useToast } from "../../context/ToastContext";
import { useAuth } from "../../context/AuthContext";
import { ROLES } from "../../utils/roles";

export function DepartmentsPage() {
  const toast = useToast();
  const { user } = useAuth();
  const navigate = useNavigate();
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
  } = usePaginatedList(departmentsApi.getAll, {
    searchFetcher: departmentsApi.search,
  });

  const [editing, setEditing] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [pendingDelete, setPendingDelete] = useState(null);
  const [deleting, setDeleting] = useState(false);

  function openCreate() {
    setEditing(null);
    setShowForm(true);
  }

  function openEdit(department) {
    setEditing(department);
    setShowForm(true);
  }

  function handleSaved() {
    setShowForm(false);
    toast.success(editing ? "Department updated" : "Department created");
    reload();
  }

  async function confirmDelete() {
    setDeleting(true);
    try {
      await departmentsApi.remove(pendingDelete.id);
      toast.success("Department deleted");
      setPendingDelete(null);
      reload();
    } catch (err) {
      toast.error(err?.response?.data?.message ?? "Could not delete department");
    } finally {
      setDeleting(false);
    }
  }

  const columns = [
    { key: "name", header: "Name" },
    {
      key: "description",
      header: "Description",
      render: (row) => row.description || "—",
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
                    <Button
                        size="sm"
                        variant="ghost"
                        onClick={(e) => {
                            e.stopPropagation();
                            openEdit(row);
                        }}
                    >
                        Edit
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
          <p className="page-eyebrow">Directory</p>
          <h1 className="page-title">Departments</h1>
          <p className="page-subtitle">
            The clinical departments patients and doctors are organized under.
          </p>
        </div>
        {isAdmin && <Button onClick={openCreate}>+ Add department</Button>}
      </div>

      <div className="page-toolbar">
        <SearchInput
          value={searchTerm}
          onChange={setSearchTerm}
          placeholder="Search departments..."
        />
      </div>

      <Table
        columns={columns}
        rows={items}
        loading={loading}
        emptyTitle="No departments found"
        emptyMessage={
          searchTerm
            ? "Try a different search term."
            : "Add the first department to get started."
        }
        onRowClick={(row) => navigate(`/doctors?departmentId=${row.id}`)}
      />

      <Pagination pagination={pagination} onPageChange={setPageNumber} />

      {showForm && (
        <DepartmentFormModal
          department={editing}
          onClose={() => setShowForm(false)}
          onSaved={handleSaved}
        />
      )}

      {pendingDelete && (
        <ConfirmDialog
          title="Delete department"
          message={`Delete "${pendingDelete.name}"? This can't be undone.`}
          confirmLabel="Delete"
          loading={deleting}
          onConfirm={confirmDelete}
          onCancel={() => setPendingDelete(null)}
        />
      )}
    </div>
  );
}
