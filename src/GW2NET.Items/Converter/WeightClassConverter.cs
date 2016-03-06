// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WeightClassConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="string" /> to objects of type <see cref="WeightClass" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Diagnostics;

    using GW2NET.Common;
    using GW2NET.Items.Armors;

    /// <summary>Converts objects of type <see cref="string" /> to objects of type <see cref="WeightClass" />.</summary>
    public sealed class WeightClassConverter : IConverter<string, WeightClass>
    {
        /// <summary>Converts the given object of type <see cref="string" /> to an object of type <see cref="WeightClass" />.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public WeightClass Convert(string value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            WeightClass result;
            if (Enum.TryParse(value, true, out result))
            {
                return result;
            }

            Debug.Assert(false, "Unknown WeightClass: " + value);
            return default(WeightClass);
        }
    }
}