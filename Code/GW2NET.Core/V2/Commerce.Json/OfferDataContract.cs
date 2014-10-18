﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfferDataContract.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Defines the OfferDataContract type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.V2.Commerce.Json
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not a public API.")]
    internal sealed class OfferDataContract
    {
        [DataMember(Name = "listings", Order = 0)]
        internal int Listings { get; set; }

        [DataMember(Name = "quantity", Order = 2)]
        internal int Quantity { get; set; }

        [DataMember(Name = "unit_price", Order = 1)]
        internal int UnitPrice { get; set; }
    }
}