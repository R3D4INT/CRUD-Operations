using backend.DAL;
using Microsoft.EntityFrameworkCore;

namespace backend
{
    //public class Startup
    //{
    //    public Startup(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }

    //    public IConfiguration Configuration { get; }

    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddDbContext<AppDBContext>(options =>
    //            options.UseSqlServer(Configuration.GetConnectionString("HospitalDb")));

    //        DependencyRegistration.RegisterRepositories(services, Configuration);

    //        // Adding MVC services
    //        services.AddControllersWithViews();
    //    }
    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    //    {
    //        if (env.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //        }
    //        else
    //        {
    //            app.UseExceptionHandler("/Home/Error");
    //            app.UseHsts();
    //        }

    //        app.UseHttpsRedirection();
    //        app.UseStaticFiles();

    //        app.UseRouting();

    //        app.UseAuthorization();

    //        app.UseEndpoints(endpoints =>
    //        {
    //            endpoints.MapControllerRoute(
    //                name: "default",
    //                pattern: "{controller=Home}/{action=Index}/{id?}");
    //        });
    //    }
    //}
}
