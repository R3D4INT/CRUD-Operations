using backend.Repositories.Implementations;
using backend.Repositories.Interfaces;
using backend.Services.Implementations;
using backend.Services.Interfaces;

namespace backend
{
    public static class DependencyRegistration
    {
        public static void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(ICountryRepository), typeof(CountryRepository));
        }
    }
}
