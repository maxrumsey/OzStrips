namespace MaxRumsey.OzStripsPlugin.GUI.Controls
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
            this.gb_open = new System.Windows.Forms.GroupBox();
            this.cb_open = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gb_int = new System.Windows.Forms.GroupBox();
            this.cb_lasttransmit = new System.Windows.Forms.CheckBox();
            this.cb_preasort = new System.Windows.Forms.CheckBox();
            this.cb_keeppicked = new System.Windows.Forms.CheckBox();
            this.gb_ads = new System.Windows.Forms.GroupBox();
            this.bt_add = new System.Windows.Forms.Button();
            this.tb_ad = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_remove = new System.Windows.Forms.Button();
            this.lb_ads = new System.Windows.Forms.ListBox();
            this.gb_server = new System.Windows.Forms.GroupBox();
            this.bt_sbset = new System.Windows.Forms.Button();
            this.rb_sb3 = new System.Windows.Forms.RadioButton();
            this.rb_sb2 = new System.Windows.Forms.RadioButton();
            this.rb_sb1 = new System.Windows.Forms.RadioButton();
            this.rb_vatsim = new System.Windows.Forms.RadioButton();
            this.gb_scale = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_scale = new System.Windows.Forms.TrackBar();
            this.llb_keyboard = new System.Windows.Forms.LinkLabel();
            this.gp_main.SuspendLayout();
            this.gb_open.SuspendLayout();
            this.gb_int.SuspendLayout();
            this.gb_ads.SuspendLayout();
            this.gb_server.SuspendLayout();
            this.gb_scale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_scale)).BeginInit();
            this.SuspendLayout();
            // 
            // gp_main
            // 
            this.gp_main.Controls.Add(this.gb_open);
            this.gp_main.Controls.Add(this.gb_int);
            this.gp_main.Controls.Add(this.gb_ads);
            this.gp_main.Controls.Add(this.gb_server);
            this.gp_main.Controls.Add(this.gb_scale);
            this.gp_main.Location = new System.Drawing.Point(4, 4);
            this.gp_main.Name = "gp_main";
            this.gp_main.Size = new System.Drawing.Size(469, 373);
            this.gp_main.TabIndex = 0;
            this.gp_main.TabStop = false;
            this.gp_main.Text = "Settings";
            // 
            // gb_open
            // 
            this.gb_open.Controls.Add(this.cb_open);
            this.gb_open.Controls.Add(this.label5);
            this.gb_open.Location = new System.Drawing.Point(7, 163);
            this.gb_open.Name = "gb_open";
            this.gb_open.Size = new System.Drawing.Size(222, 93);
            this.gb_open.TabIndex = 3;
            this.gb_open.TabStop = false;
            this.gb_open.Text = "Opening Behaviour";
            // 
            // cb_open
            // 
            this.cb_open.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_open.FormattingEnabled = true;
            this.cb_open.Location = new System.Drawing.Point(10, 63);
            this.cb_open.Name = "cb_open";
            this.cb_open.Size = new System.Drawing.Size(145, 21);
            this.cb_open.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(209, 40);
            this.label5.TabIndex = 2;
            this.label5.Text = "Should OzStrips auto-open on launch or position switch?";
            // 
            // gb_int
            // 
            this.gb_int.Controls.Add(this.cb_lasttransmit);
            this.gb_int.Controls.Add(this.cb_preasort);
            this.gb_int.Controls.Add(this.cb_keeppicked);
            this.gb_int.Location = new System.Drawing.Point(236, 285);
            this.gb_int.Name = "gb_int";
            this.gb_int.Size = new System.Drawing.Size(227, 82);
            this.gb_int.TabIndex = 6;
            this.gb_int.TabStop = false;
            this.gb_int.Text = "Strip Interaction";
            // 
            // cb_lasttransmit
            // 
            this.cb_lasttransmit.AutoSize = true;
            this.cb_lasttransmit.Location = new System.Drawing.Point(7, 57);
            this.cb_lasttransmit.Name = "cb_lasttransmit";
            this.cb_lasttransmit.Size = new System.Drawing.Size(180, 17);
            this.cb_lasttransmit.TabIndex = 2;
            this.cb_lasttransmit.Text = "Highlight last transmitting aircraft.";
            this.cb_lasttransmit.UseVisualStyleBackColor = true;
            // 
            // cb_preasort
            // 
            this.cb_preasort.AutoSize = true;
            this.cb_preasort.Location = new System.Drawing.Point(7, 38);
            this.cb_preasort.Name = "cb_preasort";
            this.cb_preasort.Size = new System.Drawing.Size(188, 17);
            this.cb_preasort.TabIndex = 1;
            this.cb_preasort.Text = "Alphabetically sort preactive strips.";
            this.cb_preasort.UseVisualStyleBackColor = true;
            // 
            // cb_keeppicked
            // 
            this.cb_keeppicked.AutoSize = true;
            this.cb_keeppicked.Location = new System.Drawing.Point(7, 20);
            this.cb_keeppicked.Name = "cb_keeppicked";
            this.cb_keeppicked.Size = new System.Drawing.Size(167, 17);
            this.cb_keeppicked.TabIndex = 0;
            this.cb_keeppicked.Text = "Keep strip picked after action.";
            this.cb_keeppicked.UseVisualStyleBackColor = true;
            // 
            // gb_ads
            // 
            this.gb_ads.Controls.Add(this.bt_add);
            this.gb_ads.Controls.Add(this.tb_ad);
            this.gb_ads.Controls.Add(this.label2);
            this.gb_ads.Controls.Add(this.bt_remove);
            this.gb_ads.Controls.Add(this.lb_ads);
            this.gb_ads.Location = new System.Drawing.Point(236, 20);
            this.gb_ads.Name = "gb_ads";
            this.gb_ads.Size = new System.Drawing.Size(227, 151);
            this.gb_ads.TabIndex = 2;
            this.gb_ads.TabStop = false;
            this.gb_ads.Text = "Custom Aerodromes";
            // 
            // bt_add
            // 
            this.bt_add.Location = new System.Drawing.Point(117, 100);
            this.bt_add.Name = "bt_add";
            this.bt_add.Size = new System.Drawing.Size(103, 23);
            this.bt_add.TabIndex = 4;
            this.bt_add.Text = "Add";
            this.bt_add.UseVisualStyleBackColor = true;
            this.bt_add.Click += new System.EventHandler(this.ADAddClick);
            // 
            // tb_ad
            // 
            this.tb_ad.Location = new System.Drawing.Point(117, 74);
            this.tb_ad.Name = "tb_ad";
            this.tb_ad.Size = new System.Drawing.Size(100, 20);
            this.tb_ad.TabIndex = 3;
            this.tb_ad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ADKeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Aerodrome Name";
            // 
            // bt_remove
            // 
            this.bt_remove.Location = new System.Drawing.Point(118, 20);
            this.bt_remove.Name = "bt_remove";
            this.bt_remove.Size = new System.Drawing.Size(103, 23);
            this.bt_remove.TabIndex = 1;
            this.bt_remove.Text = "Remove";
            this.bt_remove.UseVisualStyleBackColor = true;
            this.bt_remove.Click += new System.EventHandler(this.ADRemoveClick);
            // 
            // lb_ads
            // 
            this.lb_ads.FormattingEnabled = true;
            this.lb_ads.Location = new System.Drawing.Point(7, 20);
            this.lb_ads.Name = "lb_ads";
            this.lb_ads.Size = new System.Drawing.Size(104, 121);
            this.lb_ads.TabIndex = 0;
            // 
            // gb_server
            // 
            this.gb_server.Controls.Add(this.bt_sbset);
            this.gb_server.Controls.Add(this.rb_sb3);
            this.gb_server.Controls.Add(this.rb_sb2);
            this.gb_server.Controls.Add(this.rb_sb1);
            this.gb_server.Controls.Add(this.rb_vatsim);
            this.gb_server.Location = new System.Drawing.Point(6, 18);
            this.gb_server.Name = "gb_server";
            this.gb_server.Size = new System.Drawing.Size(222, 143);
            this.gb_server.TabIndex = 1;
            this.gb_server.TabStop = false;
            this.gb_server.Text = "Server";
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
            // gb_scale
            // 
            this.gb_scale.Controls.Add(this.label4);
            this.gb_scale.Controls.Add(this.label3);
            this.gb_scale.Controls.Add(this.tb_scale);
            this.gb_scale.Location = new System.Drawing.Point(236, 177);
            this.gb_scale.Name = "gb_scale";
            this.gb_scale.Size = new System.Drawing.Size(227, 102);
            this.gb_scale.TabIndex = 5;
            this.gb_scale.TabStop = false;
            this.gb_scale.Text = "Strip Scale";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(188, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "300%";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "100%";
            // 
            // tb_scale
            // 
            this.tb_scale.LargeChange = 100;
            this.tb_scale.Location = new System.Drawing.Point(7, 20);
            this.tb_scale.Maximum = 300;
            this.tb_scale.Minimum = 100;
            this.tb_scale.Name = "tb_scale";
            this.tb_scale.Size = new System.Drawing.Size(213, 45);
            this.tb_scale.TabIndex = 0;
            this.tb_scale.TickFrequency = 50;
            this.tb_scale.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tb_scale.Value = 100;
            // 
            // llb_keyboard
            // 
            this.llb_keyboard.AutoSize = true;
            this.llb_keyboard.Location = new System.Drawing.Point(237, 380);
            this.llb_keyboard.Name = "llb_keyboard";
            this.llb_keyboard.Size = new System.Drawing.Size(107, 13);
            this.llb_keyboard.TabIndex = 3;
            this.llb_keyboard.TabStop = true;
            this.llb_keyboard.Text = "Keyboard Commands";
            this.llb_keyboard.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.KeyboardCommandsOpened);
            // 
            // SettingsWindowControl
            // 
            this.Controls.Add(this.gp_main);
            this.Controls.Add(this.llb_keyboard);
            this.Name = "SettingsWindowControl";
            this.Size = new System.Drawing.Size(476, 403);
            this.gp_main.ResumeLayout(false);
            this.gb_open.ResumeLayout(false);
            this.gb_int.ResumeLayout(false);
            this.gb_int.PerformLayout();
            this.gb_ads.ResumeLayout(false);
            this.gb_ads.PerformLayout();
            this.gb_server.ResumeLayout(false);
            this.gb_server.PerformLayout();
            this.gb_scale.ResumeLayout(false);
            this.gb_scale.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_scale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.GroupBox gp_main;
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
        private System.Windows.Forms.GroupBox gb_scale;
        private System.Windows.Forms.TrackBar tb_scale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gb_int;
        private System.Windows.Forms.CheckBox cb_keeppicked;
        private System.Windows.Forms.CheckBox cb_preasort;
        private System.Windows.Forms.GroupBox gb_open;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cb_open;
        private System.Windows.Forms.CheckBox cb_lasttransmit;
    }
}
