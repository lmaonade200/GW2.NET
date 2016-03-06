namespace GW2NET.Factories.V2
{
    using System.Diagnostics;
    using Common;

    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Converter;

    using Skins;
    public class GatheringToolSkinConverterFactory : ITypeConverterFactory<SkinDataModel, GatheringToolSkin>
    {
        public IConverter<SkinDataModel, GatheringToolSkin> Create(string discriminator)
        {
            switch (discriminator)
            {
                case "Foraging":
                    return new ForagingToolSkinConverter();
                case "Logging":
                    return new LoggingToolSkinConverter();
                case "Mining":
                    return new MiningToolSkinConverter();
                default:
                    Debug.Assert(false, "Unknown type discriminator: " + discriminator);
                    return new UnknownGatheringToolSkinConverter();
            }
        }
    }
}
