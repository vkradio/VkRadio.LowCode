using MetaModel.Names;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - текст
    /// </summary>
    public class PFTText: PFTString
    {
        /// <summary>
        /// Конструктор функционального типа свойства - текста
        /// </summary>
        public PFTText()
        {
            //_defaultValue       = string.Empty;
            _defaultValue       = null;
            _nullable           = true;
            _quantitative       = false;
            _stringCode         = C_STRING_CODE;
            _unique             = false;
            _defaultMaxLength   = 8000;
            _defaultMinLength   = 0;
            _maxLength          = 8000;
            _minLength          = 0;

            _defaultNames.Clear();
            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "текст");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "text";
    };
}
