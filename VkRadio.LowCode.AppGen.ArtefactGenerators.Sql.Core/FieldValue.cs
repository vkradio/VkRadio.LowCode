namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;

public class FieldValue
{
    ITableField _field;
    string _value;
    PredefinedInsert _predefinedInsert;

    public ITableField Field { get => _field; set => _field = value; }

    public string Value { get => _value; set => _value = value; }

    public PredefinedInsert PredefinedInsert { get => _predefinedInsert; set => _predefinedInsert = value; }
}
