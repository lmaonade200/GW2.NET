// <copyright file="Services.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Factories.Services
{
    using System.Collections.Generic;
    using System.Net.Http;

    using DryIoc;

    using GW2NET.Builds;
    using GW2NET.Caching;
    using GW2NET.Colors;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Drawing;
    using GW2NET.Factories.V2;
    using GW2NET.Files;
    using GW2NET.Guilds;
    using GW2NET.Guilds.ApiModels;
    using GW2NET.Guilds.Converter;
    using GW2NET.Items;
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Converter;
    using GW2NET.MapInformation;
    using GW2NET.MapInformation.ApiModels;
    using GW2NET.MapInformation.Converter;
    using GW2NET.Maps;
    using GW2NET.Miscellaneous;
    using GW2NET.Miscellaneous.ApiModels;
    using GW2NET.Miscellaneous.Converter;
    using GW2NET.Quaggans;
    using GW2NET.Recipes;
    using GW2NET.Skins;
    using GW2NET.Worlds;

    using IocContainer = DryIoc.Container;

    /// <summary>Provides access to the public Guild Wars 2 api.</summary>
    public class Services
    {
        private readonly IocContainer iocContainer;

        /// <summary>Initializes a new instance of the <see cref="Services"/> class.</summary>
        /// <param name="iocContainer">The container to resolve repositories against.</param>
        public Services(IocContainer iocContainer)
        {
            this.iocContainer = iocContainer;
        }

        /// <summary>Gets access to the v2 build service.</summary>
        public BuildRepository Builds
        {
            get
            {
                this.iocContainer.Register(
                    Made.Of(() => new BuildRepository(
                        Arg.Of<HttpClient>("RepositoryClient"),
                        Arg.Of<IResponseConverter>(),
                        Arg.Of<IConverter<BuildDataModel, Build>>())));
                this.iocContainer.Register<IConverter<BuildDataModel, Build>, BuildConverter>();

                return this.iocContainer.Resolve<BuildRepository>();
            }
        }

        /// <summary>Gets access to the colors data source.</summary>
        public ColorRepository Colors
        {
            get
            {
                this.iocContainer.Register(
                Made.Of(() => new ColorRepository(
                    Arg.Of<HttpClient>("RepositoryClient"),
                    Arg.Of<IResponseConverter>(),
                    Arg.Of<ICache<int, ColorPalette>>(),
                    Arg.Of<IConverter<int, int>>(),
                    Arg.Of<IConverter<ColorPaletteDataModel, ColorPalette>>())));

                this.iocContainer.Register<IConverter<ColorPaletteDataModel, ColorPalette>, ColorPaletteConverter>();
                this.iocContainer.Register<IConverter<int[], Color>, ColorConverter>();
                this.iocContainer.Register<IConverter<ColorDataModel, ColorModel>, ColorModelConverter>();

                return this.iocContainer.Resolve<ColorRepository>();
            }
        }

        /// <summary>Gets access to commerce data sources.</summary>
        public CommerceFactory Commerce
        {
            get
            {
                return new CommerceFactory(this.iocContainer);
            }
        }

        /// <summary>Gets access to the continents data sources.</summary>
        public ContinentRepository Continents
        {
            get
            {
                this.iocContainer.Register(
                Made.Of(() => new ContinentRepository(
                    Arg.Of<HttpClient>("RepositoryClient"),
                    Arg.Of<IResponseConverter>(),
                    Arg.Of<ICache<int, Continent>>(),
                    Arg.Of<IConverter<int, int>>(),
                    Arg.Of<IConverter<ContinentDataModel, Continent>>())));

                this.iocContainer.Register<IConverter<ContinentDataModel, Continent>>();

                return this.iocContainer.Resolve<ContinentRepository>();
            }
        }

        /// <summary>Gets access to the files data sources.</summary>
        public FileRepository Files
        {
            get
            {
                this.iocContainer.Register(
               Made.Of(() => new FileRepository(
                   Arg.Of<HttpClient>("RepositoryClient"),
                   Arg.Of<IResponseConverter>(),
                   Arg.Of<ICache<string, Asset>>(),
                   Arg.Of<IConverter<string, string>>(),
                   Arg.Of<IConverter<FileDataModel, Asset>>())));

                this.iocContainer.Register<IConverter<FileDataModel, Asset>, AssetConverter>();

                return this.iocContainer.Resolve<FileRepository>();
            }
        }

        /// <summary>Gets access to the guilds data source.</summary>
        public IGuildRepository Guilds
        {
            get
            {
                this.iocContainer.Register<IGuildRepository, GuildRepository>(
                    Made.Of(() => new GuildRepository(
                        Arg.Of<HttpClient>("RepositoryClient"),
                        Arg.Of<IResponseConverter>(),
                        Arg.Of<IConverter<GuildDataModel, Guild>>())));

                this.iocContainer.Register<IConverter<GuildDataModel, Guild>>();
                this.iocContainer.Register<IConverter<EmblemDataModel, Emblem>>();
                this.iocContainer.Register<IConverter<string, EmblemTransformations>, EmblemTransformationConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, EmblemTransformations>, EmblemTransformationCollectionConverter>();

                return this.iocContainer.Resolve<GuildRepository>();
            }
        }

        /// <summary>Gets access to the items data source.</summary>
        public ItemRepository Items
        {
            get
            {
                this.iocContainer.Register(
                    Made.Of(() => new ItemRepository(
                        Arg.Of<HttpClient>("RepositoryClient"),
                        Arg.Of<IResponseConverter>(),
                        Arg.Of<ICache<int, Item>>(),
                        Arg.Of<IConverter<int, int>>(),
                        Arg.Of<IConverter<ItemDataModel, Item>>())));

                this.iocContainer.Register<IConverter<ItemDataModel, Item>, ItemConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, ItemRestrictions>, ItemRestrictionCollectionConverter>();
                this.iocContainer.Register<IConverter<string, ItemRestrictions>, ItemRestrictionConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, ItemFlags>, ItemFlagCollectionConverter>();
                this.iocContainer.Register<IConverter<string, ItemFlags>, ItemFlagConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, GameTypes>, GameTypeCollectionConverter>();
                this.iocContainer.Register<IConverter<string, GameTypes>, GameTypeConverter>();
                this.iocContainer.Register<IConverter<string, ItemRarity>, ItemRarityConverter>();
                this.iocContainer.Register<ITypeConverterFactory<ItemDataModel, Item>, ItemConverterFactory>();

                return this.iocContainer.Resolve<ItemRepository>();
            }
        }

        /// <summary>Gets access to the maps data source.</summary>
        public MapRepository Maps
        {
            get
            {
                this.iocContainer.Register(
                    Made.Of(() => new MapRepository(
                        Arg.Of<HttpClient>("RepositoryClient"),
                        Arg.Of<IResponseConverter>(),
                        Arg.Of<ICache<int, Map>>(),
                        Arg.Of<IConverter<int, int>>(),
                        Arg.Of<IConverter<MapDataModel, Map>>())));

                this.iocContainer.Register<IConverter<MapDataModel, Map>, MapConverter>();
                this.iocContainer.Register<IConverter<double[][], Rectangle>, RectangleConverter>();

                return this.iocContainer.Resolve<MapRepository>();
            }
        }

        /// <summary>Gets access to the Quaggans data source.</summary>
        public QuagganRepository Quaggans
        {
            get
            {
                this.iocContainer.Register(
                    Made.Of(() => new QuagganRepository(
                        Arg.Of<HttpClient>("RepositoryClient"),
                        Arg.Of<IResponseConverter>(),
                        Arg.Of<ICache<string, Quaggan>>(),
                        Arg.Of<IConverter<string, string>>(),
                        Arg.Of<IConverter<QuagganDataModel, Quaggan>>())));

                this.iocContainer.Register<IConverter<QuagganDataModel, Quaggan>, QuagganConverter>();

                return this.iocContainer.Resolve<QuagganRepository>();
            }
        }

        /// <summary>Gets access to the recipe data source.</summary>
        public RecipeRepository Recipes
        {
            get
            {
                this.iocContainer.Register(
                    Made.Of(() => new RecipeRepository(
                        Arg.Of<HttpClient>("RepositoryClient"),
                        Arg.Of<IResponseConverter>(),
                        Arg.Of<ICache<int, Recipe>>(),
                        Arg.Of<IConverter<int, int>>(),
                        Arg.Of<IConverter<RecipeDataModel, Recipe>>())));
                this.iocContainer.Register<RecipeConverter>();
                this.iocContainer.Register<IConverter<IEnumerable<IngredientDataModel>, IEnumerable<ItemQuantity>>, CollectionConverter<IngredientDataModel, ItemQuantity>>();
                this.iocContainer.Register<IConverter<IngredientDataModel, ItemQuantity>, ItemQuantityConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, RecipeFlags>, RecipeFlagCollectionConverter>();
                this.iocContainer.Register<IConverter<string, RecipeFlags>, RecipeFlagConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, CraftingDisciplines>, CraftingDisciplineCollectionConverter>();
                this.iocContainer.Register<IConverter<string, CraftingDisciplines>, CraftingDisciplineConverter>();
                this.iocContainer.Register<ITypeConverterFactory<RecipeDataModel, Recipe>, RecipeConverterFactory>();

                return this.iocContainer.Resolve<RecipeRepository>();
            }
        }

        /// <summary>Gets access to the skins data source.</summary>
        public SkinRepository Skins
        {
            get
            {
                this.iocContainer.Register(
                    Made.Of(() => new SkinRepository(
                        Arg.Of<HttpClient>("RepositoryClient"),
                        Arg.Of<IResponseConverter>(),
                        Arg.Of<ICache<int, Skin>>(),
                        Arg.Of<IConverter<int, int>>(),
                        Arg.Of<IConverter<SkinDataModel, Skin>>())));
                this.iocContainer.Register<IConverter<SkinDataModel, Skin>, SkinConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, SkinFlags>, SkinFlagCollectionConverter>();
                this.iocContainer.Register<IConverter<string, SkinFlags>, SkinFlagConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, ItemRestrictions>, ItemRestrictionCollectionConverter>();
                this.iocContainer.Register<IConverter<string, ItemRestrictions>, ItemRestrictionConverter>();
                this.iocContainer.Register<ITypeConverterFactory<SkinDataModel, Skin>, SkinConverterFactory>();

                return this.iocContainer.Resolve<SkinRepository>();
            }
        }

        /// <summary>Gets access to the worlds data source.</summary>
        public WorldRepository Worlds
        {
            get
            {
                this.iocContainer.Register(
                    Made.Of(() => new WorldRepository(
                        Arg.Of<HttpClient>("RepositoryClient"),
                        Arg.Of<IResponseConverter>(),
                        Arg.Of<ICache<int, World>>(),
                        Arg.Of<IConverter<int, int>>(),
                        Arg.Of<IConverter<WorldDataModel, World>>())));
                this.iocContainer.Register<IConverter<WorldDataModel, World>, WorldConverter>();

                return this.iocContainer.Resolve<WorldRepository>();
            }
        }
    }
}