namespace GW2NET.Common.Messages
{
    using System.Net.Http;

    /// <summary>
    /// Provides the base builder for a <see cref="HttpRequestMessage"/>.</summary>
    public interface IBaseBuilder
    {
        /// <summary>Builds the <see cref="HttpRequestMessage"/>.</summary>
        /// <returns></returns>
        HttpRequestMessage Build();
    }
}