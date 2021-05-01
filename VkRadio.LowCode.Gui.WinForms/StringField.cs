using System;
using System.Windows.Forms;
using System.Globalization;
using VkRadio.LowCode.Gui.Utils;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class StringField : UserControl
    {
        protected string _caption = "Caption";

        public StringField()
        {
            InitializeComponent();
        }

        public string Value { get { return T_Value.Text; } set { T_Value.Text = value; } }
        public string Caption { get { return _caption; } set { _caption = value; L_Caption.Text = _caption + ":"; } }
        public bool Multiline { get { return T_Value.Multiline; } set { T_Value.Multiline = value; if (value) T_Value.ScrollBars = ScrollBars.Vertical; } }
        public bool TreatAsDecimal { get; set; }
        public int DecimalPositions { get; set; } = 2;

        public event EventHandler ValueChanged;

        public DateTime? GetValueAsDateTimeNullable() => GetValueAsDateTimeNullable(true, out _);
        public DateTime? GetValueAsDateTimeNullable(bool in_ignoreError, out string out_errorMessage) =>
            GetValueAsDateTimeNullable(Value, in_ignoreError, out out_errorMessage);

        public static DateTime? GetValueAsDateTimeNullable(string in_value, bool in_ignoreError, out string out_errorMessage)
        {
            out_errorMessage = null;
            bool success = DateTime.TryParse(in_value, out DateTime result);
            if (success)
            {
                return result;
            }
            else
            {
                if (in_ignoreError)
                {
                    out_errorMessage = "String is in invalid format.";
                    return null;
                }
                else
                {
                    result = DateTime.Parse(in_value);
                    return result; // We won't get here because there will be an Exception on the previous step.
                }
            }
        }
        public static DateTime GetValueAsDateTime(string in_value, DateTime in_oldValue) =>
            GetValueAsDateTimeNullable(in_value, true, out _) ?? in_oldValue;

        public DateTime GetValueAsDateTime(DateTime in_oldValue) =>
            GetValueAsDateTime(Value, in_oldValue);

        public DateTime GetValueAsDateTime(DateTime in_oldValue, bool in_ignoreError, out string out_errorMessage)
        {
            var result = GetValueAsDateTimeNullable(true, out out_errorMessage);
            if (!result.HasValue)
            {
                if (in_ignoreError)
                {
                    return in_oldValue;
                }
                else
                {
                    result = DateTime.Parse(Value);
                    return result.Value; // We won't get here because there will be an Exception on the previous step.
                }
            }
            else
            {
                return result.Value;
            }
        }
        public string GetValueAsString() => GetValueAsStringNullable() ?? string.Empty;
        public string GetValueAsStringNullable() => string.IsNullOrWhiteSpace(Value) ? null : Value.Trim();
        public int GetValueAsInt(int in_oldValue)
        {
            int result;

            if (TreatAsDecimal)
            {
                decimal decimalValue = GetValueAsDecimal(in_oldValue);
                for (int i = 0; i < DecimalPositions; i++)
                    decimalValue *= 10;
                result = (int)decimalValue;
            }
            else
            {
                result = int.TryParse(Value, out result) ? result : in_oldValue;
            }

            return result;
        }
        public static int? GetValueAsIntNullable(string in_value) =>
            int.TryParse(in_value, out int tmpResult) ? tmpResult : null;
        public int? GetValueAsIntNullable()
        {
            int? result = null;

            if (TreatAsDecimal)
            {
                decimal? decimalValue = GetValueAsDecimalNullable();
                if (decimalValue.HasValue)
                {
                    decimal dec2 = decimalValue.Value;
                    for (int i = 0; i < DecimalPositions; i++)
                        dec2 *= 10;
                    result = (int)dec2;
                }
            }
            else
            {
                result = int.TryParse(Value, out int tmpResult) ? tmpResult : null;
            }

            return result;
        }
        public decimal GetValueAsDecimal(decimal in_oldValue) =>
            decimal.TryParse(Value, out decimal result) ? result : in_oldValue;
        public decimal? GetValueAsDecimalNullable() =>
            decimal.TryParse(Value, out decimal result) ? (decimal?)result : null;
        public Guid GetValueAsGuid(Guid in_value) => GetValueAsGuidNullable() ?? in_value;
        public Guid? GetValueAsGuidNullable()
        {
            if (Guid.TryParse(Value, out Guid result))
                return result;
            else
                return null;
        }

        public void SetValue(DateTime? in_value) => Value = in_value.HasValue ? in_value.Value.ToString(FormatHelper.C_DATE_TIME_FORMAT) : string.Empty;
        public void SetValue(int? in_value)
        {
            var value = !in_value.HasValue ?
                string.Empty :
                (TreatAsDecimal ?
                    FormatHelper.GetDecimalString(in_value.Value, DecimalPositions) :
                    in_value.Value.ToString());

            Value = value;
        }
        public void SetValue(decimal? in_value) => Value = in_value.HasValue ? in_value.Value.ToString(CultureInfo.CurrentCulture) : string.Empty;
        public void SetValue(string in_value) => Value = in_value ?? string.Empty;
        public void SetValue(Guid? in_value) => Value = in_value.HasValue ? in_value.Value.ToString() : string.Empty;

        void TextValueChanged(object sender, EventArgs e) => ValueChanged?.Invoke(sender, e);
    }
}
