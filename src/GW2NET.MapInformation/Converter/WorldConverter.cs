// <copyright file="WorldConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.MapInformation.Converter
{
    using System;

    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.MapInformation.ApiModels;
    using GW2NET.Worlds;

    /// <summary>Converts objects of type <see cref="WorldDataContract"/> to objects of type <see cref="World"/>.</summary>
    public sealed class WorldConverter : IConverter<WorldDataContract, World>
    {
        /// <inheritdoc />
        public World Convert(WorldDataContract value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            ApiMetadata metadata = state as ApiMetadata;
            if (metadata == null)
            {
                throw new ArgumentException("Could not cast to ApiMetadata", nameof(state));
            }

            World world = new World
            {
                WorldId = value.Id,
                Name = value.Name,
                Culture = metadata.ContentLanguage
            };

            if (value.Population != null)
            {
                Population population;
                world.Population = Enum.TryParse(value.Population, true, out population) ? population : Population.Unknown;
            }

            return world;
        }
    }
}