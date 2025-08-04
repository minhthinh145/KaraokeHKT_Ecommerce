OOADProject_FrontEnd/
├── public/
│ └── LogoHKT.jpg
├── src/
│ ├── components/
│ │ ├── Auth/
│ │ │ ├── LoginForm.tsx // Form đăng nhập với validation + toast
│ │ │ ├── AuthLayout.tsx // Layout chung cho auth pages (logo + form)
│ │ │ ├── PublicRoute.tsx // Route cho user chưa đăng nhập
│ │ │ └── ProtectedRoute.tsx // Route cho user đã đăng nhập
│ │ ├── common/
│ │ │ └── ToastProvider.tsx // Provider cho react-hot-toast
│ │ └── ui/
│ │ ├── Input.tsx // Component input tái sử dụng
│ │ └── Button.tsx // Component button tái sử dụng
│ ├── routes/
│ │ └── AppRoutes.tsx // Tất cả routing logic (public + protected)
│ ├── pages/
│ │ └── login.tsx // Login page sử dụng AuthLayout + LoginForm
│ ├── redux/
│ │ ├── store.ts // Redux store config
│ │ └── auth/
│ │ └── authSlice.ts // Auth state management + signInThunk
│ ├── hooks/
│ │ └── useToast.tsx // Custom hook cho toast notifications
│ ├── api/
│ │ ├── index.ts // API functions
│ │ └── types/
│ │ └── auth/
│ │ └── AuthDTO.ts // Type definitions
│ └── App.tsx // Main app với providers
