using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;

// TODO: Here an extraction of collections from is hard-coded for MS SQL, thus is won't work for MySQL or other database systems. Need to generalize it.

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter;

/// <summary>
/// Getter for a collection property (table part, etc.)
/// </summary>
public class CSPropertyGetterCollection : CSPropertyGetter
{
    PropertyDefinition _propertyDefinition;

    public CSPropertyGetterCollection(CSProperty property, PropertyDefinition propDef)
        : base(property, true)
        => _propertyDefinition = propDef;

    public override string[] GenerateText()
    {
        var text = new List<string>();

        if (SingleLineHint)
        {
            var fieldName = NameHelper.NamesToPascalCase(_propertyDefinition.Names, true) + "Filter";
            text.Add($"get => StorageRegistry.Instance.{Property.DOTType}Storage.ReadAsTable({fieldName});");
        }
        else
        {
            throw new NotImplementedException("Generating not-single-line getter for CSPropertyGetterCachedObject not implemented.");
        }

        return text.ToArray();
    }
}
