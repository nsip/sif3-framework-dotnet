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

using Sif.Framework.Demo.Au.Consumer.Consumers;
using Sif.Framework.Demo.Au.Consumer.Models;
using Sif.Framework.Demo.Au.Consumer.Utils;
using Sif.Framework.Models.Responses;
using Sif.Framework.Models.Settings;
using Sif.Framework.Services.Sessions;
using Sif.Framework.Utils;
using Sif.Specification.DataModel.Au;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sif.Framework.Demo.Au.Consumer
{
    internal class StudentDataTransferNoteApp : ConsoleApp
    {
        private static readonly slf4net.ILogger Log =
            slf4net.LoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

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
                RequestID = Random.Next(100000, 999999).ToString(),
                PreviousSchoolList = previousSchools.ToArray<PreviousSchoolType>()
            };

            return StudentDataTransferNote;
        }

        private static List<StudentDataTransferNote> CreateStudentDataTransferNotes(int count)
        {
            var StudentDataTransferNotesCache = new List<StudentDataTransferNote>();

            for (var i = 1; i <= count; i++)
            {
                StudentDataTransferNotesCache.Add(CreateStudentDataTransferNote());
            }

            return StudentDataTransferNotesCache;
        }

        private static void RunConsumer(IFrameworkSettings settings, ISessionService sessionService)
        {
            var consumer = new StudentDataTransferNoteConsumer(
                settings.ApplicationKey,
                settings.InstanceId,
                settings.UserToken,
                settings.SolutionId,
                settings,
                sessionService);
            consumer.Register();

            if (Log.IsInfoEnabled) Log.Info("Registered the Consumer.");

            try
            {
                // Retrieve all student data transfer notes.

                if (Log.IsInfoEnabled) Log.Info("*** Retrieve all student data transfer notes.");

                IEnumerable<StudentDataTransferNote> students = consumer.Query();

                foreach (StudentDataTransferNote student in students)
                {
                    if (Log.IsInfoEnabled)Log.Info($"Request ID is {student.RequestID}.");
                }
            }
            catch (UnauthorizedAccessException)
            {
                if (Log.IsInfoEnabled) Log.Info("Access to query students is rejected.");
            }
            catch (Exception e)
            {
                if (Log.IsErrorEnabled)
                    Log.Error(
                        $"Error running the StudentDataTransferNote Consumer.\n{ExceptionUtils.InferErrorResponseMessage(e)}",
                        e);
            }
            finally
            {
                consumer.Unregister();

                if (Log.IsInfoEnabled) Log.Info("Unregistered the Consumer.");
            }
        }

        public static void Main()
        {
            try
            {
                RunConsumer(GetSettings(SettingsSource.File), GetSessionService(SettingsSource.Database));
            }
            catch (Exception e)
            {
                if (Log.IsErrorEnabled)
                    Log.Error(
                        $"Error running the StudentDataTransferNote Consumer.\n{ExceptionUtils.InferErrorResponseMessage(e)}",
                        e);
            }

            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }
    }
}
