import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { RoleBasedRoute } from "../components/Auth";
import { ApplicationRole } from "../api/types/admins/QLHeThongtypes";

// Customer Pages
import { ProfilePage } from "../pages/ProfilePage";
import { BookingPage } from "../pages/customer/BookingPage";
import { HomePage } from "../pages/HomePage";

export const CustomerRoutes: React.FC = () => {
  return (
    <Routes>
      {/* 🏠 Customer Dashboard */}
      <Route
        path="dashboard"
        element={
          <RoleBasedRoute allowedRoles={[ApplicationRole.KhachHang]}>
            <HomePage />
          </RoleBasedRoute>
        }
      />

      {/* 👤 Profile - Chỉ cho khách hàng */}
      <Route
        path="profile"
        element={
          <RoleBasedRoute allowedRoles={[ApplicationRole.KhachHang]}>
            <ProfilePage />
          </RoleBasedRoute>
        }
      />

      {/* 🎵 Booking */}
      <Route
        path="booking"
        element={
          <RoleBasedRoute allowedRoles={[ApplicationRole.KhachHang]}>
            <BookingPage />
          </RoleBasedRoute>
        }
      />

      {/* Default redirect */}
      <Route path="*" element={<Navigate to="/customer/dashboard" replace />} />
    </Routes>
  );
};
