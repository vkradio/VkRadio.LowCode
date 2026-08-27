namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;

public abstract class FieldValueKey : FieldValue
{
    public FieldValueKey(PredefinedInsert predefinedInsert, ITableField field)
    {
        PredefinedInsert = predefinedInsert;
        Field = field;
    }
}
