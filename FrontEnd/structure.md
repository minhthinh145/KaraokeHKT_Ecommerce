# OOAD Project Frontend Structure

## Project Structure

```
OOADProject_FrontEnd/
├── public/
│   └── LogoHKT.jpg
├── src/
│   ├── api/
│   │   ├── services/
│   │   │   ├── auth/
│   │   │   │   ├── SignInOut.ts
│   │   │   │   ├── getProfile.ts
│   │   │   │   ├── updateProfile.ts
│   │   │   │   └── index.ts
│   │   │   └── validateAccount.ts
│   │   ├── axiosConfig.ts
│   │   └── types/
│   │       ├── apiResponse.ts
│   │       └── auth/
│   │           ├── AuthDTO.ts
│   │           ├── UserProfileDTO.ts
│   │           └── VerifyAccountDTO.ts
│   ├── components/
│   │   ├── Auth/
│   │   │   ├── LoginForm.tsx
│   │   │   ├── OtpVerificationModal.tsx
│   │   │   ├── SignUpForm.tsx
│   │   │   ├── layout.tsx
│   │   │   └── AuthLayout.tsx
│   │   ├── customer/
│   │   │   ├── Layout/
│   │   │   │   ├── Navigation.tsx
│   │   │   │   └── HeroSection.tsx
│   │   │   ├── Main/
│   │   │   │   ├── MainLayOut.tsx
│   │   │   │   └── NoFooterLayout.tsx
│   │   │   └── userProfile/
│   │   │       ├── ProfilePage.tsx
│   │   │       ├── ProfileHeader.tsx
│   │   │       ├── ProfileForm.tsx
│   │   │       └── ProfileStats.tsx
│   │   └── ui/
│   │       ├── Button.tsx
│   │       └── Input.tsx
│   ├── hooks/
│   │   ├── useOtpVerification.tsx
│   │   ├── useSignUpForm.ts
│   │   └── useToast.tsx
│   ├── pages/
│   │   ├── login.tsx
│   │   ├── signup.tsx
│   │   ├── HomePage.tsx
│   │   └── ProfilePage.tsx
│   ├── redux/
│   │   ├── store.ts
│   │   └── auth/
│   │       ├── authSlice.ts
│   │       ├── types.ts
│   │       ├── utils.ts
│   │       ├── thunks.ts
│   │       ├── selectors.ts
│   │       └── index.ts
│   ├── routes/
│   │   └── AppRoutes.tsx
│   ├── App.tsx
│   └── main.tsx
├── .gitignore-template
├── structure.md
├── package.json
├── tsconfig.json
├── vite.config.ts
└── index.html
```

## Architecture Highlights

### 🏗️ Modular Redux Structure

- **Separated concerns**: types, utils, thunks, selectors, slice
- **Type-safe**: Full TypeScript support với strict typing
- **Reusable selectors**: Computed values và conditional logic
- **Safe localStorage**: Error handling cho storage operations

### 🔧 API Layer

- **Axios interceptors**: Auto token refresh và request/response handling
- **Type-safe services**: Organized by feature (auth/, profile/, etc.)
- **Generic responses**: Consistent ApiResponse type
- **Error handling**: Comprehensive error management

### 🎨 Component Architecture

- **Layout system**: MainLayout, NoFooterLayout cho different page types
- **Feature components**: Organized by domain (Auth/, customer/, ui/)
- **Reusable UI**: Button, Input components với consistent styling
- **Profile system**: Complete user profile management

## Features Implemented

### ✅ Authentication System

- Login với email/password
- SignUp với validation (email, username, phone, dateOfBirth, password)
- Redux state management (loading, error, isAuthenticated)
- Token storage và management
- Auto login sau signup thành công
- Token refresh logic với interceptors
- Logout functionality

### ✅ User Profile System

- **Profile viewing**: Display user information
- **Profile editing**: Update user data (userName, phone, birthDate)
- **Profile completion**: Track missing fields và completion percentage
- **Form validation**: Real-time validation cho profile fields
- **Redux integration**: Seamless state management

### ✅ Routing System

