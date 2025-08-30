using QLQuanKaraokeHKT.Web.Extensions;
using QLQuanKaraokeHKT.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure custom services
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomSwagger();
builder.Services.AddCustomCors();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the request pipeline
app.ConfigurePipeline();

app.Run();