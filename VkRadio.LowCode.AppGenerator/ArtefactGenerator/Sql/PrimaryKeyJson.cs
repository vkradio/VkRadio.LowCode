namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    public abstract class PrimaryKeyJson
    {
        protected TableJson _table;

        public TableJson Table { get { return _table; } set { _table = value; } }
    }
}
