using System;

using MetaModel.Names;
using MetaModel.PredefinedDO;
using MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - уникальный код (GUID)
    /// </summary>
    public class PFTUniqueCode: PropertyFunctionalType
    {
        const string C_GENERATE_MARK = "generate";

        /// <summary>
        /// Конструктор функционального типа свойства - уникального кода (GUID)
        /// </summary>
        public PFTUniqueCode()
        {
            _defaultValue   = null;
            _nullable       = true;
            _quantitative   = false;
            _unique         = true;
            _stringCode     = C_STRING_CODE;

            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "уникальный код");
        }

        /// <summary>
        /// Извлечение значения свойства уникального кода из строки XML
        /// </summary>
        /// <param name="in_xmlString">Строка XML, содержащая извлекаемое значение</param>
        /// <returns>Типизированное значение свойства, либо специальная строковая метка "generate",
        /// если требуется сгенерировать уникальный код во время исполнения модели</returns>
        public override object ParseValueFromXmlString(string in_xmlString)
        {
            return in_xmlString == C_GENERATE_MARK ?
                new SGuid() :
                new SGuid(Guid.Parse(in_xmlString));
        }

        /// <summary>
        /// Создание типизированной заготовки для хранения значения.
        /// </summary>
        /// <returns>Заготовка для значения свойства</returns>
        public override IPropertyValue CreatePropertyValue()
        {
            return new PropertyValue<SGuid>() { Definition = (PropertyDefinition)_propertyDefinition };
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "unique code";
    };
}
