using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

/// <summary>
/// Functional property type - money value
/// </summary>
public class PFTMoney : PFTInteger
{
    private int _decimalPositions = 2;

    public PFTMoney()
    {
        _stringCode = C_STRING_CODE;

        _defaultNames.Clear();
        _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
        _defaultNames.Add(HumanLanguageEnum.Ru, "денежная сумма");
    }

    protected override void InitExtendedParams(XElement xelPropertyDefinition)
    {
        var xel = xelPropertyDefinition.Element("DecimalPositions");

        if (xel is not null)
        {
            _decimalPositions = int.Parse(xel.Value);
        }
    }
    public override object ParseValueFromXmlString(string xmlString)
    {
        var parts = (string.IsNullOrEmpty(xmlString) ? "0" : xmlString).Split(new char[] { '.' });

        if (parts.Length > 2)
        {
            throw new FormatException(string.Format("Invalid format of PFTMoney value: {0}.", xmlString ?? "<NULL>"));
        }

        var result = int.Parse(parts[0]);

        for (var i = 0; i < _decimalPositions; i++)
        {
            result *= 10;
        }

        if (parts.Length == 2)
        {
            var part = parts[1];

            if (part.Length > _decimalPositions)
            {
                throw new FormatException(string.Format("Invalid format of PFTMoney value: {0} (DecimalPositions = {1}).", xmlString ?? "<NULL>", _decimalPositions));
            }

            var decimalResult = 0;
            var decimalPositionIndexInString = 0;

            for (var i = _decimalPositions; i >= 1; i--)
            {
                var positionValue = part.Length > decimalPositionIndexInString
                    ? int.Parse(part.Substring(decimalPositionIndexInString, 1))
                    : 0;

                decimalPositionIndexInString++;

                for (var j = decimalPositionIndexInString; j < _decimalPositions; j++)
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

    public int DecimalPositions { get { return _decimalPositions; } set { _decimalPositions = value; } }
}
