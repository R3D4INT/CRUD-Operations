using System.Globalization;
using AutoMapper.Extensions.ExpressionMapping;
using backend.BackGroundJob;
using backend.DAL;
using backend.Repositories.Implementations;
using backend.Repositories.Interfaces;
using backend.Services.Implementations;
using backend.Services.Interfaces;
using backend.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("EN"),
                    new CultureInfo("IT")
                };

                options.DefaultRequestCulture = new RequestCulture("EN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork.Implementations.UnitOfWork>();

            builder.Services.AddScoped<ICountryRepository, CountryRepository>();

            builder.Services.AddScoped<ICountryService, CountryService>();

            builder.Services.AddAutoMapper(cfg => {
                cfg.AddExpressionMapping();
                cfg.AddProfile<MappingProfile>(); 
            }, typeof(Program));

            builder.Services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();

                var jobKeyForClearOldUsers = JobKey.Create(nameof(ClearOldUsersInDatabaseJob));
                var jobKeyForImportCountries = JobKey.Create(nameof(ImportCountriesFromExcelJob));

                options
                    .AddJob<ClearOldUsersInDatabaseJob>(jobKeyForClearOldUsers)
                    .AddTrigger(trigger => trigger.ForJob(jobKeyForClearOldUsers)
                        .WithSimpleSchedule(schedule => schedule
                            .WithIntervalInMinutes(5)));
                options
                    .AddJob<ImportCountriesFromExcelJob>(jobKeyForImportCountries)
                    .AddTrigger(trigger => trigger.ForJob(jobKeyForImportCountries)
                        .WithSimpleSchedule(schedule => schedule
                            .WithRepeatCount(0)).StartAt(DateTimeOffset.Now));
            });

            builder.Services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HospitalDb")));

            var app = builder.Build();

            app.UseRequestLocalization();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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
