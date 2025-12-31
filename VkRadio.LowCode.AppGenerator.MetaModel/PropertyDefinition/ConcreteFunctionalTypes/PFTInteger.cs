using MetaModel.Names;
using MetaModel.PredefinedDO;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - целое число
    /// </summary>
    public class PFTInteger: PropertyFunctionalType
    {
        /// <summary>
        /// Конструктор функционального типа свойства - целого числа
        /// </summary>
        public PFTInteger()
        {
            //_defaultValue   = 0;
            _defaultValue   = null;
            _nullable       = true;
            _quantitative   = true;
            _stringCode     = C_STRING_CODE;
            _unique         = false;

            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "целое число");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "integer number";

        /// <summary>
        /// Извлечение значения свойства из строки XML
        /// </summary>
        /// <param name="in_xmlString">Строка XML, содержащая извлекаемое значение</param>
        /// <returns>Типизированное значение свойства</returns>
        public override object ParseValueFromXmlString(string in_xmlString) { return int.Parse(in_xmlString); }

        /// <summary>
        /// Создание типизированной заготовки для хранения значения.
        /// </summary>
        /// <returns>Заготовка для значения свойства</returns>
        public override IPropertyValue CreatePropertyValue()
        {
            return new PropertyValue<int?>() { Definition = (PropertyDefinition)_propertyDefinition };
        }
    };
}
