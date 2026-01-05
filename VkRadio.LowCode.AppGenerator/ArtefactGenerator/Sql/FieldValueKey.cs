namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;

public abstract class FieldValueKey : FieldValue
{
    public FieldValueKey(PredefinedInsert predefinedInsert, ITableField field)
    {
        PredefinedInsert = predefinedInsert;
        Field = field;
    }
}
