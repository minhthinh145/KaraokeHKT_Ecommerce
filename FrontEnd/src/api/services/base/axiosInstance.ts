// 🔥 Re-export existing axios config with alias
export { default as axiosInstance } from "../../axiosConfig";

// 🔥 Use correct API Response structure from backend
export interface ApiResponse<T> {
  data?: T | null;
  message: string;
  status: number;
  isSuccess?: boolean;
}

// Re-export common types
export interface BaseEntity {
  id: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface PaginationParams {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDirection?: "asc" | "desc";
}

export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPrevPage: boolean;
}
