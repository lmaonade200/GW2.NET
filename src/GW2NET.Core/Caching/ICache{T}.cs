namespace GW2NET.V2.Colors.newStuff
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public interface ICache<T>
    {
        IEnumerable<T> Value { get; }

        void Add(T item);

        void AddRange(IEnumerable<T> items);

        void Clear();

        IEnumerable<T> Get(Func<T, bool> selector, CultureInfo culture);
    }
}