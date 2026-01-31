import React from "react";
import { PlusOutlined } from "@ant-design/icons";

export const AddButton: React.FC<{
  onClick?: () => void;
  className?: string;
  label?: string;
}> = ({ onClick, className = "", label = "Thêm" }) => {
  return (
    <button
      onClick={onClick}
      className={`flex flex-col items-center justify-center w-full h-full text-blue-500 hover:text-blue-700 ${className}`}
      style={{ minHeight: 80 }}
    >
      <PlusOutlined />
      <span className="text-sm font-medium mt-1">{label}</span>
    </button>
  );
};
