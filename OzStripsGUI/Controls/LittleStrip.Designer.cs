namespace MaxRumsey.OzStripsPlugin.Gui.Controls
{
    partial class LittleStrip
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
            components = new System.ComponentModel.Container();
            pl_acid = new System.Windows.Forms.Panel();
            lb_acid = new System.Windows.Forms.Label();
            pl_wtc = new System.Windows.Forms.Panel();
            lb_wtc = new System.Windows.Forms.Label();
            pl_frul = new System.Windows.Forms.Panel();
            lb_frul = new System.Windows.Forms.Label();
            pl_ssr = new System.Windows.Forms.Panel();
            lb_ssr = new System.Windows.Forms.Label();
            pl_type = new System.Windows.Forms.Panel();
            lb_type = new System.Windows.Forms.Label();
            pl_route = new System.Windows.Forms.Panel();
            lb_route = new System.Windows.Forms.Label();
            pl_ades = new System.Windows.Forms.Panel();
            lb_ades = new System.Windows.Forms.Label();
            pl_sid = new System.Windows.Forms.Panel();
            lb_sid = new System.Windows.Forms.Label();
            pl_std = new System.Windows.Forms.Panel();
            lb_std = new System.Windows.Forms.Label();
            pl_hdg = new System.Windows.Forms.Panel();
            lb_hdg = new System.Windows.Forms.Label();
            pl_alt = new System.Windows.Forms.Panel();
            lb_alt = new System.Windows.Forms.Label();
            pl_rwy = new System.Windows.Forms.Panel();
            lb_rwy = new System.Windows.Forms.Label();
            pl_clx = new System.Windows.Forms.Panel();
            lb_clx = new System.Windows.Forms.Label();
            pl_remark = new System.Windows.Forms.Panel();
            lb_remark = new System.Windows.Forms.Label();
            pl_eobt = new System.Windows.Forms.Panel();
            lb_eobt = new System.Windows.Forms.Label();
            pl_rte = new System.Windows.Forms.Panel();
            lb_rte = new System.Windows.Forms.Label();
            pl_ssricon = new System.Windows.Forms.Panel();
            lb_ssricon = new System.Windows.Forms.Label();
            pl_req = new System.Windows.Forms.Panel();
            lb_req = new System.Windows.Forms.Label();
            pl_glop = new System.Windows.Forms.Panel();
            lb_glop = new System.Windows.Forms.Label();
            pl_tot = new System.Windows.Forms.Panel();
            lb_tot = new System.Windows.Forms.Label();
            ttp_route = new System.Windows.Forms.ToolTip(components);
            ttp_cfl = new System.Windows.Forms.ToolTip(components);
            pl_rdy = new System.Windows.Forms.Panel();
            lb_rdy = new System.Windows.Forms.Label();
            pl_acid.SuspendLayout();
            pl_wtc.SuspendLayout();
            pl_frul.SuspendLayout();
            pl_ssr.SuspendLayout();
            pl_type.SuspendLayout();
            pl_route.SuspendLayout();
            pl_ades.SuspendLayout();
            pl_sid.SuspendLayout();
            pl_std.SuspendLayout();
            pl_hdg.SuspendLayout();
            pl_alt.SuspendLayout();
            pl_rwy.SuspendLayout();
            pl_clx.SuspendLayout();
            pl_remark.SuspendLayout();
            pl_eobt.SuspendLayout();
            pl_rte.SuspendLayout();
            pl_ssricon.SuspendLayout();
            pl_req.SuspendLayout();
            pl_glop.SuspendLayout();
            pl_tot.SuspendLayout();
            pl_rdy.SuspendLayout();
            SuspendLayout();
            // 
            // pl_acid
            // 
            pl_acid.BackColor = System.Drawing.Color.Transparent;
            pl_acid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_acid.Controls.Add(lb_acid);
            pl_acid.Location = new System.Drawing.Point(228, 0);
            pl_acid.Margin = new System.Windows.Forms.Padding(0);
            pl_acid.Name = "pl_acid";
            pl_acid.Size = new System.Drawing.Size(100, 38);
            pl_acid.TabIndex = 1;
            // 
            // lb_acid
            // 
            lb_acid.BackColor = System.Drawing.Color.Transparent;
            lb_acid.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_acid.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_acid.Location = new System.Drawing.Point(0, 0);
            lb_acid.Name = "lb_acid";
            lb_acid.Size = new System.Drawing.Size(98, 36);
            lb_acid.TabIndex = 0;
            lb_acid.Text = "CALLSIGN";
            lb_acid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_acid.Click += AcidClicked;
            lb_acid.DoubleClick += AcidClicked;
            // 
            // pl_wtc
            // 
            pl_wtc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_wtc.Controls.Add(lb_wtc);
            pl_wtc.Location = new System.Drawing.Point(198, 0);
            pl_wtc.Name = "pl_wtc";
            pl_wtc.Size = new System.Drawing.Size(30, 19);
            pl_wtc.TabIndex = 3;
            // 
            // lb_wtc
            // 
            lb_wtc.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_wtc.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_wtc.Location = new System.Drawing.Point(0, 0);
            lb_wtc.Name = "lb_wtc";
            lb_wtc.Size = new System.Drawing.Size(28, 17);
            lb_wtc.TabIndex = 0;
            lb_wtc.Text = "L";
            lb_wtc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_wtc.Click += OpenFDR;
            // 
            // pl_frul
            // 
            pl_frul.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_frul.Controls.Add(lb_frul);
            pl_frul.Location = new System.Drawing.Point(98, 19);
            pl_frul.Name = "pl_frul";
            pl_frul.Size = new System.Drawing.Size(30, 19);
            pl_frul.TabIndex = 2;
            // 
            // lb_frul
            // 
            lb_frul.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_frul.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_frul.Location = new System.Drawing.Point(0, 0);
            lb_frul.Name = "lb_frul";
            lb_frul.Size = new System.Drawing.Size(28, 17);
            lb_frul.TabIndex = 0;
            lb_frul.Text = "L";
            lb_frul.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pl_ssr
            // 
            pl_ssr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_ssr.Controls.Add(lb_ssr);
            pl_ssr.Location = new System.Drawing.Point(158, 19);
            pl_ssr.Name = "pl_ssr";
            pl_ssr.Size = new System.Drawing.Size(70, 19);
            pl_ssr.TabIndex = 1;
            // 
            // lb_ssr
            // 
            lb_ssr.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_ssr.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_ssr.ForeColor = System.Drawing.Color.Black;
            lb_ssr.Location = new System.Drawing.Point(0, 0);
            lb_ssr.Name = "lb_ssr";
            lb_ssr.Size = new System.Drawing.Size(68, 17);
            lb_ssr.TabIndex = 0;
            lb_ssr.Text = "ssr";
            lb_ssr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_ssr.Click += SSRClicked;
            // 
            // pl_type
            // 
            pl_type.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_type.Controls.Add(lb_type);
            pl_type.Location = new System.Drawing.Point(128, 0);
            pl_type.Name = "pl_type";
            pl_type.Size = new System.Drawing.Size(70, 19);
            pl_type.TabIndex = 0;
            // 
            // lb_type
            // 
            lb_type.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_type.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_type.Location = new System.Drawing.Point(0, 0);
            lb_type.Name = "lb_type";
            lb_type.Size = new System.Drawing.Size(68, 17);
            lb_type.TabIndex = 0;
            lb_type.Text = "AC TYPE";
            lb_type.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_type.Click += OpenFDR;
            // 
            // pl_route
            // 
            pl_route.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_route.Controls.Add(lb_route);
            pl_route.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            pl_route.Location = new System.Drawing.Point(448, 19);
            pl_route.Name = "pl_route";
            pl_route.Size = new System.Drawing.Size(80, 19);
            pl_route.TabIndex = 1;
            // 
            // lb_route
            // 
            lb_route.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lb_route.AutoEllipsis = true;
            lb_route.Location = new System.Drawing.Point(0, -3);
            lb_route.Margin = new System.Windows.Forms.Padding(0);
            lb_route.Name = "lb_route";
            lb_route.Size = new System.Drawing.Size(80, 21);
            lb_route.TabIndex = 0;
            lb_route.Text = "rte";
            lb_route.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_route.Click += OpenFDR;
            // 
            // pl_ades
            // 
            pl_ades.Controls.Add(lb_ades);
            pl_ades.Location = new System.Drawing.Point(0, 19);
            pl_ades.Name = "pl_ades";
            pl_ades.Size = new System.Drawing.Size(68, 19);
            pl_ades.TabIndex = 0;
            // 
            // lb_ades
            // 
            lb_ades.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lb_ades.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_ades.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_ades.Location = new System.Drawing.Point(0, 0);
            lb_ades.Name = "lb_ades";
            lb_ades.Size = new System.Drawing.Size(68, 19);
            lb_ades.TabIndex = 0;
            lb_ades.Text = "DEST";
            lb_ades.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_ades.Click += OpenFDR;
            // 
            // pl_sid
            // 
            pl_sid.BackColor = System.Drawing.Color.Transparent;
            pl_sid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_sid.Controls.Add(lb_sid);
            pl_sid.Location = new System.Drawing.Point(448, 0);
            pl_sid.Margin = new System.Windows.Forms.Padding(0);
            pl_sid.Name = "pl_sid";
            pl_sid.Size = new System.Drawing.Size(80, 19);
            pl_sid.TabIndex = 1;
            // 
            // lb_sid
            // 
            lb_sid.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lb_sid.BackColor = System.Drawing.Color.Lime;
            lb_sid.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_sid.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            lb_sid.Location = new System.Drawing.Point(0, -2);
            lb_sid.Name = "lb_sid";
            lb_sid.Size = new System.Drawing.Size(80, 20);
            lb_sid.TabIndex = 0;
            lb_sid.Text = "SID";
            lb_sid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_sid.Click += SidClicked;
            // 
            // pl_std
            // 
            pl_std.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_std.Controls.Add(lb_std);
            pl_std.Location = new System.Drawing.Point(-1, -1);
            pl_std.Margin = new System.Windows.Forms.Padding(0);
            pl_std.Name = "pl_std";
            pl_std.Size = new System.Drawing.Size(70, 21);
            pl_std.TabIndex = 2;
            // 
            // lb_std
            // 
            lb_std.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lb_std.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_std.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_std.Location = new System.Drawing.Point(0, 0);
            lb_std.Margin = new System.Windows.Forms.Padding(3);
            lb_std.Name = "lb_std";
            lb_std.Size = new System.Drawing.Size(68, 19);
            lb_std.TabIndex = 0;
            lb_std.Text = "GATE";
            lb_std.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_std.Click += OpenCLXBay;
            // 
            // pl_hdg
            // 
            pl_hdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_hdg.Controls.Add(lb_hdg);
            pl_hdg.Location = new System.Drawing.Point(447, 37);
            pl_hdg.Name = "pl_hdg";
            pl_hdg.Size = new System.Drawing.Size(82, 23);
            pl_hdg.TabIndex = 1;
            // 
            // lb_hdg
            // 
            lb_hdg.AutoEllipsis = true;
            lb_hdg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lb_hdg.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_hdg.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_hdg.Location = new System.Drawing.Point(0, 0);
            lb_hdg.Name = "lb_hdg";
            lb_hdg.Size = new System.Drawing.Size(80, 21);
            lb_hdg.TabIndex = 0;
            lb_hdg.Text = "Hdg";
            lb_hdg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_hdg.Click += LBHdgClicked;
            // 
            // pl_alt
            // 
            pl_alt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_alt.Controls.Add(lb_alt);
            pl_alt.Location = new System.Drawing.Point(528, 19);
            pl_alt.Name = "pl_alt";
            pl_alt.Size = new System.Drawing.Size(50, 19);
            pl_alt.TabIndex = 0;
            // 
            // lb_alt
            // 
            lb_alt.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_alt.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_alt.Location = new System.Drawing.Point(0, 0);
            lb_alt.Name = "lb_alt";
            lb_alt.Size = new System.Drawing.Size(48, 17);
            lb_alt.TabIndex = 0;
            lb_alt.Text = "Alt";
            lb_alt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_alt.Click += LBAltClicked;
            // 
            // pl_rwy
            // 
            pl_rwy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_rwy.Controls.Add(lb_rwy);
            pl_rwy.Location = new System.Drawing.Point(328, 0);
            pl_rwy.Margin = new System.Windows.Forms.Padding(0);
            pl_rwy.Name = "pl_rwy";
            pl_rwy.Size = new System.Drawing.Size(60, 20);
            pl_rwy.TabIndex = 3;
            // 
            // lb_rwy
            // 
            lb_rwy.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_rwy.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_rwy.Location = new System.Drawing.Point(0, 0);
            lb_rwy.Name = "lb_rwy";
            lb_rwy.Size = new System.Drawing.Size(58, 18);
            lb_rwy.TabIndex = 0;
            lb_rwy.Text = "rwy";
            lb_rwy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_rwy.Click += LBRwyClicked;
            // 
            // pl_clx
            // 
            pl_clx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_clx.Controls.Add(lb_clx);
            pl_clx.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            pl_clx.Location = new System.Drawing.Point(388, 0);
            pl_clx.Margin = new System.Windows.Forms.Padding(0);
            pl_clx.Name = "pl_clx";
            pl_clx.Size = new System.Drawing.Size(60, 38);
            pl_clx.TabIndex = 3;
            // 
            // lb_clx
            // 
            lb_clx.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_clx.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_clx.Location = new System.Drawing.Point(0, 0);
            lb_clx.Margin = new System.Windows.Forms.Padding(3);
            lb_clx.Name = "lb_clx";
            lb_clx.Size = new System.Drawing.Size(58, 36);
            lb_clx.TabIndex = 0;
            lb_clx.Text = "clx";
            lb_clx.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_clx.Click += OpenCLXBay;
            // 
            // pl_remark
            // 
            pl_remark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_remark.Controls.Add(lb_remark);
            pl_remark.Location = new System.Drawing.Point(247, 37);
            pl_remark.Margin = new System.Windows.Forms.Padding(0);
            pl_remark.Name = "pl_remark";
            pl_remark.Size = new System.Drawing.Size(202, 23);
            pl_remark.TabIndex = 3;
            // 
            // lb_remark
            // 
            lb_remark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lb_remark.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_remark.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_remark.Location = new System.Drawing.Point(0, 0);
            lb_remark.Margin = new System.Windows.Forms.Padding(3);
            lb_remark.Name = "lb_remark";
            lb_remark.Size = new System.Drawing.Size(200, 21);
            lb_remark.TabIndex = 0;
            lb_remark.Text = "LOCAL REMARK";
            lb_remark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_remark.Click += OpenCLXBay;
            // 
            // pl_eobt
            // 
            pl_eobt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_eobt.Controls.Add(lb_eobt);
            pl_eobt.Location = new System.Drawing.Point(68, 0);
            pl_eobt.Name = "pl_eobt";
            pl_eobt.Size = new System.Drawing.Size(60, 20);
            pl_eobt.TabIndex = 4;
            // 
            // lb_eobt
            // 
            lb_eobt.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_eobt.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_eobt.Location = new System.Drawing.Point(0, 0);
            lb_eobt.Name = "lb_eobt";
            lb_eobt.Size = new System.Drawing.Size(58, 18);
            lb_eobt.TabIndex = 0;
            lb_eobt.Text = "0000";
            lb_eobt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_eobt.Click += EOBTClicked;
            // 
            // pl_rte
            // 
            pl_rte.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_rte.Controls.Add(lb_rte);
            pl_rte.Location = new System.Drawing.Point(68, 19);
            pl_rte.Name = "pl_rte";
            pl_rte.Size = new System.Drawing.Size(30, 19);
            pl_rte.TabIndex = 6;
            // 
            // lb_rte
            // 
            lb_rte.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_rte.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_rte.Location = new System.Drawing.Point(0, 0);
            lb_rte.Name = "lb_rte";
            lb_rte.Size = new System.Drawing.Size(28, 17);
            lb_rte.TabIndex = 0;
            lb_rte.Text = "R";
            lb_rte.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_rte.Click += RouteClicked;
            lb_rte.DoubleClick += RouteClicked;
            // 
            // pl_ssricon
            // 
            pl_ssricon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_ssricon.Controls.Add(lb_ssricon);
            pl_ssricon.Location = new System.Drawing.Point(128, 19);
            pl_ssricon.Name = "pl_ssricon";
            pl_ssricon.Size = new System.Drawing.Size(30, 19);
            pl_ssricon.TabIndex = 3;
            // 
            // lb_ssricon
            // 
            lb_ssricon.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_ssricon.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_ssricon.Location = new System.Drawing.Point(0, 0);
            lb_ssricon.Name = "lb_ssricon";
            lb_ssricon.Size = new System.Drawing.Size(28, 17);
            lb_ssricon.TabIndex = 0;
            lb_ssricon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pl_req
            // 
            pl_req.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_req.Controls.Add(lb_req);
            pl_req.Location = new System.Drawing.Point(528, 0);
            pl_req.Name = "pl_req";
            pl_req.Size = new System.Drawing.Size(50, 19);
            pl_req.TabIndex = 2;
            // 
            // lb_req
            // 
            lb_req.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_req.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_req.ForeColor = System.Drawing.Color.Gray;
            lb_req.Location = new System.Drawing.Point(0, 0);
            lb_req.Name = "lb_req";
            lb_req.Size = new System.Drawing.Size(48, 17);
            lb_req.TabIndex = 1;
            lb_req.Text = "REQ LVL";
            lb_req.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_req.Click += OpenFDR;
            // 
            // pl_glop
            // 
            pl_glop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_glop.Controls.Add(lb_glop);
            pl_glop.Location = new System.Drawing.Point(-1, 37);
            pl_glop.Margin = new System.Windows.Forms.Padding(0);
            pl_glop.Name = "pl_glop";
            pl_glop.Size = new System.Drawing.Size(249, 23);
            pl_glop.TabIndex = 4;
            // 
            // lb_glop
            // 
            lb_glop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lb_glop.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_glop.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_glop.Location = new System.Drawing.Point(0, 0);
            lb_glop.Margin = new System.Windows.Forms.Padding(3);
            lb_glop.Name = "lb_glop";
            lb_glop.Size = new System.Drawing.Size(247, 21);
            lb_glop.TabIndex = 0;
            lb_glop.Text = "GLOBAL REMARK";
            lb_glop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_glop.Click += OpenCLXBay;
            // 
            // pl_tot
            // 
            pl_tot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_tot.Controls.Add(lb_tot);
            pl_tot.Location = new System.Drawing.Point(528, 37);
            pl_tot.Name = "pl_tot";
            pl_tot.Size = new System.Drawing.Size(50, 23);
            pl_tot.TabIndex = 1;
            // 
            // lb_tot
            // 
            lb_tot.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_tot.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_tot.Location = new System.Drawing.Point(0, 0);
            lb_tot.Name = "lb_tot";
            lb_tot.Size = new System.Drawing.Size(48, 21);
            lb_tot.TabIndex = 0;
            lb_tot.Text = "00:00";
            lb_tot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_tot.Click += TOTClicked;
            // 
            // ttp_route
            // 
            ttp_route.ShowAlways = true;
            // 
            // ttp_cfl
            // 
            ttp_cfl.ShowAlways = true;
            // 
            // pl_rdy
            // 
            pl_rdy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pl_rdy.Controls.Add(lb_rdy);
            pl_rdy.Location = new System.Drawing.Point(328, 18);
            pl_rdy.Margin = new System.Windows.Forms.Padding(0);
            pl_rdy.Name = "pl_rdy";
            pl_rdy.Size = new System.Drawing.Size(60, 20);
            pl_rdy.TabIndex = 4;
            // 
            // lb_rdy
            // 
            lb_rdy.Dock = System.Windows.Forms.DockStyle.Fill;
            lb_rdy.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lb_rdy.Location = new System.Drawing.Point(0, 0);
            lb_rdy.Name = "lb_rdy";
            lb_rdy.Size = new System.Drawing.Size(58, 18);
            lb_rdy.TabIndex = 0;
            lb_rdy.Text = "RDY";
            lb_rdy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lb_rdy.Click += RdyClicked;
            // 
            // LittleStrip
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Transparent;
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Controls.Add(pl_rdy);
            Controls.Add(pl_tot);
            Controls.Add(pl_glop);
            Controls.Add(pl_req);
            Controls.Add(pl_ssricon);
            Controls.Add(pl_hdg);
            Controls.Add(pl_rte);
            Controls.Add(pl_alt);
            Controls.Add(pl_eobt);
            Controls.Add(pl_route);
            Controls.Add(pl_frul);
            Controls.Add(pl_ades);
            Controls.Add(pl_wtc);
            Controls.Add(pl_remark);
            Controls.Add(pl_clx);
            Controls.Add(pl_ssr);
            Controls.Add(pl_rwy);
            Controls.Add(pl_type);
            Controls.Add(pl_std);
            Controls.Add(pl_sid);
            Controls.Add(pl_acid);
            Name = "LittleStrip";
            Size = new System.Drawing.Size(578, 60);
            pl_acid.ResumeLayout(false);
            pl_wtc.ResumeLayout(false);
            pl_frul.ResumeLayout(false);
            pl_ssr.ResumeLayout(false);
            pl_type.ResumeLayout(false);
            pl_route.ResumeLayout(false);
            pl_ades.ResumeLayout(false);
            pl_sid.ResumeLayout(false);
            pl_std.ResumeLayout(false);
            pl_hdg.ResumeLayout(false);
            pl_alt.ResumeLayout(false);
            pl_rwy.ResumeLayout(false);
            pl_clx.ResumeLayout(false);
            pl_remark.ResumeLayout(false);
            pl_eobt.ResumeLayout(false);
            pl_rte.ResumeLayout(false);
            pl_ssricon.ResumeLayout(false);
            pl_req.ResumeLayout(false);
            pl_glop.ResumeLayout(false);
            pl_tot.ResumeLayout(false);
            pl_rdy.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel pl_acid;
        private System.Windows.Forms.Label lb_acid;
        private System.Windows.Forms.Panel pl_frul;
        private System.Windows.Forms.Label lb_frul;
        private System.Windows.Forms.Panel pl_ssr;
        private System.Windows.Forms.Label lb_ssr;
        private System.Windows.Forms.Panel pl_type;
        private System.Windows.Forms.Label lb_type;
        private System.Windows.Forms.Panel pl_route;
        private System.Windows.Forms.Label lb_route;
        private System.Windows.Forms.Panel pl_ades;
        private System.Windows.Forms.Label lb_ades;
        private System.Windows.Forms.Panel pl_sid;
        private System.Windows.Forms.Label lb_sid;
        private System.Windows.Forms.Panel pl_std;
        private System.Windows.Forms.Label lb_std;
        private System.Windows.Forms.Panel pl_hdg;
        private System.Windows.Forms.Label lb_hdg;
        private System.Windows.Forms.Panel pl_alt;
        private System.Windows.Forms.Label lb_alt;
        private System.Windows.Forms.Panel pl_rwy;
        private System.Windows.Forms.Label lb_rwy;
        private System.Windows.Forms.Panel pl_wtc;
        private System.Windows.Forms.Label lb_wtc;
        private System.Windows.Forms.Panel pl_clx;
        private System.Windows.Forms.Label lb_clx;
        private System.Windows.Forms.Panel pl_remark;
        private System.Windows.Forms.Label lb_remark;
        private System.Windows.Forms.Panel pl_eobt;
        private System.Windows.Forms.Label lb_eobt;
        private System.Windows.Forms.Panel pl_rte;
        private System.Windows.Forms.Label lb_rte;
        private System.Windows.Forms.Panel pl_ssricon;
        private System.Windows.Forms.Label lb_ssricon;
        private System.Windows.Forms.Panel pl_req;
        private System.Windows.Forms.Label lb_req;
        private System.Windows.Forms.Panel pl_glop;
        private System.Windows.Forms.Label lb_glop;
        private System.Windows.Forms.Panel pl_tot;
        private System.Windows.Forms.Label lb_tot;
        private System.Windows.Forms.ToolTip ttp_route;
        private System.Windows.Forms.ToolTip ttp_cfl;
        private System.Windows.Forms.Panel pl_rdy;
        private System.Windows.Forms.Label lb_rdy;
    }
}
