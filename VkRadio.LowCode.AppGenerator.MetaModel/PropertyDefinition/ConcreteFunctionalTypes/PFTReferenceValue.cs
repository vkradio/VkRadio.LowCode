// TODO: нужно реализовать значение типа "tabled reference value" (и соотв.отношение), т.е.
// то же, что "reference value", но не одна ссылка, а коллекция ссылок, так чтобы сам справочник
// ничего не знал о том, кто на него ссылается (т.е. не имел явного ВК на "хозяина")

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - ссылка на элемент справочника
    /// </summary>
    public class PFTReferenceValue: PFTLink, IPFTDependentLink
    {
        Relationship.RelationshipReference _relationshipReference;

        /// <summary>
        /// Конструктор функционального типа свойства - ссылки на элемент справочника
        /// </summary>
        public PFTReferenceValue()
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
        public const string C_STRING_CODE = "reference value";

        public OnDeleteActionEnum OnDeleteAction { get; set; }
        public void SetDefaultOnDeleteAction() { OnDeleteAction = OnDeleteActionEnum.CannotDelete; }
    };
}
