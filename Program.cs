using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIS.Errors;
using Talabat.APIS.Extensions;
using Talabat.APIS.Helpers;
using Talabat.APIS.MiddleWares;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIS
{
    public class Program
    {
        public static async Task  Main(string[] args)
        {
            Console.WriteLine("Hello Div");
            Console.WriteLine("Hello Session01");
            var builder = WebApplication.CreateBuilder(args);
            #region Configure Services
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<APIDbContext>(optios=>
            {
                optios.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });
            // builder.Services.AddScoped<IGenericRepository<Product>,GenericRepository<Product>>();

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

            });


            builder.Services.AddApplicationServices();
            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });
            #endregion



            var app = builder.Build();


            #region UpdateDatabase

           using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var LoggerFactory= Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var DbContext = Services.GetRequiredService<APIDbContext>();
                await DbContext.Database.MigrateAsync();
                await DbContextSeed.SeedAsync(DbContext);

                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync();
            }
            catch (Exception ex) {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occure During Applying Migration");
            }
           

            #endregion


            #region Kestrol Piplines
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>();
                app.UseSwaggerMiddleware();

            }
            app.UseStaticFiles();
           // app.UseStatusCodePagesWithRedirects("/errors/{0}");
           

            app.UseHttpsRedirection();

            app.UseAuthorization();
            


            app.MapControllers();

            #endregion


            app.Run();
        }
    }
}
