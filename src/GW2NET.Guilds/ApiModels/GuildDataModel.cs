// <copyright file="GuildDataModel.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

#pragma warning disable 1591
namespace GW2NET.Guilds.ApiModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:1/guild_details")]
    public sealed class GuildDataModel
    {
        [DataMember(Name = "guild_id", Order = 0)]
        public string GuildId { get; set; }

        [DataMember(Name = "guild_name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "tag", Order = 2)]
        public string Tag { get; set; }

        [DataMember(Name = "emblem", Order = 3)]
        public EmblemDataModel Emblem { get; set; }
    }
}