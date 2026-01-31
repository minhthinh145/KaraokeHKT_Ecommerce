import React from "react";

export const ComingSoonFeatureCard: React.FC<{
  title?: string;
  desc?: string;
}> = ({
  title = "Tính năng sắp ra mắt",
  desc = "Chúng tôi đang phát triển thêm chức năng nâng cao cho nhân viên. Vui lòng quay lại sau.",
}) => (
  <div className="border rounded p-6 bg-gradient-to-r from-slate-50 to-white">
    <h3 className="text-basefont-semibold mb-2">{title}</h3>
    <p className="text-sm text-slate-600">{desc}</p>
  </div>
);
