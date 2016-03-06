// <copyright file="Population.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Worlds
{
    /// <summary>Enuerates the population of a server.</summary>
    public enum Population
    {
        /// <summary>Unknown population.</summary>
        Unknown,

        /// <summary>Low population server.</summary>
        Low,

        /// <summary>Medium population server.</summary>
        Medium,

        /// <summary>High population server.</summary>
        High,

        /// <summary>Very high population server.</summary>
        VeryHigh,

        /// <summary>Full server.</summary>
        Full
    }
}