using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend
{
    public static class DependencyRegistration
    {
        public static void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
        }
    }
}
