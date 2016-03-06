// <copyright file="IDiscoverable{TIn,TOut}.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System.Net.Http;

    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Provides the interface to discover objects on the Guild Wars 2 api.</summary>
    /// <typeparam name="TIn">The type of key used on the api server.</typeparam>
    /// <typeparam name="TOut">The type of key used in the locale application.</typeparam>
    public interface IDiscoverable<in TIn, out TOut>
    {
        /// <summary>Gets the <see cref="HttpClient"/> used to make connections to the ArenaNetServers.</summary>
        HttpClient Client { get; }

        /// <summary>Gets the <see cref="IResponseConverter"/> used to convert an <see cref="HttpResponseMessage"/> into useable objects.</summary>
        IResponseConverter ResponseConverter { get; }

        /// <summary>Gets the <see cref="IConverter{TSource,TTarget}"/> used to convert identifiers.</summary>
        IConverter<TIn, TOut> IdentifiersConverter { get; }

        /// <summary>Gets the service location without any additional paramters (i.e. culture, identifiers, etc.)</summary>
        IParameterizedBuilder ServiceLocation { get; }
    }
}