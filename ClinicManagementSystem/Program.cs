using ClinicManagementSystem.backend.Common.Extensions;
using ClinicManagementSystem.backend.Features.Doctors.DependencyInjection;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using ClinicManagementSystem.backend.Features.Doctors.Repositories;
using ClinicManagementSystem.backend.Features.Doctors.Services;
using ClinicManagementSystem.backend.Features.Doctors.Validators;

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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            // Seed the database with initial data
            await app.SeedDatabaseAsync();

            app.Run();
        }
    }
}
