// <copyright file="BuildRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Miscellaneous
{
    using System;
    using System.Net.Http;

    using GW2NET.Builds;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.Miscellaneous.ApiModels;

    /// <summary>Represents a service that retrieves data from the /v1/build.json interface.</summary>
    /// <remarks>See <a href="http://wiki.guildwars2.com/wiki/API:2/build">wiki</a> for more information.</remarks>
    public sealed class BuildRepository : RepositoryBase, IDiscoverable<BuildDataModel, Build>
    {
        /// <summary>Initializes a new instance of the <see cref="BuildRepository"/> class.</summary>
        /// <param name="httpClient"></param>
        /// <param name="identifiersConverter">The converter for <see cref="Build"/>.</param>
        /// <param name="responseConverter"></param>
        /// <exception cref="ArgumentNullException">The value of <paramref name="httpClient"/> or <paramref name="identifiersConverter"/> is a null reference.</exception>
        public BuildRepository(HttpClient httpClient, IResponseConverter responseConverter, IConverter<BuildDataModel, Build> identifiersConverter)
            : base(httpClient, responseConverter)
        {
            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            this.IdentifiersConverter = identifiersConverter;
        }
        
        /// <summary>Gets the <see cref="IConverter{TSource,TTarget}"/> used to convert identifiers.</summary>
        public IConverter<BuildDataModel, Build> IdentifiersConverter { get; }

        /// <summary>Gets the service location without any additional paramters (i.e. culture, identifiers, etc.)</summary>
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("build");
            }
        }
    }
}
