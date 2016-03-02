// <copyright file="IRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System.Net.Http;

    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Provides the interface to query objects from the Guild Wars 2 api.</summary>
    /// <typeparam name="TDataContract">The type of date returned by the api.</typeparam>
    /// <typeparam name="TValue">The type of data to convert into.</typeparam>
    public interface IRepository<in TDataContract, out TValue>
    {
        /// <summary>Gets the <see cref="HttpClient"/> used to make connections to the ArenaNetServers.</summary>
        HttpClient Client { get; }

        /// <summary>Gets the <see cref="IResponseConverter"/> used to convert an <see cref="HttpResponseMessage"/> into useable objects.</summary>
        IResponseConverter ResponseConverter { get; }

        /// <summary>Gets the <see cref="IConverter{TSource,TTarget}"/> used to convert objects of type <see cref="TDataContract"/> into object of type <see cref="TValue"/>.</summary>
        IConverter<TDataContract, TValue> ModelConverter { get; }

        /// <summary>Gets the service location without any additional paramters (i.e. culture, identifiers, etc.)</summary>
        IParameterizedBuilder ServiceLocation { get; }
    }
}
