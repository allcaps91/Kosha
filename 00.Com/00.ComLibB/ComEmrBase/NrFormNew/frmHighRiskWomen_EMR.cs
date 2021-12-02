using System;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : frmHighRiskWomen.cs
    /// Description     : 고위험 산모 구분 선택
    /// Author          : 박창욱
    /// Create Date     : 2018-07-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrinfo\FrmHighRiskWomen.frm(FrmHighRiskWomen.frm) >> frmHighRiskWomen.cs 폼이름 재정의" />	
    public partial class frmHighRiskWomen_EMR : Form
    {
        public delegate void SetGstr(string strGbn, string strTime);
        public event SetGstr rSetGstr;

        public frmHighRiskWomen_EMR()
        {
            InitializeComponent();
        }

        private void frmHighRiskWomen_EMR_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            rdo_Clear();
        }

        void rdo_Clear()
        {
            rdoOption1.Checked = false;
            rdoOption2.Checked = false;
            rdoOption3.Checked = false;
            rdoOption4.Checked = false;
            rdoOption5.Checked = false;
            rdoOption6.Checked = false;
            rdoOption7.Checked = false;
            rdoOption8.Checked = false;
            rdoOption9.Checked = false;
            rdoOption10.Checked = false;
            rdoOption11.Checked = false;
            rdoOption12.Checked = false;

            rdoOption1.Font = new Font(rdoOption1.Font, FontStyle.Regular);
            rdoOption2.Font = new Font(rdoOption2.Font, FontStyle.Regular);
            rdoOption3.Font = new Font(rdoOption3.Font, FontStyle.Regular);
            rdoOption4.Font = new Font(rdoOption4.Font, FontStyle.Regular);
            rdoOption5.Font = new Font(rdoOption5.Font, FontStyle.Regular);
            rdoOption6.Font = new Font(rdoOption6.Font, FontStyle.Regular);
            rdoOption7.Font = new Font(rdoOption7.Font, FontStyle.Regular);
            rdoOption8.Font = new Font(rdoOption8.Font, FontStyle.Regular);
            rdoOption9.Font = new Font(rdoOption9.Font, FontStyle.Regular);
            rdoOption10.Font = new Font(rdoOption10.Font, FontStyle.Regular);
            rdoOption11.Font = new Font(rdoOption11.Font, FontStyle.Regular);
            rdoOption12.Font = new Font(rdoOption12.Font, FontStyle.Regular);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            string strGbn = "";
            string strTime = "";

            if (rdoOption1.Checked == true)
            {
                strGbn = "00";
            }
            else if (rdoOption2.Checked == true)
            {
                strGbn = "01";
            }
            else if (rdoOption3.Checked == true)
            {
                strGbn = "02";
            }
            else if (rdoOption4.Checked == true)
            {
                strGbn = "03";
            }
            else if (rdoOption5.Checked == true)
            {
                strGbn = "04";
            }
            else if (rdoOption6.Checked == true)
            {
                strGbn = "05";
            }
            else if (rdoOption7.Checked == true)
            {
                strGbn = "06";
            }
            else if (rdoOption8.Checked == true)
            {
                strGbn = "07";
            }
            else if (rdoOption9.Checked == true)
            {
                strGbn = "08";
            }
            else if (rdoOption10.Checked == true)
            {
                strGbn = "09";
            }
            else if (rdoOption11.Checked == true)
            {
                strGbn = "10";
            }
            else if (rdoOption12.Checked == true)
            {
                strGbn = "11";
            }


            //0 일반
            //1 야간
            //4 심야

            if (rdoTime1.Checked == true)
            {
                strTime = "4";
            }
            else if (rdoTime2.Checked == true)
            {
                strTime = "1";
            }
            else if (rdoTime3.Checked == true)
            {
                strTime = "0";
            }


            if (strGbn == "")
            {
                ComFunc.MsgBox("고위험 구분을 선택해 주세요.");
                return;
            }

            if (strTime == "")
            {
                ComFunc.MsgBox("분만시간을 선택해 주세요.");
                return;
            }

            rSetGstr(strGbn, strTime);

            this.Close();
        }

        private void rdoOption_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                ((RadioButton)sender).Font = new Font(((RadioButton)sender).Font, FontStyle.Bold);
            }
        }
    }
}
