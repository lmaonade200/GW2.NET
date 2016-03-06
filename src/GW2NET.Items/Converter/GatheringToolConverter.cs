// <copyright file="GatheringToolConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GW2NET.Items.Converter
{
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.GatheringTools;

    /// <summary>Convertes a <see cref="ItemDataModel"/> into a <see cref="GatheringTool"/> object.</summary>
    public partial class GatheringToolConverter
    {
        partial void Merge(GatheringTool entity, ItemDataModel dataModel, object state)
        {
            int defaultSkinId;
            if (int.TryParse(dataModel.DefaultSkin, out defaultSkinId))
            {
                entity.DefaultSkinId = defaultSkinId;
            }
        }
    }
}