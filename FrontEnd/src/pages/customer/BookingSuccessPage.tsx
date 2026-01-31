import React, { useEffect } from "react";
import { Result, Button } from "antd";
import { CustomerLayout } from "../../components/customer/CustomerLayout";
import { useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";

export const BookingSuccessPage: React.FC = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  return (
    <CustomerLayout>
      <div className="py-16 px-4">
        <Result
          status="success"
          title="Thanh toán thành công!"
          subTitle="Đặt phòng của bạn đã được xác nhận. Chúc bạn có trải nghiệm vui vẻ."
          extra={[
            <Button
              type="primary"
              key="booking"
              onClick={() => navigate("/customer/booking")}
            >
              Quay lại đặt phòng
            </Button>,
            <Button
              key="history"
              onClick={() => navigate("/customer/booking/history")}
            >
              Lịch sử đặt phòng
            </Button>,
          ]}
        />
      </div>
    </CustomerLayout>
  );
};
