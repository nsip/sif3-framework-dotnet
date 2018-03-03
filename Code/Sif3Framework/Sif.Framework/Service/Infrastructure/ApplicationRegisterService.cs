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
    /// Service class for the ApplicationRegister type.
    /// <see cref="IApplicationRegisterService"/>
    /// </summary>
    public class ApplicationRegisterService : GenericService<ApplicationRegister, long>, IApplicationRegisterService
    {

        /// <summary>
        /// Create an instance of this repository class.
        /// <see cref="GenericService{T, PK}.GenericService(IGenericRepository{T, PK})"/>
        /// </summary>
        public ApplicationRegisterService(IApplicationRegisterRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// <see cref="IApplicationRegisterService.RetrieveByApplicationKey(string)"/>
        /// </summary>
        public virtual ApplicationRegister RetrieveByApplicationKey(string applicationKey)
        {
            return ((ApplicationRegisterRepository)repository).RetrieveByApplicationKey(applicationKey);
        }

    }

}
