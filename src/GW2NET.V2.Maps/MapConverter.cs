// <copyright file="MapConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Maps
{
    using System;

    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Drawing;
    using GW2NET.Maps;

    /// <summary>Converts objects of type <see cref="MapDataContract"/> to objects of type <see cref="Map"/>.</summary>
    public sealed class MapConverter : IConverter<MapDataContract, Map>
    {
        private readonly IConverter<double[][], Rectangle> rectangleConverter;

        /// <summary>Initializes a new instance of the <see cref="MapConverter"/> class.</summary>
        /// <param name="rectangleConverter">The converter for <see cref="Rectangle"/>.</param>
        /// <exception cref="ArgumentNullException">The value of <paramref name="rectangleConverter"/> is a null reference.</exception>
        public MapConverter(IConverter<double[][], Rectangle> rectangleConverter)
        {
            if (rectangleConverter == null)
            {
                throw new ArgumentNullException(nameof(rectangleConverter));
            }

            this.rectangleConverter = rectangleConverter;
        }

        /// <inheritdoc />
        public Map Convert(MapDataContract value, object state)
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

            Map map = new Map
            {
                Culture = response.ContentLanguage,
                MapName = value.Name,
                MinimumLevel = value.MinimumLevel,
                MaximumLevel = value.MaximumLevel,
                DefaultFloor = value.DefaultFloor,
                Floors = value.Floors,
                RegionId = value.RegionId,
                RegionName = value.RegionName,
                ContinentId = value.ContinentId,
                ContinentName = value.ContinentName,
                MapId = value.Id,
            };

            double[][] mapRectangle = value.MapRectangle;
            if (mapRectangle != null && mapRectangle.Length == 2)
            {
                double[] northWest = mapRectangle[0];
                if (northWest != null && northWest.Length == 2)
                {
                    double[] southEast = mapRectangle[1];
                    if (southEast != null && southEast.Length == 2)
                    {
                        map.MapRectangle = this.rectangleConverter.Convert(mapRectangle, state);
                    }
                }
            }

            double[][] continentRectangle = value.ContinentRectangle;
            if (continentRectangle != null && continentRectangle.Length == 2)
            {
                double[] northWest = continentRectangle[0];
                if (northWest != null && northWest.Length == 2)
                {
                    double[] southEast = continentRectangle[1];
                    if (southEast != null && southEast.Length == 2)
                    {
                        map.ContinentRectangle = this.rectangleConverter.Convert(continentRectangle, state);
                    }
                }
            }

            return map;
        }
    }
}