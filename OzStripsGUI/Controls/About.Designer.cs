namespace MaxRumsey.OzStripsPlugin.Gui.Controls
{
    partial class About
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lb_version = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lb_version);
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(269, 254);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "About";
            // 
            // lb_version
            // 
            this.lb_version.AutoSize = true;
            this.lb_version.Location = new System.Drawing.Point(7, 224);
            this.lb_version.Name = "lb_version";
            this.lb_version.Size = new System.Drawing.Size(42, 13);
            this.lb_version.TabIndex = 1;
            this.lb_version.Text = "Version";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(7, 20);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(256, 197);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "OzStrips\n\nAn INTAS style strip emulator for VATSYS.\n\nSpecial thanks to Matt K, Le" +
    "vi, Plane Alex, Cat Alex and Glenn for their help and support!\n\n© Max Rumsey";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "About";
            this.Size = new System.Drawing.Size(276, 261);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lb_version;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
