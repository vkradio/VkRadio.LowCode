using System.Text;
using VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.MsSql;
using VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;
using VkRadio.LowCode.AppGenerator.Targets;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql;

/// <summary>
/// Artefact package generator - SQL scriptipt to create a database
/// </summary>
public abstract class ArtefactGeneratorSqlBase : ArtefactGeneratorBase
{
    /// <summary>
    /// Database metamodel
    /// </summary>
    public DBSchemaMetaModel DBSchemaMetaModel { get; private set; }
    /// <summary>
    /// Database parameters used during app development
    /// </summary>
    public DbParams? DevelopmentDbParams { get; private set; }

    public ArtefactGeneratorSqlBase(ArtefactGenerationTarget target) : base(target) { }

    /// <summary>
    /// Generate SQL script for DB creation and initial data setup
    /// </summary>
    public override string? Generate()
    {
        //const string c_sigleFileName = "dbschema.sql";

        //switch (Target)
        //{
        //    //case TargetMySql targetMySql:
        //    //    DBSchemaMetaModel = new MySqlDBSchemaMetaModel(MetaModel, this);
        //    //    break;
        //    case SqlBaseTarget targetMsSql:
        //        DBSchemaMetaModel = new MsSqlDBSchemaMetaModel(Target.Project.MetaModel, this);
        //        break;
        //    //case TargetSQLite targetSqlite:
        //    //    DBSchemaMetaModel = new SQLiteDBSchemaMetaModel(MetaModel, this);
        //    //    break;
        //    default:
        //        throw new ApplicationException($"Unsupported SQL target type: {Target.GetType().Name}.");
        //}
        //DBSchemaMetaModel.Init();

        //// Generate artefacts
        //var scriptStrings = DBSchemaMetaModel.SchemaDeploymentScript.Generate();

        //if (!Directory.Exists(Target.OutputPath))
        //{
        //    Directory.CreateDirectory(Target.OutputPath);
        //}

        //File.WriteAllLines(Path.Combine(Target.OutputPath, c_sigleFileName), scriptStrings, Encoding.UTF8);

        //return null;

        throw new NotImplementedException();
    }
}
