namespace MaxRumsey.OzStripsPlugin.Gui.Controls
{
    partial class SettingsWindowControl
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

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gp_main = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rb_ozstrips = new System.Windows.Forms.RadioButton();
            this.rb_vatsys = new System.Windows.Forms.RadioButton();
            this.gb_server = new System.Windows.Forms.GroupBox();
            this.rb_vatsim = new System.Windows.Forms.RadioButton();
            this.rb_sb1 = new System.Windows.Forms.RadioButton();
            this.rb_sb2 = new System.Windows.Forms.RadioButton();
            this.rb_sb3 = new System.Windows.Forms.RadioButton();
            this.bt_sbset = new System.Windows.Forms.Button();
            this.gp_main.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gb_server.SuspendLayout();
            this.SuspendLayout();
            // 
            // gp_main
            // 
            this.gp_main.Controls.Add(this.gb_server);
            this.gp_main.Controls.Add(this.groupBox1);
            this.gp_main.Location = new System.Drawing.Point(4, 4);
            this.gp_main.Name = "gp_main";
            this.gp_main.Size = new System.Drawing.Size(399, 284);
            this.gp_main.TabIndex = 0;
            this.gp_main.TabStop = false;
            this.gp_main.Text = "Settings";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rb_ozstrips);
            this.groupBox1.Controls.Add(this.rb_vatsys);
            this.groupBox1.Location = new System.Drawing.Point(7, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FDR Popup Types";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Determines what type of popup box to use.";
            // 
            // rb_ozstrips
            // 
            this.rb_ozstrips.AutoSize = true;
            this.rb_ozstrips.Location = new System.Drawing.Point(6, 59);
            this.rb_ozstrips.Name = "rb_ozstrips";
            this.rb_ozstrips.Size = new System.Drawing.Size(98, 17);
            this.rb_ozstrips.TabIndex = 1;
            this.rb_ozstrips.TabStop = true;
            this.rb_ozstrips.Text = "OzStrips Popup";
            this.rb_ozstrips.UseVisualStyleBackColor = true;
            // 
            // rb_vatsys
            // 
            this.rb_vatsys.AutoSize = true;
            this.rb_vatsys.Location = new System.Drawing.Point(6, 36);
            this.rb_vatsys.Name = "rb_vatsys";
            this.rb_vatsys.Size = new System.Drawing.Size(91, 17);
            this.rb_vatsys.TabIndex = 0;
            this.rb_vatsys.TabStop = true;
            this.rb_vatsys.Text = "vatSys Popup";
            this.rb_vatsys.UseVisualStyleBackColor = true;
            // 
            // gb_server
            // 
            this.gb_server.Controls.Add(this.bt_sbset);
            this.gb_server.Controls.Add(this.rb_sb3);
            this.gb_server.Controls.Add(this.rb_sb2);
            this.gb_server.Controls.Add(this.rb_sb1);
            this.gb_server.Controls.Add(this.rb_vatsim);
            this.gb_server.Location = new System.Drawing.Point(7, 111);
            this.gb_server.Name = "gb_server";
            this.gb_server.Size = new System.Drawing.Size(222, 143);
            this.gb_server.TabIndex = 1;
            this.gb_server.TabStop = false;
            this.gb_server.Text = "Server";
            // 
            // rb_vatsim
            // 
            this.rb_vatsim.AutoSize = true;
            this.rb_vatsim.Location = new System.Drawing.Point(10, 20);
            this.rb_vatsim.Name = "rb_vatsim";
            this.rb_vatsim.Size = new System.Drawing.Size(65, 17);
            this.rb_vatsim.TabIndex = 0;
            this.rb_vatsim.TabStop = true;
            this.rb_vatsim.Text = "VATSIM";
            this.rb_vatsim.UseVisualStyleBackColor = true;
            // 
            // rb_sb1
            // 
            this.rb_sb1.AutoSize = true;
            this.rb_sb1.Location = new System.Drawing.Point(10, 43);
            this.rb_sb1.Name = "rb_sb1";
            this.rb_sb1.Size = new System.Drawing.Size(81, 17);
            this.rb_sb1.TabIndex = 1;
            this.rb_sb1.TabStop = true;
            this.rb_sb1.Text = "Sweatbox 1";
            this.rb_sb1.UseVisualStyleBackColor = true;
            // 
            // rb_sb2
            // 
            this.rb_sb2.AutoSize = true;
            this.rb_sb2.Location = new System.Drawing.Point(10, 66);
            this.rb_sb2.Name = "rb_sb2";
            this.rb_sb2.Size = new System.Drawing.Size(81, 17);
            this.rb_sb2.TabIndex = 2;
            this.rb_sb2.TabStop = true;
            this.rb_sb2.Text = "Sweatbox 2";
            this.rb_sb2.UseVisualStyleBackColor = true;
            // 
            // rb_sb3
            // 
            this.rb_sb3.AutoSize = true;
            this.rb_sb3.Location = new System.Drawing.Point(10, 89);
            this.rb_sb3.Name = "rb_sb3";
            this.rb_sb3.Size = new System.Drawing.Size(81, 17);
            this.rb_sb3.TabIndex = 3;
            this.rb_sb3.TabStop = true;
            this.rb_sb3.Text = "Sweatbox 3";
            this.rb_sb3.UseVisualStyleBackColor = true;
            // 
            // bt_sbset
            // 
            this.bt_sbset.Location = new System.Drawing.Point(10, 112);
            this.bt_sbset.Name = "bt_sbset";
            this.bt_sbset.Size = new System.Drawing.Size(75, 23);
            this.bt_sbset.TabIndex = 4;
            this.bt_sbset.Text = "Set";
            this.bt_sbset.UseVisualStyleBackColor = true;
            this.bt_sbset.Click += new System.EventHandler(this.SBButtonClick);
            // 
            // SettingsWindowControl
            // 
            this.Controls.Add(this.gp_main);
            this.Name = "SettingsWindowControl";
            this.Size = new System.Drawing.Size(406, 291);
            this.gp_main.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gb_server.ResumeLayout(false);
            this.gb_server.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox gp_main;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rb_ozstrips;
        private System.Windows.Forms.RadioButton rb_vatsys;
        private System.Windows.Forms.GroupBox gb_server;
        private System.Windows.Forms.RadioButton rb_vatsim;
        private System.Windows.Forms.RadioButton rb_sb3;
        private System.Windows.Forms.RadioButton rb_sb2;
        private System.Windows.Forms.RadioButton rb_sb1;
        private System.Windows.Forms.Button bt_sbset;
    }
}
