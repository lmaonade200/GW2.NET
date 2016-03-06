// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CombatBuffConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="BuffDTO" /> to objects of type <see cref="CombatBuff" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;

    using GW2NET.Common;
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Common;

    /// <summary>Converts objects of type <see cref="BuffDataModel" /> to objects of type <see cref="CombatBuff" />.</summary>
    public sealed class CombatBuffConverter : IConverter<BuffDataModel, CombatBuff>
    {
        /// <summary>Converts the given object of type <see cref="BuffDataModel" /> to an object of type <see cref="CombatBuff" />.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public CombatBuff Convert(BuffDataModel value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new CombatBuff { SkillId = value.SkillId, Description = value.Description };
        }
    }
}