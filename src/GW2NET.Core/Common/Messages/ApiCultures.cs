// <copyright file="ApiCultures.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    using System.Globalization;

    public static class ApiCultures
    {
        public static CultureInfo English
        {
            get
            {
                return new CultureInfo("en");
            }
        }

        public static CultureInfo Spanish
        {
            get
            {
                return new CultureInfo("es");
            }
        }

        public static CultureInfo French
        {
            get
            {
                return new CultureInfo("fr");
            }
        }

        public static CultureInfo German
        {
            get
            {
                return new CultureInfo("de");
            }
        }

        public static CultureInfo Korean
        {
            get
            {
                return new CultureInfo("ko");
            }
        }

        public static CultureInfo Chinese
        {
            get
            {
                return new CultureInfo("zh");
            }
        }
    }
}
