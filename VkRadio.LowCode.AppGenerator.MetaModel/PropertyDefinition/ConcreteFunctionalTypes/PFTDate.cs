using MetaModel.Names;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - дата
    /// </summary>
    public class PFTDate: PFTDateTime
    {
        /// <summary>
        /// Конструктор фунционального типа свойства - даты
        /// </summary>
        public PFTDate()
        {
            _defaultValue   = null;
            _nullable       = true;
            _quantitative   = false;
            _stringCode     = C_STRING_CODE;
            _unique         = false;

            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "дата");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "date";
    };
}
