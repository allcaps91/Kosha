using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComLibB;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewEntitlementSearch.cs
    /// Description     : 보호자자격조회
    /// Author          : 박창욱
    /// Create Date     : 2017-11-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iument\Frm보호자자격조회.frm(Frm보호자자격조회.frm) >> frmPmpaViewEntitlementSearch.cs 폼이름 재정의" />	
    public partial class frmPmpaViewEntitlementSearch : Form
    {
        private string gstrHelpCode = "";

        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        //단독폼으로 실행되므로 델리게이트 주석처리
        //public delegate void EventClose();
        //public event EventClose rEventClose;

        public frmPmpaViewEntitlementSearch()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtPano.Text == "" && txtSname.Text == "")
            {
                ComFunc.MsgBox("성명 혹은 등록번호가 공란입니다.");
                return;
            }

            if (txtPano.Text == "" && txtSname.Text != "")
            {
                if (txtJumin1.Text == "" || txtJumin2.Text == "")
                {
                    ComFunc.MsgBox("주민번호가 공란입니다.");
                    return;
                }
            }

            string strPano = txtPano.Text.ToString();
            //rSetHelpCode(txtPano.Text.Trim() + ",ME," + txtSname.Text.Trim() + "," + txtJumin1.Text + txtJumin2.Text + "," +
            //             ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            frmPmpaCheckNhic frm = new frmPmpaCheckNhic(strPano, "ME", txtSname.Text, txtJumin1.Text, txtJumin2.Text, ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), "");

            frm.ShowDialog();
        }

        private void frmPmpaViewEntitlementSearch_Load(object sender, EventArgs e)
        {
            
            txtPano.Text = "00000000";
            txtJumin1.Text = "";
            txtJumin2.Text = "";
            txtSname.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //rEventClose();
            this.Close();
            return;
        }

        private void txtSname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtJumin1.Focus();
            }
        }

        private void txtJumin1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtJumin2.Focus();
            }
        }

        private void txtJumin2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtJumin1.Text != "" && txtJumin2.Text != "")
                {
                    txtPano.Text = READ_Jumin2_Pano(txtJumin1.Text, txtJumin2.Text);
                }

                if (txtPano.Text != "")
                {
                    btnSearch.Focus();
                }
                else
                {
                    txtPano.Focus();
                }
            }

        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        string READ_Jumin2_Pano(string argJumin1, string argJumin2)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE Jumin1='" + argJumin1 + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Jumin3='" + clsAES.AES(argJumin2) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["Pano"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

        }
        
    }
}
