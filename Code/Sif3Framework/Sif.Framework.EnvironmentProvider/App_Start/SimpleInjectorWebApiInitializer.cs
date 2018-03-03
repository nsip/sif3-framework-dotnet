using System.Web.Http;
using Sif.Framework.Persistence;
using Sif.Framework.Persistence.NHibernate;
using Sif.Framework.Service.Authentication;
using Sif.Framework.Service.Infrastructure;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Sif.Framework.EnvironmentProvider.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace Sif.Framework.EnvironmentProvider.App_Start
{

    /// <summary>
    /// Class used to configure and initialise Dependency Injection (DI) using SimpleInjector.
    /// </summary>
    public static class SimpleInjectorWebApiInitializer
    {

        /// <summary>
        /// Initialize the container and register it as Web API Dependency Resolver.
        /// </summary>
        public static void Initialize()
        {
            // Create the DI container.
            Container container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // Register types.
            InitializeContainer(container);

            // Extension method from the integration package used to register Web API Controllers.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }

        /// <summary>
        /// Register types to be injected.
        /// </summary>
        /// <param name="container">DI container.</param>
        private static void InitializeContainer(Container container)
        {
            container.Register<IApplicationRegisterRepository, ApplicationRegisterRepository>(Lifestyle.Scoped);
            container.Register<IApplicationRegisterService, ApplicationRegisterService>(Lifestyle.Scoped);
            container.Register<IAuthenticationService, DirectAuthenticationService>(Lifestyle.Scoped);
            container.Register<IBaseSessionFactory>(() => EnvironmentProviderSessionFactory.Instance, Lifestyle.Scoped);
            container.Register<IEnvironmentRegisterRepository, EnvironmentRegisterRepository>(Lifestyle.Scoped);
            container.Register<IEnvironmentRegisterService, EnvironmentRegisterService>(Lifestyle.Scoped);
            container.Register<IEnvironmentRepository, EnvironmentRepository>(Lifestyle.Scoped);
            container.Register<IEnvironmentService, EnvironmentService>(Lifestyle.Scoped);
        }

    }

}