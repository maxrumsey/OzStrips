namespace maxrumsey.ozstrips.controls
{
    partial class BayCLXControl
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
            this.tb_clx = new System.Windows.Forms.TextBox();
            this.button12 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tb_bay = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tb_remark = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_clx
            // 
            this.tb_clx.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_clx.Location = new System.Drawing.Point(7, 19);
            this.tb_clx.MaxLength = 3;
            this.tb_clx.Name = "tb_clx";
            this.tb_clx.Size = new System.Drawing.Size(75, 26);
            this.tb_clx.TabIndex = 2;
            this.tb_clx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_clx_KeyDown);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(6, 134);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 23);
            this.button12.TabIndex = 16;
            this.button12.TabStop = false;
            this.button12.Text = "Clear";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.tb_clx);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(88, 162);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CLX";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.tb_bay);
            this.groupBox2.Location = new System.Drawing.Point(98, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(88, 162);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bay";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 134);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 16;
            this.button1.TabStop = false;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_bay
            // 
            this.tb_bay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_bay.Location = new System.Drawing.Point(7, 19);
            this.tb_bay.MaxLength = 3;
            this.tb_bay.Name = "tb_bay";
            this.tb_bay.Size = new System.Drawing.Size(75, 26);
            this.tb_bay.TabIndex = 2;
            this.tb_bay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_clx_KeyDown);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.tb_remark);
            this.groupBox3.Location = new System.Drawing.Point(4, 172);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(182, 83);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Remark";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 51);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(169, 23);
            this.button2.TabIndex = 16;
            this.button2.TabStop = false;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tb_remark
            // 
            this.tb_remark.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_remark.Location = new System.Drawing.Point(7, 19);
            this.tb_remark.MaxLength = 20;
            this.tb_remark.Name = "tb_remark";
            this.tb_remark.Size = new System.Drawing.Size(168, 26);
            this.tb_remark.TabIndex = 2;
            // 
            // BayCLXControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "BayCLXControl";
            this.Size = new System.Drawing.Size(192, 259);
            this.Load += new System.EventHandler(this.AltHdgControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox tb_clx;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_bay;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tb_remark;
    }
}
