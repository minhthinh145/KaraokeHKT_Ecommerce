import { combineReducers } from "@reduxjs/toolkit";
import { customerBookingReducer } from "./booking";

export const customerReducer = combineReducers({
    booking: customerBookingReducer,
});

export type CustomerDomainState = ReturnType<typeof customerReducer>;