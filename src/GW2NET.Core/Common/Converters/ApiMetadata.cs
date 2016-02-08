namespace GW2NET.Common.Converters
{
    using System;
    using System.Globalization;

    public class ApiMetadata
    {
        public CultureInfo ContentLanguage { get; set; }

        public DateTimeOffset RequestDate { get; set; }

        public DateTimeOffset ExpireDate { get; set; }

        public int ResultTotal { get; set; }

        public int ResultCount { get; set; }
    }
}