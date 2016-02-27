// <copyright file="ListingRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Commerce.Listings
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
    using GW2NET.V2.Commerce.Listings.Json;

    /// <summary>Represents a repository that retrieves data from the /v2/commerce/listings interface. See the remarks section for important limitations regarding this implementation.</summary>
    /// <remarks>
    /// This implementation does not retrieve associated entities.
    /// <list type="bullet">
    ///     <item>
    ///         <term><see cref="Listing.Item"/>:</term>
    ///         <description>Always <c>null</c>. Use the value of <see cref="Listing.ItemId"/> to retrieve the <see cref="Item"/>.</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public class ListingRepository : CachedRepository<Listing>, IApiService<int, Listing>, IDiscoverService<int>
    {

        private readonly IConverter<int, int> identifiersConverter;

        private readonly IConverter<ListingDataContract, Listing> listingConverter;

        /// <summary>Initializes a new instance of the <see cref="ListingRepository"/> class.</summary>
        /// <param name="serviceClient"></param>
        /// <param name="identifiersConverter"></param>
        /// <param name="listingConverter"></param>
        /// <param name="responseConverter"></param>
        /// <param name="cache"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ListingRepository(HttpClient serviceClient, ResponseConverterBase responseConverter, ICache<Listing> cache, IConverter<int, int> identifiersConverter, IConverter<ListingDataContract, Listing> listingConverter)
            : base(serviceClient, responseConverter, cache)
        {
            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            if (listingConverter == null)
            {
                throw new ArgumentNullException(nameof(listingConverter));
            }

            this.identifiersConverter = identifiersConverter;
            this.listingConverter = listingConverter;
        }

        /// <inheritdoc />
        public Task<IEnumerable<int>> DiscoverAsync()
        {
            return this.DiscoverAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<int>> DiscoverAsync(CancellationToken cancellationToken)
        {
            var request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("commerce/listings").Build();

            return await this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request, cancellationToken), this.identifiersConverter);
        }

        /// <inheritdoc />
        public Task<Listing> GetAsync(int identifier)
        {
            return this.GetAsync(identifier, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Listing> GetAsync(int identifier, CancellationToken cancellationToken)
        {
            var cacheElem = this.Cache.Get(l => l.ItemId == identifier).Single();
            if (cacheElem != null)
            {
                return cacheElem;
            }

            var request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("commerce/listings").WithIdentifier(identifier).Build();

            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request, cancellationToken), this.listingConverter);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Listing>> GetAsync()
        {
            return this.GetAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Listing>> GetAsync(CancellationToken cancellationToken)
        {
            var ids = this.DiscoverAsync(cancellationToken);
            var cacheItems = this.Cache.Value.ToList();
            var idsToQuery = (await ids).SymmetricExcept(cacheItems.Select(i => i.ItemId)).ToList();
            if (idsToQuery.Count == 0)
            {
                return cacheItems;
            }

            var idListList = this.CalculatePages(idsToQuery);

            ConcurrentBag<Listing> listings = new ConcurrentBag<Listing>(cacheItems);
            Parallel.ForEach(idListList,
                             async idList =>
                             {
                                 var request =
                                     ApiMessageBuilder.Init()
                                                      .Version(ApiVersion.V2)
                                                      .OnEndpoint("commerce/listings")
                                                      .WithIdentifiers(idList)
                                                      .Build();

                                 var response = this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request, cancellationToken), this.listingConverter);

                                 foreach (var listing in await response)
                                 {
                                     listings.Add(listing);
                                 }
                             });
            return listings;
        }

        /// <inheritdoc />
        public Task<IEnumerable<Listing>> GetAsync(IEnumerable<int> identifiers)
        {
            return this.GetAsync(identifiers, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Listing>> GetAsync(IEnumerable<int> identifiers, CancellationToken cancellationToken)
        {
            IList<int> ids = identifiers as IList<int> ?? identifiers.ToList();

            var cacheItems = this.Cache.Get(l => ids.All(i => i != l.ItemId)).ToList();
            if (cacheItems.Count == ids.Count)
            {
                return cacheItems;
            }

            var idListList = this.CalculatePages(ids.SymmetricExcept(cacheItems.Select(i => i.ItemId)).ToList());

            ConcurrentBag<Listing> listings = new ConcurrentBag<Listing>(cacheItems);
            Parallel.ForEach(idListList,
                             async idList =>
                             {
                                 var request =
                                     ApiMessageBuilder.Init()
                                                      .Version(ApiVersion.V2)
                                                      .OnEndpoint("commerce/listings")
                                                      .WithIdentifiers(idList)
                                                      .Build();

                                 var response = this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request, cancellationToken), this.listingConverter);

                                 foreach (var listing in await response)
                                 {
                                     listings.Add(listing);
                                 }
                             });
            return listings;
        }
    }
}