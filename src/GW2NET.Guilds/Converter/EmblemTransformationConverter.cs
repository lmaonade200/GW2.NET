// <copyright file="EmblemTransformationConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Guilds.Converter
{
    using System;

    using GW2NET.Common;

    /// <summary>Converts objects of type <see cref="string"/> to objects of type <see cref="EmblemTransformations"/>.</summary>
    public sealed class EmblemTransformationConverter : IConverter<string, EmblemTransformations>
    {
        /// <inheritdoc />
        public EmblemTransformations Convert(string value, object state)
        {
            EmblemTransformations result;
            return Enum.TryParse(value, true, out result) ? result : default(EmblemTransformations);
        }
    }
}