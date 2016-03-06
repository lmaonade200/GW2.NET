namespace GW2NET.Items.Converter
{
    using GW2NET.Items.ApiModels;

    public partial class MiniatureConverter
    {
        partial void Merge(Miniature entity, ItemDataModel dataModel, object state)
        {
            entity.MiniatureId = dataModel.Details.MiniPetId;
        }
    }
}
