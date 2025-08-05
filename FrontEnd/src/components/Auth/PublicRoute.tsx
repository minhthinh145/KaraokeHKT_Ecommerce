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
  redirectTo = "/",
}) => {
  const { isAuthenticated, loading } = useSelector(
    (state: RootState) => state.auth
  );


  if (isAuthenticated && !loading) {
    return <Navigate to={redirectTo} replace />;
  }


  return <>{children}</>;
};
