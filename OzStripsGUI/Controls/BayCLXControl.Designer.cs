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
            tb_clx = new System.Windows.Forms.TextBox();
            button12 = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            button1 = new System.Windows.Forms.Button();
            tb_bay = new System.Windows.Forms.TextBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            button2 = new System.Windows.Forms.Button();
            tb_remark = new System.Windows.Forms.TextBox();
            groupBox4 = new System.Windows.Forms.GroupBox();
            button3 = new System.Windows.Forms.Button();
            tb_glop = new System.Windows.Forms.TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // tb_clx
            // 
            tb_clx.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tb_clx.Location = new System.Drawing.Point(7, 19);
            tb_clx.MaxLength = 3;
            tb_clx.Name = "tb_clx";
            tb_clx.Size = new System.Drawing.Size(75, 26);
            tb_clx.TabIndex = 2;
            tb_clx.KeyDown += tb_clx_KeyDown;
            // 
            // button12
            // 
            button12.Location = new System.Drawing.Point(6, 134);
            button12.Name = "button12";
            button12.Size = new System.Drawing.Size(75, 23);
            button12.TabIndex = 16;
            button12.TabStop = false;
            button12.Text = "Clear";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button12);
            groupBox1.Controls.Add(tb_clx);
            groupBox1.Location = new System.Drawing.Point(7, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(178, 162);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "HOLD POINT";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button1);
            groupBox2.Controls.Add(tb_bay);
            groupBox2.Location = new System.Drawing.Point(200, 7);
            groupBox2.Name = "groupBox2";
            groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            groupBox2.Size = new System.Drawing.Size(182, 162);
            groupBox2.TabIndex = 17;
            groupBox2.TabStop = false;
            groupBox2.Text = "BAY NO.";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(6, 134);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 16;
            button1.TabStop = false;
            button1.Text = "Clear";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // tb_bay
            // 
            tb_bay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tb_bay.Location = new System.Drawing.Point(7, 19);
            tb_bay.MaxLength = 3;
            tb_bay.Name = "tb_bay";
            tb_bay.Size = new System.Drawing.Size(75, 26);
            tb_bay.TabIndex = 2;
            tb_bay.KeyDown += tb_clx_KeyDown;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(button2);
            groupBox3.Controls.Add(tb_remark);
            groupBox3.Location = new System.Drawing.Point(200, 175);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(182, 83);
            groupBox3.TabIndex = 17;
            groupBox3.TabStop = false;
            groupBox3.Text = "LOCAL REMARK";
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(6, 51);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(169, 23);
            button2.TabIndex = 16;
            button2.TabStop = false;
            button2.Text = "Clear";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // tb_remark
            // 
            tb_remark.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tb_remark.Location = new System.Drawing.Point(7, 19);
            tb_remark.MaxLength = 20;
            tb_remark.Name = "tb_remark";
            tb_remark.Size = new System.Drawing.Size(168, 26);
            tb_remark.TabIndex = 2;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(button3);
            groupBox4.Controls.Add(tb_glop);
            groupBox4.Location = new System.Drawing.Point(4, 175);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(182, 83);
            groupBox4.TabIndex = 18;
            groupBox4.TabStop = false;
            groupBox4.Text = "GLOBAL REMARK";
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(6, 51);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(169, 23);
            button3.TabIndex = 16;
            button3.TabStop = false;
            button3.Text = "Clear";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // tb_glop
            // 
            tb_glop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tb_glop.Location = new System.Drawing.Point(7, 19);
            tb_glop.MaxLength = 20;
            tb_glop.Name = "tb_glop";
            tb_glop.Size = new System.Drawing.Size(168, 26);
            tb_glop.TabIndex = 2;
            // 
            // BayCLXControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "BayCLXControl";
            Size = new System.Drawing.Size(389, 259);
            Load += AltHdgControl_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox tb_glop;
    }
}
