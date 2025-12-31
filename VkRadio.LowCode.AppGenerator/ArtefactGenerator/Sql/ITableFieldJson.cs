namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Интерфейс поля таблицы
    /// </summary>
    public interface ITableFieldJson: ITextDefinition
    {
        /// <summary>
        /// Наименование поля
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Допустимы ли значения NULL в поле
        /// </summary>
        bool Nullable { get; }
        /// <summary>
        /// Тип SQL (строкое представление, характерное для конкретного диалекта SQL)
        /// </summary>
        string SqlType { get; }
        /// <summary>
        /// Таблица, к которой относится поле
        /// </summary>
        TableJson Table { get; }
        /// <summary>
        /// Соответствие между полем таблицы и свойством ТОД (может отсутствовать,
        /// например, для суррогатных ключей)
        /// </summary>
        PropertyCorrespondenceJson DOTPropertyCorrespondence { get; }
        /// <summary>
        /// Являются ли значения поля уникальными в пределах таблицы
        /// </summary>
        bool Unique { get; }
    };
}
