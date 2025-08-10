import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { RoleBasedRoute } from "../components/Auth";
import { ApplicationRole } from "../api/types/admins/QLHeThongtypes";

// Admin Pages
import { QLHeThongPage } from "../pages/admin/QLHeThongPage";
import { QLNhanSuPage } from "../pages/admin/QLNhanSuPage";
import { QLKhoPage } from "../pages/admin/QlKhoPage";
import { QuanLyPhongHatPage } from "../pages/admin/QuanLyPhongHatPage";

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
            <QuanLyPhongHatPage />
          </RoleBasedRoute>
        }
      />

      {/* Default redirect cho admin routes */}
      <Route
        path="*"
        element={<Navigate to="/admin/quan-tri-he-thong" replace />}
      />
    </Routes>
  );
};
