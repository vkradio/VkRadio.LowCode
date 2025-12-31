namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    public abstract class FieldValueKeyJson: FieldValueJson
    {
        public FieldValueKeyJson(PredefinedInsertJson in_predefinedInsert, ITableFieldJson in_field)
        {
            this.PredefinedInsert = in_predefinedInsert;
            this.Field = in_field;
        }
    };
}
