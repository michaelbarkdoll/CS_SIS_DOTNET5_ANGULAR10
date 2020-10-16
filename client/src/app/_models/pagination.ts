export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

// T represents an array of members in first example
export class PaginatedResult<T> {
    result: T;     // array here (members as an example)
    pagination: Pagination;
}