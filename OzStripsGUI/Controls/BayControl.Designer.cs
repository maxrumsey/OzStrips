
using MaxRumsey.OzStripsPlugin.Gui.Controls;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls
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
            panel1 = new System.Windows.Forms.Panel();
            flp_stripbay = new CustomFLP();
            panel2 = new System.Windows.Forms.Panel();
            bt_div = new System.Windows.Forms.Button();
            bt_queue = new System.Windows.Forms.Button();
            lb_bay_name = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            panel1.Controls.Add(flp_stripbay);
            panel1.Controls.Add(panel2);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(481, 350);
            panel1.TabIndex = 0;
            // 
            // flp_stripbay
            // 
            flp_stripbay.AutoScroll = true;
            flp_stripbay.BackColor = System.Drawing.Color.Gray;
            flp_stripbay.Dock = System.Windows.Forms.DockStyle.Fill;
            flp_stripbay.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            flp_stripbay.Location = new System.Drawing.Point(0, 42);
            flp_stripbay.Name = "flp_stripbay";
            flp_stripbay.Size = new System.Drawing.Size(481, 308);
            flp_stripbay.TabIndex = 1;
            flp_stripbay.WrapContents = false;
            flp_stripbay.Click += LabelBayNameClicked;
            // 
            // panel2
            // 
            panel2.BackColor = System.Drawing.Color.Black;
            panel2.Controls.Add(bt_div);
            panel2.Controls.Add(bt_queue);
            panel2.Controls.Add(lb_bay_name);
            panel2.Dock = System.Windows.Forms.DockStyle.Top;
            panel2.Location = new System.Drawing.Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(481, 42);
            panel2.TabIndex = 0;
            // 
            // bt_div
            // 
            bt_div.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            bt_div.BackColor = System.Drawing.Color.FromArgb(140, 150, 150);
            bt_div.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            bt_div.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            bt_div.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            bt_div.Location = new System.Drawing.Point(330, 5);
            bt_div.Name = "bt_div";
            bt_div.Size = new System.Drawing.Size(145, 32);
            bt_div.TabIndex = 3;
            bt_div.TabStop = false;
            bt_div.Text = "Toggle Queue Bar";
            bt_div.UseVisualStyleBackColor = false;
            bt_div.Click += ButtonDivClicked;
            // 
            // bt_queue
            // 
            bt_queue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            bt_queue.BackColor = System.Drawing.Color.FromArgb(140, 150, 150);
            bt_queue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            bt_queue.Font = new System.Drawing.Font("Terminus (TTF)", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            bt_queue.ForeColor = System.Drawing.Color.FromArgb(0, 0, 96);
            bt_queue.Location = new System.Drawing.Point(208, 5);
            bt_queue.Name = "bt_queue";
            bt_queue.Size = new System.Drawing.Size(116, 32);
            bt_queue.TabIndex = 2;
            bt_queue.TabStop = false;
            bt_queue.Text = "Add to Queue";
            bt_queue.UseVisualStyleBackColor = false;
            bt_queue.Click += ButtonQueueClicked;
            // 
            // lb_bay_name
            // 
            lb_bay_name.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_bay_name.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_bay_name.ForeColor = System.Drawing.Color.White;
            lb_bay_name.Location = new System.Drawing.Point(0, 0);
            lb_bay_name.Name = "lb_bay_name";
            lb_bay_name.Size = new System.Drawing.Size(481, 42);
            lb_bay_name.TabIndex = 0;
            lb_bay_name.Text = "Bay Name";
            lb_bay_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lb_bay_name.Click += LabelBayNameClicked;
            // 
            // BayControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel1);
            Margin = new System.Windows.Forms.Padding(0);
            MinimumSize = new System.Drawing.Size(0, 300);
            Name = "BayControl";
            Size = new System.Drawing.Size(481, 350);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lb_bay_name;
        private CustomFLP flp_stripbay;
        private System.Windows.Forms.Button bt_queue;
        private System.Windows.Forms.Button bt_div;
    }
}
