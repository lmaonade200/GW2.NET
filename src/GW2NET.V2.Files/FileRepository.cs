// <copyright file="FileRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Files
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.Files;

    /// <summary>Represents a repository that retrieves data from the /v2/files interface.</summary>
    public sealed class FileRepository : CachedRepository<Asset>, IApiService<string, Asset>, IDiscoverService<string>
    {
        private readonly IConverter<string, string> identifiersConverter;

        private readonly IConverter<FileDataContract, Asset> assetConverter;

        /// <summary>Initializes a new instance of the <see cref="FileRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make connections with the ArenaNet servers.</param>
        /// <param name="responseConverter">The <see cref="ResponseConverterBase"/> used to convert <see cref="HttpResponseMessage"/> into objects.</param>
        /// <param name="cache">The <see cref="ICache{T}"/> used to cache api responses.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="assetConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public FileRepository(HttpClient httpClient, ResponseConverterBase responseConverter, ICache<Asset> cache, IConverter<string, string> identifiersConverter, IConverter<FileDataContract, Asset> assetConverter)
            : base(httpClient, responseConverter, cache)
        {
            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            if (assetConverter == null)
            {
                throw new ArgumentNullException(nameof(assetConverter));
            }

            this.identifiersConverter = identifiersConverter;
            this.assetConverter = assetConverter;
        }

        /// <inheritdoc />
        public Task<IEnumerable<string>> DiscoverAsync()
        {
            return this.DiscoverAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> DiscoverAsync(CancellationToken cancellationToken)
        {
            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("files").Build();
            return await this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request, cancellationToken), this.identifiersConverter);
        }

        /// <inheritdoc />
        public Task<Asset> GetAsync(string identifier)
        {
            return this.GetAsync(identifier, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Asset> GetAsync(string identifier, CancellationToken cancellationToken)
        {
            Asset cacheItem = this.Cache.Get(i => i.Identifier == identifier).SingleOrDefault();
            if (cacheItem != null)
            {
                return cacheItem;
            }

            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("files").WithIdentifier(identifier).Build();
            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request, cancellationToken), this.assetConverter);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Asset>> GetAsync()
        {
            return this.GetAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Asset>> GetAsync(CancellationToken cancellationToken)
        {
            List<Asset> cacheItems = this.Cache.Get(i => true).ToList();

            IEnumerable<string> idsToQuery = (await this.DiscoverAsync(cancellationToken)).SymmetricExcept(cacheItems.Select(i => i.Identifier));

            return (await this.GetItemsAsync(idsToQuery, this.assetConverter, cancellationToken)).Union(cacheItems);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Asset>> GetAsync(IEnumerable<string> identifiers)
        {
            return this.GetAsync(identifiers, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Asset>> GetAsync(IEnumerable<string> identifiers, CancellationToken cancellationToken)
        {
            IList<string> idList = identifiers as IList<string> ?? identifiers.ToList();
            List<Asset> cacheItems = this.Cache.Get(i => idList.All(id => id != i.Identifier)).ToList();

            if (cacheItems.Count == idList.Count)
            {
                return cacheItems;
            }

            return await this.GetItemsAsync(idList.SymmetricExcept(cacheItems.Select(i => i.Identifier)), this.assetConverter, cancellationToken);
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