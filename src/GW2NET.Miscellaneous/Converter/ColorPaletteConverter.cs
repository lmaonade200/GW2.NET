// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPaletteConverter.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Miscellaneous.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Colors;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Miscellaneous.ApiModels;

    /// <summary>Converts objects of type <see cref="ColorDataModel"/> to objects of type <see cref="ColorPalette"/>.</summary>
    public sealed class ColorPaletteConverter : IConverter<ColorPaletteDataModel, ColorPalette>
    {
        private readonly IConverter<int[], Color> colorConverter;

        private readonly IConverter<ColorDataModel, ColorModel> colorModelConverter;

        /// <summary>Initializes a new instance of the <see cref="ColorPaletteConverter"/> class.</summary>
        /// <param name="colorConverter">The converter for <see cref="Color"/>.</param>
        /// <param name="colorModelConverter">The converter for <see cref="ColorModel"/>.</param>
        public ColorPaletteConverter(IConverter<int[], Color> colorConverter, IConverter<ColorDataModel, ColorModel> colorModelConverter)
        {
            if (colorConverter == null)
            {
                throw new ArgumentNullException(nameof(colorConverter));
            }

            if (colorModelConverter == null)
            {
                throw new ArgumentNullException(nameof(colorModelConverter));
            }

            this.colorConverter = colorConverter;
            this.colorModelConverter = colorModelConverter;
        }

        /// <inheritdoc />
        public ColorPalette Convert(ColorPaletteDataModel value, object state)
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

            ColorPalette entity = new ColorPalette
            {
                ColorId = value.Id,
                Name = value.Name,
                BaseRgb = this.colorConverter.Convert(value.BaseRgb, value),
                Cloth = this.colorModelConverter.Convert(value.Cloth, value),
                Leather = this.colorModelConverter.Convert(value.Leather, value),
                Metal = this.colorModelConverter.Convert(value.Metal, value),
                Culture = response.ContentLanguage
            };

            if (value.Categories == null)
            {
                entity.Categories = new List<string>(0);
            }
            else
            {
                List<string> values = new List<string>(value.Categories.Length);
                values.AddRange(value.Categories);
                entity.Categories = values;
            }

            return entity;
        }
    }
}