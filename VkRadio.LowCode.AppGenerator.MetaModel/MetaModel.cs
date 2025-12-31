using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

using MetaModel.Names;
using MetaModel.PredefinedDO;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;
using MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace MetaModel
{
    /// <summary>
    /// Метамодель (синонимы: словарь предметной области, МПОБ)
    /// </summary>
    public class MetaModel: INamed
    {
        Dictionary<HumanLanguageEnum, string> _names;
        //int _engineFeaturesetRev = 0;
        //IDictionary<int, EngineFeaturesetRevDesc> _aboveEngineFeaturesetRevs;
        Dictionary<Guid, PredefinedDO.PredefinedDO> _allPredefinedDOs;
        Dictionary<Guid, DOTDefinition.DOTDefinition> _allDOTDefinitions;
        Dictionary<Guid, RegisterDefinition.RegisterDefinition> _allRegisterDefinitions;
        Dictionary<Guid, Relationship.Relationship> _allRelationships;
        Dictionary<Guid, PropertyDefinition.PropertyDefinition> _allPropertyDefinitions;
        Dictionary<Guid, RegisterDefinition.RegisterValueDefinition> _allRegisterValueDefinitions;

        /// <summary>
        /// Закрытый конструктор метамодели
        /// </summary>
        MetaModel()
        {
            _allPredefinedDOs = new Dictionary<Guid, PredefinedDO.PredefinedDO>();
            _allDOTDefinitions = new Dictionary<Guid, DOTDefinition.DOTDefinition>();
            _allRegisterDefinitions = new Dictionary<Guid, RegisterDefinition.RegisterDefinition>();
            _allRelationships = new Dictionary<Guid, Relationship.Relationship>();
        }

        #region Закрытые методы загрузки метамодели из файла
        /// <summary>
        /// Закрытая загрузка метамодели из файла
        /// </summary>
        /// <param name="in_filePath">Путь к файлу метамодели</param>
        void PrivateLoad(string in_filePath)
        {
            XElement xelRoot = XElement.Load(in_filePath);

            XElement xelDefaultLinks = xelRoot.Element("DefaultLinks");
            if (xelDefaultLinks != null)
            {
                if (xelDefaultLinks.Value == "strict")
                    DefaultLinksStrict = true;
                else if (xelDefaultLinks.Value == "free")
                    DefaultLinksStrict = false;
                else
                    throw new ApplicationException(string.Format("Unsupported root metamodel DefaultLinks value: \"{0}\". Supported values: \"strict\", \"free\" (default).", xelDefaultLinks.Value ?? "<NULL>"));
            }

            if (xelRoot.Name != "MetaModel")
            {
                if (xelRoot.Name != "MetaModelCompound")
                    throw new ApplicationException("Root metamodel element is not MetaModel or MetaModelCompound.");

                // Загружаем все файлы набора МПОБ.
                XElement xelPackages = xelRoot.Element("Packages");
                if (xelPackages == null)
                    throw new ApplicationException("Packages element not found in MetaModelCompound.");
                Dictionary<string, XElement> xelPackageRoots = new Dictionary<string, XElement>();
                foreach (XElement xelPackagePath in xelPackages.Elements("PackagePath"))
                {
                    FileInfo fi = new FileInfo(in_filePath);
                    string path = Path.Combine(fi.DirectoryName, xelPackagePath.Value);
                    XElement xelPackageRoot = XElement.Load(path);
                    if (xelPackageRoot.Name != "MetaModelPackage")
                        throw new ApplicationException("Root metamodel element is not MetaModelPackage.");

                    xelPackageRoots.Add(path, xelPackageRoot);
                }

                // Загрузка определений типов объектов данных и их свойств.
                foreach (XElement xel in xelPackageRoots.Values)
                    LoadDOTDefinitions(xel);
                // Загрузка определений регистров, их ключей, значений, источников.
                foreach (XElement xel in xelPackageRoots.Values)
                    LoadRegisterDefinitions(xel);
                // Загрузка связей. Ссылки на типы объектов данных и свойства проставляются сразу,
                // и одновременно у соответствующих свойств устанавливаются ссылки на соответствующие им связи.
                foreach (XElement xel in xelPackageRoots.Values)
                    LoadRelationships(xel);
                // Загрузка предопределенных объектов.
                foreach (XElement xel in xelPackageRoots.Values)
                    LoadPredefinedDOs(xel);
            }
            else
            {
                // Загрузка определений типов объектов данных и их свойств.
                LoadDOTDefinitions(xelRoot);
                // Загрузка определений регистров, их ключей, значений, источников.
                LoadRegisterDefinitions(xelRoot);
                // Загрузка связей. Ссылки на типы объектов данных и свойства проставляются сразу,
                // и одновременно у соответствующих свойств устанавливаются ссылки на соответствующие им связи.
                LoadRelationships(xelRoot);
                // Загрузка предопределенных объектов.
                LoadPredefinedDOs(xelRoot);
            }

            _names = NameDictionary.LoadNamesFromContainingXElement(xelRoot);

            // Выполняем отложенное связывание значений функциональных типов по умолчанию в виде
            // ссылок на ТОД.
            ChangePredefinedDOGuidsToRefsInDefaults();
            // Выполняем отложенное связывание ссылочных значений предопределенных объектов.
            ChangePredefinedDOGuidsToRefsInPredefinedDOPropValues();
            // Проверка определений свойств ТОД ссылочного типа и значений регистров ссылочного типа на отсутствие
            // непрогрузок.
            CheckRefPropertyIntegrity();
        }
        /// <summary>
        /// Загрузка определений ТОД
        /// </summary>
        /// <param name="in_xelRoot">Корневой элемент XML файла метамодели</param>
        void LoadDOTDefinitions(XElement in_xelRoot)
        {
            XElement xelDOTDefinitions = in_xelRoot.Element("DOTDefinitions");
            if (xelDOTDefinitions == null)
                throw new ApplicationException("Element DOTDefinitions not found in metamodel.");

            foreach (XElement xelDOTDefinition in xelDOTDefinitions.Elements("DOTDefinition"))
            {
                DOTDefinition.DOTDefinition dotDef = DOTDefinition.DOTDefinition.LoadFromXElement(this, xelDOTDefinition);
                _allDOTDefinitions.Add(dotDef.Id, dotDef);
            }
        }
        /// <summary>
        /// Загрузка определений регистров
        /// </summary>
        /// <param name="in_xelRoot">Корневой элемент XML файла метамодели</param>
        void LoadRegisterDefinitions(XElement in_xelRoot)
        {
            XElement xelRegisterDefinitions = in_xelRoot.Element("RegisterDefinitions");
            if (xelRegisterDefinitions != null)
            {
                foreach (XElement xelRegisterDefinition in xelRegisterDefinitions.Elements("RegisterDefinition"))
                {
                    RegisterDefinition.RegisterDefinition registerDef = RegisterDefinition.RegisterDefinition.LoadFromXElement(this, xelRegisterDefinition);
                    _allRegisterDefinitions.Add(registerDef.Id, registerDef);
                }
            }
        }
        /// <summary>
        /// Загрузка связей
        /// </summary>
        /// <param name="in_xelRoot">Корневой элемент XML файла метамодели</param>
        void LoadRelationships(XElement in_xelRoot)
        {
            XElement xelRels = in_xelRoot.Element("Relationships");
            if (xelRels == null)
                throw new ApplicationException("Element Relationships not found in metamodel.");

            foreach (XElement xelRel in xelRels.Elements("Relationship"))
            {
                Relationship.Relationship dotRel = Relationship.Relationship.LoadFromXElement(this, xelRel);
                _allRelationships.Add(dotRel.Id, dotRel);
            }
        }
        /// <summary>
        /// Загрузка предопределенных объектов данных
        /// </summary>
        /// <param name="in_xelRoot"></param>
        void LoadPredefinedDOs(XElement in_xelRoot)
        {
            XElement xelDOs = in_xelRoot.Element("PredefinedDOs");
            if (xelDOs == null)
                throw new ApplicationException("Element PredefinedDOs not found in metamodel.");

            foreach (XElement xelDO in xelDOs.Elements("PredefinedDO"))
            {
                PredefinedDO.PredefinedDO pdo = PredefinedDO.PredefinedDO.LoadFromXElement(this, xelDO);
                try
                {
                    _allPredefinedDOs.Add(pdo.Id, pdo);
                }
                catch (Exception ex)
                {
                    throw new UniquinessException(pdo.Id, "Неуникальный ПОД", ex);
                }
            }
        }
        /// <summary>
        /// Выполняем отложенное связывание значений функциональных типов по умолчанию в виде
        /// ссылок на ТОД
        /// </summary>
        void ChangePredefinedDOGuidsToRefsInDefaults()
        {
            foreach (PropertyDefinition.PropertyDefinition def in AllPropertyDefinitions.Values)
            {
                PFTLink pftLink = def.FunctionalType as PFTLink;
                if (pftLink != null && pftLink.DefaultValue != null)
                {
                    SRefObject refObj = (SRefObject)pftLink.DefaultValue;
                    refObj.Value = AllPredefinedDOs[refObj.Key];
                }
            }
        }
        /// <summary>
        /// Выполняем отложенное связывание ссылочных значений предопределенных объектов
        /// </summary>
        void ChangePredefinedDOGuidsToRefsInPredefinedDOPropValues()
        {
            foreach (PredefinedDO.PredefinedDO pdo in AllPredefinedDOs.Values)
            {
                foreach (IPropertyValue pValue in pdo.PropertyValues.Values)
                {
                    PFTLink pftLink = pValue.Definition.FunctionalType as PFTLink;
                    if (pftLink != null && pValue.ValueObject != null)
                    {
                        SRefObject refObj = (SRefObject)pValue.ValueObject;
                        try
                        {
                            refObj.Value = AllPredefinedDOs[refObj.Key];
                        }
                        catch (KeyNotFoundException ex)
                        {
                            throw new UniquinessException(refObj.Key, "Отложенное связывание ссылочных значений ПОД", ex);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Проверка определений свойств ТОД ссылочного типа и значений регистров ссылочного типа
        /// на отсутствие непрогрузок
        /// </summary>
        void CheckRefPropertyIntegrity()
        {
            foreach (PropertyDefinition.PropertyDefinition propDef in AllPropertyDefinitions.Values)
            {
                if (propDef.FunctionalType is PFTLink)
                {
                    PFTConnector pftConnector = propDef.FunctionalType as PFTConnector;
                    if (pftConnector != null)
                    {
                        if (pftConnector.RelationshipConnector == null)
                            throw new ApplicationException(string.Format("Property {0} have no RelationshipConnector object specified.", propDef.Id));
                        continue;
                    }
                    
                    PFTReferenceValue pftReferenceValue = propDef.FunctionalType as PFTReferenceValue;
                    if (pftReferenceValue != null)
                    {
                        if (pftReferenceValue.RelationshipReference == null)
                            throw new ApplicationException(string.Format("Property {0} have no RelationshipReference object specified.", propDef.Id));
                        continue;
                    }

                    PFTTableOwner pftTableOwner = propDef.FunctionalType as PFTTableOwner;
                    if (pftTableOwner != null)
                    {
                        if (pftTableOwner.RelationshipTable == null)
                            throw new ApplicationException(string.Format("Property {0} have no RelationshipTable object specified.", propDef.Id));
                        continue;
                    }

                    PFTTablePart pftTablePart = propDef.FunctionalType as PFTTablePart;
                    if (pftTablePart != null)
                    {
                        if (pftTablePart.RelationshipTable == null)
                            throw new ApplicationException(string.Format("Property {0} have no RelationshipTable object specified.", propDef.Id));
                        continue;
                    }

                    PFTBackReferencedTable pftBackRefTable = propDef.FunctionalType as PFTBackReferencedTable;
                    if (pftBackRefTable != null)
                    {
                        if (pftBackRefTable.RelationshipReference == null)
                            throw new ApplicationException(string.Format("Property {0} have no RelationshipReference object specified.", propDef.Id));
                        continue;
                    }

                    throw new ApplicationException(string.Format("Property {0} have unknown PFTLink type FunctionalType ({1}).", propDef.Id, propDef.FunctionalType.GetType().Name));
                }
            }
        }
        #endregion

        /// <summary>
        /// Имя метамодели (словаря, МПОБ) на разных ЕЯ
        /// </summary>
        public IDictionary<HumanLanguageEnum, string> Names { get { return _names; } }
        public bool DefaultLinksStrict { get; set; }
        /// <summary>
        /// Номер ревизии SVN движка, под который составлена метамодель
        /// </summary>
        //public int EngineFeaturesetRev { get { return _engineFeaturesetRev; } }
        /// <summary>
        /// Описания вышестоящих (более новых) версий движка
        /// </summary>
        //public IDictionary<int, EngineFeaturesetRevDesc> AboveEngineFeaturesetRevs { get { return _aboveEngineFeaturesetRevs; } }
        /// <summary>
        /// Полный словарь предопределенных объектов данных
        /// </summary>
        public IDictionary<Guid, PredefinedDO.PredefinedDO> AllPredefinedDOs { get { return _allPredefinedDOs; } }
        /// <summary>
        /// Полный словарь определений типов объектов данных (ТОД)
        /// </summary>
        public IDictionary<Guid, DOTDefinition.DOTDefinition> AllDOTDefinitions { get { return _allDOTDefinitions; } }
        /// <summary>
        /// Полный словарь определений регистров
        /// </summary>
        public IDictionary<Guid, RegisterDefinition.RegisterDefinition> AllRegisterDefinitions { get { return _allRegisterDefinitions; } }
        /// <summary>
        /// Полный словарь связей между объектами данных
        /// </summary>
        public IDictionary<Guid, Relationship.Relationship> AllRelationships { get { return _allRelationships; } }
        /// <summary>
        /// Полный словарь определений свойств ТОД
        /// </summary>
        public IDictionary<Guid, PropertyDefinition.PropertyDefinition> AllPropertyDefinitions
        {
            get
            {
                if (_allPropertyDefinitions == null)
                {
                    _allPropertyDefinitions = new Dictionary<Guid,PropertyDefinition.PropertyDefinition>();

                    foreach (DOTDefinition.DOTDefinition dotDef in _allDOTDefinitions.Values)
                    {
                        foreach (PropertyDefinition.PropertyDefinition propDef in dotDef.PropertyDefinitions.Values)
                        {
                            _allPropertyDefinitions.Add(propDef.Id, propDef);
                        }
                    }
                }
                return _allPropertyDefinitions;
            }
        }
        /// <summary>
        /// Полный словарь определений значений регистров
        /// </summary>
        public IDictionary<Guid, RegisterDefinition.RegisterValueDefinition> AllRegisterValueDefinitions
        {
            get
            {
                if (_allRegisterValueDefinitions == null)
                {
                    _allRegisterValueDefinitions = new Dictionary<Guid,RegisterDefinition.RegisterValueDefinition>();

                    foreach (RegisterDefinition.RegisterDefinition registerDef in _allRegisterDefinitions.Values)
                    {
                        foreach (RegisterDefinition.RegisterValueDefinition valueDef in registerDef.ValueDefinitions.Values)
                        {
                            _allRegisterValueDefinitions.Add(valueDef.Id, valueDef);
                        }
                    }
                }
                return _allRegisterValueDefinitions;
            }
        }

        /// <summary>
        /// Загрузка новой метамодели из файла
        /// </summary>
        /// <param name="in_filePath">Путь к файлу метамодели</param>
        /// <returns>Загруженная метамодель</returns>
        public static MetaModel Load(string in_filePath)
        {
            MetaModel metaModel = new MetaModel();
            metaModel.PrivateLoad(in_filePath);
            return metaModel;
        }
    };
}
