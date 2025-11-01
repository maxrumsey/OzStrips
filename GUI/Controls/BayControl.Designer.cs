
using MaxRumsey.OzStripsPlugin.GUI.Controls;

namespace MaxRumsey.OzStripsPlugin.GUI.Controls
{
    partial class BayControl
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
            this.basePanel = new System.Windows.Forms.Panel();
            this.pl_main = new NoScrollPanel();
            this.topPanel = new System.Windows.Forms.Panel();
            this.bt_div = new System.Windows.Forms.Button();
            this.bt_queue = new System.Windows.Forms.Button();
            this.lb_bay_name = new System.Windows.Forms.Label();
            this.basePanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.basePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.basePanel.Controls.Add(this.pl_main);
            this.basePanel.Controls.Add(this.topPanel);
            this.basePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basePanel.Location = new System.Drawing.Point(0, 0);
            this.basePanel.Name = "panel1";
            this.basePanel.Size = new System.Drawing.Size(481, 350);
            this.basePanel.TabIndex = 0;
            this.basePanel.MouseMove += new(PanelMouseMove);
            // 
            // pl_main
            // 
            this.pl_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pl_main.Location = new System.Drawing.Point(0, 36);
            this.pl_main.Name = "pl_main";
            this.pl_main.Size = new System.Drawing.Size(481, 314);
            this.pl_main.TabIndex = 1;
            this.pl_main.Click += new System.EventHandler(this.LabelBayNameClicked);
            this.pl_main.MouseMove += new(PanelMouseMove);
            // 
            // panel2
            // 
            this.topPanel.BackColor = System.Drawing.Color.Black;
            this.topPanel.Controls.Add(this.bt_div);
            this.topPanel.Controls.Add(this.bt_queue);
            this.topPanel.Controls.Add(this.lb_bay_name);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "panel2";
            this.topPanel.Size = new System.Drawing.Size(481, 36);
            this.topPanel.TabIndex = 0;
            this.topPanel.MouseMove += new(PanelMouseMove);
            // 
            // bt_div
            // 
            this.bt_div.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_div.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.bt_div.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_div.Font = new System.Drawing.Font("Terminus (TTF)", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_div.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.bt_div.Location = new System.Drawing.Point(330, 5);
            this.bt_div.Name = "bt_div";
            this.bt_div.Size = new System.Drawing.Size(145, 24);
            this.bt_div.TabIndex = 3;
            this.bt_div.TabStop = false;
            this.bt_div.Text = "Toggle Queue Bar";
            this.bt_div.UseVisualStyleBackColor = false;
            this.bt_div.Click += new System.EventHandler(this.ButtonDivClicked);
            // 
            // bt_queue
            // 
            this.bt_queue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_queue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.bt_queue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_queue.Font = new System.Drawing.Font("Terminus (TTF)", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_queue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(96)))));
            this.bt_queue.Location = new System.Drawing.Point(208, 5);
            this.bt_queue.Name = "bt_queue";
            this.bt_queue.Size = new System.Drawing.Size(116, 24);
            this.bt_queue.TabIndex = 2;
            this.bt_queue.TabStop = false;
            this.bt_queue.Text = "Add to Queue";
            this.bt_queue.UseVisualStyleBackColor = false;
            this.bt_queue.Click += new System.EventHandler(this.ButtonQueueClicked);
            // 
            // lb_bay_name
            // 
            this.lb_bay_name.Dock = System.Windows.Forms.DockStyle.Left;
            this.lb_bay_name.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_bay_name.ForeColor = System.Drawing.Color.White;
            this.lb_bay_name.Location = new System.Drawing.Point(0, 0);
            this.lb_bay_name.Name = "lb_bay_name";
            this.lb_bay_name.Size = new System.Drawing.Size(150, 36);
            this.lb_bay_name.TabIndex = 0;
            this.lb_bay_name.Text = "Bay Name";
            this.lb_bay_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lb_bay_name.Click += new System.EventHandler(this.LabelBayNameClicked);
            // 
            // BayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.basePanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(0, 50);
            this.Name = "BayControl";
            this.Size = new System.Drawing.Size(481, 350);
            this.basePanel.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel basePanel;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label lb_bay_name;
        private System.Windows.Forms.Button bt_queue;
        private System.Windows.Forms.Button bt_div;
        private NoScrollPanel pl_main;
    }
}
