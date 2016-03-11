namespace GW2NET
{
    using System;

    using GW2NET.Common;

    public class MockCacheItem : ITimeSensitive
    {
        public int Id { get; set; }

        public string Value { get; set; }

        /// <summary>Gets or sets a timestamp.</summary>
        public DateTimeOffset Timestamp { get; set; }

        public DateTimeOffset Expires { get; set; }
    }
}