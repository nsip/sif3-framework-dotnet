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

using Sif.Framework.Model.Authentication;
using Sif.Framework.Model.Infrastructure;
using Sif.Framework.Persistence;
using Sif.Framework.Persistence.NHibernate;
using Sif.Framework.Service.Authentication;
using Sif.Framework.Service.Authorisation;
using Sif.Framework.Service.Infrastructure;
using Sif.Framework.Utils;
using System;

namespace Sif.Framework.Factory
{

    /// <summary>
    /// Factory class for the creation of services.
    /// </summary>
    class ServiceFactory : IServiceFactory
    {
        private static IServiceFactory serviceFactory;

        /// <summary>
        /// Instance of IApplicationRegisterRepository.
        /// </summary>
        private IApplicationRegisterRepository ApplicationRegisterRepository => new ApplicationRegisterRepository(SessionFactory);

        /// <summary>
        /// Instance of IApplicationRegisterService.
        /// </summary>
        private IApplicationRegisterService ApplicationRegisterService => new ApplicationRegisterService(ApplicationRegisterRepository);

        /// <summary>
        /// <see cref="IServiceFactory.AuthenticationService"/>
        /// </summary>
        public IAuthenticationService AuthenticationService
        {

            get
            {

                if (EnvironmentType.DIRECT.Equals(SettingsManager.ProviderSettings.EnvironmentType))
                {
                    return new DirectAuthenticationService(ApplicationRegisterService, EnvironmentService);
                }
                else if (EnvironmentType.BROKERED.Equals(SettingsManager.ProviderSettings.EnvironmentType))
                {
                    return new BrokeredAuthenticationService(ApplicationRegisterService, EnvironmentService);
                }
                else
                {
                    return new DirectAuthenticationService(ApplicationRegisterService, EnvironmentService);
                }

            }

        }

        /// <summary>
        /// <see cref="IServiceFactory.AuthorisationService(string)"/>
        /// </summary>
        public IAuthorisationService AuthorisationService(string authorisationScheme)
        {
            return new AuthorisationService(AuthorisationTokenService(authorisationScheme), EnvironmentService);
        }

        /// <summary>
        /// <see cref="IServiceFactory.AuthorisationTokenService(string)"/>
        /// </summary>
        public IAuthorisationTokenService AuthorisationTokenService(string authorisationScheme)
        {

            if (string.IsNullOrWhiteSpace(authorisationScheme))
            {
                throw new ArgumentNullException("authorisationScheme");
            }

            if (AuthenticationMethod.Basic.ToString().Equals(authorisationScheme, StringComparison.OrdinalIgnoreCase))
            {
                return new BasicAuthorisationTokenService();
            }
            else if (AuthenticationMethod.SIF_HMACSHA256.ToString().Equals(authorisationScheme, StringComparison.OrdinalIgnoreCase))
            {
                return new HmacShaAuthorisationTokenService();
            }
            else
            {
                return new BasicAuthorisationTokenService();
            }

        }

        /// <summary>
        /// Singleton instance of this class.
        /// </summary>
        public static IServiceFactory Create
        {

            get
            {

                if (serviceFactory == null)
                {
                    serviceFactory = new ServiceFactory();
                }

                return serviceFactory;
            }

        }

        /// <summary>
        /// Instance of IEnvironmentRegisterRepository.
        /// </summary>
        private IEnvironmentRegisterRepository EnvironmentRegisterRepository => new EnvironmentRegisterRepository(SessionFactory);

        /// <summary>
        /// Instance of IEnvironmentRegisterService.
        /// </summary>
        private IEnvironmentRegisterService EnvironmentRegisterService => new EnvironmentRegisterService(EnvironmentRegisterRepository);

        /// <summary>
        /// Instance of IEnvironmentRepository.
        /// </summary>
        private IEnvironmentRepository EnvironmentRepository => new EnvironmentRepository(SessionFactory);

        /// <summary>
        /// Instance of IEnvironmentService.
        /// </summary>
        private IEnvironmentService EnvironmentService => new EnvironmentService(EnvironmentRepository, EnvironmentRegisterService);

        /// <summary>
        /// Instance of IBaseSessionFactory.
        /// </summary>
        private IBaseSessionFactory SessionFactory => EnvironmentProviderSessionFactory.Instance;

        /// <summary>
        /// Private constructor to ensure instantiation as a Singleton.
        /// </summary>
        private ServiceFactory()
        {
        }

    }

}
