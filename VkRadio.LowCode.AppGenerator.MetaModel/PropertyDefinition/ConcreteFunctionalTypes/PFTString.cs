using System.Xml.Linq;

using MetaModel.PredefinedDO;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Строковый функциональный тип свойства (абстрактный класс)
    /// </summary>
    public abstract class PFTString: PropertyFunctionalType
    {
        protected int _defaultMaxLength;
        protected int _defaultMinLength;
        protected int _maxLength;
        protected int _minLength;

        /// <summary>
        /// Конструктор строкового функционального типа свойства
        /// </summary>
        public PFTString()
        {
            _defaultValue   = null;
            _nullable       = true;
            _quantitative   = false;
            _unique         = false;
        }

        protected override void InitExtendedParams(XElement in_xelPropertyDefinition)
        {
            XElement xel = in_xelPropertyDefinition.Element("MaxLength");
            if (xel != null)
                _maxLength = int.Parse(xel.Value);
            xel = in_xelPropertyDefinition.Element("MinLength");
            if (xel != null)
                _minLength = int.Parse(xel.Value);
        }

        /// <summary>
        /// Максимальная длина строки по умолчанию
        /// </summary>
        public int DefaultMaxLength { get { return _defaultMaxLength; } }
        /// <summary>
        /// Минимальная длина строки по умолчанию
        /// </summary>
        public int DefaultMinLength { get { return _defaultMinLength; } }
        /// <summary>
        /// Максимальная длина строки
        /// </summary>
        public int MaxLength { get { return _maxLength; } }
        /// <summary>
        /// Минимальная длина строки
        /// </summary>
        public int MinLength { get { return _minLength; } }

        /// <summary>
        /// Извлечение значения свойства из строки XML
        /// </summary>
        /// <param name="in_xmlString">Строка XML, содержащая извлекаемое значение</param>
        /// <returns>Типизированное значение свойства</returns>
        public override object ParseValueFromXmlString(string in_xmlString) { return in_xmlString; }

        /// <summary>
        /// Создание типизированной заготовки для хранения значения.
        /// </summary>
        /// <returns>Заготовка для значения свойства</returns>
        public override IPropertyValue CreatePropertyValue()
        {
            return new PropertyValue<string>() { Definition = (PropertyDefinition)_propertyDefinition };
        }
    };
}
