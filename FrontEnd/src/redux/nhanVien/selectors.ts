import type { RootState } from "../store";

export const selectNVRoot = (s: RootState) => s.nhanVien;

export const selectNVScheduleState = (s: RootState) => s.nhanVien.schedule;
export const selectNVShiftChangeState = (s: RootState) => s.nhanVien.shiftChanges;

export const selectNVSchedules = (s: RootState) => s.nhanVien.schedule.data;
export const selectNVShiftChanges = (s: RootState) => s.nhanVien.shiftChanges.data;

export const selectNVProfile = (s: RootState) => s.nhanVien.profile;