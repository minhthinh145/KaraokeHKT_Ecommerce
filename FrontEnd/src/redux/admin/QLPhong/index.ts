import { combineReducers } from "@reduxjs/toolkit";
import loaiPhongReducer from "./loaiPhong/slice";
import phongHatReducer from "./phongHat/slice";

export const qlPhongReducer = combineReducers({
    loaiPhong: loaiPhongReducer,
    phongHat: phongHatReducer,
});

export * from "./types";
export * from "./loaiPhong/slice";
export * from "./loaiPhong/thunks";
export * from "./loaiPhong/selectors";
export * from "./phongHat/slice";
export * from "./phongHat/thunks";
export * from "./phongHat/selectors";