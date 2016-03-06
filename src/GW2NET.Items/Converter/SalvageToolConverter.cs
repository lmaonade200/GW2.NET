// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalvageToolConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="DetailsDTO" /> to objects of type <see cref="Tool" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Tools;

    public partial class SalvageToolConverter
    {
        partial void Merge(SalvageTool entity, ItemDataModel dataModel, object state)
        {
            var details = dataModel.Details;
            if (details == null)
            {
                return;
            }

            if (details.Charges.HasValue)
            {
                entity.Charges = details.Charges.Value;
            }
        }
    }
}