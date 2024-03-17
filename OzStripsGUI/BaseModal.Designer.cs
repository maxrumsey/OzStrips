namespace maxrumsey.ozstrips.gui
{
    partial class BaseModal
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
            this.bt_canc = new System.Windows.Forms.Button();
            this.bt_acp = new System.Windows.Forms.Button();
            this.gb_cont = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // bt_canc
            // 
            this.bt_canc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bt_canc.BackColor = System.Drawing.Color.LightCoral;
            this.bt_canc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_canc.Location = new System.Drawing.Point(13, 413);
            this.bt_canc.Name = "bt_canc";
            this.bt_canc.Size = new System.Drawing.Size(75, 23);
            this.bt_canc.TabIndex = 0;
            this.bt_canc.Text = "Cancel";
            this.bt_canc.UseVisualStyleBackColor = false;
            this.bt_canc.Click += new System.EventHandler(this.bt_canc_Click);
            // 
            // bt_acp
            // 
            this.bt_acp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_acp.BackColor = System.Drawing.Color.PaleGreen;
            this.bt_acp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_acp.Location = new System.Drawing.Point(190, 413);
            this.bt_acp.Name = "bt_acp";
            this.bt_acp.Size = new System.Drawing.Size(75, 23);
            this.bt_acp.TabIndex = 1;
            this.bt_acp.Text = "Accept";
            this.bt_acp.UseVisualStyleBackColor = false;
            this.bt_acp.Click += new System.EventHandler(this.bt_acp_Click);
            // 
            // gb_cont
            // 
            this.gb_cont.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_cont.Location = new System.Drawing.Point(13, 12);
            this.gb_cont.Name = "gb_cont";
            this.gb_cont.Size = new System.Drawing.Size(252, 392);
            this.gb_cont.TabIndex = 2;
            this.gb_cont.TabStop = false;
            // 
            // BaseModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 445);
            this.Controls.Add(this.gb_cont);
            this.Controls.Add(this.bt_acp);
            this.Controls.Add(this.bt_canc);
            this.MinimumSize = new System.Drawing.Size(293, 480);
            this.Name = "BaseModal";
            this.Text = "BaseModal";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_canc;
        private System.Windows.Forms.Button bt_acp;
        private System.Windows.Forms.GroupBox gb_cont;
    }
}