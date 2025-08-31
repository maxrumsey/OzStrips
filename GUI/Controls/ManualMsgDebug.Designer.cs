namespace MaxRumsey.OzStripsPlugin.GUI.Controls
{
    partial class ManualMsgDebug
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
            this.tbc_main = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_crossing = new System.Windows.Forms.CheckBox();
            this.tb_acid = new System.Windows.Forms.TextBox();
            this.tb_baynum = new System.Windows.Forms.TextBox();
            this.tb_cocklevel = new System.Windows.Forms.TextBox();
            this.tb_clx = new System.Windows.Forms.TextBox();
            this.tb_bay = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tbc_main.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbc_main
            // 
            this.tbc_main.Controls.Add(this.tabPage1);
            this.tbc_main.Location = new System.Drawing.Point(4, 4);
            this.tbc_main.Name = "tbc_main";
            this.tbc_main.SelectedIndex = 0;
            this.tbc_main.Size = new System.Drawing.Size(264, 252);
            this.tbc_main.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.tb_bay);
            this.tabPage1.Controls.Add(this.tb_clx);
            this.tabPage1.Controls.Add(this.tb_cocklevel);
            this.tabPage1.Controls.Add(this.tb_baynum);
            this.tabPage1.Controls.Add(this.tb_acid);
            this.tabPage1.Controls.Add(this.cb_crossing);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(256, 226);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SCDTO";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ACID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Bay #";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Cock Level";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "CLX";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "GATE";
            // 
            // cb_crossing
            // 
            this.cb_crossing.AutoSize = true;
            this.cb_crossing.Location = new System.Drawing.Point(10, 139);
            this.cb_crossing.Name = "cb_crossing";
            this.cb_crossing.Size = new System.Drawing.Size(66, 17);
            this.cb_crossing.TabIndex = 6;
            this.cb_crossing.Text = "Crossing";
            this.cb_crossing.UseVisualStyleBackColor = true;
            // 
            // tb_acid
            // 
            this.tb_acid.Location = new System.Drawing.Point(74, 4);
            this.tb_acid.Name = "tb_acid";
            this.tb_acid.Size = new System.Drawing.Size(100, 20);
            this.tb_acid.TabIndex = 7;
            // 
            // tb_baynum
            // 
            this.tb_baynum.Location = new System.Drawing.Point(74, 29);
            this.tb_baynum.Name = "tb_baynum";
            this.tb_baynum.Size = new System.Drawing.Size(100, 20);
            this.tb_baynum.TabIndex = 8;
            // 
            // tb_cocklevel
            // 
            this.tb_cocklevel.Location = new System.Drawing.Point(74, 52);
            this.tb_cocklevel.Name = "tb_cocklevel";
            this.tb_cocklevel.Size = new System.Drawing.Size(100, 20);
            this.tb_cocklevel.TabIndex = 9;
            // 
            // tb_clx
            // 
            this.tb_clx.Location = new System.Drawing.Point(74, 85);
            this.tb_clx.Name = "tb_clx";
            this.tb_clx.Size = new System.Drawing.Size(100, 20);
            this.tb_clx.TabIndex = 10;
            // 
            // tb_bay
            // 
            this.tb_bay.Location = new System.Drawing.Point(74, 111);
            this.tb_bay.Name = "tb_bay";
            this.tb_bay.Size = new System.Drawing.Size(100, 20);
            this.tb_bay.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(74, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SendButton);
            // 
            // ManualMsgDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbc_main);
            this.Name = "ManualMsgDebug";
            this.Size = new System.Drawing.Size(276, 261);
            this.tbc_main.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbc_main;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_bay;
        private System.Windows.Forms.TextBox tb_clx;
        private System.Windows.Forms.TextBox tb_cocklevel;
        private System.Windows.Forms.TextBox tb_baynum;
        private System.Windows.Forms.TextBox tb_acid;
        private System.Windows.Forms.CheckBox cb_crossing;
    }
}
