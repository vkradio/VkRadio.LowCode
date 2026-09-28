using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
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
    const string c_defaultDotNetVersion = "4.0";
    const string c_defaultDotNetProfile = "Client"; // Profile is used only if there is no <DotNetVersion> element.
                                                    // If it exists, but does not contain an inner <Profile> element,
                                                    // then profile is set to null
    string _dotNetVersion;
    string? _dotNetProfile;
    NaturalLanguageEnum _preferNaturalLanguageForComments = NaturalLanguageEnum.En;

    public ArtefactGeneratorCSharpClassic(ArtefactTypeEnum type, DomainModel domainModel, Target target)
        : base(type, domainModel, target)
    {
    }

    public string DotNetFramework => _dotNetVersion;

    public bool IsDependantOnSQLite => false;

    public string? SQLiteProjectFullPath => null;

    public string OrmLibProjectDir { get; private set; }

    /// <summary>
    /// Ormlib project name without any .csproj extension and path (as displayed in VS solution tree)
    /// </summary>
    public string OrmLibProjectName { get; private set; }

    public bool PutSimilarArtefactsToSingleFile { get; private set; }

    public override string? Generate()
    {
        // Create model of package of C# source code, based on database schema model.
        var solution = new CSharpSolution(this, ((ArtefactGeneratorSql)MsSqlTarget.ArtefactGenerator).DBSchemaMetaModel, _preferNaturalLanguageForComments);
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
                ?? throw new GeneratorException("Element DotNetVersion of the C# Target has no inner element Number");

            _dotNetVersion = (xel.Value ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(_dotNetVersion))
            {
                throw new GeneratorException("In C# Target an element DotNetVersion.Number has no value");
            }

            xel = xelDotNetVersion.Element("Profile");

            if (xel is not null && (xel.Value ?? string.Empty).Trim() != string.Empty)
            {
                _dotNetProfile = xel.Value!.Trim();
            }
        }
        else
        {
            _dotNetVersion = c_defaultDotNetVersion;
            _dotNetProfile = c_defaultDotNetProfile;
        }
        #endregion

        #region Initialize path to ormlib
        OrmLibProjectName = "orm_" + _dotNetVersion;

        if (!string.IsNullOrEmpty(_dotNetProfile))
        {
            OrmLibProjectName += "_" + _dotNetProfile;
        }

        var ormProjectFileName = OrmLibProjectName + ".csproj";

        if (xelTarget.Element("OrmLibProjectDir") is null)
        {
            throw new GeneratorException("C# Target has no <OrmLibProjectDir> value");
        }

        OrmLibProjectDir = Path.Combine(_target.Project.ProjectRootPath, xelTarget.Element("OrmLibProjectDir")!.Value, ormProjectFileName);

        if (!File.Exists(OrmLibProjectDir))
        {
            throw new GeneratorException($"File \"{OrmLibProjectDir}\" not exists");
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
    }

    public Target MsSqlTarget => Target
        .DependsOn
        .FirstOrDefault(x => x.Type == ArtefactTypeEnum.MsSql) ?? throw new GeneratorException("No MS SQL target in this target dependencies");

    public string GetOrmLibProjectFilePath() => Path.Combine(OrmLibProjectDir, OrmLibProjectName + ".csproj");


    public static readonly string C_ORMLIB_PROJECT_GUID_STRING = "BC2581BB-BC55-4E13-9AED-69CE482E092D";
}
