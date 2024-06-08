namespace MaxRumsey.OzStripsPlugin.Gui.Controls
{
    partial class SettingsWindowControl
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

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gp_main = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rb_ozstrips = new System.Windows.Forms.RadioButton();
            this.rb_vatsys = new System.Windows.Forms.RadioButton();
            this.gp_main.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gp_main
            // 
            this.gp_main.Controls.Add(this.groupBox1);
            this.gp_main.Location = new System.Drawing.Point(4, 4);
            this.gp_main.Name = "gp_main";
            this.gp_main.Size = new System.Drawing.Size(399, 284);
            this.gp_main.TabIndex = 0;
            this.gp_main.TabStop = false;
            this.gp_main.Text = "Settings";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rb_ozstrips);
            this.groupBox1.Controls.Add(this.rb_vatsys);
            this.groupBox1.Location = new System.Drawing.Point(7, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FDR Popup Types";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Determines what type of popup box to use.";
            // 
            // rb_ozstrips
            // 
            this.rb_ozstrips.AutoSize = true;
            this.rb_ozstrips.Location = new System.Drawing.Point(6, 59);
            this.rb_ozstrips.Name = "rb_ozstrips";
            this.rb_ozstrips.Size = new System.Drawing.Size(98, 17);
            this.rb_ozstrips.TabIndex = 1;
            this.rb_ozstrips.TabStop = true;
            this.rb_ozstrips.Text = "OzStrips Popup";
            this.rb_ozstrips.UseVisualStyleBackColor = true;
            // 
            // rb_vatsys
            // 
            this.rb_vatsys.AutoSize = true;
            this.rb_vatsys.Location = new System.Drawing.Point(6, 36);
            this.rb_vatsys.Name = "rb_vatsys";
            this.rb_vatsys.Size = new System.Drawing.Size(91, 17);
            this.rb_vatsys.TabIndex = 0;
            this.rb_vatsys.TabStop = true;
            this.rb_vatsys.Text = "vatSys Popup";
            this.rb_vatsys.UseVisualStyleBackColor = true;
            // 
            // SettingsWindowControl
            // 
            this.Controls.Add(this.gp_main);
            this.Name = "SettingsWindowControl";
            this.Size = new System.Drawing.Size(406, 291);
            this.gp_main.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox gp_main;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rb_ozstrips;
        private System.Windows.Forms.RadioButton rb_vatsys;
    }
}
