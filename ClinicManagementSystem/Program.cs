using ClinicManagementSystem.backend.Common.Extensions;
using ClinicManagementSystem.backend.Features.Doctors.DependencyInjection;
using ClinicManagementSystem.backend.Middleware;
using System.Text.Json.Serialization;

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
            builder.Services.AddAuthenticationServices(builder.Configuration);
            builder.Services.AddAuthorizationServices();
            builder.Services.AddGoogleAuthentication(builder.Configuration);

            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddApplicationServices();

            //AddSwagger service
            //builder.Services.AddApplicationSwagger();
            builder.Services.AddSwaggerServices();


            //Add application services and repositories using the extension method
            builder.Services.AddApplicationServices();



            //Configure feature application services
            builder.Services.AddApplicationServices();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
            });

            // Add global exception handling
            builder.Services.AddGlobalExceptionHandling();

            // Add rate limiting
            builder.Services.AddApplicationRateLimiting();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("Frontend");

            app.UseAuthentication();


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
