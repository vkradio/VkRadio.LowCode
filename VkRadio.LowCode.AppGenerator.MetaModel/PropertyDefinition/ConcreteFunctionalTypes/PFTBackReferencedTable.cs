namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - коллекция (таблица) объектов,
    /// косвенно вычисляемая из ссылок на данный объект справочника.
    /// </summary>
    public class PFTBackReferencedTable: PFTLink
    {
        Relationship.RelationshipReference _relationshipReference;

        /// <summary>
        /// Конструктор функционального типа свойства - коллекции (таблицы) объектов,
        /// косвенно вычисляемой из ссылок на данный объект справочника.
        /// </summary>
        public PFTBackReferencedTable()
        {
            _stringCode = C_STRING_CODE;
        }

        /// <summary>
        /// Связь
        /// </summary>
        public Relationship.RelationshipReference RelationshipReference
        {
            get { return _relationshipReference; }
            set
            {
                _relationshipReference = value;

                // Извлекаем из определения ТОД справочника имена и устанавливаем их
                // в качестве имен по умолчанию для текущего свойства.
                //_defaultNames.Clear();
                //if (_relationshipReference != null)
                //{
                //    foreach (var name in _relationshipReference.ReferenceDefinition.Names)
                //        _defaultNames.Add(name.Key, name.Value);
                //}
            }
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "back referenced table";
    };
}
