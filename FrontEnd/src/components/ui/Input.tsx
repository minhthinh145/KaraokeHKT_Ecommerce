import React from "react";

interface InputProps {
  label: string;
  placeholder: string;
  type?: string;
  value?: string;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onKeyPress?: (e: React.KeyboardEvent<HTMLInputElement>) => void; // 🔥 Chỉ thêm dòng này
  required?: boolean;
  maxLength?: number; // 🔥 Thêm dòng này
  disabled?: boolean; // 🔥 Thêm dòng này
}

export const Input: React.FC<InputProps> = ({
  label,
  placeholder,
  type = "text",
  value,
  onChange,
  onKeyPress, // 🔥 Thêm prop này
  required = false,
  maxLength, // 🔥 Thêm prop này
  disabled = false, // 🔥 Thêm prop này
}) => (
  <div className="w-full flex flex-col gap-2">
    <label className="text-base font-normal">
      {label}
      {required && <span className="text-red-500">*</span>}
    </label>
    <input
      className={`w-full px-4 py-3 bg-sky-300 rounded-lg border outline-none ${
        disabled ? "opacity-50 cursor-not-allowed" : ""
      }`} // 🔥 Thêm disabled styling
      type={type}
      placeholder={placeholder}
      value={value}
      onChange={onChange}
      onKeyPress={onKeyPress} // 🔥 Thêm event handler
      required={required}
      maxLength={maxLength} // 🔥 Thêm maxLength
      disabled={disabled} // 🔥 Thêm disabled
    />
  </div>
);
