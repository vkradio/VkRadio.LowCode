namespace VkRadio.LowCode.Gui.WinForms
{
    partial class DOCard
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
            this.B_Rollback = new System.Windows.Forms.Button();
            this.B_Commit = new System.Windows.Forms.Button();
            this.PAN_Content = new System.Windows.Forms.Panel();
            this.PAN_Control = new System.Windows.Forms.Panel();
            this.PAN_Control.SuspendLayout();
            this.SuspendLayout();
            // 
            // B_Rollback
            // 
            this.B_Rollback.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.B_Rollback.Location = new System.Drawing.Point(258, 9);
            this.B_Rollback.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Rollback.Name = "B_Rollback";
            this.B_Rollback.Size = new System.Drawing.Size(88, 27);
            this.B_Rollback.TabIndex = 5;
            this.B_Rollback.Text = "Rollback";
            this.B_Rollback.UseVisualStyleBackColor = true;
            this.B_Rollback.Click += new System.EventHandler(this.B_Rollback_Click);
            // 
            // B_Commit
            // 
            this.B_Commit.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.B_Commit.Location = new System.Drawing.Point(163, 9);
            this.B_Commit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_Commit.Name = "B_Commit";
            this.B_Commit.Size = new System.Drawing.Size(88, 27);
            this.B_Commit.TabIndex = 4;
            this.B_Commit.Text = "Apply";
            this.B_Commit.UseVisualStyleBackColor = true;
            this.B_Commit.Click += new System.EventHandler(this.B_Commit_Click);
            // 
            // PAN_Content
            // 
            this.PAN_Content.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PAN_Content.AutoScroll = true;
            this.PAN_Content.Location = new System.Drawing.Point(0, 0);
            this.PAN_Content.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PAN_Content.Name = "PAN_Content";
            this.PAN_Content.Size = new System.Drawing.Size(349, 209);
            this.PAN_Content.TabIndex = 6;
            // 
            // PAN_Control
            // 
            this.PAN_Control.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PAN_Control.Controls.Add(this.B_Rollback);
            this.PAN_Control.Controls.Add(this.B_Commit);
            this.PAN_Control.Location = new System.Drawing.Point(0, 207);
            this.PAN_Control.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PAN_Control.Name = "PAN_Control";
            this.PAN_Control.Size = new System.Drawing.Size(349, 45);
            this.PAN_Control.TabIndex = 7;
            // 
            // DOCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PAN_Control);
            this.Controls.Add(this.PAN_Content);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DOCard";
            this.Size = new System.Drawing.Size(349, 252);
            this.PAN_Control.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button B_Rollback;
        private System.Windows.Forms.Button B_Commit;
        private System.Windows.Forms.Panel PAN_Content;
        private System.Windows.Forms.Panel PAN_Control;
    }
}
