// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeDataModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Defines the AttributeDTO type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#pragma warning disable 1591
namespace GW2NET.Items.ApiModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed class AttributeDataModel
    {
        [DataMember(Name = "attribute", Order = 0)]
        public string Attribute { get; set; }

        [DataMember(Name = "modifier", Order = 1)]
        public int Modifier { get; set; }
    }
}