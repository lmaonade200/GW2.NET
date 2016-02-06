namespace GW2NET.Common.Messages
{
    public interface IMessageBuilder : IBaseBuilder
    {
        IParameterizedBuilder OnEndpoint(string endpoint);
    }
}