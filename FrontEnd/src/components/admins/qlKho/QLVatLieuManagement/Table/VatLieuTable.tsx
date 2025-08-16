import React, { useMemo } from "react";
import { GenericQLTable } from "../../../uiForAll/GenericQLTable";
import { QLActionButton } from "../../../uiForAll/QLActionButton";
import type { VatLieuDetailDTO } from "../../../../../api";

interface Props {
  data: VatLieuDetailDTO[];
  loading: boolean;
  onUpdate: (row: VatLieuDetailDTO) => void;
  onToggleNgungCungCap: (row: VatLieuDetailDTO, next: boolean) => void;
  // XÓA onOpenUpdateSL
}

const formatMoney = (n?: number | null) => {
  if (n == null) return "-";
  return n.toLocaleString("vi-VN") + " ₫";
};

export const VatLieuTable: React.FC<Props> = ({
  data,
  loading,
  onUpdate,
  onToggleNgungCungCap,
}) => {
  const columns = useMemo(
    () => [
      {
        key: "hinhAnhSanPham",
        title: "Hình ảnh",
        dataIndex: "hinhAnhSanPham",
        width: 90,
        align: "center" as const,
        className: "text-center",
        render: (url: string | null) => (
          <div className="flex items-center justify-center">
            {url ? (
              <img
                src={url}
                alt="Ảnh"
                className="w-12 h-12 object-cover rounded border border-slate-200 bg-slate-50"
                onError={(e) => (e.currentTarget.style.display = "none")}
              />
            ) : (
              <span className="text-gray-400 text-xs">Không có</span>
            )}
          </div>
        ),
      },
      {
        key: "tenVatLieu",
        title: <span className="font-semibold text-slate-800">Vật tư</span>,
        dataIndex: "tenVatLieu",
        width: 230,
        className: "font-semibold",
        render: (v: string) => (
          <span className="font-semibold text-slate-800">{v}</span>
        ),
      },
      {
        key: "donViTinh",
        title: "ĐVT",
        dataIndex: "donViTinh",
        width: 80,
        align: "center" as const,
        className: "text-center font-medium",
      },
      {
        key: "giaNhapHienTai",
        title: (
          <div className="w-full text-center leading-tight">
            <span className="font-medium text-slate-700">GIÁ NHẬP</span>
            <br />
            <span className="text-[10px] text-green-600">(VND)</span>
          </div>
        ),
        dataIndex: "giaNhapHienTai",
        width: 140,
        align: "center" as const,
        className: "text-center",
        render: (v: number | null) =>
          v == null ? (
            <span className="text-gray-400">-</span>
          ) : (
            <span className="inline-block min-w-[92px] text-center px-2 py-1 rounded-md text-xs font-semibold bg-green-50 text-green-700 border border-green-200">
              {v.toLocaleString("vi-VN")} ₫
            </span>
          ),
      },
      {
        key: "giaBanHienTai",
        title: (
          <div className="w-full text-center leading-tight">
            <span className="font-medium text-slate-700">GIÁ BÁN</span>
            <br />
            <span className="text-[10px] text-amber-600">(VND)</span>
          </div>
        ),
        dataIndex: "giaBanHienTai",
        width: 140,
        align: "center" as const,
        className: "text-center",
        render: (v: number | null) =>
          v == null ? (
            <span className="text-gray-400">-</span>
          ) : (
            <span className="inline-block min-w-[92px] text-center px-2 py-1 rounded-md text-xs font-semibold bg-amber-50 text-amber-700 border border-amber-200">
              {v.toLocaleString("vi-VN")} ₫
            </span>
          ),
      },
      {
        key: "giaBan",
        title: "Giá bán",
        width: 220,
        render: (_: any, row: VatLieuDetailDTO) => {
          if (row.dongGiaAllCa) {
            return (
              <span className="px-2 py-1 rounded bg-emerald-50 text-emerald-700 text-xs font-semibold">
                Đồng giá: {formatMoney(row.giaBanChung)}
              </span>
            );
          }
          return (
            <div className="flex flex-col gap-1">
              <span className="font-bold px-2 py-1 rounded bg-blue-50 text-blue-700 text-xs w-fit mx-auto block text-center">
                Ca 1:{" "}
                <span className="font-normal">
                  {formatMoney(row.giaBanCa1)}
                </span>
              </span>
              <span className="font-bold px-2 py-1 rounded bg-orange-50 text-orange-700 text-xs w-fit mx-auto block text-center">
                Ca 2:{" "}
                <span className="font-normal">
                  {formatMoney(row.giaBanCa2)}
                </span>
              </span>
              <span className="font-bold px-2 py-1 rounded bg-pink-50 text-pink-700 text-xs w-fit mx-auto block text-center">
                Ca 3:{" "}
                <span className="font-normal">
                  {formatMoney(row.giaBanCa3)}
                </span>
              </span>
            </div>
          );
        },
      },
      {
        key: "ngungCungCap",
        title: "Trạng thái",
        dataIndex: "ngungCungCap",
        width: 150,
        align: "center" as const,
        className: "text-center",
        render: (v: boolean) => (
          <span
            className={`inline-block px-2 py-1 rounded-md text-xs font-medium border ${
              v
                ? "bg-red-50 text-red-700 border-red-200"
                : "bg-emerald-50 text-emerald-700 border-emerald-200"
            }`}
          >
            {v ? "Ngừng cung cấp" : "Đang cung cấp"}
          </span>
        ),
      },
    ],
    []
  );

  const extraRowActions = (row: VatLieuDetailDTO) => (
    <div className="flex items-center justify-center gap-2">
      {/* XÓA NÚT CẬP NHẬT SL */}
      {row.ngungCungCap ? (
        <QLActionButton
          onClick={() => onToggleNgungCungCap(row, false)}
          color="green"
          title="Tiếp tục cung cấp"
          icon={
            <svg
              className="w-4 h-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M5 13l4 4L19 7"
              />
            </svg>
          }
        >
          Tiếp tục
        </QLActionButton>
      ) : (
        <QLActionButton
          onClick={() => onToggleNgungCungCap(row, true)}
          color="red"
          title="Ngừng cung cấp"
          icon={
            <svg
              className="w-4 h-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M18.364 5.636l-1.414 1.414M6.343 17.657l-1.415 1.415M5.636 5.636l1.414 1.414M17.657 17.657l1.415 1.415M12 8v4m0 4h.01"
              />
            </svg>
          }
        >
          Ngừng cung cấp
        </QLActionButton>
      )}
    </div>
  );

  return (
    <GenericQLTable<VatLieuDetailDTO>
      data={data}
      loading={loading}
      columns={columns as any}
      rowKey={"maVatLieu"}
      tableName="vật tư"
      showUpdateAction={true}
      onUpdate={onUpdate}
      showDeleteAction={false}
      extraRowActions={extraRowActions}
    />
  );
};
