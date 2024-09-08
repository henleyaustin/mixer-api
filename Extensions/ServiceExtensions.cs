namespace mixer_api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFirebaseAndLocal",
                    policy => policy
                        .WithOrigins(
                            "https://web-mixer.web.app",
                            "https://web-mixer.firebaseapp.com",
                            "http://localhost:4200"
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            // Register AudioManager with DI
            services.AddScoped<AudioManager>();
        }
    }
}
