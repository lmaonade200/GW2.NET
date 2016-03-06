// <copyright file="ApiMetadata.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Converters
{
    using System;
    using System.Globalization;
    using System.Net.Http;

    /// <summary>Stores various medatata provided by an <see cref="HttpResponseMessage"/>.</summary>
    public class ApiMetadata
    {
        /// <summary>Gets or sets the language of the retrived content.</summary>
        public CultureInfo ContentLanguage { get; set; }

        /// <summary>Gets or sets the date the request originated.</summary>
        public DateTimeOffset RequestDate { get; set; }

        /// <summary>Gets or sets the date an <see cref="ITimeSensitive"/> object expires.</summary>
        public DateTimeOffset ExpireDate { get; set; }

        /// <summary>Gets or sets the maximum number of objects the endpoint can return.</summary>
        public int ResultTotal { get; set; }

        /// <summary>Gets or sets the current number of objects the request returned.</summary>
        public int ResultCount { get; set; }
    }
}