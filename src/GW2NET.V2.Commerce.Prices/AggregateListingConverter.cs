// <copyright file="AggregateListingConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Commerce.Prices
{
    using System;

    using GW2NET.Commerce;
    using GW2NET.Common;

    /// <summary>Converts objects of type <see cref="AggregateListingDataContract"/> to objects of type <see cref="AggregateListing"/>.</summary>
    public sealed class AggregateListingConverter : IConverter<AggregateListingDataContract, AggregateListing>
    {
        private readonly IConverter<AggregateOfferDataContract, AggregateOffer> aggregateOfferConverter;

        /// <summary>Initializes a new instance of the <see cref="AggregateListingConverter"/> class.</summary>
        /// <param name="aggregateOfferConverter">The converter for <see cref="AggregateOffer"/>.</param>
        public AggregateListingConverter(IConverter<AggregateOfferDataContract, AggregateOffer> aggregateOfferConverter)
        {
            if (aggregateOfferConverter == null)
            {
                throw new ArgumentNullException("aggregateOfferConverter");
            }

            this.aggregateOfferConverter = aggregateOfferConverter;
        }

        /// <inheritdoc />
        public AggregateListing Convert(AggregateListingDataContract value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (state == null)
            {
                throw new ArgumentNullException("state", "Precondition: state is IResponse");
            }

            IResponse response = state as IResponse;
            if (response == null)
            {
                throw new ArgumentException("Precondition: state is IResponse", "state");
            }

            AggregateListing aggregateListing = new AggregateListing
            {
                ItemId = value.Id,
                Timestamp = response.Date,
                Whitelisted = value.Whitelisted
            };
            if (value.BuyOffers != null)
            {
                aggregateListing.BuyOffers = this.aggregateOfferConverter.Convert(value.BuyOffers, value);
            }

            if (value.SellOffers != null)
            {
                aggregateListing.SellOffers = this.aggregateOfferConverter.Convert(value.SellOffers, value);
            }

            return aggregateListing;
        }
    }
}