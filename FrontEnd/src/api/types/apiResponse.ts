export interface ApiResponse<T> {
  data?: T | null;
  message: string;
  status: number;
  isSuccess?: boolean;
}
