namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

public abstract class FieldValueKey : FieldValue
{
    public FieldValueKey(PredefinedInsert predefinedInsert, ITableField field)
    {
        PredefinedInsert = predefinedInsert;
        Field = field;
    }
}
