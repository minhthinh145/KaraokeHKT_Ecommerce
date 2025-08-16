import React, { useMemo } from "react";
import { Spin } from "antd";

export interface InventoryRow {
  maVatLieu: number;
  tenVatLieu: string;
  hinhAnhSanPham?: string | null;
  soLuongTonKho?: number | null;
}

interface InventorySummaryTableProps {
  data: InventoryRow[];
  loading: boolean;
  title?: string;
  onExport?: () => void;
}

export const InventorySummaryTable: React.FC<InventorySummaryTableProps> = ({
  data,
  loading,
  title = "Thống kê kho hàng",
  onExport,
}) => {
  const rows = useMemo(() => data, [data]);

  return (
    <div className="bg-white border border-slate-200 rounded-xl shadow-sm overflow-hidden w-full max-w-3xl">
      <div className="flex items-center justify-between px-6 py-4 border-b border-slate-200 bg-slate-50">
        <h2 className="text-2xl font-bold text-slate-800">{title}</h2>
        <button
          onClick={onExport}
          className="inline-flex items-center gap-2 px-4 py-2 text-base font-semibold text-emerald-700 bg-emerald-50 border border-emerald-200 rounded-lg hover:bg-emerald-100 hover:border-emerald-300 focus:outline-none focus:ring-2 focus:ring-offset-1 focus:ring-emerald-500 transition"
        >
          <svg
            className="w-5 h-5"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M4 16v2a2 2 0 002 2h12a2 2 0 002-2v-2M7 10l5 5m0 0l5-5m-5 5V4"
            />
          </svg>
          Export Excel
        </button>
      </div>

      <div className="overflow-x-auto">
        <table className="min-w-full text-lg">
          <thead>
            <tr className="bg-slate-100 text-slate-700">
              <th className="py-4 px-4 font-bold text-center w-40">Hình ảnh</th>
              <th className="py-4 px-4 font-bold text-left">Vật tư</th>
              <th className="py-4 px-4 font-bold text-center w-40">Số lượng</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={3} className="py-12">
                  <div className="flex justify-center">
                    <Spin size="large" />
                  </div>
                </td>
              </tr>
            ) : rows.length === 0 ? (
              <tr>
                <td
                  colSpan={3}
                  className="py-10 text-center text-slate-400 text-xl"
                >
                  Không có dữ liệu
                </td>
              </tr>
            ) : (
              rows.map((r, idx) => (
                <tr
                  key={r.maVatLieu}
                  className={`${
                    idx % 2 === 0 ? "bg-white" : "bg-slate-50"
                  } hover:bg-indigo-50 transition`}
                >
                  <td className="py-4 px-4">
                    <div className="flex justify-center">
                      {r.hinhAnhSanPham ? (
                        <img
                          src={r.hinhAnhSanPham}
                          alt={r.tenVatLieu}
                          className="w-24 h-24 object-cover rounded-xl border border-slate-200 bg-slate-100"
                          onError={(e) => (
                            (e.currentTarget.style.visibility = "hidden"),
                            (e.currentTarget.style.height = "0px")
                          )}
                        />
                      ) : (
                        <span className="text-lg text-slate-400">—</span>
                      )}
                    </div>
                  </td>
                  <td className="py-4 px-4">
                    <span className="font-bold text-slate-900 text-xl">
                      {r.tenVatLieu}
                    </span>
                  </td>
                  <td className="py-4 px-4 text-center">
                    <span className="inline-block min-w-[80px] px-3 py-2 rounded-lg bg-slate-100 text-slate-800 font-bold text-xl">
                      {r.soLuongTonKho ?? 0}
                    </span>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};
