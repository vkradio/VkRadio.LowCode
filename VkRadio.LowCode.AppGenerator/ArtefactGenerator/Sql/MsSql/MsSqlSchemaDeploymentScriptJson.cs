using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql
{
    public class MsSqlSchemaDeploymentScriptJson: SchemaDeploymentScriptJson
    {
        public MsSqlSchemaDeploymentScriptJson(DBSchemaMetaModelJson in_dbSchemaMetaModel)
            : base(in_dbSchemaMetaModel)
            => _quoteSymbol = "\"";

        /// <summary>
        /// Генерирование строк сприпта
        /// </summary>
        /// <returns>Массив строк</returns>
        public override string[] Generate()
        {
            var target = _dbSchemaMetaModel.ArtefactGeneratorSql.Target;
            var project = target.Project;
            var generator = _dbSchemaMetaModel.ArtefactGeneratorSql;

            List<string> result = new List<string>();

            string dbName = (generator.DevelopmentDbParams.HasValue && !string.IsNullOrEmpty(generator.DevelopmentDbParams.Value.DbName)) ?
                generator.DevelopmentDbParams.Value.DbName :
                "INSERT_ACTUAL_DB_NAME_HERE";

            result.Add($"create database \"{dbName}\";");
            result.Add("go");
            result.Add($"use \"{dbName}\";");
            result.Add("go");
            result.Add(string.Empty);

            foreach (TableJson table in TablesSorted)
            {
                result.AddRange(table.GenerateText());
                result.Add("go");
                result.AddRange(((MsSqlTableJson)table).GenerateConstraints());
                result.Add(string.Empty);
            }

            foreach (PredefinedInsertJson insert in _predefinedInserts)
                result.AddRange(insert.GenerateText());

            if (_predefinedInserts.Count != 0 && _fkConstraints.Count != 0)
                result.Add(string.Empty);
            foreach (ForeignKeyConstraint fkConstraint in _fkConstraints)
                result.AddRange(fkConstraint.GenerateText());

            return result.ToArray();
        }
    };
}
