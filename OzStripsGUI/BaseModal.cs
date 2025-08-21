using System;
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
        child.Location = new(6, 16);

        Text = text;
        BringToFront();

        Focus();

        _child.MouseMove += MouseMoved;
        MouseMove += MouseMoved;
    }

    public Control Child => _child;

    private void MouseMoved(object sender, MouseEventArgs e)
    {
        Focus();
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
            ReturnEvent(this, new(_child));
        }

        Close();
        MainForm.MainFormInstance?.Focus();
    }

    /// <inheritdoc/>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (ActiveControl is not Button)
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }

        switch (keyData)
        {
            case Keys.Enter:
                ExitModal(true);
                return true; // suppress default handling of space
            case Keys.Escape:
                ExitModal();
                return true; // suppress default handling of space
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

    private void BaseModal_FormClosed(object sender, FormClosedEventArgs e)
    {
        Dispose();
    }
}
