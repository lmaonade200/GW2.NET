// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameTypeCollectionConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="T:ICollection{string}" /> to objects of type <see cref="GameTypes" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;

    /// <summary>Converts objects of type <see cref="T:ICollection{string}" /> to objects of type <see cref="GameTypes" />.</summary>
    public sealed class GameTypeCollectionConverter : IConverter<ICollection<string>, GameTypes>
    {
        private readonly IConverter<string, GameTypes> gameTypeConverter;

        /// <summary>Initializes a new instance of the <see cref="GameTypeCollectionConverter" /> class.</summary>
        /// <param name="gameTypeConverter">The converter for <see cref="GameTypes" />.</param>
        public GameTypeCollectionConverter(IConverter<string, GameTypes> gameTypeConverter)
        {
            if (gameTypeConverter == null)
            {
                throw new ArgumentNullException(nameof(gameTypeConverter));
            }

            this.gameTypeConverter = gameTypeConverter;
        }

        /// <summary>
        ///     Converts the given object of type <see cref="T:ICollection{string}" /> to an object of type
        ///     <see cref="GameTypes" />.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public GameTypes Convert(ICollection<string> value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var result = default(GameTypes);
            foreach (var s in value)
            {
                result |= this.gameTypeConverter.Convert(s, state);
            }

            return result;
        }
    }
}