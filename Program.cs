using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Talabat.APIS.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;

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
           builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            //builder.Services.AddAutoMapper(m=>m.AddProfile(new MappingProfiles()));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
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
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            #endregion


            app.Run();
        }
    }
}
