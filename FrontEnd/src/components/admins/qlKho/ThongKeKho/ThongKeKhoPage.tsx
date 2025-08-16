import React, { useEffect, useCallback } from "react";
import { useVatLieu } from "../../../../hooks/QLKho/useVatLieu";
import { InventorySummaryTable } from "../../uiForAll/QLKho/InventorySummaryTable";
import * as XLSX from "xlsx";
import { saveAs } from "file-saver";

export const ThongKeKhoPage: React.FC = () => {
  const { data, load, loading } = useVatLieu();

  useEffect(() => {
    load();
  }, [load]);

  const handleExport = useCallback(() => {
    // Chỉ lấy tên vật tư và số lượng
    const exportData = data.map((d) => ({
      "Tên vật tư": d.tenVatLieu,
      "Số lượng tồn kho": d.soLuongTonKho ?? 0,
    }));

    const ws = XLSX.utils.json_to_sheet(exportData, {
      header: ["Tên vật tư", "Số lượng tồn kho"],
    });

    // Style: header bold, cột rộng
    ws["!cols"] = [{ wch: 30 }, { wch: 18 }];
    ws["A1"].s = { font: { bold: true } };
    ws["B1"].s = { font: { bold: true } };

    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Thống kê kho");

    // Xuất file
    const wbout = XLSX.write(wb, { bookType: "xlsx", type: "array" });
    saveAs(
      new Blob([wbout], { type: "application/octet-stream" }),
      `thong_ke_kho_${new Date().toISOString().slice(0, 10)}.xlsx`
    );
  }, [data]);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-900">
            Thống kê kho hàng
          </h1>
          <p className="mt-1 text-sm text-slate-600">
            Danh sách vật tư và số lượng tồn hiện tại
          </p>
        </div>
        <button
          onClick={load}
          className="inline-flex items-center gap-2 px-4 py-2 rounded-lg text-sm font-medium bg-indigo-600 text-white hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
        >
          <svg
            className="w-4 h-4"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M4 4v5h.582M20 20v-5h-.581M5.5 9A7.5 7.5 0 0119 9M5 15a7.5 7.5 0 0013.5 0"
            />
          </svg>
          Làm mới
        </button>
      </div>

      <div className="flex justify-center items-center min-h-[60vh]">
        <InventorySummaryTable
          data={data}
          loading={loading}
          onExport={handleExport}
          title="Bảng thống kê vật tư"
        />
      </div>
    </div>
  );
};
