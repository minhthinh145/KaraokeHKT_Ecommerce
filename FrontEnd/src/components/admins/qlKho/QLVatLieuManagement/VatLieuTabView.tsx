import React from "react";
import { Tabs } from "antd";
import { VatLieuTable } from "./Table/VatLieuTable";
import type { VatLieuDetailDTO } from "../../../../api";

interface Props {
  data: VatLieuDetailDTO[];
  loading: boolean;
  onUpdate: (row: VatLieuDetailDTO) => void;
  onToggleNgungCungCap: (row: VatLieuDetailDTO, next: boolean) => void;
}

export const VatLieuTabView: React.FC<Props> = ({
  data,
  loading,
  onUpdate,
  onToggleNgungCungCap,
}) => {
  const dangCungCap = data.filter((x) => !x.ngungCungCap);
  const daNgungCungCap = data.filter((x) => x.ngungCungCap);

  return (
    <Tabs defaultActiveKey="1" className="mb-4">
      <Tabs.TabPane tab="Đang cung cấp" key="1">
        <div className="mb-3 flex flex-wrap gap-3 text-xs">
          <div className="inline-flex items-center gap-1">
            <span className="inline-block w-3 h-3 rounded bg-green-500 border border-green-600" />
            <span className="text-slate-600">Giá nhập</span>
          </div>
          <div className="inline-flex items-center gap-1">
            <span className="inline-block w-3 h-3 rounded bg-amber-500 border border-amber-600" />
            <span className="text-slate-600">Giá bán</span>
          </div>
        </div>
        <VatLieuTable
          data={dangCungCap}
          loading={loading}
          onUpdate={onUpdate}
          onToggleNgungCungCap={onToggleNgungCungCap}
        />
      </Tabs.TabPane>
      <Tabs.TabPane tab="Đã ngừng cung cấp" key="2">
        <div className="mb-3 flex flex-wrap gap-3 text-xs">
          <div className="inline-flex items-center gap-1">
            <span className="inline-block w-3 h-3 rounded bg-green-500 border border-green-600" />
            <span className="text-slate-600">Giá nhập</span>
          </div>
          <div className="inline-flex items-center gap-1">
            <span className="inline-block w-3 h-3 rounded bg-amber-500 border border-amber-600" />
            <span className="text-slate-600">Giá bán</span>
          </div>
        </div>
        <VatLieuTable
          data={daNgungCungCap}
          loading={loading}
          onUpdate={onUpdate}
          onToggleNgungCungCap={onToggleNgungCungCap}
        />
      </Tabs.TabPane>
    </Tabs>
  );
};
