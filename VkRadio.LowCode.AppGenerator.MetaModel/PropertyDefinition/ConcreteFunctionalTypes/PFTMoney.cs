using System;
using System.Xml.Linq;

using MetaModel.Names;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - денежная сумма
    /// </summary>
    public class PFTMoney: PFTInteger
    {
        int _decimalPositions = 2;

        /// <summary>
        /// Конструктор функционального типа свойства - денежной суммы
        /// </summary>
        public PFTMoney()
        {
            _stringCode = C_STRING_CODE;

            _defaultNames.Clear();
            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "денежная сумма");
        }

        protected override void InitExtendedParams(XElement in_xelPropertyDefinition)
        {
            XElement xel = in_xelPropertyDefinition.Element("DecimalPositions");
            if (xel != null)
                _decimalPositions = int.Parse(xel.Value);
        }
        public override object ParseValueFromXmlString(string in_xmlString)
        {
            string[] parts = (string.IsNullOrEmpty(in_xmlString) ? "0" : in_xmlString).Split(new char[] { '.' });
            if (parts.Length > 2)
                throw new FormatException(string.Format("Invalid format of PFTMoney value: {0}.", in_xmlString ?? "<NULL>"));

            int result = int.Parse(parts[0]);

            for (int i = 0; i < _decimalPositions; i++)
                result *= 10;

            if (parts.Length == 2)
            {
                string part = parts[1];

                if (part.Length > _decimalPositions)
                    throw new FormatException(string.Format("Invalid format of PFTMoney value: {0} (DecimalPositions = {1}).", in_xmlString ?? "<NULL>", _decimalPositions));

                int decimalResult = 0;

                int decimalPositionIndexInString = 0;
                for (int i = _decimalPositions; i >= 1; i--)
                {
                    int positionValue = part.Length > decimalPositionIndexInString ? int.Parse(part.Substring(decimalPositionIndexInString, 1)) : 0;
                    decimalPositionIndexInString++;

                    for (int j = decimalPositionIndexInString; j < _decimalPositions; j++)
                        positionValue *= 10;

                    decimalResult += positionValue;
                }

                result += decimalResult;
            }

            return result;
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        new public const string C_STRING_CODE = "money";

        public int DecimalPositions { get { return _decimalPositions; } set { _decimalPositions = value; } }
    };
}
