using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.MsSql;
using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core;

public class CSharpTarget : Target
{
    public MsSqlTarget DependencyTargetMsSql { get; private set; } = default!;

    public CSharpTarget(
        Guid id,
        ArtefactTypeEnum type,
        ArtefactGenerationProject project,
        XElement xelTarget,
        string? outputPath,
        Guid? useOutputPathFromTargetId,
        IEnumerable<Guid> dependsOnIds,
        Func<ArtefactTypeEnum, DomainModel, Target, ArtefactGenerator> artefactGeneratorConstructor
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
    }

    //public override Task InitializeAfterLoad()
    //{
    //    base.InitializeAfterLoad();

    //    DependencyTargetMsSql = Project
    //        .Targets
    //        .FirstOrDefault(x => x.Type == ArtefactTypeCodeEnum.MsSql) as MsSqlTarget
    //            ?? throw new ApplicationException($"{ArtefactTypeCodeEnum.MsSql} dependency Target not found for {Type} Target Id {Id}");

    //    return Task.CompletedTask;
    //}
}
