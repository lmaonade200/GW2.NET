// <copyright file="ApiMessageBuilder.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;

    /// <summary>Provides a fluent interface to build a <see cref="HttpRequestMessage"/> for the Guild Wars 2 API.</summary>
    public class ApiMessageBuilder : IMessageBuilder, IVersionedBuilder, IParameterizedBuilder, IPagedBuilder
    {
        private readonly Dictionary<string, string> queryParameters;

        private readonly List<string> identifiers;

        private ApiVersion apiVersion;

        private string endpoint;

        /// <summary>Initializes a new instance of the <see cref="ApiMessageBuilder"/> class.</summary>
        private ApiMessageBuilder()
        {
            this.queryParameters = new Dictionary<string, string>();
            this.identifiers = new List<string>();
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
            Uri uri = new Uri($"{this.apiVersion}/{this.endpoint}{this.BuildParameterString()}", UriKind.Relative);

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, uri);

            string language = this.queryParameters.SingleOrDefault(k => k.Key == "lang").Value;
            message.Headers.Add("Accept-Language", string.Equals(language, "iv") ? "en" : language);

            return message;
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
                // If there was no culture passed we should
                // assume that nothing should happen.
                if (culture == null)
                {
                    return this;
                }

                this.queryParameters.Add("lang", culture.TwoLetterISOLanguageName == "iv" ? "en" : culture.TwoLetterISOLanguageName);
            }

            return this;
        }

        /// <inheritdoc />
        IBaseBuilder IParameterizedBuilder.WithIdentifier<TKey>(TKey identifier)
        {
            this.identifiers.Add(identifier.ToString());

            return this;
        }

        /// <inheritdoc />
        // ReSharper disable once ParameterHidesMember
        IBaseBuilder IParameterizedBuilder.WithIdentifiers<TKey>(IEnumerable<TKey> identifiers)
        {
            this.identifiers.AddRange(identifiers.Select(i => i.ToString()));

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

        /// <summary>Builds the parameter url string.</summary>
        /// <returns>The url string.</returns>
        private string BuildParameterString()
        {
            if (this.queryParameters.ContainsKey("page"))
            {
                return $"?page={this.queryParameters["page"]}&page_size={this.queryParameters["page_size"]}";
            }

            switch (this.identifiers.Count)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return $"/{this.identifiers.First()}";
            }

            return $"?ids={string.Join(",", this.identifiers)}";
        }
    }
}