import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { EmployeeLayout } from "../../components/employees/uiForAll/EmployeeLayout";
import {
  EmployeeSidebar,
  type EmpParent,
  type EmpTab,
} from "../../components/employees/uiForAll/EmployeeSidebar";
import { ComingSoonFeatureCard } from "../../components/employees/ComingSoonFeatureCard";

import { XemCaManagement } from "../../components/employees/common/XemCaManagement";
import { ChuyenCaManagement } from "../../components/employees/common/ChuyenCaManagement";
import { useEmployeeBaseData } from "../../hooks/nhanVien/useEmployeeBaseData";
import { useNhanVienAll } from "../../hooks/nhanVien";

export const NhanVienFrontDeskServicePage: React.FC = () => {
  const { weekStart: weekParam } = useParams();
  const base = useEmployeeBaseData();
  const domain = useNhanVienAll(base.maNhanVien || "", {
    autoLoad: !!base.maNhanVien, // đảm bảo false khi chưa có
  });

  const [activeParent, setActiveParent] = useState<EmpParent>("quan-ly-lich");
  const [activeTab, setActiveTab] = useState<EmpTab>("lich-lam-viec");
  const [weekStart, setWeekStart] = useState(base.weekStart); // base.weekStart lấy từ domain hiện tại

  // Khi weekStart đổi: gọi domain refresh

  if (base.loadingProfile)
    return <div className="p-4 text-sm text-gray-500">Đang tải hồ sơ...</div>;
  if (!base.maNhanVien)
    return (
      <div className="p-4 text-sm text-red-500">
        Không tìm thấy tài khoản (hoặc phiên hết hạn)
      </div>
    );

  const render = () => {
    switch (activeTab) {
      case "lich-lam-viec":
        return (
          <XemCaManagement
            caList={base.caList}
            weekStart={weekStart}
            data={domain.schedule.data}
            loading={domain.schedule.loading}
            onChangeWeekStart={setWeekStart}
          />
        );
      case "yeu-cau-chuyen-ca":
        return (
          <ChuyenCaManagement
            caList={base.caList}
            weekStart={weekStart}
            schedule={domain.schedule.data}
            scheduleLoading={domain.schedule.loading}
            shiftChanges={domain.shiftChanges.data}
            shiftChangesLoading={domain.shiftChanges.loading}
            draggingId={domain.schedule.draggingLichId}
            beginDrag={domain.schedule.beginDrag}
            endDrag={domain.schedule.endDrag}
            createShiftChange={domain.shiftChanges.create}
            deleteShiftChange={domain.shiftChanges.remove}
            refreshAll={domain.refreshAll}
            onChangeWeekStart={setWeekStart}
          />
        );
      case "bao-cao-tong-quan":
        return (
          <ComingSoonFeatureCard
            title="Báo cáo tổng quan"
            desc="Trang báo cáo tổng quan đang được phát triển."
          />
        );
      default:
        return (
          <ComingSoonFeatureCard
            title="Tính năng sắp có"
            desc="Các tiện ích khác sẽ được bổ sung."
          />
        );
    }
  };

  return (
    <EmployeeLayout
      sidebarContent={
        <EmployeeSidebar
          isCollapsed={false}
          activeParent={activeParent}
          activeTab={activeTab}
          onParentChange={setActiveParent}
          onTabChange={setActiveTab}
        />
      }
      pageTitle="Nhân viên dịch vụ"
      showSearch={false}
      showFilter={false}
    >
      {render()}
    </EmployeeLayout>
  );
};
