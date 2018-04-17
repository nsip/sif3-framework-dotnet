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

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sif.Framework.WebApi.ActionResults
{

    /// <summary>
    /// Action result that represents a challenge on authentication failure.
    /// This class is based on the article <a href="https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-filters">Authentication Filters in ASP.NET Web API 2</a>.
    /// </summary>
    class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {

        /// <summary>
        /// Challenge to be returned on authentication failure.
        /// </summary>
        public AuthenticationHeaderValue Challenge { get; private set; }


        /// <summary>
        /// Original action result from authentication failure that the challenge will be returned with.
        /// </summary>
        public IHttpActionResult InnerResult { get; private set; }

        /// <summary>
        /// Create an instance of this action result.
        /// </summary>
        /// <param name="challenge">Challenge to be returned on authentication failure.</param>
        /// <param name="innerResult">Original action result from authentication failure that the challenge will be returned with.</param>
        public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            Challenge = challenge;
            InnerResult = innerResult;
        }

        /// <summary>
        /// <see cref="IHttpActionResult.ExecuteAsync(CancellationToken)"/>
        /// </summary>
        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {

                // Only add one challenge per authentication scheme.
                if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == Challenge.Scheme))
                {
                    response.Headers.WwwAuthenticate.Add(Challenge);
                }

            }

            return response;
        }

    }

}
