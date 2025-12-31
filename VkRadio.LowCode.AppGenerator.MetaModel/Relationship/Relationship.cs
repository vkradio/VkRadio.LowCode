using System;
using System.Xml.Linq;

using MetaModel.Names;

namespace MetaModel.Relationship
{
    /// <summary>
    /// Абстрактная связь между типами объектов данных (ТОД)
    /// </summary>
    public abstract class Relationship: IUnique
    {
        protected Guid _id;
        protected MetaModel _metaModel;

        /// <summary>
        /// Конструктор абстрактной связи между ТОД
        /// </summary>
        /// <param name="in_id">Id связи</param>
        /// <param name="in_metaModel">Метамодель</param>
        protected Relationship(Guid in_id, MetaModel in_metaModel) { _id = in_id; _metaModel = in_metaModel; }

        /// <summary>
        /// Догрузка конкретных свойств связи из узла XML
        /// </summary>
        /// <param name="in_xel">Узел XML, содержащий описание связи</param>
        protected abstract void LoadFromXElement(XElement in_xel);

        /// <summary>
        /// Уникальный идентификатор связи между объектами
        /// </summary>
        public Guid Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Метамодель
        /// </summary>
        public MetaModel MetaModel { get { return _metaModel; } }

        /// <summary>
        /// Загрузка связи между ТОД из узла XML
        /// </summary>
        /// <param name="in_metaModel">Метамодель</param>
        /// <param name="in_xel">Узел XML, содержащий описание связи</param>
        /// <returns>Связь между ТОД</returns>
        public static Relationship LoadFromXElement(MetaModel in_metaModel, XElement in_xel)
        {
            // 1. Загрузка базовых свойств IUnique.
            Guid id = new Guid(in_xel.Element("Id").Value);
            string relTypeCode = in_xel.Element("Type").Value;
            
            // 2. Загрузка описания конкретного типа связи.
            Relationship rel;
            switch (relTypeCode)
            {
                case RelationshipConnector.C_TYPE_CODE:
                    rel = new RelationshipConnector(id, in_metaModel);
                    break;
                case RelationshipReference.C_TYPE_CODE:
                    rel = new RelationshipReference(id, in_metaModel);
                    break;
                case RelationshipTable.C_TYPE_CODE:
                    rel = new RelationshipTable(id, in_metaModel);
                    break;
                default:
                    throw new ApplicationException(string.Format("Element Relationship Id {0} has unsupported Type - {1}.", id, relTypeCode ?? "<NULL>"));
            }
            rel.LoadFromXElement(in_xel);

            return rel;
        }
    };
}
