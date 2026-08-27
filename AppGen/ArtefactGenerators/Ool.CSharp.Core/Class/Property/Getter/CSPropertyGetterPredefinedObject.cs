using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Getter;

public class CSPropertyGetterPredefinedObject : CSPropertyGetter
{
    public CSPropertyGetterPredefinedObject(CSProperty property)
        : base(property)
    {
    }

    public override string[] GenerateText()
    {
        var dotClassName = CSharpHelper.GenerateDOTClassName(CorrespondingPEI.DOTDefinition);
        return [$"get => ({dotClassName})StorageRegistry.Instance.{dotClassName}Storage.Restore({IdConstName});"];
    }

    public PredefinedEntityInstance CorrespondingPEI { get; set; }
    public string IdConstName { get; set; }
}
