import React from "react";
import { Card, Button, Typography, Divider, Tooltip } from "antd";
import { EditOutlined } from "@ant-design/icons";
import { useCaLamViec } from "../../../../../hooks/QLNhanSu/useCaLamViec";
import type { LuongCaLamViecDTO } from "../../../../../api";
const { Text, Title } = Typography;

// Props riêng cho UI card
export interface DefaultLuongCardData {
  maLuongCaLamViec: number;
  tenCaLamViec: string;
  giaCa: number;
  gioBatDauCa: string;
  gioKetThucCa: string;
}

interface Props {
  defaults: (DefaultLuongCardData & { dto: LuongCaLamViecDTO })[];
  onEdit: (item: LuongCaLamViecDTO) => void;
}

export const DefaultLuongCards: React.FC<Props> = ({ defaults, onEdit }) => {
  const { caLamViecList } = useCaLamViec(); // Để tính số giờ ca
  return (
    <div className="mb-8">
      <div className="flex items-center gap-2 mb-4">
        <Title level={3} className="!mb-0 !text-blue-700 tracking-wide">
          Lương mặc định theo ca
        </Title>
      </div>
      <div className="grid gap-6 md:grid-cols-3">
        {defaults.map((d) => {
          // Tính số giờ ca từ caLamViecList
          const ca = caLamViecList.find((c) => c.maCa === d.dto.maCa);
          let soGio = 0;
          if (ca?.gioBatDauCa && ca?.gioKetThucCa) {
            const [h1, m1] = ca.gioBatDauCa.split(":").map(Number);
            const [h2, m2] = ca.gioKetThucCa.split(":").map(Number);
            soGio = h2 + (m2 || 0) / 60 - (h1 + (m1 || 0) / 60);
            if (soGio <= 0) soGio += 24;
          }
          const luong1h = soGio ? Math.round((d.giaCa || 0) / soGio) : 0;

          return (
            <Card
              key={d.maLuongCaLamViec}
              className="hover:shadow-xl transition-all border-2 border-blue-100 hover:border-blue-400 bg-gradient-to-br from-blue-50 to-white"
              bodyStyle={{ padding: "28px 20px 20px 20px" }}
              actions={[
                <Tooltip title="Chức năng sửa sẽ sớm có!">
                  <Button
                    key="edit"
                    type="text"
                    icon={<EditOutlined />}
                    onClick={() => onEdit(d.dto)}
                    disabled
                    className="text-blue-600"
                  >
                    Chỉnh sửa
                  </Button>
                </Tooltip>,
              ]}
            >
              <div className="text-center">
                <Text className="block text-2xl font-extrabold text-blue-800 mb-1 uppercase tracking-wide drop-shadow">
                  {d.tenCaLamViec}
                </Text>
                <div className="text-4xl font-black text-blue-600 mb-2 drop-shadow">
                  {d.giaCa?.toLocaleString("vi-VN") || "0"}{" "}
                  <span className="text-lg font-bold">₫</span>
                </div>
                <Divider className="!my-3" />
                <div className="flex items-center justify-center gap-2 mb-1">
                  <div className="w-2 h-2 rounded-full bg-green-500 animate-pulse"></div>
                  <Text type="secondary" className="text-xs font-semibold">
                    Lương cố định
                  </Text>
                </div>
                <Text type="secondary" className="block text-xs mt-1">
                  {soGio
                    ? `= ${luong1h.toLocaleString(
                        "vi-VN"
                      )}₫ / giờ (${soGio}h/ca)`
                    : "Không xác định số giờ"}
                </Text>
              </div>
            </Card>
          );
        })}
      </div>
    </div>
  );
};
