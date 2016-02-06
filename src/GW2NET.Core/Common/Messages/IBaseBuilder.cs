namespace GW2NET.Common.Messages
{
    using System.Net.Http;

    public interface IBaseBuilder
    {
        HttpRequestMessage Build();
    }
}