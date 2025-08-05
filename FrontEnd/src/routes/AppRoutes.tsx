import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { ProtectedRoute } from "../components/Auth/ProtectedRoute";
import { PublicRoute } from "../components/Auth/PublicRoute";
import { LoginPage } from "../pages/login";
import { SignUpPage } from "../pages/signup";
import { HomePage } from "../pages/HomePage";
export const AppRoutes: React.FC = () => {
  return (
    <Routes>
      {/* Home Page - Public, ai cũng vào được */}
      <Route path="/" element={<HomePage />} />

      {/* Auth Routes - chỉ truy cập được khi chưa đăng nhập */}
      <Route
        path="/login"
        element={
          <PublicRoute>
            <LoginPage />
          </PublicRoute>
        }
      />
      <Route
        path="/signup"
        element={
          <PublicRoute>
            <SignUpPage />
          </PublicRoute>
        }
      />

      {/* Các chức năng khác - Protected, cần login */}
      <Route
        path="/booking"
        element={
          <ProtectedRoute>
            <div>Booking Page (Coming Soon)</div>
          </ProtectedRoute>
        }
      />
      <Route
        path="/profile"
        element={
          <ProtectedRoute>
            <div>Services Page (Coming Soon)</div>
          </ProtectedRoute>
        }
      />
      <Route
        path="/services"
        element={
          <ProtectedRoute>
            <div>Services Page (Coming Soon)</div>
          </ProtectedRoute>
        }
      />
      <Route
        path="/contact"
        element={
          <ProtectedRoute>
            <div>Contact Page (Coming Soon)</div>
          </ProtectedRoute>
        }
      />

      {/* 404 Route */}
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
};
