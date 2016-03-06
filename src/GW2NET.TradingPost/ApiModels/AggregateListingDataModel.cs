// <copyright file="AggregateListingDataModel.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

#pragma warning disable 1591
namespace GW2NET.TradingPost.ApiModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/commerce/prices")]
    public sealed class AggregateListingDataModel
    {
        [DataMember(Name = "buys", Order = 1)]
        public AggregateOfferDataModel BuyOffers { get; set; }

        [DataMember(Name = "id", Order = 0)]
        public int Id { get; set; }

        [DataMember(Name = "sells", Order = 2)]
        public AggregateOfferDataModel SellOffers { get; set; }

        [DataMember(Name = "whitelisted", Order = 3)]
        public bool Whitelisted { get; set; }
    }
}