namespace MaxRumsey.OzStripsPlugin.GUI.Controls
{
    partial class PDCControl
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
            this.lb_title = new System.Windows.Forms.Label();
            this.tb_pdc = new System.Windows.Forms.TextBox();
            this.bt_vatsyspdc = new System.Windows.Forms.Button();
            this.cb_delivery = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_title
            // 
            this.lb_title.AutoSize = true;
            this.lb_title.Location = new System.Drawing.Point(3, 3);
            this.lb_title.Name = "lb_title";
            this.lb_title.Size = new System.Drawing.Size(35, 13);
            this.lb_title.TabIndex = 1;
            this.lb_title.Text = "PDC: ";
            // 
            // tb_pdc
            // 
            this.tb_pdc.AcceptsReturn = true;
            this.tb_pdc.Location = new System.Drawing.Point(3, 19);
            this.tb_pdc.Multiline = true;
            this.tb_pdc.Name = "tb_pdc";
            this.tb_pdc.Size = new System.Drawing.Size(400, 261);
            this.tb_pdc.TabIndex = 0;
            // 
            // bt_vatsyspdc
            // 
            this.bt_vatsyspdc.Location = new System.Drawing.Point(3, 283);
            this.bt_vatsyspdc.Name = "bt_vatsyspdc";
            this.bt_vatsyspdc.Size = new System.Drawing.Size(173, 23);
            this.bt_vatsyspdc.TabIndex = 2;
            this.bt_vatsyspdc.Text = "Open Default PDC Editor";
            this.bt_vatsyspdc.UseVisualStyleBackColor = true;
            this.bt_vatsyspdc.Click += new System.EventHandler(this.OpenVatsysPDC);
            // 
            // cb_delivery
            // 
            this.cb_delivery.FormattingEnabled = true;
            this.cb_delivery.Location = new System.Drawing.Point(182, 284);
            this.cb_delivery.Name = "cb_delivery";
            this.cb_delivery.Size = new System.Drawing.Size(82, 21);
            this.cb_delivery.TabIndex = 3;
            this.cb_delivery.Text = "DELIVERY";
            this.cb_delivery.SelectedIndexChanged += new System.EventHandler(this.cb_delivery_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(6, 326);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Error:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(270, 283);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ResetButtonClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 310);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Do not remove \"@\" symbols from the PDC.";
            // 
            // PDCControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_delivery);
            this.Controls.Add(this.bt_vatsyspdc);
            this.Controls.Add(this.lb_title);
            this.Controls.Add(this.tb_pdc);
            this.Name = "PDCControl";
            this.Size = new System.Drawing.Size(409, 349);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_pdc;
        private System.Windows.Forms.Label lb_title;
        private System.Windows.Forms.Button bt_vatsyspdc;
        private System.Windows.Forms.ComboBox cb_delivery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
    }
}
