using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Boolean functional property type
/// </summary>
public class PFTBoolean : PropertyFunctionalType
{
    public PFTBoolean()
    {
        _defaultValue = null;
        _nullable = true;
        _quantitative = false;
        _stringCode = C_STRING_CODE;
        _unique = false;
        _systemType = typeof(bool);

        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "булево");
    }

    /// <summary>
    /// Строковый код фунционального типа свойства (используется в файле метамодели)
    /// </summary>
    public const string C_STRING_CODE = "boolean";

    /// <summary>
    /// Извлечение значения свойства из строки XML
    /// </summary>
    /// <param name="in_xmlString">Строка XML, содержащая извлекаемое значение</param>
    /// <returns>Типизированное значение свойства</returns>
    public override object ParseValueFromXmlString(string in_xmlString) { return bool.Parse(in_xmlString); }

    /// <summary>
    /// Создание типизированной заготовки для хранения значения.
    /// </summary>
    /// <returns>Заготовка для значения свойства</returns>
    public override IPropertyValue CreatePropertyValue()
    {
        return new PropertyValue<bool?>() { Definition = (PropertyDefinition)_propertyDefinition };
    }
};
