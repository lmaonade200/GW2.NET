﻿// <copyright file="ColorConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Colors
{
    using System;

    using GW2NET.Colors;
    using GW2NET.Common;

    /// <summary>Converts objects of type <see cref="T:int[]"/> to objects of type <see cref="Color"/>.</summary>
    public sealed class ColorConverter : IConverter<int[], Color>
    {
        /// <inheritdoc />
        public Color Convert(int[] value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length != 3)
            {
                throw new ArgumentException("Precondition: value.Length == 3", nameof(value));
            }

            return new Color(value[0], value[1], value[2]);
        }
    }
}