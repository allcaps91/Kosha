using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 위험환자관리 메세지
/// Author : 박병규
/// Create Date : 2017.10.18
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaViewMsg : Form
    {
        string FstrMsgGubun = string.Empty;

        public frmPmpaViewMsg()
        {
            InitializeComponent();
            setParam();
        }

        public frmPmpaViewMsg(string ArgMsgGubun)
        {
            InitializeComponent();
            FstrMsgGubun = ArgMsgGubun;
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            int i = 0;
            int nStart = 0;
            int nEnd = 0;

            if (FstrMsgGubun == "R")
                RtxtMsg.Rtf = clsPmpaPb.GstrDoctMsg.Replace("`","'");
            else if (FstrMsgGubun == "S")
                RtxtMsg.Text = clsPmpaPb.GstrDoctMsg;

            for (i = 1; i <= RtxtMsg.Text.Length; i++)
            {
                if (nStart == 0 && VB.Mid(RtxtMsg.Text, i, 1) == "☞")
                    nStart = i - 1;

                if (nStart != 0)
                {
                    if (VB.Mid(RtxtMsg.Text, i, 1) == "\r")
                        nEnd = i - 1 - nStart;
                }

                if (nStart != 0 && nEnd != 0)
                {
                    RtxtMsg.SelectionStart = nStart;
                    RtxtMsg.SelectionLength = nEnd;
                    RtxtMsg.SelectionColor = Color.Red;
                    nStart = 0;
                    nEnd = 0;
                }
            }

            RtxtMsg.SelectionStart = i;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            clsPmpaPb.GstrDoctMsg = "";
            this.Close();
        }
    }
}
