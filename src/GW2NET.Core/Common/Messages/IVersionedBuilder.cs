namespace GW2NET.Common.Messages
{
    public interface IVersionedBuilder : IBaseBuilder
    {
        IMessageBuilder Version(ApiVersion version);
    }
}