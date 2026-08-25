namespace VkRadio.LowCode.ArtefactGenerators.SqlCore;

public abstract class FieldValueKey : FieldValue
{
    public FieldValueKey(PredefinedInsert predefinedInsert, ITableField field)
    {
        PredefinedInsert = predefinedInsert;
        Field = field;
    }
}
