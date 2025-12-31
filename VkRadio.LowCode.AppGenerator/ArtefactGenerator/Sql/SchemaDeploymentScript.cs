using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Логическое описание скрипта SQL для создания БД
    /// </summary>
    public abstract class SchemaDeploymentScript
    {
        protected DBSchemaMetaModel _dbSchemaMetaModel;
        protected Dictionary<string, Table> _tables = new();
        protected Table[] _tablesSorted;
        protected List<ForeignKeyConstraint> _fkConstraints = new();
        protected List<PredefinedInsert> _predefinedInserts = new();
        protected string _quoteSymbol = string.Empty;

        protected virtual void SortTables()
        {
            var tablesSorted = new List<Table>(_tables.Values);
            tablesSorted.Sort((t1, t2) => string.Compare(t1.Name, t2.Name));
            _tablesSorted = tablesSorted.ToArray();
        }

        public SchemaDeploymentScript(DBSchemaMetaModel in_dbSchemaMetaModel) => _dbSchemaMetaModel = in_dbSchemaMetaModel;

        /// <summary>
        /// Метамодель схемы БД
        /// </summary>
        public DBSchemaMetaModel DBSchemaMetaModel { get { return _dbSchemaMetaModel; } }
        /// <summary>
        /// Логические описания создаваемых таблиц
        /// </summary>
        public IDictionary<string, Table> Tables => _tables;
        /// <summary>
        /// Ограничения по внешним ключам.
        /// </summary>
        public List<ForeignKeyConstraint> FKConstraints => _fkConstraints;
        /// <summary>
        /// Логические описания вставляемых в таблицы предопределенных значений
        /// </summary>
        public List<PredefinedInsert> PredefinedInserts => _predefinedInserts;
        public string QuoteSymbol => _quoteSymbol;

        /// <summary>
        /// Генерирование строк сприпта
        /// </summary>
        /// <returns>Массив строк</returns>
        public abstract string[] Generate();

        public IList<Table> TablesSorted
        {
            get
            {
                if (_tablesSorted == null)
                    SortTables();
                return _tablesSorted;
            }
        }
    }
}
