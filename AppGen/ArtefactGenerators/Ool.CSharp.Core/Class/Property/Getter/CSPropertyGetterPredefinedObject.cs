using Ardalis.GuardClauses;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
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
        var type = (CSharpReferenceType)Property.Type;
        var notNullableExclamation = type.SupportNullableRefTypes;

        var entityClassName = CSharpHelper.GenerateEntityClassName(CorrespondingPEI.EntityDefinition);

        return [$"get => ({entityClassName})StorageRegistry.Instance.{entityClassName}Storage.Restore({IdConstName}){(notNullableExclamation ? "!" : string.Empty)};"];
    }

    public PredefinedEntityInstance CorrespondingPEI { get; private set; }

    public string IdConstName { get; private set; }
}
