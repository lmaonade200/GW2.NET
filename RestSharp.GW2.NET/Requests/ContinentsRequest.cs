﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContinentsRequest.cs" company="GW2.Net Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using GW2DotNET.V1.Core;
using GW2DotNET.V1.Core.MapsInformation.Continents;

namespace RestSharp.GW2DotNET.Requests
{
    /// <summary>
    ///     Represents a request for static information about the continents.
    /// </summary>
    /// <remarks>
    ///     See <a href="http://wiki.guildwars2.com/wiki/API:1/continents" /> for more information.
    /// </remarks>
    public class ContinentsRequest : ServiceRequest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ContinentsRequest" /> class.
        /// </summary>
        public ContinentsRequest()
            : base(Resources.Continents)
        {
        }

        /// <summary>
        ///     Sends the current request and returns a response.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <returns>The response.</returns>
        public IServiceResponse<ContinentsResult> GetResponse(IServiceClient serviceClient)
        {
            return base.GetResponse<ContinentsResult>(serviceClient);
        }

        /// <summary>
        ///     Sends the current request and returns a response.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <returns>The response.</returns>
        public Task<IServiceResponse<ContinentsResult>> GetResponseAsync(IServiceClient serviceClient)
        {
            return base.GetResponseAsync<ContinentsResult>(serviceClient);
        }

        /// <summary>
        ///     Sends the current request and returns a response.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> that provides cancellation support.</param>
        /// <returns>The response.</returns>
        public Task<IServiceResponse<ContinentsResult>> GetResponseAsync(IServiceClient serviceClient, CancellationToken cancellationToken)
        {
            return base.GetResponseAsync<ContinentsResult>(serviceClient, cancellationToken);
        }
    }
}