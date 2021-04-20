using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Windows.Forms;
using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class DOList : UserControl
    {
        protected const string c_noWayNoRowsSelected  = "Не выбрана строка.";
        protected const string c_askDeleteObject      = "Вы действительно хотите удалить объект?";
        protected const string c_captionFault         = "Отказ";
        protected const string c_captionQuestion      = "Вопрос";

        protected IDOStorage _storage;
        protected UILauncher _uiLauncher;
        protected bool _selectable;
        protected FilterAbstract _filter;
        protected DbParameter[] _params;
        protected bool _notSortedYet = true;
        protected int _defaultSortFieldIndex = -1;
        protected bool _defaultSortAscending = true;
        protected int[] _decimalIntPositions;

        protected void UpdateCount()
        {
            L_Count.Text = "Всего: " + (DGV_List.DataSource != null ?
                ((DataTable)DGV_List.DataSource).Rows.Count.ToString() :
                "0");
        }

        public virtual void RefreshTable(DataRow in_rowToDelete = null)
        {
            if (_storage != null)
            {
                Cursor oldCur = this.Cursor;
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    DGV_List.AutoGenerateColumns = false;

                    if (DGV_List.DataSource == null)
                    {
                        // Здесь добавил _params, но это будет работать только при создании нового окна со списком
                        // объектов. Если же нужно будет в будущем расширить функционал - в уже созданном окне применять
                        // фильтры отбора, то скорее всего это работать не будет - скорее всего старые строки в таблице
                        // не будут отсекаться при применении фильтра.
                        DGV_List.DataSource = _storage.ReadAsTable(_filter, _params);
                    }
                    else
                    {
                        _storage.RefreshTable(_filter, (DataTable)DGV_List.DataSource);

                        if (in_rowToDelete != null)
                            ((DataTable)DGV_ListProtected.DataSource).Rows.Remove(in_rowToDelete);
                    }

                    if (_defaultSortFieldIndex != -1)
                    {
                        if (_notSortedYet ||
                            (DGV_List.SortedColumn.Index == _defaultSortFieldIndex && DGV_List.SortOrder == (_defaultSortAscending ? SortOrder.Ascending : SortOrder.Descending)))
                        {
                            DGV_List.Sort(DGV_List.Columns[_defaultSortFieldIndex], _defaultSortAscending ? System.ComponentModel.ListSortDirection.Ascending : System.ComponentModel.ListSortDirection.Descending);
                            DGV_List.Columns[_defaultSortFieldIndex].HeaderCell.SortGlyphDirection = _defaultSortAscending ? SortOrder.Ascending : SortOrder.Descending;
                            _notSortedYet = false;
                        }
                    }

                    UpdateCount();
                }
                finally
                {
                    this.Cursor = oldCur;
                }
            }
        }
        protected Guid? GetCurrentId()
        {
            if (DGV_List.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
                return null;
            DataGridViewRow row = DGV_List.SelectedRows[0];
            return Orm.DbProviderFactory.Default.GuidStyle == GuidStyle.AsMs ? (Guid)row.Cells["COL_Id"].Value : new Guid((byte[])row.Cells["COL_Id"].Value);
        }
        protected DbMappedDOT GetSelectedObject(bool in_ignoreEmptyClick)
        {
            Guid? id = GetCurrentId();
            if (!id.HasValue)
            {
                if (!in_ignoreEmptyClick)
                    MessageBox.Show(this, c_noWayNoRowsSelected, c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }

            DbMappedDOT o = _storage.Restore(id.Value);
            if (o == null)
            {
                MessageBox.Show(this, "Объект больше не существует.", c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            return o;
        }
        protected bool ShowCard(bool in_ignoreEmptyClick)
        {
            DbMappedDOT o = GetSelectedObject(in_ignoreEmptyClick);
            if (o != null)
            {
                o = (DbMappedDOT)o.Clone();
            
                using (IFRM_Card frm = _uiLauncher.CreateCard(o))
                {
                    ((Form)frm).ShowDialog(this);
                    return frm.Changed;
                }
            }
            else
            {
                return false;
            }
        }

        protected void InitializeComponentProtected() { InitializeComponent(); }

        protected DataGridView DGV_ListProtected { get { return DGV_List; } }
        protected Label L_CountProtected { get { return L_Count; } }

        public DOList() { InitializeComponent(); }

        protected virtual DbMappedDOT SetupNewObject(DbMappedDOT in_o) { return in_o; }

        void DOList_Load(object sender, EventArgs e) {}
        void B_Refresh_Click(object sender, EventArgs e) { RefreshTable(); }
        void B_Card_Click(object sender, EventArgs e)
        {
            if (ShowCard(false))
                RefreshTable();
        }
        void DGV_List_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_selectable)
            {
                B_Pick_Click(sender, e);
            }
            else
            {
                if (ShowCard(true))
                    RefreshTable();
            }
        }
        void B_Create_Click(object sender, EventArgs e)
        {
            DbMappedDOT o = SetupNewObject(_storage.CreateNew());
            using (IFRM_Card frm = _uiLauncher.CreateCard(o))
            {
                ((Form)frm).ShowDialog(this);
                if (frm.Changed)
                    RefreshTable();
            }
        }
        public void B_Delete_Click(object sender, EventArgs e)
        {
            Guid? id = GetCurrentId();

            if (!id.HasValue)
            {
                MessageBox.Show(this, c_noWayNoRowsSelected, c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (_storage.PredefinedObjects.ContainsKey(id.Value))
            {
                MessageBox.Show(this, _storage.GetMessageCannotDeletePredefinedObject(), c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show(this, c_askDeleteObject, c_captionQuestion, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                string error = null;
                try
                {
                    error = _storage.Delete(id.Value, true);
                }
                catch (ZeroRowsAffectedException)
                {
                    MessageBox.Show(this, "Объект больше не существует.", c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (error == null)
                {
                    DataTable table = (DataTable)DGV_List.DataSource;
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow row = table.Rows[i];
                        Guid idOfRow = Orm.DbProviderFactory.Default.GuidStyle == GuidStyle.AsMs ? (Guid)row[0] : new Guid((byte[])row[0]);
                        if (idOfRow == id.Value)
                        {
                            table.Rows.RemoveAt(i);
                            break;
                        }
                    }
                    RefreshTable();
                }
                else
                {
                    MessageBox.Show(this, error, c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        void B_Pick_Click(object sender, EventArgs e)
        {
            if (_selectable)
            {
                DbMappedDOT selObj = GetSelectedObject(true);
                if (selObj != null)
                {
                    selObj = (DbMappedDOT)selObj.Clone();
                    Pick(sender, new SelectedObjectEventArgs(selObj));
                }
            }
        }
        void B_Filter_Click(object sender, EventArgs e)
        {
            object filterFrm = _uiLauncher.CreateFilterForm();
            if (filterFrm == null)
            {
                MessageBox.Show(this, "Фильтр для этого типа объектов не предусмотрен.", "Отказ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        void B_Export_Click(object sender, EventArgs e)
        {
            if (DGV_List.Rows.Count == 0)
            {
                MessageBox.Show(this, "Таблица пуста.", "Отказ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<string> textLines = new List<string>();
            string header = string.Empty;
            for (int i = 1; i < DGV_List.Columns.Count; i++)
            {
                DataGridViewColumn col = DGV_List.Columns[i];
                if (header != string.Empty)
                    header += ";";
                header += "\"" + col.DataPropertyName + "\"";
            }
            textLines.Add(header);
            foreach (DataGridViewRow row in DGV_List.Rows)
            {
                string textRow = string.Empty;
                for (int i = 1; i < row.Cells.Count; i++)
                {
                    DataGridViewCell cell = row.Cells[i];

                    if (i != 1)
                        textRow += ";";

                    if (cell.ValueType == typeof(string))
                        textRow += "\"" + ((cell.Value as string) ?? string.Empty).Replace("\"", "\"\"") + "\"";
                    else
                        textRow += cell.Value.ToString();
                }
                textLines.Add(textRow);
            }

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.CheckPathExists = true;
                dlg.AddExtension = true;
                dlg.AutoUpgradeEnabled = true;
                dlg.DefaultExt = "csv";
                dlg.DereferenceLinks = true;
                dlg.Filter = "Comma-separated values|*.csv|All files|*.*";
                dlg.FilterIndex = 0;
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                dlg.OverwritePrompt = true;
                dlg.Title = "Сохранение таблицы";
                dlg.ValidateNames = true;
                dlg.FileName = _uiLauncher.DotName;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (File.Exists(dlg.FileName))
                        File.Delete(dlg.FileName);
                    using (StreamWriter file = new StreamWriter(dlg.FileName, false, Encoding.UTF8))
                    {
                        foreach (string str in textLines)
                            file.WriteLine(str);
                    }
                    MessageBox.Show(this, "Таблица сохранена в файл CSV.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public virtual void Init(IDOStorage in_storage, UILauncher in_uiLauncher, FilterAbstract in_filter, int? in_defaultSortFieldIndex = null, bool? in_defaultSortAscending = null, DbParameter[] in_params = null)
        {
            _storage = in_storage;
            _uiLauncher = in_uiLauncher;
            _filter = in_filter;
            _params = in_params;
            if (in_defaultSortFieldIndex.HasValue)
                _defaultSortFieldIndex = in_defaultSortFieldIndex.Value;
            if (in_defaultSortAscending.HasValue)
                _defaultSortAscending = in_defaultSortAscending.Value;
            RefreshTable();
        }

        public bool Selectable
        {
            get { return _selectable; }
            set
            {
                _selectable = value;
                B_Pick.Enabled = _selectable;
                B_Pick.Visible = _selectable;
            }
        }
        public DataGridView DGV_ListPublic { get { return DGV_List; } }

        public event EventHandler<SelectedObjectEventArgs> Pick;
    };
}
