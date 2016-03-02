namespace GW2NET.Common
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public static class DiscoverableExtensions
    {
        public static Task<IEnumerable<T>> DiscoverAsync<T>(this IDiscoverableNew<T> discoverable)
        {
            return DiscoverAsync(discoverable, CancellationToken.None);
        }

        public static async Task<IEnumerable<T>> DiscoverAsync<T>(this IDiscoverableNew<T> discoverable, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = discoverable.ServiceLocation.Build();

            return await discoverable.ResponseConverter.ConvertSetAsync(await discoverable.Client.SendAsync(request, cancellationToken), discoverable.IdentifiersConverter);
        }
    }
}