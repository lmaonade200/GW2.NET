namespace GW2NET.V2.Builds
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class MockBuildMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.Run(
                            () => new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new StringContent("{\"id\":45075}")
                                {
                                    Headers = { ContentLanguage = { "en" } }
                                },
                                Headers =
                                {
                                    Date = DateTimeOffset.Now
                                },
                                RequestMessage = request
                            }, cancellationToken);
        }
    }
}