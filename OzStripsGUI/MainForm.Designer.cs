using MaxRumsey.OzStripsPlugin.Gui.Controls;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pl_controlbar = new System.Windows.Forms.Panel();
            this.bt_flip = new System.Windows.Forms.Button();
            this.bt_bar = new System.Windows.Forms.Button();
            this.pl_atis = new System.Windows.Forms.Panel();
            this.lb_atis = new System.Windows.Forms.Label();
            this.bt_cross = new System.Windows.Forms.Button();
            this.bt_inhibit = new System.Windows.Forms.Button();
            this.pl_ad = new System.Windows.Forms.Panel();
            this.lb_ad = new System.Windows.Forms.Label();
            this.pl_stat = new System.Windows.Forms.Panel();
            this.tb_Time = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ts_ad = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_mode = new System.Windows.Forms.ToolStripMenuItem();
            this.smartResizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.threeColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twoColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oneColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colDisabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_toggleCircuit = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changelogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadStripToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadAerodromeListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tt_metar = new System.Windows.Forms.ToolTip(this.components);
            this.tt_clients = new System.Windows.Forms.ToolTip(this.components);
            this.pl_controlbar.SuspendLayout();
            this.pl_atis.SuspendLayout();
            this.pl_ad.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pl_controlbar
            // 
            this.pl_controlbar.AutoScroll = true;
            this.pl_controlbar.AutoSize = true;
            this.pl_controlbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.pl_controlbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pl_controlbar.Controls.Add(this.bt_flip);
            this.pl_controlbar.Controls.Add(this.bt_bar);
            this.pl_controlbar.Controls.Add(this.pl_atis);
            this.pl_controlbar.Controls.Add(this.bt_cross);
            this.pl_controlbar.Controls.Add(this.bt_inhibit);
            this.pl_controlbar.Controls.Add(this.pl_ad);
            this.pl_controlbar.Controls.Add(this.pl_stat);
            this.pl_controlbar.Controls.Add(this.tb_Time);
            this.pl_controlbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pl_controlbar.Location = new System.Drawing.Point(0, 916);
            this.pl_controlbar.MinimumSize = new System.Drawing.Size(2, 45);
            this.pl_controlbar.Name = "pl_controlbar";
            this.pl_controlbar.Size = new System.Drawing.Size(1784, 45);
            this.pl_controlbar.TabIndex = 0;
            // 
            // bt_flip
            // 
            this.bt_flip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.bt_flip.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_flip.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_flip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.bt_flip.Location = new System.Drawing.Point(742, 3);
            this.bt_flip.Name = "bt_flip";
            this.bt_flip.Size = new System.Drawing.Size(96, 37);
            this.bt_flip.TabIndex = 8;
            this.bt_flip.TabStop = false;
            this.bt_flip.Text = "FLIP FLOP";
            this.bt_flip.UseVisualStyleBackColor = false;
            // 
            // bt_bar
            // 
            this.bt_bar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.bt_bar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_bar.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_bar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.bt_bar.Location = new System.Drawing.Point(642, 3);
            this.bt_bar.Name = "bt_bar";
            this.bt_bar.Size = new System.Drawing.Size(96, 37);
            this.bt_bar.TabIndex = 7;
            this.bt_bar.TabStop = false;
            this.bt_bar.Text = "ADD BAR";
            this.bt_bar.UseVisualStyleBackColor = false;
            // 
            // pl_atis
            // 
            this.pl_atis.BackColor = System.Drawing.Color.DarkGray;
            this.pl_atis.Controls.Add(this.lb_atis);
            this.pl_atis.Location = new System.Drawing.Point(349, 3);
            this.pl_atis.Name = "pl_atis";
            this.pl_atis.Size = new System.Drawing.Size(42, 37);
            this.pl_atis.TabIndex = 3;
            // 
            // lb_atis
            // 
            this.lb_atis.BackColor = System.Drawing.Color.Silver;
            this.lb_atis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_atis.Font = new System.Drawing.Font("Terminus (TTF)", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_atis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.lb_atis.Location = new System.Drawing.Point(0, 0);
            this.lb_atis.Name = "lb_atis";
            this.lb_atis.Size = new System.Drawing.Size(42, 37);
            this.lb_atis.TabIndex = 0;
            this.lb_atis.Text = "Z";
            this.lb_atis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bt_cross
            // 
            this.bt_cross.BackColor = System.Drawing.Color.RosyBrown;
            this.bt_cross.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_cross.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_cross.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.bt_cross.Location = new System.Drawing.Point(496, 3);
            this.bt_cross.Name = "bt_cross";
            this.bt_cross.Size = new System.Drawing.Size(142, 37);
            this.bt_cross.TabIndex = 5;
            this.bt_cross.TabStop = false;
            this.bt_cross.Text = "XX CROSS XX";
            this.bt_cross.UseVisualStyleBackColor = false;
            // 
            // bt_inhibit
            // 
            this.bt_inhibit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.bt_inhibit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_inhibit.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_inhibit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.bt_inhibit.Location = new System.Drawing.Point(396, 3);
            this.bt_inhibit.Name = "bt_inhibit";
            this.bt_inhibit.Size = new System.Drawing.Size(96, 37);
            this.bt_inhibit.TabIndex = 3;
            this.bt_inhibit.TabStop = false;
            this.bt_inhibit.Text = "INHIBIT";
            this.bt_inhibit.UseVisualStyleBackColor = false;
            // 
            // pl_ad
            // 
            this.pl_ad.BackColor = System.Drawing.Color.DarkGray;
            this.pl_ad.Controls.Add(this.lb_ad);
            this.pl_ad.Location = new System.Drawing.Point(247, 3);
            this.pl_ad.Name = "pl_ad";
            this.pl_ad.Size = new System.Drawing.Size(96, 37);
            this.pl_ad.TabIndex = 2;
            // 
            // lb_ad
            // 
            this.lb_ad.BackColor = System.Drawing.Color.Silver;
            this.lb_ad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_ad.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.lb_ad.Location = new System.Drawing.Point(0, 0);
            this.lb_ad.Name = "lb_ad";
            this.lb_ad.Size = new System.Drawing.Size(96, 37);
            this.lb_ad.TabIndex = 0;
            this.lb_ad.Text = "????";
            this.lb_ad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pl_stat
            // 
            this.pl_stat.BackColor = System.Drawing.Color.OrangeRed;
            this.pl_stat.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pl_stat.Location = new System.Drawing.Point(4, 3);
            this.pl_stat.Name = "pl_stat";
            this.pl_stat.Size = new System.Drawing.Size(96, 37);
            this.pl_stat.TabIndex = 1;
            // 
            // tb_Time
            // 
            this.tb_Time.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_Time.BackColor = System.Drawing.SystemColors.Info;
            this.tb_Time.Enabled = false;
            this.tb_Time.Font = new System.Drawing.Font("Terminus (TTF)", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Time.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.tb_Time.Location = new System.Drawing.Point(106, 3);
            this.tb_Time.Name = "tb_Time";
            this.tb_Time.ReadOnly = true;
            this.tb_Time.Size = new System.Drawing.Size(137, 37);
            this.tb_Time.TabIndex = 0;
            this.tb_Time.Text = "Time";
            this.tb_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ts_ad,
            this.viewToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1784, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ts_ad
            // 
            this.ts_ad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyToolStripMenuItem,
            this.toolStripTextBox1,
            this.toolStripSeparator1});
            this.ts_ad.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ts_ad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.ts_ad.Name = "ts_ad";
            this.ts_ad.Size = new System.Drawing.Size(92, 21);
            this.ts_ad.Text = "Aerodrome";
            // 
            // modifyToolStripMenuItem
            // 
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            this.modifyToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.modifyToolStripMenuItem.Tag = "permanent";
            this.modifyToolStripMenuItem.Text = "Modify";
            this.modifyToolStripMenuItem.Click += new System.EventHandler(this.ModifyButtonClicked);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.Tag = "permanent";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            this.toolStripSeparator1.Tag = "permanent";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ts_mode,
            this.smartResizeToolStripMenuItem,
            this.ts_toggleCircuit});
            this.viewToolStripMenuItem.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(52, 21);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // ts_mode
            // 
            this.ts_mode.Name = "ts_mode";
            this.ts_mode.Size = new System.Drawing.Size(220, 22);
            this.ts_mode.Text = "View Mode";
            // 
            // smartResizeToolStripMenuItem
            // 
            this.smartResizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.threeColumnsToolStripMenuItem,
            this.twoColumnsToolStripMenuItem,
            this.oneColumnToolStripMenuItem,
            this.colDisabledToolStripMenuItem});
            this.smartResizeToolStripMenuItem.Name = "smartResizeToolStripMenuItem";
            this.smartResizeToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.smartResizeToolStripMenuItem.Text = "Smart Resize";
            // 
            // threeColumnsToolStripMenuItem
            // 
            this.threeColumnsToolStripMenuItem.Name = "threeColumnsToolStripMenuItem";
            this.threeColumnsToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.threeColumnsToolStripMenuItem.Text = "3 Columns or Less";
            this.threeColumnsToolStripMenuItem.Click += new System.EventHandler(this.ThreeColumnsToolStripMenuItem_Click);
            // 
            // twoColumnsToolStripMenuItem
            // 
            this.twoColumnsToolStripMenuItem.Name = "twoColumnsToolStripMenuItem";
            this.twoColumnsToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.twoColumnsToolStripMenuItem.Text = "2 Columns or Less";
            this.twoColumnsToolStripMenuItem.Click += new System.EventHandler(this.TwoColumnsToolStripMenuItem_Click);
            // 
            // oneColumnToolStripMenuItem
            // 
            this.oneColumnToolStripMenuItem.Name = "oneColumnToolStripMenuItem";
            this.oneColumnToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.oneColumnToolStripMenuItem.Text = "1 Column";
            this.oneColumnToolStripMenuItem.Click += new System.EventHandler(this.OneColumnToolStripMenuItem_Click);
            // 
            // colDisabledToolStripMenuItem
            // 
            this.colDisabledToolStripMenuItem.Name = "colDisabledToolStripMenuItem";
            this.colDisabledToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.colDisabledToolStripMenuItem.Text = "Disabled";
            this.colDisabledToolStripMenuItem.Click += new System.EventHandler(this.ColDisabledToolStripMenuItem_Click);
            // 
            // ts_toggleCircuit
            // 
            this.ts_toggleCircuit.Name = "ts_toggleCircuit";
            this.ts_toggleCircuit.Size = new System.Drawing.Size(220, 22);
            this.ts_toggleCircuit.Text = "Toggle Circuit Bay";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(84, 21);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gitHubToolStripMenuItem,
            this.documentationToolStripMenuItem,
            this.changelogToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.toolStripSeparator2,
            this.debugToolStripMenuItem});
            this.helpToolStripMenuItem.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(52, 21);
            this.helpToolStripMenuItem.Text = "Info";
            // 
            // gitHubToolStripMenuItem
            // 
            this.gitHubToolStripMenuItem.Name = "gitHubToolStripMenuItem";
            this.gitHubToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.gitHubToolStripMenuItem.Text = "GitHub";
            this.gitHubToolStripMenuItem.Click += new System.EventHandler(this.GitHubToolStripMenuItem_Click);
            // 
            // documentationToolStripMenuItem
            // 
            this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            this.documentationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.documentationToolStripMenuItem.Text = "Documentation";
            this.documentationToolStripMenuItem.Click += new System.EventHandler(this.DocumentationToolStripMenuItem_Click);
            // 
            // changelogToolStripMenuItem
            // 
            this.changelogToolStripMenuItem.Name = "changelogToolStripMenuItem";
            this.changelogToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.changelogToolStripMenuItem.Text = "Changelog";
            this.changelogToolStripMenuItem.Click += new System.EventHandler(this.ChangelogToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.reloadStripToolStripMenuItem,
            this.reloadAerodromeListToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(228, 22);
            this.toolStripMenuItem1.Text = "SignalR Log";
            // 
            // reloadStripToolStripMenuItem
            // 
            this.reloadStripToolStripMenuItem.Name = "reloadStripToolStripMenuItem";
            this.reloadStripToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.reloadStripToolStripMenuItem.Text = "ReloadStrip";
            this.reloadStripToolStripMenuItem.Click += new System.EventHandler(this.ReloadStripItem);
            // 
            // reloadAerodromeListToolStripMenuItem
            // 
            this.reloadAerodromeListToolStripMenuItem.Name = "reloadAerodromeListToolStripMenuItem";
            this.reloadAerodromeListToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.reloadAerodromeListToolStripMenuItem.Text = "ReloadAerodromeList";
            this.reloadAerodromeListToolStripMenuItem.Click += new System.EventHandler(this.ReloadAerodromes);
            // 
            // tt_metar
            // 
            this.tt_metar.ToolTipTitle = "METAR";
            // 
            // tt_clients
            // 
            this.tt_clients.ToolTipTitle = "Online Clients";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1784, 961);
            this.Controls.Add(this.pl_controlbar);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "OzStrips";
            this.pl_controlbar.ResumeLayout(false);
            this.pl_controlbar.PerformLayout();
            this.pl_atis.ResumeLayout(false);
            this.pl_ad.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pl_controlbar;
        private System.Windows.Forms.TextBox tb_Time;
        private System.Windows.Forms.Panel pl_stat;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel pl_ad;
        private System.Windows.Forms.Label lb_ad;
        private System.Windows.Forms.Button bt_inhibit;
        private System.Windows.Forms.ToolStripMenuItem ts_mode;
        private System.Windows.Forms.Button bt_cross;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolTip tt_metar;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gitHubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.Panel pl_atis;
        private System.Windows.Forms.Label lb_atis;
        private System.Windows.Forms.ToolStripMenuItem changelogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem ts_ad;
        private ToolStripMenuItem modifyToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripMenuItem reloadStripToolStripMenuItem;
        private Button bt_bar;
        private ToolStripMenuItem smartResizeToolStripMenuItem;
        private ToolStripMenuItem threeColumnsToolStripMenuItem;
        private ToolStripMenuItem twoColumnsToolStripMenuItem;
        private ToolStripMenuItem oneColumnToolStripMenuItem;
        private ToolStripMenuItem colDisabledToolStripMenuItem;
        private Button bt_flip;
        private ToolStripMenuItem reloadAerodromeListToolStripMenuItem;
        private ToolStripMenuItem ts_toggleCircuit;
        private ToolTip tt_clients;
    }
}
