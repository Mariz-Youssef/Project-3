using ClinicManagementSystem.backend.Common.Extensions;
using ClinicManagementSystem.backend.Common.RateLimiting;
using ClinicManagementSystem.backend.Features.Doctors.DependencyInjection;

namespace ClinicManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Configure the database services using the extension method
            builder.Services.AddDatabaseServices(builder.Configuration);

            //Configure the identity services using the extension method
            builder.Services.AddIdentityServices();
            builder.Services.AddMapping();
            builder.Services.AddValidation();
            builder.Services.AddDoctorFeature();

            //AddSwagger service
            builder.Services.AddApplicationSwagger();

            //Add application services and repositories using the extension method
            builder.Services.AddApplicationServices();



            builder.Services.AddControllers();

            // Add global exception handling
            builder.Services.AddGlobalExceptionHandling();

            // Add rate limiting
            builder.Services.AddApplicationRateLimiting();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use the global exception handler middleware
            app.UseExceptionHandler();

            // Use the rate limiting middleware
            app.UseRateLimiter();

            //Use Swagger
            app.UseApplicationSwagger();

            app.UseAuthorization();


            app.MapControllers();

            // Seed the database with initial data
          await app.SeedDatabaseAsync();

            app.Run();
        }
    }
}
