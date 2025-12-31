using System;
using System.Collections.Generic;
using System.Xml.Linq;

using MetaModel.Names;

namespace MetaModel.RegisterDefinition
{
    /// <summary>
    /// Определение части ключа регистра
    /// </summary>
    public class RegisterKeyDefinition: IUniqueNamed
    {
        Guid _id;
        Dictionary<HumanLanguageEnum, string> _names;
        RegisterDefinition _registerDefinition;
        DOTDefinition.DOTDefinition _dotDefinition;

        /// <summary>
        /// Уникальный идентификатор определения ключа регистра
        /// </summary>
        public Guid Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Словарь имен определения ключа регистра
        /// </summary>
        public IDictionary<HumanLanguageEnum, string> Names { get { return _names; } }
        /// <summary>
        /// Определение регистра, в который входит данный ключ
        /// </summary>
        public RegisterDefinition RegisterDefinition { get { return _registerDefinition; } set { _registerDefinition = value; } }
        /// <summary>
        /// Определение ключевого ТОД
        /// </summary>
        public DOTDefinition.DOTDefinition DOTDefinition { get { return _dotDefinition; } }

        /// <summary>
        /// Загрузка определения ключа регистра из узла XML
        /// </summary>
        /// <param name="in_metaModel">Метамодель</param>
        /// <param name="in_xel">Узел XML</param>
        /// <returns>Определение ключа регистра</returns>
        public static RegisterKeyDefinition LoadFromXElement(MetaModel in_metaModel, XElement in_xel)
        {
            // 1. Загрузка свойств IUniqueNamed.
            Guid id = new Guid(in_xel.Element("Id").Value);
            Dictionary<HumanLanguageEnum, string> names = NameDictionary.LoadNamesFromContainingXElement(in_xel);

            // 2. Нахождение определения ТОД, на который указывает ключ.
            Guid dotDefinitionId = new Guid(in_xel.Element("DOTDefinitionId").Value);
            DOTDefinition.DOTDefinition dotDefinition = in_metaModel.AllDOTDefinitions[dotDefinitionId];

            return new RegisterKeyDefinition()
            {
                _id = id,
                _names = names,
                _dotDefinition = dotDefinition
            };
        }
    };
}
