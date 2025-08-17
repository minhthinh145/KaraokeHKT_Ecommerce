import React from "react";
import { LichLamViecTable } from "../../admins/uiForAll/LichLamViecTable";
import type { LichLamViecDTO } from "../../../api/types/admins/QLNhanSutypes";
import type { CaBrief } from "../../admins/uiForAll/LichLamViecTable";
import { Typography } from "antd";
import dayjs from "dayjs";
import { WeekNavigator } from "../../admins/uiForAll/WeekNavigator";
import RefreshButton from "../../common/RefreshButton";

interface XemCaManagementProps {
  caList: CaBrief[];
  weekStart: string;
  data: LichLamViecDTO[];
  loading?: boolean;
  showSendNoti?: boolean;
  onChangeWeekStart?: (newWeekStart: string) => void;
  onRefresh?: () => void; // thêm optional
}

export const XemCaManagement: React.FC<XemCaManagementProps> = ({
  caList,
  weekStart,
  data,
  loading,
  showSendNoti = false,
  onChangeWeekStart,
  onRefresh,
}) => {
  const handleRefresh = () => {
    onRefresh?.();
  };

  return (
    <div>
      <div className="mb-4 flex items-center justify-between">
        <div>
          <Typography.Title level={3} className="!mb-0">
            Lịch làm việc tuần
          </Typography.Title>
          <Typography.Text type="secondary">
            Xem lịch làm việc và ca theo tuần
          </Typography.Text>
        </div>
        <RefreshButton onClick={handleRefresh} loading={loading} />
      </div>

      <WeekNavigator
        weekStart={weekStart}
        onChangeWeekStart={(ws) => onChangeWeekStart?.(ws)}
        className="mb-3"
        yearRange={{ min: dayjs().year() - 1, max: dayjs().year() + 1 }}
      />

      <LichLamViecTable
        caList={caList}
        weekStart={weekStart}
        data={data}
        canAdd={false}
        canEdit={false}
        canDelete={false}
        showSendNoti={showSendNoti}
        loading={loading}
        enableShiftRequestDrag={false}
      />
    </div>
  );
};
