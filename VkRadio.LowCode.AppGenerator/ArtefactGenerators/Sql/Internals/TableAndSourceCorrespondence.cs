namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

/// <summary>
/// Abstract correspondence between a table and its prototype in a main MetaModel
/// </summary>
public abstract class TableAndSourceCorrespondence
{
    protected Table _table;
    protected DBSchemaMetaModel _dbSchemaMetaModel;

    /// <summary>
    /// Table that has a correspondence
    /// </summary>
    public Table Table { get { return _table; } set { _table = value; } }
    /// <summary>
    /// MetaModel
    /// </summary>
    public DBSchemaMetaModel DBSchemaMetaModel { get { return _dbSchemaMetaModel; } set { _dbSchemaMetaModel = value; } }
}
