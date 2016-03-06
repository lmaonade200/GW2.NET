// <copyright file="IBaseBuilder.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    using System.Net.Http;

    /// <summary>
    /// Provides the base builder for a <see cref="HttpRequestMessage"/>.</summary>
    public interface IBaseBuilder
    {
        /// <summary>Builds the <see cref="HttpRequestMessage"/>.</summary>
        /// <returns>The complete <see cref="HttpRequestMessage"/>.</returns>
        HttpRequestMessage Build();
    }
}