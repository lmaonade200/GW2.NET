// <copyright file="ListingConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Commerce.Listings
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Commerce;
    using GW2NET.Common;
    using GW2NET.Common.Converters;

    /// <summary>Converts objects of type <see cref="ListingDataContract"/> to objects of type <see cref="Listing"/>.</summary>
    public sealed class ListingConverter : IConverter<ListingDataContract, Listing>
    {
        private readonly IConverter<ICollection<ListingOfferDataContract>, ICollection<Offer>> offerCollectionConverter;

        /// <summary>Initializes a new instance of the <see cref="ListingConverter"/> class.</summary>
        /// <param name="offerCollectionConverter"></param>
        public ListingConverter(IConverter<ICollection<ListingOfferDataContract>, ICollection<Offer>> offerCollectionConverter)
        {
            if (offerCollectionConverter == null)
            {
                throw new ArgumentNullException(nameof(offerCollectionConverter));
            }

            this.offerCollectionConverter = offerCollectionConverter;
        }

        /// <summary>Converts the given object of type <see cref="ListingDataContract"/> to an object of type <see cref="Listing"/>.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public Listing Convert(ListingDataContract value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            ApiMetadata response = state as ApiMetadata;
            if (response == null)
            {
                throw new ArgumentException("Could not cast to ApiMetadata", nameof(state));
            }

            return new Listing
            {
                ItemId = value.Id,
                BuyOffers = this.offerCollectionConverter.Convert(value.BuyOffers, value),
                SellOffers = this.offerCollectionConverter.Convert(value.SellOffers, value),
                Timestamp = response.RequestDate
            };
        }
    }
}