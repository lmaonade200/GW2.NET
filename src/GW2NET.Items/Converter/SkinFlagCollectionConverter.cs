// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkinFlagCollectionConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="T:ICollection{string}" /> to objects of type <see cref="SkinFlags" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Skins;

    /// <summary>Converts objects of type <see cref="T:ICollection{string}"/> to objects of type <see cref="SkinFlags"/>.</summary>
    public sealed class SkinFlagCollectionConverter : IConverter<ICollection<string>, SkinFlags>
    {
        private readonly IConverter<string, SkinFlags> skinFlagsConverter;

        /// <summary>Initializes a new instance of the <see cref="SkinFlagCollectionConverter"/> class.</summary>
        /// <param name="skinFlagsConverter">The converter for <see cref="SkinFlags"/>.</param>
        /// <exception cref="ArgumentNullException">The value of <paramref name="skinFlagsConverter"/> is a null reference.</exception>
        public SkinFlagCollectionConverter(IConverter<string, SkinFlags> skinFlagsConverter)
        {
            if (skinFlagsConverter == null)
            {
                throw new ArgumentNullException(nameof(skinFlagsConverter));
            }

            this.skinFlagsConverter = skinFlagsConverter;
        }

        /// <inheritdoc />
        public SkinFlags Convert(ICollection<string> value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            SkinFlags result = default(SkinFlags);
            foreach (var s in value)
            {
                result = result | this.skinFlagsConverter.Convert(s, state);
            }

            return result;
        }
    }
}