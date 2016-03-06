// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DamageTypeConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="string" /> to objects of type <see cref="DamageType" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Diagnostics;

    using GW2NET.Common;
    using GW2NET.Items.Weapons;

    /// <summary>Converts objects of type <see cref="string" /> to objects of type <see cref="DamageType" />.</summary>
    public sealed class DamageTypeConverter : IConverter<string, DamageType>
    {
        /// <summary>Converts the given object of type <see cref="string" /> to an object of type <see cref="DamageType" />.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public DamageType Convert(string value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            DamageType result;
            if (Enum.TryParse(value, true, out result))
            {
                return result;
            }

            Debug.Assert(false, "Unknown DamageType: " + value);
            return default(DamageType);
        }
    }
}