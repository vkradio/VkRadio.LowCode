using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;

public abstract class SqlBaseTarget : Target
{
    public SqlBaseTarget(
        Guid id,
        ArtefactTypeEnum type,
        ArtefactGenerationProject project,
        XElement xelTarget,
        string? outputPath,
        Guid? useOutputPathFromTargetId,
        IEnumerable<Guid> dependsOnIds,
        Func<ArtefactTypeEnum, DomainModel, Target, ArtefactGenerator> artefactGeneratorConstructor,
        DbParams dbParams
    ) : base(
        id,
        type,
        project,
        xelTarget,
        outputPath,
        useOutputPathFromTargetId,
        dependsOnIds,
        artefactGeneratorConstructor
    )
    {
        DbParams = dbParams;
    }

    public DbParams DbParams { get; private set; }

    //protected override void InitConcrete(XElement xelTarget)
    //{
    //    var xel = xelTarget.Element("DevelopmentDbParams");

    //    if (xel is not null)
    //    {
    //        DbParams = DbParams.ReadFromXElement(xel);
    //    }
    //}
}
