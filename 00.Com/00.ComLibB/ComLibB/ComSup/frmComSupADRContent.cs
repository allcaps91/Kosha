using ComBase;
using System;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupADRContent.cs
    /// Description     : 약물이상반응의 내용(여러개 선택 가능)
    /// Author          : 이정현
    /// Create Date     : 2018-01-15
    /// <history> 
    /// 약물이상반응의 내용(여러개 선택 가능)
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\Frm약물이상반응.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupADRContent : Form
    {
        public delegate void SendDataHandler(string SendRetValue);
        public event SendDataHandler SendEvent;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmComSupADRContent()
        {
            InitializeComponent();
        }

        private void frmComSupADRContent_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string strTemp = "";

            strTemp = READ_CHK_NAME();

            if (strTemp == "")
            {
                ComFunc.MsgBox("선택된 반응이 없습니다.");
                return;
            }

            if (SendEvent != null)
            {
                SendEvent(strTemp);
            }
        }

        private string READ_CHK_NAME()
        {
            string rtnVal = "";

            Control[] controls = ComFunc.GetAllControls(this);

            foreach(Control control in controls)
            {
                if (control is CheckBox)
                {
                    if (((CheckBox)control).Checked == true)
                    {
                        rtnVal += ((CheckBox)control).Text + ", ";
                    }
                }
            }

            if (rtnVal != "")
            {
                rtnVal = VB.Mid(rtnVal, 1, rtnVal.Length - 2);
            }

            return rtnVal;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (rEventClosed == null)
            {
                this.Close();
            }
            else
            {
                rEventClosed();
            }
        }
    }
}
