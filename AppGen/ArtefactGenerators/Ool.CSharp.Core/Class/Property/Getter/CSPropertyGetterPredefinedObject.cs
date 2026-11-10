using Ardalis.GuardClauses;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Getter;

public class CSPropertyGetterPredefinedObject : CSPropertyGetter
{
    public CSPropertyGetterPredefinedObject(CSProperty property, PredefinedEntityInstance correspondingPredefinedObject, string idConstName)
        : base(property)
    {
        CorrespondingPEI = Guard.Against.Null(correspondingPredefinedObject, nameof(correspondingPredefinedObject));
        IdConstName = Guard.Against.NullOrEmpty(idConstName, nameof(idConstName));
    }

    public override string[] GenerateText()
    {
        var entityClassName = CSharpHelper.GenerateEntityClassName(CorrespondingPEI.EntityDefinition);
        return [$"get => ({entityClassName})StorageRegistry.Instance.{entityClassName}Storage.Restore({IdConstName});"];
    }

    public PredefinedEntityInstance CorrespondingPEI { get; private set; }

    public string IdConstName { get; private set; }
}
