﻿/*
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

using Sif.Framework.Model.Exceptions;
using Sif.Framework.Model.Infrastructure;
using Sif.Framework.Service.Infrastructure;
using Sif.Framework.Service.Mapper;
using Sif.Framework.Utils;
using Sif.Specification.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Sif.Framework.Service.Authorisation
{

    /// <summary>
    /// <see cref="IAuthorisationService">IAuthorisationService</see>
    /// </summary>
    public class AuthorisationService : IAuthorisationService
    {
        private readonly IEnvironmentService environmentService;

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="environmentService">Environment service.</param>
        public AuthorisationService(IEnvironmentService environmentService)
        {
            this.environmentService = environmentService;
        }

        /// <summary>
        /// <see cref="IAuthorisationService.IsAuthorised(HttpRequestHeaders, string, string, RightType, RightValue, string)">IsAuthorised</see>
        /// </summary>
        public virtual bool IsAuthorised(HttpRequestHeaders headers,
            string sessionToken,
            string serviceName,
            RightType permission,
            RightValue privilege = RightValue.APPROVED,
            string zoneId = null)
        {
            bool isAuthorised = true;
            environmentType retrievedEnvironment = environmentService.RetrieveBySessionToken(sessionToken);
            Environment environment = MapperFactory.CreateInstance<environmentType, Environment>(retrievedEnvironment);

            if (environment == null)
            {
                throw new InvalidSessionException("Session token does not have an associated environment definition.");
            }

            Right operationPolicy = new Right(permission, privilege);
            string serviceType = HttpUtils.GetHeaderValue(headers, "serviceType");

            // Retrieving permissions for requester.
            IDictionary<string, Right> requesterPermissions = GetRightsForService(serviceType, serviceName, EnvironmentUtils.GetTargetZone(environment, zoneId));

            if (requesterPermissions == null)
            {
                isAuthorised = false;
            }
            else
            {

                // Checking the appropriate rights.
                try
                {
                    RightsUtils.CheckRight(requesterPermissions, operationPolicy);
                }
                catch (RejectedException)
                {
                    isAuthorised = false;
                }

            }

            return isAuthorised;
        }

        /// <summary>
        /// Retrieve the rights for a service in a given zone.
        /// </summary>
        /// <param name="serviceType">The service type used for the request <see cref="ServiceType"/> for valid types.</param>
        /// <param name="serviceName">The service name.</param>
        /// <param name="zone">The zone to retrieve the rights for.</param>
        /// <returns>An array of declared rights</returns>
        private IDictionary<string, Right> GetRightsForService(string serviceType, string serviceName, ProvisionedZone zone)
        {
            Model.Infrastructure.Service service =
                (from Model.Infrastructure.Service s in zone.Services
                 where s.Type.Equals(serviceType) && s.Name.Equals(serviceName)
                 select s).FirstOrDefault();

            return service?.Rights;
        }

    }

}
