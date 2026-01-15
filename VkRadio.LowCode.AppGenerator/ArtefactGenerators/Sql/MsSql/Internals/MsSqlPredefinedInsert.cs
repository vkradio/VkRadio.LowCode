using VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.MsSql.Internals;

public class MsSqlPredefinedInsert : PredefinedInsert
{
    public MsSqlPredefinedInsert()
    {
        _quoteSymbol = "\"";
    }
}
