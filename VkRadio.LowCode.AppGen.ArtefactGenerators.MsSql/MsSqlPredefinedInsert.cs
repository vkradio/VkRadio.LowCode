using VkRadio.LowCode.AppGen.ArtefactGenerators.SqlCore;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.MsSql;

public class MsSqlPredefinedInsert : PredefinedInsert
{
    public MsSqlPredefinedInsert()
    {
        _quoteSymbol = "\"";
    }
}
