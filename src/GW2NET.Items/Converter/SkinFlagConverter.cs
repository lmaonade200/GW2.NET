// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkinFlagConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="string" /> to objects of type <see cref="SkinFlags" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Diagnostics;

    using GW2NET.Common;
    using GW2NET.Skins;

    /// <summary>Converts objects of type <see cref="string"/> to objects of type <see cref="SkinFlags"/>.</summary>
    public sealed class SkinFlagConverter : IConverter<string, SkinFlags>
    {
        /// <inheritdoc />
        public SkinFlags Convert(string value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            SkinFlags result;
            if (Enum.TryParse(value, true, out result))
            {
                return result;
            }

            Debug.Assert(false, "Unknown SkinFlags: " + value);
            return default(SkinFlags);
        }
    }
}