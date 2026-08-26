using VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.MsSql;

public class MsSqlPredefinedInsert : PredefinedInsert
{
    public MsSqlPredefinedInsert()
    {
        _quoteSymbol = "\"";
    }
}
