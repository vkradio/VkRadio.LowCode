namespace MetaModel.PredefinedDO
{
    /// <summary>
    /// Значение свойства
    /// </summary>
    /// <typeparam name="T">Системный тип значения</typeparam>
    public class PropertyValue<T>: IPropertyValue
    {
        PropertyDefinition.PropertyDefinition _definition;
        T _value;

        /// <summary>
        /// Определение свойства
        /// </summary>
        public PropertyDefinition.PropertyDefinition Definition { get { return _definition; } set { _definition = value; } }
        /// <summary>
        /// Типизированное значение свойства
        /// </summary>
        public T Value { get { return _value; } }
        /// <summary>
        /// Значение свойства, возвращаемое в виде абстрактного типа object
        /// </summary>
        public object ValueObject { get { return _value; } set { _value = (T)value; } }
    };
}
