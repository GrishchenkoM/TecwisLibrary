using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using BusinessLogic;
using BusinessLogic.Repositories;

namespace Web.IoC
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            // получаем экземпляр контейнера
            var builder = new ContainerBuilder();

            // регистрируем контроллер в текущей сборке
            builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // регистрируем споставление типов
            builder.RegisterType<DbDataContext>().As<DbDataContext>().WithParameter("DefaultConnection",
                                                   ConfigurationManager.ConnectionStrings[1].ConnectionString);

            builder.RegisterType<Authors>().As<AuthorRepository>();
            builder.RegisterType<Books>().As<BookRepository>();
            

            builder.RegisterType<DataManager>().As<IDataManager>();

            // создаем новый контейнер с теми зависимостями, которые определены выше
            var container = builder.Build();

            // установка сопоставителя зависимостей
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var apiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = apiResolver;
        }
    }
}