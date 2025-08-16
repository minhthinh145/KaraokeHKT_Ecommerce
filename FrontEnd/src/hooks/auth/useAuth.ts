import { useCallback } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import type { AppDispatch } from "../../redux/store";
import type { UseAuthReturn, UserRole } from "../../types/auth";
import {
  signInThunk,
  logoutThunk,
  fetchProfileThunk,
  clearError,
} from "../../redux/auth";
import {
  selectAuth,
  selectUser,
  selectUserRole,
  selectIsAuthenticated,
  selectUserDefaultRoute,
} from "../../redux/auth/selectors";
import {
  getDefaultRoute,
  canUserAccessRoute,
  isUserInRoleGroup,
  ROLE_GROUPS,
} from "../../constants/auth";
import { ApplicationRole } from "../../api/types/admins/QLHeThongtypes";
import type { SignInDTO } from "../../api";

export const useAuth = (): UseAuthReturn => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  // Selectors
  const auth = useSelector(selectAuth);
  const user = useSelector(selectUser);
  const userRole = useSelector(selectUserRole) as UserRole | null;
  const isAuthenticated = useSelector(selectIsAuthenticated) as boolean;
  const defaultRoute = useSelector(selectUserDefaultRoute) as string;

  // 🔥 Core login function - KHÔNG có UI logic
  const login = useCallback(
    async (credentials: SignInDTO) => {
      const payload = {
        ...credentials,
        email: credentials.email,
      };
      try {
        const result = await dispatch(signInThunk(payload)).unwrap();
        if (result.user) {
          try {
            await dispatch(fetchProfileThunk()).unwrap();
          } catch (profileError) {
            console.warn("Failed to load profile after login:", profileError);
          }
        }
        return { success: true, data: result };
      } catch (error: any) {
        // Trả về error để useSignInForm xử lý
        throw error;
      }
    },
    [dispatch]
  );

  const logout = useCallback(() => {
    dispatch(logoutThunk());
    navigate("/login");
  }, [dispatch, navigate]);

  const loadProfile = useCallback(async () => {
    try {
      const result = await dispatch(fetchProfileThunk()).unwrap();
      return result;
    } catch (error: any) {
      throw error;
    }
  }, [dispatch]);

  const clearAuthError = useCallback(() => {
    dispatch(clearError());
  }, [dispatch]);

  // Role checks
  const hasRole = useCallback(
    (role: UserRole): boolean => {
      return userRole === role;
    },
    [userRole]
  );

  const isInRoleGroupCb = useCallback(
    (group: keyof typeof ROLE_GROUPS): boolean => {
      return isUserInRoleGroup(userRole, group);
    },
    [userRole]
  );

  const canAccessRouteCb = useCallback(
    (route: string): boolean => {
      return canUserAccessRoute(userRole, route);
    },
    [userRole]
  );

  // Specific role checks
  const isQuanTriHeThong = useCallback(
    (): boolean => userRole === ApplicationRole.QuanTriHeThong,
    [userRole]
  );

  const isQuanLy = useCallback((): boolean => {
    return [
      ApplicationRole.QuanLyNhanSu,
      ApplicationRole.QuanLyKho,
      ApplicationRole.QuanLyPhongHat,
    ].includes(userRole as any);
  }, [userRole]);

  const isNhanVien = useCallback((): boolean => {
    return [
      ApplicationRole.NhanVienKho,
      ApplicationRole.NhanVienPhucVu,
      ApplicationRole.NhanVienTiepTan,
    ].includes(userRole as any);
  }, [userRole]);

  const isAdmin = useCallback(
    (): boolean => userRole === ApplicationRole.QuanTriHeThong,
    [userRole]
  );

  const isKhachHang = useCallback(
    (): boolean => userRole === ApplicationRole.KhachHang,
    [userRole]
  );

  // Navigation helpers
  const navigateToDefaultRoute = useCallback(() => {
    const route = getDefaultRoute(userRole);
    navigate(route);
  }, [userRole, navigate]);

  const navigateToRole = useCallback(
    (role: UserRole) => {
      const route = getDefaultRoute(role);
      navigate(route);
    },
    [navigate]
  );

  return {
    isAuthenticated,
    accessToken: auth.accessToken,
    refreshToken: auth.refreshToken,
    user,
    userRole,
    defaultRoute,
    loading: auth.loading,
    error: auth.error,

    // Core actions
    login,
    logout,
    loadProfile,
    clearError: clearAuthError,

    // Role checks
    hasRole,
    isInRoleGroup: isInRoleGroupCb,
    canAccessRoute: canAccessRouteCb,

    isQuanTriHeThong,
    isQuanLy,
    isNhanVien,
    isAdmin,

    // Navigation
    navigateToDefaultRoute,
    navigateToRole,

    // Profile info
    hasProfile: !!user?.profile,
    profileLoaded: user?.profileLoaded || false,
    userName: user?.profile?.userName || "",
    userEmail: user?.profile?.email || "",
    userPhone: user?.profile?.phone || "",
  };
};

export default useAuth;
