using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using VkRadio.LowCode.Libs.Orm;

namespace VkRadio.LowCode.Libs.Gui.WinForms
{
    public partial class DOList : UserControl
    {
        protected const string c_noWayNoRowsSelected = "Row not selected.";
        protected const string c_askDeleteObject  = "Do you really want to delete this object?";
        protected const string c_captionFault = "Refusal";
        protected const string c_captionQuestion = "Question";

        protected IDOStorage? _storage;
        protected UILauncher? _uiLauncher;
        protected bool _isSelectable;
        protected FilterAbstract? _filter;
        protected DbParameter[]? _dbParameters;
        protected bool _notSortedYet = true;
        protected int _defaultSortFieldIndex = -1;
        protected bool _defaultSortAscending = true;
        protected int[] _decimalIntPositions = default!;

        protected void UpdateCount()
        {
            L_Count.Text = "Total: " + (DGV_List.DataSource != null ?
                ((DataTable)DGV_List.DataSource).Rows.Count.ToString(CultureInfo.CurrentCulture) :
                "0");
        }

        public virtual void RefreshTable(DataRow? rowToDelete = null)
        {
            if (_storage != null)
            {
                var oldCur = Cursor;
                try
                {
                    Cursor = Cursors.WaitCursor;
                    DGV_List.AutoGenerateColumns = false;

                    if (DGV_List.DataSource == null)
                    {
                        // Here I've added dbParameters, but it will work only on the creation of a new window containing
                        // the list of objects. If in the future there will be a need to extend the functionality -
                        // to apply filters in the already created window, then probably it won't work, probably old
                        // rows won't be deleting on filter apply.
                        DGV_List.DataSource = _storage.ReadAsTable(_filter, _dbParameters);
                    }
                    else
                    {
                        _storage.RefreshTable(_filter, (DataTable)DGV_List.DataSource);

                        if (rowToDelete != null)
                            ((DataTable)DGV_ListProtected.DataSource).Rows.Remove(rowToDelete);
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
                    Cursor = oldCur;
                }
            }
        }

        protected Guid? GetCurrentId()
        {
            if (DGV_List.Rows.GetRowCount(DataGridViewElementStates.Selected) <= 0)
                return null;
            var row = DGV_List.SelectedRows[0];
            return Libs.Orm.DbProviderFactory.Default.GuidStyle == GuidStyle.AsMs ? (Guid)row.Cells["COL_Id"].Value : new Guid((byte[])row.Cells["COL_Id"].Value);
        }

        protected DbMappedDOT? GetSelectedObject(bool ignoreEmptyClick)
        {
            var id = GetCurrentId();
            if (!id.HasValue)
            {
                if (!ignoreEmptyClick)
                    MessageBox.Show(this, c_noWayNoRowsSelected, c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }

            var o = _storage!.Restore(id.Value);
            if (o == null)
                MessageBox.Show(this, "Object no longer exists.", c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return o;
        }

        protected bool ShowCard(bool ignoreEmptyClick)
        {
            var o = GetSelectedObject(ignoreEmptyClick);
            if (o != null)
            {
                o = (DbMappedDOT)o.Clone();

                using var frm = _uiLauncher!.CreateCard(o);
                ((Form)frm).ShowDialog(this);
                return frm.Changed;
            }
            else
            {
                return false;
            }
        }

        protected void InitializeComponentProtected() => InitializeComponent();

        protected DataGridView DGV_ListProtected => DGV_List;

        protected Label L_CountProtected => L_Count;

        public DOList() => InitializeComponent();

        protected virtual DbMappedDOT SetupNewObject(DbMappedDOT obj) => obj;

        void DOList_Load(object sender, EventArgs e) { }

        void B_Refresh_Click(object sender, EventArgs e) => RefreshTable();

        void B_Card_Click(object sender, EventArgs e)
        {
            if (ShowCard(false))
                RefreshTable();
        }

        void DGV_List_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_isSelectable)
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
            var o = SetupNewObject(_storage!.CreateNew());
            using var frm = _uiLauncher!.CreateCard(o);
            ((Form)frm).ShowDialog(this);
            if (frm.Changed)
                RefreshTable();
        }

        [SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "We have no dangerous security functionality here")]
        public void B_Delete_Click(object sender, EventArgs e)
        {
            var id = GetCurrentId();

            if (!id.HasValue)
            {
                MessageBox.Show(this, c_noWayNoRowsSelected, c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (_storage!.PredefinedObjects.ContainsKey(id.Value))
            {
                MessageBox.Show(this, _storage.GetMessageCannotDeletePredefinedObject(), c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show(this, c_askDeleteObject, c_captionQuestion, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                string? error;
                try
                {
                    error = _storage.Delete(id.Value, true);
                }
                catch (ZeroRowsAffectedException)
                {
                    MessageBox.Show(this, "Object no longer exists.", c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (error == null)
                {
                    var table = (DataTable)DGV_List.DataSource;
                    for (var i = 0; i < table.Rows.Count; i++)
                    {
                        var row = table.Rows[i];
                        var idOfRow = Libs.Orm.DbProviderFactory.Default.GuidStyle == GuidStyle.AsMs ? (Guid)row[0] : new Guid((byte[])row[0]);
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
            if (_isSelectable)
            {
                var selObj = GetSelectedObject(true);
                if (selObj != null)
                {
                    selObj = (DbMappedDOT)selObj.Clone();
                    Pick?.Invoke(sender, new SelectedObjectEventArgs(selObj!));
                }
            }
        }

        void B_Filter_Click(object sender, EventArgs e)
        {
            var filterFrm = _uiLauncher!.CreateFilterForm();
            if (filterFrm == null)
            {
                MessageBox.Show(this, "Filter for this type of objects is not provided.", "Refusal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "We will fix it in the future task of localization")]
        void B_Export_Click(object sender, EventArgs e)
        {
            if (DGV_List.Rows.Count == 0)
            {
                MessageBox.Show(this, "Table is empty.", "Refusal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var textLines = new List<string>();
            var header = string.Empty;
            for (var i = 1; i < DGV_List.Columns.Count; i++)
            {
                var col = DGV_List.Columns[i];
                if (!string.IsNullOrEmpty(header))
                    header += ";";
                header += "\"" + col.DataPropertyName + "\"";
            }
            textLines.Add(header);
            foreach (DataGridViewRow row in DGV_List.Rows)
            {
                var textRow = string.Empty;
                for (var i = 1; i < row.Cells.Count; i++)
                {
                    var cell = row.Cells[i];

                    if (i != 1)
                        textRow += ";";

                    if (cell.ValueType == typeof(string))
                        textRow += "\"" + ((cell.Value as string) ?? string.Empty).Replace("\"", "\"\"") + "\"";
                    else
                        textRow += cell.Value.ToString();
                }
                textLines.Add(textRow);
            }

            using var dlg = new SaveFileDialog();
            dlg.CheckPathExists = true;
            dlg.AddExtension = true;
            dlg.AutoUpgradeEnabled = true;
            dlg.DefaultExt = "csv";
            dlg.DereferenceLinks = true;
            dlg.Filter = "Comma-separated values|*.csv|All files|*.*";
            dlg.FilterIndex = 0;
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            dlg.OverwritePrompt = true;
            dlg.Title = "Saving the table";
            dlg.ValidateNames = true;
            dlg.FileName = _uiLauncher!.DotName;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                    File.Delete(dlg.FileName);
                using (var file = new StreamWriter(dlg.FileName, false, Encoding.UTF8))
                {
                    foreach (string str in textLines)
                        file.WriteLine(str);
                }
                MessageBox.Show(this, "Table saved to CSV file.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public virtual void Init(IDOStorage storage, UILauncher uiLauncher, FilterAbstract? filter, int? defaultSortFieldIndex = null, bool? defaultSortAscending = null, DbParameter[]? dbParameters = null)
        {
            Guard.Against.Null(storage, nameof(storage));
            Guard.Against.Null(uiLauncher, nameof(uiLauncher));

            this._storage = storage;
            this._uiLauncher = uiLauncher;
            this._filter = filter;
            this._dbParameters = dbParameters;
            if (defaultSortFieldIndex.HasValue)
                this._defaultSortFieldIndex = defaultSortFieldIndex.Value;
            if (defaultSortAscending.HasValue)
                this._defaultSortAscending = defaultSortAscending.Value;
            RefreshTable();
        }

        public bool Selectable
        {
            get => _isSelectable;
            set
            {
                _isSelectable = value;
                B_Pick.Enabled = _isSelectable;
                B_Pick.Visible = _isSelectable;
            }
        }

        public DataGridView DGV_ListPublic => DGV_List;

        public event EventHandler<SelectedObjectEventArgs>? Pick;
    };
}
