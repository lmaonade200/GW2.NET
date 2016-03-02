// <copyright file="ListingRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Commerce.Listings
{
    using System;
    using System.Net.Http;

    using GW2NET.Caching;
    using GW2NET.Commerce;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.Items;

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
    public sealed class ListingRepository : CachedRepository<int, Listing>, IDiscoverableNew<int>, ICachedRepositoryNew<int, ListingDataContract, Listing>
    {

        /// <summary>Initializes a new instance of the <see cref="ListingRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make connections with the ArenaNet servers.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> used to convert <see cref="HttpResponseMessage"/> into objects.</param>
        /// <param name="cache">The <see cref="ICache{T}"/> used to cache api responses.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="modelConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public ListingRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<int, Listing> cache, IConverter<int, int> identifiersConverter, IConverter<ListingDataContract, Listing> modelConverter)
            : base(httpClient, responseConverter, cache)
        {
            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            if (modelConverter == null)
            {
                throw new ArgumentNullException(nameof(modelConverter));
            }

            this.IdentifiersConverter = identifiersConverter;
            this.ModelConverter = modelConverter;
        }

        /// <inheritdoc />
        public IConverter<int, int> IdentifiersConverter { get; }

        /// <inheritdoc />
        public IConverter<ListingDataContract, Listing> ModelConverter { get; }

        /// <inheritdoc />
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("commerce/listings");
            }
        }
    }
}