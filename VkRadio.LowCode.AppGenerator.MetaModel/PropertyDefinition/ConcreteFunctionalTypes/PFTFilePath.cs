using MetaModel.Names;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - путь к файловому ресурсу
    /// </summary>
    public class PFTFilePath: PFTString
    {
        /// <summary>
        /// Конструктор функционального типа свойства - пути к файловому ресурсу
        /// </summary>
        public PFTFilePath()
        {
            _defaultValue       = null;
            _nullable           = true;
            _quantitative       = false;
            _stringCode         = C_STRING_CODE;
            _unique             = false;
            _defaultMaxLength   = 255;
            _defaultMinLength   = 0;
            _maxLength          = 255;
            _minLength          = 0;

            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "путь к файлу");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "file path";
    };
}
