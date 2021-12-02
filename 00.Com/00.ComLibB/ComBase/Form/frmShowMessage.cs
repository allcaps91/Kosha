using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ComBase
{
    public partial class frmShowMessage : Form
    {
        private Button btnOk = null;
        private Button btnNo = null;
        private Button btnYes = null;

        private Form mOwner = null;
        private string mMessageText = "";
        private string mMessageTitle = "";
        private MessageBoxButtons mMessageButton = MessageBoxButtons.OK;
        private MessageBoxIcon mMessageIcon = MessageBoxIcon.Information;
        private MessageBoxDefaultButton mDefaultButton = MessageBoxDefaultButton.Button2;

        public frmShowMessage()
        {
            InitializeComponent();
        }

        public frmShowMessage(Form owner, string messageText, string messageTitle, MessageBoxButtons messageButton, MessageBoxIcon messageIcon, MessageBoxDefaultButton defaultButton)
        {
            InitializeComponent();
            mOwner = Owner;
            mMessageText = messageText;
            mMessageTitle = messageTitle;
            mMessageButton = messageButton;
            mMessageIcon = messageIcon;
            mDefaultButton = defaultButton;
        }

        private void frmShowMessage_Load(object sender, EventArgs e)
        {
            if (mMessageButton == MessageBoxButtons.YesNo)
            {
                if (mDefaultButton == MessageBoxDefaultButton.Button1)
                {
                    btnNo.TabIndex = 3;
                    btnYes.TabIndex = 0;
                    btnYes.Focus(); 
                }
                else
                {
                    btnYes.TabIndex = 3;
                    btnNo.TabIndex = 0;
                    btnNo.Focus(); 
                }
            }
            this.Height = this.lblMessageText.Height + 100;
        }

        
        protected override void OnPaint(PaintEventArgs e)
        {
            //Rectangle rect = this.ClientRectangle;
            //LinearGradientBrush brush = new LinearGradientBrush(rect, Color.SkyBlue, Color.AliceBlue, 60);
            //e.Graphics.FillRectangle(brush, rect);
            //base.OnPaint(e);
        }

        
        private void setMessage(string messageText)
        {
            int maxWidth = Screen.GetWorkingArea(this).Width - 480;
            int useWidth = Math.Min(TextRenderer.MeasureText(messageText,
                                    new Font("맑은 고딕", 10, FontStyle.Regular)).Width, maxWidth);
            useWidth = Math.Max(useWidth, 400);
            useWidth = lblMessageText.Width;
            int useHeight = Math.Max(64, TextRenderer.MeasureText(messageText,
                                         new Font("맑은 고딕", 10, FontStyle.Regular),
                                         new Size(useWidth, 0),
                                         TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak)
                                         .Height);

            lblMessageText.Size = new Size(useWidth, useHeight);
            this.lblMessageText.Text = messageText;
        }

        private void setMessage_Old(string messageText)
        {
            //int number = Math.Abs(messageText.Length / 30);
            int number = Math.Abs(Encoding.Default.GetByteCount(messageText) / 30);
            MatchCollection matches = Regex.Matches(messageText, "\r\n");
            int cnt = matches.Count;

            if (number != 0)
            {
                if (cnt > number)
                {
                    this.lblMessageText.Height = cnt * 25;
                    //this.Height = this.Height + (cnt * 25);
                }
                else
                {
                    number = 2;
                    this.lblMessageText.Height = number * 25;
                    if (number > 0 && number < 8)
                    {
                        this.Height = this.Height + (cnt * 25);
                    }
                }
            }
            else
            {
                if (cnt > 0)
                {
                    this.lblMessageText.Height = cnt * 25;
                    //this.Height = this.Height + (cnt * 25);
                }
            }

            this.lblMessageText.Text = messageText;
        }

        private void addButton(MessageBoxButtons MessageButton)
        {
            switch (MessageButton)
            {
                case MessageBoxButtons.OK:
                    {
                        //If type of enumButton is OK then we add OK button only.
                        btnOk = new Button();  //Create object of Button.
                        btnOk.Text = "확 인(&Y)";  //Here we set text of Button.
                        btnOk.DialogResult = DialogResult.OK;  //Set DialogResult property of button.
                        btnOk.FlatStyle = FlatStyle.Popup;  //Set flat appearence of button.
                        btnOk.FlatAppearance.BorderSize = 0;
                        btnOk.SetBounds(pnlShowMessage.ClientSize.Width - 110, 5, 100, 32);  // Set bounds of button.
                        pnlShowMessage.Controls.Add(btnOk);  //Finally Add button control on panel.
                    }
                    break;
                case MessageBoxButtons.YesNo:
                    {
                        btnYes = new Button();
                        //btnYes.Click += new System.EventHandler(btnYes_Click);
                        btnYes.Text = "예(&Y)";
                        btnYes.DialogResult = DialogResult.Yes;
                        btnYes.FlatStyle = FlatStyle.Popup;
                        btnYes.FlatAppearance.BorderSize = 0;
                        //btnYes.SetBounds((pnlShowMessage.ClientSize.Width - (btnNo.ClientSize.Width + 5 + 80)), 5, 80, 25);
                        btnYes.SetBounds(90, 5, 100, 32);
                        pnlShowMessage.Controls.Add(btnYes);

                        btnNo = new Button();
                        btnNo.Text = "아니요(&N)";
                        btnNo.DialogResult = DialogResult.No;
                        btnNo.FlatStyle = FlatStyle.Popup;
                        btnNo.FlatAppearance.BorderSize = 0;
                        //btnNo.SetBounds((pnlShowMessage.ClientSize.Width - 70), 5, 80, 25);
                        btnNo.SetBounds(300, 5, 100, 32);
                        pnlShowMessage.Controls.Add(btnNo);
                    }
                    break;
            }
        }
        
        private void addIconImage(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    pictureBox1.Image = imageList1.Images["Error"];  //Error is key name in imagelist control which uniqly identified images in ImageList control.
                    break;
                case MessageBoxIcon.Information:
                    pictureBox1.Image = imageList1.Images["Information"];
                    break;
                case MessageBoxIcon.Question:
                    pictureBox1.Image = imageList1.Images["Question"];
                    break;
                case MessageBoxIcon.Warning:
                    pictureBox1.Image = imageList1.Images["Warning"];
                    break;
            }
        }
        
        #region Overloaded Show message to display message box.

        internal static DialogResult Show(string messageText)
        {
            DialogResult rtnVal = DialogResult.OK;
            frmShowMessage frmMessage = new frmShowMessage(null, messageText, "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            frmMessage.setMessage(messageText);
            frmMessage.addIconImage(MessageBoxIcon.Information);
            frmMessage.addButton(MessageBoxButtons.OK);
            frmMessage.TopMost = true;
            frmMessage.ShowDialog();
            rtnVal = frmMessage.ShowDialog();
            return rtnVal;
        }

        internal static DialogResult Show(Form owner, string messageText)
        {
            DialogResult rtnVal = DialogResult.OK;
            frmShowMessage frmMessage = new frmShowMessage(owner, messageText, "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            frmMessage.setMessage(messageText);
            frmMessage.addIconImage(MessageBoxIcon.Information);
            frmMessage.addButton(MessageBoxButtons.OK);
            frmMessage.Owner = owner;
            frmMessage.StartPosition = FormStartPosition.CenterParent;
            frmMessage.TopMost = true;
            rtnVal = frmMessage.ShowDialog();
            return rtnVal;
        }

        internal static DialogResult Show(string messageText, string messageTitle)
        {
            DialogResult rtnVal = DialogResult.OK;
            frmShowMessage frmMessage = new frmShowMessage(null, messageText, messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            frmMessage.Text = messageTitle;
            frmMessage.setMessage(messageText);
            frmMessage.addIconImage(MessageBoxIcon.Information);
            frmMessage.addButton(MessageBoxButtons.OK);
            frmMessage.TopMost = true;
            rtnVal = frmMessage.ShowDialog();
            return rtnVal;
        }

        internal static DialogResult Show(Form owner, string messageText, string messageTitle)
        {
            DialogResult rtnVal = DialogResult.OK;
            frmShowMessage frmMessage = new frmShowMessage(owner, messageText, messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            frmMessage.Text = messageTitle;
            frmMessage.setMessage(messageText);
            frmMessage.addIconImage(MessageBoxIcon.Information);
            frmMessage.addButton(MessageBoxButtons.OK);
            frmMessage.Owner = owner;
            frmMessage.StartPosition = FormStartPosition.CenterParent;
            frmMessage.TopMost = true;
            rtnVal = frmMessage.ShowDialog();
            return rtnVal;
        }

        internal static DialogResult Show(string messageText, string messageTitle,  MessageBoxButtons messageButton, MessageBoxIcon messageIcon)
        {
            DialogResult rtnVal = DialogResult.OK;
            frmShowMessage frmMessage = new frmShowMessage(null, messageText, messageTitle, messageButton, messageIcon, MessageBoxDefaultButton.Button1);
            frmMessage.setMessage(messageText);
            frmMessage.Text = messageTitle;
            frmMessage.addIconImage(messageIcon);
            frmMessage.addButton(messageButton);
            frmMessage.TopMost = true;
            rtnVal = frmMessage.ShowDialog();
            return rtnVal;
        }

        internal static DialogResult Show(Form owner, string messageText, string messageTitle, MessageBoxButtons messageButton, MessageBoxIcon messageIcon)
        {
            DialogResult rtnVal = DialogResult.OK;
            frmShowMessage frmMessage = new frmShowMessage(owner, messageText, messageTitle, messageButton, messageIcon, MessageBoxDefaultButton.Button1);
            frmMessage.setMessage(messageText);
            frmMessage.Text = messageTitle;
            frmMessage.addIconImage(messageIcon);
            frmMessage.addButton(messageButton);
            frmMessage.Owner = owner;
            frmMessage.StartPosition = FormStartPosition.CenterParent;
            frmMessage.TopMost = true;
            rtnVal = frmMessage.ShowDialog();
            return rtnVal;
        }

        internal static DialogResult Show(string messageText, string messageTitle, MessageBoxButtons messageButton, MessageBoxIcon messageIcon, MessageBoxDefaultButton defaultButton)
        {
            DialogResult rtnVal = DialogResult.OK;
            frmShowMessage frmMessage = new frmShowMessage(null, messageText, messageTitle, messageButton, messageIcon, defaultButton);
            frmMessage.setMessage(messageText);
            frmMessage.Text = messageTitle;
            frmMessage.addIconImage(messageIcon);
            frmMessage.addButton(messageButton);
            frmMessage.TopMost = true;
            rtnVal = frmMessage.ShowDialog();
            return rtnVal;
        }

        internal static DialogResult Show(Form owner, string messageText, string messageTitle, MessageBoxButtons messageButton, MessageBoxIcon messageIcon, MessageBoxDefaultButton defaultButton)
        {
            DialogResult rtnVal = DialogResult.OK;
            frmShowMessage frmMessage = new frmShowMessage(owner, messageText, messageTitle, messageButton, messageIcon, defaultButton);
            frmMessage.setMessage(messageText);
            frmMessage.Text = messageTitle;
            frmMessage.addIconImage(messageIcon);
            frmMessage.addButton(messageButton);
            frmMessage.Owner = owner;
            frmMessage.StartPosition = FormStartPosition.CenterParent;
            frmMessage.TopMost = true;
            rtnVal = frmMessage.ShowDialog();
            return rtnVal;
        }

        #endregion


    }
    
}
