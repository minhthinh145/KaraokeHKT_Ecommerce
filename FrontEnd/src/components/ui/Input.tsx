import React from "react";

interface InputProps {
  label: string;
  placeholder: string;
  type?: string;
  value?: string;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
  required?: boolean;
}

export const Input: React.FC<InputProps> = ({
  label,
  placeholder,
  type = "text",
  value,
  onChange,
  required = false,
}) => (
  <div className="w-full flex flex-col gap-2">
    <label className="text-base font-normal">
      {label}
      {required && <span className="text-red-500">*</span>}
    </label>
    <input
      className="w-full px-4 py-3 bg-sky-300 rounded-lg border outline-none"
      type={type}
      placeholder={placeholder}
      value={value}
      onChange={onChange}
      required={required}
    />
  </div>
);
