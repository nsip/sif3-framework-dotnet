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

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sif.Framework.WebApi.ActionResults
{

    /// <summary>
    /// Action result that represents an authentication failure.
    /// This class is based on the article <a href="https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-filters">Authentication Filters in ASP.NET Web API 2</a>.
    /// </summary>
    class AuthenticationFailureResult : IHttpActionResult
    {

        /// <summary>
        /// The reason for the authentication failure.
        /// </summary>
        public string ReasonPhrase { get; private set; }

        /// <summary>
        /// Message associated with the HTTP request.
        /// </summary>
        public HttpRequestMessage Request { get; private set; }

        /// <summary>
        /// Create an instance of this action result.
        /// </summary>
        /// <param name="reasonPhrase">The reason for the authentication failure.</param>
        /// <param name="request">Message associated with the HTTP request.</param>
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        /// <summary>
        /// Create an appropriate response message that represents an authentication failure.
        /// </summary>
        /// <returns>Response message that represents an authentication failure.</returns>
        private HttpResponseMessage Execute()
        {

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = Request,
                ReasonPhrase = ReasonPhrase
            };

            return response;
        }

        /// <summary>
        /// <see cref="IHttpActionResult.ExecuteAsync(CancellationToken)"/>
        /// </summary>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

    }

}
