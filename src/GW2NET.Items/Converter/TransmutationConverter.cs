namespace GW2NET.Items.Converter
{
    using System.Collections.Generic;

    using GW2NET.Items.ApiModels;

    public partial class TransmutationConverter
    {
        partial void Merge(Transmutation entity, ItemDataModel dataModel, object state)
        {
            if (dataModel.Details.Skins == null)
            {
                entity.SkinIds = new List<int>(0);
            }
            else
            {
                var values = new List<int>(dataModel.Details.Skins.Length);
                values.AddRange(dataModel.Details.Skins);
                entity.SkinIds = values;
            }
        }
    }
}
