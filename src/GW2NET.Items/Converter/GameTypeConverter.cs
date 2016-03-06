// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameTypeConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="string" /> to objects of type <see cref="GameTypes" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Diagnostics;

    using GW2NET.Common;

    /// <summary>Converts objects of type <see cref="string" /> to objects of type <see cref="GameTypes" />.</summary>
    public sealed class GameTypeConverter : IConverter<string, GameTypes>
    {
        /// <summary>Converts the given object of type <see cref="string" /> to an object of type <see cref="GameTypes" />.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public GameTypes Convert(string value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            GameTypes result;
            if (Enum.TryParse(value, true, out result))
            {
                return result;
            }

            Debug.Assert(false, "Unknown GameTypes: " + value);
            return default(GameTypes);
        }
    }
}