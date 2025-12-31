using MetaModel.Names;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Поле внешнего ключа в таблице
    /// </summary>
    public abstract class ForeignKeyFieldJson : ITableFieldJson
    {
        protected abstract string CreateDefaultValue(SRefObject in_srefObject);

        public string QuoteSymbol { get; protected set; }
        /// <summary>
        /// Наименование поля
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// Допустимы ли значения NULL
        /// </summary>
        public bool Nullable { get; protected set; }
        /// <summary>
        /// Тип SQL (строка)
        /// </summary>
        public string SqlType { get; protected set; }
        /// <summary>
        /// Таблица, в состав которой входит ВК
        /// </summary>
        public TableJson Table { get; protected set; }
        /// <summary>
        /// Соответствие между ВК и определением свойства ТОД
        /// </summary>
        public PropertyCorrespondenceJson DOTPropertyCorrespondence { get; protected set; }
        /// <summary>
        /// Значение по умолчанию
        /// </summary>
        public string DefaultValue { get; protected set; }
        /// <summary>
        /// Являются ли значения ВК уникальными в пределах таблицы
        /// </summary>
        public bool Unique { get; protected set; }

        /// <summary>
        /// Конструктор поля внешнего ключа, берущий за основу определение свойства ТОД
        /// </summary>
        /// <param name="in_table">Таблица, к которой принадлежит свойство</param>
        /// <param name="in_tableAndDOTCorrespondence">Соответствие между таблицей и ТОД</param>
        /// <param name="in_propertyDefinition">Свойство ТОД</param>
        public ForeignKeyFieldJson(TableAndDOTCorrespondenceJson in_tableAndDOTCorrespondence, PropertyDefinition in_propertyDefinition)
        {
            Table = in_tableAndDOTCorrespondence.Table;
            Name = NameHelper.NameToUnderscoreSeparatedName(in_propertyDefinition.Names[HumanLanguageEnum.En]) + "_id";
            Nullable = in_propertyDefinition.FunctionalType.Nullable;
            Unique = in_propertyDefinition.FunctionalType.Unique;
            DOTPropertyCorrespondence = new PropertyCorrespondenceJson() { PropertyDefinition = in_propertyDefinition, TableAndDOTCorrespondence = in_tableAndDOTCorrespondence, TableField = this };
        }
        public void Init()
        {
            if (DOTPropertyCorrespondence.PropertyDefinition.DefaultValue != null)
            {
                SRefObject value = (SRefObject)DOTPropertyCorrespondence.PropertyDefinition.DefaultValue;
                DefaultValue = CreateDefaultValue(value);
            }
        }

        /// <summary>
        /// Генерирование текстового представления объявления поля ВК на SQL
        /// </summary>
        /// <returns>Объявление поля ВК на SQL</returns>
        public virtual string[] GenerateText()
        {
            string result = $"{QuoteSymbol}{Name}{QuoteSymbol} {SqlType} {(Nullable ? "null" : "not null")}";
            if (Table.SchemaDeploymentScript.DBSchemaMetaModel.GenerateConstraintsInline)
            {
                if (Unique)
                    result += " " + DBSchemaHelper.C_KEYWORD_UNIQUE;
                if (DefaultValue != null)
                    result += $" {DBSchemaHelper.C_KEYWORD_DEFAULT} {DefaultValue}";
            }
            // TODO: Необходимо также проставлять ссылку references, но, поскольку трудно сразу предсказать
            // зависимости между таблицами, целесообразно отложить создание constraint'ов на будущее
            // и сделать это в виде создания отдельных ключей, который будут прописаны после создания
            // всех таблиц.
            return new string[1] { result };
        }
    };
}
