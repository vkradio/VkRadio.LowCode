using System;
using System.Collections.Generic;
using System.Xml.Linq;

using MetaModel.Names;
using MetaModel.PropertyDefinition;

namespace MetaModel.RegisterDefinition
{
    /// <summary>
    /// Определение одного из значений регистра
    /// </summary>
    public class RegisterValueDefinition: IPropertyDefinition
    {
        Guid _id;
        Dictionary<HumanLanguageEnum, string> _names;
        RegisterDefinition _registerDefinition;
        PropertyFunctionalType _functionalType;

        /// <summary>
        /// Уникальный идентификатор определения значения регистра
        /// </summary>
        public Guid Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Словарь имен определения значения регистра
        /// </summary>
        public IDictionary<HumanLanguageEnum, string> Names { get { return _names; } }
        /// <summary>
        /// Определение регистра, в который входит данное значение
        /// </summary>
        public RegisterDefinition RegisterDefinition { get { return _registerDefinition; } set { _registerDefinition = value; } }
        /// <summary>
        /// Функциональный тип значения регистра
        /// </summary>
        public PropertyFunctionalType FunctionalType { get { return _functionalType; } }
        /// <summary>
        /// Признак упорядочения объектов в списке
        /// </summary>
        public ListOrderEnum? ListOrder { get; set; }

        /// <summary>
        /// Загрузка определения значения регистра из узла XML
        /// </summary>
        /// <param name="in_metaModel">Метамодель</param>
        /// <param name="in_xel">Узел XML</param>
        /// <returns>Определение значения регистра</returns>
        public static RegisterValueDefinition LoadFromXElement(MetaModel in_metaModel, XElement in_xel)
        {
            // 1. Загрузка свойств IUniqueNamed.
            Guid id = new Guid(in_xel.Element("Id").Value);
            Dictionary<HumanLanguageEnum, string> names = NameDictionary.LoadNamesFromContainingXElement(in_xel);

            // 2. Нахождение определения ТОД, на который указывает ключ.
            //Guid dotDefinitionId = new Guid(in_xel.Element("DOTDefinitionId").Value);
            //DOTDefinition.DOTDefinition dotDefinition = in_metaModel.AllDOTDefinitions[dotDefinitionId];

            // 3. Загрузка описания функционального типа значения регистра.
            PropertyFunctionalType ft = PropertyFunctionalType.LoadFromXElement(in_xel, in_metaModel);

            // 4. Создание объекта, описывающего значение регистра.
            RegisterValueDefinition rvd = new RegisterValueDefinition()
            {
                _id = id,
                _names = names,
                _functionalType = ft
            };

            // 5. Отложенное связывание функционального типа значения регистра со значением регистра.
            rvd.FunctionalType.PropertyDefinition = rvd;

            return rvd;
        }
    };
}
