import React from "react";
import { Routes, Route } from "react-router-dom";
import { QLHeThongPage } from "../pages/QLHeThongPage";
import { QLNhanSuPage } from "../pages/QLNhanSuPage";

export const AdminRoutes: React.FC = () => {
  return (
    <Routes>
      <Route path="ql-he-thong" element={<QLHeThongPage />} />
      <Route path="ql-nhan-su" element={<QLNhanSuPage />} />
      {/* Default redirect */}
      <Route index element={<QLHeThongPage />} />
    </Routes>
  );
};
