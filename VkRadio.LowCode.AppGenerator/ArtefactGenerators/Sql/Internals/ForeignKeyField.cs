using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

/// <summary>
/// Foreign Key field of a table
/// </summary>
public abstract class ForeignKeyField : ITableField
{
    protected abstract string CreateDefaultValue(SRefObject in_srefObject);

    public string QuoteSymbol { get; protected set; }
    
    public string Name { get; protected set; }
    
    public bool Nullable { get; protected set; }
    
    public string SqlType { get; protected set; }
    
    public Table Table { get; protected set; }
    
    public PropertyCorrespondence DOTPropertyCorrespondence { get; protected set; }
    
    public string DefaultValue { get; protected set; }
    
    public bool Unique { get; protected set; }

    public ForeignKeyField(TableAndDOTCorrespondence tableAndDOTCorrespondence, PropertyDefinition propertyDefinition)
    {
        Table = tableAndDOTCorrespondence.Table;
        Name = NameHelper.NameToUnderscoreSeparatedName(propertyDefinition.Names[HumanLanguageEnum.En]) + "_id";
        Nullable = propertyDefinition.FunctionalType.Nullable;
        Unique = propertyDefinition.FunctionalType.Unique;
        DOTPropertyCorrespondence = new PropertyCorrespondence { PropertyDefinition = propertyDefinition, TableAndDOTCorrespondence = tableAndDOTCorrespondence, TableField = this };
    }

    public void Init()
    {
        if (DOTPropertyCorrespondence.PropertyDefinition.DefaultValue != null)
        {
            var value = (SRefObject)DOTPropertyCorrespondence.PropertyDefinition.DefaultValue;
            DefaultValue = CreateDefaultValue(value);
        }
    }

    public virtual string[] GenerateText()
    {
        var result = $"{QuoteSymbol}{Name}{QuoteSymbol} {SqlType} {(Nullable ? "null" : "not null")}";

        if (Table.SchemaDeploymentScript.DBSchemaMetaModel.GenerateConstraintsInline)
        {
            if (Unique)
            {
                result += " " + DBSchemaHelper.C_KEYWORD_UNIQUE;
            }

            if (DefaultValue is not null)
            {
                result += $" {DBSchemaHelper.C_KEYWORD_DEFAULT} {DefaultValue}";
            }
        }

        // TODO: Also need to do something with constraints, or are they already implemented?
        return [result];
    }
}
