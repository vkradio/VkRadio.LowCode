using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.DotNetVersions;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component.ProjectRoot;

public class ProjectFileBaseSdkStyleFormat : ProjectFile
{
    public ProjectFileBaseSdkStyleFormat(CSharpProjectBase projectPackage)
        : base(projectPackage)
    {
        _lastLineWNewLine = true;

        var generator = (ArtefactGeneratorCSharpClassic)projectPackage.ParentPackage.ArtefactGenerationTarget.ArtefactGenerator;

        _predefinedCode.Add("<Project Sdk=\"Microsoft.NET.Sdk.WindowsDesktop\">");
        _predefinedCode.Add(string.Empty);
        _predefinedCode.Add("  <PropertyGroup>");
        _predefinedCode.Add($"    <TargetFramework>{TargetFramework.VersionToTfm(generator.DotNetVersion)}</TargetFramework>");
        _predefinedCode.Add("    <UseWindowsForms>true</UseWindowsForms>");
        _predefinedCode.Add("    <LangVersion>latest</LangVersion>");
        _predefinedCode.Add("    <Nullable>enable</Nullable>");
        _predefinedCode.Add("    <CLSCompliant>false</CLSCompliant>");
        _predefinedCode.Add("  </PropertyGroup>");
        _predefinedCode.Add(string.Empty);
        _predefinedCode.Add("  <ItemGroup>");
        _predefinedCode.AddRange(generator.LibsParams.GenerateProjectReferenceLines(projectPackage.FullPath));
        _predefinedCode.Add("  </ItemGroup>");
        _predefinedCode.Add(string.Empty);

        var classNames = Package
            .GuiPackage
            .ElementsPackage
            .MainComponents
            .Values
            .Select(x => x.MainClass.Name.Substring(3))
            .OrderBy(x => x)
            .ToList();

        #region Mark User Controls
        _predefinedCode.Add("  <ItemGroup>");
        _predefinedCode.AddRange(classNames
            .Select(x => new string[]
            {
                $"    <Compile Update=\"Gui\\Elements\\DOP{x}.cs\">",
                "      <SubType>UserControl</SubType>",
                "    </Compile>",
                $"    <Compile Update=\"Gui\\Lists\\DOL{x}.cs\">",
                "      <SubType>UserControl</SubType>",
                "    </Compile>"
            })
            .SelectMany(x => x)
        );
        _predefinedCode.Add("  </ItemGroup>");
        _predefinedCode.Add(string.Empty);
        #endregion

        #region Mark .resx files as Designer files
        _predefinedCode.Add("  <ItemGroup>");
        _predefinedCode.AddRange(classNames
            .Select(x => new string[]
            {
                $"    <EmbeddedResource Update=\"Gui\\Elements\\DOP{x}.resx\">",
                "      <SubType>Designer</SubType>",
                "    </EmbeddedResource>"
            })
            .SelectMany(x => x)
        );
        _predefinedCode.Add("  </ItemGroup>");
        _predefinedCode.Add(string.Empty);
        #endregion

        _predefinedCode.Add("</Project>");
    }

    public new CSharpProjectBase Package => (CSharpProjectBase)base.Package;
}
