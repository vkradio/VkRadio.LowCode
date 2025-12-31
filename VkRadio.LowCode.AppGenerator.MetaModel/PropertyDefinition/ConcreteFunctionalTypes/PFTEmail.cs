using MetaModel.Names;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - e-mail
    /// </summary>
    public class PFTEmail: PFTString
    {
        /// <summary>
        /// Конструктор функционального типа свойства - e-mail
        /// </summary>
        public PFTEmail()
        {
            _defaultValue       = null;
            _nullable           = true;
            _quantitative       = false;
            _stringCode         = C_STRING_CODE;
            _unique             = false;
            _defaultMaxLength   = 100;
            _defaultMinLength   = 0;
            _maxLength          = 100;
            _minLength          = 0;

            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "адрес электронной почты");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "e-mail";
    };
}
