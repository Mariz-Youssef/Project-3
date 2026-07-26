import "./Table.css";
import { EmptyState } from "./EmptyState";
import { Loader } from "./Loader";

/**
 * columns: [{ key, header, render?(row) }]
 */
export function Table({
  columns,
  rows,
  loading,
  emptyTitle = "Nothing here yet",
  emptyMessage = "No records match your current view.",
  keyField = "id",
  onRowClick,
}) {
  if (loading) {
    return (
      <div className="table-state">
        <Loader label="Loading records" />
      </div>
    );
  }

  if (!rows || rows.length === 0) {
    return (
      <div className="table-state">
        <EmptyState title={emptyTitle} message={emptyMessage} />
      </div>
    );
  }

  return (
    <div className="table-scroll">
      <table className="data-table">
        <thead>
          <tr>
            {columns.map((col) => (
              <th key={col.key}>{col.header}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {rows.map((row) => (
            <tr
              key={row[keyField]}
              onClick={onRowClick ? () => onRowClick(row) : undefined}
              className={onRowClick ? "is-clickable" : ""}
            >
              {columns.map((col) => (
                <td key={col.key}>
                  {col.render ? col.render(row) : row[col.key]}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
