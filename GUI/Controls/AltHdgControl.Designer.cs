namespace MaxRumsey.OzStripsPlugin.GUI.Controls
{
    partial class AltHdgControl
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
            this.cb_alt = new System.Windows.Forms.ComboBox();
            this.tb_alt = new System.Windows.Forms.TextBox();
            this.button12 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_depfreq = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_runway = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cb_sid = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_alt
            // 
            this.cb_alt.FormattingEnabled = true;
            this.cb_alt.Items.AddRange(new object[] {
            "030",
            "040",
            "050"});
            this.cb_alt.Location = new System.Drawing.Point(6, 19);
            this.cb_alt.Name = "cb_alt";
            this.cb_alt.Size = new System.Drawing.Size(75, 21);
            this.cb_alt.TabIndex = 1;
            this.cb_alt.TabStop = false;
            this.cb_alt.SelectedIndexChanged += new System.EventHandler(this.AltitudeComboSelectedChanged);
            // 
            // tb_alt
            // 
            this.tb_alt.Location = new System.Drawing.Point(6, 46);
            this.tb_alt.MaxLength = 3;
            this.tb_alt.Name = "tb_alt";
            this.tb_alt.Size = new System.Drawing.Size(75, 20);
            this.tb_alt.TabIndex = 2;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(6, 72);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 23);
            this.button12.TabIndex = 16;
            this.button12.TabStop = false;
            this.button12.Text = "Clear";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.ClearAltitudeButtonClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.cb_alt);
            this.groupBox1.Controls.Add(this.tb_alt);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(88, 108);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Altitude";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_depfreq);
            this.groupBox2.Location = new System.Drawing.Point(99, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(105, 108);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dep Freq";
            // 
            // cb_depfreq
            // 
            this.cb_depfreq.FormattingEnabled = true;
            this.cb_depfreq.Items.AddRange(new object[] {});
            this.cb_depfreq.Location = new System.Drawing.Point(6, 19);
            this.cb_depfreq.Name = "cb_depfreq";
            this.cb_depfreq.Size = new System.Drawing.Size(93, 21);
            this.cb_depfreq.TabIndex = 2;
            this.cb_depfreq.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_runway);
            this.groupBox3.Location = new System.Drawing.Point(4, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(88, 67);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Runway";
            // 
            // cb_runway
            // 
            this.cb_runway.FormattingEnabled = true;
            this.cb_runway.Location = new System.Drawing.Point(6, 19);
            this.cb_runway.Name = "cb_runway";
            this.cb_runway.Size = new System.Drawing.Size(75, 21);
            this.cb_runway.TabIndex = 1;
            this.cb_runway.SelectedIndexChanged += new System.EventHandler(this.ComboRwySelectedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cb_sid);
            this.groupBox4.Location = new System.Drawing.Point(99, 118);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(105, 67);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "SID";
            // 
            // cb_sid
            // 
            this.cb_sid.FormattingEnabled = true;
            this.cb_sid.Location = new System.Drawing.Point(6, 19);
            this.cb_sid.Name = "cb_sid";
            this.cb_sid.Size = new System.Drawing.Size(75, 21);
            this.cb_sid.TabIndex = 1;
            this.cb_sid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AltKeyDownChanged);
            // 
            // AltHdgControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "AltHdgControl";
            this.Size = new System.Drawing.Size(213, 194);
            this.Load += new System.EventHandler(this.AltHdgControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cb_alt;
        private System.Windows.Forms.TextBox tb_alt;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cb_runway;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cb_sid;
        private System.Windows.Forms.ComboBox cb_depfreq;
    }
}
