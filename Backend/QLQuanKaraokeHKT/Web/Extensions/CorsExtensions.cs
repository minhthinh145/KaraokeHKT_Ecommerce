namespace QLQuanKaraokeHKT.Web.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowVercelAndBackend", policy =>
                {
                    policy.WithOrigins(
                        "https://ooad-karaoke-hkt-3kgx.vercel.app",
                        "https://ooad-karaoke-hkt-3kgx-git-main-minhthinh145s-projects.vercel.app",
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