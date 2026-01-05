namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql.MsSql;

public class MsSqlFieldValueKey : FieldValueKey
{
    public MsSqlFieldValueKey(PredefinedInsert predefinedInsert, ITableField field, Guid value)
        : base(predefinedInsert, field)
    {
        Value = DBSchemaHelper.GuidToMsSqlValueString(value);
    }
}
