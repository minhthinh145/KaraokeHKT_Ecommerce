using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

// ✅ THÊM USING CHO BOOKING SYSTEM
using QLQuanKaraokeHKT.Core.Entities;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.HRM;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Room;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Customer;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Booking;
using QLQuanKaraokeHKT.Core.Interfaces.Repositories.Inventory;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Payment;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Auth;
using QLQuanKaraokeHKT.Core.Interfaces.Services.External;
using QLQuanKaraokeHKT.Core.Interfaces.Services.AccountManagement;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Booking;
using QLQuanKaraokeHKT.Core.Interfaces.Services.HRM;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Room;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Inventory;
using QLQuanKaraokeHKT.Core.Interfaces.Services.Customer;
using QLQuanKaraokeHKT.Infrastructure.Data;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Auth;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Booking;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Customer;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Room;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.HRM;
using QLQuanKaraokeHKT.Infrastructure.Repositories.Implementations.Inventory;
using QLQuanKaraokeHKT.Application.Services.External;
using QLQuanKaraokeHKT.Application.Services.Payment;
using QLQuanKaraokeHKT.Application.Services.Auth;
using QLQuanKaraokeHKT.Application.Services.AccountManagement;
using QLQuanKaraokeHKT.Application.Services.Booking;
using QLQuanKaraokeHKT.Application.Services.Room;
using QLQuanKaraokeHKT.Application.Services.Customer;
using QLQuanKaraokeHKT.Application.Services.HRM;
using QLQuanKaraokeHKT.Application.Services.Inventory;
using QLQuanKaraokeHKT.Application.Services.Background;
using QLQuanKaraokeHKT.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "KeyBoard API", Version = "v1" });

    // Định nghĩa security cho OpenAPI 2.0
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey // OpenAPI 2.0 yêu cầu type này
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

//Install ConnectionString
var connectionString = builder.Configuration.GetConnectionString("DeployConnection");

builder.Services.AddDbContext<QlkaraokeHktContext>(options =>
    options.UseSqlServer(connectionString));

//Identity
builder.Services.AddIdentity<TaiKhoan, VaiTro>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<QlkaraokeHktContext>()
.AddDefaultTokenProviders();

//Authen
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option => {
    option.SaveToken = true;
    option.RequireHttpsMetadata = false;
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});


//
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVercelAndBackend", policy =>
    {
        policy.WithOrigins(
            "https://ooad-karaoke-hkt-3kgx.vercel.app",
            "https://ooad-karaoke-hkt-3kgx-git-main-minhthinh145s-projects.vercel.app",
            "https://qlquankaraokehktt.runasp.net" // thêm origin này
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Cho phép tất cả các nguồn
               .AllowAnyMethod()
         .AllowAnyHeader();

    });
    options.AddPolicy("AllowVercelAndBackend", policy =>
    {
        policy.WithOrigins(
            "https://ooad-karaoke-hkt-3kgx.vercel.app",
            "https://ooad-karaoke-hkt-3kgx-git-main-minhthinh145s-projects.vercel.app",
            "https://qlquankaraokehktt.runasp.net" // thêm origin này
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});



// Fix AutoMapper registration - use the actual ApplicationMapper profile
builder.Services.AddAutoMapper(typeof(ApplicationMapper));

builder.Services.Configure<VNPayConfig>(builder.Configuration.GetSection("VNPay"));

//Dependencies
// Authentication Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVerifyAuthService, VerifyAuthService>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();

//ChangePassword Service
builder.Services.AddScoped<IChangePasswordService, ChangePasswordService>();

// Account Services
builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();
builder.Services.AddScoped<IQLNhanSuService, QLNhanSuService>();
builder.Services.AddScoped<IAccountBaseService, AccountBaseService>();
builder.Services.AddScoped<IAdminAccountService, AdminAccountService>();
builder.Services.AddScoped<INhanVienAccountService, NhanVienAccountService>();
builder.Services.AddScoped<IKhachHangAccountService, KhachHangAccountService>();
builder.Services.AddScoped<IAccountManagementService, AccountManagementService>();
builder.Services.AddScoped<IQLCaLamViecService, QLCaLamViecService>();
builder.Services.AddScoped<IQuanLyTienLuongService, QuanLyTienLuongService>();
builder.Services.AddScoped<IQLLichLamViecService, QLLichLamViecService>();
builder.Services.AddScoped<IQLVatLieuService, QLVatLieuService>();
builder.Services.AddScoped<IQLLoaiPhongService, QLLoaiPhongService>();
builder.Services.AddScoped<IQLPhongHatService, QLPhongHatService>();
builder.Services.AddScoped<IQLYeuCauChuyenCaService, QLYeuCauChuyenCaService>();

// Repositories
builder.Services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IMaOtpRepository, MaOtpRepository>();
builder.Services.AddScoped<IKhacHangRepository, KhacHangRepository>();
builder.Services.AddScoped<INhanVienRepository, NhanVienRepository>();
builder.Services.AddScoped<ITaiKhoanQuanLyRepository, TaiKhoanQuanLyRepository>();
builder.Services.AddScoped<ICaLamViecRepository, CaLamViecRepository>();
builder.Services.AddScoped<ILuongCaLamViecRepository, LuongCaLamViecRepository>();
builder.Services.AddScoped<ILichLamViecRepository, LichLamViecRepository>();
builder.Services.AddScoped<IVatLieuRepository, VatLieuRepository>();
builder.Services.AddScoped<ISanPhamDichVuRepository, SanPhamDichVuRepository>();
builder.Services.AddScoped<IMonAnRepository, MonAnRepository>();
builder.Services.AddScoped<IGiaVatLieuRepository, GiaVatLieuRepository>();
builder.Services.AddScoped<IGiaDichVuRepository, GiaDichVuRepository>();
builder.Services.AddScoped<IPhongHatRepository, PhongHatRepository>();
builder.Services.AddScoped<ILoaiPhongRepository, LoaiPhongRepository>();
builder.Services.AddScoped<IYeuCauChuyenCaRepository, YeuCauChuyenCaRepository>();
builder.Services.AddScoped<ILichSuSuDungPhongRepository, LichSuSuDungPhongRepository>();
builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddScoped<IThuePhongRepository, ThuePhongRepository>();

builder.Services.AddScoped<IKhachHangDatPhongService, KhachHangDatPhongService>();
builder.Services.AddScoped<IVNPayService, VNPayService>();

builder.Services.AddScoped<IMaOtpService, MaOtpService>();

builder.Services.AddScoped<ISendEmailService, SendEmailService>();

builder.Services.AddHostedService<AutoBookingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors("AllowAllOrigins");
app.UseCors("AllowAllOrigins"); 
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();