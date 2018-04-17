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
using Sif.Framework.Model.Exceptions;
using Sif.Framework.Model.Infrastructure;
using Sif.Framework.Service.Authorisation;
using Sif.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Sif.Framework.WebApi.Filters
{

    /// <summary>
    /// Filter used for SIF Rights authorisation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    class AuthorisationAttribute : AuthorizationFilterAttribute
    {
        private readonly bool isServicePath;
        private readonly RightType rightType;
        private readonly RightValue rightValue;

        /// <summary>
        /// Create an instance of this attribute.
        /// </summary>
        /// <param name="rightType">Type of Rights, i.e. ADMIN, CREATE, DELETE, PROVIDE, QUERY, SUBSCRIBE, UPDATE.</param>
        /// <param name="rightValue">Rights value, i.e. APPROVED, REJECTED, SUPPORTED.</param>
        /// <param name="isServicePath">Flag indicating whether this attribute is checking for Service Path authorisation.</param>
        public AuthorisationAttribute(RightType rightType, RightValue rightValue = RightValue.APPROVED, bool isServicePath = false)
        {
            this.rightType = rightType;
            this.rightValue = rightValue;
            this.isServicePath = isServicePath;
        }

        /// <summary>
        /// Determine the parameters associated with the method that this attribute is associated with.
        /// </summary>
        /// <param name="actionContext">Context associated with the Action (method) of the Controller.</param>
        /// <returns>Parameters associated with the method that this attribute is associated with.</returns>
        private IDictionary<string, ParameterInfo> GetMethodParameters(HttpActionContext actionContext)
        {
            IDictionary<string, ParameterInfo> methodParameters = new Dictionary<string, ParameterInfo>();
            ReflectedHttpActionDescriptor actionDescriptor = actionContext.ActionDescriptor as ReflectedHttpActionDescriptor;
            ParameterInfo[] parameters = actionDescriptor.MethodInfo.GetParameters();

            foreach (ParameterInfo parameter in parameters)
            {
                methodParameters.Add(parameter.Name, parameter);
            }

            return methodParameters;
        }

        /// <summary>
        /// <see cref="AuthorizationFilterAttribute.OnAuthorization(HttpActionContext)"/>
        /// </summary>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            try
            {
                IHttpController controller = actionContext.ControllerContext.Controller;

                // Authorisation checks only apply to SIF Providers.
                if (ProviderUtils.isController(controller.GetType()))
                {
                    string serviceName = controller.GetType().GetProperty("TypeName").GetValue(controller).ToString();
                    serviceName = $"{serviceName}s";

                    // If the authorisation check is for a Service Path Action (method), generate an appropriate
                    // service name value that can be matched against the ACL.
                    if (isServicePath)
                    {
                        string[] segments = actionContext.Request.RequestUri.Segments;
                        IDictionary<string, ParameterInfo> parameters = GetMethodParameters(actionContext);

                        // The parameter names are specific (hard-coded) to the Service Path Action (method).
                        if (parameters.ContainsKey("object1"))
                        {
                            parameters.TryGetValue("object1", out ParameterInfo object1);
                            parameters.TryGetValue("object2", out ParameterInfo object2);
                            parameters.TryGetValue("object3", out ParameterInfo object3);

                            if (object3?.RawDefaultValue != null && segments.Length >= 7)
                            {
                                serviceName = $"{segments[segments.Length - 7]}{{}}/{segments[segments.Length - 5]}{{}}/{segments[segments.Length - 3]}{{}}/{serviceName}";
                            }
                            else if (object2?.RawDefaultValue != null && segments.Length >= 5)
                            {
                                serviceName = $"{segments[segments.Length - 5]}{{}}/{segments[segments.Length - 3]}{{}}/{serviceName}";
                            }
                            else if (object1?.RawDefaultValue != null && segments.Length >= 3)
                            {
                                serviceName = $"{segments[segments.Length - 3]}{{}}/{serviceName}";
                            }

                        }

                    }

                    // The authorisation service needs the authorisation scheme (i.e. Basic, SIF_HMACSHA256) associated
                    // with the Request before it can be instantiated.
                    IAuthorisationService authorisationService = ServiceFactory.Create.AuthorisationService(actionContext.Request.Headers?.Authorization.Scheme);

                    try
                    {

                        if (!authorisationService.IsAuthorised(actionContext.Request.Headers, serviceName, rightType, rightValue))
                        {

                            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                            {
                                ReasonPhrase = "Request is not authorised: Rights are insufficient."
                            };

                        }

                    }
                    catch (InvalidSessionException e)
                    {

                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                        {
                            ReasonPhrase = $"Request is not authorised: {e.GetBaseException().Message}"
                        };

                    }

                }

            }
            catch (Exception e)
            {

                actionContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = $"Error occurred authorising the Request: {e.GetBaseException().Message}"
                };

            }

        }

    }

}
