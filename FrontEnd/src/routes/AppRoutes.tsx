import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { ProtectedRoute } from "../components/Auth/ProtectedRoute";
import { PublicRoute } from "../components/Auth/PublicRoute";
import { LoginPage } from "../pages/login";

export const AppRoutes: React.FC = () => {
  return (
    <Routes>
      {/* Public Routes - chỉ truy cập được khi chưa đăng nhập */}
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
            <div>SignUp Page (Coming Soon)</div>
          </PublicRoute>
        }
      />
      <Route
        path="/forgot-password"
        element={
          <PublicRoute>
            <div>Forgot Password Page (Coming Soon)</div>
          </PublicRoute>
        }
      />

      {/* Protected Routes - chỉ truy cập được khi đã đăng nhập */}
      <Route
        path="/dashboard"
        element={
          <ProtectedRoute>
            <div>Dashboard Page (Coming Soon)</div>
          </ProtectedRoute>
        }
      />
      <Route
        path="/profile"
        element={
          <ProtectedRoute>
            <div>Profile Page (Coming Soon)</div>
          </ProtectedRoute>
        }
      />

      {/* Default Routes */}
      <Route path="/" element={<Navigate to="/login" replace />} />

      {/* 404 Route */}
      <Route path="*" element={<div>404 - Page Not Found</div>} />
    </Routes>
  );
};
