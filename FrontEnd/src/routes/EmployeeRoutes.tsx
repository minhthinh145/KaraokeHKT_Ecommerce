import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { RoleBasedRoute } from "../components/Auth";
import { ApplicationRole } from "../api/types/admins/QLHeThongtypes";

// Employee Pages
import { NhanVienKhoPage } from "../pages/employee/NhanVienKhoPage";
import { NhanVienPhucVuPage } from "../pages/employee/NhanVienPhucVuPage";
import { NhanVienTiepTanPage } from "../pages/employee/NhanVienTiepTanPage";

export const EmployeeRoutes: React.FC = () => {
  return (
    <Routes>
      {/* 📦 Nhân viên kho */}
      <Route
        path="nhan-vien-kho"
        element={
          <RoleBasedRoute
            allowedRoles={[
              ApplicationRole.QuanTriHeThong,
              ApplicationRole.QuanLyKho,
              ApplicationRole.NhanVienKho,
            ]}
          >
            <NhanVienKhoPage />
          </RoleBasedRoute>
        }
      />

      {/* 🍻 Nhân viên phục vụ */}
      <Route
        path="nhan-vien-phuc-vu"
        element={
          <RoleBasedRoute
            allowedRoles={[
              ApplicationRole.QuanTriHeThong,
              ApplicationRole.QuanLyPhongHat,
              ApplicationRole.NhanVienPhucVu,
            ]}
          >
            <NhanVienPhucVuPage />
          </RoleBasedRoute>
        }
      />

      {/* 🎫 Nhân viên tiếp tân */}
      <Route
        path="nhan-vien-tiep-tan"
        element={
          <RoleBasedRoute
            allowedRoles={[
              ApplicationRole.QuanTriHeThong,
              ApplicationRole.QuanLyPhongHat,
              ApplicationRole.NhanVienTiepTan,
            ]}
          >
            <NhanVienTiepTanPage />
          </RoleBasedRoute>
        }
      />

      {/* Default redirect */}
      <Route
        path="*"
        element={<Navigate to="/employee/nhan-vien-kho" replace />}
      />
    </Routes>
  );
};
