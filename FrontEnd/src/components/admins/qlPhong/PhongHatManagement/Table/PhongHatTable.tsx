import React, { useMemo } from "react";
import { GenericQLTable } from "../../../uiForAll/GenericQLTable";
import { QLActionButton } from "../../../uiForAll/QLActionButton";
import type { PhongHatDetailDTO } from "../../../../../api/types";

interface Props {
  data: PhongHatDetailDTO[];
  loading: boolean;
  onEdit: (row: PhongHatDetailDTO) => void;
  onToggleNgung: (row: PhongHatDetailDTO, next: boolean) => void; // <-- sửa lại nhận 2 tham số
}

const statusBadge = (row: PhongHatDetailDTO) => {
  if (row.ngungHoatDong)
    return (
      <span className="px-2 py-1 text-xs rounded bg-red-100 text-red-700 font-bold">
        Ngừng
      </span>
    );
  if (row.dangSuDung)
    return (
      <span className="px-2 py-1 text-xs rounded bg-emerald-100 text-emerald-700 font-bold">
        Đang sử dụng
      </span>
    );
  return (
    <span className="px-2 py-1 text-xs rounded bg-slate-100 text-slate-600 font-bold">
      Sẵn sàng
    </span>
  );
};

const formatMoney = (n?: number | null) =>
  n == null ? "-" : n.toLocaleString("vi-VN") + " ₫";

export const PhongHatTable: React.FC<Props> = ({
  data,
  loading,
  onEdit,
  onToggleNgung,
}) => {
  // Sắp xếp data theo giá thuê hiện tại tăng dần
  const sortedData = useMemo(
    () =>
      [...data].sort(
        (a, b) => (a.giaThueHienTai ?? 0) - (b.giaThueHienTai ?? 0)
      ),
    [data]
  );

  const columns = useMemo(
    () => [
      {
        key: "img",
        title: "Ảnh phòng",
        width: 100,
        render: (_: any, row: PhongHatDetailDTO) => (
          <div className="w-32 h-32 flex items-center justify-center overflow-hidden rounded-lg bg-slate-100">
            {(row as any).hinhAnhPhong || row.hinhAnhSanPham ? (
              <img
                src={(row as any).hinhAnhPhong || row.hinhAnhSanPham!}
                alt={row.tenSanPham || ""}
                className="object-cover object-center w-full h-full"
              />
            ) : (
              <span className="text-slate-400  text-center px-1">
                Không ảnh
              </span>
            )}
          </div>
        ),
      },
      {
        key: "tenPhong",
        title: "Tên phòng",
        dataIndex: "tenPhong",
        width: 250,
        render: (_: any, row: PhongHatDetailDTO) => (
          <span className="font-bold text-base text-slate-800">
            {row.tenSanPham}
          </span>
        ),
      },
      {
        key: "loai",
        title: "Loại / Sức chứa",
        width: 170,
        render: (_: any, row: PhongHatDetailDTO) => {
          // Gán màu nền khác nhau cho từng loại phòng
          let bg = "bg-indigo-50 text-indigo-600";
          if (row.tenLoaiPhong?.toLowerCase().includes("vip"))
            bg = "bg-yellow-50 text-yellow-700";
          else if (row.tenLoaiPhong?.toLowerCase().includes("trung"))
            bg = "bg-blue-50 text-blue-700";
          else if (row.tenLoaiPhong?.toLowerCase().includes("nhỏ"))
            bg = "bg-pink-50 text-pink-700";
          // Thêm các loại khác nếu muốn

          return (
            <div className="flex flex-col items-center gap-1">
              <span className={`text-xs font-bold px-2 py-0.5 rounded ${bg}`}>
                {row.tenLoaiPhong}
              </span>
              <span className="text-[11px] font-bold px-2 py-0.5 rounded bg-slate-50 text-slate-600">
                {row.sucChua} người
              </span>
            </div>
          );
        },
      },
      {
        key: "status",
        title: "Trạng thái",
        width: 120,
        render: (_: any, row: PhongHatDetailDTO) => statusBadge(row), // đã font-bold
      },
      {
        key: "giaThueChiTiet",
        title: "Giá thuê",
        width: 220,
        render: (_: any, row: PhongHatDetailDTO) => {
          if (row.dongGiaAllCa) {
            return (
              <span className="px-2 py-1 rounded bg-emerald-50 text-emerald-700 text-xs font-semibold w-fit  mx-auto block text-center">
                Đồng giá: {formatMoney(row.giaThueChung)}
              </span>
            );
          }
          return (
            <div className="flex flex-col gap-1 items-center">
              <span className="font-bold px-2 py-1 rounded bg-blue-50 text-blue-700 text-xs w-fit text-center">
                Ca 1:{" "}
                <span className="font-normal">
                  {formatMoney(row.giaThueCa1)}
                </span>
              </span>
              <span className="font-bold px-2 py-1 rounded bg-orange-50 text-orange-700 text-xs w-fit text-center">
                Ca 2:{" "}
                <span className="font-normal">
                  {formatMoney(row.giaThueCa2)}
                </span>
              </span>
              <span className="font-bold px-2 py-1 rounded bg-pink-50 text-pink-700 text-xs w-fit text-center">
                Ca 3:{" "}
                <span className="font-normal">
                  {formatMoney(row.giaThueCa3)}
                </span>
              </span>
            </div>
          );
        },
      },
    ],
    []
  );

  const extraRowActions = (row: PhongHatDetailDTO) => (
    <div className="flex items-center justify-center gap-2">
      <QLActionButton
        onClick={() =>
          row.ngungHoatDong
            ? onToggleNgung(row, false)
            : onToggleNgung(row, true)
        }
        color={row.ngungHoatDong ? "green" : "red"}
        title={row.ngungHoatDong ? "Kích hoạt lại" : "Ngừng hoạt động"}
      >
        {row.ngungHoatDong ? "Kích hoạt" : "Ngừng"}
      </QLActionButton>
    </div>
  );

  return (
    <GenericQLTable<PhongHatDetailDTO>
      data={sortedData}
      loading={loading}
      columns={columns as any}
      rowKey="maPhong"
      tableName="phòng hát"
      showUpdateAction
      onUpdate={onEdit}
      showDeleteAction={false}
      extraRowActions={extraRowActions}
    />
  );
};
