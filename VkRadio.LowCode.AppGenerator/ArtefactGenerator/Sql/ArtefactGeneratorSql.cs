using System;
using System.IO;
using System.Text;

using ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Генератор пакета артефактов - скрипта SQL создания БД
    /// </summary>
    public class ArtefactGeneratorSql : ArtefactGenerator
    {
        /// <summary>
        /// Метамодель БД
        /// </summary>
        public DBSchemaMetaModel DBSchemaMetaModel { get; private set; }
        /// <summary>
        /// Параметры БД, используемые при разработке приложения
        /// </summary>
        public DbParams? DevelopmentDbParams { get; private set; }

        public ArtefactGeneratorSql(ArtefactGenerationTarget target) : base(target) { }

        /// <summary>
        /// Generate SQL script for DB creation and initial data setup
        /// </summary>
        public override string Generate()
        {
            const string c_sigleFileName = "dbschema.sql";

            // Создание модели скрипта SQL на основе метамодели.
            switch (Target)
            {
                //case TargetMySql targetMySql:
                //    DBSchemaMetaModel = new MySqlDBSchemaMetaModel(MetaModel, this);
                //    break;
                case TargetSql targetMsSql:
                    DBSchemaMetaModel = new MsSqlDBSchemaMetaModel(Target.Project.MetaModel, this);
                    break;
                //case TargetSQLite targetSqlite:
                //    DBSchemaMetaModel = new SQLiteDBSchemaMetaModel(MetaModel, this);
                //    break;
                default:
                    throw new ApplicationException($"Unsupported SQL target type: {Target.GetType().Name}.");
            }
            DBSchemaMetaModel.Init();

            // Генерирование артефактов.
            string[] scriptStrings = DBSchemaMetaModel.SchemaDeploymentScript.Generate();

            if (!Directory.Exists(Target.OutputPath))
                Directory.CreateDirectory(Target.OutputPath);

            File.WriteAllLines(Path.Combine(Target.OutputPath, c_sigleFileName), scriptStrings, Encoding.UTF8);

            return null;
        }
    };
}
