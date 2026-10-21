using Ardalis.GuardClauses;
using System;
using System.Windows.Forms;
using VkRadio.LowCode.Libs.Orm;

namespace VkRadio.LowCode.Libs.Gui.WinForms
{
    public partial class DOListForm : Form
    {
        protected string _dotName = default!;
        protected IDOStorage _storage = default!;
        protected DOList _doList = default!;
        protected bool _selectMode;
        protected DbMappedDOT? _selectedValue;

        protected void ValueSelected(SelectedObjectEventArgs eventArgs)
        {
            Guard.Against.Null(eventArgs, nameof(eventArgs));

            Pick?.Invoke(this, eventArgs);
            _selectedValue = eventArgs.SelectedObject;
            DialogResult = DialogResult.OK;
        }

        public DOListForm() => InitializeComponent();
        public DOListForm(string dotName, IDOStorage storage, bool selectMode) : this()
        {
            _dotName = dotName;
            _storage = storage;
            _selectMode = selectMode;

            base.Text = "List of objects " + this._dotName;
        }

        public virtual void InitByDOList(DOList doList)
        {
            _doList = doList;
            _doList.Dock = DockStyle.Fill;
            Controls.Add(this._doList);

            if (_selectMode)
            {
                _doList.Selectable = true;
                _doList.Pick += (s, e) => ValueSelected(e);
            }
        }

        public bool Selectable { get => _doList?.Selectable ?? false; set { if (_doList != null) _doList.Selectable = value; } }
        public DbMappedDOT? SelectedValue { get { return _selectedValue; } }

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
                    _doList.RefreshTable();
                    break;
                case Keys.Delete:
                    e.SuppressKeyPress = true;
                    _doList.B_Delete_Click(sender, e);
                    break;
            }
        }
    }
}
