using Ardalis.GuardClauses;
using System;
using System.Globalization;
using System.Windows.Forms;
using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class SelectorField : UserControl
    {
        protected string caption = "Caption";
        protected DbMappedDOT? refObject;

        void B_Clear_Click(object? sender, EventArgs e) => ClearClick?.Invoke(sender, e);
        void B_Select_Click(object? sender, EventArgs e) => SelectClick?.Invoke(sender, e);
        void B_QuickSelect_Click(object? sender, EventArgs e) => QuickSelectClick?.Invoke(sender, e);
        void B_Card_Click(object? sender, EventArgs e) => CardClick?.Invoke(sender, e);
        void OnValueChange() => ValueChanged?.Invoke(this, default!); // TODO: Something wrong here; what can we do to get rid of this "default!" and do it right?

        public SelectorField()
        {
            InitializeComponent();

            CTRL_Buttons.ClearClick += (s, e) => B_Clear_Click(s, e);
            CTRL_Buttons.SelectClick += (s, e) => B_Select_Click(s, e);
            CTRL_Buttons.QuickSelectClick += (s, e) => B_QuickSelect_Click(s, e);
            CTRL_Buttons.CardClick += (s, e) => B_Card_Click(s, e);
        }

        public string Value { get => T_Value.Text; set => T_Value.Text = value; }
        public string Caption { get => caption; set { caption = value; L_Caption.Text = caption + ":"; } }
        public DbMappedDOT? RefObject { get => refObject; set => SetValue(value); }
        public bool EnabledClear { get => CTRL_Buttons.EnabledClear; set => CTRL_Buttons.EnabledClear = value; }
        public bool EnabledSelect { get => CTRL_Buttons.EnabledSelect; set => CTRL_Buttons.EnabledSelect = value; }
        public bool EnabledQuickSelect { get => CTRL_Buttons.EnabledQuickSelect; set => CTRL_Buttons.EnabledQuickSelect = value; }
        public bool EnabledCard { get => CTRL_Buttons.EnabledCard; set => CTRL_Buttons.EnabledCard = value; }
        public bool UseClear { get => CTRL_Buttons.UseClear; set => CTRL_Buttons.UseClear = value; }
        public bool UseSelect { get => CTRL_Buttons.UseSelect; set => CTRL_Buttons.UseSelect = value; }
        public bool UseQuickSelect { get => CTRL_Buttons.UseQuickSelect; set => CTRL_Buttons.UseQuickSelect = value; }
        public bool UseCard { get => CTRL_Buttons.UseCard; set => CTRL_Buttons.UseCard = value; }

        public event EventHandler? ValueChanged;
        public event EventHandler? ClearClick;
        public event EventHandler? SelectClick;
        public event EventHandler? QuickSelectClick;
        public event EventHandler? CardClick;

        public void SetValue(DbMappedDOT? refObject)
        {
            this.refObject = refObject;
            Value = refObject?.ToString() ?? string.Empty;
            OnValueChange();
        }
        public void SetValue(int count)
        {
            Value = count.ToString(CultureInfo.CurrentCulture);
            //OnValueChange();
        }
        public void UpdateValue(DbMappedDOT refObject)
        {
            Guard.Against.Null(refObject, nameof(refObject));

            this.refObject = refObject;
            Value = refObject.ToString()!;
        }
    }
}
