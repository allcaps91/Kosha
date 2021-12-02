using System;
using System.Data;
using System.Windows.Forms;


namespace ComBase
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-16
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\mtsEmr\CADEX\FrmBloodAgree" >> frmBloodAgree.cs 폼이름 재정의" />
    /// 
    public partial class frmBloodAgree : Form
    {
        string inmomation = "";

        string fstrPTNO = "";
        string fstrIO = "";
        string FstrDeptCode = "";
        string fstrMEDFRDATE = "";

        public frmBloodAgree(string inmomationX)
        {
            inmomation = inmomationX;//GstrHelpCode = gptEmrPt.PtPtNo & "||I||" & gptEmrPt.PtMedDeptCd & "||" & gptEmrPt.PtMedFrDate

            InitializeComponent();
        }

        public frmBloodAgree()
        {
            InitializeComponent();
        }

        private void frmBloodAgree_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string[] str = new string[4];

            str = VB.Split(inmomation, "||");

            fstrPTNO = str[0];
            fstrIO = str[1];
            FstrDeptCode = str[2];
            fstrMEDFRDATE = str[3];

            if (fstrPTNO == "")
            {
                MessageBox.Show("등록번호가 없습니다. 의료정보과로 연락바랍니다");
                btnSave.Enabled = false;
            }

            ReadBloodAgree();
        }

        private void ReadBloodAgree()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            lbDate.Text = "";
            lbSabun.Text = "";
            txtMemo.Text = "";
            chkOK.Checked = false;

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WRITEDATE, WRITESABUN, MEMO, GUBUN ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMR_CADEX_BLOODAGREE A";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + fstrPTNO + "' ";
                if (FstrDeptCode == "HD")
                {
                    SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE >= '" + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-365).ToString("yyyy-MM-dd").Replace("-", "");
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE = '" + fstrMEDFRDATE + "'";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY WRITEDATE DESC, WRITETIME DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    lbDate.Text = VB.Val(dt.Rows[0]["WRITEDATE"].ToString().Trim()).ToString("0000-00-00");
                    lbSabun.Text = clsVbfunc.READ_INSA_BUSE(clsDB.DbCon, VB.Val(dt.Rows[0]["WRITESABUN"].ToString().Trim()).ToString("00000")) + " " +
                                    clsVbfunc.GetInSaName(clsDB.DbCon, VB.Val(dt.Rows[0]["WRITESABUN"].ToString().Trim()).ToString("00000"));
                    txtMemo.Text = dt.Rows[0]["MEMO"].ToString().Trim();
                    chkOK.Checked = dt.Rows[0]["GUBUN"].ToString().Trim() == "1" ? true : false;
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " DELETE " + ComNum.DB_EMR + "EMR_CADEX_BLOODAGREE ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + fstrPTNO + "'";
                SQL = SQL + ComNum.VBLF + " AND  IO = '" + fstrIO + "' ";
                SQL = SQL + ComNum.VBLF + " AND MEDFRDATE = '" + fstrMEDFRDATE + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;

                }

                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_EMR + "EMR_CADEX_BLOODAGREE (";
                SQL = SQL + ComNum.VBLF + " MEDFRDATE, WRITEDATE, WRITETIME, WRITESABUN, PTNO, ";
                SQL = SQL + ComNum.VBLF + " IO, DEPTCODE, GUBUN, MEMO) VALUES (";
                SQL = SQL + ComNum.VBLF + "'" + fstrMEDFRDATE + "','" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-").Replace("-", "") + "','" + VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), 4) + "','" + clsType.User.Sabun + "','" + fstrPTNO + "', ";
                SQL = SQL + ComNum.VBLF + "'" + fstrIO + "','" + FstrDeptCode + "','" + (chkOK.Checked == true ? "1" : "0") + "','" + (txtMemo.Text).Trim() + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;

                }



                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                this.Close();

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
