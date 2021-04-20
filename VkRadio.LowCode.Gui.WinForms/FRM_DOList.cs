using System;
using System.Windows.Forms;
using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class FRM_DOList : Form
    {
        protected string _dotName;
        protected IDOStorage _storage;
        protected DOList _doList;
        protected bool _selectMode;
        protected DbMappedDOT _selectedValue;

        protected void ValueSelected(SelectedObjectEventArgs in_ea)
        {
            if (Pick != null)
                Pick(this, in_ea);
            _selectedValue = in_ea.SelectedObject;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public FRM_DOList()
        {
            InitializeComponent();
        }
        public FRM_DOList(string in_dotName, IDOStorage in_storage, bool in_selectMode) : this()
        {
            _dotName = in_dotName;
            _storage = in_storage;
            _selectMode = in_selectMode;

            Text = "Список объектов " + _dotName;
        }

        public virtual void InitByDOList(DOList in_doList)
        {
            _doList = in_doList;
            _doList.Dock = DockStyle.Fill;
            Controls.Add(_doList);

            if (_selectMode)
            {
                _doList.Selectable = true;
                _doList.Pick += (s, e) => ValueSelected(e);
            }
        }

        public bool Selectable { get { return _doList != null ? _doList.Selectable : false; } set { if (_doList != null) _doList.Selectable = value; } }
        public DbMappedDOT SelectedValue { get { return _selectedValue; } }

        public event EventHandler<SelectedObjectEventArgs> Pick;

        private void FRM_DOList_KeyDown(object sender, KeyEventArgs e)
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
