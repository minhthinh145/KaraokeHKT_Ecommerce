import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { RoleBasedRoute } from "../components/Auth";
import { ApplicationRole } from "../api/types/admins/QLHeThongtypes";

// Admin Pages
import { QLHeThongPage } from "../pages/admin/QLHeThongPage";
import { QLNhanSuPage } from "../pages/admin/QLNhanSuPage";
import { QLKhoPage } from "../pages/admin/QlKhoPage";
import { QLPhongPage } from "../pages/admin/QLPhongPage";
import { BookingSuccessPage } from "../pages/customer/BookingSuccessPage";
import { BookingFailedPage } from "../pages/customer/BookingFailedPage";

export const AdminRoutes: React.FC = () => {
  return (
    <Routes>
      {/* 🏛️ Quản trị hệ thống - Chỉ QuanTriHeThong */}
      <Route
        path="quan-tri-he-thong"
        element={
          <RoleBasedRoute allowedRoles={[ApplicationRole.QuanTriHeThong]}>
            <QLHeThongPage />
          </RoleBasedRoute>
        }
      />

      {/* 👥 Quản lý nhân sự - QuanTriHeThong + QuanLyNhanSu */}
      <Route
        path="quan-ly-nhan-su"
        element={
          <RoleBasedRoute allowedRoles={[ApplicationRole.QuanLyNhanSu]}>
            <QLNhanSuPage />
          </RoleBasedRoute>
        }
      />

      {/* 📦 Quản lý kho - QuanTriHeThong + QuanLyKho */}
      <Route
        path="quan-ly-kho"
        element={
          <RoleBasedRoute allowedRoles={[ApplicationRole.QuanLyKho]}>
            <QLKhoPage />
          </RoleBasedRoute>
        }
      />

      {/* 🎤 Quản lý phòng hát - QuanTriHeThong + QuanLyPhongHat */}
      <Route
        path="quan-ly-phong-hat"
        element={
          <RoleBasedRoute allowedRoles={[ApplicationRole.QuanLyPhongHat]}>
            <QLPhongPage />
          </RoleBasedRoute>
        }
      />

      {/* Đường dẫn cho trang thành công và thất bại của booking */}
      <Route path="/booking/success" element={<BookingSuccessPage />} />
      <Route path="/booking/failed" element={<BookingFailedPage />} />

      {/* Default redirect cho admin routes */}
      <Route
        path="*"
        element={<Navigate to="/admin/quan-tri-he-thong" replace />}
      />
    </Routes>
  );
};
