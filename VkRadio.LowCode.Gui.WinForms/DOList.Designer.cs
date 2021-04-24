namespace VkRadio.LowCode.Gui.WinForms
{
    partial class DOList
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.B_Create = new System.Windows.Forms.Button();
            this.B_Card = new System.Windows.Forms.Button();
            this.B_Refresh = new System.Windows.Forms.Button();
            this.B_Delete = new System.Windows.Forms.Button();
            this.DGV_List = new System.Windows.Forms.DataGridView();
            this.COL_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.B_Pick = new System.Windows.Forms.Button();
            this.B_Filter = new System.Windows.Forms.Button();
            this.L_Count = new System.Windows.Forms.Label();
            this.B_Export = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_List)).BeginInit();
            this.SuspendLayout();
            // 
            // B_Create
            // 
            this.B_Create.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Create.Location = new System.Drawing.Point(397, 449);
            this.B_Create.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Create.Name = "B_Create";
            this.B_Create.Size = new System.Drawing.Size(115, 27);
            this.B_Create.TabIndex = 9;
            this.B_Create.Text = "Create";
            this.B_Create.UseVisualStyleBackColor = true;
            this.B_Create.Click += new System.EventHandler(this.B_Create_Click);
            // 
            // B_Card
            // 
            this.B_Card.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Card.Location = new System.Drawing.Point(519, 449);
            this.B_Card.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Card.Name = "B_Card";
            this.B_Card.Size = new System.Drawing.Size(115, 27);
            this.B_Card.TabIndex = 8;
            this.B_Card.Text = "Card";
            this.B_Card.UseVisualStyleBackColor = true;
            this.B_Card.Click += new System.EventHandler(this.B_Card_Click);
            // 
            // B_Refresh
            // 
            this.B_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Refresh.Location = new System.Drawing.Point(642, 449);
            this.B_Refresh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Refresh.Name = "B_Refresh";
            this.B_Refresh.Size = new System.Drawing.Size(115, 27);
            this.B_Refresh.TabIndex = 7;
            this.B_Refresh.Text = "Refresh";
            this.B_Refresh.UseVisualStyleBackColor = true;
            this.B_Refresh.Click += new System.EventHandler(this.B_Refresh_Click);
            // 
            // B_Delete
            // 
            this.B_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Delete.Location = new System.Drawing.Point(764, 449);
            this.B_Delete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Delete.Name = "B_Delete";
            this.B_Delete.Size = new System.Drawing.Size(115, 27);
            this.B_Delete.TabIndex = 6;
            this.B_Delete.Text = "Delete";
            this.B_Delete.UseVisualStyleBackColor = true;
            this.B_Delete.Click += new System.EventHandler(this.B_Delete_Click);
            // 
            // DGV_List
            // 
            this.DGV_List.AllowUserToAddRows = false;
            this.DGV_List.AllowUserToDeleteRows = false;
            this.DGV_List.AllowUserToOrderColumns = true;
            this.DGV_List.AllowUserToResizeRows = false;
            this.DGV_List.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_List.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COL_Id});
            this.DGV_List.Location = new System.Drawing.Point(4, 0);
            this.DGV_List.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DGV_List.MultiSelect = false;
            this.DGV_List.Name = "DGV_List";
            this.DGV_List.ReadOnly = true;
            this.DGV_List.RowHeadersVisible = false;
            this.DGV_List.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_List.Size = new System.Drawing.Size(875, 442);
            this.DGV_List.TabIndex = 5;
            this.DGV_List.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_List_CellDoubleClick);
            // 
            // COL_Id
            // 
            this.COL_Id.DataPropertyName = "id";
            this.COL_Id.HeaderText = "Id";
            this.COL_Id.Name = "COL_Id";
            this.COL_Id.ReadOnly = true;
            this.COL_Id.Visible = false;
            // 
            // B_Pick
            // 
            this.B_Pick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Pick.Enabled = false;
            this.B_Pick.Location = new System.Drawing.Point(274, 449);
            this.B_Pick.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Pick.Name = "B_Pick";
            this.B_Pick.Size = new System.Drawing.Size(115, 27);
            this.B_Pick.TabIndex = 10;
            this.B_Pick.Text = "Select";
            this.B_Pick.UseVisualStyleBackColor = true;
            this.B_Pick.Visible = false;
            this.B_Pick.Click += new System.EventHandler(this.B_Pick_Click);
            // 
            // B_Filter
            // 
            this.B_Filter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Filter.Location = new System.Drawing.Point(764, 482);
            this.B_Filter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Filter.Name = "B_Filter";
            this.B_Filter.Size = new System.Drawing.Size(115, 27);
            this.B_Filter.TabIndex = 11;
            this.B_Filter.Text = "Filter";
            this.B_Filter.UseVisualStyleBackColor = true;
            this.B_Filter.Click += new System.EventHandler(this.B_Filter_Click);
            // 
            // L_Count
            // 
            this.L_Count.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.L_Count.AutoSize = true;
            this.L_Count.Location = new System.Drawing.Point(4, 492);
            this.L_Count.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.L_Count.Name = "L_Count";
            this.L_Count.Size = new System.Drawing.Size(38, 15);
            this.L_Count.TabIndex = 12;
            this.L_Count.Text = "label1";
            // 
            // B_Export
            // 
            this.B_Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Export.Location = new System.Drawing.Point(642, 482);
            this.B_Export.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Export.Name = "B_Export";
            this.B_Export.Size = new System.Drawing.Size(115, 27);
            this.B_Export.TabIndex = 13;
            this.B_Export.Text = "Export";
            this.B_Export.UseVisualStyleBackColor = true;
            this.B_Export.Click += new System.EventHandler(this.B_Export_Click);
            // 
            // DOList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.B_Export);
            this.Controls.Add(this.L_Count);
            this.Controls.Add(this.B_Filter);
            this.Controls.Add(this.B_Pick);
            this.Controls.Add(this.B_Create);
            this.Controls.Add(this.B_Card);
            this.Controls.Add(this.B_Refresh);
            this.Controls.Add(this.B_Delete);
            this.Controls.Add(this.DGV_List);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DOList";
            this.Size = new System.Drawing.Size(882, 516);
            this.Load += new System.EventHandler(this.DOList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_List)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_Create;
        private System.Windows.Forms.Button B_Card;
        private System.Windows.Forms.Button B_Refresh;
        private System.Windows.Forms.Button B_Delete;
        private System.Windows.Forms.DataGridView DGV_List;
        private System.Windows.Forms.DataGridViewTextBoxColumn COL_Id;
        private System.Windows.Forms.Button B_Pick;
        private System.Windows.Forms.Button B_Filter;
        private System.Windows.Forms.Label L_Count;
        private System.Windows.Forms.Button B_Export;
    }
}
