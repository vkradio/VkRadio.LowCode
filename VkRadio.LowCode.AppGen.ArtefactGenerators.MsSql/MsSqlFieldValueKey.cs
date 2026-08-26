using VkRadio.LowCode.AppGen.ArtefactGenerators.SqlCore;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.MsSql;

public class MsSqlFieldValueKey : FieldValueKey
{
    public MsSqlFieldValueKey(PredefinedInsert predefinedInsert, ITableField field, Guid value)
        : base(predefinedInsert, field)
    {
        Value = DBSchemaHelper.GuidToMsSqlValueString(value);
    }
}
