// <copyright file="EmblemTransformationCollectionConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Guilds.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GW2NET.Common;

    /// <summary>Converts objects of type <see cref="T:ICollection{string}"/> to objects of type <see cref="EmblemTransformations"/>.</summary>
    public sealed class EmblemTransformationCollectionConverter : IConverter<ICollection<string>, EmblemTransformations>
    {
        private readonly IConverter<string, EmblemTransformations> emblemTransformationConverter;

        /// <summary>Initializes a new instance of the <see cref="EmblemTransformationCollectionConverter"/> class.</summary>
        /// <param name="emblemTransformationConverter">The emblem transformation converter.</param>
        /// <exception cref="ArgumentNullException">Thrown when the emblem transformation covnerter is null.</exception>
        public EmblemTransformationCollectionConverter(IConverter<string, EmblemTransformations> emblemTransformationConverter)
        {
            if (emblemTransformationConverter == null)
            {
                throw new ArgumentNullException(nameof(emblemTransformationConverter));
            }

            this.emblemTransformationConverter = emblemTransformationConverter;
        }

        /// <inheritdoc />
        public EmblemTransformations Convert(ICollection<string> value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value.Aggregate(default(EmblemTransformations), (current, s) => current | this.emblemTransformationConverter.Convert(s, state));
        }
    }
}