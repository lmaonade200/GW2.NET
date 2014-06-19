// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapServiceRequest.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Represents a request for details regarding maps in the game.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2DotNET.V1.Maps
{
    using GW2DotNET.Extensions;
    using GW2DotNET.V1.Common;

    /// <summary>Represents a request for details regarding maps in the game.</summary>
    public class MapServiceRequest : ServiceRequest
    {
        /// <summary>Infrastructure. Stores a parameter.</summary>
        private int? mapId;

        /// <summary>Initializes a new instance of the <see cref="MapServiceRequest" /> class.</summary>
        public MapServiceRequest()
            : base(Services.Maps)
        {
        }

        /// <summary>Gets or sets the map filter.</summary>
        public int? MapId
        {
            get
            {
                return this.mapId;
            }

            set
            {
                this.FormData["map_id"] = (this.mapId = value).ToStringInvariant();
            }
        }
    }
}