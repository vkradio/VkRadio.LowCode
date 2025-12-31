namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - табличная (подчиненная) часть
    /// </summary>
    public class PFTTablePart: PFTLink
    {
        Relationship.RelationshipTable _relationshipTable;

        /// <summary>
        /// Конструктор функционального типа свойства - табличной (подчиненной) части
        /// </summary>
        public PFTTablePart()
        {
            _stringCode = C_STRING_CODE;
        }

        /// <summary>
        /// Связь
        /// </summary>
        public Relationship.RelationshipTable RelationshipTable { get { return _relationshipTable; } set { _relationshipTable = value; } }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "table";
    };
}
