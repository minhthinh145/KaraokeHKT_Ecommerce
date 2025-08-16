import React from "react";

export interface EmployeeCardProps {
  name: string;
  id?: string;
  info?: string;
  onEdit?: () => void;
  onDelete?: () => void;
  editable?: boolean;
  deletable?: boolean;
  showId?: boolean; // ẩn/hiện mã NV
  className?: string;
}

export const EmployeeCard: React.FC<EmployeeCardProps> = ({
  name,
  id,
  info,
  onEdit,
  onDelete,
  editable = true,
  deletable = true,
  showId = false, // mặc định ẩn mã nhân viên cho lịch làm việc
  className = "",
}) => {
  return (
    <div
      className={`bg-blue-50 rounded-xl border border-blue-300 shadow p-3 flex flex-col items-center max-w-xs ${className}`}
    >
      <div className="text-center text-blue-700 text-base font-semibold">
        {name !== id ? name : "Đang tải..."}
      </div>
      {showId && id ? (
        <div className="text-center text-black text-sm font-medium mb-1">
          Mã nhân viên: {id}
        </div>
      ) : null}
      {info ? (
        <div className="text-center text-zinc-700 text-xs font-bold mb-2 break-words">
          {info}
        </div>
      ) : null}
      {(editable || deletable) && (
        <div className="flex justify-center text-xl gap-3 mt-2">
          {editable && (
            <button
              onClick={onEdit}
              className="text-blue-600 hover:text-blue-800"
              aria-label="edit"
            >
              ✎
            </button>
          )}
          {deletable && (
            <button
              onClick={onDelete}
              className="text-red-500 hover:text-red-700"
              aria-label="delete"
            >
              🗑
            </button>
          )}
        </div>
      )}
    </div>
  );
};
