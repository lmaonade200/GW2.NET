// <copyright file="ItemRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Items
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
    using GW2NET.Items;
    using GW2NET.V2.Items.Json;

    /// <summary>Represents a repository that retrieves data from the /v2/items interface. See the remarks section for important limitations regarding this implementation.</summary>
    /// <remarks>
    /// This implementation does not retrieve associated entities.
    /// <list type="bullet">
    ///     <item>
    ///         <description><see cref="Item"/>: <see cref="Item.BuildId"/> is always <c>0</c>. Retrieve the build number from the build service.</description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="ISkinnable"/>: <see cref="ISkinnable.DefaultSkin"/> is always <c>null</c>. Use the value of <see cref="ISkinnable.DefaultSkinId"/> to retrieve the skin (applies to <see cref="Armor"/>, <see cref="Backpack"/>, <see cref="GatheringTool"/> and <see cref="Weapon"/>).</description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="IUpgradable"/>: <see cref="IUpgradable.SuffixItem"/> is always <c>null</c>. Use the value of <see cref="IUpgradable.SuffixItemId"/> to retrieve the suffix item (applies to <see cref="Armor"/>, <see cref="Backpack"/>, <see cref="Trinket"/> and <see cref="Weapon"/>).</description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="IUpgradable"/>: <see cref="IUpgradable.SecondarySuffixItem"/> is always <c>null</c>. Use the value of <see cref="IUpgradable.SecondarySuffixItemId"/> to retrieve the secondary suffix item (applies to <see cref="Armor"/>, <see cref="Backpack"/>, <see cref="Trinket"/> and <see cref="Weapon"/>).</description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="InfusionSlot"/>: <see cref="InfusionSlot.Item"/> is always <c>null</c>. Use the value of <see cref="InfusionSlot.ItemId"/> to retrieve the infusion item.</description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="DyeUnlocker"/>: <see cref="DyeUnlocker.Color"/> is always <c>null</c>. Use the value of <see cref="DyeUnlocker.ColorId"/> to retrieve the color.</description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="CraftingRecipeUnlocker"/>: <see cref="CraftingRecipeUnlocker.Recipe"/> is always <c>null</c>. Use the value of <see cref="CraftingRecipeUnlocker.RecipeId"/> to retrieve the recipe.</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public class ItemRepository : CachedRepository<Item>, IApiService<int, Item>, IDiscoverService<int>, ILocalizable
    {

        private readonly IConverter<int, int> identifiersConverter;

        private readonly IConverter<ItemDTO, Item> itemConverter;

        /// <summary>Initializes a new instance of the <see cref="ItemRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make connections with the ArenaNet servers.</param>
        /// <param name="responseConverter">The <see cref="ResponseConverterBase"/> used to convert <see cref="HttpResponseMessage"/> into objects.</param>
        /// <param name="cache">The <see cref="ICache{T}"/> used to cache api responses.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="itemConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public ItemRepository(
            HttpClient httpClient,
            ResponseConverterBase responseConverter,
            ICache<Item> cache,
            IConverter<int, int> identifiersConverter,
            IConverter<ItemDTO, Item> itemConverter)
            : base(httpClient, responseConverter, cache)
        {
            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            if (responseConverter == null)
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
            var request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("items").Build();
            return await this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request, cancellationToken), this.identifiersConverter);
        }

        /// <inheritdoc />
        public Task<Item> GetAsync(int identifier)
        {
            return this.GetAsync(identifier, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Item> GetAsync(int identifier, CancellationToken cancellationToken)
        {
            var cacheItem = this.Cache.Get(i => i.ItemId == identifier).SingleOrDefault();
            if (cacheItem != null)
            {
                return cacheItem;
            }

            var request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("items").WithIdentifier(identifier).Build();
            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request, cancellationToken), this.itemConverter);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Item>> GetAsync()
        {
            return this.GetAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Item>> GetAsync(CancellationToken cancellationToken)
        {
            var cacheItems = this.Cache.Get(i => true).ToList();
            var idsToQuery = (await this.DiscoverAsync(cancellationToken)).SymmetricExcept(cacheItems.Select(i => i.ItemId));

            return await this.GetItemsAsync(idsToQuery, this.itemConverter, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Item>> GetAsync(IEnumerable<int> identifiers)
        {
            return this.GetAsync(identifiers, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Item>> GetAsync(IEnumerable<int> identifiers, CancellationToken cancellationToken)
        {
            var ids = identifiers as IList<int> ?? identifiers.ToList();
            var cacheItems = this.Cache.Get(i => ids.All(id => id != i.ItemId)).ToList();
            if (ids.Count == cacheItems.Count)
            {
                return cacheItems;
            }

            return await this.GetItemsAsync(ids.SymmetricExcept(cacheItems.Select(i => i.ItemId)), this.itemConverter, cancellationToken);
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