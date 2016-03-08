// <copyright file="EnumExtensions.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements.Converter
{
    using System;

    public static class StringExtensions
    {
        public static TReturn TryParse<TReturn>(this string stringValue)
            where TReturn : struct
        {
            TReturn returnEnum;
            return Enum.TryParse(stringValue, true, out returnEnum) ? returnEnum : default(TReturn);
        }
    }
}