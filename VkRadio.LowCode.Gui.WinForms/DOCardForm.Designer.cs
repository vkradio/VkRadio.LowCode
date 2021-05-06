namespace VkRadio.LowCode.Gui.WinForms
{
    partial class DOCardForm
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
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                if (doCard != null)
                    doCard.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FRM_DOCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 511);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "FRM_DOCard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FRM_DOCard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FRM_DOCard_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FRM_DOCard_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}