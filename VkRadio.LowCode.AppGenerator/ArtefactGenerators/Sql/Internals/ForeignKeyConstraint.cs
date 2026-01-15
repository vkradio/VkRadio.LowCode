using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

/// <summary>
/// Link tables using Foreign Keys
/// </summary>
public class ForeignKeyConstraint : ITextDefinition
{
    protected string _quoteSymbol = "\"";

    public ForeignKeyConstraint(string tableName, string refTableName, string refFieldName, OnDeleteActionEnum onDeleteAction)
    {
        TableName = tableName;
        RefTableName = refTableName;
        RefFieldName = refFieldName;
        OnDeleteAction = onDeleteAction;
    }

    /// <summary>
    /// Table name on that constraint is applied
    /// </summary>
    public string TableName { get; private set; }
    /// <summary>
    /// Table name on that FK is referencing
    /// </summary>
    public string RefTableName { get; private set; }
    /// <summary>
    /// FK field name
    /// </summary>
    public string RefFieldName { get; private set; }
    /// <summary>
    /// What to do when referenced by FK row is being deleted
    /// </summary>
    public OnDeleteActionEnum OnDeleteAction { get; private set; }

    public string[] GenerateText()
    {
        var result = new List<string>
        {
            string.Format("alter table {0}{1}{0}", _quoteSymbol, TableName),
            string.Format("\tadd constraint {0}fk_{1}_{2}{0}", _quoteSymbol, TableName, RefFieldName),
            string.Format("\tforeign key ({0}{1}{0}) references {0}{2}{0} ({0}id{0})", _quoteSymbol, RefFieldName, RefTableName)
        };

        if (OnDeleteAction == OnDeleteActionEnum.CannotDelete)
        {
            result[result.Count - 1] += ";";
        }
        else
        {
            var deleteCommand = OnDeleteAction switch
            {
                OnDeleteActionEnum.Delete => "cascade",
                OnDeleteActionEnum.ResetToNull => "set null",
                OnDeleteActionEnum.ResetToDefault => "set default",
                _ => throw new ApplicationException(string.Format("ForeignKeyConstraint for table {0}.{1} has unsupported FK delete action: {2}.", TableName, RefFieldName, (int)OnDeleteAction)),
            };

            result.Add(string.Format("\ton delete {0};", deleteCommand));
        }

        result.Add("go");

        return result.ToArray();
    }
}
