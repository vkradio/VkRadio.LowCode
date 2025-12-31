namespace MetaModel.PropertyDefinition
{
    /// <summary>
    /// Действие при удаление объекта, на который ссылается ВК
    /// </summary>
    public enum OnDeleteActionEnum
    {
        /// <summary>
        /// Ничего не делать, оставлять &quot;дырку&quot;
        /// </summary>
        Ingnore,
        /// <summary>
        /// Удаление объекта невозможно
        /// </summary>
        CannotDelete,
        /// <summary>
        /// Удалить связанные объекты каскадно
        /// </summary>
        Delete,
        /// <summary>
        /// Сбросить ВК связанных объектов в NULL
        /// </summary>
        ResetToNull,
        /// <summary>
        /// Сбросить ВК связанных объектов в значение по умолчанию
        /// </summary>
        ResetToDefault
    };
}
