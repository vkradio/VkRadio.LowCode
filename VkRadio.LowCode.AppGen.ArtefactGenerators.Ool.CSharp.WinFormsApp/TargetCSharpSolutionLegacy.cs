using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp;

public class TargetCSharpSolutionLegacy : Target
{
    protected TargetCSharpSolutionLegacy(
        Guid id,
        ArtefactTypeEnum type,
        ArtefactGenerationProject project,
        XElement xelTarget,
        string? outputPath,
        Guid? useOutputPathFromTargetId,
        IEnumerable<Guid> dependsOnIds,
        Func<ArtefactTypeEnum, DomainModel, Target, ArtefactGenerator> artefactGeneratorConstructor)
        : base(id, type, project, xelTarget, outputPath, useOutputPathFromTargetId, dependsOnIds, artefactGeneratorConstructor)
    {
    }

    //protected override void CreateGenerator() => Generator = new ArtefactGeneratorCSharpClassic(this);

    public bool IsDependantOnSQLite { get => false; }

    public string? SQLiteProjectFullPath { get => null; }

    //public new TargetCSharpAppLegacy Parent { get => (TargetCSharpAppLegacy)base.Parent; }

    //public new ArtefactGeneratorCSharpClassic Generator { get; set; } // { get => (ArtefactGeneratorCSharpClassic)base.Generator; set => base.Generator = value; }
}
