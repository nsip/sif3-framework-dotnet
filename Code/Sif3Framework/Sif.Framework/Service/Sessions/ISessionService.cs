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

namespace Sif.Framework.Service.Sessions
{
    /// <summary>
    /// This interface represents operations associated with SIF Framework sessions.
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Check whether a session entry already exists (based on the passed criteria parameters).
        /// </summary>
        /// <param name="applicationKey">Application key.</param>
        /// <param name="solutionId">Solution ID.</param>
        /// <param name="userToken">User token.</param>
        /// <param name="instanceId">Instance ID.</param>
        /// <returns>True if a session entry already exists; false otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">applicationKey is null.</exception>
        bool HasSession(
            string applicationKey,
            string solutionId = null,
            string userToken = null,
            string instanceId = null);

        /// <summary>
        /// Check whether a session entry already exists (based on the passed session token).
        /// </summary>
        /// <param name="sessionToken">Session token.</param>
        /// <returns>True if a session entry already exists; false otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">sessionToken is null.</exception>
        bool HasSessionToken(string sessionToken);

        /// <summary>
        /// Remove a session entry.
        /// </summary>
        /// <param name="sessionToken">Session token associated with the session entry.</param>
        /// <exception cref="System.ArgumentNullException">sessionToken is null.</exception>
        /// <exception cref="Tardigrade.Framework.Exceptions.DuplicateFoundException">Multiple session entries exist for the session token.</exception>
        /// <exception cref="Tardigrade.Framework.Exceptions.NotFoundException">No session entry exists for the session token.</exception>
        void RemoveSession(string sessionToken);

        /// <summary>
        /// Retrieve the Environment URL of a session entry that matches the passed criteria parameters.
        /// </summary>
        /// <param name="applicationKey">Application key.</param>
        /// <param name="solutionId">Solution ID.</param>
        /// <param name="userToken">User token.</param>
        /// <param name="instanceId">Instance ID.</param>
        /// <returns>Environment URL of the matched session entry; null if no match found.</returns>
        /// <exception cref="System.ArgumentNullException">applicationKey is null.</exception>
        /// <exception cref="Tardigrade.Framework.Exceptions.DuplicateFoundException">Multiple session entries exist for the passed criteria parameters.</exception>
        string RetrieveEnvironmentUrl(
            string applicationKey,
            string solutionId = null,
            string userToken = null,
            string instanceId = null);

        /// <summary>
        /// Retrieve the Queue unique identifier of a session entry that matches the passed criteria parameters.
        /// </summary>
        /// <param name="applicationKey">Application key.</param>
        /// <param name="solutionId">Solution ID.</param>
        /// <param name="userToken">User token.</param>
        /// <param name="instanceId">Instance ID.</param>
        /// <returns>Queue unique identifier of the matched session entry; null if no match found.</returns>
        /// <exception cref="System.ArgumentNullException">applicationKey is null.</exception>
        /// <exception cref="Tardigrade.Framework.Exceptions.DuplicateFoundException">Multiple session entries exist for the passed criteria parameters.</exception>
        string RetrieveQueueId(
            string applicationKey,
            string solutionId = null,
            string userToken = null,
            string instanceId = null);

        /// <summary>
        /// Retrieve the session token of a session entry that matches the passed criteria parameters.
        /// </summary>
        /// <param name="applicationKey">Application key.</param>
        /// <param name="solutionId">Solution ID.</param>
        /// <param name="userToken">User token.</param>
        /// <param name="instanceId">Instance ID.</param>
        /// <returns>Session token of the matched session entry; null if no match found.</returns>
        /// <exception cref="System.ArgumentNullException">applicationKey is null.</exception>
        /// <exception cref="Tardigrade.Framework.Exceptions.DuplicateFoundException">Multiple session entries exist for the passed criteria parameters.</exception>
        string RetrieveSessionToken(
            string applicationKey,
            string solutionId = null,
            string userToken = null,
            string instanceId = null);

        /// <summary>
        /// Retrieve the Subscription unique identifier of a session entry that matches the passed criteria parameters.
        /// </summary>
        /// <param name="applicationKey">Application key.</param>
        /// <param name="solutionId">Solution ID.</param>
        /// <param name="userToken">User token.</param>
        /// <param name="instanceId">Instance ID.</param>
        /// <returns>Subscription unique identifier of the matched session entry; null if no match found.</returns>
        /// <exception cref="System.ArgumentNullException">applicationKey is null.</exception>
        /// <exception cref="Tardigrade.Framework.Exceptions.DuplicateFoundException">Multiple session entries exist for the passed criteria parameters.</exception>
        string RetrieveSubscriptionId(
            string applicationKey,
            string solutionId = null,
            string userToken = null,
            string instanceId = null);

        /// <summary>
        /// Store a session entry associated with Consumer/Provider registration.
        /// </summary>
        /// <param name="applicationKey">Application key.</param>
        /// <param name="sessionToken">Session token.</param>
        /// <param name="environmentUrl">Environment URL.</param>
        /// <param name="solutionId">Solution ID.</param>
        /// <param name="userToken">User token.</param>
        /// <param name="instanceId">Instance ID.</param>
        /// <exception cref="Tardigrade.Framework.Exceptions.AlreadyExistsException">A session entry already exists for the applicationKey/solutionId/userToken/instanceId combination and/or sessionToken specified.</exception>
        /// <exception cref="System.ArgumentNullException">applicationKey, sessionToken and/or environmentUrl are null.</exception>
        void StoreSession(
            string applicationKey,
            string sessionToken,
            string environmentUrl,
            string solutionId = null,
            string userToken = null,
            string instanceId = null);

        /// <summary>
        /// Update the Queue unique identifier associated with a session entry.
        /// </summary>
        /// <param name="queueId">Queue unique identifier.</param>
        /// <param name="applicationKey">Application key.</param>
        /// <param name="solutionId">Solution ID.</param>
        /// <param name="userToken">User token.</param>
        /// <param name="instanceId">Instance ID.</param>
        /// <exception cref="System.ArgumentNullException">queueId and/or applicationKey are null.</exception>
        /// <exception cref="Tardigrade.Framework.Exceptions.DuplicateFoundException">Multiple session entries exist for the passed criteria parameters.</exception>
        /// <exception cref="Tardigrade.Framework.Exceptions.NotFoundException">No session entry exists for the passed criteria parameters.</exception>
        void UpdateQueueId(
            string queueId,
            string applicationKey,
            string solutionId = null,
            string userToken = null,
            string instanceId = null);

        /// <summary>
        /// Update the Subscription unique identifier associated with a session entry.
        /// </summary>
        /// <param name="subscriptionId">Subscription unique identifier.</param>
        /// <param name="applicationKey">Application key.</param>
        /// <param name="solutionId">Solution ID.</param>
        /// <param name="userToken">User token.</param>
        /// <param name="instanceId">Instance ID.</param>
        /// <exception cref="System.ArgumentNullException">subscriptionId and/or applicationKey are null.</exception>
        /// <exception cref="Tardigrade.Framework.Exceptions.DuplicateFoundException">Multiple session entries exist for the passed criteria parameters.</exception>
        /// <exception cref="Tardigrade.Framework.Exceptions.NotFoundException">No session entry exists for the passed criteria parameters.</exception>
        void UpdateSubscriptionId(
            string subscriptionId,
            string applicationKey,
            string solutionId = null,
            string userToken = null,
            string instanceId = null);
    }
}