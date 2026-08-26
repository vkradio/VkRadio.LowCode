using VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;
using VkRadio.LowCode.AppGen.Domain;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.MsSql;

public class MsSqlDBSchemaMetaModel : DBSchemaDomainModel
{
    protected override Table CreateTable(EntityDefinition entityDefinition)
    {
        var tableDef = new MsSqlTable(GenerateTableName(entityDefinition), _schemaDeploymentScript);
        
        tableDef.PrimaryKey = new MsSqlPKSingle
        {
            Table = tableDef
        };
        
        return tableDef;
    }

    protected override ValueField CreateTableFieldValue(TableAndEntityCorrespondence correspondense, PropertyDefinition propertyDefinition)
    {
        var vf = new MsSqlValueField(correspondense, propertyDefinition);
        vf.Init();
        return vf;
    }

    protected override ForeignKeyField CreateForeignKeyField(TableAndEntityCorrespondence correspondense, PropertyDefinition propertyDefinition)
    {
        var fk = new MsSqlForeignKeyField(correspondense, propertyDefinition);
        fk.Init();
        return fk;
    }

    protected override PredefinedInsert CreatePredefinedInsert() => new MsSqlPredefinedInsert();

    protected override FieldValueKey CreateFieldValueKey(PredefinedInsert predefinedInsert, ITableField field, Guid value) => new MsSqlFieldValueKey(predefinedInsert, field, value);
    
    protected override string GetValueStringForRefId(SRefObject value) => DBSchemaHelper.GuidToMsSqlValueString(value.Key);
    
    protected override string GetValueStringForUniqueCode(Guid value) => DBSchemaHelper.GuidToMsSqlValueString(value);
    
    protected override string GetDefaultStringRepForUniqueCodeGenerator() => throw new ApplicationException("GUID generator function not implemented/supported for target MsSQL.");
    
    //protected override string GetValueStringForString(string value) { return "N'" + value.Replace("'", "''").Replace("\n", "' + char(10) + N'") + "'"; } // CR = char(13), LF = char(10)
    
    protected override SchemaDeploymentScript CreateSchemaDeploymentScript() => new MsSqlSchemaDeploymentScript(this);

    public MsSqlDBSchemaMetaModel(DomainModel metaModel, ArtefactGeneratorSql artefactGeneratorSql)
        : base(metaModel, artefactGeneratorSql)
    {
        _supportsForeignKeyConstraints = true;
        GenerateConstraintsInline = false;
    }

    public override string GetValueStringForString(string in_value) => "N'" + in_value.Replace("'", "''").Replace("\n", "' + char(10) + N'") + "'"; // CR = char(13), LF = char(10)
}
