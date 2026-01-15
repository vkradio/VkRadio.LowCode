using VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.MsSql.Internals;

public class MsSqlFieldValueKey : FieldValueKey
{
    public MsSqlFieldValueKey(PredefinedInsert predefinedInsert, ITableField field, Guid value)
        : base(predefinedInsert, field)
    {
        Value = DBSchemaHelper.GuidToMsSqlValueString(value);
    }
}
