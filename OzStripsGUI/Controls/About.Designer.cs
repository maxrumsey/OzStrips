namespace maxrumsey.ozstrips.controls
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
            groupBox1 = new System.Windows.Forms.GroupBox();
            lb_version = new System.Windows.Forms.Label();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lb_version);
            groupBox1.Controls.Add(richTextBox1);
            groupBox1.Location = new System.Drawing.Point(4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            groupBox1.Size = new System.Drawing.Size(269, 254);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "About";
            // 
            // lb_version
            // 
            lb_version.AutoSize = true;
            lb_version.Location = new System.Drawing.Point(7, 224);
            lb_version.Name = "lb_version";
            lb_version.Size = new System.Drawing.Size(42, 13);
            lb_version.TabIndex = 1;
            lb_version.Text = "Version";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new System.Drawing.Point(7, 20);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new System.Drawing.Size(256, 197);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "OzStrips\n\nA stripboard extension for VATSYS.\n\nSpecial thanks to Levi, Plane Alex, Cat Alex and Glenn for their help and support!\n\n© Max Rumsey";
            // 
            // About
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Name = "About";
            Size = new System.Drawing.Size(276, 261);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lb_version;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
