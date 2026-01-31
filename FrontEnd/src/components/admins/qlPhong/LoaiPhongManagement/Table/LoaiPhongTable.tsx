import React, { useMemo } from "react";
import { GenericQLTable } from "../../../uiForAll/GenericQLTable";
import type { LoaiPhongDTO } from "../../../../../redux/admin/QLPhong/types";

interface Props {
  data: LoaiPhongDTO[];
  loading: boolean;
  onUpdate: (row: LoaiPhongDTO) => void;
}

const getBgClass = (sucChua: number) => {
  if (sucChua < 10) return "bg-green-100 text-green-700";
  if (sucChua < 15) return "bg-yellow-100 text-yellow-800";
  return "bg-red-100 text-red-700";
};

export const LoaiPhongTable: React.FC<Props> = ({
  data,
  loading,
  onUpdate,
}) => {
  const columns = useMemo(
    () => [
      {
        key: "tenLoaiPhong",
        title: "Loại phòng",
        dataIndex: "tenLoaiPhong",
        width: 250,
        render: (v: string) => (
          <span className="font-semibold text-slate-800">{v}</span>
        ),
      },
      {
        key: "sucChua",
        title: "Sức chứa",
        dataIndex: "sucChua",
        width: 120,
        render: (v: number) => (
          <span className={`px-3 py-1 rounded font-semibold ${getBgClass(v)}`}>
            {v} người
          </span>
        ),
      },
    ],
    []
  );

  return (
    <GenericQLTable<LoaiPhongDTO>
      data={data}
      loading={loading}
      columns={columns as any}
      rowKey={"maLoaiPhong"}
      tableName="loại phòng"
      showUpdateAction
      onUpdate={onUpdate}
      showDeleteAction={false}
    />
  );
};
