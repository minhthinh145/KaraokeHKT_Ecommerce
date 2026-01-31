import React from "react";
import { Routes, Route } from "react-router-dom";
import { RoleBasedRoute } from "../components/Auth";
import { ROLE_GROUPS } from "../constants/auth";

// Route Components
import { AdminRoutes } from "./AdminRoutes";
import { EmployeeRoutes } from "./EmployeeRoutes";
import { CustomerRoutes } from "./CustomerRoutes";

// Public Pages
import { LoginPage } from "../pages/login";
import { HomePage } from "../pages/HomePage";
import { SignUpPage } from "../pages/signup";

export const AppRoutes: React.FC = () => {
  return (
    <Routes>
      {/* 👤 HomePage chỉ cho CUSTOMER */}
      <Route
        path="/"
        element={
          <RoleBasedRoute allowedRoles={[...ROLE_GROUPS.CUSTOMER]}>
            <HomePage />
          </RoleBasedRoute>
        }
      />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/signup" element={<SignUpPage />} />

      {/* 🏛️ Admin Routes */}
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

      {/* 👷‍♂️ Employee Routes */}
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

      {/* 👤 Customer Routes */}
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
