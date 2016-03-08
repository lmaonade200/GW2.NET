// <copyright file="StringToGuidConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Converters
{
    using System;

    public class StringToGuidConverter :IConverter<string, Guid>
    {
        /// <inheritdoc />
        public Guid Convert(string value, object state = null)
        {
            return new Guid(value);
        }
    }
}
