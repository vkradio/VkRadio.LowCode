using System;
using System.Xml.Linq;

using MetaModel.Names;

namespace MetaModel.Relationship
{
    /// <summary>
    /// Связь типа &quot;объект-объект&quot;
    /// </summary>
    public class RelationshipConnector: Relationship
    {
        RelationshipConnectorEnd _end1;
        RelationshipConnectorEnd _end2;

        /// <summary>
        /// Конструктор связи
        /// </summary>
        /// <param name="in_id">Id связи</param>
        /// <param name="in_metaModel">Метамодель</param>
        public RelationshipConnector(Guid in_id, MetaModel in_metaModel) : base(in_id, in_metaModel) {}

        /// <summary>
        /// Первый конец связи
        /// </summary>
        public RelationshipConnectorEnd End1 { get { return _end1; } }
        /// <summary>
        /// Второй конец связи
        /// </summary>
        public RelationshipConnectorEnd End2 { get { return _end2; } }

        /// <summary>
        /// Получение противоположного заданному конца связи.
        /// </summary>
        /// <param name="in_thisEnd">Конец связи, для котого нужно извлечь противоположный конец</param>
        /// <returns>Противоположный конец</returns>
        public RelationshipConnectorEnd GetOtherEnd(RelationshipConnectorEnd in_thisEnd)
        {
            return _end1.PropertyDefinition.Id == in_thisEnd.PropertyDefinition.Id ? _end2 : _end1;
        }

        /// <summary>
        /// Код типа связи
        /// </summary>
        public const string C_TYPE_CODE = "connector";

        /// <summary>
        /// Догрузка конкретных свойств связи из узла XML
        /// </summary>
        /// <param name="in_xel">Узел XML, содержащий описание связи</param>
        protected override void LoadFromXElement(XElement in_xel)
        {
            _end1 = RelationshipConnectorEnd.LoadFromXElement(this, in_xel.Element("End1"), 1);
            _end2 = RelationshipConnectorEnd.LoadFromXElement(this, in_xel.Element("End2"), 2);

            _end1.OtherEnd = _end2;
            _end2.OtherEnd = _end1;

            // Отложенное взаимное обогащение словарей имен свойств. Если какое-либо свойство не имело имени
            // на каком-либо языке, такое имя берется из определения ТОД, на который оно указывает, если только
            // такое имя есть у ТОД.
            NameDictionary.EnrichNames(_end1.PropertyDefinition.Names, _end2.PropertyDefinition.OwnerDefinition.Names);
            NameDictionary.EnrichNames(_end2.PropertyDefinition.Names, _end1.PropertyDefinition.OwnerDefinition.Names);
        }
    };
}
