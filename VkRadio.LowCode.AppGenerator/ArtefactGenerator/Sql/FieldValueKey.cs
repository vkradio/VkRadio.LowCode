namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    public abstract class FieldValueKey : FieldValue
    {
        public FieldValueKey(PredefinedInsert in_predefinedInsert, ITableField in_field)
        {
            PredefinedInsert = in_predefinedInsert;
            Field = in_field;
        }
    }
}
