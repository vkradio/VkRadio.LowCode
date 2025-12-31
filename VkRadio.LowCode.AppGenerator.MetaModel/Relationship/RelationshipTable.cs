using System;
using System.Xml.Linq;

using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;
using MetaModel.Names;

namespace MetaModel.Relationship
{
    /// <summary>
    /// Связь типа &quot;объект и подчиненная ему таблица&quot;
    /// </summary>
    public class RelationshipTable: Relationship
    {
        bool _supportsHierarchy;
        PropertyDefinition.PropertyDefinition _propertyDefinitionInOwner;
        PropertyDefinition.PropertyDefinition _propertyDefinitionInTable;

        /// <summary>
        /// Конструктор связи
        /// </summary>
        /// <param name="in_id">Id связи</param>
        /// <param name="in_metaModel">Метамодель</param>
        public RelationshipTable(Guid in_id, MetaModel in_metaModel) : base(in_id, in_metaModel) {}

        /// <summary>
        /// Поддержка иерархических структур. Позволяет отсутствовать владельцу у корневого узла
        /// </summary>
        public bool SupportsHierarchy { get { return _supportsHierarchy; } }
        /// <summary>
        /// Определение свойства - владельца табличной части (представлено как коллекция в ООП)
        /// </summary>
        public PropertyDefinition.PropertyDefinition PropertyDefinitionInOwner { get { return _propertyDefinitionInOwner; } }
        /// <summary>
        /// Определение свойства табличной части, указывающей на ее владельца (представлено как ВК в БД)
        /// </summary>
        public PropertyDefinition.PropertyDefinition PropertyDefinitionInTable { get { return _propertyDefinitionInTable; } }

        /// <summary>
        /// Код типа связи
        /// </summary>
        public const string C_TYPE_CODE = "table";

        /// <summary>
        /// Догрузка конкретных свойств связи из узла XML
        /// </summary>
        /// <param name="in_xel">Узел XML, содержащий описание связи</param>
        protected override void LoadFromXElement(XElement in_xel)
        {
            XElement xel = in_xel.Element("SupportsHierarchy");
            _supportsHierarchy = xel != null ? (bool)xel : false;

            Guid ownerPropertyDefinitionId = new Guid(in_xel.Element("PropertyDefinitionIdInOwner").Value);
            _propertyDefinitionInOwner = _metaModel.AllPropertyDefinitions[ownerPropertyDefinitionId];
            PFTTablePart ftTablePart = (PFTTablePart)_propertyDefinitionInOwner.FunctionalType;
            ftTablePart.RelationshipTable = this;

            Guid tablePropertyDefinitionId = new Guid(in_xel.Element("PropertyDefinitionIdInTable").Value);
            _propertyDefinitionInTable = _metaModel.AllPropertyDefinitions[tablePropertyDefinitionId];
            PFTTableOwner ftTableOwner = (PFTTableOwner)_propertyDefinitionInTable.FunctionalType;
            ftTableOwner.RelationshipTable = this;
            _propertyDefinitionInTable.FunctionalType.Nullable = _supportsHierarchy;

            // Отложенное обогащение словарей имен свойств. Если какое-либо свойство не имело имени
            // на каком-либо языке, такое имя берется из определения ТОД, на который оно указывает, если только
            // такое имя есть у ТОД.
            NameDictionary.EnrichNamesForCollection(_propertyDefinitionInOwner.Names, _propertyDefinitionInTable.OwnerDefinition.Names);
            NameDictionary.EnrichNames(_propertyDefinitionInTable.Names, _propertyDefinitionInOwner.OwnerDefinition.Names);
        }
    };
}
