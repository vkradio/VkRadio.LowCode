namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Абстрактное соответствие между таблицей и ее прообразом в основной метамодели
    /// </summary>
    public abstract class TableAndSourceCorrespondence
    {
        protected Table _table;
        protected DBSchemaMetaModel _dbSchemaMetaModel;

        /// <summary>
        /// Таблица, для которой установлено соответствие
        /// </summary>
        public Table Table { get { return _table; } set { _table = value; } }
        /// <summary>
        /// Метамодель схемы БД
        /// </summary>
        public DBSchemaMetaModel DBSchemaMetaModel { get { return _dbSchemaMetaModel; } set { _dbSchemaMetaModel = value; } }
    };
}
