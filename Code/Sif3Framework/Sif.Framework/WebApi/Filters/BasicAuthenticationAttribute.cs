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

using Sif.Framework.Factory;
using Sif.Framework.Service.Authentication;
using Sif.Framework.WebApi.ActionResults;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Sif.Framework.WebApi.Filters
{

    /// <summary>
    /// Filter used for Basic authentication.
    /// This class is based on the article <a href="https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-filters">Authentication Filters in ASP.NET Web API 2</a>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// <see cref="IFilter.AllowMultiple"/>
        /// </summary>
        public bool AllowMultiple => false;

        /// <summary>
        /// Create an instance of this attribute.
        /// </summary>
        public BasicAuthenticationAttribute() : this(ServiceFactory.Create.AuthenticationService)
        {
        }

        /// <summary>
        /// Create an instance of this attribute.
        /// </summary>
        /// <param name="authenticationService">Authentication service.</param>
        protected BasicAuthenticationAttribute(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// <see cref="IAuthenticationFilter.AuthenticateAsync(HttpAuthenticationContext, CancellationToken)"/>
        /// </summary>
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {

            // 1. Look for credentials in the request.
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
            {
                return Task.CompletedTask;
            }

            // 3. If there are credentials but the filter does not recognize the 
            //    authentication scheme, do nothing.
            if (authorization.Scheme != "Basic")
            {
                return Task.CompletedTask;
            }

            // 4. If there are credentials that the filter understands, try to validate them.
            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return Task.CompletedTask;
            }

            // 5. If the credentials are bad, set the error result.
            if (!authenticationService.VerifyAuthenticationHeader(request.Headers))
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// <see cref="IAuthenticationFilter.ChallengeAsync(HttpAuthenticationChallengeContext, CancellationToken)"/>
        /// </summary>
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            AuthenticationHeaderValue challenge = new AuthenticationHeaderValue("Basic");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);

            return Task.CompletedTask;
        }

    }

}
