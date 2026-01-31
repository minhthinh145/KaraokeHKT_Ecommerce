import axiosInstance from "../../axiosConfig";
import type { ApiResponse, SearchParams, SearchResult } from "../base/types";

// 🔍 Generic search function
export const search = async <T>(
  endpoint: string,
  params: SearchParams
): Promise<ApiResponse<SearchResult<T>>> => {
  try {

    const queryParams = new URLSearchParams();

    if (params.query) queryParams.append("query", params.query);
    if (params.sortBy) queryParams.append("sortBy", params.sortBy);
    if (params.sortDirection)
      queryParams.append("sortDirection", params.sortDirection);
    if (params.page) queryParams.append("page", params.page.toString());
    if (params.pageSize)
      queryParams.append("pageSize", params.pageSize.toString());

    // Add filters
    if (params.filters) {
      Object.entries(params.filters).forEach(([key, value]) => {
        if (value !== undefined && value !== null && value !== "") {
          queryParams.append(`filter.${key}`, value.toString());
        }
      });
    }

    const url = `${endpoint}?${queryParams.toString()}`;
    const response = await axiosInstance.get(url);

    return response.data as ApiResponse<SearchResult<T>>;
  } catch (error: any) {
    console.error("❌ Search error:", error.response?.data || error.message);
    throw new Error(
      error.response?.data?.message || "Có lỗi xảy ra khi tìm kiếm"
    );
  }
};

// 🔍 Search employees
export const searchEmployees = async (params: SearchParams) => {
  return search("NhanSu/nhanvien/search", params);
};

// 🔍 Search employee accounts
export const searchEmployeeAccounts = async (params: SearchParams) => {
  return search("QLHeThong/taikhoan/nhanvien/search", params);
};

// 🔍 Search customer accounts
export const searchCustomerAccounts = async (params: SearchParams) => {
  return search("QLHeThong/taikhoan/khachhang/search", params);
};

// 🔍 Advanced search with multiple criteria
export const advancedSearch = async <T>(
  endpoint: string,
  searchCriteria: {
    textSearch?: {
      query: string;
      fields: string[];
    };
    dateRange?: {
      field: string;
      from?: string;
      to?: string;
    };
    numericRange?: {
      field: string;
      min?: number;
      max?: number;
    };
    exactMatch?: Record<string, any>;
  }
): Promise<ApiResponse<T[]>> => {
  try {

    const response = await axiosInstance.post(
      `${endpoint}/advanced-search`,
      searchCriteria
    );

    return response.data as ApiResponse<T[]>;
  } catch (error: any) {
    console.error(
      "❌ Advanced search error:",
      error.response?.data || error.message
    );
    throw new Error(
      error.response?.data?.message || "Có lỗi xảy ra khi tìm kiếm nâng cao"
    );
  }
};
