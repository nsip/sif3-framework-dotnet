[assembly: WebActivator.PostApplicationStartMethod(typeof(Sif.Framework.EnvironmentProvider.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace Sif.Framework.EnvironmentProvider.App_Start
{
    using Controllers;
    using Model.Infrastructure;
    using Persistence;
    using Persistence.NHibernate;
    using Service.Authentication;
    using Service.Infrastructure;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using System.Web.Http;
    using Utils;

    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.Options.PropertySelectionBehavior = new PropertySelectionBehaviour<PropertyInjectAttribute>();


            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
       
            container.Verify();
            
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
     
        private static void InitializeContainer(Container container)
        {
            container.Register<IApplicationRegisterService, ApplicationRegisterService>(Lifestyle.Scoped);
            container.Register<IEnvironmentRepository, EnvironmentRepository>(Lifestyle.Scoped);
            container.Register<IEnvironmentService, EnvironmentService>(Lifestyle.Scoped);
            container.Register<IAuthenticationService, DirectAuthenticationService>(Lifestyle.Scoped);
            //container.RegisterConditional<IAuthenticationService, DirectAuthenticationService>(Lifestyle.Singleton, c => c.Consumer.ImplementationType == typeof(EnvironmentsController));
            //container.RegisterConditional<IAuthenticationService, BrokeredAuthenticationService>(Lifestyle.Singleton, c => EnvironmentType.BROKERED.Equals(SettingsManager.ProviderSettings.EnvironmentType));
            //container.RegisterConditional<IAuthenticationService, DirectAuthenticationService>(Lifestyle.Singleton, c => EnvironmentType.DIRECT.Equals(SettingsManager.ProviderSettings.EnvironmentType) || !c.Handled);
        }
    }
}