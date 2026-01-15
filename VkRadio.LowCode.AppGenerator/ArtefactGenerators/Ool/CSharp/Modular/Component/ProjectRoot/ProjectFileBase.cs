using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Component.ProjectRoot;

public class ProjectFileBase : ProjectFile
{
    public ProjectFileBase(CSharpProjectBase projectPackage)
        : base(projectPackage)
    {
        DoNotOverwriteIfAlreadyExists = true;

        var generator = projectPackage.ParentPackage.ArtefactGenerationTarget.Generator;
        var cSharpAppTarget = generator.Target.Parent;

        _predefinedCode.Add($"<Project Sdk=\"Microsoft.NET.Sdk\">");
        _predefinedCode.Add(string.Empty);
        _predefinedCode.Add($"  <PropertyGroup>");
        _predefinedCode.Add($"    <TargetFramework>netstandard2.0</TargetFramework>");
        _predefinedCode.Add($"    <LangVersion>latest</LangVersion>");
        _predefinedCode.Add($"  </PropertyGroup>");
        _predefinedCode.Add(string.Empty);
        _predefinedCode.Add($"  <ItemGroup>");
        _predefinedCode.Add($"    <PackageReference Include=\"System.Data.SqlClient\" Version=\"4.4.0\" />");
        _predefinedCode.Add($"  </ItemGroup>");
        _predefinedCode.Add(string.Empty);
        _predefinedCode.Add($"  <ItemGroup>");
        _predefinedCode.Add($"    <ProjectReference Include=\"{FileHelper.GetRelativePath(projectPackage.FullPath + "\\", cSharpAppTarget.GetOrmLibProjectFilePathDb())}\" />");
        _predefinedCode.Add($"    <ProjectReference Include=\"{FileHelper.GetRelativePath(projectPackage.FullPath + "\\", cSharpAppTarget.GetOrmLibProjectFilePathDbConcrete())}\" />");
        _predefinedCode.Add($"    <ProjectReference Include=\"{FileHelper.GetRelativePath(projectPackage.FullPath + "\\", cSharpAppTarget.GetOrmLibProjectFilePathUtil())}\" />");
        _predefinedCode.Add($"  </ItemGroup>");
        _predefinedCode.Add(string.Empty);
        _predefinedCode.Add($"</Project>");
    }

    public new CSharpProjectBase Package { get { return (CSharpProjectBase)base.Package; } }
}
