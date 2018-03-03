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

using NHibernate;
using System;

namespace Sif.Framework.Persistence.NHibernate
{

    /// <summary>
    /// <see cref="IEnvironmentRepository"/>
    /// </summary>
    public class EnvironmentRepository : GenericRepository<Model.Infrastructure.Environment, Guid>, IEnvironmentRepository
    {

        /// <summary>
        /// Create an instance of this repository class with a predefined sessionFactory instance.
        /// </summary>
        protected EnvironmentRepository() : this(EnvironmentProviderSessionFactory.Instance)
        {
        }

        /// <summary>
        /// <see cref="GenericRepository{T, PK}.GenericRepository(IBaseSessionFactory)"/>
        /// </summary>
        public EnvironmentRepository(IBaseSessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        /// <summary>
        /// <see cref="IEnvironmentRepository.RetrieveBySessionToken(string)"/>
        /// </summary>
        public virtual Model.Infrastructure.Environment RetrieveBySessionToken(string sessionToken)
        {

            if (string.IsNullOrWhiteSpace(sessionToken))
            {
                throw new ArgumentNullException("sessionToken");
            }

            using (ISession session = sessionFactory.OpenSession())
            {
                return session.QueryOver<Model.Infrastructure.Environment>().Where(e => e.SessionToken == sessionToken).SingleOrDefault();
            }

        }

    }

}
