# OOAD Project Frontend Structure

## Project Structure

```
POOADProject_FrontEnd/
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
│   │   │   ├── changePassword.ts
│   │   │   └── validateAccount.ts
│   │   ├── axiosConfig.ts
│   │   └── types/
│   │       ├── apiResponse.ts
│   │       └── auth/
│   │           ├── AuthDTO.ts
│   │           ├── UserProfileDTO.ts
│   │           ├── VerifyAccountDTO.ts
│   │           └── ChangePassword.ts
│   ├── components/
│   │   ├── Auth/
│   │   │   ├── LoginForm.tsx
│   │   │   ├── SignUpForm.tsx
│   │   │   ├── layout.tsx
│   │   │   ├── AuthLayout.tsx
│   │   │   └── OtpVerification/
│   │   │       ├── LoginOtpModal.tsx
│   │   │       └── OtpVerificationModal.tsx
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
│   │   │       ├── rofileForm.tsx
│   │   │       └── ProfileStats.tsx
│   │   └── ui/
│   │       ├── Button.tsx
│   │       └── Input.tsx
│   ├── hooks/
│   │   ├── useOtpVerification.tsx
│   │   ├── useSignUpForm.ts
│   │   ├── useSignInForm.ts
│   │   ├── useChangePassword.ts
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
│   │   ├── AppRoutes.tsx
│   │   ├── PublicRoute.tsx
│   │   └── ProtectedRoute.tsx
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
- **Full response objects**: Pass complete backend responses to frontend

### 🎨 Component Architecture

- **Layout system**: MainLayout, NoFooterLayout cho different page types
- **Feature components**: Organized by domain (Auth/, customer/, ui/)
- **Reusable UI**: Button, Input components với consistent styling
- **Profile system**: Complete user profile management
- **OTP system**: Modular OTP verification components

## Features Implemented

### ✅ Authentication System

- **Login**: Email/password với smart error handling
- **SignUp**: Full validation (email, username, phone, dateOfBirth, password)
- **Account Activation**: OTP verification flow cho chưa active accounts
- **Account Recovery**: Password change với OTP verification
- **Redux state management**: loading, error, isAuthenticated states
- **Token storage**: Secure localStorage management
- **Route protection**: PublicRoute và ProtectedRoute guards
- **Auto-login**: Seamless login after successful verification

### ✅ Smart Error Handling

- **Account not active detection**: Detect `data: false` from backend
- **User-friendly messages**: "Email hoặc mật khẩu không đúng" cho all login errors
- **System error logging**: Console errors without exposing to users
- **Toast notifications**: Contextual success/error messages
- **Modal confirmations**: Account activation confirmation dialogs

### ✅ OTP Verification System

- **Unified OTP components**: Reusable OTP modal cho multiple flows
- **Smart OTP handling**: Auto-detection của OTP needs
- **Resend functionality**: Countdown timer và resend logic
- **Multiple flows**: SignUp verification, Account activation, Password change
- **Error recovery**: Proper error handling cho OTP failures

### ✅ User Profile System

- **Profile viewing**: Display user information
- **Profile editing**: Update user data (userName, phone, birthDate)
- **Profile completion**: Track missing fields và completion percentage
- **Form validation**: Real-time validation cho profile fields
- **Redux integration**: Seamless state management

### ✅ Routing System

- **Public routes**: login, signup, home (không redirect khi loading)
- **Protected routes**: profile, dashboard
- **Route guards**: Smart routing based on auth state
- **Navigation component**: Active states và responsive design
- **Form persistence**: Maintain form data across re-renders

### ✅ UI/UX Components

- **Layouts**: MainLayout (với header/footer), NoFooterLayout
- **Profile components**: ProfileHeader, ProfileForm, ProfileStats
- **Navigation**: Responsive navigation với routing
- **Reusable UI**: Input, Button components with loading states
- **AuthLayout**: Consistent auth pages styling
- **Modal system**: Confirmation dialogs và OTP modals

### ✅ State Management

- **Modular Redux**: Separated auth logic
- **Type safety**: Full TypeScript support
- **Selectors**: Computed values (displayName, initials, completion %)
- **Utils**: Safe localStorage operations
- **Thunks**: Async operations với full error response handling
- **Clean signup flow**: Proper signup without premature authentication

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
├── thunks.ts         # Async operations với full response handling
├── selectors.ts      # Computed values và conditional logic
└── authSlice.ts      # Slice definition với proper signup flow
```

## API Services Structure

```
src/api/services/
├── auth/
│   ├── SignInOut.ts      # Login/logout operations
│   ├── getProfile.ts     # Fetch user profile
│   ├── updateProfile.ts  # Update user profile
│   └── index.ts          # Barrel exports
├── changePassword.ts     # Password change operations
└── validateAccount.ts    # Account validation và OTP
```

## Authentication Flow Details

### 🔐 Login Flow

1. **User submits credentials** → [`signInThunk`](src/redux/auth/thunks.ts )
2. **Backend response detection**:
   - Success → Login + redirect
   - `data: false` → Show activation modal
   - Other errors → "Email hoặc mật khẩu không đúng"
3. **Activation confirmation** → Show OTP modal
4. **OTP verification** → Account activated → Return to login

### 🔐 SignUp Flow

1. **User submits form** → Validation
2. **SignUp success** → Show OTP modal (no premature auth)
3. **OTP verification** → Account activated
4. **Redirect to login** → User can now login

### 🔐 Password Change Flow

1. **User enters current + new password** → [`requestChangePassword`](src/hooks/useChangePassword.ts )
2. **OTP sent** → Show OTP modal
3. **OTP verification** → [`confirmChangePassword`](src/hooks/useChangePassword.ts )
4. **Success** → Password changed

## Hooks Architecture

### 🎣 Specialized Hooks

- **useSignInForm**: Login logic với activation detection
- **useSignUpForm**: Signup với OTP integration
- **useChangePassword**: Password change workflow
- **useOtpVerification**: Reusable OTP logic
- **useToast**: Centralized notification system

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

## Development Improvements

### 🚀 Recent Enhancements

- **Smart error handling**: Backend response parsing
- **OTP system**: Unified verification flow
- **Route protection**: Proper loading state handling
- **Form persistence**: No data loss during re-renders
- **Clean separation**: SignIn vs SignUp vs OTP flows
- **Type safety**: Full backend response typing

### 🛠️ Code Quality Improvements

- **Consistent error handling**: Standardized across all auth flows
- **Reusable components**: OTP modal used across multiple features
- **Clean state management**: Proper Redux patterns
- **User-friendly UX**: No confusing error messages
- **Production-ready**: Proper error logging và user feedback

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
- **Production-ready**: Clean, maintainable code

### 🔧 Development Workflow

- **Hot reload**: Vite dev server
- **Type checking**: Real-time TypeScript validation
- **State debugging**: Redux DevTools integration
- **API testing**: Axios request/response logging
- **Toast feedback**: User-friendly error messages
- **Form debugging**: State persistence across re-renders

### 📝 Conventions

- **File naming**: PascalCase cho components, camelCase cho utilities
- **Import organization**: External → Internal → Relative
- **Component structure**: Props interface → Component → Export
- **Redux pattern**: Thunks → Slice → Selectors → Components
- **Error handling**: Consistent patterns across all features
- **Hook naming**: Descriptive names cho specific use cases