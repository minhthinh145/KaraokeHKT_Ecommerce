import React from "react";
import { Navigate } from "react-router-dom";
import { useSelector } from "react-redux";
import type { RootState } from "../../redux/store";

interface PublicRouteProps {
  children: React.ReactNode;
  redirectTo?: string;
}

export const PublicRoute: React.FC<PublicRouteProps> = ({
  children,
  redirectTo = "/dashboard",
}) => {
  const { isAuthenticated } = useSelector((state: RootState) => state.auth);

  // Nếu đã đăng nhập, redirect đến dashboard
  //if (isAuthenticated) {
  //return <Navigate to={redirectTo} replace />;
  //}

  return <>{children}</>;
};
