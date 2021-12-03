using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComLibB
{
    /// <summary>
    /// Class Name : frmPmpaJSimsaGijun
    /// File Name : frmPmpaJSimsaGijun.cs
    /// Title or Description : 재원심사시 심사기준
    /// Author : 김민철
    /// Create Date : 2017-06-15
    /// Update History : 
    /// <seealso cref="d:\psmh\Ocs\FrmSimSaGiJun.frm"/>
    /// </summary>
    public partial class frmPmpaJSimsaGijun : Form
    {
        string FStrSuCode;

        public frmPmpaJSimsaGijun(string sSuCode)
        {   
            InitializeComponent();
            FStrSuCode = sSuCode;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Screen_Clear()
        {
            txtSuCode.Text = "";
            lblSuName.Text = "";
            txtChkMsg.Text = "";
            txtRemark.Text = "";
        }

        private void frmPmpaJSimsaGijun_Load(object sender, EventArgs e)
        {
            Screen_Clear();

            if (FStrSuCode != "")
            {
                txtSuCode.Text = FStrSuCode;                
            }
            
            Screen_Display(txtSuCode.Text.Trim());

            //if (clsPmpaPb.GstrSuCode != "")
            //{
            //    txtSuCode.Text = clsPmpaPb.GstrSuCode.ToUpper();
            //    Screen_Display(txtSuCode.Text.Trim());
            //}
        }

        private void Screen_Display(string strSuCode)
        {

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUNAMEK, UNIT ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                SQL += ComNum.VBLF + "  WHERE SUNEXT = '" + strSuCode + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("수가에 등록되지 않은 코드입니다.");
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                lblSuName.Text = Dt.Rows[0]["SUNAMEK"].ToString().Trim();

                Dt.Dispose();
                Dt = null;


                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.SUCODE, A.DAYQTY, A.IPWONQTY, A.BCHECK, ";
                SQL += ComNum.VBLF + "        A.GUPDEPT1, A.GUPDEPT2, A.GUPDEPT3 , A.GUPDEPT4 , A.GUPDEPT5 ,";
                SQL += ComNum.VBLF + "        A.BIGUPDEPT1, A.BIGUPDEPT2, A.BIGUPDEPT3, A.BIGUPDEPT4, A.BIGUPDEPT5, ";
                SQL += ComNum.VBLF + "        A.CHECKMSG, A.REMARK, A.DISPLAY, A.ITEMCD, A.FRRESULT, A.TORESULT  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "JSIM_GIJUN A ";
                SQL += ComNum.VBLF + "  WHERE A.SUCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                txtChkMsg.Text = Dt.Rows[0]["CHECKMSG"].ToString().Trim();
                txtRemark.Text = Dt.Rows[0]["REMARK"].ToString().Trim(); 

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void txtSuCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                txtSuCode.Text = txtSuCode.Text.ToUpper();
                Screen_Display(txtSuCode.Text.Trim());
            }
        }

    }
}
