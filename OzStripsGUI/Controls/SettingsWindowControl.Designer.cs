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
            gp_main = new System.Windows.Forms.GroupBox();
            gb_ads = new System.Windows.Forms.GroupBox();
            bt_add = new System.Windows.Forms.Button();
            tb_ad = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            bt_remove = new System.Windows.Forms.Button();
            lb_ads = new System.Windows.Forms.ListBox();
            gb_server = new System.Windows.Forms.GroupBox();
            bt_sbset = new System.Windows.Forms.Button();
            rb_sb3 = new System.Windows.Forms.RadioButton();
            rb_sb2 = new System.Windows.Forms.RadioButton();
            rb_sb1 = new System.Windows.Forms.RadioButton();
            rb_vatsim = new System.Windows.Forms.RadioButton();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            rb_ozstrips = new System.Windows.Forms.RadioButton();
            rb_vatsys = new System.Windows.Forms.RadioButton();
            llb_keyboard = new System.Windows.Forms.LinkLabel();
            gp_main.SuspendLayout();
            gb_ads.SuspendLayout();
            gb_server.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // gp_main
            // 
            gp_main.Controls.Add(llb_keyboard);
            gp_main.Controls.Add(gb_ads);
            gp_main.Controls.Add(gb_server);
            gp_main.Controls.Add(groupBox1);
            gp_main.Location = new System.Drawing.Point(4, 4);
            gp_main.Name = "gp_main";
            gp_main.Size = new System.Drawing.Size(469, 284);
            gp_main.TabIndex = 0;
            gp_main.TabStop = false;
            gp_main.Text = "Settings";
            // 
            // gb_ads
            // 
            gb_ads.Controls.Add(bt_add);
            gb_ads.Controls.Add(tb_ad);
            gb_ads.Controls.Add(label2);
            gb_ads.Controls.Add(bt_remove);
            gb_ads.Controls.Add(lb_ads);
            gb_ads.Location = new System.Drawing.Point(236, 20);
            gb_ads.Name = "gb_ads";
            gb_ads.Size = new System.Drawing.Size(227, 151);
            gb_ads.TabIndex = 2;
            gb_ads.TabStop = false;
            gb_ads.Text = "Aerodromes";
            // 
            // bt_add
            // 
            bt_add.Location = new System.Drawing.Point(117, 100);
            bt_add.Name = "bt_add";
            bt_add.Size = new System.Drawing.Size(103, 23);
            bt_add.TabIndex = 4;
            bt_add.Text = "Add";
            bt_add.UseVisualStyleBackColor = true;
            bt_add.Click += ADAddClick;
            // 
            // tb_ad
            // 
            tb_ad.Location = new System.Drawing.Point(117, 74);
            tb_ad.Name = "tb_ad";
            tb_ad.Size = new System.Drawing.Size(100, 20);
            tb_ad.TabIndex = 3;
            tb_ad.KeyPress += ADKeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(117, 61);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(89, 13);
            label2.TabIndex = 2;
            label2.Text = "Aerodrome Name";
            // 
            // bt_remove
            // 
            bt_remove.Location = new System.Drawing.Point(118, 20);
            bt_remove.Name = "bt_remove";
            bt_remove.Size = new System.Drawing.Size(103, 23);
            bt_remove.TabIndex = 1;
            bt_remove.Text = "Remove";
            bt_remove.UseVisualStyleBackColor = true;
            bt_remove.Click += ADRemoveClick;
            // 
            // lb_ads
            // 
            lb_ads.FormattingEnabled = true;
            lb_ads.Location = new System.Drawing.Point(7, 20);
            lb_ads.Name = "lb_ads";
            lb_ads.Size = new System.Drawing.Size(104, 121);
            lb_ads.TabIndex = 0;
            // 
            // gb_server
            // 
            gb_server.Controls.Add(bt_sbset);
            gb_server.Controls.Add(rb_sb3);
            gb_server.Controls.Add(rb_sb2);
            gb_server.Controls.Add(rb_sb1);
            gb_server.Controls.Add(rb_vatsim);
            gb_server.Location = new System.Drawing.Point(7, 111);
            gb_server.Name = "gb_server";
            gb_server.Size = new System.Drawing.Size(222, 143);
            gb_server.TabIndex = 1;
            gb_server.TabStop = false;
            gb_server.Text = "Server";
            // 
            // bt_sbset
            // 
            bt_sbset.Location = new System.Drawing.Point(10, 112);
            bt_sbset.Name = "bt_sbset";
            bt_sbset.Size = new System.Drawing.Size(75, 23);
            bt_sbset.TabIndex = 4;
            bt_sbset.Text = "Set";
            bt_sbset.UseVisualStyleBackColor = true;
            bt_sbset.Click += SBButtonClick;
            // 
            // rb_sb3
            // 
            rb_sb3.AutoSize = true;
            rb_sb3.Location = new System.Drawing.Point(10, 89);
            rb_sb3.Name = "rb_sb3";
            rb_sb3.Size = new System.Drawing.Size(81, 17);
            rb_sb3.TabIndex = 3;
            rb_sb3.TabStop = true;
            rb_sb3.Text = "Sweatbox 3";
            rb_sb3.UseVisualStyleBackColor = true;
            // 
            // rb_sb2
            // 
            rb_sb2.AutoSize = true;
            rb_sb2.Location = new System.Drawing.Point(10, 66);
            rb_sb2.Name = "rb_sb2";
            rb_sb2.Size = new System.Drawing.Size(81, 17);
            rb_sb2.TabIndex = 2;
            rb_sb2.TabStop = true;
            rb_sb2.Text = "Sweatbox 2";
            rb_sb2.UseVisualStyleBackColor = true;
            // 
            // rb_sb1
            // 
            rb_sb1.AutoSize = true;
            rb_sb1.Location = new System.Drawing.Point(10, 43);
            rb_sb1.Name = "rb_sb1";
            rb_sb1.Size = new System.Drawing.Size(81, 17);
            rb_sb1.TabIndex = 1;
            rb_sb1.TabStop = true;
            rb_sb1.Text = "Sweatbox 1";
            rb_sb1.UseVisualStyleBackColor = true;
            // 
            // rb_vatsim
            // 
            rb_vatsim.AutoSize = true;
            rb_vatsim.Location = new System.Drawing.Point(10, 20);
            rb_vatsim.Name = "rb_vatsim";
            rb_vatsim.Size = new System.Drawing.Size(65, 17);
            rb_vatsim.TabIndex = 0;
            rb_vatsim.TabStop = true;
            rb_vatsim.Text = "VATSIM";
            rb_vatsim.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(rb_ozstrips);
            groupBox1.Controls.Add(rb_vatsys);
            groupBox1.Location = new System.Drawing.Point(7, 20);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(222, 84);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "FDR Popup Types";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 20);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(209, 13);
            label1.TabIndex = 2;
            label1.Text = "Determines what type of popup box to use.";
            // 
            // rb_ozstrips
            // 
            rb_ozstrips.AutoSize = true;
            rb_ozstrips.Location = new System.Drawing.Point(6, 59);
            rb_ozstrips.Name = "rb_ozstrips";
            rb_ozstrips.Size = new System.Drawing.Size(98, 17);
            rb_ozstrips.TabIndex = 1;
            rb_ozstrips.TabStop = true;
            rb_ozstrips.Text = "OzStrips Popup";
            rb_ozstrips.UseVisualStyleBackColor = true;
            // 
            // rb_vatsys
            // 
            rb_vatsys.AutoSize = true;
            rb_vatsys.Location = new System.Drawing.Point(6, 36);
            rb_vatsys.Name = "rb_vatsys";
            rb_vatsys.Size = new System.Drawing.Size(91, 17);
            rb_vatsys.TabIndex = 0;
            rb_vatsys.TabStop = true;
            rb_vatsys.Text = "vatSys Popup";
            rb_vatsys.UseVisualStyleBackColor = true;
            // 
            // llb_keyboard
            // 
            llb_keyboard.AutoSize = true;
            llb_keyboard.Location = new System.Drawing.Point(237, 182);
            llb_keyboard.Name = "llb_keyboard";
            llb_keyboard.Size = new System.Drawing.Size(107, 13);
            llb_keyboard.TabIndex = 3;
            llb_keyboard.TabStop = true;
            llb_keyboard.Text = "Keyboard Commands";
            llb_keyboard.LinkClicked += KeyboardCommandsOpened;
            // 
            // SettingsWindowControl
            // 
            Controls.Add(gp_main);
            Name = "SettingsWindowControl";
            Size = new System.Drawing.Size(476, 296);
            gp_main.ResumeLayout(false);
            gp_main.PerformLayout();
            gb_ads.ResumeLayout(false);
            gb_ads.PerformLayout();
            gb_server.ResumeLayout(false);
            gb_server.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox gb_ads;
        private System.Windows.Forms.Button bt_add;
        private System.Windows.Forms.TextBox tb_ad;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_remove;
        private System.Windows.Forms.ListBox lb_ads;
        private System.Windows.Forms.LinkLabel llb_keyboard;
    }
}
