using Ardalis.GuardClauses;
using System;
using System.Windows.Forms;
using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class DOListForm : Form
    {
        protected string dotName = default!;
        protected IDOStorage storage = default!;
        protected DOList doList = default!;
        protected bool selectMode;
        protected DbMappedDOT? selectedValue;

        protected void ValueSelected(SelectedObjectEventArgs eventArgs)
        {
            Guard.Against.Null(eventArgs, nameof(eventArgs));

            Pick?.Invoke(this, eventArgs);
            selectedValue = eventArgs.SelectedObject;
            DialogResult = DialogResult.OK;
        }

        public DOListForm() => InitializeComponent();
        public DOListForm(string dotName, IDOStorage storage, bool selectMode) : this()
        {
            this.dotName = dotName;
            this.storage = storage;
            this.selectMode = selectMode;

            base.Text = "List of objects " + this.dotName;
        }

        public virtual void InitByDOList(DOList doList)
        {
            this.doList = doList;
            this.doList.Dock = DockStyle.Fill;
            Controls.Add(this.doList);

            if (selectMode)
            {
                this.doList.Selectable = true;
                this.doList.Pick += (s, e) => ValueSelected(e);
            }
        }

        public bool Selectable { get => doList?.Selectable ?? false; set { if (doList != null) doList.Selectable = value; } }
        public DbMappedDOT? SelectedValue { get { return selectedValue; } }

        public event EventHandler<SelectedObjectEventArgs>? Pick;

        void FRM_DOList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    e.SuppressKeyPress = true;
                    Close();
                    break;
                case Keys.F5:
                    e.SuppressKeyPress = true;
                    doList.RefreshTable();
                    break;
                case Keys.Delete:
                    e.SuppressKeyPress = true;
                    doList.B_Delete_Click(sender, e);
                    break;
            }
        }
    }
}
