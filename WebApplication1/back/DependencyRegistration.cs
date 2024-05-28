using System.Runtime.Remoting.Contexts;
using System.Web.Services.Description;
using Microsoft.Extensions.DependencyInjection;

namespace back
{
    public class DependencyRegistration
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}