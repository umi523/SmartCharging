using Microsoft.EntityFrameworkCore;
using SmartCharging.Configurations;
using SmartChargingService.Implementations;
using SmartChargingService.Interfaces;
using SmartCharginRepository.DatabaseContext;
using SmartCharginRepository.Implementations;
using SmartCharginRepository.Interfaces;
using System.Text.Json.Serialization;

namespace SmartCharging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SmartCharginDbContext>(
                options => options.UseInMemoryDatabase(databaseName: "SmartCharginDatabase"));

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.MaxDepth = 32; // Adjust max depth as needed
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<IConnectorService, ConnectorService>();
            builder.Services.AddScoped<IConnectorRepository, ConnectorRepository>();
            builder.Services.AddScoped<IChargeStationService, ChargeStationService>();
            builder.Services.AddScoped<IChargeStationRepository, ChargeStationRepository>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SmartCharginDbContext>();
                SmartChargingDataSeed.Initialize(context);
            }

            app.Run();
        }
    }
}
