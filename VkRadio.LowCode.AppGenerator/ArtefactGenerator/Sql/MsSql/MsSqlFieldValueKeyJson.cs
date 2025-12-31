using System;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql
{
    public class MsSqlFieldValueKeyJson: FieldValueKeyJson
    {
        public MsSqlFieldValueKeyJson(PredefinedInsertJson in_predefinedInsert, ITableFieldJson in_field, Guid in_value)
            : base(in_predefinedInsert, in_field)
        {
            this.Value = DBSchemaHelper.GuidToMsSqlValueString(in_value);
        }
    };
}
