// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CombatAttributeConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="AttributeDTO" /> to objects of type <see cref="CombatAttribute" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Common;

    /// <summary>Convertes a <see cref="AttributeDataModel"/> into a <see cref="CombatAttribute"/> object.</summary>
    public partial class CombatAttributeConverter
    {
        partial void Merge(CombatAttribute entity, AttributeDataModel dataModel, object state)
        {
            entity.Modifier = dataModel.Modifier;
        }
    }
}