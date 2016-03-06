// <copyright file="IPagedBuilder.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    /// <summary>
    /// Provides the interface to set the page size of an <see cref="System.Net.Http.HttpRequestMessage"/>.</summary>
    public interface IPagedBuilder : IBaseBuilder
    {
        /// <summary> Sets the page size for a request.</summary>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A <see cref="IBaseBuilder"/> used to build the request.</returns>
        IBaseBuilder WithSize(int pageSize);
    }
}