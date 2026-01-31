import React, { useState } from "react";
import { useParams } from "react-router-dom";
import {
  EmployeeSidebar,
  type EmpParent,
  type EmpTab,
} from "../../components/employees/uiForAll/EmployeeSidebar";
import { EmployeeLayout } from "../../components/employees/uiForAll/EmployeeLayout";
import { useEmployeeBaseData } from "../../hooks/nhanVien/useEmployeeBaseData";
import { useNhanVienAll } from "../../hooks/nhanVien/useNhanVienAll";
import { ModalCreateShiftChange } from "../../components/employees/shiftChanges/ModalCreateShiftChange";
import { ComingSoonFeatureCard } from "../../components/employees/ComingSoonFeatureCard";

import dayjs from "dayjs";
import { ConfirmDialog } from "../../components/admins/uiForAll/ConfirmDialog";
import { XemCaManagement } from "../../components/employees/common/XemCaManagement";
import { ChuyenCaManagement } from "../../components/employees/common/ChuyenCaManagement";

export const NhanVienKhoPage: React.FC = () => {
  const { weekStart: weekParam } = useParams();
  const base = useEmployeeBaseData();
  const domain = useNhanVienAll(base.maNhanVien || "", {
    autoLoad: !!base.maNhanVien,
  });

  const [activeParent, setActiveParent] = useState<EmpParent>("quan-ly-lich");
  const [activeTab, setActiveTab] = useState<EmpTab>("lich-lam-viec");
  const [openModal, setOpenModal] = useState(false);
  const [draft, setDraft] = useState<{
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
  } | null>(null);
  const [pendingDrop, setPendingDrop] = useState<{
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
  } | null>(null);
  const [showConfirmDrop, setShowConfirmDrop] = useState(false);

  if (base.loadingProfile)
    return <div className="p-4 text-sm text-gray-500">Đang tải hồ sơ...</div>;
  if (!base.maNhanVien)
    return (
      <div className="p-4 text-sm text-red-500">
        Không tìm thấy tài khoản (hoặc phiên hết hạn)
      </div>
    );

  const handleDraft = (d: {
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
  }) => {
    setDraft(d);
    setOpenModal(true);
  };

  const handleShiftDropCreate = (p: {
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
  }) => {
    setPendingDrop(p);
    setShowConfirmDrop(true);
  };

  const render = () => {
    switch (activeTab) {
      case "lich-lam-viec":
        return (
          <XemCaManagement
            caList={base.caList}
            weekStart={weekParam || base.weekStart}
            data={domain.schedule.data}
            loading={domain.schedule.loading}
          />
        );
      case "yeu-cau-chuyen-ca":
        return (
          <ChuyenCaManagement
            caList={base.caList}
            weekStart={weekParam || base.weekStart}
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
          />
        );
      default:
        return (
          <ComingSoonFeatureCard
            title="Tính năng sắp có"
            desc="Các tiện ích khác sẽ bổ sung."
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
      pageTitle="Nhân viên kho"
      showSearch={false}
      showFilter={false}
    >
      {render()}
    </EmployeeLayout>
  );
};
