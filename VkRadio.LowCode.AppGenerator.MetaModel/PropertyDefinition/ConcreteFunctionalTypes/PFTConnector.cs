using MetaModel.Relationship;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - связь между единичными объектами
    /// </summary>
    public class PFTConnector : PFTLink, IPFTDependentLink
    {
        Relationship.RelationshipConnector _relationshipConnector;

        /// <summary>
        /// Конструктор функционального типа свойства - связи между единичными объектами
        /// </summary>
        public PFTConnector()
        {
            _stringCode = C_STRING_CODE;
        }

        /// <summary>
        /// Связь
        /// </summary>
        public Relationship.RelationshipConnector RelationshipConnector
        {
            get { return _relationshipConnector; }
            set
            {
                _relationshipConnector = value;

                // Устанавливаем имена свойства по умолчанию, извлекая их из противоположного
                // конца связи, т.е. из определения ТОД, на который указывает данная связь.
                //_defaultNames.Clear();
                //if (_relationshipConnector != null)
                //{
                //    RelationshipConnectorEnd end = _relationshipConnector.End1.PropertyDefinition.Id == _propertyDefinition.Id ?
                //        _relationshipConnector.End2 :
                //        _relationshipConnector.End1;
                    
                //    foreach (var name in end.PropertyDefinition.OwnerDefinition.Names)
                //        _defaultNames.Add(name.Key, name.Value);
                //}
            }
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "object connector";

        public OnDeleteActionEnum OnDeleteAction { get; set; }
        public void SetDefaultOnDeleteAction() { OnDeleteAction = OnDeleteActionEnum.Ingnore; }
    };
}
