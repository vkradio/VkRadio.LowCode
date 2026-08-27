using VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.MsSql.Internals;

public class MsSqlValueField : ValueField, IMsSqlConstraint
{
    public MsSqlValueField(TableAndDOTCorrespondence tableAndDOTCorrespondence, PropertyDefinition propertyDefinition)
        : base(tableAndDOTCorrespondence, propertyDefinition)
    {
        _sqlTypesWDeniedDefaults = [];
        _boolSqlType = "bit";
        _quoteSymbol = "\"";
        _generateConstraintsInline = false;
    }

    protected override void SetupStringField(PropertyDefinition propertyDefinition)
    {
        var pft = (PFTString)propertyDefinition.FunctionalType;
        var len = pft.MaxLength;
        // TODO: Need to clarify whether it should be 4000 or 8000, because here
        // http://stackoverflow.com/questions/564755/sql-server-text-type-vs-varchar-data-type
        // is stated 8000, but it was used in text, and ntext probably will be twice less
        _sqlType = string.Format("nvarchar({0})", len <= 4000 ? len.ToString() : "MAX");

        var defaultDeniedBySql = false;

        foreach (string sqlType in _sqlTypesWDeniedDefaults)
        {
            if (sqlType == _sqlType)
            {
                defaultDeniedBySql = true;
                break;
            }
        }

        SetupStringFieldDefault(defaultDeniedBySql, propertyDefinition);
    }

    protected override void SetupUniqueCodeField(PropertyDefinition propertyDefinition)
    {
        PFTUniqueCode pft = (PFTUniqueCode)propertyDefinition.FunctionalType;
        _sqlType = MsSqlForeignKeyFieldJson.C_SQL_TYPE;
        // TODO: No default here, like it is not required
    }

    public IList<string> GenerateConstraints()
    {
        var result = new List<string>();

        if (_unique)
        {
            result.Add(string.Format("alter table {0}{1}{0} add constraint {0}ux_{1}_{2}{0} unique nonclustered ({0}{2}{0});", _quoteSymbol, _table.Name, _name));
        }

        if (_defaultValue is not null)
        {
            result.Add(string.Format("alter table {0}{1}{0} add constraint {0}df_{1}_{2}{0} default {3} for {0}{2}{0};", _quoteSymbol, _table.Name, _name, _defaultValue));
        }

        return result;
    }
}
