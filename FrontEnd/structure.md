OOADProject_FrontEnd/
├── public/
│ └── LogoHKT.jpg
├── src/
│ ├── api/
│ │ ├── services/
│ │ │ ├── SignInOut.ts
│ │ │ └── validateAccount.ts
│ │ ├── axiosConfig.ts
│ │ └── types/
│ │ ├── apiResponse.ts
│ │ └── auth/
│ │ ├── AuthDTO.ts
│ │ └── VerifyAccountDTO.ts
│ ├── components/
│ │ ├── Auth/
│ │ │ ├── LoginForm.tsx
│ │ │ ├── OtpVerificationModal.tsx
│ │ │ ├── SignUpForm.tsx
│ │ │ ├── layout.tsx
│ │ │ └── AuthLayout.tsx
│ │ └── ui/
│ │ ├── Button.tsx
│ │ └── Input.tsx
│ ├── hooks/
│ │ ├── useOtpVerification.tsx
│ │ ├── useSignUpForm.ts
│ │ └── useToast.tsx
│ ├── pages/
│ │ ├── login.tsx
│ │ └── signup.tsx
│ ├── redux/
│ │ ├── store.ts
│ │ └── auth/
│ │ └── authSlice.ts
│ ├── routes/
│ │ └── AppRoutes.tsx
│ ├── App.tsx
│ └── main.tsx
├── .gitignore-template
├── structure.md
├── package.json
├── tsconfig.json
├── vite.config.ts
└── index.html

# Đặc biệt:

- Logic xử lý form đăng ký đã được tách riêng vào custom hook: `hooks/useSignUpForm.ts`
- Logic xử lý OTP đã được tách riêng vào custom hook: `hooks/useOtpVerification.tsx`
- UI/UX của form và modal OTP chỉ còn nhận state và gọi hàm từ custom hook.
- Các component UI tái sử dụng: `Input`, `Button`
- Các API service đã chuẩn hóa

Features đã implement:
├── Authentication System
│ ├── ✅ Login với email/password
│ ├── ✅ SignUp với 6 fields (email, username, phoneNumber, dateOfBirth, password, confirmPassword)
│ ├── ✅ Redux state management (loading, error, isAuthenticated)
│ ├── ✅ Token storage trong localStorage
│ ├── ✅ Auto login sau signup thành công
│ └── ✅ Toast notifications cho UX
├── Routing System
│ ├── ✅ Public routes (login, signup)
│ ├── ✅ Protected routes (dashboard - chưa implement)
│ └── ✅ Route guards (PublicRoute, ProtectedRoute)
├── UI Components
│ ├── ✅ Reusable Input component
│ ├── ✅ Reusable Button component
│ └── ✅ AuthLayout cho consistent auth pages
└── API Layer
├── ✅ Axios configuration
├── ✅ Type-safe API calls
├── ✅ Generic ApiResponse type
└── ✅ Organized DTO types

Tech Stack:
├── Frontend Framework: React 18 + TypeScript
├── Build Tool: Vite
├── State Management: Redux Toolkit
├── Routing: React Router v6
├── HTTP Client: Axios
├── Notifications: React Hot Toast
├── Styling: CSS/Tailwind (cần xác nhận)
└── Type Safety: TypeScript với strict mode

Next Steps:
├── 🔄 Implement validateAccount.ts service
├── 🔄 Add dashboard/protected pages
├── 🔄 Add logout functionality
├── 🔄 Add token refresh logic
├── 🔄 Add form validation enhancements
└── 🔄 Setup git repository ở folder cha (OOAD_Project)
