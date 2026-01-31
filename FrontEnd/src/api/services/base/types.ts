// 🔥 Re-export correct ApiResponse from existing types
export type { ApiResponse } from "../../types/apiResponse";

// Re-export other types from axiosInstance
export type {
  BaseEntity,
  PaginationParams,
  PaginatedResponse,
} from "./axiosInstance";

// Additional types specific to shared services
export interface SearchParams {
  query?: string;
  filters?: Record<string, any>;
  sortBy?: string;
  sortDirection?: "asc" | "desc";
  page?: number;
  pageSize?: number;
}

export interface SearchResult<T> {
  data: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

// Error response structure
export interface ApiError {
  message: string;
  status: number;
  isSuccess: boolean;
  data?: any;
}

// HTTP methods
export type HttpMethod = "GET" | "POST" | "PUT" | "DELETE" | "PATCH";

// Request configuration
export interface RequestConfig {
  method: HttpMethod;
  url: string;
  data?: any;
  params?: Record<string, any>;
  headers?: Record<string, string>;
}
