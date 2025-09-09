namespace QLQuanKaraokeHKT.Web.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration )
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowVercelAndBackend", policy =>
                {
                    policy.WithOrigins(
                        configuration.GetValue<string>("DeployUrl:FrontendUrl")!,
                        "https://qlquankaraokehktt.runasp.net"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });

                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}