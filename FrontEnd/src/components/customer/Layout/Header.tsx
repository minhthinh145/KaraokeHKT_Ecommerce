import React from "react";
import { Navigation } from "./Navigation";
import { UserMenu } from "./UserMenu";

export const Header: React.FC = () => {
  return (
    <header className="fixed top-0 left-0 w-full h-16 bg-white/95 backdrop-blur-sm border-b border-neutral-200 z-50 shadow-sm">
      <div className="container mx-auto flex items-center justify-between h-full px-4">
        {/* Logo */}
        <div className="flex items-center space-x-4">
          <img
            className="w-10 h-10 rounded-full object-cover"
            src="/LogoHKT.jpg"
            alt="Karaoke HKT Logo"
          />
          <h1 className="text-xl lg:text-2xl font-bold font-['Space_Grotesk'] text-black">
            KARAOKE HKT
          </h1>
        </div>

        {/* Navigation - Hidden on mobile */}
        <div className="hidden lg:block">
          <Navigation />
        </div>

        {/* User Menu */}
        <UserMenu />
      </div>
    </header>
  );
};
