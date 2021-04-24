using System;
using System.Windows.Forms;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class CommandButtons : UserControl
    {
        void B_Clear_Click(object sender, EventArgs e) => ClearClick?.Invoke(sender, e);
        void B_Select_Click(object sender, EventArgs e) => SelectClick?.Invoke(sender, e);
        void B_QuickSelect_Click(object sender, EventArgs e) => QuickSelectClick?.Invoke(sender, e);
        void B_Card_Click(object sender, EventArgs e) => CardClick?.Invoke(sender, e);

        public CommandButtons() => InitializeComponent();

        public bool EnabledClear { get { return B_Clear.Enabled; } set { B_Clear.Enabled = value; } }
        public bool EnabledSelect { get { return B_Select.Enabled; } set { B_Select.Enabled = value; } }
        public bool EnabledQuickSelect { get { return B_QuickSelect.Enabled; } set { B_QuickSelect.Enabled = value; } }
        public bool EnabledCard { get { return B_Card.Enabled; } set { B_Card.Enabled = value; } }
        public bool UseClear { get; set; } = true;
        public bool UseSelect { get; set; } = true;
        public bool UseQuickSelect { get; set; } = true;
        public bool UseCard { get; set; } = true;

        public event EventHandler ClearClick;
        public event EventHandler SelectClick;
        public event EventHandler QuickSelectClick;
        public event EventHandler CardClick;
    }
}
