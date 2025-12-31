using System;
using System.Xml.Linq;

using pd = MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;
using MetaModel.Names;

namespace MetaModel.Relationship
{
    /// <summary>
    /// Связь типа &quot;ссылка на значение справочника&quot;
    /// </summary>
    public class RelationshipReference: Relationship
    {
        pd.IPropertyDefinition _ownerPropertyDefinition;
        pd.PropertyDefinition _backRefTablePropertyDefinition;
        DOTDefinition.DOTDefinition _referenceDefinition;

        /// <summary>
        /// Конструктор связи
        /// </summary>
        /// <param name="in_id">Id связи</param>
        /// <param name="in_metaModel">Метамодель</param>
        public RelationshipReference(Guid in_id, MetaModel in_metaModel) : base(in_id, in_metaModel) {}

        /// <summary>
        /// Определение свойства ТОД или значения регистра, содержащее ссылку на значение
        /// справочника (другими словами, ВК на справочник)
        /// </summary>
        public pd.IPropertyDefinition OwnerPropertyDefinition { get { return _ownerPropertyDefinition; } }
        /// <summary>
        /// Определение свойства ТОД справочника, являющегося неявно выводимой таблицей объектов из ссылок
        /// на этот справочник. Может отсутствовать для обычных связей со справочниками (большинство
        /// связей именно такие).
        /// </summary>
        public pd.PropertyDefinition BackRefTablePropertyDefinition { get { return _backRefTablePropertyDefinition; } }
        /// <summary>
        /// Определение ТОД справочника
        /// </summary>
        public DOTDefinition.DOTDefinition ReferenceDefinition { get { return _referenceDefinition; } }

        /// <summary>
        /// Код типа связи
        /// </summary>
        public const string C_TYPE_CODE = "reference";

        /// <summary>
        /// Догрузка конкретных свойств связи из узла XML
        /// </summary>
        /// <param name="in_xel">Узел XML, содержащий описание связи</param>
        protected override void LoadFromXElement(XElement in_xel)
        {
            Guid ownerPropertyDefinitionId = new Guid(in_xel.Element("OwnerPropertyDefinitionId").Value);
            _ownerPropertyDefinition = _metaModel.AllPropertyDefinitions.ContainsKey(ownerPropertyDefinitionId) ?
                (pd.IPropertyDefinition)_metaModel.AllPropertyDefinitions[ownerPropertyDefinitionId] :
                (pd.IPropertyDefinition)_metaModel.AllRegisterValueDefinitions[ownerPropertyDefinitionId];

            XElement xel = in_xel.Element("ReferenceDefinitionId");
            if (xel == null)
            {
                Guid backRefTablePropertyDefinitionId = new Guid(in_xel.Element("BackRefTablePropertyDefinitionId").Value);
                _backRefTablePropertyDefinition = (pd.PropertyDefinition)_metaModel.AllPropertyDefinitions[backRefTablePropertyDefinitionId];

                // Обратное связывание свойства с данной связью.
                PFTBackReferencedTable ftBackRefTable = (PFTBackReferencedTable)_backRefTablePropertyDefinition.FunctionalType;
                ftBackRefTable.RelationshipReference = this;

                _referenceDefinition = _backRefTablePropertyDefinition.OwnerDefinition;
            }
            else
            {
                Guid referenceDefinitionId = new Guid(xel.Value);
                _referenceDefinition = _metaModel.AllDOTDefinitions[referenceDefinitionId];
            }

            // Обратное связывание свойства с данной связью.
            PFTReferenceValue ftRefValue = (PFTReferenceValue)_ownerPropertyDefinition.FunctionalType;
            ftRefValue.RelationshipReference = this;

            // Отложенное обогащение словаря имен свойства. Если какое-либо свойство не имело имени
            // на каком-либо языке, такое имя берется из определения ТОД, на который оно указывает, если только
            // такое имя есть у ТОД.
            NameDictionary.EnrichNames(_ownerPropertyDefinition.Names, _referenceDefinition.Names);
            if (_backRefTablePropertyDefinition != null) // TODO: Здесь после реализации регистров возможен глюк, т.к. считаем, что определение свойства только из ТОД, но не из регистра.
                NameDictionary.EnrichNames(_backRefTablePropertyDefinition.Names, ((pd.PropertyDefinition)_ownerPropertyDefinition).OwnerDefinition.Names);
        }
    };
}
