using System;
using System.Windows.Forms;
using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class SelectorField : UserControl
    {
        protected string _caption = "Caption";
        protected DbMappedDOT _refObject;

        void B_Clear_Click(object sender, EventArgs e)
        {
            if (ClearClick != null)
                ClearClick(sender, e);
        }
        void B_Select_Click(object sender, EventArgs e)
        {
            if (SelectClick != null)
                SelectClick(sender, e);
        }
        void B_QuickSelect_Click(object sender, EventArgs e)
        {
            if (QuickSelectClick != null)
                QuickSelectClick(sender, e);
        }
        void B_Card_Click(object sender, EventArgs e)
        {
            if (CardClick != null)
                CardClick(sender, e);
        }
        void OnValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        public SelectorField()
        {
            InitializeComponent();

            CTRL_Buttons.ClearClick += (s, e) => B_Clear_Click(s, e);
            CTRL_Buttons.SelectClick += (s, e) => B_Select_Click(s, e);
            CTRL_Buttons.QuickSelectClick += (s, e) => B_QuickSelect_Click(s, e);
            CTRL_Buttons.CardClick += (s, e) => B_Card_Click(s, e);
        }

        public string Value { get { return T_Value.Text; } set { T_Value.Text = value; } }
        public string Caption { get { return _caption; } set { _caption = value; L_Caption.Text = _caption + ":"; } }
        public DbMappedDOT RefObject { get { return _refObject; } set { SetValue(value); } }
        public bool EnabledClear { get { return CTRL_Buttons.EnabledClear; } set { CTRL_Buttons.EnabledClear = value; } }
        public bool EnabledSelect { get { return CTRL_Buttons.EnabledSelect; } set { CTRL_Buttons.EnabledSelect = value; } }
        public bool EnabledQuickSelect { get { return CTRL_Buttons.EnabledQuickSelect; } set { CTRL_Buttons.EnabledQuickSelect = value; } }
        public bool EnabledCard { get { return CTRL_Buttons.EnabledCard; } set { CTRL_Buttons.EnabledCard = value; } }
        public bool UseClear { get { return CTRL_Buttons.UseClear; } set { CTRL_Buttons.UseClear = value; } }
        public bool UseSelect { get { return CTRL_Buttons.UseSelect; } set { CTRL_Buttons.UseSelect = value; } }
        public bool UseQuickSelect { get { return CTRL_Buttons.UseQuickSelect; } set { CTRL_Buttons.UseQuickSelect = value; } }
        public bool UseCard { get { return CTRL_Buttons.UseCard; } set { CTRL_Buttons.UseCard = value; } }

        public event EventHandler ValueChanged;
        public event EventHandler ClearClick;
        public event EventHandler SelectClick;
        public event EventHandler QuickSelectClick;
        public event EventHandler CardClick;

        public void SetValue(DbMappedDOT in_refObject)
        {
            _refObject = in_refObject;
            Value = in_refObject != null ? in_refObject.ToString() : string.Empty;
            OnValueChange();
        }
        public void SetValue(int in_count)
        {
            Value = in_count.ToString();
            //OnValueChange();
        }
        public void UpdateValue(DbMappedDOT in_refObject)
        {
            _refObject = in_refObject;
            Value = in_refObject != null ? in_refObject.ToString() : string.Empty;
        }
    }
}
