// <copyright file="MiniatureConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GW2NET.Items.Converter
{
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Miniatures;

    public partial class MiniatureConverter
    {
        partial void Merge(Miniature entity, ItemDataModel dataModel, object state)
        {
            entity.MiniatureId = dataModel.Details.MiniPetId;
        }
    }
}
