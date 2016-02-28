// <copyright file="AggregateListingRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Commerce.Prices
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Caching;
    using GW2NET.Commerce;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.Items;

    /// <summary>Represents a repository that retrieves data from the /v2/commerce/prices interface. See the remarks section for important limitations regarding this implementation.</summary>
    /// <remarks>
    /// This implementation does not retrieve associated entities.
    /// <list type="bullet">
    ///     <item>
    ///         <term><see cref="AggregateListing.Item"/>:</term>
    ///         <description>Always <c>null</c>. Use the value of <see cref="AggregateListing.ItemId"/> to retrieve the <see cref="Item"/>.</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public class AggregateListingRepository : CachedRepository<AggregateListing>, IApiService<int, AggregateListing>, IDiscoverService<int>
    {
        private readonly IConverter<int, int> identifiersConverter;

        private readonly IConverter<AggregateListingDataContract, AggregateListing> aggregateListingConverter;


        /// <summary>Initializes a new instance of the <see cref="AggregateListingRepository"/> class.</summary>
        /// <param name="httpClient"></param>
        /// <param name="cache"></param>
        /// <param name="identifiersConverter"></param>
        /// <param name="aggregateListingConverter"></param>
        /// <param name="httpResponseConverter"></param>
        public AggregateListingRepository(
            HttpClient httpClient,
            ResponseConverterBase httpResponseConverter,
            ICache<AggregateListing> cache,
            IConverter<int, int> identifiersConverter,
            IConverter<AggregateListingDataContract, AggregateListing> aggregateListingConverter)
            : base(httpClient, httpResponseConverter, cache)
        {
            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            if (aggregateListingConverter == null)
            {
                throw new ArgumentNullException(nameof(aggregateListingConverter));
            }

            this.identifiersConverter = identifiersConverter;
            this.aggregateListingConverter = aggregateListingConverter;
        }

        /// <inheritdoc />
        public Task<IEnumerable<int>> DiscoverAsync()
        {
            return this.DiscoverAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<int>> DiscoverAsync(CancellationToken cancellationToken)
        {
            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("commerce/prices").Build();
            return await this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request, cancellationToken), this.identifiersConverter);
        }

        /// <inheritdoc />
        public Task<AggregateListing> GetAsync(int identifier)
        {
            return this.GetAsync(identifier, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<AggregateListing> GetAsync(int identifier, CancellationToken cancellationToken)
        {
            AggregateListing cacheItem = this.Cache.Get(i => i.ItemId == identifier).SingleOrDefault();
            if (cacheItem != null)
            {
                return cacheItem;
            }

            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("commerce/prices").WithIdentifier(identifier).Build();

            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request, cancellationToken), this.aggregateListingConverter);
        }

        /// <inheritdoc />
        public Task<IEnumerable<AggregateListing>> GetAsync()
        {
            return this.GetAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AggregateListing>> GetAsync(CancellationToken cancellationToken)
        {
            // We need to do this, so we get all non stale items
            List<AggregateListing> cacheListings = this.Cache.Get(l => true).ToList();

            IEnumerable<int> idsToQuery = (await this.DiscoverAsync(cancellationToken)).SymmetricExcept(cacheListings.Select(l => l.ItemId));

            IEnumerable<AggregateListing> listings = (await this.GetAsync(idsToQuery, cancellationToken)).Union(cacheListings);

            return listings.Union(cacheListings);
        }

        /// <inheritdoc />
        public Task<IEnumerable<AggregateListing>> GetAsync(IEnumerable<int> identifiers)
        {
            return this.GetAsync(identifiers, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AggregateListing>> GetAsync(IEnumerable<int> identifiers, CancellationToken cancellationToken)
        {
            IList<int> ids = identifiers as IList<int> ?? identifiers.ToList();

            // We need to do this, so we get all non stale items
            List<AggregateListing> cacheListings = this.Cache.Get(l => ids.All(id => id == l.ItemId)).ToList();
            if (cacheListings.Count == ids.Count)
            {
                return cacheListings;
            }

            IEnumerable<IEnumerable<int>> idListList = this.CalculatePages((await this.DiscoverAsync(cancellationToken)).SymmetricExcept(cacheListings.Select(l => l.ItemId)));

            ConcurrentBag<AggregateListing> aggregateListings = new ConcurrentBag<AggregateListing>(cacheListings);
            Parallel.ForEach(idListList,
                             async idList =>
                             {
                                 HttpRequestMessage request = ApiMessageBuilder.Init()
                                                      .Version(ApiVersion.V2)
                                                      .OnEndpoint("commerce/prices")
                                                      .WithIdentifiers(idList)
                                                      .Build();
                                 HttpResponseMessage response = await this.Client.SendAsync(request, cancellationToken);

                                 foreach (AggregateListing aggregateListing in await this.ResponseConverter.ConvertSetAsync(response, this.aggregateListingConverter))
                                 {
                                     aggregateListings.Add(aggregateListing);
                                 }
                             });

            return aggregateListings;
        }
    }
}