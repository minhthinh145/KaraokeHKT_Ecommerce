import React, { useMemo } from "react";
import { Result, Button } from "antd";
import { useNavigate, useLocation } from "react-router-dom";
import { MainLayout } from "../../components/customer/Main/MainLayOut";

export const BookingFailedPage: React.FC = () => {
  const navigate = useNavigate();
  const { search } = useLocation();
  const message = useMemo(() => {
    const p = new URLSearchParams(search);
    return decodeURIComponent(
      p.get("message") || "Thanh toán thất bại hoặc bị hủy."
    );
  }, [search]);

  return (
    <MainLayout>
      <div className="py-16 px-4">
        <Result
          status="error"
          title="Thanh toán thất bại"
          subTitle={message}
          extra={[
            <Button
              key="retry"
              type="primary"
              onClick={() => navigate("/booking")}
            >
              Thử lại
            </Button>,
          ]}
        />
      </div>
    </MainLayout>
  );
};
