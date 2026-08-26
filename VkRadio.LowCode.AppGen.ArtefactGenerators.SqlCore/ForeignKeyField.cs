using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.SqlCore;

/// <summary>
/// Foreign Key field of a table
/// </summary>
public abstract class ForeignKeyField : ITableField
{
    protected abstract string CreateDefaultValue(SRefObject srefObject);

    public string QuoteSymbol { get; protected set; }
    
    public string Name { get; protected set; }
    
    public bool Nullable { get; protected set; }
    
    public string SqlType { get; protected set; }
    
    public Table Table { get; protected set; }
    
    public PropertyCorrespondence EntityPropertyCorrespondence { get; protected set; }
    
    public string DefaultValue { get; protected set; }
    
    public bool Unique { get; protected set; }

    public ForeignKeyField(TableAndEntityCorrespondence tableAndEntityCorrespondence, PropertyDefinition propertyDefinition)
    {
        Table = tableAndEntityCorrespondence.Table;
        Name = NameHelper.NameToUnderscoreSeparatedName(propertyDefinition.Names[NaturalLanguageEnum.En]) + "_id";
        Nullable = propertyDefinition.FunctionalType.Nullable;
        Unique = propertyDefinition.FunctionalType.Unique;
        EntityPropertyCorrespondence = new PropertyCorrespondence
        {
            PropertyDefinition = propertyDefinition,
            TableAndEntityCorrespondence = tableAndEntityCorrespondence,
            TableField = this
        };
    }

    public void Init()
    {
        if (EntityPropertyCorrespondence.PropertyDefinition.DefaultValue is not null)
        {
            var value = (SRefObject)EntityPropertyCorrespondence.PropertyDefinition.DefaultValue;
            DefaultValue = CreateDefaultValue(value);
        }
    }

    public virtual string[] GenerateText()
    {
        var result = $"{QuoteSymbol}{Name}{QuoteSymbol} {SqlType} {(Nullable ? "null" : "not null")}";

        if (Table.SchemaDeploymentScript.DBSchemaDomainModel.GenerateConstraintsInline)
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
