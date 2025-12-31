using System;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql
{
    public class MsSqlFieldValueKey : FieldValueKey
    {
        public MsSqlFieldValueKey(PredefinedInsert in_predefinedInsert, ITableField in_field, Guid in_value)
            : base(in_predefinedInsert, in_field)
        {
            Value = DBSchemaHelper.GuidToMsSqlValueString(in_value);
        }
    }
}
