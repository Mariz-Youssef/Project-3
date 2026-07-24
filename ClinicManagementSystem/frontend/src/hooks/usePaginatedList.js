import { useCallback, useEffect, useState } from "react";
import { unwrapError } from "../api/axiosClient";

/**
 * Drives a paginated, optionally searchable list.
 * fetcher(params) must resolve to { items, pagination }.
 * searchFetcher(term, params) is used instead when a search term is present.
 */
export function usePaginatedList(fetcher, { pageSize = 10, searchFetcher } = {}) {
  const [items, setItems] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);
  const [searchTerm, setSearchTerm] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [reloadToken, setReloadToken] = useState(0);

  const reload = useCallback(() => setReloadToken((t) => t + 1), []);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    setError(null);

    const params = { pageNumber, pageSize };
    const request =
      searchTerm && searchFetcher
        ? searchFetcher(searchTerm, params)
        : fetcher(params);

    request
      .then((result) => {
        if (cancelled) return;
        setItems(result.items ?? []);
        setPagination(result.pagination ?? null);
      })
      .catch((err) => {
        if (cancelled) return;
        setError(unwrapError(err).message);
        setItems([]);
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });

    return () => {
      cancelled = true;
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pageNumber, searchTerm, reloadToken]);

  function updateSearchTerm(term) {
    setSearchTerm(term);
    setPageNumber(1);
  }

  return {
    items,
    pagination,
    pageNumber,
    setPageNumber,
    searchTerm,
    setSearchTerm: updateSearchTerm,
    loading,
    error,
    reload,
  };
}
