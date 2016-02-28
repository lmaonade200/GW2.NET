// <copyright file="ContinentRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Continents
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

    /// <summary>Represents a repository that retrieves data from the /v2/continents interface.</summary>
    public sealed class ContinentRepository : CachedRepository<Continent>, IApiService<int, Continent>, IDiscoverService<int>, ILocalizable
    {
        private readonly IConverter<int, int> identifiersConverter;

        private readonly IConverter<ContinentDataContract, Continent> continentConverter;

        /// <summary>Initializes a new instance of the <see cref="ContinentRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make connections with the ArenaNet servers.</param>
        /// <param name="responseConverter">The <see cref="ResponseConverterBase"/> used to convert <see cref="HttpResponseMessage"/> into objects.</param>
        /// <param name="cache">The <see cref="ICache{T}"/> used to cache api responses.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="continentConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public ContinentRepository(HttpClient httpClient, ResponseConverterBase responseConverter, ICache<Continent> cache, IConverter<int, int> identifiersConverter, IConverter<ContinentDataContract, Continent> continentConverter)
            : base(httpClient, responseConverter, cache)
        {
            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            if (continentConverter == null)
            {
                throw new ArgumentNullException(nameof(continentConverter));
            }

            this.identifiersConverter = identifiersConverter;
            this.continentConverter = continentConverter;
        }

        /// <summary>Gets or sets the locale.</summary>
        public CultureInfo Culture { get; set; }

        /// <inheritdoc />
        public Task<IEnumerable<int>> DiscoverAsync()
        {
            return this.DiscoverAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<int>> DiscoverAsync(CancellationToken cancellationToken)
        {
            var request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("continents").Build();
            return await this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request, cancellationToken), this.identifiersConverter);
        }

        /// <inheritdoc />
        public Task<Continent> GetAsync(int identifier)
        {
            return this.GetAsync(identifier, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Continent> GetAsync(int identifier, CancellationToken cancellationToken)
        {
            var cacheItem = this.Cache.Get(i => i.ContinentId == identifier).SingleOrDefault();
            if (cacheItem != null)
            {
                return cacheItem;
            }

            var request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("continents").ForCulture(this.Culture).WithIdentifier(identifier).Build();
            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request, cancellationToken), this.continentConverter);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Continent>> GetAsync()
        {
            return this.GetAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Continent>> GetAsync(CancellationToken cancellationToken)
        {
            var cacheItems = this.Cache.Get(i => true).ToList();
            var ids = (await this.DiscoverAsync(cancellationToken)).SymmetricExcept(cacheItems.Select(i => i.ContinentId));

            return (await this.GetItemsAsync(ids, this.continentConverter, cancellationToken)).Union(cacheItems);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Continent>> GetAsync(IEnumerable<int> identifiers)
        {
            return this.GetAsync(identifiers, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Continent>> GetAsync(IEnumerable<int> identifiers, CancellationToken cancellationToken)
        {
            IList<int> ids = identifiers as IList<int> ?? identifiers.ToList();
            var cacheItems = this.Cache.Get(i => ids.All(id => id != i.ContinentId)).ToList();

            if (cacheItems.Count == ids.Count)
            {
                return cacheItems;
            }

            return (await this.GetItemsAsync(ids.SymmetricExcept(cacheItems.Select(i => i.ContinentId)), this.continentConverter, cancellationToken)).Union(cacheItems);
        }

        private async Task<IEnumerable<TValue>> GetItemsAsync<TKey, TDataContract, TValue>(IEnumerable<TKey> ids, IConverter<TDataContract, TValue> itemConverter, CancellationToken cancellationToken)
        {
            var idListList = this.CalculatePages(ids);

            ConcurrentBag<TValue> items = new ConcurrentBag<TValue>();
            Parallel.ForEach(idListList,
                             async idList =>
                             {
                                 var request =
                                     ApiMessageBuilder.Init()
                                                      .Version(ApiVersion.V2)
                                                      .OnEndpoint("continents")
                                                      .ForCulture(this.Culture)
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