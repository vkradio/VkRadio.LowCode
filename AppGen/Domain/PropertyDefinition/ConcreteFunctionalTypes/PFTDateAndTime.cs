using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - date and time
/// </summary>
public class PFTDateAndTime : PFTDateTime
{
    public override string StringCode => C_STRING_CODE;

    /// <summary>
    /// Constructor
    /// </summary>
    public PFTDateAndTime(bool? nullable, bool? unique, XElement xElement)
        : base(nullable ?? true, unique ?? false, xElement)
    {
        DefaultValue = null;
        Quantitative = false;

        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "дата и время");
    }

    /// <summary>
    /// String code of a functional property type (used in MetaModel files)
    /// </summary>
    public const string C_STRING_CODE = "date and time";
}
