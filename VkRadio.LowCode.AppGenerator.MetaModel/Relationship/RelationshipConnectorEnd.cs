using System;
using System.Xml.Linq;

using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace MetaModel.Relationship
{
    /// <summary>
    /// Один из двух концов связи типа &quot;объект-объект&quot;
    /// </summary>
    public class RelationshipConnectorEnd
    {
        RelationshipConnector _connector;
        RelationshipConnectorEnd _otherEnd;
        byte _endNum;
        //bool _navigable;
        //bool _nullable;
        PropertyDefinition.PropertyDefinition _propertyDefinition;

        /// <summary>
        /// Связь, в которую входит данный конец
        /// </summary>
        public RelationshipConnector Connector { get { return _connector; } }
        /// <summary>
        /// Противоположный конец связи
        /// </summary>
        public RelationshipConnectorEnd OtherEnd { get { return _otherEnd; } set { _otherEnd = value; } }
        /// <summary>
        /// Порядковый номер конца (1 или 2)
        /// </summary>
        public byte EndNum { get { return _endNum; } }
        /// <summary>
        /// Объект, на который указывает конец, доступен (видим) с противоположного конца
        /// </summary>
        //public bool Navigable { get { return _navigable; } }
        /// <summary>
        /// Объект, на который указывает конец, может отсутствовать
        /// </summary>
        //public bool Nullable { get { return _nullable; } }
        /// <summary>
        /// Определение свойства, к которому прикреплен конец
        /// </summary>
        public PropertyDefinition.PropertyDefinition PropertyDefinition { get { return _propertyDefinition; } }

        /// <summary>
        /// Загрузка конца связи из узла XML
        /// </summary>
        /// <param name="in_connector">Связь, в которую входит конец</param>
        /// <param name="in_xel">Узел XML</param>
        /// <param name="in_endNum">Номер конца (1 или 2)</param>
        /// <returns></returns>
        public static RelationshipConnectorEnd LoadFromXElement(RelationshipConnector in_connector, XElement in_xel, byte in_endNum)
        {
            //XElement xel = in_xel.Element("Navigable");
            //bool navigable = xel != null ? (bool)xel : true;
            //xel = in_xel.Element("Nullable");
            //bool nullable = xel != null ? (bool)xel : true;

            PropertyDefinition.PropertyDefinition pd = in_connector.MetaModel.AllPropertyDefinitions[new Guid(in_xel.Element("PropertyDefinitionId").Value)];

            RelationshipConnectorEnd end = new RelationshipConnectorEnd()
            {
                _connector = in_connector,
                _endNum = in_endNum,
                //_navigable = navigable,
                //_nullable = nullable,
                _propertyDefinition = pd
            };

            PFTConnector ftConnector = (PFTConnector)pd.FunctionalType;
            ftConnector.RelationshipConnector = in_connector;
            
            return end;
        }
    };
}
