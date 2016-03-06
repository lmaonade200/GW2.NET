// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GW2Bootstrapper.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Provides access to Guild Wars 2 data sources and services.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net.Http;

    using DryIoc;

    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.Common.Serializers;
    using GW2NET.Compression;
    using GW2NET.Factories;
    using GW2NET.Factories.Services;
    using GW2NET.Rendering;

    /// <summary>Provides access to Guild Wars 2 data sources and services.</summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Naming is intended.")]
    public class GW2Bootstrapper
    {
        private readonly Container container;

        /// <summary>Initializes a new instance of the <see cref="GW2Bootstrapper"/> class.</summary>
        public GW2Bootstrapper()
            : this(string.Empty)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="GW2Bootstrapper"/> class.</summary>
        /// <param name="apiKey">The api key.</param>
        public GW2Bootstrapper(string apiKey)
        {
            // Create the container
            this.container = new Container();

            // Register Stuff
            this.RegisterHttpObjects();
            this.RegisterSerializerFactories();
            this.RegisterCommon();
            this.RegisterAuthorizedHttpClient(apiKey);

            // Register Repositories
            this.RegisterRendering();

            // Pupulate the repository properties
            this.Services = new Services(this.container);
            this.AuthorizedServices = new AuthorizedServices(this.container);
            this.Local = new FactoryForLocal();
        }

        private void RegisterRendering()
        {
            this.container.Register<IRenderRepository, RenderRepository>(
                Made.Of(() => new RenderRepository(Arg.Of<HttpClient>("RenderingClient"))));
        }

        /// <summary>Gets access to specialty services that do not require a network connection.</summary>
        public FactoryForLocal Local { get; private set; }

        /// <summary>Gets access to the rendering service.</summary>
        public IRenderRepository Rendering
        {
            get
            {
                return this.container.Resolve<IRenderRepository>();
            }
        }

        /// <summary>Gets access to the public area of the version 2 of the Guild Wars 2 API.</summary>
        public Services Services { get; private set; }

        /// <summary> Gets access to the authorized area of the Guild Wars 2 API.</summary>
        public AuthorizedServices AuthorizedServices { get; private set; }

        /// <summary>Sets the api key for further use.</summary>
        /// <param name="apiKey">The api key.</param>
        public void SetApiKey(string apiKey)
        {
            if (KeyUtilities.IsValid(apiKey))
            {
                this.container.Unregister<AuthenticatedMessageHandler>();
                this.container.Unregister<string>("ApiKey");

                this.RegisterAuthorizedHttpClient(apiKey);
            }
            else
            {
                throw new ArgumentException("The api key didn't have the required format.", nameof(apiKey));
            }
        }

        private void RegisterSerializerFactories()
        {
            this.container.Register<ISerializerFactory, JsonSerializerFactory>(serviceKey: "JsonSerializerFactory");
            this.container.Register<ISerializerFactory, BinarySerializerFactory>(serviceKey: "BinarySerializerFactory");
        }

        private void RegisterHttpObjects()
        {
            // Register some helper instances
            this.container.RegisterInstance(true, Reuse.Transient, serviceKey: "DisposeHandler");
            this.container.RegisterInstance(new Uri("https://api.guildwars2.com/", UriKind.Absolute), Reuse.Transient, serviceKey: "RepositoryUri");
            this.container.RegisterInstance(new Uri("https://render.guildwars2.com", UriKind.Absolute), Reuse.Transient, serviceKey: "RenderingUri");

            // Register the converter for an HttpResponse
            this.container.Register<IResponseConverter, HttpResponseConverter>(
                Made.Of(() => new HttpResponseConverter(
                    Arg.Of<ISerializerFactory>("JsonSerializerFactory"),
                    Arg.Of<ISerializerFactory>("JsonSerializerFactory"),
                    Arg.Of<IConverter<Stream, Stream>>("GzipInflator"))));

            // Register MessageHandlers
            this.container.Register<HttpMessageHandler, HttpClientHandler>(
                Reuse.Singleton,
                serviceKey: "BaseMessageHandler");

            // Register the HttpClients
            this.container.Register(
                Made.Of(() => new HttpClient(Arg.Of<HttpMessageHandler>("BaseMessageHandler"), Arg.Of<bool>("DisposeHandler"))
                {
                    BaseAddress = Arg.Of<Uri>("RepositoryUri")
                }),
                Reuse.Singleton,
                serviceKey: "RepositoryClient");

            this.container.Register(
                Made.Of(() => new HttpClient(Arg.Of<HttpMessageHandler>("BaseMessageHandler"), Arg.Of<bool>("DisposeHandler"))
                {
                    BaseAddress = Arg.Of<Uri>("RenderingUri")
                }),
                Reuse.Singleton,
                serviceKey: "RenderingClient");
        }

        private void RegisterAuthorizedHttpClient(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return;
            }

            if (KeyUtilities.IsValid(apiKey))
            {
                this.container.RegisterInstance(apiKey, Reuse.Transient, serviceKey: "ApiKey");
                this.container.Register<HttpMessageHandler, AuthenticatedMessageHandler>(
                    Made.Of(() => new AuthenticatedMessageHandler(Arg.Of<HttpMessageHandler>("AuthenticatedMessageHandler"), Arg.Of<string>("ApiKey"))),
                    Reuse.Singleton);

                this.container.Register(
                    Made.Of(() => new HttpClient(Arg.Of<HttpMessageHandler>("AuthenticatedMessageHandler"), Arg.Of<bool>("DisposeHandler"))
                    {
                        BaseAddress = Arg.Of<Uri>("RepositoryUri")
                    }),
                    Reuse.Transient,
                    serviceKey: "AuthenticatedRepositoryClient");
            }
            else
            {
                throw new ArgumentException("The api key didn't have the required format.", nameof(apiKey));
            }
        }

        private void RegisterCommon()
        {
            this.container.Register<IConverter<int, int>, ConverterAdapter<int>>();
            this.container.Register<IConverter<string, string>, ConverterAdapter<string>>();
            this.container.Register<IConverter<Stream, Stream>, GzipInflator>(serviceKey: "GzipInflator");
            this.container.Register(typeof(ICache<,>), typeof(MemoryCache<,>));
        }
    }
}