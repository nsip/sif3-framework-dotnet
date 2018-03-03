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

using Sif.Framework.Model.Infrastructure;
using Sif.Framework.Persistence;
using Sif.Framework.Persistence.NHibernate;

namespace Sif.Framework.Service.Infrastructure
{

    /// <summary>
    /// Service class for the EnvironmentRegister type.
    /// <see cref="IEnvironmentRegisterService"/>
    /// </summary>
    public class EnvironmentRegisterService : GenericService<EnvironmentRegister, long>, IEnvironmentRegisterService
    {

        /// <summary>
        /// Create an instance of this repository class.
        /// <see cref="GenericService{T, PK}.GenericService(IGenericRepository{T, PK})"/>
        /// </summary>
        public EnvironmentRegisterService(IEnvironmentRegisterRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// <see cref="IEnvironmentRegisterService.RetrieveByUniqueIdentifiers(string, string, string, string)"/>
        /// </summary>
        public virtual EnvironmentRegister RetrieveByUniqueIdentifiers(string applicationKey, string instanceId, string userToken, string solutionId)
        {
            EnvironmentRegisterRepository repo = (EnvironmentRegisterRepository)repository;

            EnvironmentRegister environmentRegister =
                // Let's try the lowest level first. Remember solutionId is optional.
                repo.RetrieveByUniqueIdentifiers(applicationKey, instanceId, userToken, solutionId)
                // Try the next level up. We have no result, yet.
                ?? repo.RetrieveByUniqueIdentifiers(applicationKey, null, userToken, solutionId)
                // And finally try the top level. We still have no result.
                ?? repo.RetrieveByUniqueIdentifiers(applicationKey, null, null, solutionId)
                // If we get here then there is no template listed for the given keys.
                ;

            return environmentRegister;
        }

    }

}
