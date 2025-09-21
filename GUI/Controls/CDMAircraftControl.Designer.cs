namespace MaxRumsey.OzStripsPlugin.GUI.Controls;

partial class CDMAircraftControl
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
            this.lb_acid = new System.Windows.Forms.Label();
            this.lb_items = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lb_acid
            // 
            this.lb_acid.AutoSize = true;
            this.lb_acid.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_acid.Location = new System.Drawing.Point(12, 9);
            this.lb_acid.Name = "lb_acid";
            this.lb_acid.Size = new System.Drawing.Size(93, 25);
            this.lb_acid.TabIndex = 0;
            this.lb_acid.Text = "Callsign: ";
            // 
            // lb_items
            // 
            this.lb_items.FormattingEnabled = true;
            this.lb_items.Location = new System.Drawing.Point(17, 38);
            this.lb_items.Name = "lb_items";
            this.lb_items.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lb_items.Size = new System.Drawing.Size(236, 186);
            this.lb_items.TabIndex = 1;
            // 
            // CDMAircraftControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lb_items);
            this.Controls.Add(this.lb_acid);
            this.Name = "CDMAircraftControl";
            this.Size = new System.Drawing.Size(269, 242);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lb_acid;
    private System.Windows.Forms.ListBox lb_items;
}
