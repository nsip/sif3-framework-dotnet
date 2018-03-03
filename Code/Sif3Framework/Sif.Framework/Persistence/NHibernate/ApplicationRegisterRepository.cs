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

using NHibernate;
using Sif.Framework.Model.Infrastructure;
using System;

namespace Sif.Framework.Persistence.NHibernate
{

    /// <summary>
    /// <see cref="IEnvironmentRegisterRepository"/>
    /// </summary>
    public class ApplicationRegisterRepository : GenericRepository<ApplicationRegister, long>, IApplicationRegisterRepository
    {

        /// <summary>
        /// Create an instance of this repository class with a predefined sessionFactory instance.
        /// </summary>
        protected ApplicationRegisterRepository() : this(EnvironmentProviderSessionFactory.Instance)
        {
        }

        /// <summary>
        /// <see cref="GenericRepository{T, PK}.GenericRepository(IBaseSessionFactory)"/>
        /// </summary>
        public ApplicationRegisterRepository(IBaseSessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        /// <summary>
        /// <see cref="IApplicationRegisterRepository.RetrieveByApplicationKey(string)"/>
        /// </summary>
        public virtual ApplicationRegister RetrieveByApplicationKey(string applicationKey)
        {

            if (string.IsNullOrWhiteSpace(applicationKey))
            {
                throw new ArgumentNullException("applicationKey");
            }

            using (ISession session = sessionFactory.OpenSession())
            {
                return session.QueryOver<ApplicationRegister>().Where(e => e.ApplicationKey == applicationKey).SingleOrDefault();
            }

        }

    }

}
