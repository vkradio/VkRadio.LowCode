using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;

/// <summary>
/// Functional type of a property
/// </summary>
public abstract class PropertyFunctionalType
{
    protected IPropertyDefinition _propertyDefinition;
    protected object _defaultValue;
    protected bool _nullable;
    protected bool _quantitative;
    protected string _stringCode;
    protected bool _unique;
    protected Type _systemType;
    protected Dictionary<HumanLanguageEnum, string> _defaultNames = new Dictionary<HumanLanguageEnum,string>();

    /// <summary>
    /// Определение свойства ТОД или значения регистра, к которому относится данный ФТ
    /// </summary>
    public IPropertyDefinition PropertyDefinition { get { return _propertyDefinition; } set { _propertyDefinition = value; } }
    /// <summary>
    /// Значение типа свойства по умолчанию
    /// </summary>
    public object DefaultValue { get { return _defaultValue; } set { _defaultValue = value; } }
    /// <summary>
    /// Допускает ли тип свойства отсутствующее значение
    /// </summary>
    public bool Nullable { get { return _nullable; } set { _nullable = value; } }
    /// <summary>
    /// Является ли тип свойства количественным
    /// </summary>
    public bool Quantitative { get { return _quantitative; } }
    /// <summary>
    /// Строковый код типа
    /// </summary>
    public string StringCode { get { return _stringCode; } }
    /// <summary>
    /// Является ли тип свойства уникальным значением (среди однородных свойств объектов-владельцев)
    /// </summary>
    public bool Unique { get { return _unique; } set { _unique = value; } }
    /// <summary>
    /// Системный тип, с помощью которого хранится значение
    /// (это свойство не описывается в файле метамодели, а служит только
    /// для ее функционирования внутри среды исполнения, в данном случае .NET)
    /// </summary>
    public Type SystemType { get { return _systemType; } }
    /// <summary>
    /// Имена для свойства по умолчанию.
    /// Если в определении свойства не задано имя на том или ином языке, оно берется по умолчанию
    /// отсюда - из функционального типа. При этом ссылочные ФТ имеют имена, совпадающие с именами
    /// определений ТОД, на которые они ссылаются.
    /// </summary>
    public IDictionary<HumanLanguageEnum, string> DefaultNames { get { return _defaultNames; } }

    /// <summary>
    /// Извлечение значения свойства из строки XML
    /// </summary>
    /// <param name="in_xmlString">Строка XML, содержащая извлекаемое значение</param>
    /// <returns>Типизированное значение свойства</returns>
    public abstract object ParseValueFromXmlString(string in_xmlString);

    /// <summary>
    /// Инициализация расширенного набора параметров для функционального типа
    /// </summary>
    /// <param name="in_xelPropertyDefinition">Элемент XML, содержащий определение свойства</param>
    protected virtual void InitExtendedParams(XElement in_xelPropertyDefinition) {}

