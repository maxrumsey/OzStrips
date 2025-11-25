namespace MaxRumsey.OzStripsPlugin.GUI.Controls
{
    partial class DebugSetATIS
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
            this.tb_atis = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lb_title
            // 
            this.lb_title.AutoSize = true;
            this.lb_title.Location = new System.Drawing.Point(3, 3);
            this.lb_title.Name = "lb_title";
            this.lb_title.Size = new System.Drawing.Size(34, 13);
            this.lb_title.TabIndex = 1;
            this.lb_title.Text = "ATIS:";
            // 
            // tb_pdc
            // 
            this.tb_atis.Location = new System.Drawing.Point(3, 19);
            this.tb_atis.Multiline = true;
            this.tb_atis.Name = "tb_pdc";
            this.tb_atis.Size = new System.Drawing.Size(400, 261);
            this.tb_atis.TabIndex = 0;
            // 
            // DebugSetATIS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lb_title);
            this.Controls.Add(this.tb_atis);
            this.Name = "DebugSetATIS";
            this.Size = new System.Drawing.Size(409, 309);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_atis;
        private System.Windows.Forms.Label lb_title;
    }
}
