using System;

using MetaModel.PredefinedDO;
using MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Асбтрактный функциональный тип свойства - связь с другими объектами
    /// </summary>
    public abstract class PFTLink: PropertyFunctionalType
    {
        /// <summary>
        /// Конструктор функционального типа свойства - связи с другими объектами
        /// </summary>
        public PFTLink()
        {
            _defaultValue   = null;
            _nullable       = true;
            _quantitative   = false;
            _unique         = false;
        }

        /// <summary>
        /// Извлечение значения ссылки (Guid) из строки XML
        /// </summary>
        /// <param name="in_xmlString">Строка XML, содержащая ссылку в виде Guid</param>
        /// <returns>Ссылочное значение, содержащее только ссылку, без значения объекта</returns>
        public override object ParseValueFromXmlString(string in_xmlString) { return new SRefObject(new Guid(in_xmlString)); }

        /// <summary>
        /// Создание типизированной заготовки для хранения значения.
        /// </summary>
        /// <returns>Заготовка для значения свойства</returns>
        public override IPropertyValue CreatePropertyValue()
        {
            return new PropertyValue<SRefObject>() { Definition = (PropertyDefinition)_propertyDefinition };
        }
    };
}
