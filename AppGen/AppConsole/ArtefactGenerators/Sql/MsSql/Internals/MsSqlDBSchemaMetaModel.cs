using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql.MsSql;
using VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;
using VkRadio.LowCode.AppGenerator.MetaModel.DOTDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.MsSql.Internals;

public class MsSqlDBSchemaMetaModel : DBSchemaMetaModel
{
    protected override Table CreateTable(DOTDefinition dotDefinition)
    {
        var tableDef = new MsSqlTable(GenerateTableName(dotDefinition), _schemaDeploymentScript);
        
        tableDef.PrimaryKey = new MsSqlPKSingle
        {
            Table = tableDef
        };
        
        return tableDef;
    }

    protected override ValueField CreateTableFieldValue(TableAndDOTCorrespondence correspondense, PropertyDefinition propertyDefinition)
    {
        var vf = new MsSqlValueField(correspondense, propertyDefinition);
        vf.Init();
        return vf;
    }

    protected override ForeignKeyField CreateForeignKeyField(TableAndDOTCorrespondence correspondense, PropertyDefinition propertyDefinition)
    {
        var fk = new MsSqlForeignKeyField(correspondense, propertyDefinition);
        fk.Init();
        return fk;
    }

    protected override PredefinedInsert CreatePredefinedInsert() => new MsSqlPredefinedInsert();

    protected override FieldValueKey CreateFieldValueKey(PredefinedInsert predefinedInsert, ITableField field, Guid value) => new MsSqlFieldValueKey(predefinedInsert, field, value);
    
    protected override string GetValueStringForRefId(SRefObject in_value) => DBSchemaHelper.GuidToMsSqlValueString(in_value.Key);
    
    protected override string GetValueStringForUniqueCode(Guid in_value) => DBSchemaHelper.GuidToMsSqlValueString(in_value);
    
    protected override string GetDefaultStringRepForUniqueCodeGenerator() => throw new ApplicationException("GUID generator function not implemented/supported for target MsSQL.");
    
    //protected override string GetValueStringForString(string in_value) { return "N'" + in_value.Replace("'", "''").Replace("\n", "' + char(10) + N'") + "'"; } // CR = char(13), LF = char(10)
    
    protected override SchemaDeploymentScript CreateSchemaDeploymentScript() => new MsSqlSchemaDeploymentScript(this);

    public MsSqlDBSchemaMetaModel(MetaModel.MetaModel metaModel, ArtefactGeneratorSqlBase artefactGeneratorSql)
        : base(metaModel, artefactGeneratorSql)
    {
        _supportsForeignKeyConstraints = true;
        GenerateConstraintsInline = false;
    }

    public override string GetValueStringForString(string in_value) => "N'" + in_value.Replace("'", "''").Replace("\n", "' + char(10) + N'") + "'"; // CR = char(13), LF = char(10)
}
