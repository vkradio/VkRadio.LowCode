using System;
using System.Windows.Forms;
using System.Globalization;
using VkRadio.LowCode.Gui.Utils;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class StringField : UserControl
    {
        protected string _caption = "Caption";
        protected bool _treatAsDecimal;
        protected int _decimalPositions = 2;

        public StringField()
        {
            InitializeComponent();
        }

        public string Value { get { return T_Value.Text; } set { T_Value.Text = value; } }
        public string Caption { get { return _caption; } set { _caption = value; L_Caption.Text = _caption + ":"; } }
        public bool Multiline { get { return T_Value.Multiline; } set { T_Value.Multiline = value; if (value) T_Value.ScrollBars = ScrollBars.Vertical; } }
        public bool TreatAsDecimal { get { return _treatAsDecimal; } set { _treatAsDecimal = value; } }
        public int DecimalPositions { get { return _decimalPositions; } set { _decimalPositions = value; } }

        public event EventHandler ValueChanged;

        public DateTime? GetValueAsDateTimeNullable()
        {
            string errStub;
            return GetValueAsDateTimeNullable(true, out errStub);
        }
        public DateTime? GetValueAsDateTimeNullable(bool in_ignoreError, out string out_errorMessage)
        {
            return GetValueAsDateTimeNullable(Value, in_ignoreError, out out_errorMessage);
        }

        public static DateTime? GetValueAsDateTimeNullable(string in_value, bool in_ignoreError, out string out_errorMessage)
        {
            DateTime result;
            out_errorMessage = null;
            bool success = DateTime.TryParse(in_value, out result);
            if (success)
            {
                return result;
            }
            else
            {
                if (in_ignoreError)
                {
                    out_errorMessage = "Строка в неверном формате.";
                    return null;
                }
                else
                {
                    result = DateTime.Parse(in_value);
                    return result; // Досюда на самом деле не дойдет, т.к. на предыдущем шаге будет Exception.
                }
            }
        }
        public static DateTime GetValueAsDateTime(string in_value, DateTime in_oldValue)
        {
            string errorMessageStub;
            DateTime? result = GetValueAsDateTimeNullable(in_value, true, out errorMessageStub);
            return result.HasValue ? result.Value : in_oldValue;
        }

        public DateTime GetValueAsDateTime(DateTime in_oldValue)
        {
            return GetValueAsDateTime(Value, in_oldValue);
        }
        public DateTime GetValueAsDateTime(DateTime in_oldValue, bool in_ignoreError, out string out_errorMessage)
        {
            DateTime? result = GetValueAsDateTimeNullable(true, out out_errorMessage);
            if (!result.HasValue)
            {
                if (in_ignoreError)
                {
                    return in_oldValue;
                }
                else
                {
                    result = DateTime.Parse(Value);
                    return result.Value; // Досюда на самом деле не дойдет, т.к. на предыдущем шаге будет Exception.
                }
            }
            else
            {
                return result.Value;
            }
        }
        public string GetValueAsString()
        {
            return GetValueAsStringNullable() ?? string.Empty;
        }
        public string GetValueAsStringNullable()
        {
            return string.IsNullOrWhiteSpace(Value) ? null : Value.Trim();
        }
        public int GetValueAsInt(int in_oldValue)
        {
            int result;

            if (_treatAsDecimal)
            {
                decimal decimalValue = GetValueAsDecimal(in_oldValue);
                for (int i = 0; i < _decimalPositions; i++)
                    decimalValue *= 10;
                result = (int)decimalValue;
            }
            else
            {
                result = int.TryParse(Value, out result) ? result : in_oldValue;
            }

            return result;
        }
        public static int? GetValueAsIntNullable(string in_value)
        {
            int? result = null;

            {
                int tmpResult;
                result = int.TryParse(in_value, out tmpResult) ? (int?)tmpResult : null;
            }

            return result;
        }
        public int? GetValueAsIntNullable()
        {
            int? result = null;

            if (_treatAsDecimal)
            {
                decimal? decimalValue = GetValueAsDecimalNullable();
                if (decimalValue.HasValue)
                {
                    decimal dec2 = decimalValue.Value;
                    for (int i = 0; i < _decimalPositions; i++)
                        dec2 *= 10;
                    result = (int)dec2;
                }
            }
            else
            {
                int tmpResult;
                result = int.TryParse(Value, out tmpResult) ? (int?)tmpResult : null;
            }

            return result;
        }
        public decimal GetValueAsDecimal(decimal in_oldValue)
        {
            decimal result;
            return decimal.TryParse(Value, out result) ? result : in_oldValue;
        }
        public decimal? GetValueAsDecimalNullable()
        {
            decimal result;
            return decimal.TryParse(Value, out result) ? (decimal?)result : null;
        }
        public Guid GetValueAsGuid(Guid in_value)
        {
            Guid? result = GetValueAsGuidNullable();
            return result.HasValue ? result.Value : in_value;
        }
        public Guid? GetValueAsGuidNullable()
        {
            Guid result;
            if (Guid.TryParse(Value, out result))
                return result;
            else
                return null;
        }

        public void SetValue(DateTime? in_value)
        {
            Value = in_value.HasValue ? in_value.Value.ToString(FormatHelper.C_DATE_TIME_FORMAT) : string.Empty;
        }
        public void SetValue(int? in_value)
        {
            string value = !in_value.HasValue ?
                string.Empty :
                (_treatAsDecimal ?
                    FormatHelper.GetDecimalString(in_value.Value, _decimalPositions) :
                    in_value.Value.ToString());

            Value = value;
        }
        public void SetValue(decimal? in_value)
        {
            Value = in_value.HasValue ? in_value.Value.ToString(CultureInfo.CurrentCulture) : string.Empty;
        }
        public void SetValue(string in_value) { Value = in_value ?? string.Empty; }
        public void SetValue(Guid? in_value)
        {
            Value = in_value.HasValue ? in_value.Value.ToString() : string.Empty;
        }

        private void T_Value_TextChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(sender, e);
        }
    }
}
