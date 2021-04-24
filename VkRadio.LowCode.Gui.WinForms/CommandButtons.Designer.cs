namespace VkRadio.LowCode.Gui.WinForms
{
    partial class CommandButtons
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
            this.components = new System.ComponentModel.Container();
            this.B_Clear = new System.Windows.Forms.Button();
            this.TT_Main = new System.Windows.Forms.ToolTip(this.components);
            this.B_Select = new System.Windows.Forms.Button();
            this.B_Card = new System.Windows.Forms.Button();
            this.B_QuickSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // B_Clear
            // 
            this.B_Clear.Dock = System.Windows.Forms.DockStyle.Left;
            this.B_Clear.Location = new System.Drawing.Point(0, 0);
            this.B_Clear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Clear.Name = "B_Clear";
            this.B_Clear.Size = new System.Drawing.Size(26, 22);
            this.B_Clear.TabIndex = 0;
            this.B_Clear.Text = "X";
            this.TT_Main.SetToolTip(this.B_Clear, "Clear");
            this.B_Clear.UseVisualStyleBackColor = true;
            this.B_Clear.Click += new System.EventHandler(this.B_Clear_Click);
            // 
            // B_Select
            // 
            this.B_Select.Dock = System.Windows.Forms.DockStyle.Left;
            this.B_Select.Location = new System.Drawing.Point(26, 0);
            this.B_Select.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Select.Name = "B_Select";
            this.B_Select.Size = new System.Drawing.Size(26, 22);
            this.B_Select.TabIndex = 1;
            this.B_Select.Text = "...";
            this.TT_Main.SetToolTip(this.B_Select, "List");
            this.B_Select.UseVisualStyleBackColor = true;
            this.B_Select.Click += new System.EventHandler(this.B_Select_Click);
            // 
            // B_Card
            // 
            this.B_Card.Dock = System.Windows.Forms.DockStyle.Left;
            this.B_Card.Location = new System.Drawing.Point(52, 0);
            this.B_Card.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Card.Name = "B_Card";
            this.B_Card.Size = new System.Drawing.Size(26, 22);
            this.B_Card.TabIndex = 2;
            this.B_Card.Text = "O";
            this.TT_Main.SetToolTip(this.B_Card, "Properties");
            this.B_Card.UseVisualStyleBackColor = true;
            this.B_Card.Click += new System.EventHandler(this.B_Card_Click);
            // 
            // B_QuickSelect
            // 
            this.B_QuickSelect.Dock = System.Windows.Forms.DockStyle.Left;
            this.B_QuickSelect.Location = new System.Drawing.Point(78, 0);
            this.B_QuickSelect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_QuickSelect.Name = "B_QuickSelect";
            this.B_QuickSelect.Size = new System.Drawing.Size(26, 22);
            this.B_QuickSelect.TabIndex = 3;
            this.B_QuickSelect.Text = "V";
            this.TT_Main.SetToolTip(this.B_QuickSelect, "Quick Select");
            this.B_QuickSelect.UseVisualStyleBackColor = true;
            this.B_QuickSelect.Click += new System.EventHandler(this.B_QuickSelect_Click);
            // 
            // CommandButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.B_QuickSelect);
            this.Controls.Add(this.B_Card);
            this.Controls.Add(this.B_Select);
            this.Controls.Add(this.B_Clear);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CommandButtons";
            this.Size = new System.Drawing.Size(104, 22);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button B_Clear;
        private System.Windows.Forms.ToolTip TT_Main;
        private System.Windows.Forms.Button B_Select;
        private System.Windows.Forms.Button B_Card;
        private System.Windows.Forms.Button B_QuickSelect;
    }
}
