namespace MaxRumsey.OzStripsPlugin.Gui.Controls
{
    partial class RerouteControl
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
            this.bt_save = new System.Windows.Forms.Button();
            this.lb_possibleroutes = new System.Windows.Forms.Label();
            this.ls_routes = new System.Windows.Forms.ListBox();
            this.lb_route = new System.Windows.Forms.Label();
            this.tb_route = new System.Windows.Forms.TextBox();
            this.lb_saving = new System.Windows.Forms.Label();
            this.lb_error = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bt_save
            // 
            this.bt_save.Location = new System.Drawing.Point(3, 257);
            this.bt_save.Name = "bt_save";
            this.bt_save.Size = new System.Drawing.Size(400, 23);
            this.bt_save.TabIndex = 4;
            this.bt_save.Text = "Amend Flightplan";
            this.bt_save.UseVisualStyleBackColor = true;
            this.bt_save.Click += new System.EventHandler(this.Save_Click);
            // 
            // lb_possibleroutes
            // 
            this.lb_possibleroutes.AutoSize = true;
            this.lb_possibleroutes.Location = new System.Drawing.Point(3, 74);
            this.lb_possibleroutes.Name = "lb_possibleroutes";
            this.lb_possibleroutes.Size = new System.Drawing.Size(67, 13);
            this.lb_possibleroutes.TabIndex = 3;
            this.lb_possibleroutes.Text = "Valid Routes";
            // 
            // ls_routes
            // 
            this.ls_routes.FormattingEnabled = true;
            this.ls_routes.HorizontalScrollbar = true;
            this.ls_routes.Location = new System.Drawing.Point(3, 90);
            this.ls_routes.Name = "ls_routes";
            this.ls_routes.Size = new System.Drawing.Size(400, 121);
            this.ls_routes.TabIndex = 2;
            // 
            // lb_route
            // 
            this.lb_route.AutoSize = true;
            this.lb_route.Location = new System.Drawing.Point(3, 3);
            this.lb_route.Name = "lb_route";
            this.lb_route.Size = new System.Drawing.Size(36, 13);
            this.lb_route.TabIndex = 1;
            this.lb_route.Text = "Route";
            // 
            // tb_route
            // 
            this.tb_route.Location = new System.Drawing.Point(3, 19);
            this.tb_route.Multiline = true;
            this.tb_route.Name = "tb_route";
            this.tb_route.Size = new System.Drawing.Size(400, 44);
            this.tb_route.TabIndex = 0;
            // 
            // lb_saving
            // 
            this.lb_saving.AutoSize = true;
            this.lb_saving.ForeColor = System.Drawing.Color.Red;
            this.lb_saving.Location = new System.Drawing.Point(74, 214);
            this.lb_saving.Name = "lb_saving";
            this.lb_saving.Size = new System.Drawing.Size(263, 13);
            this.lb_saving.TabIndex = 6;
            this.lb_saving.Text = "Ensure metadata is removed from route prior to saving.";
            // 
            // lb_error
            // 
            this.lb_error.AutoSize = true;
            this.lb_error.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_error.ForeColor = System.Drawing.Color.Red;
            this.lb_error.Location = new System.Drawing.Point(3, 232);
            this.lb_error.Name = "lb_error";
            this.lb_error.Size = new System.Drawing.Size(0, 13);
            this.lb_error.TabIndex = 7;
            // 
            // RerouteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lb_error);
            this.Controls.Add(this.lb_saving);
            this.Controls.Add(this.bt_save);
            this.Controls.Add(this.lb_route);
            this.Controls.Add(this.lb_possibleroutes);
            this.Controls.Add(this.tb_route);
            this.Controls.Add(this.ls_routes);
            this.Name = "RerouteControl";
            this.Size = new System.Drawing.Size(409, 283);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_route;
        private System.Windows.Forms.Label lb_route;
        private System.Windows.Forms.ListBox ls_routes;
        private System.Windows.Forms.Label lb_possibleroutes;
        private System.Windows.Forms.Button bt_save;
        private System.Windows.Forms.Label lb_saving;
        private System.Windows.Forms.Label lb_error;
    }
}
