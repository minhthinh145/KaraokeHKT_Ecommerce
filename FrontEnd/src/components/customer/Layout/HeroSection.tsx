import React from "react";
import { useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import { Button } from "../../ui/Button";
import type { RootState } from "../../../redux/store";

export const HeroSection: React.FC = () => {
  const navigate = useNavigate();
  const { isAuthenticated } = useSelector((state: RootState) => state.auth);

  const handleBookingClick = () => {
    if (isAuthenticated) {
      navigate("/customer/booking");
    } else {
      navigate("/login");
    }
  };

  return (
    <section className="w-full flex flex-col lg:flex-row items-center justify-center px-4 py-12">
      {/* Left: Image with overlay text */}
      <div className="relative w-full lg:w-1/2 max-w-[700px] aspect-[710/553] rounded-3xl overflow-hidden shadow-xl flex items-center justify-center">
        <img
          src="/heropng.jpg"
          alt="Karaoke Interior"
          className="w-full h-full object-cover opacity-60"
        />
        {/* Overlay for darken */}
        <div className="absolute inset-0 bg-black/40" />
        {/* Text on image */}
        <div className="absolute inset-0 flex flex-col items-center justify-center z-10 px-4">
          <h1 className="text-white text-3xl md:text-4xl lg:text-5xl font-bold font-['Space_Grotesk'] mb-6 text-center drop-shadow-lg">
            THƯ GIÃN CÙNG ÂM NHẠC
          </h1>
          <Button
            variant="primary"
            size="large"
            onClick={handleBookingClick}
            className="bg-indigo-700 text-white text-lg font-medium px-8 py-3 rounded-lg hover:bg-indigo-800 transition-colors shadow"
          >
            ĐẶT PHÒNG NGAY
          </Button>
        </div>
      </div>

      {/* Right: About text */}
      <div className="w-full lg:w-1/2 flex flex-col justify-center items-start mt-10 lg:mt-0 lg:pl-12">
        <h2 className="text-2xl md:text-3xl font-bold font-['Space_Grotesk'] text-white mb-4 border-b-2 border-indigo-400 inline-block px-2">
          GIỚI THIỆU
        </h2>
        <div className="text-white text-base md:text-lg font-medium mb-4 leading-relaxed">
          <p className="mb-2 font-semibold">
            Karaoke HKT – Nơi Âm Nhạc Thăng Hoa
          </p>
          <p>
            Karaoke HKT mang đến cho bạn không gian giải trí hiện đại, âm thanh
            ánh sáng đỉnh cao cùng hệ thống phòng hát sang trọng, đa dạng từ
            Standard đến VIP. Chúng tôi cam kết đem lại trải nghiệm ca hát tuyệt
            vời, dịch vụ chu đáo và thực đơn phong phú cho những phút giây thư
            giãn trọn vẹn bên gia đình và bạn bè.
          </p>
        </div>
        <p className="text-white text-base md:text-lg font-bold font-['Space_Grotesk']">
          Hát hay – Giá hợp lý – Phục vụ tận tâm
        </p>
      </div>
    </section>
  );
};
