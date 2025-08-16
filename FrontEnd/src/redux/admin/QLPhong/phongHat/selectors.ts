import type { RootState } from "../../../store";

export const selectPhongHatSlice = (s: RootState) => s.qlPhong.phongHat;
export const selectPhongHatData = (s: RootState) => selectPhongHatSlice(s).data;
export const selectPhongHatLoading = (s: RootState) => selectPhongHatSlice(s).loading;
export const selectPhongHatUI = (s: RootState) => selectPhongHatSlice(s).ui;
export const selectPhongHatFiltered = (s: RootState) => {
    const { data, ui } = selectPhongHatSlice(s);
    return data.filter(d => {
        const matchSearch = !ui.searchQuery ||
            d.tenSanPham?.toLowerCase().includes(ui.searchQuery.toLowerCase()) ||
            d.tenLoaiPhong?.toLowerCase().includes(ui.searchQuery.toLowerCase());
        const matchStatus =
            ui.statusFilter === "ALL" ||
            (ui.statusFilter === "ACTIVE" && !d.ngungHoatDong) ||
            (ui.statusFilter === "INACTIVE" && d.ngungHoatDong);
        return matchSearch && matchStatus;
    });
};