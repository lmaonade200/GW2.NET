﻿// <copyright file="CommerceFactory.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Factories.Services
{
    using System;
    using System.Net.Http;

    using DryIoc;

    using GW2NET.Caching;
    using GW2NET.Commerce;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.TradingPost;
    using GW2NET.TradingPost.ApiModels;
    using GW2NET.TradingPost.Converter;

    /// <summary>Provides access to commerce data sources based on the /v2/ api.</summary>
    public class CommerceFactory
    {
        private readonly Container iocContainer;

        /// <summary>Initializes a new instance of the <see cref="CommerceFactory"/> class.</summary>
        /// <param name="iocContainer"></param>
        /// <exception cref="ArgumentNullException">The value of <paramref name="iocContainer"/> is a null reference.</exception>
        public CommerceFactory(Container iocContainer)
        {
            if (iocContainer == null)
            {
                throw new ArgumentNullException(nameof(iocContainer));
            }

            this.iocContainer = iocContainer;

            this.InitCurrencyExchange();
            this.InitListings();
            this.InitAggregateListing();
        }

        /// <summary>Gets access to the gem exchange data source.</summary>
        public ICurrencyExchange Exchange
        {
            get
            {
                return this.iocContainer.Resolve<ICurrencyExchange>();
            }
        }

        /// <summary>Gets access to the listings data source.</summary>
        public ListingRepository Listings
        {
            get
            {
                return this.iocContainer.Resolve<ListingRepository>();
            }
        }

        /// <summary>Gets access to the aggregate listings data source.</summary>
        public AggregateListingRepository Prices
        {
            get
            {
                return this.iocContainer.Resolve<AggregateListingRepository>();
            }
        }

        private void InitAggregateListing()
        {
            this.iocContainer.Register(
                Made.Of(() => new AggregateListingRepository(
                    Arg.Of<HttpClient>("RepositoryClient"),
                    Arg.Of<IResponseConverter>(),
                    Arg.Of<ICache<int, AggregateListing>>(),
                    Arg.Of<IConverter<int, int>>(),
                    Arg.Of<IConverter<AggregateListingDataModel, AggregateListing>>())));

            this.iocContainer.Register<IConverter<AggregateListingDataModel, AggregateListing>, AggregateListingConverter>();
            this.iocContainer.Register<IConverter<AggregateOfferDataModel, AggregateOffer>, AggregateOfferConverter>();
        }

        private void InitListings()
        {
            this.iocContainer.Register(
                Made.Of(() => new ListingRepository(
                    Arg.Of<HttpClient>("RepositoryClient"),
                    Arg.Of<IResponseConverter>(),
                    Arg.Of<ICache<int, Listing>>(),
                    Arg.Of<IConverter<int, int>>(),
                    Arg.Of<IConverter<ListingDataModel, Listing>>())));

            this.iocContainer.Register<IConverter<ListingDataModel, Listing>, ListingConverter>();
            this.iocContainer.Register<IConverter<ListingOfferDataModel, Offer>, OfferConverter>();
            this.iocContainer.Register(Made.Of(() => new CollectionConverter<ListingOfferDataModel, Offer>(Arg.Of<IConverter<ListingOfferDataModel, Offer>>())));
        }

        private void InitCurrencyExchange()
        {
            this.iocContainer.Register<IConverter<ExchangeDataModel, Exchange>, ExchangeConverter>();
        }
    }
}