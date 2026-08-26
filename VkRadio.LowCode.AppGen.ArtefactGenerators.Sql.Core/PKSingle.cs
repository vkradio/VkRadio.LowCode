namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;

/// <summary>
/// Primary Key, consisting of one field
/// </summary>
public abstract class PKSingle : PrimaryKey, ITableField
{
    protected string _name;
    protected string _sqlType;
    protected string _quoteSymbol;

    /// <summary>
    /// Table field name
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Are NULL values allowed (obviously never allowed for PKs)
    /// </summary>
    public bool Nullable => false;

    /// <summary>
    /// SQL type (string literal)
    /// </summary>
    public string SqlType => _sqlType;

    public PropertyCorrespondence? EntityPropertyCorrespondence => null;

    public bool Unique => true;

    public PKSingle()
    {
        _name = "id";
    }

    public virtual string[] GenerateText()
    {
        return [string.Format("{0}{1}{2} {3} not null primary key", _quoteSymbol, _name, _quoteSymbol, _sqlType)];
    }
}
