namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

public class PredefinedInsert : ITextDefinition
{
    protected const string c_insertPattern = "insert into {0}{1}{2} ({3}) values ({4});";

    protected SchemaDeploymentScript _schemaDeploymentScript;
    protected List<FieldValue> _fieldValues = new();
    protected Table _table;
    protected string _quoteSymbol = string.Empty;

    public SchemaDeploymentScript SchemaDeploymentScript { get { return _schemaDeploymentScript; } set { _schemaDeploymentScript = value; } }
    public IList<FieldValue> FieldValues { get { return _fieldValues; } }
    public Table Table { get { return _table; } set { _table = value; } }

    public string[] GenerateText()
    {
        var fieldsText = string.Empty;

        foreach (var fv in _fieldValues)
        {
            if (fieldsText != string.Empty)
            {
                fieldsText += ", ";
            }

            fieldsText += _quoteSymbol + fv.Field.Name + _quoteSymbol;
        }

        var valuesText = string.Empty;

        foreach (var fv in _fieldValues)
        {
            if (valuesText != string.Empty)
            {
                valuesText += ", ";
            }

            valuesText += fv.Value;
        }

        return
        [
            string.Format(
                c_insertPattern,
                _quoteSymbol, _table.Name, _quoteSymbol,
                fieldsText,
                valuesText
            )
        ];
    }

    public static int PredefinedInsertComparer(PredefinedInsert insert1, PredefinedInsert insert2)
    {
        var result = string.Compare(insert1.Table.Name, insert2.Table.Name);

        if (result == 0)
        {
            result = string.Compare(insert1.FieldValues[0].Value, insert2.FieldValues[0].Value);
        }

        return result;
    }
}
