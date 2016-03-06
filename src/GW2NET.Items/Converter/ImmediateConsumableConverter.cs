// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImmediateConsumableConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="DetailsDTO" /> to objects of type <see cref="ImmediateConsumable" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;

    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Consumables;

    public partial class ImmediateConsumableConverter
    {
        partial void Merge(ImmediateConsumable entity, ItemDataModel dataModel, object state)
        {
            var details = dataModel.Details;
            if (details == null)
            {
                return;
            }

            var duration = details.Duration;
            if (duration.HasValue)
            {
                entity.Duration = TimeSpan.FromMilliseconds(duration.Value);
            }

            entity.Effect = details.Description;
        }
    }
}