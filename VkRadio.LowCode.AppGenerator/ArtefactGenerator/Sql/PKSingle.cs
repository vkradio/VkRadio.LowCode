namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Первичный ключ, состоящий из одного поля
    /// </summary>
    public abstract class PKSingle : PrimaryKey, ITableField
    {
        protected string _name;
        protected string _sqlType;
        protected string _quoteSymbol;

        /// <summary>
        /// Имя поля таблицы
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// Допустимы ли значения NULL (для ПК - никогда недопустимы)
        /// </summary>
        public bool Nullable { get { return false; } }
        /// <summary>
        /// Тип SQL (строка)
        /// </summary>
        public string SqlType { get { return _sqlType; } }
        /// <summary>
        /// Соответствующее полю таблицы свойство типа объектов данных (для ПК неприменимо и всегда равно null)
        /// </summary>
        public PropertyCorrespondence DOTPropertyCorrespondence { get { return null; } }
        /// <summary>
        /// Признак уникальности значений - для ПК всегда true по определению
        /// </summary>
        public bool Unique { get { return true; } }

        /// <summary>
        /// Конструктор для единичного ПК по умолчанию
        /// </summary>
        public PKSingle()
        {
            _name = "id";
        }

        /// <summary>
        /// Генерирование текста ПК
        /// </summary>
        /// <returns>строка объявления поля ПК в таблице (без запятой на конце)</returns>
        public virtual string[] GenerateText()
        {
            return new string[1] { string.Format("{0}{1}{2} {3} not null primary key", _quoteSymbol, _name, _quoteSymbol, _sqlType) };
        }
    };
}
