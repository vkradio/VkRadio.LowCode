using System;
using System.Windows.Forms;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class CommandButtons : UserControl
    {
        protected bool _useClear = true;
        protected bool _useSelect = true;
        protected bool _useQuickSelect = true;
        protected bool _useCard = true;

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

        public CommandButtons()
        {
            InitializeComponent();
        }

        public bool EnabledClear { get { return B_Clear.Enabled; } set { B_Clear.Enabled = value; } }
        public bool EnabledSelect { get { return B_Select.Enabled; } set { B_Select.Enabled = value; } }
        public bool EnabledQuickSelect { get { return B_QuickSelect.Enabled; } set { B_QuickSelect.Enabled = value; } }
        public bool EnabledCard { get { return B_Card.Enabled; } set { B_Card.Enabled = value; } }
        public bool UseClear { get { return _useClear; } set { _useClear = value; } }
        public bool UseSelect { get { return _useSelect; } set { _useSelect = value; } }
        public bool UseQuickSelect { get { return _useQuickSelect; } set { _useQuickSelect = value; } }
        public bool UseCard { get { return _useCard; } set { _useCard = value; } }

        public event EventHandler ClearClick;
        public event EventHandler SelectClick;
        public event EventHandler QuickSelectClick;
        public event EventHandler CardClick;
    }
}
