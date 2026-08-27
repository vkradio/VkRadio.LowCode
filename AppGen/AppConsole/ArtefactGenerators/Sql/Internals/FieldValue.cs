namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

public class FieldValue
{
    ITableField _field;
    string _value;
    PredefinedInsert _predefinedInsert;

    public ITableField Field { get { return _field; } set { _field = value; } }
    public string Value { get { return _value; } set { _value = value; } }
    public PredefinedInsert PredefinedInsert { get { return _predefinedInsert; } set { _predefinedInsert = value; } }
}
