namespace MetaModel.PredefinedDO
{
    /// <summary>
    /// Значение свойства предопределенного объекта
    /// </summary>
    public interface IPropertyValue
    {
        /// <summary>
        /// Определение свойства
        /// </summary>
        PropertyDefinition.PropertyDefinition Definition { get; set; }

        /// <summary>
        /// Значение, приведенное к абстрактному типу object
        /// </summary>
        object ValueObject { get; set; }
    };
}
