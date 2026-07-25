import { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { doctorsApi } from "../../api/doctorsApi";
import { unwrapError } from "../../api/axiosClient";
import { Table } from "../../components/common/Table";
import { Pagination } from "../../components/common/Pagination";
import { SearchInput } from "../../components/common/SearchInput";
import { Button } from "../../components/common/Button";
import { Badge } from "../../components/common/Badge";
import { ConfirmDialog } from "../../components/common/ConfirmDialog";
import { DoctorFormModal } from "./DoctorFormModal";
import { useToast } from "../../context/ToastContext";
import { useAuth } from "../../context/AuthContext";
import { ROLES } from "../../utils/roles";
import { personDisplayName } from "../../utils/personDisplay";

const PAGE_SIZE = 10;

export function DoctorsPage() {
  const toast = useToast();
  const navigate = useNavigate();
  const { user } = useAuth();
  const isAdmin = user?.role === ROLES.ADMIN;

  const [searchParams, setSearchParams] = useSearchParams();
  const departmentId = searchParams.get("departmentId");

  const [specialization, setSpecialization] = useState("");
  const [items, setItems] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);
  const [loading, setLoading] = useState(true);
  const [reloadToken, setReloadToken] = useState(0);

  const [editing, setEditing] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [pendingDelete, setPendingDelete] = useState(null);
  const [deleting, setDeleting] = useState(false);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    const params = { pageNumber, pageSize: PAGE_SIZE };

    const request = specialization
      ? doctorsApi.getBySpecialization(specialization, params)
      : departmentId
        ? doctorsApi.getByDepartment(departmentId, params)
        : doctorsApi.getAll(params);

    request
      .then((res) => {
        if (cancelled) return;
        setItems(res.items);
        setPagination(res.pagination);
      })
      .catch((err) => {
        if (cancelled) return;
        toast.error(unwrapError(err).message);
        setItems([]);
      })
      .finally(() => !cancelled && setLoading(false));

    return () => {
      cancelled = true;
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [departmentId, specialization, pageNumber, reloadToken]);

  function clearDepartmentFilter() {
    searchParams.delete("departmentId");
    setSearchParams(searchParams);
  }

  function openCreate() {
    setEditing(null);
    setShowForm(true);
  }

  function openEdit(doctor, e) {
    e.stopPropagation();
    setEditing(doctor);
    setShowForm(true);
  }

  function handleSaved() {
    setShowForm(false);
    toast.success(editing ? "Doctor updated" : "Doctor created");
    setReloadToken((t) => t + 1);
  }

  async function confirmDelete() {
    setDeleting(true);
    try {
      await doctorsApi.remove(pendingDelete.id);
      toast.success("Doctor deleted");
      setPendingDelete(null);
      setReloadToken((t) => t + 1);
    } catch (err) {
      toast.error(unwrapError(err).message);
    } finally {
      setDeleting(false);
    }
  }

  const columns = [
    {
      key: "name",
      header: "Name",
      render: (row) => personDisplayName(row, "Doctor"),
    },
    {
      key: "specialization",
      header: "Specialization",
      render: (row) => <Badge tone="mint">{row.specialization}</Badge>,
    },
    //{ key: "licenseNumber", header: "License #", render: (row) => row.licenseNumber || "—" },
    //{
    //  key: "yearsOfExperience",
    //  header: "Experience",
    //  render: (row) => (row.yearsOfExperience != null ? `${row.yearsOfExperience} yrs` : "—"),
    //},
    {
      key: "consultationFee",
      header: "Fee",
      render: (row) =>
        row.consultationFee != null ? `$${Number(row.consultationFee).toFixed(2)}` : "—",
    },
    ...(isAdmin
      ? [
          {
            key: "actions",
            header: "",
            render: (row) => (
              <div className="table-row-actions">
                <Button size="sm" variant="ghost" onClick={(e) => openEdit(row, e)}>
                  Edit
                </Button>
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
          <p className="page-eyebrow">Care team</p>
          <h1 className="page-title">Doctors</h1>
          <p className="page-subtitle">
            Profiles, specializations, working hours, and leave records.
          </p>
        </div>
        {isAdmin && <Button onClick={openCreate}>+ Add doctor</Button>}
      </div>

      <div className="page-toolbar">
        <SearchInput
          value={specialization}
          onChange={(v) => {
            setSpecialization(v);
            setPageNumber(1);
          }}
          placeholder="Filter by specialization..."
        />
        {departmentId && (
          <Badge tone="grey">
            Department #{departmentId}{" "}
            <button
              onClick={clearDepartmentFilter}
              style={{
                border: "none",
                background: "none",
                marginLeft: 6,
                cursor: "pointer",
                color: "inherit",
              }}
            >
              ✕
            </button>
          </Badge>
        )}
      </div>

      <Table
        columns={columns}
        rows={items}
        loading={loading}
        emptyTitle="No doctors found"
        emptyMessage="Try clearing filters or add a new doctor."
        onRowClick={(row) => navigate(`/doctors/${row.id}`)}
      />

      <Pagination pagination={pagination} onPageChange={setPageNumber} />

      {showForm && (
        <DoctorFormModal
          doctor={editing}
          onClose={() => setShowForm(false)}
          onSaved={handleSaved}
        />
      )}

      {pendingDelete && (
        <ConfirmDialog
          title="Delete doctor"
          message={`Delete ${personDisplayName(pendingDelete, "Doctor")}'s profile? This can't be undone.`}
          confirmLabel="Delete"
          loading={deleting}
          onConfirm={confirmDelete}
          onCancel={() => setPendingDelete(null)}
        />
      )}
    </div>
  );
}
