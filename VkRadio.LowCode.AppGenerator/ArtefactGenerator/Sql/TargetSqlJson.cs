namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    public class TargetSqlJson : ArtefactGenerationTargetJson
    {
        /// <summary>
        /// Model of database schema (tables, fields, constraints, etc.)
        /// </summary>
        public DBSchemaMetaModelJson DBSchemaModel { get; private set; }

        protected override void CreateGenerator() => Generator = new ArtefactGeneratorSqlJson(this);

        public new ArtefactGeneratorSqlJson Generator { get => (ArtefactGeneratorSqlJson)base.Generator; set => base.Generator = value; }
    };
}
