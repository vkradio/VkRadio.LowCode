using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;
using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Getter;

public class CSPropertyGetterPredefinedObject : CSPropertyGetter
{
    public CSPropertyGetterPredefinedObject(CSProperty property, PredefinedEntityInstance correspondingPredefinedObject, string idConstName)
        : base(property)
    {
        CorrespondingPEI = GuardRule.Forbid.Null(correspondingPredefinedObject, nameof(correspondingPredefinedObject));
        IdConstName = GuardRule.Forbid.NullOrEmpty(idConstName, nameof(idConstName));
    }

    public override string[] GenerateText()
    {
        var type = (CSharpReferenceType)Property.Type;
        var notNullableExclamation = type.SupportNullableRefTypes;

        var entityClassName = CSharpHelper.GenerateEntityClassName(CorrespondingPEI.EntityDefinition);

        return [$"get => ({entityClassName})StorageRegistry.Instance.{entityClassName}Storage.Restore({IdConstName}){(notNullableExclamation ? "!" : string.Empty)};"];
    }

    public PredefinedEntityInstance CorrespondingPEI { get; private set; }

    public string IdConstName { get; private set; }
}
