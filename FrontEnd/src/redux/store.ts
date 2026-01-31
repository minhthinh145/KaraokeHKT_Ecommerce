import { configureStore } from "@reduxjs/toolkit";
import { qlHeThongReducer } from "./admin/QLHeThong";
import { qlNhanSuReducer } from "./admin/QLNhanSu"; // 🔥 Add QLNhanSu
import authReducer from "./auth/authSlice";
import { combineReducers } from "@reduxjs/toolkit";
import { vatLieuReducer } from "./admin/QLKho";
import { qlPhongReducer } from "./admin/QLPhong";
import { nhanVienReducer } from "./nhanVien";
import { customerReducer } from "./customer"; // ⬅️ ADD

// 🏪 Configure Store
export const store = configureStore({
  reducer: {
    auth: authReducer, // Existing auth state
    qlHeThong: qlHeThongReducer, // New QLHeThong state
    qlNhanSu: qlNhanSuReducer, // 🔥 Add to store
    qlKho: combineReducers({
      vatLieu: vatLieuReducer,
    }),
    qlPhong: qlPhongReducer, // domain QLPhongS
    nhanVien: nhanVienReducer, // ⬅️ thêm domain cho employee self-service
    customer: customerReducer, // ⬅️ ADD domain customer
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
