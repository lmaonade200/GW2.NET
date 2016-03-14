namespace GW2NET
{
    using System;
    using System.Globalization;

    using GW2NET.Common;

    public class MockCacheItem : ITimeSensitive, ILocalizable
    {
        public int Id { get; set; }

        public string Value { get; set; }

        /// <summary>Gets or sets a timestamp.</summary>
        public DateTimeOffset Timestamp { get; set; }

        public DateTimeOffset Expires { get; set; }

        /// <summary>Gets or sets the locale.</summary>
        public CultureInfo Culture { get; set; }
    }
}