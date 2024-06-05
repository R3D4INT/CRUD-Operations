using AutoMapper.Extensions.ExpressionMapping;
using backend.DAL;
using backend.Repositories.Implementations;
using backend.Repositories.Interfaces;
using backend.Services.Implementations;
using backend.Services.Interfaces;
using backend.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork.Implementations.UnitOfWork>();

            builder.Services.AddAutoMapper(cfg => {
                cfg.AddExpressionMapping();
                cfg.AddProfile<MappingProfile>(); 
            }, typeof(Startup));

            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HospitalDb")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