    /// <summary>
    /// Извлечение функционального типа свойства из узла XML, содержащего определение свойства
    /// </summary>
    /// <param name="in_xel">Узел XML, содержащий определение свойства</param>
    /// <param name="in_metaModel">Метамодель</param>
    /// <returns>Функциональный тип свойства</returns>
    public static PropertyFunctionalType LoadFromXElement(XElement in_xel, MetaModel in_metaModel)
    {
        XElement xel = in_xel.Element("Nullable");
        bool? nullable = xel != null ? (bool?)bool.Parse(xel.Value) : null;
        xel = in_xel.Element("Unique");
        bool? unique = xel != null ? (bool?)bool.Parse(xel.Value) : null;

        string ftName = in_xel.Element("FunctionalType").Value;
        PropertyFunctionalType result;
        switch (ftName)
        {
            case PFTBackReferencedTable.C_STRING_CODE:
                result = new PFTBackReferencedTable();
                break;
            case PFTBoolean.C_STRING_CODE:
                result = new PFTBoolean();
                break;
            case PFTConnector.C_STRING_CODE:
                result = new PFTConnector();
                break;
            case PFTDate.C_STRING_CODE:
                result = new PFTDate();
                break;
            case PFTDateAndTime.C_STRING_CODE:
                result = new PFTDateAndTime();
                break;
            case PFTDecimal.C_STRING_CODE:
                result = new PFTDecimal();
                break;
            case PFTEmail.C_STRING_CODE:
                result = new PFTEmail();
                break;
            case PFTFilePath.C_STRING_CODE:
                result = new PFTFilePath();
                break;
            case PFTInteger.C_STRING_CODE:
                result = new PFTInteger();
                break;
            case PFTMoney.C_STRING_CODE:
                result = new PFTMoney();
                break;
            case PFTName.C_STRING_CODE:
                result = new PFTName();
                break;
            case PFTOrderNumber.C_STRING_CODE:
                result = new PFTOrderNumber();
                break;
            case PFTPassword.C_STRING_CODE:
                result = new PFTPassword();
                break;
            case PFTPrice.C_STRING_CODE:
                result = new PFTPrice();
                break;
            case PFTQuantity.C_STRING_CODE:
                result = new PFTQuantity();
                break;
            case PFTReferenceValue.C_STRING_CODE:
                result = new PFTReferenceValue();
                break;
            case PFTShortText.C_STRING_CODE:
                result = new PFTShortText();
                break;
            case PFTTableOwner.C_STRING_CODE:
                result = new PFTTableOwner();
                break;
            case PFTTablePart.C_STRING_CODE:
                result = new PFTTablePart();
                break;
            case PFTText.C_STRING_CODE:
                result = new PFTText();
                break;
            case PFTTime.C_STRING_CODE:
                result = new PFTTime();
                break;
            case PFTUniqueCode.C_STRING_CODE:
                result = new PFTUniqueCode();
                break;
            default:
                throw new ApplicationException(string.Format("Property functional type not supported: {0}.", ftName ?? "<NULL>"));
        }

        if (nullable.HasValue)
            result.Nullable = nullable.Value;
        if (unique.HasValue)
            result.Unique = unique.Value;

        IPFTDependentLink dependentLink = result as IPFTDependentLink;
        if (dependentLink != null)
        {
            xel = in_xel.Element("OnRefObjectDelete");
            if (xel != null)
            {
                switch (xel.Value)
                {
                    case "ignore":
                        dependentLink.OnDeleteAction = OnDeleteActionEnum.Ingnore;
                        break;
                    case "delete":
                        dependentLink.OnDeleteAction = OnDeleteActionEnum.Delete;
                        break;
                    case "block":
                        dependentLink.OnDeleteAction = OnDeleteActionEnum.CannotDelete;
                        break;
                    case "set default value":
                        dependentLink.OnDeleteAction = OnDeleteActionEnum.ResetToDefault;
                        break;
                    case "set null":
                        dependentLink.OnDeleteAction = OnDeleteActionEnum.ResetToNull;
                        break;
                    default:
                        throw new ApplicationException(string.Format("Property {0} has unsupported OnRefObjectDelete value: {1}.", in_xel.Element("Id") != null ? in_xel.Element("Id").Value : "<NULL>", xel.Value ?? "<NULL>"));
                }
            }
            else
            {
                if (in_metaModel.DefaultLinksStrict)
                {
                    if (result.Nullable)
                        dependentLink.OnDeleteAction = OnDeleteActionEnum.ResetToNull;
                    else
                        dependentLink.SetDefaultOnDeleteAction();
                }
            }
        }

        result.InitExtendedParams(in_xel);

        return result;
    }

    /// <summary>
    /// Создание типизированной заготовки для хранения значения.
    /// </summary>
    /// <returns>Заготовка для значения свойства</returns>
    public abstract IPropertyValue CreatePropertyValue();
};
