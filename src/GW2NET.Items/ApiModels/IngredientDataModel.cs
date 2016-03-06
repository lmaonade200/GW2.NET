// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IngredientDataModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Defines the IngredientDTO type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#pragma warning disable 1591
namespace GW2NET.Items.ApiModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/recipes")]
    public sealed class IngredientDataModel
    {
        [DataMember(Order = 1, Name = "count")]
        public int Count { get; set; }

        [DataMember(Order = 0, Name = "item_id")]
        public int ItemId { get; set; }
    }
}