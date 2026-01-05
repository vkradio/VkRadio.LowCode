namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql.MsSql;

public class MsSqlPKSingle : PKSingle, IMsSqlConstraint
{
    public MsSqlPKSingle()
    {
        _sqlType = "uniqueidentifier";
        _quoteSymbol = "\"";
    }

    public override string[] GenerateText()
    {
        var result = base.GenerateText();
        result[0] = result[0].Replace(" primary key", string.Empty);
        return result;
    }

    public IList<string> GenerateConstraints()
    {
        var result = new List<string>
        {
            string.Format("alter table {0}{1}{0} add constraint {0}pk_{1}{0} primary key clustered ({0}{2}{0});", _quoteSymbol, _table.Name, _name)
        };

        return result;
    }
}
