﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnknownContainerDetails.cs" company="GW2.Net Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Represents detailed information about an unknown container.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2DotNET.V1.ItemsDetails.Types.ItemTypes.Containers.ContainerTypes
{
    using GW2DotNET.V1.Common.Converters;

    using Newtonsoft.Json;

    /// <summary>Represents detailed information about an unknown container.</summary>
    [JsonConverter(typeof(DefaultJsonConverter))]
    public class UnknownContainerDetails : ContainerDetails
    {
        /// <summary>Initializes a new instance of the <see cref="UnknownContainerDetails" /> class.</summary>
        public UnknownContainerDetails()
            : base(ContainerType.Unknown)
        {
        }
    }
}