import { ReactNode } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "@/providers/AuthProvider";

type ProtectedRouteProps = {
  children: ReactNode;
  requiredRoles?: string[];
  redirectTo?: string;
};

export function ProtectedRoute({
  children,
  requiredRoles,
  redirectTo = "/",
}: ProtectedRouteProps) {
  const auth = useAuth();
  const location = useLocation();

  const isAuthenticated = auth.isAuthenticated;
  const roles = auth.roles ?? [];

  const hasRequiredRole = !requiredRoles?.length
    ? true
    : requiredRoles.some((r) => roles.includes(r.toLowerCase()));

  if (!isAuthenticated || !hasRequiredRole) {
    return <Navigate to={redirectTo} replace state={{ from: location }} />;
  }

  return <>{children}</>;
}
