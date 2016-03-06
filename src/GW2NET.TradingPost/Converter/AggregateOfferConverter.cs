// <copyright file="AggregateOfferConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.TradingPost.Converter
{
    using System;

    using GW2NET.Commerce;
    using GW2NET.Common;
    using GW2NET.TradingPost.ApiModels;

    /// <summary>Converts objects of type <see cref="AggregateOfferDataModel"/> to objects of type <see cref="AggregateOffer"/>.</summary>
    public sealed class AggregateOfferConverter : IConverter<AggregateOfferDataModel, AggregateOffer>
    {
        /// <summary>Converts the given object of type <see cref="AggregateOfferDataModel"/> to an object of type <see cref="AggregateOffer"/>.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public AggregateOffer Convert(AggregateOfferDataModel value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new AggregateOffer
            {
                Quantity = value.Quantity,
                UnitPrice = value.UnitPrice
            };
        }
    }
}