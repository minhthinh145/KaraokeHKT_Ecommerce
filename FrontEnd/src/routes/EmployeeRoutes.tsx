import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { RoleBasedRoute } from "../components/Auth";
import { ApplicationRole } from "../api/types/admins/QLHeThongtypes";

// Employee Pages
import { NhanVienFrontDeskServicePage } from "../pages/employees/NhanVienFrontDeskServicePage";
import { NhanVienKhoPage } from "../pages/employees/NhanVienKhoPage";

export const EmployeeRoutes: React.FC = () => {
  return (
    <Routes>
      <Route
        path="nhan-vien-phuc-vu/:weekStart?"
        element={
          <RoleBasedRoute allowedRoles={[ApplicationRole.NhanVienPhucVu]}>
            <NhanVienFrontDeskServicePage />
          </RoleBasedRoute>
        }
      />
      <Route
        path="nhan-vien-tiep-tan/:weekStart?"
        element={
          <RoleBasedRoute allowedRoles={[ApplicationRole.NhanVienTiepTan]}>
            <NhanVienFrontDeskServicePage />
          </RoleBasedRoute>
        }
      />
      <Route
        path="nhan-vien-kho/:weekStart?"
        element={
          <RoleBasedRoute allowedRoles={[ApplicationRole.NhanVienKho]}>
            <NhanVienKhoPage />
          </RoleBasedRoute>
        }
      />
      <Route index element={<Navigate to="nhan-vien-phuc-vu" replace />} />
      <Route path="*" element={<Navigate to="nhan-vien-phuc-vu" replace />} />
    </Routes>
  );
};
