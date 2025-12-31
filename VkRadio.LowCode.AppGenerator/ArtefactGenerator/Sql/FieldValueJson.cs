namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    public class FieldValueJson
    {
        ITableFieldJson _field;
        string _value;
        PredefinedInsertJson _predefinedInsert;

        public ITableFieldJson Field { get { return _field; } set { _field = value; } }
        public string Value { get { return _value; } set { _value = value; } }
        public PredefinedInsertJson PredefinedInsert { get { return _predefinedInsert; } set { _predefinedInsert = value; } }
    };
}
