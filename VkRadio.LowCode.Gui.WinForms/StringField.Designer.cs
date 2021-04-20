namespace VkRadio.LowCode.Gui.WinForms
{
    partial class StringField
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
            this.T_Value = new System.Windows.Forms.TextBox();
            this.L_Caption = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // T_Value
            // 
            this.T_Value.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.T_Value.Location = new System.Drawing.Point(3, 17);
            this.T_Value.Name = "T_Value";
            this.T_Value.Size = new System.Drawing.Size(211, 20);
            this.T_Value.TabIndex = 3;
            this.T_Value.TextChanged += new System.EventHandler(this.T_Value_TextChanged);
            // 
            // L_Caption
            // 
            this.L_Caption.AutoSize = true;
            this.L_Caption.Location = new System.Drawing.Point(0, 0);
            this.L_Caption.Name = "L_Caption";
            this.L_Caption.Size = new System.Drawing.Size(46, 13);
            this.L_Caption.TabIndex = 2;
            this.L_Caption.Text = "Caption:";
            // 
            // StringField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.T_Value);
            this.Controls.Add(this.L_Caption);
            this.Name = "StringField";
            this.Size = new System.Drawing.Size(216, 37);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox T_Value;
        private System.Windows.Forms.Label L_Caption;
    }
}
