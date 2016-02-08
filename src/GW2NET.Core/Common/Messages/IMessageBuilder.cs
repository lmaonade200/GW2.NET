namespace GW2NET.Common.Messages
{
    /// <summary>
    /// Provides the interface for a <see cref="System.Net.Http.HttpRequestMessage"/> on an endpoint
    /// </summary>
    public interface IMessageBuilder : IBaseBuilder
    {
        /// <summary>Sets the endpoint the <see cref="System.Net.Http.HttpRequestMessage"/> targets.</summary>
        /// <param name="endpoint">The name of the endpoint.</param>
        /// <returns>An <see cref="IParameterizedBuilder"/> used to further refine the message.</returns>
        IParameterizedBuilder OnEndpoint(string endpoint);
    }
}