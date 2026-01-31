using QLQuanKaraokeHKT.Web.Extensions;
using QLQuanKaraokeHKT.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configure custom services
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomSwagger();
builder.Services.AddCustomCors(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.ConfigurePipeline();

app.Run();