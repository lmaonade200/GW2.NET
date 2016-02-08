// <copyright file="GW2ApiRequestBuilder.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Text;

    /// <summary>Provides a fluent interface to build a <see cref="HttpRequestMessage"/> for the Guild Wars 2 API.</summary>
    public class ApiMessageBuilder : IMessageBuilder, IVersionedBuilder, IParameterizedBuilder, IPagedBuilder
    {
        private readonly Dictionary<string, string> queryParameters;

        private readonly HashSet<int> identifiers;

        private ApiVersion apiVersion;

        private string endpoint;

        /// <summary>Prevents a new instance of the <see cref="ApiMessageBuilder"/> to be created.</summary>
        private ApiMessageBuilder()
        {
            this.queryParameters = new Dictionary<string, string>();
            this.identifiers = new HashSet<int>();
        }

        /// <summary>Creates a new instance of the <see cref="ApiMessageBuilder"/>.</summary>
        /// <returns>The created instance as an <see cref="IVersionedBuilder"/>.</returns>
        public static IVersionedBuilder Init()
        {
            return new ApiMessageBuilder();
        }

        /// <inheritdoc />
        HttpRequestMessage IBaseBuilder.Build()
        {
            Uri uri = new Uri($"{this.apiVersion}/{this.endpoint}{this.BuildQueryString()}", UriKind.Relative);

            return new HttpRequestMessage(HttpMethod.Get, uri);
        }

        /// <inheritdoc />
        IBaseBuilder IPagedBuilder.WithSize(int pageSize)
        {
            if (this.queryParameters.ContainsKey("page_size"))
            {
                this.queryParameters["page_size"] = pageSize.ToString();
            }
            else
            {
                this.queryParameters.Add("page_size", pageSize.ToString());
            }

            return this;
        }

        /// <inheritdoc />
        IParameterizedBuilder IParameterizedBuilder.WithParameter(string key, string value)
        {
            this.queryParameters.Add(key, value);
            return this;
        }

        /// <inheritdoc />
        IParameterizedBuilder IParameterizedBuilder.ForCulture(CultureInfo culture)
        {
            if (this.queryParameters.ContainsKey("lang"))
            {
                this.queryParameters["lang"] = culture.TwoLetterISOLanguageName;
            }
            else
            {
                this.queryParameters.Add("lang", culture.TwoLetterISOLanguageName);
            }

            return this;
        }

        /// <inheritdoc />
        IBaseBuilder IParameterizedBuilder.WithIdentifier(int identifier)
        {
            this.identifiers.Add(identifier);

            return this;
        }

        /// <inheritdoc />
        // ReSharper disable once ParameterHidesMember
        IBaseBuilder IParameterizedBuilder.WithIdentifiers(IEnumerable<int> identifiers)
        {
            this.identifiers.UnionWith(identifiers);

            return this;
        }

        /// <inheritdoc />
        IBaseBuilder IParameterizedBuilder.WithQuantity(int quantity)
        {
            if (this.queryParameters.ContainsKey("quantity"))
            {
                this.queryParameters["quantity"] = quantity.ToString();
            }
            else
            {
                this.queryParameters.Add("quantity", quantity.ToString());
            }

            return this;
        }

        /// <inheritdoc />
        IPagedBuilder IParameterizedBuilder.OnPage(int pageIndex)
        {
            if (this.queryParameters.ContainsKey("page"))
            {
                this.queryParameters["page"] = pageIndex.ToString();
            }
            else
            {
                this.queryParameters.Add("page", pageIndex.ToString());
            }

            return this;
        }

        /// <inheritdoc />
        IMessageBuilder IVersionedBuilder.Version(ApiVersion version)
        {
            this.apiVersion = version;
            return this;
        }

        /// <inheritdoc />
        // ReSharper disable once ParameterHidesMember
        IParameterizedBuilder IMessageBuilder.OnEndpoint(string endpoint)
        {
            this.endpoint = endpoint;
            return this;
        }

        /// <summary>Builds the query string from the stored data.</summary>
        /// <returns>A <see cref="string"/> representing the parameters of the query.</returns>
        private string BuildQueryString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (this.identifiers.Count == 1)
            {
                stringBuilder.Append($"/{this.identifiers.First()}");
            }

            stringBuilder.Append("?");

            if (this.identifiers.Count > 1)
            {
                stringBuilder.Append($"ids={string.Join(",", this.identifiers)}&");
            }

            foreach (KeyValuePair<string, string> valuePair in this.queryParameters.Where(pair => pair.Key != "ids"))
            {
                if (valuePair.Key == "lang")
                {
                    if (valuePair.Value == "iv")
                    {
                        continue;
                    }

                    stringBuilder.Append($"{valuePair.Key}={valuePair.Value}&");
                }

                stringBuilder.Append($"{valuePair.Key}={valuePair.Value}&");
            }

            if (stringBuilder.ToString().EndsWith("&"))
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }

            return stringBuilder.Length == 1 ? string.Empty : stringBuilder.ToString();
        }
    }
}