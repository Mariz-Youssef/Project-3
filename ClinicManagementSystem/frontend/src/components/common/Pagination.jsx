import "./Pagination.css";

/**
 * pagination shape (normalized): { pageNumber, pageSize, totalCount, totalPages }
 * Falls back gracefully if some fields are missing.
 */
export function Pagination({ pagination, onPageChange }) {
  if (!pagination) return null;

  const pageNumber =
    pagination.pageNumber ?? pagination.page ?? pagination.currentPage ?? 1;
  const totalPages =
    pagination.totalPages ??
    (pagination.totalCount && pagination.pageSize
      ? Math.ceil(pagination.totalCount / pagination.pageSize)
      : 1);

  if (totalPages <= 1) return null;

  return (
    <div className="pagination">
      <button
        type="button"
        disabled={pageNumber <= 1}
        onClick={() => onPageChange(pageNumber - 1)}
      >
        ← Prev
      </button>
      <span className="pagination__label">
        Page {pageNumber} of {totalPages}
        {pagination.totalCount != null && (
          <span className="pagination__count"> · {pagination.totalCount} total</span>
        )}
      </span>
      <button
        type="button"
        disabled={pageNumber >= totalPages}
        onClick={() => onPageChange(pageNumber + 1)}
      >
        Next →
      </button>
    </div>
  );
}
