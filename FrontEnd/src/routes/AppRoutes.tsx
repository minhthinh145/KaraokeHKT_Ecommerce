import React from "react";
import { Routes, Route } from "react-router-dom";
import { RoleBasedRoute } from "../components/Auth";
import { ROLE_GROUPS } from "../constants/auth";

// Route Components
import { AdminRoutes } from "./AdminRoutes";
import { EmployeeRoutes } from "./EmployeeRoutes";
import { CustomerRoutes } from "./CustomerRoutes";

// Public Pages - 🔥 SỬA: Dùng đúng path hiện có
import { LoginPage } from "../pages/login";
import { HomePage } from "../pages/HomePage";

export const AppRoutes: React.FC = () => {
  return (
    <Routes>
      {/* 🔓 Public Routes */}
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<LoginPage />} />

      {/* 🏛️ Admin Routes - Admin + Manager */}
      <Route
        path="/admin/*"
        element={
          <RoleBasedRoute
            allowedRoles={[...ROLE_GROUPS.ADMIN, ...ROLE_GROUPS.MANAGER]}
          >
            <AdminRoutes />
          </RoleBasedRoute>
        }
      />

      {/* 👷‍♂️ Employee Routes - Nhân viên */}
      <Route
        path="/employee/*"
        element={
          <RoleBasedRoute
            allowedRoles={[
              ...ROLE_GROUPS.ADMIN,
              ...ROLE_GROUPS.MANAGER,
              ...ROLE_GROUPS.EMPLOYEE,
            ]}
          >
            <EmployeeRoutes />
          </RoleBasedRoute>
        }
      />

      {/* 👤 Customer Routes - Khách hàng */}
      <Route
        path="/customer/*"
        element={
          <RoleBasedRoute allowedRoles={[...ROLE_GROUPS.CUSTOMER]}>
            <CustomerRoutes />
          </RoleBasedRoute>
        }
      />

      {/* 404 Page */}
      <Route path="*" element={<div>404 - Không tìm thấy trang</div>} />
    </Routes>
  );
};
