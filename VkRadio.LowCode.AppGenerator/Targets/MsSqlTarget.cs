using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;

namespace VkRadio.LowCode.AppGenerator.Targets;

public class MsSqlTarget : SqlBaseTarget
{
    public MsSqlTarget(
        Guid id,
        string outputPath,
        string? devDbHost,
        string? devDbName,
        bool? devDbOsSecurityUseCurrentUser
    ) : base(
        id,
        outputPath,
        new DbParams(devDbHost, devDbName, devDbOsSecurityUseCurrentUser ?? false, null, null)
    )
    {
    }
}
