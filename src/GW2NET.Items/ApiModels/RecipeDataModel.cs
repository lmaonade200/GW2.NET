// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecipeDataModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Defines the RecipeDTO type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#pragma warning disable 1591
namespace GW2NET.Items.ApiModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/recipes")]
    public sealed class RecipeDataModel
    {
        [DataMember(Order = 5, Name = "disciplines")]
        public ICollection<string> Disciplines { get; set; }

        [DataMember(Order = 6, Name = "flags")]
        public ICollection<string> Flags { get; set; }

        [DataMember(Order = 8, Name = "id")]
        public int Id { get; set; }

        [DataMember(Order = 7, Name = "ingredients")]
        public ICollection<IngredientDataModel> Ingredients { get; set; }

        [DataMember(Order = 3, Name = "min_rating")]
        public int MinRating { get; set; }

        [DataMember(Order = 2, Name = "output_item_count")]
        public int OutputItemCount { get; set; }

        [DataMember(Order = 1, Name = "output_item_id")]
        public int OutputItemId { get; set; }

        [DataMember(Order = 4, Name = "time_to_craft_ms")]
        public double TimeToCraftMs { get; set; }

        [DataMember(Order = 0, Name = "type")]
        public string Type { get; set; }

        [DataMember(Order = 9, Name = "chat_link")]
        public string ChatLink { get; set; }
    }
}