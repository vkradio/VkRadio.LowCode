namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;

/// <summary>
/// Abstract correspondence between a table and its prototype in a main MetaModel
/// </summary>
public abstract class TableAndSourceCorrespondence
{
    protected Table _table;
    protected DBSchemaDomainModel _dbSchemaDomainModel;

    /// <summary>
    /// Table that has a correspondence
    /// </summary>
    public Table Table { get => _table; set => _table = value; }

    /// <summary>
    /// DomainModel
    /// </summary>
    public DBSchemaDomainModel DBSchemaDomainModel { get => _dbSchemaDomainModel; set => _dbSchemaDomainModel = value; }
}
