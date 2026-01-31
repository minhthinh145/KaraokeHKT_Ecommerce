// Sửa dùng getAllCaLamViecs thay vì API trùng lặp
import { useEffect, useState, useMemo, useRef } from "react";
import { useSelector, useDispatch } from "react-redux";
import { getAllCaLamViecs } from "../../api/services/shared/quanLyCaLamViecAPI";
import { apiGetScheduleByNhanVien } from "../../api/services/employee/employeeAPI";
import type { LichLamViecDTO } from "../../api/types/admins/QLNhanSutypes";
import { fetchNhanVienProfile } from "../../redux/nhanVien";

interface CaInfo {
    maCa: number;
    tenCaLamViec: string;
    tenCa?: string;
    gioBatDauCa: string;
    gioKetThucCa: string;
}

const startOfWeekISO = (now = new Date()) => {
    const day = now.getDay();
    const diff = now.getDate() - (day === 0 ? 6 : day - 1);
    const monday = new Date(now.setDate(diff));
    return monday.toISOString().slice(0, 10);
};

export const useEmployeeBaseData = () => {
    const dispatch = useDispatch<any>();
    const authUser = useSelector((s: any) => s.auth?.user);
    const nvProfile = useSelector((s: any) => s.nhanVien?.profile);
    const nvProfileLoading = useSelector((s: any) => s.nhanVien?.profileLoading);
    // Fallback ưu tiên: profile -> auth
    const maNhanVien: string | undefined =
        nvProfile?.maNhanVien ||
        nvProfile?.maNv ||
        authUser?.maNhanVien ||
        authUser?.maNv ||
        authUser?.id;

    const [caList, setCaList] = useState<CaInfo[]>([]);
    const [lich, setLich] = useState<LichLamViecDTO[]>([]);
    const [loading, setLoading] = useState(false);
    const [err, setErr] = useState<string | null>(null);

    const fetchedRef = useRef(false);

    useEffect(() => {
        if (!nvProfile && !nvProfileLoading && !fetchedRef.current) {
            fetchedRef.current = true;
            dispatch(fetchNhanVienProfile());
        }
    }, [nvProfile, nvProfileLoading, dispatch]);

    const load = async () => {
        if (!maNhanVien) return;
        setLoading(true);
        setErr(null);
        try {
            const [caRes, lichRes] = await Promise.all([
                getAllCaLamViecs(),
                apiGetScheduleByNhanVien(maNhanVien),
            ]);
            setCaList(
                (caRes.data || []).map((c: any) => ({
                    maCa: c.maCa,
                    tenCaLamViec: c.tenCaLamViec || c.tenCa || c.tenCaLamViec,
                    tenCa: c.tenCa,
                    gioBatDauCa: c.gioBatDauCa,
                    gioKetThucCa: c.gioKetThucCa,
                }))
            );
            setLich(lichRes.data || []);
        } catch (e: any) {
            setErr(e.message || "Lỗi tải dữ liệu");
        } finally {
            setLoading(false);
        }
    };

    // Load ca + lịch khi đã có mã
    useEffect(() => {
        if (maNhanVien) load();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [maNhanVien]);

    const caOptions = useMemo(
        () =>
            caList.map((c) => ({
                value: c.maCa,
                label: `${c.tenCaLamViec} (${c.gioBatDauCa} - ${c.gioKetThucCa})`,
            })),
        [caList]
    );

    return {
        maNhanVien,
        weekStart: startOfWeekISO(),
        caList,
        caOptions,
        lich,
        loading,
        loadingProfile: nvProfileLoading,
        error: err,
        refresh: load,
    };
};