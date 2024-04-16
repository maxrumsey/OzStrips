namespace maxrumsey.ozstrips.controls
{
    partial class MsgListDebug
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
            this.gb_parent = new System.Windows.Forms.GroupBox();
            this.rtb_text = new System.Windows.Forms.RichTextBox();
            this.lb_menu = new System.Windows.Forms.ListBox();
            this.gb_parent.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_parent
            // 
            this.gb_parent.Controls.Add(this.rtb_text);
            this.gb_parent.Controls.Add(this.lb_menu);
            this.gb_parent.Location = new System.Drawing.Point(4, 4);
            this.gb_parent.Name = "gb_parent";
            this.gb_parent.Size = new System.Drawing.Size(918, 363);
            this.gb_parent.TabIndex = 0;
            this.gb_parent.TabStop = false;
            this.gb_parent.Text = "Message List";
            // 
            // rtb_text
            // 
            this.rtb_text.Location = new System.Drawing.Point(229, 20);
            this.rtb_text.Name = "rtb_text";
            this.rtb_text.Size = new System.Drawing.Size(683, 329);
            this.rtb_text.TabIndex = 1;
            this.rtb_text.Text = "";
            // 
            // lb_menu
            // 
            this.lb_menu.FormattingEnabled = true;
            this.lb_menu.Location = new System.Drawing.Point(7, 20);
            this.lb_menu.Name = "lb_menu";
            this.lb_menu.Size = new System.Drawing.Size(215, 329);
            this.lb_menu.TabIndex = 0;
            this.lb_menu.Click += new System.EventHandler(this.lb_menu_Click);
            this.lb_menu.SelectedIndexChanged += new System.EventHandler(this.lb_menu_Click);
            // 
            // MsgListDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gb_parent);
            this.Name = "MsgListDebug";
            this.Size = new System.Drawing.Size(925, 370);
            this.gb_parent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_parent;
        private System.Windows.Forms.ListBox lb_menu;
        private System.Windows.Forms.RichTextBox rtb_text;
    }
}
