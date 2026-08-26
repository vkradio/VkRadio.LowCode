namespace VkRadio.LowCode.AppGen.ArtefactGenerators.SqlCore;

public abstract class PrimaryKey
{
    protected Table _table;

    public Table Table { get => _table; set => _table = value; }
}
