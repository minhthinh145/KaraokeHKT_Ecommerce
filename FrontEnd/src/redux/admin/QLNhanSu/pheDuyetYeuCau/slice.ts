import { createSlice } from "@reduxjs/toolkit";
import { localReducers } from "./reducers";
import { buildExtraReducers } from "./extraReducers";
import {
    pheDuyetYeuCauInitialState,
    type PheDuyetYeuCauChuyenCaSliceState,
} from "../types";

const slice = createSlice({
    name: "pheDuyetYeuCau",
    initialState: pheDuyetYeuCauInitialState as PheDuyetYeuCauChuyenCaSliceState,
    reducers: localReducers,
    extraReducers: (b) => buildExtraReducers(b),
});

export const {
    setSearchYeuCau,
    setStatusFilterYeuCau,
    clearPheDuyetYeuCauError,
    resetPheDuyetYeuCauState,
} = slice.actions;

export default slice.reducer;