import React from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../../hooks/auth/useAuth";
import type { RoleBasedRouteProps, UserRole } from "../../types/auth";
import { getDefaultRoute } from "../../constants/auth";

export const RoleBasedRoute: React.FC<RoleBasedRouteProps> = ({
  children,
  allowedRoles,
  fallbackRoute,
}) => {
  const { isAuthenticated, userRole, loading } = useAuth();

  // 🔥 Show loading
  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-blue-500"></div>
      </div>
    );
  }

  // 🔥 Chưa đăng nhập
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  // 🔥 Không có quyền truy cập
  if (!userRole || !allowedRoles.includes(userRole as UserRole)) {
    // Nếu có fallbackRoute custom, dùng nó
    if (fallbackRoute) {
      return <Navigate to={fallbackRoute} replace />;
    }

    // Ngược lại, redirect về default route của user
    const defaultRoute = getDefaultRouteForRole(userRole);
    return <Navigate to={defaultRoute} replace />;
  }

  // 🔥 Có quyền -> render children
  return <>{children}</>;
};

// Helper function
const getDefaultRouteForRole = (role: UserRole | null | undefined): string => {
  if (!role) return "/login";
  return getDefaultRoute(role);
};
