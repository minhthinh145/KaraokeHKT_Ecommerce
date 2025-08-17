import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { RoleBasedRoute } from "../components/Auth";
import { ApplicationRole } from "../api/types/admins/QLHeThongtypes";

// Customer Pages
import { ProfilePage } from "../pages/ProfilePage";
import { BookingPage } from "../pages/customer/BookingPage";
import { HomePage } from "../pages/HomePage";
import { CustomerLayout } from "../components/customer/CustomerLayout";
import { BookingSuccessPage } from "../pages/customer/BookingSuccessPage";
import { BookingFailedPage } from "../pages/customer/BookingFailedPage";
import { BookingHistoryPage } from "../pages/customer/BookingHistoryPage";

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

      <Route path="booking/success" element={<BookingSuccessPage />} />
      <Route path="booking/failed" element={<BookingFailedPage />} />
      <Route path="booking/history" element={<BookingHistoryPage />} />

      {/* Default redirect */}
      <Route path="*" element={<Navigate to="/customer/dashboard" replace />} />
    </Routes>
  );
};

// Nếu bạn đang dùng mảng RouteObject thay vì JSX:
export const customerRoutes = [
  { path: "/booking", element: <BookingPage /> },
  { path: "/booking/history", element: <BookingHistoryPage /> },
  // success / failed (root có thể đã thêm)
];
