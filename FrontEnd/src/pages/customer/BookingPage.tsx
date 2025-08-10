import React from "react";

export const BookingPage: React.FC = () => {
  return (
    <div className="min-h-screen bg-gray-50">
      <div className="container mx-auto px-4 py-8">
        <div className="bg-white rounded-lg shadow-sm p-6">
          <h1 className="text-2xl font-bold text-gray-800 mb-6">
            Đặt phòng karaoke 🎵
          </h1>

          <div className="text-center py-16">
            <div className="text-6xl mb-4">🎤</div>
            <h2 className="text-xl font-semibold text-gray-700 mb-2">
              Chức năng đặt phòng
            </h2>
            <p className="text-gray-500">
              Tính năng đặt phòng karaoke sẽ sớm có. Bạn sẽ có thể chọn phòng,
              thời gian và thanh toán online.
            </p>
            <div className="mt-6">
              <div className="inline-flex items-center px-4 py-2 bg-blue-50 rounded-lg">
                <span className="text-blue-600 font-medium">
                  🚧 Đang phát triển...
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
