// <copyright file="AggregateListingRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.TradingPost
{
    using System;
    using System.Net.Http;

    using GW2NET.Caching;
    using GW2NET.Commerce;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.TradingPost.ApiModels;

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
    public sealed class AggregateListingRepository : CachedRepository<int, AggregateListing>, IDiscoverable<int>, ICachedRepository<int, AggregateListingDataModel, AggregateListing>
    {
        /// <summary>Initializes a new instance of the <see cref="AggregateListingRepository"/> class.</summary>
        /// <param name="httpClient"></param>
        /// <param name="cache"></param>
        /// <param name="identifiersConverter"></param>
        /// <param name="modelConverter"></param>
        /// <param name="httpResponseConverter"></param>
        public AggregateListingRepository(
            HttpClient httpClient,
            IResponseConverter httpResponseConverter,
            ICache<int, AggregateListing> cache,
            IConverter<int, int> identifiersConverter,
            IConverter<AggregateListingDataModel, AggregateListing> modelConverter)
            : base(httpClient, httpResponseConverter, cache)
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
        public IConverter<AggregateListingDataModel, AggregateListing> ModelConverter { get; }

        /// <inheritdoc />
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("commerce/prices");
            }
        }
    }
}