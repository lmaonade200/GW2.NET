﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorModelConverter.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Converts objects of type  to objects of type .
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.V2.Colors
{
    using System;

    using GW2NET.Colors;
    using GW2NET.Common;

    /// <summary>Converts objects of type <see cref="ColorDataContract"/> to objects of type <see cref="ColorModel"/>.</summary>
    public sealed class ColorModelConverter : IConverter<ColorDataContract, ColorModel>
    {
        private readonly IConverter<int[], Color> colorConverter;

        /// <summary>Initializes a new instance of the <see cref="ColorModelConverter"/> class.</summary>
        /// <param name="colorConverter">The converter for <see cref="Color"/>.</param>
        public ColorModelConverter(IConverter<int[], Color> colorConverter)
        {
            if (colorConverter == null)
            {
                throw new ArgumentNullException(nameof(colorConverter));
            }

            this.colorConverter = colorConverter;
        }

        /// <inheritdoc />
        public ColorModel Convert(ColorDataContract value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            ColorModel colorModel = new ColorModel
            {
                Brightness = value.Brightness,
                Contrast = value.Contrast,
                Hue = value.Hue,
                Saturation = value.Saturation,
                Lightness = value.Lightness
            };
            int[] rgb = value.Rgb;
            if (rgb != null && rgb.Length == 3)
            {
                colorModel.Rgb = this.colorConverter.Convert(rgb, value);
            }

            return colorModel;
        }
    }
}