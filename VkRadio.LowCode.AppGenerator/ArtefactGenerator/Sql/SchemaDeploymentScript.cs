namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;

/// <summary>
/// Logical representation of an SQL script that creates a database
/// </summary>
public abstract class SchemaDeploymentScript
{
    protected DBSchemaMetaModel _dbSchemaMetaModel;
    protected Dictionary<string, Table> _tables = new();
    protected Table[] _tablesSorted;
    protected List<ForeignKeyConstraint> _fkConstraints = new();
    protected List<PredefinedInsert> _predefinedInserts = new();
    protected string _quoteSymbol = string.Empty;

    protected virtual void SortTables()
    {
        var tablesSorted = new List<Table>(_tables.Values);
        tablesSorted.Sort((t1, t2) => string.Compare(t1.Name, t2.Name));
        _tablesSorted = tablesSorted.ToArray();
    }

    public SchemaDeploymentScript(DBSchemaMetaModel dbSchemaMetaModel) => _dbSchemaMetaModel = dbSchemaMetaModel;

    /// <summary>
    /// Database scheme metamodel
    /// </summary>
    public DBSchemaMetaModel DBSchemaMetaModel { get { return _dbSchemaMetaModel; } }
    /// <summary>
    /// Logical description of tables to be created
    /// </summary>
    public IDictionary<string, Table> Tables => _tables;
    /// <summary>
    /// Foreign key constraints
    /// </summary>
    public List<ForeignKeyConstraint> FKConstraints => _fkConstraints;
    /// <summary>
    /// Logical descriptions of predifined inserted rows
    /// </summary>
    public List<PredefinedInsert> PredefinedInserts => _predefinedInserts;
    public string QuoteSymbol => _quoteSymbol;

    /// <summary>
    /// Generate SQL script strings
    /// </summary>
    /// <returns>Array of SQL script strings</returns>
    public abstract string[] Generate();

    public IList<Table> TablesSorted
    {
        get
        {
            if (_tablesSorted is null)
            {
                SortTables();
            }

            return _tablesSorted;
        }
    }
}
