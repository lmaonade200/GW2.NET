// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BagConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="DetailsDTO" /> to objects of type <see cref="Bag" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Bags;

    public partial class BagConverter
    {
        partial void Merge(Bag entity, ItemDataModel dataModel, object state)
        {
            var details = dataModel.Details;
            if (details == null)
            {
                return;
            }

            if (details.Size.HasValue)
            {
                entity.Size = details.Size.Value;
            }

            if (details.NoSellOrSort.HasValue)
            {
                entity.NoSellOrSort = details.NoSellOrSort.Value;
            }
        }
    }
}