using System;
using System.Windows.Forms;
using System.Globalization;
using VkRadio.LowCode.Gui.Utils;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class StringField : UserControl
    {
        protected string caption = "Caption";

        public StringField() => InitializeComponent();

        public string Value { get => T_Value.Text; set => T_Value.Text = value; }
        public string Caption { get => caption; set { caption = value; L_Caption.Text = caption + ":"; } }
        public bool Multiline { get => T_Value.Multiline; set { T_Value.Multiline = value; if (value) T_Value.ScrollBars = ScrollBars.Vertical; } }
        public bool TreatAsDecimal { get; set; }
        public int DecimalPositions { get; set; } = 2;

        public event EventHandler? ValueChanged;

        public DateTime? GetValueAsDateTimeNullable() => GetValueAsDateTimeNullable(true, out _);

        public DateTime? GetValueAsDateTimeNullable(bool ignoreError, out string? errorMessage) => GetValueAsDateTimeNullable(Value, ignoreError, out errorMessage);

        public static DateTime? GetValueAsDateTimeNullable(string inputValue, bool ignoreError, out string? errorMessage)
        {
            errorMessage = null;
            var success = DateTime.TryParse(inputValue, out DateTime result);
            if (success)
            {
                return result;
            }
            else
            {
                if (ignoreError)
                {
                    errorMessage = "String is in invalid format.";
                    return null;
                }
                else
                {
                    result = DateTime.ParseExact(inputValue, FormatHelper.C_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                    return result; // We won't get here because there will be an Exception on the previous step.
                }
            }
        }

        public static DateTime GetValueAsDateTime(string inputValue, DateTime oldValue) => GetValueAsDateTimeNullable(inputValue, true, out _) ?? oldValue;

        public DateTime GetValueAsDateTime(DateTime oldValue) => GetValueAsDateTime(Value, oldValue);

        public DateTime GetValueAsDateTime(DateTime oldValue, bool ignoreError, out string? errorMessage)
        {
            var result = GetValueAsDateTimeNullable(true, out errorMessage);
            if (!result.HasValue)
            {
                if (ignoreError)
                {
                    return oldValue;
                }
                else
                {
                    result = DateTime.ParseExact(Value, FormatHelper.C_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                    return result.Value; // We won't get here because there will be an Exception on the previous step.
                }
            }
            else
            {
                return result.Value;
            }
        }

        public string GetValueAsString() => GetValueAsStringNullable() ?? string.Empty;

        public string? GetValueAsStringNullable() => string.IsNullOrWhiteSpace(Value) ? null : Value.Trim();

        public int GetValueAsInt(int oldValue)
        {
            int result;

            if (TreatAsDecimal)
            {
                var decimalValue = GetValueAsDecimal(oldValue);
                for (var i = 0; i < DecimalPositions; i++)
                    decimalValue *= 10;
                result = (int)decimalValue;
            }
            else
            {
                result = int.TryParse(Value, out result) ? result : oldValue;
            }

            return result;
        }

        public static int? GetValueAsIntNullable(string inputValue) => int.TryParse(inputValue, out int result) ? result : null;

        public int? GetValueAsIntNullable()
        {
            int? result = null;

            if (TreatAsDecimal)
            {
                var decimalValue = GetValueAsDecimalNullable();
                if (decimalValue.HasValue)
                {
                    var dec2 = decimalValue.Value;
                    for (var i = 0; i < DecimalPositions; i++)
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

        public decimal GetValueAsDecimal(decimal oldValue) => decimal.TryParse(Value, out decimal result) ? result : oldValue;

        public decimal? GetValueAsDecimalNullable() => decimal.TryParse(Value, out decimal result) ? result : null;

        public Guid GetValueAsGuid(Guid value) => GetValueAsGuidNullable() ?? value;

        public Guid? GetValueAsGuidNullable() => Guid.TryParse(Value, out Guid result) ? result : null;

        public void SetValue(DateTime? inputValue) => Value = inputValue.HasValue ? inputValue.Value.ToString(FormatHelper.C_DATE_TIME_FORMAT, CultureInfo.CurrentCulture) : string.Empty;

        public void SetValue(int? inputValue)
        {
            var value = !inputValue.HasValue ?
                string.Empty :
                (TreatAsDecimal ?
                    FormatHelper.GetDecimalString(inputValue.Value, DecimalPositions) :
                    inputValue.Value.ToString(CultureInfo.InvariantCulture));

            Value = value;
        }

        public void SetValue(decimal? inputValue) => Value = inputValue?.ToString(CultureInfo.CurrentCulture) ?? string.Empty;

        public void SetValue(string? inputValue) => Value = inputValue ?? string.Empty;

        public void SetValue(Guid? inputValue) => Value = inputValue?.ToString() ?? string.Empty;

        void TextValueChanged(object sender, EventArgs e) => ValueChanged?.Invoke(sender, e);
    }
}
