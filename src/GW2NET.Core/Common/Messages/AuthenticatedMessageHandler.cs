// <copyright file="AuthenticatedMessageHandler.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>Message handler to make authenticated requests against the Guild Wars 2 api.</summary>
    public class AuthenticatedMessageHandler : DelegatingHandler
    {
        private readonly string apiKey;

        /// <summary>Initializes a new instance of the <see cref="AuthenticatedMessageHandler"/> class.</summary>
        /// <param name="innerHandler">The inner handler used to make the actual request.</param>
        /// <param name="apiKey">The api-key used to authenticate requests.</param>
        public AuthenticatedMessageHandler(HttpMessageHandler innerHandler, string apiKey)
        {
            if (!KeyUtilities.IsValid(apiKey))
            {
                throw new ArgumentException("The api-key did not have the correct format.");
            }

            this.apiKey = apiKey;
            this.InnerHandler = innerHandler;
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.apiKey);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
