using System.Globalization;

using MetaModel.Names;
using MetaModel.PredefinedDO;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - число с фиксированным количеством дробных разрядов
    /// </summary>
    public class PFTDecimal: PropertyFunctionalType
    {
        /// <summary>
        /// Конструктор функционального типа свойства - числа с фиксированным количеством дробных разрядов
        /// </summary>
        public PFTDecimal()
        {
            //_defaultValue   = 0.0m;
            _defaultValue   = null;
            _nullable       = true;
            _quantitative   = true;
            _stringCode     = C_STRING_CODE;
            _unique         = false;

            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "десятичное число");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "decimal number";

        /// <summary>
        /// Извлечение значения свойства из строки XML
        /// </summary>
        /// <param name="in_xmlString">Строка XML, содержащая извлекаемое значение</param>
        /// <returns>Типизированное значение свойства</returns>
        public override object ParseValueFromXmlString(string in_xmlString) { return decimal.Parse(in_xmlString, CultureInfo.InvariantCulture); }

        /// <summary>
        /// Создание типизированной заготовки для хранения значения.
        /// </summary>
        /// <returns>Заготовка для значения свойства</returns>
        public override IPropertyValue CreatePropertyValue()
        {
            return new PropertyValue<decimal?>() { Definition = (PropertyDefinition)_propertyDefinition };
        }
    };
}
