using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Forms;
using VkRadio.LowCode.Gui.WinForms;

namespace VkRadio.LowCode.TestBed.Generated.Gui.Lists
{
    /// <summary>
    /// Drive Account Object List
    /// </summary>
    public partial class DOLDriveAccount: DOList
    {
        /// <summary>
        /// List constructor
        /// </summary>
        [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Extraction of string literals from resources should be implemented in the future")]
        public DOLDriveAccount()
        {
            InitializeComponent();
        
            DGV_ListProtected.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "name",
                    HeaderText = "Name",
                    Name = "COL_Name",
                    ReadOnly = true,
                    Visible = true,
                    Width = 200,
                    DisplayIndex = 0
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "code",
                    HeaderText = "Code",
                    Name = "COL_Code",
                    ReadOnly = true,
                    Visible = true,
                    Width = 200,
                    DisplayIndex = 1
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "default_value",
                    HeaderText = "Def.val",
                    Name = "COL_DefaultValue",
                    ReadOnly = true,
                    Visible = true,
                    Width = 60,
                    DisplayIndex = 2
                }
            });
            for (var i = 1; i < DGV_ListProtected.Columns.Count; i++)
                DGV_ListProtected.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DGV_ListProtected.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        
            defaultSortFieldIndex = 1;
        
            decimalIntPositions = new int[]
            {
                0,
                0,
                0
            };
        
            DGV_ListProtected.CellFormatting += new DataGridViewCellFormattingEventHandler(DGV_ListProtected_CellFormatting);
        }

        /// <summary>
        /// Formatting handler for some values
        /// </summary>
        void DGV_ListProtected_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
                return;
            var intPositions = decimalIntPositions[e.ColumnIndex - 1];
            if (intPositions != 0)
            {
                var eValue = e.Value as int?;
                if (eValue.HasValue)
                {
                    e.Value = (eValue.Value / 100m).ToString($"N{intPositions}", CultureInfo.CurrentCulture);
                    e.FormattingApplied = true;
                }
            }
            else
            {
                var boolValue = e.Value as bool?;
                if (boolValue.HasValue)
                {
                    e.Value = boolValue.Value ? "yes" : "no";
                    e.FormattingApplied = true;
                }
            }
        }
    }
}
