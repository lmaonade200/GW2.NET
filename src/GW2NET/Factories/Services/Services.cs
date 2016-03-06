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
    using GW2NET.Items;
    using GW2NET.MapInformation;
    using GW2NET.MapInformation.ApiModels;
    using GW2NET.MapInformation.Converter;
    using GW2NET.Maps;
    using GW2NET.Quaggans;
    using GW2NET.Recipes;
    using GW2NET.Skins;
    using GW2NET.V1.Guilds;
    using GW2NET.V2.Builds;
    using GW2NET.V2.Colors;
    using GW2NET.V2.Files;
    using GW2NET.V2.Items;
    using GW2NET.V2.Items.Converters;
    using GW2NET.V2.Items.Json;
    using GW2NET.V2.Quaggans;
    using GW2NET.V2.Recipes;
    using GW2NET.V2.Recipes.Converters;
    using GW2NET.V2.Recipes.Json;
    using GW2NET.V2.Skins;
    using GW2NET.V2.Skins.Converters;
    using GW2NET.V2.Skins.Json;
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
        public IBuildRepository Builds
        {
            get
            {
                this.iocContainer.Register<IBuildRepository, BuildRepository>(
                    Made.Of(() => new BuildRepository(
                        Arg.Of<HttpClient>("RepositoryClient"),
                        Arg.Of<IResponseConverter>(),
                        Arg.Of<IConverter<BuildDataContract, Build>>())));
                this.iocContainer.Register<IConverter<BuildDataContract, Build>, BuildConverter>();

                return this.iocContainer.Resolve<IBuildRepository>();
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
                    Arg.Of<IConverter<ColorPaletteDataContract, ColorPalette>>())));

                this.iocContainer.Register<IConverter<ColorPaletteDataContract, ColorPalette>, ColorPaletteConverter>();
                this.iocContainer.Register<IConverter<int[], Color>, ColorConverter>();
                this.iocContainer.Register<IConverter<ColorDataContract, ColorModel>, ColorModelConverter>();

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
                    Arg.Of<IConverter<ContinentDataContract, Continent>>())));

                this.iocContainer.Register<IConverter<ContinentDataContract, Continent>>();

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
                   Arg.Of<IConverter<FileDataContract, Asset>>())));

                this.iocContainer.Register<IConverter<FileDataContract, Asset>, AssetConverter>();

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
                        Arg.Of<IConverter<GuildDataContract, Guild>>())));

                this.iocContainer.Register<IConverter<GuildDataContract, Guild>>();
                this.iocContainer.Register<IConverter<EmblemDataContract, Emblem>>();
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
                        Arg.Of<IConverter<ItemDTO, Item>>())));

                this.iocContainer.Register<IConverter<ItemDTO, Item>, ItemConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, ItemRestrictions>, ItemRestrictionCollectionConverter>();
                this.iocContainer.Register<IConverter<string, ItemRestrictions>, ItemRestrictionConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, ItemFlags>, ItemFlagCollectionConverter>();
                this.iocContainer.Register<IConverter<string, ItemFlags>, ItemFlagConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, GameTypes>, GameTypeCollectionConverter>();
                this.iocContainer.Register<IConverter<string, GameTypes>, GameTypeConverter>();
                this.iocContainer.Register<IConverter<string, ItemRarity>, ItemRarityConverter>();
                this.iocContainer.Register<ITypeConverterFactory<ItemDTO, Item>, ItemConverterFactory>();

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
                        Arg.Of<IConverter<MapDataContract, Map>>())));

                this.iocContainer.Register<IConverter<MapDataContract, Map>, MapConverter>();
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
                        Arg.Of<IConverter<QuagganDataContract, Quaggan>>())));

                this.iocContainer.Register<IConverter<QuagganDataContract, Quaggan>, QuagganConverter>();

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
                        Arg.Of<IConverter<RecipeDTO, Recipe>>())));
                this.iocContainer.Register<RecipeConverter>();
                this.iocContainer.Register<IConverter<ICollection<IngredientDTO>, ICollection<ItemQuantity>>, CollectionConverter<IngredientDTO, ItemQuantity>>();
                this.iocContainer.Register<IConverter<IngredientDTO, ItemQuantity>, ItemQuantityConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, RecipeFlags>, RecipeFlagCollectionConverter>();
                this.iocContainer.Register<IConverter<string, RecipeFlags>, RecipeFlagConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, CraftingDisciplines>, CraftingDisciplineCollectionConverter>();
                this.iocContainer.Register<IConverter<string, CraftingDisciplines>, CraftingDisciplineConverter>();
                this.iocContainer.Register<ITypeConverterFactory<RecipeDTO, Recipe>, RecipeConverterFactory>();

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
                        Arg.Of<IConverter<SkinDTO, Skin>>())));
                this.iocContainer.Register<IConverter<SkinDTO, Skin>, SkinConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, SkinFlags>, SkinFlagCollectionConverter>();
                this.iocContainer.Register<IConverter<string, SkinFlags>, SkinFlagConverter>();
                this.iocContainer.Register<IConverter<ICollection<string>, ItemRestrictions>, ItemRestrictionCollectionConverter>();
                this.iocContainer.Register<IConverter<string, ItemRestrictions>, ItemRestrictionConverter>();
                this.iocContainer.Register<ITypeConverterFactory<SkinDTO, Skin>, SkinConverterFactory>();

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
                        Arg.Of<IConverter<WorldDataContract, World>>())));
                this.iocContainer.Register<IConverter<WorldDataContract, World>, WorldConverter>();

                return this.iocContainer.Resolve<WorldRepository>();
            }
        }
    }
}