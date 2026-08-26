namespace VkRadio.LowCode.AppGen.ArtefactGenerators.SqlCore;

/// <summary>
/// Logical representation of an SQL script that creates a database
/// </summary>
public abstract class SchemaDeploymentScript
{
    protected DBSchemaDomainModel _dbSchemaDomainModel;
    protected Dictionary<string, Table> _tables = [];
    protected Table[] _tablesSorted;
    protected List<ForeignKeyConstraint> _fkConstraints = [];
    protected List<PredefinedInsert> _predefinedInserts = [];
    protected string _quoteSymbol = string.Empty;

    protected virtual void SortTables()
    {
        _tablesSorted = _tables
            .Values
            .OrderBy(t => t.Name)
            .ToArray();
    }

    public SchemaDeploymentScript(DBSchemaDomainModel dbSchemaMetaModel) => _dbSchemaDomainModel = dbSchemaMetaModel;

    /// <summary>
    /// Database scheme metamodel
    /// </summary>
    public DBSchemaDomainModel DBSchemaDomainModel => _dbSchemaDomainModel;

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

            return _tablesSorted!;
        }
    }
}
