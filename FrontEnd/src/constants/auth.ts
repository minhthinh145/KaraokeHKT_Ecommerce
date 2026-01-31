import { ApplicationRole } from "../api/types/admins/QLHeThongtypes";

// 🔥 SỬA: Định nghĩa UserRole đúng cách
export type UserRole = keyof typeof ApplicationRole;

// 🔥 ROLE GROUPS - Phân nhóm rõ ràng
export const ROLE_GROUPS = {
  ADMIN: [ApplicationRole.QuanTriHeThong],
  MANAGER: [
    ApplicationRole.QuanLyNhanSu,
    ApplicationRole.QuanLyKho,
    ApplicationRole.QuanLyPhongHat,
  ],
  EMPLOYEE: [
    ApplicationRole.NhanVienKho,
    ApplicationRole.NhanVienPhucVu,
    ApplicationRole.NhanVienTiepTan,
  ],
  CUSTOMER: [ApplicationRole.KhachHang],
} as const;

// 🔥 ROUTE MAPPING theo role - SỬA type
export const ROLE_ROUTES: Record<string, string> = {
  [ApplicationRole.QuanTriHeThong]: "/admin/quan-tri-he-thong",
  [ApplicationRole.QuanLyNhanSu]: "/admin/quan-ly-nhan-su",
  [ApplicationRole.QuanLyKho]: "/admin/quan-ly-kho",
  [ApplicationRole.QuanLyPhongHat]: "/admin/quan-ly-phong-hat",
  [ApplicationRole.NhanVienKho]: "/employee/nhan-vien-kho",
  [ApplicationRole.NhanVienPhucVu]: "/employee/nhan-vien-phuc-vu",
  [ApplicationRole.NhanVienTiepTan]: "/employee/nhan-vien-tiep-tan",
  [ApplicationRole.KhachHang]: "/customer/dashboard",
} as const;

// 🔥 ROUTE PERMISSIONS
export const ROUTE_PERMISSIONS: Record<string, string[]> = {
  // Admin & Manager routes
  "/admin/quan-tri-he-thong": [ApplicationRole.QuanTriHeThong],
  "/admin/quan-ly-nhan-su": [
    ApplicationRole.QuanTriHeThong,
    ApplicationRole.QuanLyNhanSu,
  ],
  "/admin/quan-ly-kho": [
    ApplicationRole.QuanTriHeThong,
    ApplicationRole.QuanLyKho,
  ],
  "/admin/quan-ly-phong-hat": [
    ApplicationRole.QuanTriHeThong,
    ApplicationRole.QuanLyPhongHat,
  ],

  // Employee routes
  "/employee/nhan-vien-kho": [
    ApplicationRole.QuanTriHeThong,
    ApplicationRole.QuanLyKho,
    ApplicationRole.NhanVienKho,
  ],
  "/employee/nhan-vien-phuc-vu": [
    ApplicationRole.QuanTriHeThong,
    ApplicationRole.QuanLyPhongHat,
    ApplicationRole.NhanVienPhucVu,
  ],
  "/employee/nhan-vien-tiep-tan": [
    ApplicationRole.QuanTriHeThong,
    ApplicationRole.QuanLyPhongHat,
    ApplicationRole.NhanVienTiepTan,
  ],

  // Customer routes
  "/customer/dashboard": [ApplicationRole.KhachHang],
  "/customer/profile": [ApplicationRole.KhachHang],
  "/customer/booking": [ApplicationRole.KhachHang],
} as const;

// 🔥 Helper functions - SỬA type
export const getDefaultRoute = (role: string | null | undefined): string => {
  if (!role) return "/login";
  return ROLE_ROUTES[role] || "/login";
};

export const isUserInRoleGroup = (
  userRole: string | null | undefined,
  group: keyof typeof ROLE_GROUPS
): boolean => {
  if (!userRole) return false;
  // Không cần ép kiểu, includes hoạt động với readonly array
  return (ROLE_GROUPS[group] as unknown as string[]).includes(
    userRole as string
  );
};
export const canUserAccessRoute = (
  userRole: string | null | undefined,
  route: string
): boolean => {
  if (!userRole) return false;
  const allowedRoles = ROUTE_PERMISSIONS[route];
  if (!allowedRoles) return true;
  return allowedRoles.includes(userRole);
};

