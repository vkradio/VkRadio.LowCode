using System;

using MetaModel.DOTDefinition;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql
{
    public class MsSqlDBSchemaMetaModel : DBSchemaMetaModel
    {
        protected override Table CreateTable(DOTDefinition in_dotDefinition)
        {
            var tableDef = new MsSqlTable(GenerateTableName(in_dotDefinition), _schemaDeploymentScript);
            tableDef.PrimaryKey = new MsSqlPKSingle { Table = tableDef };
            return tableDef;
        }
        protected override ValueField CreateTableFieldValue(TableAndDOTCorrespondence in_correspondense, PropertyDefinition in_propertyDefinition)
        {
            var vf = new MsSqlValueField(in_correspondense, in_propertyDefinition);
            vf.Init();
            return vf;
        }
        protected override ForeignKeyField CreateForeignKeyField(TableAndDOTCorrespondence in_correspondense, PropertyDefinition in_propertyDefinition)
        {
            var fk = new MsSqlForeignKeyField(in_correspondense, in_propertyDefinition);
            fk.Init();
            return fk;
        }
        protected override PredefinedInsert CreatePredefinedInsert() => new MsSqlPredefinedInsert();
        protected override FieldValueKey CreateFieldValueKey(PredefinedInsert in_predefinedInsert, ITableField in_field, Guid in_value) { return new MsSqlFieldValueKey(in_predefinedInsert, in_field, in_value); }
        protected override string GetValueStringForRefId(SRefObject in_value) { return DBSchemaHelper.GuidToMsSqlValueString(in_value.Key); }
        protected override string GetValueStringForUniqueCode(Guid in_value) { return DBSchemaHelper.GuidToMsSqlValueString(in_value); }
        protected override string GetDefaultStringRepForUniqueCodeGenerator() { throw new ApplicationException("GUID generator function not implemented/supported for target MsSQL."); }
        //protected override string GetValueStringForString(string in_value) { return "N'" + in_value.Replace("'", "''").Replace("\n", "' + char(10) + N'") + "'"; } // CR = char(13), LF = char(10)
        protected override SchemaDeploymentScript CreateSchemaDeploymentScript() => new MsSqlSchemaDeploymentScript(this);

        public MsSqlDBSchemaMetaModel(MetaModel.MetaModel in_metaModel, ArtefactGeneratorSql in_artefactGeneratorSql)
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
