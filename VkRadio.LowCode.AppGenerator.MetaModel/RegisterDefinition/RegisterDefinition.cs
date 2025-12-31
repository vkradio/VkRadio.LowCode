using System;
using System.Collections.Generic;
using System.Xml.Linq;

using MetaModel.Names;

namespace MetaModel.RegisterDefinition
{
    /// <summary>
    /// Определение абстрактного регистра
    /// </summary>
    public abstract class RegisterDefinition: IUniqueNamed
    {
        Guid _id;
        Dictionary<HumanLanguageEnum, string> _names;
        MetaModel _metaModel;
        Dictionary<Guid, RegisterKeyDefinition> _keyDefinitions;
        Dictionary<Guid, RegisterValueDefinition> _valueDefinitions;
        List<RegisterSourceDefinition> _sourceDefinitions;

        /// <summary>
        /// Уникальный идентификатор определения регистра
        /// </summary>
        public Guid Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Словарь имен определения регистра
        /// </summary>
        public IDictionary<HumanLanguageEnum, string> Names { get { return _names; } }
        /// <summary>
        /// Метамодель
        /// </summary>
        public MetaModel MetaModel { get { return MetaModel; } }
        /// <summary>
        /// Опредеделения ключей регистра
        /// </summary>
        public IDictionary<Guid, RegisterKeyDefinition> KeyDefinitions { get { return _keyDefinitions; } }
        /// <summary>
        /// Определения значений регистра
        /// </summary>
        public IDictionary<Guid, RegisterValueDefinition> ValueDefinitions { get { return _valueDefinitions; } }
        /// <summary>
        /// Определение допустимых типов источников регистра
        /// </summary>
        public IList<RegisterSourceDefinition> SourceDefinitions { get { return _sourceDefinitions; } }

        /// <summary>
        /// Загрузка определение регистра из узла XML
        /// </summary>
        /// <param name="in_metaModel">Метамодель</param>
        /// <param name="in_xel">Узел XML</param>
        /// <returns>Определение регистра</returns>
        public static RegisterDefinition LoadFromXElement(MetaModel in_metaModel, XElement in_xel)
        {
            // 1. Загрузка свойств IUniqueNamed.
            Guid id = new Guid(in_xel.Element("Id").Value);
            Dictionary<HumanLanguageEnum, string> names = NameDictionary.LoadNamesFromContainingXElement(in_xel);

            // 2. Загрузка определений ключей регистра.
            Dictionary<Guid, RegisterKeyDefinition> keyDefs = new Dictionary<Guid,RegisterKeyDefinition>();
            XElement xelKeys = in_xel.Element("Keys");
            foreach (XElement xel in xelKeys.Elements("Key"))
            {
                RegisterKeyDefinition keyDef = RegisterKeyDefinition.LoadFromXElement(in_metaModel, xel);
                keyDefs.Add(keyDef.Id, keyDef);
            }

            // 3. Загрузка определений значений регистра.
            Dictionary<Guid, RegisterValueDefinition> valueDefs = new Dictionary<Guid,RegisterValueDefinition>();
            XElement xelValues = in_xel.Element("Values");
            foreach (XElement xel in xelValues.Elements("Value"))
            {
                RegisterValueDefinition valueDef = RegisterValueDefinition.LoadFromXElement(in_metaModel, xel);
                valueDefs.Add(valueDef.Id, valueDef);
            }

            // 4. Загрузка перечня допустимых источников регистра.
            List<RegisterSourceDefinition> sources = new List<RegisterSourceDefinition>();
            XElement xelSources = in_xel.Element("Sources");
            if (xelSources != null)
            {
                foreach (XElement xel in xelSources.Elements("Source"))
                {
                    RegisterSourceDefinition srcDef = RegisterSourceDefinition.LoadFromXElement(in_metaModel, xel);
                    sources.Add(srcDef);
                }
            }

            // 5. Создание объекта - определения регистра.
            RegisterDefinition regDef;
            string regTypeCode = in_xel.Element("Type").Value;
            switch (regTypeCode)
            {
                case RDAccumulator.C_TYPE_CODE:
                    regDef = new RDAccumulator();
                    break;
                case RDInformation.C_TYPE_CODE:
                    regDef = new RDInformation();
                    break;
                case RDTurnover.C_TYPE_CODE:
                    regDef = new RDTurnover();
                    break;
                default:
                    throw new ApplicationException(string.Format("Unsupported register type code {0} for register definition {1}.", regTypeCode ?? "<NULL>", id));
            }
            regDef._id = id;
            regDef._names = names;
            regDef._metaModel = in_metaModel;
            regDef._keyDefinitions = keyDefs;
            regDef._valueDefinitions = valueDefs;
            regDef._sourceDefinitions = sources;

            // 6. Отложенное связывание свойств, указывающих на определение регистра.
            foreach (RegisterKeyDefinition keyDef in keyDefs.Values)
                keyDef.RegisterDefinition = regDef;
            foreach (RegisterValueDefinition valueDef in valueDefs.Values)
                valueDef.RegisterDefinition = regDef;
            foreach (RegisterSourceDefinition src in sources)
                src.RegisterDefinition = regDef;

            return regDef;
        }
    };
}
