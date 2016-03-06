// <copyright file="BuildConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Miscellaneous.Converter
{
    using System;

    using GW2NET.Builds;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Miscellaneous.ApiModels;

    /// <summary>Converts objects of type <see cref="BuildDataModel"/> to objects of type <see cref="Build"/>.</summary>
    public sealed class BuildConverter : IConverter<BuildDataModel, Build>
    {
        /// <inheritdoc />
        public Build Convert(BuildDataModel value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            ApiMetadata response = state as ApiMetadata;
            if (response == null)
            {
                throw new ArgumentException("Could not cast to ApiMetadata", nameof(state));
            }

            return new Build
            {
                BuildId = value.BuildId,
                Timestamp = response.RequestDate
            };
        }
    }
}