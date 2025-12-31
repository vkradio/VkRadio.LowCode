using System;
using System.Collections.Generic;
using System.Xml.Linq;

using MetaModel.Names;

namespace MetaModel.PropertyDefinition
{
    /// <summary>
    /// Определение свойства
    /// </summary>
    public class PropertyDefinition: IUniqueNamed, IPropertyDefinition
    {
        Guid _id;
        Dictionary<HumanLanguageEnum, string> _names;
        DOTDefinition.DOTDefinition _ownerDefinition;
        PropertyFunctionalType _functionalType;
        object _defaultValue;

        /// <summary>
        /// Уникальный идентификатор определения свойства
        /// </summary>
        public Guid Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Словарь имен свойства
        /// </summary>
        public IDictionary<HumanLanguageEnum, string> Names { get { return _names; } }
        /// <summary>
        /// Определение ТОД, ялвяющееся владельцем определения данного свойства
        /// </summary>
        public DOTDefinition.DOTDefinition OwnerDefinition { get { return _ownerDefinition; } set { _ownerDefinition = value; } }
        /// <summary>
        /// Функциональный тип свойства
        /// </summary>
        public PropertyFunctionalType FunctionalType { get { return _functionalType; } }
        /// <summary>
        /// Значение свойства по умолчанию
        /// </summary>
        public object DefaultValue { get { return _defaultValue; } }
        /// <summary>
        /// Признак упорядочения объектов в списке
        /// </summary>
        public ListOrderEnum? ListOrder { get; set; }

        static ListOrderEnum ParseListOrderValue(string in_string)
        {
            if (string.IsNullOrEmpty(in_string))
                return ListOrderEnum.Default;

            switch (in_string)
            {
                case "asc":
                    return ListOrderEnum.Asc;
                case "desc":
                    return ListOrderEnum.Desc;
                default:
                    throw new FormatException(string.Format("Неверное значение свойства ListOrder: \"{0}\" (допустимо \"asc\", \"desc\" или пустое).", in_string ?? "<NULL>"));
            }
        }

        /// <summary>
        /// Загрузка определения свойства ТОД из узла XML
        /// </summary>
        /// <param name="in_xel">Узел XML, содержащий определение свойства ТОД</param>
        /// <param name="in_metaModel">Метамодель</param>
        /// <returns>Определение свойства ТОД</returns>
        public static PropertyDefinition LoadFromXElement(XElement in_xel, MetaModel in_metaModel)
        {
            // 1. Загрузка свойств IUniqueNamed.
            Guid id = new Guid(in_xel.Element("Id").Value);
            Dictionary<HumanLanguageEnum, string> names = NameDictionary.LoadNamesFromContainingXElement(in_xel);

            // 2. Извлечение функционального типа свойства.
            PropertyFunctionalType ft = PropertyFunctionalType.LoadFromXElement(in_xel, in_metaModel);

            // 3. Если в определении свойства не заданы все нужные имена, извлекаем их из функционального типа
            //    свойства, а для ссылочных свойств - из ТОД, на которые они указывают.
            NameDictionary.EnrichNames(names, ft.DefaultNames);

            // 4. Извлечение значения по умолчанию. Если есть, берем его из определения свойства,
            //    иначе наследуем из функционального типа.
            XElement xelDefaultValue = in_xel.Element("DefaultValue");
            object defaultValue = xelDefaultValue != null ?
                ft.ParseValueFromXmlString(xelDefaultValue.Value) :
                ft.DefaultValue;

            // 5. Есть ли признак упорядочения списков объектов по этому полю?
            XElement xelListOrder = in_xel.Element("ListOrder");
            ListOrderEnum? listOrder = xelListOrder != null ?
                (ListOrderEnum?)ParseListOrderValue(xelListOrder.Value) :
                null;

            // 6. Формирование определения свойства.
            PropertyDefinition pd = new PropertyDefinition()
            {
                _id = id,
                _names = names,
                _functionalType = ft,
                _defaultValue = defaultValue,
                ListOrder = listOrder
            };

            // 7. Отложенное связывание функционального типа свойства с определением свойства.
            pd.FunctionalType.PropertyDefinition = pd;

            return pd;
        }
    };
}
