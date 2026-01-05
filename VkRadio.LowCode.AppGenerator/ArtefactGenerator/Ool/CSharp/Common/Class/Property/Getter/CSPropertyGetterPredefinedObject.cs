using VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter;

public class CSPropertyGetterPredefinedObject : CSPropertyGetter
{
    public CSPropertyGetterPredefinedObject(CSProperty property)
        : base(property)
    {
    }

    public override string[] GenerateText()
    {
        var dotClassName = CSharpHelper.GenerateDOTClassName(CorrespondingPDO.DOTDefinition);
        return [$"get => ({dotClassName})StorageRegistry.Instance.{dotClassName}Storage.Restore({IdConstName});"];
    }

    public PredefinedDO CorrespondingPDO { get; set; }
    public string IdConstName { get; set; }
}
