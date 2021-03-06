﻿/*
 * Copyright 2021 Systemic Pty Ltd
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

using Sif.Framework.Model.DataModels;
using Sif.Framework.Model.Infrastructure;
using Sif.Framework.Model.Settings;
using Sif.Framework.Service.Serialisation;
using Sif.Framework.Service.Sessions;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sif.Framework.Consumers
{
    /// <summary>
    /// This is a convenience class for Consumers of SIF data model objects whereby the primary key is of type
    /// System.String and the multiple objects entity is represented as a list of single objects.
    /// </summary>
    /// <typeparam name="T">Type that defines a SIF data model object.</typeparam>
    public class BasicConsumer<T> : Consumer<T, List<T>, string>, IBasicConsumer<T> where T : ISifRefId<string>
    {
        /// <inheritdoc />
        protected BasicConsumer(
            Environment environment,
            IFrameworkSettings settings = null,
            ISessionService sessionService = null)
            : base(environment, settings, sessionService)
        {
        }

        /// <inheritdoc />
        public BasicConsumer(
            string applicationKey,
            string instanceId = null,
            string userToken = null,
            string solutionId = null,
            IFrameworkSettings settings = null,
            ISessionService sessionService = null)
            : base(applicationKey, instanceId, userToken, solutionId, settings, sessionService)
        {
        }

        /// <summary>
        /// <see cref="Consumer{TSingle,TMultiple,TPrimaryKey}.SerialiseMultiple(TMultiple)">SerialiseMultiple</see>
        /// </summary>
        protected override string SerialiseMultiple(List<T> obj)
        {
            var xmlRootAttribute = new XmlRootAttribute(TypeName + "s")
            {
                Namespace = ConsumerSettings.DataModelNamespace,
                IsNullable = false
            };

            return SerialiserFactory.GetSerialiser<List<T>>(ContentType, xmlRootAttribute).Serialise(obj);
        }

        /// <summary>
        /// <see cref="Consumer{TSingle,TMultiple,TPrimaryKey}.DeserialiseMultiple(string)">DeserialiseMultiple</see>
        /// </summary>
        protected override List<T> DeserialiseMultiple(string payload)
        {
            var xmlRootAttribute = new XmlRootAttribute(TypeName + "s")
            {
                Namespace = ConsumerSettings.DataModelNamespace,
                IsNullable = false
            };

            return SerialiserFactory.GetSerialiser<List<T>>(Accept, xmlRootAttribute).Deserialise(payload);
        }
    }
}