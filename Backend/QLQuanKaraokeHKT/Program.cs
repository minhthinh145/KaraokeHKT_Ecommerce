using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QLQuanKaraokeHKT.AuthenticationService;
using QLQuanKaraokeHKT.AuthenticationService.ChangePassword;
using QLQuanKaraokeHKT.ExternalService.Implementation;
using QLQuanKaraokeHKT.ExternalService.Interfaces;
using QLQuanKaraokeHKT.Helpers;
using QLQuanKaraokeHKT.Models;
using QLQuanKaraokeHKT.Repositories.Implementation;
using QLQuanKaraokeHKT.Repositories.Interfaces;
using QLQuanKaraokeHKT.Repositories.TaiKhoanRepo;
using QLQuanKaraokeHKT.Services.Implementation;
using QLQuanKaraokeHKT.Services.Interfaces;
using QLQuanKaraokeHKT.Services.QLHeThongServices;
using QLQuanKaraokeHKT.Services.QLHeThongServices.Implementation;
using QLQuanKaraokeHKT.Services.QLHeThongServices.Interface;
using QLQuanKaraokeHKT.Services.TaiKhoanService;
using System.Text;

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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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

//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Cho phép tất cả các nguồn
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Fix AutoMapper registration - use the actual ApplicationMapper profile
builder.Services.AddAutoMapper(typeof(ApplicationMapper));

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


// Repositories
builder.Services.AddScoped<ITaiKhoanRepository, TaiKhoanRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IMaOtpRepository, MaOtpRepository>();
builder.Services.AddScoped<IKhacHangRepository, KhacHangRepository>();
builder.Services.AddScoped<INhanVienRepository, NhanVienRepository>();
builder.Services.AddScoped<ITaiKhoanQuanLyRepository, TaiKhoanQuanLyRepository>();


// OTP Services
builder.Services.AddScoped<IMaOtpService, MaOtpService>();
// External Services
builder.Services.AddScoped<ISendEmailService, SendEmailService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();