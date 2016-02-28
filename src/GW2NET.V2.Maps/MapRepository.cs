// <copyright file="MapRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Maps
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.Maps;

    /// <summary>Represents a repository that retrieves data from the /v2/items interface. See the remarks section for important limitations regarding this implementation.</summary>
    /// <remarks>
    /// This implementation does not retrieve associated entities.
    /// </remarks>
    public sealed class MapRepository : CachedRepository<Map>, IApiService<int, Map>, IDiscoverService<int>, ILocalizable
    {
        private readonly IConverter<int, int> identifiersConverter;

        private readonly IConverter<MapDataContract, Map> itemConverter;

        /// <summary>Initializes a new instance of the <see cref="MapRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make connections with the ArenaNet servers.</param>
        /// <param name="responseConverter">The <see cref="ResponseConverterBase"/> used to convert <see cref="HttpResponseMessage"/> into objects.</param>
        /// <param name="cache">The <see cref="ICache{T}"/> used to cache api responses.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="itemConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public MapRepository(
            HttpClient httpClient,
            ResponseConverterBase responseConverter,
            ICache<Map> cache,
            IConverter<int, int> identifiersConverter,
            IConverter<MapDataContract, Map> itemConverter)
            : base(httpClient, responseConverter, cache)
        {
            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            if (itemConverter == null)
            {
                throw new ArgumentNullException(nameof(itemConverter));
            }

            this.identifiersConverter = identifiersConverter;
            this.itemConverter = itemConverter;
        }

        /// <inheritdoc />
        public CultureInfo Culture { get; set; }

        /// <inheritdoc />
        public Task<IEnumerable<int>> DiscoverAsync()
        {
            return this.DiscoverAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<int>> DiscoverAsync(CancellationToken cancellationToken)
        {
            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("maps").Build();
            return await this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request, cancellationToken), this.identifiersConverter);
        }
        
        /// <inheritdoc />
        public Task<Map> GetAsync(int identifier)
        {
            return this.GetAsync(identifier, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Map> GetAsync(int identifier, CancellationToken cancellationToken)
        {
            Map cacheItem = this.Cache.Get(i => i.MapId == identifier).SingleOrDefault();
            if (cacheItem != null)
            {
                return cacheItem;
            }

            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("maps").WithIdentifier(identifier).Build();
            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request, cancellationToken), this.itemConverter);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Map>> GetAsync()
        {
            return this.GetAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Map>> GetAsync(CancellationToken cancellationToken)
        {
            List<Map> cacheItems = this.Cache.Get(i => true).ToList();

            IEnumerable<int> idsToQuery = (await this.DiscoverAsync(cancellationToken)).SymmetricExcept(cacheItems.Select(i => i.MapId));
            return (await this.GetItemsAsync(idsToQuery, this.itemConverter, cancellationToken)).Union(cacheItems);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Map>> GetAsync(IEnumerable<int> identifiers)
        {
            return this.GetAsync(identifiers, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Map>> GetAsync(IEnumerable<int> identifiers, CancellationToken cancellationToken)
        {
            IList<int> ids = identifiers as IList<int> ?? identifiers.ToList();
            List<Map> cacheItems = this.Cache.Get(i => ids.All(id => id != i.MapId)).ToList();
            if (cacheItems.Count == ids.Count)
            {
                return cacheItems;
            }

            return (await this.GetItemsAsync(ids.SymmetricExcept(cacheItems.Select(i => i.MapId)), this.itemConverter,cancellationToken)).Union(cacheItems);
        }

        private async Task<IEnumerable<TValue>> GetItemsAsync<TKey, TDataContract, TValue>(IEnumerable<TKey> ids, IConverter<TDataContract, TValue> itemConverter, CancellationToken cancellationToken)
        {
            IEnumerable<IEnumerable<TKey>> idListList = this.CalculatePages(ids);

            ConcurrentBag<TValue> items = new ConcurrentBag<TValue>();
            Parallel.ForEach(idListList,
                             async idList =>
                             {
                                 HttpRequestMessage request =
                                     ApiMessageBuilder.Init()
                                                      .Version(ApiVersion.V2)
                                                      .OnEndpoint("continents")
                                                      .WithIdentifiers(idList)
                                                      .Build();

                                 Task<IEnumerable<TValue>> responseItems = this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request, cancellationToken), itemConverter);

                                 foreach (TValue item in await responseItems)
                                 {
                                     items.Add(item);
                                 }
                             });

            return items;
        }
    }
}