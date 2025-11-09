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
            this.tb_pdc.Location = new System.Drawing.Point(3, 19);
            this.tb_pdc.Multiline = true;
            this.tb_pdc.Name = "tb_pdc";
            this.tb_pdc.Size = new System.Drawing.Size(400, 261);
            this.tb_pdc.TabIndex = 0;
            // 
            // PDCControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lb_title);
            this.Controls.Add(this.tb_pdc);
            this.Name = "PDCControl";
            this.Size = new System.Drawing.Size(409, 283);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_pdc;
        private System.Windows.Forms.Label lb_title;
    }
}
