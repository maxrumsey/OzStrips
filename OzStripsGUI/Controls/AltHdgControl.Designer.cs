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
            cb_alt = new System.Windows.Forms.ComboBox();
            tb_alt = new System.Windows.Forms.TextBox();
            button12 = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            button11 = new System.Windows.Forms.Button();
            tb_hdg = new System.Windows.Forms.TextBox();
            button10 = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            button9 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button8 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            button7 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            button6 = new System.Windows.Forms.Button();
            button5 = new System.Windows.Forms.Button();
            groupBox3 = new System.Windows.Forms.GroupBox();
            cb_runway = new System.Windows.Forms.ComboBox();
            groupBox4 = new System.Windows.Forms.GroupBox();
            cb_sid = new System.Windows.Forms.ComboBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // cb_alt
            // 
            cb_alt.FormattingEnabled = true;
            cb_alt.Items.AddRange(new object[] { "030", "040", "050" });
            cb_alt.Location = new System.Drawing.Point(6, 19);
            cb_alt.Name = "cb_alt";
            cb_alt.Size = new System.Drawing.Size(75, 21);
            cb_alt.TabIndex = 1;
            cb_alt.TabStop = false;
            cb_alt.SelectedIndexChanged += cb_alt_SelectedIndexChanged;
            cb_alt.KeyDown += tb_alt_KeyDown;
            // 
            // tb_alt
            // 
            tb_alt.Location = new System.Drawing.Point(6, 108);
            tb_alt.MaxLength = 3;
            tb_alt.Name = "tb_alt";
            tb_alt.Size = new System.Drawing.Size(75, 20);
            tb_alt.TabIndex = 2;
            tb_alt.KeyDown += tb_alt_KeyDown;
            // 
            // button12
            // 
            button12.Location = new System.Drawing.Point(6, 134);
            button12.Name = "button12";
            button12.Size = new System.Drawing.Size(75, 23);
            button12.TabIndex = 16;
            button12.TabStop = false;
            button12.Text = "Clear";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button12);
            groupBox1.Controls.Add(cb_alt);
            groupBox1.Controls.Add(tb_alt);
            groupBox1.Location = new System.Drawing.Point(4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(88, 162);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Altitude";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button11);
            groupBox2.Controls.Add(tb_hdg);
            groupBox2.Controls.Add(button10);
            groupBox2.Controls.Add(button1);
            groupBox2.Controls.Add(button9);
            groupBox2.Controls.Add(button2);
            groupBox2.Controls.Add(button8);
            groupBox2.Controls.Add(button3);
            groupBox2.Controls.Add(button7);
            groupBox2.Controls.Add(button4);
            groupBox2.Controls.Add(button6);
            groupBox2.Controls.Add(button5);
            groupBox2.Location = new System.Drawing.Point(99, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(105, 162);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "Heading";
            // 
            // button11
            // 
            button11.Location = new System.Drawing.Point(34, 133);
            button11.Name = "button11";
            button11.Size = new System.Drawing.Size(50, 23);
            button11.TabIndex = 15;
            button11.TabStop = false;
            button11.Text = "Clear";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // tb_hdg
            // 
            tb_hdg.Location = new System.Drawing.Point(6, 20);
            tb_hdg.MaxLength = 3;
            tb_hdg.Name = "tb_hdg";
            tb_hdg.Size = new System.Drawing.Size(89, 20);
            tb_hdg.TabIndex = 4;
            tb_hdg.KeyDown += tb_alt_KeyDown;
            // 
            // button10
            // 
            button10.Location = new System.Drawing.Point(6, 133);
            button10.Name = "button10";
            button10.Size = new System.Drawing.Size(22, 23);
            button10.TabIndex = 14;
            button10.TabStop = false;
            button10.Text = "0";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(6, 47);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(22, 23);
            button1.TabIndex = 5;
            button1.TabStop = false;
            button1.Text = "7";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button9
            // 
            button9.Location = new System.Drawing.Point(62, 105);
            button9.Name = "button9";
            button9.Size = new System.Drawing.Size(22, 23);
            button9.TabIndex = 13;
            button9.TabStop = false;
            button9.Text = "3";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(34, 47);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(22, 23);
            button2.TabIndex = 6;
            button2.TabStop = false;
            button2.Text = "8";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button8
            // 
            button8.Location = new System.Drawing.Point(34, 105);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(22, 23);
            button8.TabIndex = 12;
            button8.TabStop = false;
            button8.Text = "2";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(62, 47);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(22, 23);
            button3.TabIndex = 7;
            button3.TabStop = false;
            button3.Text = "9";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button7
            // 
            button7.Location = new System.Drawing.Point(6, 105);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(22, 23);
            button7.TabIndex = 11;
            button7.TabStop = false;
            button7.Text = "1";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(6, 76);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(22, 23);
            button4.TabIndex = 8;
            button4.TabStop = false;
            button4.Text = "4";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button6
            // 
            button6.Location = new System.Drawing.Point(62, 76);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(22, 23);
            button6.TabIndex = 10;
            button6.TabStop = false;
            button6.Text = "6";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.Location = new System.Drawing.Point(34, 76);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(22, 23);
            button5.TabIndex = 9;
            button5.TabStop = false;
            button5.Text = "5";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(cb_runway);
            groupBox3.Location = new System.Drawing.Point(4, 172);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(88, 67);
            groupBox3.TabIndex = 17;
            groupBox3.TabStop = false;
            groupBox3.Text = "Runway";
            // 
            // cb_runway
            // 
            cb_runway.FormattingEnabled = true;
            cb_runway.Location = new System.Drawing.Point(6, 19);
            cb_runway.Name = "cb_runway";
            cb_runway.Size = new System.Drawing.Size(75, 21);
            cb_runway.TabIndex = 1;
            cb_runway.SelectedIndexChanged += cb_runway_SelectedIndexChanged;
            cb_runway.KeyDown += tb_alt_KeyDown;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(cb_sid);
            groupBox4.Location = new System.Drawing.Point(99, 172);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(105, 67);
            groupBox4.TabIndex = 18;
            groupBox4.TabStop = false;
            groupBox4.Text = "SID";
            // 
            // cb_sid
            // 
            cb_sid.FormattingEnabled = true;
            cb_sid.Location = new System.Drawing.Point(6, 19);
            cb_sid.Name = "cb_sid";
            cb_sid.Size = new System.Drawing.Size(75, 21);
            cb_sid.TabIndex = 1;
            cb_sid.KeyDown += tb_alt_KeyDown;
            // 
            // AltHdgControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "AltHdgControl";
            Size = new System.Drawing.Size(213, 244);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            ResumeLayout(false);
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
