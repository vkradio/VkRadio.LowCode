namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Абстрактное соответствие между таблицей и ее прообразом в основной метамодели
    /// </summary>
    public abstract class TableAndSourceCorrespondenceJson
    {
        protected TableJson _table;
        protected DBSchemaMetaModelJson _dbSchemaMetaModel;

        /// <summary>
        /// Таблица, для которой установлено соответствие
        /// </summary>
        public TableJson Table { get { return _table; } set { _table = value; } }
        /// <summary>
        /// Метамодель схемы БД
        /// </summary>
        public DBSchemaMetaModelJson DBSchemaMetaModel { get { return _dbSchemaMetaModel; } set { _dbSchemaMetaModel = value; } }
    };
}
