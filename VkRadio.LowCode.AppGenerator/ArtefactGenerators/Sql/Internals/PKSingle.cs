namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

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
    public string Name { get { return _name; } }
    /// <summary>
    /// Are NULL values allowed (obviously never allowed for PKs)
    /// </summary>
    public bool Nullable => false;
    /// <summary>
    /// SQL type (string literal)
    /// </summary>
    public string SqlType { get { return _sqlType; } }
    public PropertyCorrespondence DOTPropertyCorrespondence { get { return null; } }
    public bool Unique { get { return true; } }

    public PKSingle()
    {
        _name = "id";
    }

    public virtual string[] GenerateText()
    {
        return [string.Format("{0}{1}{2} {3} not null primary key", _quoteSymbol, _name, _quoteSymbol, _sqlType)];
    }
}
