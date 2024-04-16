namespace maxrumsey.ozstrips.controls
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
            this.button11 = new System.Windows.Forms.Button();
            this.tb_hdg = new System.Windows.Forms.TextBox();
            this.button10 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
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
            this.cb_alt.Text = "050";
            this.cb_alt.SelectedIndexChanged += new System.EventHandler(this.cb_alt_SelectedIndexChanged);
            this.cb_alt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_alt_KeyDown);
            // 
            // tb_alt
            // 
            this.tb_alt.Location = new System.Drawing.Point(6, 108);
            this.tb_alt.MaxLength = 3;
            this.tb_alt.Name = "tb_alt";
            this.tb_alt.Size = new System.Drawing.Size(75, 20);
            this.tb_alt.TabIndex = 2;
            this.tb_alt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_alt_KeyDown);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(6, 134);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 23);
            this.button12.TabIndex = 16;
            this.button12.TabStop = false;
            this.button12.Text = "Clear";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.cb_alt);
            this.groupBox1.Controls.Add(this.tb_alt);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(88, 162);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Altitude";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button11);
            this.groupBox2.Controls.Add(this.tb_hdg);
            this.groupBox2.Controls.Add(this.button10);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button9);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Location = new System.Drawing.Point(99, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(105, 162);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Heading";
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(34, 133);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(50, 23);
            this.button11.TabIndex = 15;
            this.button11.TabStop = false;
            this.button11.Text = "Clear";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // tb_hdg
            // 
            this.tb_hdg.Location = new System.Drawing.Point(6, 20);
            this.tb_hdg.MaxLength = 3;
            this.tb_hdg.Name = "tb_hdg";
            this.tb_hdg.Size = new System.Drawing.Size(89, 20);
            this.tb_hdg.TabIndex = 4;
            this.tb_hdg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_alt_KeyDown);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(6, 133);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(22, 23);
            this.button10.TabIndex = 14;
            this.button10.TabStop = false;
            this.button10.Text = "0";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 23);
            this.button1.TabIndex = 5;
            this.button1.TabStop = false;
            this.button1.Text = "7";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(62, 105);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(22, 23);
            this.button9.TabIndex = 13;
            this.button9.TabStop = false;
            this.button9.Text = "3";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(34, 47);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(22, 23);
            this.button2.TabIndex = 6;
            this.button2.TabStop = false;
            this.button2.Text = "8";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(34, 105);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(22, 23);
            this.button8.TabIndex = 12;
            this.button8.TabStop = false;
            this.button8.Text = "2";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(62, 47);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(22, 23);
            this.button3.TabIndex = 7;
            this.button3.TabStop = false;
            this.button3.Text = "9";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(6, 105);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(22, 23);
            this.button7.TabIndex = 11;
            this.button7.TabStop = false;
            this.button7.Text = "1";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(6, 76);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(22, 23);
            this.button4.TabIndex = 8;
            this.button4.TabStop = false;
            this.button4.Text = "4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(62, 76);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(22, 23);
            this.button6.TabIndex = 10;
            this.button6.TabStop = false;
            this.button6.Text = "6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(34, 76);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(22, 23);
            this.button5.TabIndex = 9;
            this.button5.TabStop = false;
            this.button5.Text = "5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_runway);
            this.groupBox3.Location = new System.Drawing.Point(4, 172);
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
            this.cb_runway.SelectedIndexChanged += new System.EventHandler(this.cb_runway_SelectedIndexChanged);
            this.cb_runway.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_alt_KeyDown);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cb_sid);
            this.groupBox4.Location = new System.Drawing.Point(99, 172);
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
            this.cb_sid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_alt_KeyDown);
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
            this.Size = new System.Drawing.Size(213, 244);
            this.Load += new System.EventHandler(this.AltHdgControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.TextBox tb_hdg;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cb_runway;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cb_sid;
    }
}
