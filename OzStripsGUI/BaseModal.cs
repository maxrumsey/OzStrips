using System;
using System.Drawing;
using System.Windows.Forms;

namespace MaxRumsey.OzStripsPlugin.Gui;

/// <summary>
/// A base modal form.
/// </summary>
public partial class BaseModal : Form
{
    private readonly Control _child;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseModal"/> class.
    /// </summary>
    /// <param name="child">The child.</param>
    /// <param name="text">The text.</param>
    public BaseModal(Control child, string text)
    {
        StartPosition = FormStartPosition.CenterScreen;
        InitializeComponent();
        _child = child;
        gb_cont.Controls.Add(child);
        child.Anchor = AnchorStyles.Top;
        child.Location = new Point(6, 16);

        Text = text;
        BringToFront();
    }

    /// <summary>
    /// A event when the modal is returned.
    /// </summary>
    public event EventHandler<ModalReturnArgs>? ReturnEvent;

    /// <summary>
    /// Exits the model.
    /// </summary>
    /// <param name="senddata">If the return event should be fired or not.</param>
    public void ExitModal(bool senddata = false)
    {
        if (senddata && ReturnEvent != null)
        {
            ReturnEvent(this, new ModalReturnArgs(_child));
        }

        Close();
    }

    /// <inheritdoc/>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        var btn = ActiveControl as Button;
        if (btn != null)
        {
            if (keyData == Keys.Enter)
            {
                ExitModal(true);
                return true; // suppress default handling of space
            }
            else if (keyData == Keys.Escape)
            {
                ExitModal();
                return true; // suppress default handling of space
            }
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void ButtonCancelClicked(object sender, EventArgs e)
    {
        ExitModal();
    }

    private void ButtonAcceptedClicked(object sender, EventArgs e)
    {
        // to add
        // child.confirm();
        ExitModal(true);
    }
}
