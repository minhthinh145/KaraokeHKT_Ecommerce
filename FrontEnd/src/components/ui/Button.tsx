import React from "react";

interface ButtonProps {
  children: React.ReactNode;
  onClick?: () => void;
  type?: "button" | "submit";
  fullWidth?: boolean;
}

export const Button: React.FC<ButtonProps> = ({
  children,
  onClick,
  type = "button",
  fullWidth = false,
}) => (
  <button
    type={type}
    onClick={onClick}
    className={`p-3 bg-indigo-700 rounded-lg text-white font-medium ${
      fullWidth ? "w-full" : ""
    }`}
  >
    {children}
  </button>
);
