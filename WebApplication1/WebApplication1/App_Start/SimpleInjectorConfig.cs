using back;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace WebApplication1.App_Start
{
    public static class SimpleInjectorConfig
    {
        public static void RegisterDependencies()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Регистрация ваших репозиториев
            container.Register<IUserRepository, UserRepository>(Lifestyle.Scoped);

            // Регистрация DbContext
            container.Register<AppDBContext>(Lifestyle.Scoped);

            // Регистрация контроллеров MVC
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // Валидация конфигурации
            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}