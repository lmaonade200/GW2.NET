namespace GW2NET.Common.Messages
{
    using System.Collections.Generic;
    using System.Globalization;

    public interface IParameterizedBuilder : IBaseBuilder
    {
        IParameterizedBuilder WithParameter(string key, string value);

        IParameterizedBuilder ForCulture(CultureInfo culture);

        IBaseBuilder WithIdentifier(int identifier);

        IBaseBuilder WithIdentifiers(IEnumerable<int> identifiers);

        IBaseBuilder WithQuantity(int quantity);

        IPagedBuilder OnPage(int pageIndex);
    }
}