// <copyright file="DiscoverableExtensions.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>Contains extension methods for repositories of type <see cref="IDiscoverable{T}"/> to discover objects.</summary>
    public static class DiscoverableExtensions
    {
        /// <summary>Gets a set of identifiers from the Guild Wars 2 api.</summary>
        /// <typeparam name="T">The identifiers type.</typeparam>
        /// <param name="discoverable">The discoverable repository.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the discovered identifiers.</returns>
        public static Task<IEnumerable<T>> DiscoverAsync<T>(this IDiscoverable<T> discoverable)
        {
            return DiscoverAsync(discoverable, CancellationToken.None);
        }

        /// <summary>Gets a set of identifiers from the Guild Wars 2 api.</summary>
        /// <typeparam name="T">The identifiers type.</typeparam>
        /// <param name="discoverable">The discoverable repository.</param>
        /// <param name="cancellationToken">A token signalling the cancellation of the operation.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the discovered identifiers.</returns>
        public static async Task<IEnumerable<T>> DiscoverAsync<T>(this IDiscoverable<T> discoverable, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = discoverable.ServiceLocation.Build();

            return await discoverable.ResponseConverter.ConvertSetAsync(await discoverable.Client.SendAsync(request, cancellationToken), discoverable.IdentifiersConverter);
        }
    }
}