// <copyright file="ItemModifiers.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.ChatLinks.Interop
{
    using System;

    /// <summary>Enumerates item modifiers.</summary>
    [Flags]
    public enum ItemModifiers : byte
    {
        /// <summary>An item with no modifier.</summary>
        None = 0,

        /// <summary>A suffix object.</summary>
        SuffixItem = 0x40,

        /// <summary>An secondary suffix object.</summary>
        SecondarySuffixItem = 0x60,

        /// <summary>A skin object.</summary>
        Skin = 0x80
    }
}