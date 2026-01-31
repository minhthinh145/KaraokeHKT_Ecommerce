import React, { useEffect } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../../hooks/auth/useAuth";
import type { ProtectedRouteProps } from "../../types/auth";
import type { UserRole } from "../../types/auth";
import { getDefaultRoute } from "../../constants/auth";

export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  children,
  requiredRole,
  fallbackRoute = "/login",
}) => {
  const { isAuthenticated, userRole, loading, loadProfile, profileLoaded } =
    useAuth();
  const location = useLocation();

  // 🔥 Auto load profile nếu đã login nhưng chưa có profile
  useEffect(() => {
    if (isAuthenticated && !profileLoaded && !loading) {
      loadProfile().catch((error) => {
        console.warn("Failed to load profile:", error);
      });
    }
  }, [isAuthenticated, profileLoaded, loading, loadProfile]);

  // 🔥 Show loading khi đang xác thực
  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-blue-500"></div>
      </div>
    );
  }

  // 🔥 Chưa đăng nhập -> redirect to login
  if (!isAuthenticated) {
    return (
      <Navigate
        to={fallbackRoute}
        state={{ from: location.pathname }}
        replace
      />
    );
  }

  // 🔥 Đã đăng nhập nhưng không đủ quyền -> redirect to default route của user
  if (requiredRole && userRole !== requiredRole) {
    const defaultRoute = getDefaultRouteForRole(userRole);
    return <Navigate to={defaultRoute} replace />;
  }

  // 🔥 Đủ điều kiện -> render children
  return <>{children}</>;
};

// Helper function để lấy default route
const getDefaultRouteForRole = (role: UserRole | null | undefined): string => {
  if (!role) return "/login";
  return getDefaultRoute(role);
};
