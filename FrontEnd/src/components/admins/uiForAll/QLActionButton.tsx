import React from "react";

interface QLActionButtonProps {
  onClick: () => void;
  children: React.ReactNode;
  color?: "indigo" | "red" | "green";
  icon?: React.ReactNode;
  title?: string;
}

export const QLActionButton: React.FC<QLActionButtonProps> = ({
  onClick,
  children,
  color = "indigo",
  icon,
  title,
}) => {
  const colorClass =
    color === "red"
      ? "text-red-700 bg-red-50 border-red-200 hover:bg-red-100 hover:border-red-300 focus:ring-red-500"
      : color === "green"
      ? "text-green-700 bg-green-50 border-green-200 hover:bg-green-100 hover:border-green-300 focus:ring-green-500"
      : "text-indigo-700 bg-indigo-50 border-indigo-200 hover:bg-indigo-100 hover:border-indigo-300 focus:ring-indigo-500";
  return (
    <button
      onClick={onClick}
      className={`
        inline-flex items-center gap-2 px-3 py-2 text-sm font-medium
        border rounded-lg
        focus:outline-none focus:ring-2 focus:ring-offset-2
        transition-all duration-200
        ${colorClass}
      `}
      title={title}
      type="button"
    >
      {icon}
      <span>{children}</span>
    </button>
  );
};
