// <copyright file="ContinentConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Continents
{
    using System;

    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Drawing;
    using GW2NET.Maps;

    /// <summary>Converts a <see cref="ContinentDataContract"/> into the corresponding <see cref="Continent"/>.</summary>
    public sealed class ContinentConverter : IConverter<ContinentDataContract, Continent>
    {
        /// <inheritdoc />
        public Continent Convert(ContinentDataContract value, object state)
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

            return new Continent
            {
                ContinentDimensions = new Size2D(value.Dimensions[0], value.Dimensions[1]),
                ContinentId = value.Id,
                FloorIds = value.Floors,
                MaximumZoom = value.MaximumZoom,
                MinimumZoom = value.MinimumZoom,
                Name = value.Name,
                Culture = response.ContentLanguage
            };
        }
    }
}