using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;
using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.MsSql;

public class MsSqlTarget : SqlBaseTarget
{
    public MsSqlTarget(
        Guid id,
        ArtefactTypeEnum type,
        ArtefactGenerationProject project,
        XElement xelTarget,
        string? outputPath,
        Guid? useOutputPathFromTargetId,
        IEnumerable<Guid> dependsOnIds,
        Func<ArtefactTypeEnum, DomainModel, Target, ArtefactGenerator> artefactGeneratorConstructor,
        string? devDbHost,
        string? devDbName,
        bool? devDbOsSecurityUseCurrentUser
    ) : base(
        id,
        type,
        project,
        xelTarget,
        outputPath,
        useOutputPathFromTargetId,
        dependsOnIds,
        artefactGeneratorConstructor,
        new DbParams(devDbHost, devDbName, devDbOsSecurityUseCurrentUser ?? false, null, null)
    )
    {
    }
}
