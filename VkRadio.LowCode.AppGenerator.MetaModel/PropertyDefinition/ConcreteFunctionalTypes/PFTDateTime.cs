using System;
using System.Xml;

using MetaModel.PredefinedDO;
using MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Абстрактный функциональный тип свойства, содержащий дату и/или время
    /// </summary>
    public abstract class PFTDateTime: PropertyFunctionalType
    {
        /// <summary>
        /// Конструктор функционального типа свойства, содержащего дату и/или время
        /// </summary>
        public PFTDateTime()
        {
            _defaultValue       = null;
            _nullable           = true;
            _quantitative       = false;
            _unique             = false;
        }

        /// <summary>
        /// Извлечение значения свойства даты и времени из строки XML
        /// </summary>
        /// <param name="in_xmlString">Строка XML, содержащая извлекаемое значение</param>
        /// <returns>Типизированное значение свойства, либо специальная строковая метка "runtime",
        /// если требуется использовать текущее системное время исполнения модели</returns>
        public override object ParseValueFromXmlString(string in_xmlString)
        {
            return in_xmlString == C_RUNTIME_MARK ?
                new SDateTime() :
                new SDateTime(XmlConvert.ToDateTime(in_xmlString, XmlDateTimeSerializationMode.RoundtripKind));
        }

        /// <summary>
        /// Создание типизированной заготовки для хранения значения.
        /// </summary>
        /// <returns>Заготовка для значения свойства</returns>
        public override IPropertyValue CreatePropertyValue()
        {
            return new PropertyValue<SDateTime>() { Definition = (PropertyDefinition)_propertyDefinition };
        }

        /// <summary>
        /// Строковый код, указывающий на использование системного времени
        /// исполнения модели
        /// </summary>
        public const string C_RUNTIME_MARK = "runtime";
    };
}
