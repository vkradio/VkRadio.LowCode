using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - money value
/// </summary>
public class PFTMoney : PFTInteger
{
    public override string StringCode => C_STRING_CODE;

    public PFTMoney(bool? nullable, bool? unique, XElement xElement)
        : base(nullable, unique, xElement)
    {
        var xel = xElement.Element("DecimalPositions");

        DecimalPositions = xel is not null
            ? int.Parse(xel.Value)
            : 2;

        _defaultNames.Clear();
        _defaultNames.Add(NaturalLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(NaturalLanguageEnum.Ru, "денежная сумма");
    }

    public override object ParseValueFromXmlString(string xmlString)
    {
        var parts = (string.IsNullOrEmpty(xmlString) ? "0" : xmlString).Split(['.']);

        if (parts.Length > 2)
        {
            throw new FormatException(string.Format("Invalid format of PFTMoney value: {0}.", xmlString ?? "<NULL>"));
        }

        var result = int.Parse(parts[0]);

        for (var i = 0; i < DecimalPositions; i++)
        {
            result *= 10;
        }

        if (parts.Length == 2)
        {
            var part = parts[1];

            if (part.Length > DecimalPositions)
            {
                throw new FormatException(string.Format("Invalid format of PFTMoney value: {0} (DecimalPositions = {1}).", xmlString ?? "<NULL>", DecimalPositions));
            }

            var decimalResult = 0;
            var decimalPositionIndexInString = 0;

            for (var i = DecimalPositions; i >= 1; i--)
            {
                var positionValue = part.Length > decimalPositionIndexInString
                    ? int.Parse(part.Substring(decimalPositionIndexInString, 1))
                    : 0;

                decimalPositionIndexInString++;

                for (var j = decimalPositionIndexInString; j < DecimalPositions; j++)
                {
                    positionValue *= 10;
                }

                decimalResult += positionValue;
            }

            result += decimalResult;
        }

        return result;
    }

    new public const string C_STRING_CODE = "money";

    public int DecimalPositions { get; private set; }
}
