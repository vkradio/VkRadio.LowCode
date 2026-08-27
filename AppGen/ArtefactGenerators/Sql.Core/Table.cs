namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;

public abstract class Table : ITextDefinition
{
    protected string _name;
    protected SchemaDeploymentScript _schemaDeploymentScript;
    //protected List<DynamicTypeForeignKey> _dynamicTypeForeignKeys = new List<DynamicTypeForeignKey>();
    protected List<ForeignKeyField> _foreignKeyFields = [];
    protected List<ValueField> _valueFields = [];
    protected PrimaryKey _primaryKey;
    protected string _quoteSymbol;

    /// <summary>
    /// Private constructor, to prevent creation of a table without parameters
    /// </summary>
    private Table() { }

    /// <summary>
    /// Public constructor to set a table name
    /// </summary>
    /// <param name="name">Table name</param>
    /// <param name="schemaDeploymentScript">Logical representation of an SQL database deployment script</param>
    public Table(string name, SchemaDeploymentScript schemaDeploymentScript)
    {
        _name = name;
        _schemaDeploymentScript = schemaDeploymentScript;
    }

    /// <summary>
    /// Generate a table declaration &quot;tail&quot; - that located between a closing bracket and a semicolon (;)
    /// </summary>
    /// <returns></returns>
    protected virtual string GenerateTableDefTail() => string.Empty;

    public string Name => _name;

    public SchemaDeploymentScript SchemaDeploymentScript => _schemaDeploymentScript;

    //public IList<DynamicTypeForeignKey> DynamicTypeForeignKeys => _dynamicTypeForeignKeys;

    public IList<ForeignKeyField> ForeignKeyFields => _foreignKeyFields;

    public IList<ITableField> AllFields
    {
        get
        {
            var fields = new List<ITableField>();

            // Add a Primary Key
            if (_primaryKey is PKSingle)
            {
                fields.Add((PKSingle)_primaryKey);
            }
            //else if (_primaryKey is PKCompound)
            //{
            //    PKCompound pkCompound = (PKCompound)_primaryKey;
            //    foreach (PKCompoundPart keyPart in pkCompound.Parts)
            //        fields.Add(keyPart);
            //}
            else
            {
                throw new ApplicationException(string.Format("Unsupported PrimaryKey type: {0}.", _primaryKey.GetType().Name));
            }

            // Add fields for explicit (non-reference) values
            foreach (var field in _valueFields)
            {
                fields.Add(field);
            }

            // Add fields for ordinary FKs
            foreach (var fk in _foreignKeyFields)
            {
                fields.Add(fk);
            }

            //// Add fields for dynamically typed compound FKs
            //foreach (DynamicTypeForeignKey dynFK in _dynamicTypeForeignKeys)
            //{
            //    fields.Add(dynFK.RefField);
            //    fields.Add(dynFK.TypeField);
            //}

            return fields;
        }
    }

    public IList<ValueField> ValueFields => _valueFields;

    public PrimaryKey PrimaryKey { get => _primaryKey; set => _primaryKey = value; }

    public string QuoteSymbol => _quoteSymbol;

    public string[] GenerateText()
    {
        var result = new List<string>
        {
            string.Format("create table {0}{1}{2}", _quoteSymbol, _name, _quoteSymbol),
            "("
        };

        var fields = AllFields;

        for (var i = 0; i < fields.Count; i++)
        {
            var fieldStrings = fields[i].GenerateText();

            if (fieldStrings.Length != 0)
            {
                for (var j = 0; j < fieldStrings.Length; j++)
                {
                    fieldStrings[j] = DBSchemaHelper.C_TAB + fieldStrings[j];
                }

                if (i < fields.Count - 1)
                {
                    fieldStrings[^1] += ",";
                }

                result.AddRange(fieldStrings);
            }
        }

        result.Add(")" + GenerateTableDefTail() + ";");

        return [.. result];
    }
}
