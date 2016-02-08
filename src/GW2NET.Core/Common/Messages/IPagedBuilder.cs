namespace GW2NET.Common.Messages
{
    /// <summary>
    /// Provides the interface to set the page size of an <see cref="System.Net.Http.HttpRequestMessage"/>.</summary>
    public interface IPagedBuilder : IBaseBuilder
    {
        /// <summary> Sets the page size for a request.</summary>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A <see cref="IBaseBuilder"/> used to build the request.</returns>
        IBaseBuilder WithSize(int pageSize);
    }
}