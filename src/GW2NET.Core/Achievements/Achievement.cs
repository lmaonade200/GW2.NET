// <copyright file="Achievement.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements
{
    using System.Collections.Generic;

    /// <summary>Describes a Guild Ward 2 achivement.</summary>
    /// <remarks>
    /// <para>This object includes optional values, which are represented by a null value.</para>
    /// </remarks>
    public class Achievement
    {
        /// <summary>The achievement id.</summary>
        public int Id { get; set; }

        /// <summary>(Optional) The achievement icon.</summary>
        public string Icon { get; set; }

        /// <summary>The achievement name.</summary>
        public string Name { get; set; }

        /// <summary>The achievement description.</summary>
        public string Description { get; set; }

        /// <summary>The achievement requirements.</summary>
        public string Requirement { get; set; }

        /// <summary>The achievement type.</summary>
        public AchievementType Type { get; set; }

        /// <summary>The achievement categories.</summary>
        public AchievementFlags Flags { get; set; }

        /// <summary>The achievement tiers.</summary>
        public IEnumerable<Tier> Tiers { get; set; }

        /// <summary>(Optional) The achievement rewards.</summary>
        public IEnumerable<Reward> Rewards { get; set; }

        /// <summary>(Optional) Additional information pretending the achievement of the achievement.</summary>
        public IEnumerable<AchievementBit>[] Bits { get; set; }

        /// <summary>The maximum number of AP that can be rewarded by the achievement.</summary>
        public int? PointCap { get; set; }
    }
}