using System;
using System.Collections.Generic;
using System.Xml.Linq;

using MetaModel.Names;
using MetaModel.Names.Translit.RU;

namespace MetaModel.PredefinedDO
{
    /// <summary>
    /// Предопределенный объект данных
    /// </summary>
    public class PredefinedDO: IUniqueNamed
    {
        Guid _id;
        MetaModel _metaModel;
        DOTDefinition.DOTDefinition _dotDefinition;
        Dictionary<Guid, IPropertyValue> _propertyValues;
        Dictionary<HumanLanguageEnum, string> _names;

        /// <summary>
        /// Уникальный идентификатор предопределенного объекта
        /// </summary>
        public Guid Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Метамодель
        /// </summary>
        public MetaModel MetaModel { get { return _metaModel; } }
        /// <summary>
        /// Определение типа объекта данных
        /// </summary>
        public DOTDefinition.DOTDefinition DOTDefinition { get { return _dotDefinition; } }
        /// <summary>
        /// Значения свойств
        /// </summary>
        public IDictionary<Guid, IPropertyValue> PropertyValues { get { return _propertyValues; } }
        /// <summary>
        /// Словарь имен ПОД
        /// </summary>
        public IDictionary<HumanLanguageEnum, string> Names { get { return _names; } }

        /// <summary>
        /// Загрузка предопределенного объекта данных из узла XML
        /// </summary>
        /// <param name="in_metaModel">Метамодель</param>
        /// <param name="in_xel">Узел XML</param>
        /// <returns>Предопределенный объект данных</returns>
        public static PredefinedDO LoadFromXElement(MetaModel in_metaModel, XElement in_xel)
        {
            // 1. Считывание Id предопределенного объекта.
            Guid id = new Guid(in_xel.Element("Id").Value);

            // 2. Связывание с определением ТОД.
            Guid dotDefinitionId = new Guid(in_xel.Element("DOTDefinitionId").Value);
            DOTDefinition.DOTDefinition dotDefinition = in_metaModel.AllDOTDefinitions[dotDefinitionId];

            // 3. Загрузка предопределенных имен, если они заданы (иначе будет браться из свойств).
            Dictionary<HumanLanguageEnum, string> names = NameDictionary.LoadNamesFromContainingXElement(in_xel);
            bool searchNameProp = names.Count == 0;

            // 4. Загрузка значений для заданных свойств и установка значений по умолчанию для остальных
            //    свойств.
            Dictionary<Guid, IPropertyValue> propertyValues = new Dictionary<Guid,IPropertyValue>();
            XElement xelValues = in_xel.Element("PropertyValues");
            foreach (PropertyDefinition.PropertyDefinition propDef in dotDefinition.PropertyDefinitions.Values)
            {
                // Ищем узел XML, содержащий значение свойства.
                // TODO: Здесь наверняка можно написать поиск компактнее, используя функционал LINQ.
                XElement xelPropValue = null;
                foreach (XElement xel in xelValues.Elements("PropertyValue"))
                {
                    XElement xelPropDefId = xel.Element("PropertyDefinitionId");
                    Guid propDefId = new Guid(xelPropDefId.Value);
                    if (propDefId == propDef.Id)
                    {
                        xelPropValue = xel;
                        break;
                    }
                }

                // Если узел со значением найден, загружаем его, иначе используем значение
                // по умолчанию, извлекаемое из определения свойства.
                IPropertyValue propertyValue = propDef.FunctionalType.CreatePropertyValue();
                if (xelPropValue != null)
                {
                    XElement xelValue = xelPropValue.Element("Value");
                    XAttribute xat = xelValue.Attribute("UseName");
                    if (xat != null && bool.Parse(xat.Value))
                    {
                        if (names.Count == 0)
                            throw new ApplicationException(string.Format("UseName=\"True\" is set for PredefinedDO Id {0}, but no Name element exists.", id));
                        // TODO: Здесь нужен более обобщенный и гибкий механизм переключения между локальными и английскими текстами.
                        propertyValue.ValueObject = names.ContainsKey(HumanLanguageEnum.Ru) ? names[HumanLanguageEnum.Ru] : names[HumanLanguageEnum.En];
                    }
                    else
                    {
                        propertyValue.ValueObject = propDef.FunctionalType.ParseValueFromXmlString(xelValue.Value);
                    }
                }
                else
                {
                    propertyValue.ValueObject = propDef.DefaultValue;
                }

                propertyValues.Add(propertyValue.Definition.Id, propertyValue);

                // Если имена ПОД не заданы явно тегом Name, ищем подходящее имя среди свойств.
                if (searchNameProp && propDef.Names[HumanLanguageEnum.En] == "name")
                {
                    string nameValue = xelPropValue.Element("Value").Value;
                    HumanLanguageEnum lang = NameDictionary.DetectLanguage(nameValue);
                    if (lang == HumanLanguageEnum.Ru)
                    {
                        names.Add(HumanLanguageEnum.Ru, nameValue);
                        nameValue = Transliteration.Front(nameValue);
                    }
                    names.Add(HumanLanguageEnum.En, nameValue);

                    searchNameProp = false;
                }
            }

            // 5. Если имя ПОД так и не найдено, берем его из первого попавшегося значения
            //    свойства.
            if (searchNameProp)
            {
                foreach (IPropertyValue pval in propertyValues.Values)
                {
                    string nameValue = pval.ValueObject.ToString();
                    HumanLanguageEnum lang = NameDictionary.DetectLanguage(nameValue);
                    if (lang == HumanLanguageEnum.Ru)
                        nameValue = Transliteration.Front(nameValue);
                    names.Add(HumanLanguageEnum.En, nameValue);

                    break;
                }
            }

            // 6. Создание предопределенного объекта на основе ранее считанных параметров.
            PredefinedDO pdo = new PredefinedDO()
            {
                _id = id,
                _dotDefinition = dotDefinition,
                _metaModel = in_metaModel,
                _propertyValues = propertyValues,
                _names = names
            };

            return pdo;
        }
    };
}
