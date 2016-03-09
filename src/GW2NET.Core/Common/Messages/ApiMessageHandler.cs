// <copyright file="ApiMessageHandler.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    using System;
    using System.Threading.Tasks;

    using System.Net.Http;
    using System.Threading;

    using Polly;

    /// <summary>Represents the most low level message handler to make requests against the api.</summary>
    public class ApiMessageHandler : DelegatingHandler
    {
        private readonly byte numberOfRetries;

        /// <summary>Initializes a new instance of the <see cref="ApiMessageHandler" /> class.</summary>
        /// <param name="numberOfRetries">An 8-bit unsingned integer to set the number of retries (Max: 255).</param>
        public ApiMessageHandler(byte numberOfRetries = 3)
        {
            this.numberOfRetries = numberOfRetries;
            this.InnerHandler = new HttpClientHandler();
        }

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync = base.SendAsync;

            return await Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(this.numberOfRetries, retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount)))
                .ExecuteAsync(
                async () =>
                {
                    var responseMessage = await sendAsync(request, cancellationToken).ConfigureAwait(false);

                    return responseMessage.EnsureSuccessStatusCode();
                });
        }
    }
}
