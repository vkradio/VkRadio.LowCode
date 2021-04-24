namespace VkRadio.LowCode.TestBed.Generated.Gui.Elements
{
    partial class DOPDriveAccount
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
            this.SF_Name = new VkRadio.LowCode.Gui.WinForms.StringField();
            this.SF_Code = new VkRadio.LowCode.Gui.WinForms.StringField();
            this.SF_DefaultValue = new VkRadio.LowCode.Gui.WinForms.StringField();
            this.SuspendLayout();
            // 
            // SF_Name
            // 
            this.SF_Name.Caption = "Name";
            this.SF_Name.Location = new System.Drawing.Point(0, 3);
            this.SF_Name.Name = "SF_Name";
            this.SF_Name.Size = new System.Drawing.Size(305, 37);
            this.SF_Name.TabIndex = 20;
            this.SF_Name.Value = "";
            // 
            // SF_Code
            // 
            this.SF_Code.Caption = "Code";
            this.SF_Code.Location = new System.Drawing.Point(0, 46);
            this.SF_Code.Name = "SF_Code";
            this.SF_Code.Size = new System.Drawing.Size(305, 37);
            this.SF_Code.TabIndex = 21;
            this.SF_Code.Value = "";
            // 
            // SF_DefaultValue
            // 
            this.SF_DefaultValue.Caption = "Default Value";
            this.SF_DefaultValue.Location = new System.Drawing.Point(0, 89);
            this.SF_DefaultValue.Name = "SF_DefaultValue";
            this.SF_DefaultValue.Size = new System.Drawing.Size(305, 37);
            this.SF_DefaultValue.TabIndex = 22;
            this.SF_DefaultValue.Value = "";
            // 
            // DOPDriveAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SF_Name);
            this.Controls.Add(this.SF_Code);
            this.Controls.Add(this.SF_DefaultValue);
            this.Name = "DOPDriveAccount";
            this.Size = new System.Drawing.Size(613, 174);
            this.ResumeLayout(false);

        }

        #endregion

        private VkRadio.LowCode.Gui.WinForms.StringField SF_Name;
        private VkRadio.LowCode.Gui.WinForms.StringField SF_Code;
        private VkRadio.LowCode.Gui.WinForms.StringField SF_DefaultValue;
    }
}
