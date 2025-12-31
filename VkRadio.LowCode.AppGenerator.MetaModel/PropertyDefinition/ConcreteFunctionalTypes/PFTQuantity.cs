using MetaModel.Names;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - количество
    /// </summary>
    public class PFTQuantity: PFTInteger
    {
        /// <summary>
        /// Конструктор функционального типа свойства - количества
        /// </summary>
        public PFTQuantity()
        {
            //_defaultValue   = 0;
            _defaultValue   = null;
            _nullable       = true;
            _quantitative   = true;
            _stringCode     = C_STRING_CODE;
            _unique         = false;

            _defaultNames.Clear();
            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "количество");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        new public const string C_STRING_CODE = "quantity";
    };
}
