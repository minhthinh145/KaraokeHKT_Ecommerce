# Hệ thống Quản lý Quán Karaoke HKT

Hệ thống quản lý toàn diện cho quán karaoke, hỗ trợ đặt phòng trực tuyến, quản lý nhân sự, kho hàng và thanh toán điện tử.

## 📋 Tổng quan

**QL Karaoke HKT** là ứng dụng web full-stack được phát triển theo mô hình Clean Architecture, phục vụ nhu cầu vận hành và quản lý quán karaoke một cách chuyên nghiệp.

### Tính năng chính

**Dành cho khách hàng:**
- Đăng ký tài khoản, xác thực OTP qua email
- Xem danh sách phòng khả dụng theo loại phòng
- Đặt phòng trực tuyến với thanh toán VNPay
- Theo dõi lịch sử đặt phòng và trạng thái thanh toán

**Dành cho quản trị viên:**
- Quản lý phòng hát (thêm, sửa, xóa, cập nhật giá)
- Quản lý nhân sự (nhân viên, ca làm việc, lịch làm việc, tính lương)
- Quản lý kho (vật liệu, nhập xuất kho)
- Quản lý hệ thống (tài khoản, phân quyền)

## 🛠️ Công nghệ sử dụng

### Backend
| Công nghệ | Mô tả |
|-----------|-------|
| .NET 8.0 | Framework chính |
| Entity Framework Core | ORM |
| SQL Server | Cơ sở dữ liệu |
| JWT | Xác thực và phân quyền |
| AutoMapper | Mapping DTO |
| VNPay | Cổng thanh toán |
| MailKit | Gửi email OTP |

### Frontend
| Công nghệ | Mô tả |
|-----------|-------|
| React 19 | UI Library |
| TypeScript | Ngôn ngữ lập trình |
| Vite | Build tool |
| Redux Toolkit | State management |
| Ant Design | UI Components |
| Tailwind CSS | Styling |
| Axios | HTTP Client |

## 📁 Cấu trúc dự án

```
├── Backend/
│   ├── QLQuanKaraokeHKT.Presentation/    # API Controllers, Program.cs
│   ├── QLQuanKaraokeHKT.Application/     # DTOs, Services, Repositories interfaces
│   ├── QLQuanKaraokeHKT.Domain/          # Entities
│   ├── QLQuanKaraokeHKT.Infrastructure/  # DbContext, Migrations, Repository implementations
│   ├── QLQuanKaraokeHKT.Shared/          # Constants, Enums, Extensions
│   └── QLQuanKaraokeHKT.UnitTests/       # Unit tests
│
├── FrontEnd/
│   └── src/
│       ├── api/          # API calls và Axios config
│       ├── components/   # React components
│       ├── pages/        # Các trang (admin, customer, employees)
│       ├── redux/        # Redux slices và thunks
│       ├── hooks/        # Custom hooks
│       └── routes/       # Route definitions
│
├── docker-compose.yml    # Docker orchestration
└── README.md
```

## 🚀 Hướng dẫn cài đặt

### Yêu cầu hệ thống
- Docker và Docker Compose
- Node.js 20+ (nếu chạy FE riêng)
- .NET 8.0 SDK (nếu chạy BE riêng)

### Chạy với Docker (khuyến nghị)

```bash
# Clone repository
git clone https://github.com/minhthinh145/KaraokeHKT_Ecommerce.git
cd KaraokeHKT_Ecommerce

# Khởi động tất cả services
docker compose up --build -d

# Xem logs
docker compose logs -f
```

Sau khi khởi động:
- Frontend: http://localhost:3000
- Backend API: http://localhost:8080
- Swagger: http://localhost:8080/swagger
- SQL Server: localhost:1433

### Chạy thủ công

**Backend:**
```bash
cd Backend/QLQuanKaraokeHKT.Presentation
dotnet restore
dotnet run
```

**Frontend:**
```bash
cd FrontEnd
npm install
npm run dev
```

## ⚙️ Cấu hình

### Backend (appsettings.json)
- `ConnectionStrings:DefaultConnection` - Connection string SQL Server
- `JWT:Secret` - Secret key cho JWT
- `VNPay` - Cấu hình cổng thanh toán VNPay
- `EmailSettings` - Cấu hình SMTP để gửi email

### Frontend (.env)
```env
VITE_API_URL=http://localhost:8080/api
```

## 📚 API Endpoints

| Module | Endpoint | Mô tả |
|--------|----------|-------|
| Auth | `/api/Auth/*` | Đăng nhập, đăng ký, refresh token |
| Booking | `/api/KhachHangBooking/*` | Đặt phòng, lịch sử, thanh toán |
| Room | `/api/PhongHat/*`, `/api/LoaiPhong/*` | Quản lý phòng hát |
| HRM | `/api/NhanVien/*`, `/api/CaLamViec/*` | Quản lý nhân sự |
| Inventory | `/api/VatLieu/*` | Quản lý kho |
| Payment | `/api/VNPay/*` | Thanh toán VNPay |

Chi tiết API xem tại Swagger: http://localhost:8080/swagger



## 📄 License

MIT License
