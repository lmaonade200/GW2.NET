// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DyeUnlockerConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="DetailsDTO" /> to objects of type <see cref="DyeUnlocker" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Consumables;

    public partial class DyeUnlockerConverter
    {
        partial void Merge(DyeUnlocker entity, ItemDataModel dataModel, object state)
        {
            var details = dataModel.Details;
            if (details == null)
            {
                return;
            }

            if (details.ColorId.HasValue)
            {
                entity.ColorId = details.ColorId.Value;
            }
        }
    }
}