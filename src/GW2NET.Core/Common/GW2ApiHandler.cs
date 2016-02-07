namespace GW2NET.Common
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class GW2ApiHandler : DelegatingHandler
    {
        public GW2ApiHandler()
        {
            // For now add this as inner handler by default,
            // since we don't have a web config.
            this.InnerHandler = new HttpClientHandler();
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                var serviceException = new ServiceException("An error occurred while deserializing response data. See the inner exception for details", ex);
                // serviceException.Request = request;
                throw serviceException;
            }

            DateTimeOffset? responseDate = response.Headers.Date;
            // do something with the date ...

            return response;
        }
    }
}