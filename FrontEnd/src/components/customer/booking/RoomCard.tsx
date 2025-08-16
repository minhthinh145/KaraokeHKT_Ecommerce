import React from "react";
import { Button, Tag } from "antd";
import type { PhongHatForCustomerDTO } from "../../../api/customer/bookingApi";

interface RoomCardProps {
  room: PhongHatForCustomerDTO;
  onBook: (r: PhongHatForCustomerDTO) => void;
}

export const RoomCard: React.FC<RoomCardProps> = ({ room, onBook }) => {
  const img =
    room.hinhAnhPhong ||
    "https://via.placeholder.com/600x360?text=Phong+Karaoke";
  return (
    <div className="bg-white rounded-lg shadow-sm border hover:shadow-md transition flex flex-col">
      <div className="relative w-full h-40 overflow-hidden rounded-t-lg">
        <img
          src={img}
          alt={room.tenPhong}
          className="w-full h-full object-cover"
          loading="lazy"
        />
        <div className="absolute top-2 left-2">
          {room.available ? (
            <Tag color="green">Trống</Tag>
          ) : (
            <Tag color="red">Bận</Tag>
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
        <div className="text-sm">
          Giá: <b>{room.giaThueHienTai.toLocaleString()} đ / giờ</b>
        </div>
        {room.moTa && (
          <div className="text-xs text-gray-500 line-clamp-2">{room.moTa}</div>
        )}
        <div className="pt-2 mt-auto">
          <Button
            type="primary"
            block
            disabled={!room.available}
            onClick={() => onBook(room)}
          >
            Đặt phòng
          </Button>
        </div>
      </div>
    </div>
  );
};
