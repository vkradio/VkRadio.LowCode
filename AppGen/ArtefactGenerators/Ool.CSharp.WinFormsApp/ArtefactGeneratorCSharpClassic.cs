using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.DotNetVersions;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Libs;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;
using VkRadio.LowCode.AppGen.Domain;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp;

/// <summary>
/// Generator of artefact package &quot;C# source code&quot;
/// </summary>
public class ArtefactGeneratorCSharpClassic : ArtefactGenerator
{
    private readonly Version c_defaultDotNetVersion = new Version("4.8");

    private readonly NaturalLanguageEnum _uiLanguage;

    private NaturalLanguageEnum _preferNaturalLanguageForComments = NaturalLanguageEnum.En;

    public ArtefactGeneratorCSharpClassic(ArtefactTypeEnum type, DomainModel domainModel, Target target, NaturalLanguageEnum uiLanguage)
        : base(type, domainModel, target)
    {
        _uiLanguage = uiLanguage;
    }

    public Version DotNetVersion { get; private set; } = default!;

    public LibsParams LibsParams { get; private set; } = default!;

    public bool PutSimilarArtefactsToSingleFile { get; private set; }

    public bool GenerateDemoApp { get; private set; }

    public bool DoNoOverwriteAppIfExists { get; private set; }

    public bool SupportHiDpi { get; private set; }

    public bool ProjectFileInModernSdkStyle { get; private set; } = true;

    public bool SupportNullableReferenceTypes { get; private set; }

    public override string? Generate()
    {
        // Create model of package of C# source code, based on database schema model.
        var solution = new CSharpSolution(this, ((ArtefactGeneratorSql)MsSqlTarget.ArtefactGenerator).DBSchemaMetaModel, _preferNaturalLanguageForComments, _uiLanguage, SupportHiDpi);
        solution.Init();

        // Generate artefacts.
        solution.GeneratePackage();

        return null;
    }

    public override void InitFromTargetXElement(XElement xelTarget)
    {
        #region Initialize .NET version
        var xelDotNetVersion = xelTarget.Element("DotNetVersion");

        if (xelDotNetVersion is not null)
        {
            var xel = xelDotNetVersion.Element("Number")
                ?? throw new GeneratorException("Element DotNetVersion of C# Target has no inner element Number");

            var versionStr = (xel.Value ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(versionStr))
            {
                throw new GeneratorException("In C# Target an element DotNetVersion.Number has no value");
            }

            DotNetVersion = new Version(versionStr);
        }
        else
        {
            DotNetVersion = c_defaultDotNetVersion;
        }

        if (DotNetVersion.IsModern() ||
            (DotNetVersion.Major >= 4 && DotNetVersion.Minor >= 7))
        {
            SupportHiDpi = true;
        }
        #endregion

        LibsParams = LibsParams.Deserialize(xelTarget, this);

        #region Demo App
        var xelDemoApp = xelTarget.Element("DemoApp");

        if (xelDemoApp is not null)
        {
            var generateIt = xelDemoApp.Element("Generate");

            if (generateIt is not null &&
                bool.TryParse(generateIt.Value, out var parsedGenerateIt))
            {
                GenerateDemoApp = parsedGenerateIt;
            }

            var doNotOverwrite = xelDemoApp.Element("DoNotOverwriteIfExists");

            if (doNotOverwrite is not null &&
                bool.TryParse(doNotOverwrite.Value, out var parsedDoNotOverwrite))
            {
                DoNoOverwriteAppIfExists = parsedDoNotOverwrite;
            }
        }
        #endregion

        var xelCommentLanguage = xelTarget.Element("PreferNaturalLanguageForComments");

        if (xelCommentLanguage is not null &&
            Enum.TryParse<NaturalLanguageEnum>(xelCommentLanguage.Value ?? string.Empty, true, out var parsedLang))
        {
            _preferNaturalLanguageForComments = parsedLang;
        }

        var xelPutSimilarArtefactsToSingleFile = xelTarget.Element("PutSimilarArtefactsToSingleFile");

        if (xelPutSimilarArtefactsToSingleFile is not null && bool.TryParse(xelPutSimilarArtefactsToSingleFile.Value ?? string.Empty, out var parsedSingleFile))
        {
            PutSimilarArtefactsToSingleFile = parsedSingleFile;
        }

        var xelProjectFileStyle = xelTarget.Element("ProjectFileInModernSdkStyle");

        if (xelProjectFileStyle is not null &&
            bool.TryParse(xelProjectFileStyle.Value, out var projectFileInModernSdkStyle))
        {
            ProjectFileInModernSdkStyle = projectFileInModernSdkStyle;
        }

        var xelNullableRefTypes = xelTarget.Element("SupportNullableReferenceTypes");

        if (xelNullableRefTypes is not null &&
            bool.TryParse(xelNullableRefTypes.Value, out var supportNullableRefTypes))
        {
            if (!ProjectFileInModernSdkStyle && supportNullableRefTypes)
            {
                throw new GeneratorException("<SupportNullableReferenceTypes>true</SupportNullableReferenceTypes> is not a valid value for non-SDK-style project files; and those should be enabled by <ProjectFileInModernSdkStyle> element.");
            }

            SupportNullableReferenceTypes = supportNullableRefTypes;
        }
    }

    public Target MsSqlTarget => Target
        .DependsOn
        .FirstOrDefault(x => x.Type == ArtefactTypeEnum.MsSql) ?? throw new GeneratorException("No MS SQL target in this target dependencies");
}
