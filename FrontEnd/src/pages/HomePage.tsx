import React from "react";
import { MainLayout } from "../../src/components/customer/Main/MainLayOut";
import { HeroSection } from "../components/customer/Layout/HeroSection";

export const HomePage: React.FC = () => {
  return (
    <MainLayout>
      <HeroSection />
    </MainLayout>
  );
};
