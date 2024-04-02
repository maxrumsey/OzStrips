namespace maxrumsey.ozstrips.gui
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.flp_stripbay = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lb_bay_name = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.flp_stripbay);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(481, 322);
            this.panel1.TabIndex = 0;
            // 
            // flp_stripbay
            // 
            this.flp_stripbay.AutoScroll = true;
            this.flp_stripbay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flp_stripbay.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.flp_stripbay.Location = new System.Drawing.Point(0, 42);
            this.flp_stripbay.Name = "flp_stripbay";
            this.flp_stripbay.Size = new System.Drawing.Size(481, 280);
            this.flp_stripbay.TabIndex = 1;
            this.flp_stripbay.WrapContents = false;
            this.flp_stripbay.Click += new System.EventHandler(this.lb_bay_name_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel2.Controls.Add(this.lb_bay_name);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(481, 42);
            this.panel2.TabIndex = 0;
            // 
            // lb_bay_name
            // 
            this.lb_bay_name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_bay_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_bay_name.Location = new System.Drawing.Point(0, 0);
            this.lb_bay_name.Name = "lb_bay_name";
            this.lb_bay_name.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.lb_bay_name.Size = new System.Drawing.Size(481, 42);
            this.lb_bay_name.TabIndex = 0;
            this.lb_bay_name.Text = "Bay Name";
            this.lb_bay_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lb_bay_name.Click += new System.EventHandler(this.lb_bay_name_Click);
            // 
            // BayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BayControl";
            this.Size = new System.Drawing.Size(481, 322);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lb_bay_name;
        private System.Windows.Forms.FlowLayoutPanel flp_stripbay;
    }
}
