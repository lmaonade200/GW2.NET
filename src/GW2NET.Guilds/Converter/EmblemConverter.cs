// <copyright file="EmblemConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Guilds.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Guilds.ApiModels;

    /// <summary>Converts objects of type <see cref="EmblemDataModel"/> to objects of type <see cref="Emblem"/>.</summary>
    public sealed class EmblemConverter : IConverter<EmblemDataModel, Emblem>
    {
        private readonly IConverter<ICollection<string>, EmblemTransformations> emblemTransformationsConverter;

        /// <summary>Initializes a new instance of the <see cref="EmblemConverter"/> class.</summary>
        /// <param name="emblemTransformationsConverter">The emblem transformation converter.</param>
        /// <exception cref="ArgumentNullException">Thrown when the inner converter is null.</exception>
        public EmblemConverter(IConverter<ICollection<string>, EmblemTransformations> emblemTransformationsConverter)
        {
            if (emblemTransformationsConverter == null)
            {
                throw new ArgumentNullException(nameof(emblemTransformationsConverter));
            }

            this.emblemTransformationsConverter = emblemTransformationsConverter;
        }

        /// <inheritdoc />
        public Emblem Convert(EmblemDataModel value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Emblem emblem = new Emblem
            {
                BackgroundId = value.BackgroundId,
                ForegroundId = value.ForegroundId,
                BackgroundColorId = value.BackgroundColorId,
                ForegroundPrimaryColorId = value.ForegroundPrimaryColorId,
                ForegroundSecondaryColorId = value.ForegroundSecondaryColorId
            };
            string[] flags = value.Flags;
            if (flags != null)
            {
                emblem.Flags = this.emblemTransformationsConverter.Convert(flags, state);
            }

            return emblem;
        }
    }
}