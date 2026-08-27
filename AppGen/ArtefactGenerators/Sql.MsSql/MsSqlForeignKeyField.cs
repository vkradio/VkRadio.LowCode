using VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.MsSql;

class MsSqlForeignKeyField : ForeignKeyField, IMsSqlConstraint
{
    protected override string CreateDefaultValue(SRefObject srefObject)
    {
        return DBSchemaHelper.GuidToMsSqlValueString(srefObject.Key);
    }

    public MsSqlForeignKeyField(TableAndEntityCorrespondence tableAndEntityCorrespondence, PropertyDefinition propertyDefinition)
        : base(tableAndEntityCorrespondence, propertyDefinition)
    {
        SqlType = C_SQL_TYPE;
        QuoteSymbol = "\"";
    }

    public const string C_SQL_TYPE = "uniqueidentifier";

    public IList<string> GenerateConstraints()
    {
        var result = new List<string>();

        if (Unique)
        {
            result.Add($"alter table {QuoteSymbol}{Table.Name}{QuoteSymbol} add constraint {QuoteSymbol}ux_{Table.Name}_{Name}{QuoteSymbol} unique nonclustered ({QuoteSymbol}{Name}{QuoteSymbol});");
        }

        if (DefaultValue is not null)
        {
            result.Add($"alter table {QuoteSymbol}{Table.Name}{QuoteSymbol} add constraint {QuoteSymbol}df_{Table.Name}_{Name}{QuoteSymbol} default {DefaultValue} for {QuoteSymbol}{Name}{QuoteSymbol};");
        }

        return result;
    }
}
