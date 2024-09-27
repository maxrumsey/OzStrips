namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

partial class BarCreator
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
            this.label1 = new System.Windows.Forms.Label();
            this.cb_bay = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_item = new System.Windows.Forms.ComboBox();
            this.tb_text = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bay";
            // 
            // cb_bay
            // 
            this.cb_bay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_bay.FormattingEnabled = true;
            this.cb_bay.Location = new System.Drawing.Point(50, 4);
            this.cb_bay.Name = "cb_bay";
            this.cb_bay.Size = new System.Drawing.Size(183, 21);
            this.cb_bay.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Item";
            // 
            // cb_item
            // 
            this.cb_item.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_item.FormattingEnabled = true;
            this.cb_item.Location = new System.Drawing.Point(50, 37);
            this.cb_item.Name = "cb_item";
            this.cb_item.Size = new System.Drawing.Size(183, 21);
            this.cb_item.TabIndex = 3;
            this.cb_item.SelectedIndexChanged += new System.EventHandler(this.BarTypeChanged);
            // 
            // tb_text
            // 
            this.tb_text.Location = new System.Drawing.Point(50, 73);
            this.tb_text.Multiline = true;
            this.tb_text.Name = "tb_text";
            this.tb_text.Size = new System.Drawing.Size(183, 51);
            this.tb_text.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Text";
            // 
            // BarCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_text);
            this.Controls.Add(this.cb_item);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cb_bay);
            this.Controls.Add(this.label1);
            this.Name = "BarCreator";
            this.Size = new System.Drawing.Size(244, 127);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cb_bay;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cb_item;
    private System.Windows.Forms.TextBox tb_text;
    private System.Windows.Forms.Label label3;
}
