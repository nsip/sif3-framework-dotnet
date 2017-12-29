﻿/*
 * Copyright 2017 Systemic Pty Ltd
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

using Sif.Framework.Demo.Broker.Models;
using Sif.Framework.Demo.Broker.Services;
using Sif.Framework.Providers;
using Sif.Framework.Service.Providers;
using Sif.Framework.WebApi.ModelBinders;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Sif.Specification.Infrastructure;
using Sif.Specification.DataModel.Au;
using System;

namespace Sif.Framework.Demo.Broker.Controllers
{

    public class QueuesProvider : BasicProvider<Queue>
    {

        public QueuesProvider() : this(new QueueService())
        {
        }

        protected QueuesProvider(IBasicProviderService<Queue> service) : base(service)
        {
        }

        [NonAction]
        public override IHttpActionResult BroadcastEvents(string zoneId = null, string contextId = null)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        public override IHttpActionResult Delete(deleteRequestType deleteRequest, [MatrixParameter] string[] zoneId = null, [MatrixParameter] string[] contextId = null)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        public override IHttpActionResult Get(string object1, [FromUri(Name = "id1")] string refId1, string object2 = null, [FromUri(Name = "id2")] string refId2 = null, string object3 = null, [FromUri(Name = "id3")] string refId3 = null, [MatrixParameter] string[] zoneId = null, [MatrixParameter] string[] contextId = null)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        public override IHttpActionResult Head([MatrixParameter] string[] zoneId = null, [MatrixParameter] string[] contextId = null)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [NonAction]
        public override IHttpActionResult Post(List<Queue> objs, [MatrixParameter] string[] zoneId = null, [MatrixParameter] string[] contextId = null)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [Route("~/api/Queues/Queue")]
        public override IHttpActionResult Post(Queue obj, [MatrixParameter] string[] zoneId = null, [MatrixParameter] string[] contextId = null)
        {
            return base.Post(obj, zoneId, contextId);
        }

        public override IHttpActionResult Put(List<Queue> objs, [MatrixParameter] string[] zoneId = null, [MatrixParameter] string[] contextId = null)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        public override IHttpActionResult Put([FromUri(Name = "id")] string refId, Queue obj, [MatrixParameter] string[] zoneId = null, [MatrixParameter] string[] contextId = null)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        [Route("~/api/Queues/{queueId}/messages")]
        public IHttpActionResult Get(string queueId)
        {
            NameOfRecordType name = new NameOfRecordType { Type = NameOfRecordTypeType.LGL, FamilyName = "Simpson", GivenName = "Bart" };
            PersonInfoType personInfo = new PersonInfoType { Name = name };
            StudentPersonalType student = new StudentPersonalType { RefId = Guid.NewGuid().ToString(), LocalId = "666", PersonInfo = personInfo };
            ICollection<StudentPersonalType> students = new List<StudentPersonalType>() { student };

            return Ok(students);
        }

    }

}