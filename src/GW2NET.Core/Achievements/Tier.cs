// <copyright file="Tier.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements
{
    /// <summary>Describes the achievement's current tier.</summary>
    public class Tier
    {
        /// <summary> The number of "things" (achievement-specific) that must be completed to achieve this tier.</summary>
        public int Count { get; set; }

        /// <summary>The amount of AP awarded for completing this tier.</summary>
        public int Points { get; set; }
        
    }
}