using Ardalis.GuardClauses;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

// TODO: Here an extraction of collections from is hard-coded for MS SQL, thus is won't work for MySQL or other database systems. Need to generalize it.

/*
namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Getter;

/// <summary>
/// Getter for a collection property (table part, etc.)
/// </summary>
public class CSPropertyGetterCollection : ICSProperty //CSPropertyGetter
{
    PropertyDefinition _propertyDefinition;

    public CSPropertyGetterCollection(CSPropertyCollectionReader property, PropertyDefinition propDef)
    //: base(property, true, "")
    //=> _propertyDefinition = propDef;
    {
        _propertyDefinition = Guard.Against.Null(propDef, nameof(propDef));
    }

    public string Name => throw new NotImplementedException();

    public string[] GenerateText()
    {
        var text = new List<string>();

        if (SingleLineHint)
        {
            var filterPropName = NameHelper.NamesToPascalCase(_propertyDefinition.Names, true) + "Filter";
            text.Add($"get => StorageRegistry.Instance.{Property.DOTType}Storage.ReadAsTable({filterPropName});");
        }
        else
        {
            throw new NotImplementedException("Generating not-single-line getter for CSPropertyGetterCachedObject not implemented.");
        }

        return [.. text];
    }
}
*/
