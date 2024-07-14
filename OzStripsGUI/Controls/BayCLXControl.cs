using System;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui.Controls;

/// <summary>
/// Represents a bay clearance.
/// </summary>
public partial class BayCLXControl : UserControl
{
    private readonly string _callingLabel;

    /// <summary>
    /// Initializes a new instance of the <see cref="BayCLXControl"/> class.
    /// </summary>
    /// <param name="controller">The strip controller.</param>
    /// <param name="labelName">The label that opened the control.</param>
    public BayCLXControl(StripController controller, string labelName)
    {
        InitializeComponent();

        tb_clx.Text = controller.CLX;
        tb_bay.Text = controller.Gate;
        tb_remark.Text = controller.Remark;
        tb_glop.Text = controller.FDR.GlobalOpData;

        _callingLabel = labelName;
    }

    /// <summary>
    /// Gets or sets the base modal.
    /// </summary>
    public BaseModal? BaseModal { get; set; }

    /// <summary>
    /// Gets the clearance text.
    /// </summary>
    public string CLX => tb_clx.Text;

    /// <summary>
    /// Gets the gate text.
    /// </summary>
    public string Gate => tb_bay.Text;

    /// <summary>
    /// Gets the remarks text.
    /// </summary>
    public string Remark => tb_remark.Text;

    /// <summary>
    /// Gets the glip text.
    /// </summary>
    public string Glop => tb_glop.Text;

    private void AltHdgControl_Load(object sender, EventArgs e)
    {
        if (BaseModal is null)
        {
            return;
        }

        switch (_callingLabel)
        {
            case "lb_remark":
                ActiveControl = tb_remark;
                break;
            case "lb_glop":
                ActiveControl = tb_glop;
                break;
            case "lb_std":
                ActiveControl = tb_bay;
                break;
            case "lb_clx":
                ActiveControl = tb_clx;
                break;
        }
    }

    private void ClearButtonTextCleared(object sender, EventArgs e)
    {
        tb_clx.Text = string.Empty;
    }

    private void BayButtonClearTextClicked(object sender, EventArgs e)
    {
        tb_bay.Text = string.Empty;
    }

    private void AcceptKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.Enter:
                BaseModal?.ExitModal(true);
                break;
            case Keys.Escape:
                BaseModal?.ExitModal();
                break;
        }
    }

    private void RemarkButtonClearClicked(object sender, EventArgs e)
    {
        tb_remark.Text = string.Empty;
    }

    private void GlopButtonClearClicked(object sender, EventArgs e)
    {
        tb_glop.Text = string.Empty;
    }
}
