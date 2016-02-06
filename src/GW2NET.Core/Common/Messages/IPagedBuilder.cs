namespace GW2NET.Common.Messages
{
    public interface IPagedBuilder : IBaseBuilder
    {
        IBaseBuilder WithSize(int pageSize);
    }
}