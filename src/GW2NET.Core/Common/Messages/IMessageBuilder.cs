// <copyright file="IMessageBuilder.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    /// <summary>
    /// Provides the interface for a <see cref="System.Net.Http.HttpRequestMessage"/> on an endpoint
    /// </summary>
    public interface IMessageBuilder : IBaseBuilder
    {
        /// <summary>Sets the endpoint the <see cref="System.Net.Http.HttpRequestMessage"/> targets.</summary>
        /// <param name="endpoint">The name of the endpoint.</param>
        /// <returns>An <see cref="IParameterizedBuilder"/> used to further refine the message.</returns>
        IParameterizedBuilder OnEndpoint(string endpoint);
    }
}