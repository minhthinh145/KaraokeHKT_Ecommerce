import React from "react";
import { HiLockClosed } from "react-icons/hi";
interface AuthLayoutProps {
  children: React.ReactNode;
  title: string;
  subtitle: string;
  footerText: string;
  footerLinkText: string;
  onFooterLinkClick: () => void;
  extraLinkText?: string;
  onExtraLinkClick?: () => void;
}

export const AuthLayout: React.FC<AuthLayoutProps> = ({
  children,
  title,
  subtitle,
  footerText,
  footerLinkText,
  onFooterLinkClick,
  extraLinkText,
  onExtraLinkClick,
}) => (
  <div className="min-h-screen bg-gray-100 flex items-center justify-center p-4">
    <div className="w-full max-w-4xl bg-white rounded-3xl border-2 border-black shadow-2xl flex flex-col lg:flex-row overflow-hidden">
      {/* Left */}
      <div className="lg:w-1/2 bg-blue-900 flex flex-col items-center justify-center p-8">
        <div className="w-48 h-48 bg-white rounded-full flex items-center justify-center overflow-hidden">
          <img
            className="w-40 h-40 rounded-full object-cover"
            src="/LogoHKT.jpg"
            alt="Logo"
          />
        </div>
      </div>
      {/* Right */}
      <div className="lg:w-1/2 bg-white flex flex-col p-8">
        {/* Icon khóa và tiêu đề căn giữa */}
        <div className="flex flex-col items-center mb-6">
          <HiLockClosed className="text-4xl text-blue-700 mb-3" />
          <h1 className="text-3xl font-bold text-center">{title}</h1>
          <p className="text-base text-gray-600 text-center mt-2">{subtitle}</p>
        </div>

        <div className="flex-1">{children}</div>

        <div className="flex flex-col gap-2 mt-3 text-sm">
          {extraLinkText && (
            <button
              onClick={onExtraLinkClick}
              className="text-blue-700 font-bold underline w-fit self-end"
            >
              {extraLinkText}
            </button>
          )}
          <div className="text-center">
            <span className="text-gray-600">{footerText} </span>
            <button
              onClick={onFooterLinkClick}
              className="text-blue-700 font-bold underline"
            >
              {footerLinkText}
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
);