// 🔥 Navigation items - SỬA type
export const getNavigationItems = (userRole: string | null | undefined) => {
  if (!userRole) return [];

  const baseItems = [
    {
      label: "Dashboard",
      route: getDefaultRoute(userRole),
      icon: "🏠",
    },
  ];

  // Admin navigation
  if (userRole === ApplicationRole.QuanTriHeThong) {
    return [
      ...baseItems,
      {
        label: "Quản trị hệ thống",
        route: "/admin/quan-tri-he-thong",
        icon: "⚙️",
      },
      { label: "Quản lý nhân sự", route: "/admin/quan-ly-nhan-su", icon: "👥" },
      { label: "Quản lý kho", route: "/admin/quan-ly-kho", icon: "📦" },
      {
        label: "Quản lý phòng hát",
        route: "/admin/quan-ly-phong-hat",
        icon: "🎤",
      },
    ];
  }

  // 🔥 SỬA: Manager items - dùng string type
  const managerItems: Record<string, any[]> = {
    [ApplicationRole.QuanLyNhanSu]: [
      { label: "Quản lý nhân sự", route: "/admin/quan-ly-nhan-su", icon: "👥" },
    ],
    [ApplicationRole.QuanLyKho]: [
      { label: "Quản lý kho", route: "/admin/quan-ly-kho", icon: "📦" },
    ],
    [ApplicationRole.QuanLyPhongHat]: [
      {
        label: "Quản lý phòng hát",
        route: "/admin/quan-ly-phong-hat",
        icon: "🎤",
      },
    ],
    [ApplicationRole.NhanVienKho]: [
      { label: "Kho hàng", route: "/employee/nhan-vien-kho", icon: "📦" },
    ],
    [ApplicationRole.NhanVienPhucVu]: [
      { label: "Phục vụ", route: "/employee/nhan-vien-phuc-vu", icon: "🍻" },
    ],
    [ApplicationRole.NhanVienTiepTan]: [
      { label: "Tiếp tân", route: "/employee/nhan-vien-tiep-tan", icon: "🎫" },
    ],
    [ApplicationRole.KhachHang]: [
      { label: "Thông tin cá nhân", route: "/customer/profile", icon: "👤" },
      { label: "Đặt phòng", route: "/customer/booking", icon: "🎵" },
    ],
  };

  return [...baseItems, ...(managerItems[userRole] || [])];
};

// 🔥 Role descriptions - SỬA type
export const RoleDescriptions: Record<string, string> = {
  [ApplicationRole.QuanLyKho]: "Quản lý kho",
  [ApplicationRole.QuanLyNhanSu]: "Quản lý nhân sự",
  [ApplicationRole.QuanLyPhongHat]: "Quản lý phòng hát",
  [ApplicationRole.NhanVienKho]: "Nhân viên kho",
  [ApplicationRole.NhanVienPhucVu]: "Nhân viên phục vụ",
  [ApplicationRole.NhanVienTiepTan]: "Nhân viên tiếp tân",
  [ApplicationRole.QuanTriHeThong]: "Quản trị hệ thống",
  [ApplicationRole.KhachHang]: "Khách hàng",
};
export const EMPLOYEE_ROLES = [
  ApplicationRole.NhanVienKho,
  ApplicationRole.NhanVienPhucVu,
  ApplicationRole.NhanVienTiepTan,
] as const;

export const MANAGER_ROLES = [
  ApplicationRole.QuanLyKho,
  ApplicationRole.QuanLyNhanSu,
  ApplicationRole.QuanLyPhongHat,
] as const;

export const ROLE_OPTIONS_ADMIN = [
  { value: "QuanTriHeThong", label: "Quản trị hệ thống" },
  { value: "QuanLyKho", label: "Quản lý kho vật liệu" },
  { value: "QuanLyNhanSu", label: "Quản lý nhân sự" },
  { value: "QuanLyPhongHat", label: "Quản lý phòng hát" },
];
