using VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.MsSql;

public class MsSqlFieldValueKey : FieldValueKey
{
    public MsSqlFieldValueKey(PredefinedInsert predefinedInsert, ITableField field, Guid value)
        : base(predefinedInsert, field)
    {
        Value = DBSchemaHelper.GuidToMsSqlValueString(value);
    }
}
