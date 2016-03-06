// <copyright file="UInt24.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

#pragma warning disable CS3003
#pragma warning disable CS3001
namespace GW2NET.ChatLinks.Interop
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>Represents a 24-bit unsigned integer.</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct UInt24
    {
        private readonly byte b0;

        private readonly byte b1;

        private readonly byte b2;

        /// <summary>Initializes a new instance of the <see cref="UInt24"/> struct.</summary>
        /// <param name="value">The value of this unsigned 24-bit integer.</param>
        public UInt24(uint value)
        {
            var b = BitConverter.GetBytes(value);
            this.b0 = b[0];
            this.b1 = b[1];
            this.b2 = b[2];
        }

        /// <summary>Gets the value of this 24-bit unsigned integer.</summary>
        public uint Value
        {
            get
            {
                return (uint)(this.b0 | (this.b1 << 8) | (this.b2 << 16));
            }
        }
    }
}