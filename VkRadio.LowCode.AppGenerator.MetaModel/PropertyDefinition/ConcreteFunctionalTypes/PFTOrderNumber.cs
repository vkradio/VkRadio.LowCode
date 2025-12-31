using MetaModel.Names;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - порядковый номер
    /// </summary>
    public class PFTOrderNumber: PFTInteger
    {
        /// <summary>
        /// Конструктор функционального типа свойства - порядкового номера
        /// </summary>
        public PFTOrderNumber()
        {
            _defaultValue   = null;
            _nullable       = false;
            _quantitative   = true;
            _stringCode     = C_STRING_CODE;
            _unique         = true;

            _defaultNames.Clear();
            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "порядковый номер");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        new public const string C_STRING_CODE = "order number";
    };
}
