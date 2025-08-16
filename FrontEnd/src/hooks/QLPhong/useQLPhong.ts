import { useQLLoaiPhong } from "./useQLLoaiPhong";
import { useQLPhongHat } from "./useQLPhongHat";

export const useQLPhong = () => {
    const loaiPhong = useQLLoaiPhong({ autoLoad: true });
    const phongHat = useQLPhongHat({ autoLoad: true });

    const refreshAll = () => {
        loaiPhong.refresh();
        phongHat.refresh();
    };

    return { loaiPhong, phongHat, refreshAll };
};