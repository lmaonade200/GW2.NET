namespace GW2NET.Common
{
    using System.Collections.Generic;

    /// <summary>Defines a slice of a bigger collection.</summary>
    /// <typeparam name="T">The type of the collection</typeparam>
    public interface ISlice<T> : ICollection<T>
    {
        /// <summary>Gets or sets the total number of items a collection could have.</summary>
        /// <remarks>
        /// This property makes the total number of items public.
        /// Do not confuse it with the capacity of a collection,
        /// which can still be different.
        /// </remarks>
        int TotalCount { get; set; }
    }
}