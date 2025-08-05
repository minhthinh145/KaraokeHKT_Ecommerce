import React from "react";
import { Header } from "../Layout/Header";
import { Footer } from "../Layout/Footer";

interface MainLayoutProps {
  children: React.ReactNode;
}

export const MainLayout: React.FC<MainLayoutProps> = ({ children }) => {
  return (
    <div className="min-h-screen flex flex-col bg-gradient-to-l from-indigo-600 to-indigo-900">
      <Header />
      <main className="flex-1 flex items-center justify-center pt-16">
        {children}
      </main>
      <Footer />
    </div>
  );
};
