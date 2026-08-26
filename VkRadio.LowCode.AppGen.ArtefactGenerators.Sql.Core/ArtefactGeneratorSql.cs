using System.Text;
using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;

public class ArtefactGeneratorSql : ArtefactGenerator
{
    private DBSchemaDomainModel _dbSchemaMetaModel;
    private DbParams? _developmentDbParams;
    private readonly Func<DomainModel, ArtefactGeneratorSql, DBSchemaDomainModel> _dbSchemaDomainModelConstructor;

    public ArtefactGeneratorSql(
        Func<DomainModel, ArtefactGeneratorSql, DBSchemaDomainModel> dbSchemaDomainModelConstructor,
        ArtefactTypeEnum type,
        DomainModel domainModel,
        Target target
    ) : base(type, domainModel, target)
    {
        _dbSchemaDomainModelConstructor = dbSchemaDomainModelConstructor;
    }

    /// <summary>
    /// DB Domain Model
    /// </summary>
    public DBSchemaDomainModel DBSchemaMetaModel { get { return _dbSchemaMetaModel; } }

    /// <summary>
    /// DB parameter used during development
    /// </summary>
    public DbParams? DevelopmentDbParams { get { return _developmentDbParams; } }

    /// <summary>
    /// Generate DB schema and SQL artefacts
    /// </summary>
    public override string? Generate()
    {
        const string c_sigleFileName = "dbschema.sql";

        _dbSchemaMetaModel = _dbSchemaDomainModelConstructor(_domainModel, this);
        _dbSchemaMetaModel.Init();

        // Generate artefacts
        var scriptStrings = _dbSchemaMetaModel.SchemaDeploymentScript.Generate();

        if (string.IsNullOrWhiteSpace(_target.OutputPath))
        {
            throw new InvalidOperationException($"{nameof(_target.OutputPath)} is empty");
        }

        if (!Directory.Exists(_target.OutputPath))
        {
            Directory.CreateDirectory(_target.OutputPath);
        }

        File.WriteAllLines(Path.Combine(_target.OutputPath, c_sigleFileName), scriptStrings, Encoding.UTF8);

        return null;
    }

    protected override void InitFromTargetXElement(XElement xelTarget)
    {
        var xel = xelTarget.Element("DevelopmentDbParams");

        if (xel is not null)
        {
            _developmentDbParams = DbParams.ReadFromXElement(xel);
        }
    }
}
