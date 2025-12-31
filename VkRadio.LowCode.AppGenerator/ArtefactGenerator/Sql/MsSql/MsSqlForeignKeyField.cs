using System.Collections.Generic;

using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql
{
    class MsSqlForeignKeyField : ForeignKeyField, IMsSqlConstraint
    {
        protected override string CreateDefaultValue(SRefObject in_srefObject)
        {
            return DBSchemaHelper.GuidToMsSqlValueString(in_srefObject.Key);
        }

        public MsSqlForeignKeyField(TableAndDOTCorrespondence in_tableAndDOTCorrespondence, PropertyDefinition in_propertyDefinition)
            : base(in_tableAndDOTCorrespondence, in_propertyDefinition)
        {
            SqlType = C_SQL_TYPE;
            QuoteSymbol = "\"";
        }

        public const string C_SQL_TYPE = "uniqueidentifier";

        public IList<string> GenerateConstraints()
        {
            List<string> result = new List<string>();
            if (Unique)
                result.Add($"alter table {QuoteSymbol}{Table.Name}{QuoteSymbol} add constraint {QuoteSymbol}ux_{Table.Name}_{Name}{QuoteSymbol} unique nonclustered ({QuoteSymbol}{Name}{QuoteSymbol});");
            if (DefaultValue != null)
                result.Add($"alter table {QuoteSymbol}{Table.Name}{QuoteSymbol} add constraint {QuoteSymbol}df_{Table.Name}_{Name}{QuoteSymbol} default {DefaultValue} for {QuoteSymbol}{Name}{QuoteSymbol};");
            return result;
        }
    };
}
