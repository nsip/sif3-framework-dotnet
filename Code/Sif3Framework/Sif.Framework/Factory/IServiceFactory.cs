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

using Sif.Framework.Service.Authentication;
using Sif.Framework.Service.Authorisation;

namespace Sif.Framework.Factory
{

    /// <summary>
    /// Factory interface for the creation of services.
    /// </summary>
    interface IServiceFactory
    {

        /// <summary>
        /// Instance of an IAuthenticationService.
        /// </summary>
        IAuthenticationService AuthenticationService { get; }

        /// <summary>
        /// Instance of an IAuthorisationService.
        /// </summary>
        /// <param name="authorisationScheme">Scheme used for authorisation, i.e. Basic, SIF_HMACSHA256.</param>
        /// <returns>Instance of an IAuthorisationService.</returns>
        /// <exception cref="System.ArgumentNullException">authorisationScheme is null or empty.</exception>"
        IAuthorisationService AuthorisationService(string authorisationScheme);

        /// <summary>
        /// Instance of an IAuthorisationTokenService.
        /// </summary>
        /// <param name="authorisationScheme">Scheme used for authorisation, i.e. Basic, SIF_HMACSHA256.</param>
        /// <returns>Instance of an IAuthorisationTokenService.</returns>
        /// <exception cref="System.ArgumentNullException">authorisationScheme is null or empty.</exception>"
        IAuthorisationTokenService AuthorisationTokenService(string authorisationScheme);

    }

}
