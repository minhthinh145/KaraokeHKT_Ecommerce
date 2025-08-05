import React, { useState } from "react";
import { Button } from "../../ui/Button";
import { FaFacebookF, FaInstagram } from "react-icons/fa";

export const Footer: React.FC = () => {
  const [email, setEmail] = useState("");

  const handleNewsletterSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Newsletter signup:", email);
    setEmail("");
  };

  return (
    <footer className="bg-zinc-800 text-white">
      <div className="container mx-auto px-8 py-12">
        {/* Main Footer Content */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 mb-8">
          {/* Company Info */}
          <div className="space-y-4">
            <h3 className="text-xl font-bold font-['Space_Grotesk']">
              Karaoke HKT
            </h3>
            <div className="text-sm leading-relaxed space-y-2">
              <p>
                🎤 Trải nghiệm âm nhạc đỉnh cao cùng không gian hiện đại và dịch
                vụ chuyên nghiệp.
              </p>
              <p>
                <strong>Địa chỉ:</strong> 519-521 Sư Vạn Hạnh, Phường 12, Quận
                10, Hồ Chí Minh
              </p>
              <p>
                <strong>Hotline:</strong> 0909 123 456
              </p>
              <p>
                <strong>Email:</strong> info@karaokehkt.vn
              </p>
            </div>
          </div>

          {/* Newsletter */}
          <div className="space-y-4">
            <h3 className="text-xl font-bold font-['Space_Grotesk']">
              Đăng ký nhận tin khuyến mãi
            </h3>
            <form onSubmit={handleNewsletterSubmit} className="space-y-3">
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="Nhập email của bạn"
                className="w-full bg-zinc-700 text-white placeholder-zinc-400 px-4 py-2 rounded-lg border border-zinc-600 focus:border-indigo-500 focus:outline-none"
                required
              />
              <Button
                type="submit"
                variant="primary"
                className="w-full bg-indigo-700 text-white py-2 rounded-lg hover:bg-indigo-800 transition-colors"
              >
                Gửi
              </Button>
            </form>
          </div>

          {/* Social Media */}
          <div className="space-y-4">
            <h3 className="text-xl font-bold font-['Space_Grotesk']">
              Theo dõi chúng tôi
            </h3>
            <div className="flex space-x-4">
              <a
                href="https://facebook.com/"
                target="_blank"
                rel="noopener noreferrer"
                className="w-10 h-10 bg-zinc-700 rounded-lg flex items-center justify-center cursor-pointer hover:bg-blue-600 transition-colors"
                aria-label="Facebook"
              >
                <FaFacebookF className="text-white text-xl" />
              </a>
              <a
                href="https://instagram.com/"
                target="_blank"
                rel="noopener noreferrer"
                className="w-10 h-10 bg-zinc-700 rounded-lg flex items-center justify-center cursor-pointer hover:bg-pink-500 transition-colors"
                aria-label="Instagram"
              >
                <FaInstagram className="text-white text-xl" />
              </a>
            </div>
          </div>
        </div>

        {/* Copyright */}
        <div className="border-t border-zinc-700 pt-6 text-center">
          <p className="text-sm text-zinc-400">
            © 2025 Hệ thống được lập trình bởi team HKT
          </p>
        </div>
      </div>
    </footer>
  );
};
