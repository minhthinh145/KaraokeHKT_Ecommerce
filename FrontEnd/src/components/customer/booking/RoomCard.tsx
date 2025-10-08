import React from "react";
import { Button, Tag } from "antd";
import type { PhongHatForCustomerDTO } from "../../../api";

interface RoomCardProps {
  room: PhongHatForCustomerDTO & {
    // Extend (in case backend refactor not yet reflected in local DTO)
    giaThueCa1?: number | null;
    giaThueCa2?: number | null;
    giaThueCa3?: number | null;
    coGiaTheoCa?: boolean;
    giaThueHienTai: number;
    moTa?: string | null;
  };
  onBook: (r: PhongHatForCustomerDTO) => void;
}

export const RoomCard: React.FC<RoomCardProps> = ({ room, onBook }) => {
  const img =
    room.hinhAnhPhong ||
    "https://via.placeholder.com/600x360?text=Phong+Karaoke";

  const formatCurrency = (v?: number | null) =>
    v == null ? "-" : v.toLocaleString("vi-VN") + " đ";

  const renderPrice = () => {
    if (room.coGiaTheoCa) {
      return (
        <div className="text-sm">
          <b>Giá theo ca:</b>
          <div className="mt-1 space-y-0.5">
            {room.giaThueCa1 != null && (
              <div>
                Ca 1: <b>{formatCurrency(room.giaThueCa1)}</b>
              </div>
            )}
            {room.giaThueCa2 != null && (
              <div>
                Ca 2: <b>{formatCurrency(room.giaThueCa2)}</b>
              </div>
            )}
            {room.giaThueCa3 != null && (
              <div>
                Ca 3: <b>{formatCurrency(room.giaThueCa3)}</b>
              </div>
            )}
          </div>
        </div>
      );
    }
    return (
      <div className="text-sm">
        Giá: <b>{formatCurrency(room.giaThueHienTai)} / giờ</b>
      </div>
    );
  };

  return (
    <div className="bg-white rounded-lg shadow-sm border hover:shadow-md transition flex flex-col">
      <div className="relative w-full h-40 overflow-hidden rounded-t-lg">
        <img
          src={img}
          alt={room.tenPhong}
          className="w-full h-full object-cover"
          loading="lazy"
        />
        <div className="absolute top-2 left-2 flex gap-1">
          {/* Replaced availability tag with pricing mode tag */}
          {room.coGiaTheoCa ? (
            <Tag color="blue">Giá theo ca</Tag>
          ) : (
            <Tag color="purple">Giá giờ</Tag>
          )}
        </div>
      </div>
      <div className="p-4 flex flex-col gap-2 flex-1">
        <div>
          <div className="font-semibold text-gray-800 truncate">
            {room.tenPhong}
          </div>
          <div className="text-xs text-gray-500">{room.tenLoaiPhong}</div>
        </div>
        <div className="text-sm">
          Sức chứa: <b>{room.sucChua}</b>
        </div>
        {/* Updated price section */}
        {renderPrice()}
        {room.moTa && (
          <div className="text-xs text-gray-500 line-clamp-2">{room.moTa}</div>
        )}
        <div className="pt-2 mt-auto">
          <Button
            type="primary"
            block
            // Disabled removed; API now returns only available rooms
            onClick={() => onBook(room)}
          >
            Đặt phòng
          </Button>
        </div>
      </div>
    </div>
  );
};
