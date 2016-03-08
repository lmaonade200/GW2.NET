// <copyright file="AchievementFlags.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements
{
    using System;

    /// <summary>Describes possible achievement categories.</summary>
    [Flags]
    public enum AchievementFlags
    {
        /// <summary>Can only get progress in PvP or WvW.</summary>
        Pvp = 0,

        /// <summary>Is a meta achievement.</summary>
        CategoryDisplay = 1,

        /// <summary>Affects in-game UI collation.</summary>
        MoveToTop = 1 << 1,

        /// <summary>Doesn't appear in the "nearly complete" UI.</summary>
        IgnoreNearlyComplete = 1 << 2,

        /// <summary>Can be repeated multiple times.</summary>
        Repeatable = 1 << 3
    }
}