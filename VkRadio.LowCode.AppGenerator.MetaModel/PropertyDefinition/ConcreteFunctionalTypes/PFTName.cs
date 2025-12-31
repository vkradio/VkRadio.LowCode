using MetaModel.Names;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - наименование (имя)
    /// </summary>
    public class PFTName: PFTString
    {
        /// <summary>
        /// Конструктор функционального типа свойства - наименования
        /// </summary>
        public PFTName()
        {
            //_defaultValue       = string.Empty;
            _defaultValue       = null;
            _nullable           = false;
            _quantitative       = false;
            _stringCode         = C_STRING_CODE;
            _unique             = true;
            _defaultMaxLength   = 255;
            _defaultMinLength   = 1;
            _maxLength          = 255;
            _minLength          = 1;

            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "наименование");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "name";
    };
}