- Public routes (login, signup, home)
- Protected routes (profile, dashboard)
- Route guards (PublicRoute, ProtectedRoute)
- Navigation component với active states

### ✅ UI/UX Components

- **Layouts**: MainLayout (với header/footer), NoFooterLayout
- **Profile components**: ProfileHeader, ProfileForm, ProfileStats
- **Navigation**: Responsive navigation với routing
- **Reusable UI**: Input, Button components
- **AuthLayout**: Consistent auth pages styling

### ✅ State Management

- **Modular Redux**: Separated auth logic
- **Type safety**: Full TypeScript support
- **Selectors**: Computed values (displayName, initials, completion %)
- **Utils**: Safe localStorage operations
- **Thunks**: Async operations (signin, signup, profile CRUD)

### ✅ API Integration

- **Axios configuration**: Base URL, interceptors
- **Token management**: Auto-attach tokens, refresh on 401
- **Service organization**: auth/, profile/ services
- **Type-safe calls**: DTO types cho request/response
- **Error handling**: Consistent error responses

## Tech Stack

```
├── Frontend Framework: React 18 + TypeScript
├── Build Tool: Vite
├── State Management: Redux Toolkit
├── Routing: React Router v6
├── HTTP Client: Axios với interceptors
├── Notifications: React Hot Toast
├── Styling: Tailwind CSS
└── Type Safety: TypeScript với strict mode
```

## Redux Auth Module Structure

```
src/redux/auth/
├── index.ts          # Barrel exports
├── types.ts          # AuthState, response types
├── utils.ts          # localStorage helpers, validation
├── thunks.ts         # Async operations (signin, signup, profile)
├── selectors.ts      # Computed values và conditional logic
└── authSlice.ts      # Slice definition với reducers
```

## API Services Structure

```
src/api/services/
├── auth/
│   ├── SignInOut.ts      # Login/logout operations
│   ├── getProfile.ts     # Fetch user profile
│   ├── updateProfile.ts  # Update user profile
│   └── index.ts          # Barrel exports
└── validateAccount.ts    # Account validation
```

## Profile Management Features

### ✅ User Profile

- **Display**: Show user information (userName, email, phone, birthDate)
- **Edit mode**: Toggle edit/view modes
- **Validation**: Real-time field validation
- **Auto-save**: Update Redux state immediately
- **API sync**: Sync với backend on save
- **Loading states**: Handle loading/error states

### ✅ Profile Selectors

- `selectUserDisplayName`: Formatted display name
- `selectUserInitials`: User initials cho avatar
- `selectProfileCompletionPercentage`: Profile completion %
- `selectMissingProfileFields`: Fields còn thiếu
- `selectIsValidEmail/Phone`: Validation selectors
- `selectFormattedBirthDate`: Formatted dates

## Next Steps & Roadmap

### 🔄 In Progress

- Dashboard/home page enhancements
- Profile image upload
- Advanced form validation
- Email verification flow

### 📋 Planned Features

- **Booking System**: Room booking functionality
- **Service Management**: Hotel services
- **Admin Panel**: Management interface
- **Reports**: Analytics và reporting
- **Notifications**: Real-time notifications

### 🛠️ Technical Improvements

- Unit testing setup (Jest + React Testing Library)
- E2E testing (Playwright/Cypress)
- Performance optimization
- Accessibility improvements
- PWA capabilities

## Development Notes

### 🎯 Code Quality

- **Separation of concerns**: Each file has single responsibility
- **Type safety**: Comprehensive TypeScript usage
- **Reusability**: Modular components và hooks
- **Error handling**: Comprehensive error management
- **Performance**: Optimized re-renders với proper selectors

### 🔧 Development Workflow

- **Hot reload**: Vite dev server
- **Type checking**: Real-time TypeScript validation
- **State debugging**: Redux DevTools integration
- **API testing**: Axios request/response logging
- **Toast feedback**: User-friendly error messages

### 📝 Conventions

- **File naming**: PascalCase cho components, camelCase cho utilities
- **Import organization**: External → Internal → Relative
- **Component structure**: Props interface → Component → Export
- **Redux pattern**: Thunks → Slice → Selectors → Components
