// <copyright file="ExchangeDataModel.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

#pragma warning disable 1591
namespace GW2NET.TradingPost.ApiModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/commerce/exchange")]
    public class ExchangeDataModel
    {
        [DataMember(Order = 0, Name = "coins_per_gem")]
        public int CoinsPerGem { get; set; }

        [DataMember(Order = 1, Name = "quantity")]
        public int Quantity { get; set; }
    }
}