using VkRadio.LowCode.AppGenerator.Targets;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic;

public class TargetCSharpSolutionLegacy : ArtefactGenerationTarget
{
    //protected override void CreateGenerator() => Generator = new ArtefactGeneratorCSharpClassic(this);

    public bool IsDependantOnSQLite { get => false; }
    public string? SQLiteProjectFullPath { get => null; }
    //public new TargetCSharpAppLegacy Parent { get => (TargetCSharpAppLegacy)base.Parent; }
    //public new ArtefactGeneratorCSharpClassic Generator { get; set; } // { get => (ArtefactGeneratorCSharpClassic)base.Generator; set => base.Generator = value; }
}
