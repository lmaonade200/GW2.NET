// <copyright file="Header.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.ChatLinks.Interop
{
    /// <summary>Enumerates the header bytes.</summary>
    public enum Header : byte
    {
        /// <summary>An unknown header.</summary>
        Unknown = 0,

        /// <summary>A coin chat link.</summary>
        Coin = 1,

        /// <summary>An item chat link.</summary>
        Item = 2,

        /// <summary>A text chat link.</summary>
        Text = 3,

        /// <summary>A map chat link.</summary>
        Map = 4,

        /// <summary>A pvp chat link.</summary>
        PvP = 5,

        /// <summary>A skill chat link.</summary>
        Skill = 7,

        /// <summary>A trait chat link.</summary>
        Trait = 8,

        /// <summary>A player chat link.</summary>
        Player = 9,

        /// <summary>A recipe chat link.</summary>
        Recipe = 10,

        /// <summary>A skin chat link.</summary>
        Skin = 11,

        /// <summary>An outfit chat link.</summary>
        Outfit = 12
    }
}