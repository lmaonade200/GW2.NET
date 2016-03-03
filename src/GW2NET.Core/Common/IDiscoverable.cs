// <copyright file="IDiscoverable.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System.Net.Http;

    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Provides the interface to query objects from the Guild Wars 2 api.</summary>
    /// <typeparam name="T">The type of key used to identify items in the cache.</typeparam>
    public interface IDiscoverable<T>
    {
        /// <summary>Gets the <see cref="HttpClient"/> used to make connections to the ArenaNetServers.</summary>
        HttpClient Client { get; }

        /// <summary>Gets the <see cref="IResponseConverter"/> used to convert an <see cref="HttpResponseMessage"/> into useable objects.</summary>
        IResponseConverter ResponseConverter { get; }

        /// <summary>Gets the <see cref="IConverter{TSource,TTarget}"/> used to convert identifiers.</summary>
        IConverter<T, T> IdentifiersConverter { get; }

        /// <summary>Gets the service location without any additional paramters (i.e. culture, identifiers, etc.)</summary>
        IParameterizedBuilder ServiceLocation { get; }
    }
}
