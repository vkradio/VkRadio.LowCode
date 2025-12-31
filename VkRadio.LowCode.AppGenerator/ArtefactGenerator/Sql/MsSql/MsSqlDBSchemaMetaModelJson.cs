using System;

using MetaModel.DOTDefinition;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql
{
    public class MsSqlDBSchemaMetaModelJson: DBSchemaMetaModelJson
    {
        protected override TableJson CreateTable(DOTDefinition in_dotDefinition)
        {
            TableJson tableDef = new MsSqlTableJson(GenerateTableName(in_dotDefinition), _schemaDeploymentScript);
            tableDef.PrimaryKey = new MsSqlPKSingleJson() { Table = tableDef };
            return tableDef;
        }
        protected override ValueFieldJson CreateTableFieldValue(TableAndDOTCorrespondenceJson in_correspondense, PropertyDefinition in_propertyDefinition)
        {
            ValueFieldJson vf = new MsSqlValueFieldJson(in_correspondense, in_propertyDefinition);
            vf.Init();
            return vf;
        }
        protected override ForeignKeyFieldJson CreateForeignKeyField(TableAndDOTCorrespondenceJson in_correspondense, PropertyDefinition in_propertyDefinition)
        {
            ForeignKeyFieldJson fk = new MsSqlForeignKeyFieldJson(in_correspondense, in_propertyDefinition);
            fk.Init();
            return fk;
        }
        protected override PredefinedInsertJson CreatePredefinedInsert() { return new MsSqlPredefinedInsertJson(); }
        protected override FieldValueKeyJson CreateFieldValueKey(PredefinedInsertJson in_predefinedInsert, ITableFieldJson in_field, Guid in_value) { return new MsSqlFieldValueKeyJson(in_predefinedInsert, in_field, in_value); }
        protected override string GetValueStringForRefId(SRefObject in_value) { return DBSchemaHelper.GuidToMsSqlValueString(in_value.Key); }
        protected override string GetValueStringForUniqueCode(Guid in_value) { return DBSchemaHelper.GuidToMsSqlValueString(in_value); }
        protected override string GetDefaultStringRepForUniqueCodeGenerator() { throw new ApplicationException("GUID generator function not implemented/supported for target MsSQL."); }
        //protected override string GetValueStringForString(string in_value) { return "N'" + in_value.Replace("'", "''").Replace("\n", "' + char(10) + N'") + "'"; } // CR = char(13), LF = char(10)
        protected override SchemaDeploymentScriptJson CreateSchemaDeploymentScript() { return new MsSqlSchemaDeploymentScriptJson(this); }

        public MsSqlDBSchemaMetaModelJson(MetaModel.MetaModel in_metaModel, ArtefactGeneratorSqlJson in_artefactGeneratorSql)
            : base(in_metaModel, in_artefactGeneratorSql)
        {
            _supportsForeignKeyConstraints = true;
            GenerateConstraintsInline = false;
        }

        /// <summary>
        /// Получение строкового литерала для строкового значения свойства ТОД или ПОД
        /// </summary>
        /// <param name="in_value">Строковое значение свойства ТОД или ПОД</param>
        /// <returns>Адаптированный к диалекту SQL строковый литерал</returns>
        public override string GetValueStringForString(string in_value) { return "N'" + in_value.Replace("'", "''").Replace("\n", "' + char(10) + N'") + "'"; } // CR = char(13), LF = char(10)
    };
}
