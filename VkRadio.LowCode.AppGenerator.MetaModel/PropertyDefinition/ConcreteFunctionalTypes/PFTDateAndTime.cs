using MetaModel.Names;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - дата и время
    /// </summary>
    public class PFTDateAndTime: PFTDateTime
    {
        /// <summary>
        /// Конструктор фунционального типа свойства - даты и времени
        /// </summary>
        public PFTDateAndTime()
        {
            _defaultValue   = null;
            _nullable       = true;
            _quantitative   = false;
            _stringCode     = C_STRING_CODE;
            _unique         = false;

            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "дата и время");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "date and time";
    };
}
