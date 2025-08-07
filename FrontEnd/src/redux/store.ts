import { configureStore } from "@reduxjs/toolkit";
import { qlHeThongReducer } from "./admin/QLHeThong";
import { qlNhanSuReducer } from "./admin/QLNhanSu"; // 🔥 Add QLNhanSu
import authReducer from "./auth/authSlice";

// 🏪 Configure Store
export const store = configureStore({
  reducer: {
    auth: authReducer, // Existing auth state
    qlHeThong: qlHeThongReducer, // New QLHeThong state
    qlNhanSu: qlNhanSuReducer, // 🔥 Add to store
    // Add other reducers here...
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: {
        ignoredActions: ["persist/PERSIST"],
      },
    }),
});

// 🎯 Export types
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;
