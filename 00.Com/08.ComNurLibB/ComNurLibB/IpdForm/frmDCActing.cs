using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComNurLibB
{
    public partial class frmDCActing : Form
    {
        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        //Messgae Send
        public delegate void SendMsg(string strHospCode );
        public event SendMsg rSendMsg;

        public frmDCActing()
        {
            InitializeComponent();
        }

        private void frmDCActing_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string strHospCode = "";

            if (radioCase1.Checked ==true )
            {
                strHospCode = "DC사유" + radioCase1.Text;

            }
            else if (radioCase2.Checked == true)
            {
                strHospCode = "DC사유" + radioCase2.Text;
            }
            else if (radioCase3.Checked == true)
            {
                strHospCode = "DC사유" + radioCase3.Text;
            }
            else if (radioCase4.Checked == true)
            {
                strHospCode = "DC사유" + radioCase4.Text + TxtRemark.Text; ;
            }
            
            rSendMsg(strHospCode);
            rEventClosed();
        }
    }
}
