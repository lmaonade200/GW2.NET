// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfixUpgradeDataModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Defines the InfixUpgradeDTO type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#pragma warning disable 1591
namespace GW2NET.Items.ApiModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed class InfixUpgradeDataModel
    {
        [DataMember(Name = "attributes", Order = 1)]
        public ICollection<AttributeDataModel> Attributes { get; set; }

        [DataMember(Name = "buff", Order = 0)]
        public BuffDataModel Buff { get; set; }
    }
}