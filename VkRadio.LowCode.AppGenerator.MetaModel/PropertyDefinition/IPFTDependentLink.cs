namespace MetaModel.PropertyDefinition
{
    public interface IPFTDependentLink
    {
        OnDeleteActionEnum OnDeleteAction { get; set; }
        void SetDefaultOnDeleteAction();
    };
}
