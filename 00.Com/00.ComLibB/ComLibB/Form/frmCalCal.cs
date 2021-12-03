using System;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmCalCal
    /// Description     : 권장 열량 계산기
    /// Author          : 전상원
    /// Create Date     : 2018-05-24
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Ocs\ipdocs\iorder\mtsiorder.vbp(FrmCalcal) >> frmCalCal.cs 폼이름 재정의" />
    public partial class frmCalCal : Form
    {
        public frmCalCal()
        {
            InitializeComponent();
        }

        private void frmCalCal_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Clear();
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            string strHEIGHT = "";
            string strWEIGHT = "";
            string strSex = "";

            strHEIGHT = VB.Trim(txtHeight.Text);
            strWEIGHT = VB.Trim(txtWeight.Text);

            if (rdoSex1.Checked == true)
            {
                strSex = "M";
            }
            else if (rdoSex2.Checked == true)
            {
                strSex = "F";
            }

            if (strHEIGHT == "")
            {
                ComFunc.MsgBox("키가 공란입니다.");
                return;
            }

            if (strWEIGHT == "")
            {
                ComFunc.MsgBox("몸무게가 공란입니다.");
                return;
            }

            if (strSex == "")
            {
                ComFunc.MsgBox("성별이 공란입니다.");
                return;
            }

            lblCal.Text = GET_Kcal(strHEIGHT, strWEIGHT, strSex);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Clear()
        {
            txtHeight.Text = "";
            txtWeight.Text = "";
            txtAge.Text = "";

            rdoSex1.Checked = false;
            rdoSex2.Checked = false;
            lblCal.Text = "";
        }

        private string GET_Kcal(string argH, string argW, string ArgSex)
        {
            //권장 열량 구하기(세자리에서 반올림 함)
            //권장 열량 = 적정체중 * 30
            //적정체중 = (남) 키 * 키 * 22
            //적정체중 = (여) 키 * 키 * 21

            string rtnVal = "";

            if (VB.IsNumeric(argH) && VB.IsNumeric(argW))
            {

            }
            else
            {
                rtnVal = "Error";
                return rtnVal;
            }

            if (argH == "")
            {
                return rtnVal;
            }

            if (ArgSex == "M")
            {
                rtnVal = (Math.Round((((VB.Val(argH) * 0.01) * (VB.Val(argH) * 0.01) * 22) * 30) * 0.01) * 100).ToString().Trim();
            }
            else if (ArgSex == "F")
            {
                rtnVal = (Math.Round((((VB.Val(argH) * 0.01) * (VB.Val(argH) * 0.01) * 21) * 30) * 0.01) * 100).ToString().Trim();
            }
            else
            {
                ComFunc.MsgBox("성별이 없습니다.", "확인");
            }

            return rtnVal;
        }
    }
}
