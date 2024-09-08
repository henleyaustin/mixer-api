namespace mixer_api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {

            app.UseHttpsRedirection();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Use the CORS policy before any request handling
            app.UseCors("AllowFirebaseAndLocal");

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
