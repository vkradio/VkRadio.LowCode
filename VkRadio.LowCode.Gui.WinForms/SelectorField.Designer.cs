namespace VkRadio.LowCode.Gui.WinForms
{
    partial class SelectorField
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
            this.CTRL_Buttons = new Gui.WinForms.CommandButtons();
            this.SuspendLayout();
            // 
            // T_Value
            // 
            this.T_Value.Location = new System.Drawing.Point(3, 16);
            this.T_Value.Name = "T_Value";
            this.T_Value.ReadOnly = true;
            this.T_Value.Size = new System.Drawing.Size(211, 20);
            this.T_Value.TabIndex = 5;
            // 
            // L_Caption
            // 
            this.L_Caption.AutoSize = true;
            this.L_Caption.Location = new System.Drawing.Point(0, 0);
            this.L_Caption.Name = "L_Caption";
            this.L_Caption.Size = new System.Drawing.Size(46, 13);
            this.L_Caption.TabIndex = 4;
            this.L_Caption.Text = "Caption:";
            // 
            // CTRL_Buttons
            // 
            this.CTRL_Buttons.EnabledCard = true;
            this.CTRL_Buttons.EnabledClear = true;
            this.CTRL_Buttons.EnabledSelect = true;
            this.CTRL_Buttons.Location = new System.Drawing.Point(215, 16);
            this.CTRL_Buttons.Name = "CTRL_Buttons";
            this.CTRL_Buttons.Size = new System.Drawing.Size(87, 19);
            this.CTRL_Buttons.TabIndex = 6;
            this.CTRL_Buttons.UseCard = true;
            this.CTRL_Buttons.UseClear = true;
            this.CTRL_Buttons.UseSelect = true;
            // 
            // SelectorField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CTRL_Buttons);
            this.Controls.Add(this.T_Value);
            this.Controls.Add(this.L_Caption);
            this.Name = "SelectorField";
            this.Size = new System.Drawing.Size(305, 36);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox T_Value;
        private System.Windows.Forms.Label L_Caption;
        private CommandButtons CTRL_Buttons;
    }
}
