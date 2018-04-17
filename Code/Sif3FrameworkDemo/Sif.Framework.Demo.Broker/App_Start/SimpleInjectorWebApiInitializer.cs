/*
 * Copyright 2018 Systemic Pty Ltd
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Sif.Framework.Persistence;
using Sif.Framework.Persistence.NHibernate;
using Sif.Framework.Service.Infrastructure;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Web.Http;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Sif.Framework.Demo.Broker.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace Sif.Framework.Demo.Broker.App_Start
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
            container.Register<IBaseSessionFactory>(() => EnvironmentProviderSessionFactory.Instance, Lifestyle.Scoped);
            container.Register<IEnvironmentRegisterRepository, EnvironmentRegisterRepository>(Lifestyle.Scoped);
            container.Register<IEnvironmentRegisterService, EnvironmentRegisterService>(Lifestyle.Scoped);
            container.Register<IEnvironmentRepository, EnvironmentRepository>(Lifestyle.Scoped);
            container.Register<IEnvironmentService, EnvironmentService>(Lifestyle.Scoped);
        }

    }

}