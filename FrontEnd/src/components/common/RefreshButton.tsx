//// filepath: d:\OOAD_Project\FrontEnd\src\components\common\RefreshButton.tsx
import React from "react";
import { Button } from "antd";
import { ReloadOutlined } from "@ant-design/icons";

interface RefreshButtonProps {
  onClick: () => void;
  loading?: boolean;
  children?: React.ReactNode;
  className?: string;
}

export const RefreshButton: React.FC<RefreshButtonProps> = ({
  onClick,
  loading,
  children = "Làm mới",
  className,
}) => {
  return (
    <Button
      icon={<ReloadOutlined />}
      onClick={onClick}
      loading={loading}
      className={
        className ||
        "bg-gradient-to-r from-blue-500 to-blue-600 hover:from-blue-600 hover:to-blue-700 border-none text-white font-medium"
      }
    >
      {children}
    </Button>
  );
};
export default RefreshButton;
