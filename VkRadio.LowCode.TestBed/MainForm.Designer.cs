
namespace VkRadio.LowCode.TestBed
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ObjectListButton = new System.Windows.Forms.Button();
            this.SqlServerTextBox = new System.Windows.Forms.TextBox();
            this.DatabaseTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ObjectListButton
            // 
            this.ObjectListButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectListButton.Location = new System.Drawing.Point(80, 93);
            this.ObjectListButton.Name = "ObjectListButton";
            this.ObjectListButton.Size = new System.Drawing.Size(183, 23);
            this.ObjectListButton.TabIndex = 0;
            this.ObjectListButton.Text = "Object List";
            this.ObjectListButton.UseVisualStyleBackColor = true;
            this.ObjectListButton.Click += new System.EventHandler(this.ObjectListButton_Click);
            // 
            // SqlServerTextBox
            // 
            this.SqlServerTextBox.Location = new System.Drawing.Point(81, 17);
            this.SqlServerTextBox.Name = "SqlServerTextBox";
            this.SqlServerTextBox.Size = new System.Drawing.Size(153, 23);
            this.SqlServerTextBox.TabIndex = 1;
            // 
            // DatabaseTextBox
            // 
            this.DatabaseTextBox.Location = new System.Drawing.Point(81, 46);
            this.DatabaseTextBox.Name = "DatabaseTextBox";
            this.DatabaseTextBox.Size = new System.Drawing.Size(153, 23);
            this.DatabaseTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "SQL Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Database";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 139);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DatabaseTextBox);
            this.Controls.Add(this.SqlServerTextBox);
            this.Controls.Add(this.ObjectListButton);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "vkradio Low Code Test Bed";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ObjectListButton;
        private System.Windows.Forms.TextBox SqlServerTextBox;
        private System.Windows.Forms.TextBox DatabaseTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

