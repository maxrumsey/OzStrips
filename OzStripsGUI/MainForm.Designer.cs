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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pl_controlbar = new System.Windows.Forms.Panel();
            this.bt_force = new System.Windows.Forms.Button();
            this.bt_inhibit = new System.Windows.Forms.Button();
            this.pl_ad = new System.Windows.Forms.Panel();
            this.lb_ad = new System.Windows.Forms.Label();
            this.pl_stat = new System.Windows.Forms.Panel();
            this.lb_stat = new System.Windows.Forms.Label();
            this.tb_Time = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.forceRerenderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_ad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ts_mode = new System.Windows.Forms.ToolStripMenuItem();
            this.aCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMCACDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aDCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aDCSMCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flp_main = new System.Windows.Forms.FlowLayoutPanel();
            this.bt_cross = new System.Windows.Forms.Button();
            this.pl_controlbar.SuspendLayout();
            this.pl_ad.SuspendLayout();
            this.pl_stat.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pl_controlbar
            // 
            this.pl_controlbar.BackColor = System.Drawing.Color.Gainsboro;
            this.pl_controlbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pl_controlbar.Controls.Add(this.bt_cross);
            this.pl_controlbar.Controls.Add(this.bt_force);
            this.pl_controlbar.Controls.Add(this.bt_inhibit);
            this.pl_controlbar.Controls.Add(this.pl_ad);
            this.pl_controlbar.Controls.Add(this.pl_stat);
            this.pl_controlbar.Controls.Add(this.tb_Time);
            this.pl_controlbar.Cursor = System.Windows.Forms.Cursors.Default;
            this.pl_controlbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pl_controlbar.Location = new System.Drawing.Point(0, 916);
            this.pl_controlbar.Name = "pl_controlbar";
            this.pl_controlbar.Size = new System.Drawing.Size(1784, 45);
            this.pl_controlbar.TabIndex = 0;
            // 
            // bt_force
            // 
            this.bt_force.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_force.Location = new System.Drawing.Point(445, 3);
            this.bt_force.Name = "bt_force";
            this.bt_force.Size = new System.Drawing.Size(96, 37);
            this.bt_force.TabIndex = 4;
            this.bt_force.TabStop = false;
            this.bt_force.Text = "FOR STP";
            this.bt_force.UseVisualStyleBackColor = true;
            this.bt_force.Click += new System.EventHandler(this.bt_force_Click);
            // 
            // bt_inhibit
            // 
            this.bt_inhibit.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_inhibit.Location = new System.Drawing.Point(347, 3);
            this.bt_inhibit.Name = "bt_inhibit";
            this.bt_inhibit.Size = new System.Drawing.Size(96, 37);
            this.bt_inhibit.TabIndex = 3;
            this.bt_inhibit.TabStop = false;
            this.bt_inhibit.Text = "INHIBIT";
            this.bt_inhibit.UseVisualStyleBackColor = true;
            this.bt_inhibit.Click += new System.EventHandler(this.bt_inhibit_Click);
            // 
            // pl_ad
            // 
            this.pl_ad.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pl_ad.Controls.Add(this.lb_ad);
            this.pl_ad.Location = new System.Drawing.Point(247, 3);
            this.pl_ad.Name = "pl_ad";
            this.pl_ad.Size = new System.Drawing.Size(96, 37);
            this.pl_ad.TabIndex = 2;
            // 
            // lb_ad
            // 
            this.lb_ad.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_ad.AutoSize = true;
            this.lb_ad.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ad.Location = new System.Drawing.Point(28, 11);
            this.lb_ad.Name = "lb_ad";
            this.lb_ad.Size = new System.Drawing.Size(39, 16);
            this.lb_ad.TabIndex = 0;
            this.lb_ad.Text = "YMML";
            // 
            // pl_stat
            // 
            this.pl_stat.BackColor = System.Drawing.Color.OrangeRed;
            this.pl_stat.Controls.Add(this.lb_stat);
            this.pl_stat.Location = new System.Drawing.Point(4, 3);
            this.pl_stat.Name = "pl_stat";
            this.pl_stat.Size = new System.Drawing.Size(96, 37);
            this.pl_stat.TabIndex = 1;
            // 
            // lb_stat
            // 
            this.lb_stat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_stat.AutoSize = true;
            this.lb_stat.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_stat.Location = new System.Drawing.Point(7, 11);
            this.lb_stat.Name = "lb_stat";
            this.lb_stat.Size = new System.Drawing.Size(79, 16);
            this.lb_stat.TabIndex = 0;
            this.lb_stat.Text = "CONN STAT";
            // 
            // tb_Time
            // 
            this.tb_Time.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tb_Time.BackColor = System.Drawing.SystemColors.Info;
            this.tb_Time.Enabled = false;
            this.tb_Time.Font = new System.Drawing.Font("Terminus (TTF)", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Time.Location = new System.Drawing.Point(106, 3);
            this.tb_Time.Name = "tb_Time";
            this.tb_Time.ReadOnly = true;
            this.tb_Time.Size = new System.Drawing.Size(137, 37);
            this.tb_Time.TabIndex = 0;
            this.tb_Time.Text = "Time";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forceRerenderToolStripMenuItem,
            this.ts_ad,
            this.ts_mode});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1784, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // forceRerenderToolStripMenuItem
            // 
            this.forceRerenderToolStripMenuItem.Name = "forceRerenderToolStripMenuItem";
            this.forceRerenderToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            this.forceRerenderToolStripMenuItem.Text = "Force Rerender";
            this.forceRerenderToolStripMenuItem.Click += new System.EventHandler(this.forceRerenderToolStripMenuItem_Click);
            // 
            // ts_ad
            // 
            this.ts_ad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.toolStripSeparator1});
            this.ts_ad.Name = "ts_ad";
            this.ts_ad.Size = new System.Drawing.Size(79, 20);
            this.ts_ad.Text = "Aerodrome";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox1.MaxLength = 4;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.ToolTipText = "Aerodrome";
            this.toolStripTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox1_KeyPress);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // ts_mode
            // 
            this.ts_mode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aCDToolStripMenuItem,
            this.sMCToolStripMenuItem,
            this.sMCACDToolStripMenuItem,
            this.aDCToolStripMenuItem,
            this.aDCSMCToolStripMenuItem,
            this.allToolStripMenuItem});
            this.ts_mode.Name = "ts_mode";
            this.ts_mode.Size = new System.Drawing.Size(78, 20);
            this.ts_mode.Text = "View Mode";
            // 
            // aCDToolStripMenuItem
            // 
            this.aCDToolStripMenuItem.Name = "aCDToolStripMenuItem";
            this.aCDToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.aCDToolStripMenuItem.Text = "ACD";
            this.aCDToolStripMenuItem.Click += new System.EventHandler(this.aCDToolStripMenuItem_Click);
            // 
            // sMCToolStripMenuItem
            // 
            this.sMCToolStripMenuItem.Name = "sMCToolStripMenuItem";
            this.sMCToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.sMCToolStripMenuItem.Text = "SMC";
            this.sMCToolStripMenuItem.Click += new System.EventHandler(this.sMCToolStripMenuItem_Click);
            // 
            // sMCACDToolStripMenuItem
            // 
            this.sMCACDToolStripMenuItem.Name = "sMCACDToolStripMenuItem";
            this.sMCACDToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.sMCACDToolStripMenuItem.Text = "SMC+ACD";
            this.sMCACDToolStripMenuItem.Click += new System.EventHandler(this.sMCACDToolStripMenuItem_Click);
            // 
            // aDCToolStripMenuItem
            // 
            this.aDCToolStripMenuItem.Name = "aDCToolStripMenuItem";
            this.aDCToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.aDCToolStripMenuItem.Text = "ADC";
            this.aDCToolStripMenuItem.Click += new System.EventHandler(this.aDCToolStripMenuItem_Click);
            // 
            // aDCSMCToolStripMenuItem
            // 
            this.aDCSMCToolStripMenuItem.Name = "aDCSMCToolStripMenuItem";
            this.aDCSMCToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.aDCSMCToolStripMenuItem.Text = "ADC+SMC";
            this.aDCSMCToolStripMenuItem.Click += new System.EventHandler(this.aDCSMCToolStripMenuItem_Click);
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // flp_main
            // 
            this.flp_main.AutoScroll = true;
            this.flp_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flp_main.Location = new System.Drawing.Point(0, 24);
            this.flp_main.Margin = new System.Windows.Forms.Padding(0);
            this.flp_main.Name = "flp_main";
            this.flp_main.Size = new System.Drawing.Size(1784, 892);
            this.flp_main.TabIndex = 2;
            this.flp_main.WrapContents = false;
            // 
            // bt_cross
            // 
            this.bt_cross.BackColor = System.Drawing.Color.RosyBrown;
            this.bt_cross.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_cross.Font = new System.Drawing.Font("Terminus (TTF)", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_cross.Location = new System.Drawing.Point(543, 3);
            this.bt_cross.Name = "bt_cross";
            this.bt_cross.Size = new System.Drawing.Size(142, 37);
            this.bt_cross.TabIndex = 5;
            this.bt_cross.TabStop = false;
            this.bt_cross.Text = "XX CROSS XX";
            this.bt_cross.UseVisualStyleBackColor = false;
            this.bt_cross.Click += new System.EventHandler(this.bt_cross_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1784, 961);
            this.Controls.Add(this.flp_main);
            this.Controls.Add(this.pl_controlbar);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "OzStrips";
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.pl_controlbar.ResumeLayout(false);
            this.pl_controlbar.PerformLayout();
            this.pl_ad.ResumeLayout(false);
            this.pl_ad.PerformLayout();
            this.pl_stat.ResumeLayout(false);
            this.pl_stat.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pl_controlbar;
        private System.Windows.Forms.TextBox tb_Time;
        private System.Windows.Forms.Panel pl_stat;
        private System.Windows.Forms.Label lb_stat;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem forceRerenderToolStripMenuItem;
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
    }
}

