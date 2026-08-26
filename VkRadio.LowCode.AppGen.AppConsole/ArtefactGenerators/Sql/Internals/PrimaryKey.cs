namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

public abstract class PrimaryKey
{
    protected Table _table;

    public Table Table { get { return _table; } set { _table = value; } }
}
