/*
 * Copyright 2024 Systemic Pty Ltd
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

using Sif.Framework.Demo.Au.Provider.Models;
using Sif.Framework.Demo.Au.Provider.Utils;
using Sif.Framework.Models.Parameters;
using Sif.Framework.Models.Query;
using Sif.Framework.Services.Providers;
using Sif.Specification.DataModel.Au;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sif.Framework.Demo.Au.Provider.Services
{
    public class StudentDataTransferNoteService : IBasicProviderService<StudentDataTransferNote>
    {
        private static readonly IDictionary<string, StudentDataTransferNote> Cache;
        private static readonly Random Random = new Random();

        private static StudentDataTransferNote CreateStudentDataTransferNote()
        {
            List<PreviousSchoolType> previousSchools = new List<PreviousSchoolType>
            {
                new PreviousSchoolType { Name = RandomNameGenerator.FamilyName + " School" },
                new PreviousSchoolType { Name = RandomNameGenerator.FamilyName + " School" }
            };

            var StudentDataTransferNote = new StudentDataTransferNote
            {
                RefId = Guid.NewGuid().ToString(),
                RequestID = Random.Next(100000, 999999).ToString(),
                PreviousSchoolList = previousSchools.ToArray<PreviousSchoolType>()
            };

            return StudentDataTransferNote;
        }

        private static IDictionary<string, StudentDataTransferNote> CreateStudentDataTransferNotes(int count)
        {
            IDictionary<string, StudentDataTransferNote> StudentDataTransferNotesCache = new Dictionary<string, StudentDataTransferNote>();

            for (var i = 1; i <= count; i++)
            {
                StudentDataTransferNote studentDataTransferNote = CreateStudentDataTransferNote();
                StudentDataTransferNotesCache.Add(studentDataTransferNote.RefId, studentDataTransferNote);
            }

            return StudentDataTransferNotesCache;
        }

        static StudentDataTransferNoteService()
        {
            Cache = CreateStudentDataTransferNotes(2);
        }

        public StudentDataTransferNote Create(
            StudentDataTransferNote obj,
            bool? mustUseAdvisory = null,
            string zoneId = null,
            string contextId = null,
            params RequestParameter[] requestParameters)
        {
            throw new NotImplementedException();
        }

        public void Delete(
            string refId,
            string zoneId = null,
            string contextId = null,
            params RequestParameter[] requestParameters)
        {
            throw new NotImplementedException();
        }

        public StudentDataTransferNote Retrieve(
            string refId,
            string zoneId = null,
            string contextId = null,
            params RequestParameter[] requestParameters)
        {
            throw new NotImplementedException();
        }

        public List<StudentDataTransferNote> Retrieve(
            uint? pageIndex = null,
            uint? pageSize = null,
            string zoneId = null,
            string contextId = null,
            params RequestParameter[] requestParameters)
        {
            return Cache.Values.ToList();
        }

        public List<StudentDataTransferNote> Retrieve(
            StudentDataTransferNote obj,
            uint? pageIndex = null,
            uint? pageSize = null,
            string zoneId = null,
            string contextId = null,
            params RequestParameter[] requestParameters)
        {
            throw new NotImplementedException();
        }

        public List<StudentDataTransferNote> Retrieve(
            IEnumerable<EqualCondition> conditions,
            uint? pageIndex = null,
            uint? pageSize = null,
            string zoneId = null,
            string contextId = null,
            params RequestParameter[] requestParameters)
        {
            throw new NotImplementedException();
        }

        public void Update(
            StudentDataTransferNote obj,
            string zoneId = null,
            string contextId = null,
            params RequestParameter[] requestParameters)
        {
            throw new NotImplementedException();
        }
    }
}