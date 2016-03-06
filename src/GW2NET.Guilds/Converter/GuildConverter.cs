// <copyright file="GuildConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Guilds.Converter
{
    using System;

    using GW2NET.Common;
    using GW2NET.Guilds.ApiModels;

    /// <summary>Converts objects of type <see cref="GuildDataModel"/> to objects of type <see cref="Guild"/>.</summary>
    public sealed class GuildConverter : IConverter<GuildDataModel, Guild>
    {
        private readonly IConverter<EmblemDataModel, Emblem> emblemConverter;

        /// <summary>Initializes a new instance of the <see cref="GuildConverter"/> class.</summary>
        /// <param name="emblemConverter">The emblem converter.</param>
        /// <exception cref="ArgumentNullException">Thrown when the emblem converter is null.</exception>
        public GuildConverter(IConverter<EmblemDataModel, Emblem> emblemConverter)
        {
            if (emblemConverter == null)
            {
                throw new ArgumentNullException(nameof(emblemConverter));
            }

            this.emblemConverter = emblemConverter;
        }

        /// <inheritdoc />
        public Guild Convert(GuildDataModel value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Guild guild = new Guild
            {
                Name = value.Name,
                Tag = value.Tag
            };

            Guid id;
            if (Guid.TryParse(value.GuildId, out id))
            {
                guild.GuildId = id;
            }

            EmblemDataModel emblem = value.Emblem;
            if (emblem != null)
            {
                guild.Emblem = this.emblemConverter.Convert(emblem, state);
            }

            return guild;
        }
    }
}