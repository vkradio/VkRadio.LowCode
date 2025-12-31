using MetaModel.Names;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - цена
    /// </summary>
    public class PFTPrice: PFTMoney
    {
        /// <summary>
        /// Конструктор функционального типа свойства - цены
        /// </summary>
        public PFTPrice()
        {
            _stringCode = C_STRING_CODE;

            _defaultNames.Clear();
            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "цена");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        new public const string C_STRING_CODE = "price";
    };
}
