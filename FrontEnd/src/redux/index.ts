// Re-export store setup
export * from "./store";

// Re-export domain modules (admin includes qlNhanSu with nhanVien + tienLuong)
export * from "./admin";
export * from "./auth";

// Typed hooks
export { useAppDispatch, useAppSelector } from "./hooks";
