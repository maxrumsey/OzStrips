namespace maxrumsey.ozstrips.gui
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            pl_controlbar = new System.Windows.Forms.Panel();
            bt_pdc = new System.Windows.Forms.Button();
            bt_cross = new System.Windows.Forms.Button();
            bt_force = new System.Windows.Forms.Button();
            bt_inhibit = new System.Windows.Forms.Button();
            pl_ad = new System.Windows.Forms.Panel();
            lb_ad = new System.Windows.Forms.Label();
            pl_stat = new System.Windows.Forms.Panel();
            lb_stat = new System.Windows.Forms.Label();
            tb_Time = new System.Windows.Forms.TextBox();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            ts_ad = new System.Windows.Forms.ToolStripMenuItem();
            toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ts_mode = new System.Windows.Forms.ToolStripMenuItem();
            aCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sMCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sMCACDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aDCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aDCSMCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            flp_main = new System.Windows.Forms.FlowLayoutPanel();
            tt_metar = new System.Windows.Forms.ToolTip(components);
            pl_controlbar.SuspendLayout();
            pl_ad.SuspendLayout();
            pl_stat.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // pl_controlbar
            // 
            pl_controlbar.BackColor = System.Drawing.Color.FromArgb(160, 170, 170);
            pl_controlbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_controlbar.Controls.Add(bt_pdc);
            pl_controlbar.Controls.Add(bt_cross);
            pl_controlbar.Controls.Add(bt_force);
            pl_controlbar.Controls.Add(bt_inhibit);
            pl_controlbar.Controls.Add(pl_ad);
            pl_controlbar.Controls.Add(pl_stat);
            pl_controlbar.Controls.Add(tb_Time);
            pl_controlbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            pl_controlbar.Location = new System.Drawing.Point(0, 916);
            pl_controlbar.Name = "pl_controlbar";
            pl_controlbar.Size = new System.Drawing.Size(1784, 45);
            pl_controlbar.TabIndex = 0;
            // 
            // bt_pdc
            // 
            bt_pdc.BackColor = System.Drawing.Color.FromArgb(140, 150, 150);
            bt_pdc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            bt_pdc.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            bt_pdc.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            bt_pdc.Location = new System.Drawing.Point(687, 3);
            bt_pdc.Name = "bt_pdc";
            bt_pdc.Size = new System.Drawing.Size(96, 37);
            bt_pdc.TabIndex = 6;
            bt_pdc.TabStop = false;
            bt_pdc.Text = "PDC";
            bt_pdc.UseVisualStyleBackColor = false;
            bt_pdc.Click += bt_pdc_Click;
            // 
            // bt_cross
            // 
            bt_cross.BackColor = System.Drawing.Color.RosyBrown;
            bt_cross.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            bt_cross.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            bt_cross.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            bt_cross.Location = new System.Drawing.Point(543, 3);
            bt_cross.Name = "bt_cross";
            bt_cross.Size = new System.Drawing.Size(142, 37);
            bt_cross.TabIndex = 5;
            bt_cross.TabStop = false;
            bt_cross.Text = "XX CROSS XX";
            bt_cross.UseVisualStyleBackColor = false;
            bt_cross.Click += bt_cross_Click;
            // 
            // bt_force
            // 
            bt_force.BackColor = System.Drawing.Color.FromArgb(140, 150, 150);
            bt_force.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            bt_force.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            bt_force.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            bt_force.Location = new System.Drawing.Point(445, 3);
            bt_force.Name = "bt_force";
            bt_force.Size = new System.Drawing.Size(96, 37);
            bt_force.TabIndex = 4;
            bt_force.TabStop = false;
            bt_force.Text = "FOR STP";
            bt_force.UseVisualStyleBackColor = false;
            bt_force.Click += bt_force_Click;
            // 
            // bt_inhibit
            // 
            bt_inhibit.BackColor = System.Drawing.Color.FromArgb(140, 150, 150);
            bt_inhibit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            bt_inhibit.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            bt_inhibit.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            bt_inhibit.Location = new System.Drawing.Point(347, 3);
            bt_inhibit.Name = "bt_inhibit";
            bt_inhibit.Size = new System.Drawing.Size(96, 37);
            bt_inhibit.TabIndex = 3;
            bt_inhibit.TabStop = false;
            bt_inhibit.Text = "INHIBIT";
            bt_inhibit.UseVisualStyleBackColor = false;
            bt_inhibit.Click += bt_inhibit_Click;
            // 
            // pl_ad
            // 
            pl_ad.BackColor = System.Drawing.Color.DarkGray;
            pl_ad.Controls.Add(lb_ad);
            pl_ad.Location = new System.Drawing.Point(247, 3);
            pl_ad.Name = "pl_ad";
            pl_ad.Size = new System.Drawing.Size(96, 37);
            pl_ad.TabIndex = 2;
            // 
            // lb_ad
            // 
            lb_ad.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_ad.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_ad.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            lb_ad.Location = new System.Drawing.Point(0, 0);
            lb_ad.Name = "lb_ad";
            lb_ad.Size = new System.Drawing.Size(96, 37);
            lb_ad.TabIndex = 0;
            lb_ad.Text = "????";
            lb_ad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pl_stat
            // 
            pl_stat.BackColor = System.Drawing.Color.OrangeRed;
            pl_stat.Controls.Add(lb_stat);
            pl_stat.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            pl_stat.Location = new System.Drawing.Point(4, 3);
            pl_stat.Name = "pl_stat";
            pl_stat.Size = new System.Drawing.Size(96, 37);
            pl_stat.TabIndex = 1;
            // 
            // lb_stat
            // 
            lb_stat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lb_stat.AutoSize = true;
            lb_stat.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_stat.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            lb_stat.Location = new System.Drawing.Point(7, 11);
            lb_stat.Name = "lb_stat";
            lb_stat.Size = new System.Drawing.Size(80, 17);
            lb_stat.TabIndex = 0;
            lb_stat.Text = "CONN STAT";
            // 
            // tb_Time
            // 
            tb_Time.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            tb_Time.BackColor = System.Drawing.SystemColors.Info;
            tb_Time.Enabled = false;
            tb_Time.Font = new System.Drawing.Font("Terminus (TTF)", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            tb_Time.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            tb_Time.Location = new System.Drawing.Point(106, 3);
            tb_Time.Name = "tb_Time";
            tb_Time.ReadOnly = true;
            tb_Time.Size = new System.Drawing.Size(137, 37);
            tb_Time.TabIndex = 0;
            tb_Time.Text = "Time";
            tb_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = System.Drawing.Color.FromArgb(160, 170, 170);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ts_ad, ts_mode, debugToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(1784, 25);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += menuStrip1_ItemClicked;
            // 
            // ts_ad
            // 
            ts_ad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripTextBox1, toolStripSeparator1 });
            ts_ad.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            ts_ad.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            ts_ad.Name = "ts_ad";
            ts_ad.Size = new System.Drawing.Size(92, 21);
            ts_ad.Text = "Aerodrome";
            ts_ad.Click += ts_ad_Click;
            // 
            // toolStripTextBox1
            // 
            toolStripTextBox1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            toolStripTextBox1.MaxLength = 4;
            toolStripTextBox1.Name = "toolStripTextBox1";
            toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            toolStripTextBox1.ToolTipText = "Aerodrome";
            toolStripTextBox1.KeyPress += toolStripTextBox1_KeyPress;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // ts_mode
            // 
            ts_mode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aCDToolStripMenuItem, sMCToolStripMenuItem, sMCACDToolStripMenuItem, aDCToolStripMenuItem, aDCSMCToolStripMenuItem, allToolStripMenuItem });
            ts_mode.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            ts_mode.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            ts_mode.Name = "ts_mode";
            ts_mode.Size = new System.Drawing.Size(92, 21);
            ts_mode.Text = "View Mode";
            // 
            // aCDToolStripMenuItem
            // 
            aCDToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            aCDToolStripMenuItem.Name = "aCDToolStripMenuItem";
            aCDToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            aCDToolStripMenuItem.Text = "ACD";
            aCDToolStripMenuItem.Click += aCDToolStripMenuItem_Click;
            // 
            // sMCToolStripMenuItem
            // 
            sMCToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            sMCToolStripMenuItem.Name = "sMCToolStripMenuItem";
            sMCToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            sMCToolStripMenuItem.Text = "SMC";
            sMCToolStripMenuItem.Click += sMCToolStripMenuItem_Click;
            // 
            // sMCACDToolStripMenuItem
            // 
            sMCACDToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            sMCACDToolStripMenuItem.Name = "sMCACDToolStripMenuItem";
            sMCACDToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            sMCACDToolStripMenuItem.Text = "SMC+ACD";
            sMCACDToolStripMenuItem.Click += sMCACDToolStripMenuItem_Click;
            // 
            // aDCToolStripMenuItem
            // 
            aDCToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            aDCToolStripMenuItem.Name = "aDCToolStripMenuItem";
            aDCToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            aDCToolStripMenuItem.Text = "ADC";
            aDCToolStripMenuItem.Click += aDCToolStripMenuItem_Click;
            // 
            // aDCSMCToolStripMenuItem
            // 
            aDCSMCToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            aDCSMCToolStripMenuItem.Name = "aDCSMCToolStripMenuItem";
            aDCSMCToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            aDCSMCToolStripMenuItem.Text = "ADC+SMC";
            aDCSMCToolStripMenuItem.Click += aDCSMCToolStripMenuItem_Click;
            // 
            // allToolStripMenuItem
            // 
            allToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            allToolStripMenuItem.Name = "allToolStripMenuItem";
            allToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            allToolStripMenuItem.Text = "All";
            allToolStripMenuItem.Click += allToolStripMenuItem_Click;
            // 
            // debugToolStripMenuItem
            // 
            debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1 });
            debugToolStripMenuItem.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            debugToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            debugToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            debugToolStripMenuItem.Text = "Debug";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            toolStripMenuItem1.Text = "SocketIO Log";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            aboutToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // flp_main
            // 
            flp_main.AutoScroll = true;
            flp_main.BackColor = System.Drawing.Color.FromArgb(160, 170, 170);
            flp_main.Dock = System.Windows.Forms.DockStyle.Fill;
            flp_main.Location = new System.Drawing.Point(0, 25);
            flp_main.Margin = new System.Windows.Forms.Padding(0);
            flp_main.Name = "flp_main";
            flp_main.Size = new System.Drawing.Size(1784, 891);
            flp_main.TabIndex = 2;
            flp_main.WrapContents = false;
            // 
            // tt_metar
            // 
            tt_metar.ToolTipTitle = "METAR";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Gray;
            ClientSize = new System.Drawing.Size(1784, 961);
            Controls.Add(flp_main);
            Controls.Add(pl_controlbar);
            Controls.Add(menuStrip1);
            DoubleBuffered = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "OzStrips";
            FormClosed += MainForm_FormClosed;
            SizeChanged += MainForm_SizeChanged;
            pl_controlbar.ResumeLayout(false);
            pl_controlbar.PerformLayout();
            pl_ad.ResumeLayout(false);
            pl_stat.ResumeLayout(false);
            pl_stat.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pl_controlbar;
        private System.Windows.Forms.TextBox tb_Time;
        private System.Windows.Forms.Panel pl_stat;
        private System.Windows.Forms.Label lb_stat;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.FlowLayoutPanel flp_main;
        private System.Windows.Forms.ToolStripMenuItem ts_ad;
        private System.Windows.Forms.Panel pl_ad;
        private System.Windows.Forms.Label lb_ad;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.Button bt_inhibit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button bt_force;
        private System.Windows.Forms.ToolStripMenuItem ts_mode;
        private System.Windows.Forms.ToolStripMenuItem aCDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sMCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sMCACDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aDCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aDCSMCToolStripMenuItem;
        private System.Windows.Forms.Button bt_cross;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button bt_pdc;
        private System.Windows.Forms.ToolTip tt_metar;
    }
}

