namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - ссылка на владельца таблицы
    /// </summary>
    public class PFTTableOwner: PFTLink, IPFTDependentLink
    {
        Relationship.RelationshipTable _relationshipTable;

        /// <summary>
        /// Конструктор функционального типа свойства - ссылки на владельца таблицы
        /// </summary>
        public PFTTableOwner()
        {
            _stringCode = C_STRING_CODE;
        }

        /// <summary>
        /// Связь
        /// </summary>
        public Relationship.RelationshipTable RelationshipTable
        {
            get { return _relationshipTable; }
            set
            {
                _relationshipTable = value;

                // Извлекаем имена из определения ТОД, на который указывает связь, и делаем
                // их именами по умолчанию для данного свойства.
                //_defaultNames.Clear();
                //if (_relationshipTable != null)
                //{
                //    DOTDefinition.DOTDefinition otherDOT = _relationshipTable.OwnerPropertyDefinition.Id == _propertyDefinition.Id ?
                //        _relationshipTable.TablePropertyDefinition.OwnerDefinition :
                //        _relationshipTable.OwnerPropertyDefinition.OwnerDefinition;

                //    foreach (var name in otherDOT.Names)
                //        _defaultNames.Add(name.Key, name.Value);
                //}
            }
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "table owner";

        public OnDeleteActionEnum OnDeleteAction { get; set; }
        public void SetDefaultOnDeleteAction() { OnDeleteAction = OnDeleteActionEnum.Delete; }
    };
}
