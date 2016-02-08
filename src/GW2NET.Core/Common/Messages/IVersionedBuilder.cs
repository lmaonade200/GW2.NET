namespace GW2NET.Common.Messages
{
    /// <summary>
    /// Provides the interface to set the version of the api to query.
    /// </summary>
    public interface IVersionedBuilder : IBaseBuilder
    {
        /// <summary>
        /// Sets the version of the request <see cref="System.Uri"/>.</summary>
        /// <param name="version">The strongly typed version.</param>
        /// <returns>An <see cref="IMessageBuilder"/> used to set the endpoint of the request.</returns>
        IMessageBuilder Version(ApiVersion version);
    }
}