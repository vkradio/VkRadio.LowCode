using Ardalis.GuardClauses;
using System.Text;
using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;

public class ArtefactGeneratorSql : ArtefactGenerator
{
    private readonly Func<DomainModel, ArtefactGeneratorSql, DBSchemaDomainModel> _dbSchemaDomainModelConstructor;

    public ArtefactGeneratorSql(
        Func<DomainModel, ArtefactGeneratorSql, DBSchemaDomainModel> dbSchemaDomainModelConstructor,
        ArtefactTypeEnum type,
        DomainModel domainModel,
        Target target
    ) : base(type, domainModel, target)
    {
        _dbSchemaDomainModelConstructor = Guard.Against.Null(dbSchemaDomainModelConstructor, nameof(dbSchemaDomainModelConstructor));
    }

    /// <summary>
    /// DB Domain Model
    /// </summary>
    public DBSchemaDomainModel DBSchemaMetaModel { get; private set; } = default!;

    /// <summary>
    /// DB parameter used during development
    /// </summary>
    public DbParams? DevelopmentDbParams { get; private set; }

    /// <summary>
    /// Generate DB schema and SQL artefacts
    /// </summary>
    public override string? Generate()
    {
        const string c_sigleFileName = "dbschema.sql";

        DBSchemaMetaModel = _dbSchemaDomainModelConstructor(DomainModel, this);
        DBSchemaMetaModel.Init();

        // Generate artefacts
        var scriptStrings = DBSchemaMetaModel.SchemaDeploymentScript.Generate();

        if (string.IsNullOrWhiteSpace(Target.OutputPath))
        {
            throw new InvalidOperationException($"{nameof(Target.OutputPath)} is empty");
        }

        if (!Directory.Exists(Target.OutputPath))
        {
            Directory.CreateDirectory(Target.OutputPath);
        }

        File.WriteAllLines(Path.Combine(Target.OutputPath, c_sigleFileName), scriptStrings, Encoding.UTF8);

        return null;
    }

    public override void InitFromTargetXElement(XElement xelTarget)
    {
        var xel = xelTarget.Element("DevelopmentDbParams");

        if (xel is not null)
        {
            DevelopmentDbParams = DbParams.ReadFromXElement(xel);
        }
    }
}
