// <copyright file="GW2ApiVersion.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    /// <summary>Enumerates the api versions available.</summary>
    public enum ApiVersion
    {
        /// <summary>Version 1 of the public api.</summary>
        V1 = 0,

        /// <summary>Version 2 of the public api.</summary>
        V2 = 1 << 0
    }
}