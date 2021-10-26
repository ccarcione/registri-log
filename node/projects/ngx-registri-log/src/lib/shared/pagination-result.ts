export class PageInfo {
    totalCount: number;
    pageSize: number;
    currentPage: number;
    totalPages: number;
  }
  
  export class PaginationResult<T> {
    data: T[];
    pagination: PageInfo;
  }
  