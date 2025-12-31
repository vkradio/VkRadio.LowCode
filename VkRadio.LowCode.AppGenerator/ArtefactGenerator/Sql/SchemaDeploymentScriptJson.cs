using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Логическое описание скрипта SQL для создания БД
    /// </summary>
    public abstract class SchemaDeploymentScriptJson
    {
        protected DBSchemaMetaModelJson _dbSchemaMetaModel;
        protected Dictionary<string, TableJson> _tables = new Dictionary<string,TableJson>();
        protected TableJson[] _tablesSorted;
        protected List<ForeignKeyConstraint> _fkConstraints = new List<ForeignKeyConstraint>();
        protected List<PredefinedInsertJson> _predefinedInserts = new List<PredefinedInsertJson>();
        protected string _quoteSymbol = string.Empty;

        protected virtual void SortTables()
        {
            List<TableJson> tablesSorted = new List<TableJson>(_tables.Values);
            tablesSorted.Sort((t1, t2) => string.Compare(t1.Name, t2.Name));
            _tablesSorted = tablesSorted.ToArray();
        }

        public SchemaDeploymentScriptJson(DBSchemaMetaModelJson in_dbSchemaMetaModel) { _dbSchemaMetaModel = in_dbSchemaMetaModel; }

        /// <summary>
        /// Метамодель схемы БД
        /// </summary>
        public DBSchemaMetaModelJson DBSchemaMetaModel { get { return _dbSchemaMetaModel; } }
        /// <summary>
        /// Логические описания создаваемых таблиц
        /// </summary>
        public IDictionary<string, TableJson> Tables { get { return _tables; } }
        /// <summary>
        /// Ограничения по внешним ключам.
        /// </summary>
        public List<ForeignKeyConstraint> FKConstraints { get { return _fkConstraints; } }
        /// <summary>
        /// Логические описания вставляемых в таблицы предопределенных значений
        /// </summary>
        public List<PredefinedInsertJson> PredefinedInserts { get { return _predefinedInserts; } }
        public string QuoteSymbol { get { return _quoteSymbol; } }

        /// <summary>
        /// Генерирование строк сприпта
        /// </summary>
        /// <returns>Массив строк</returns>
        public abstract string[] Generate();

        public IList<TableJson> TablesSorted
        {
            get
            {
                if (_tablesSorted == null)
                    SortTables();
                return _tablesSorted;
            }
        }
    };
}
