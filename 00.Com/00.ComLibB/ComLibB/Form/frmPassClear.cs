using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmPassClear : Form
    {
        public frmPassClear()
        {
            InitializeComponent();
        }

        private void frmPassClear_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            txtSabun.Text = "";
            lblInfo.Text = "";
        }

        /// <summary>
        /// 텍스트 박스의 값으로 lblInfo에 성명 표시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSabun_KeyPress(object sender, KeyPressEventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (e.KeyChar == 13)
            {
                lblInfo.Text = "";

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Name, PassWard, Grade, Part, ProgramID,Charge ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_PASS ";
                SQL = SQL + ComNum.VBLF + " WHERE ProgramID = ' ' ";
                SQL = SQL + ComNum.VBLF + "   AND IDnumber = " + VB.Val(txtSabun.Text) + " ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblInfo.Text = "성명 : " + dt.Rows[0]["NAME"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                SendKeys.Send("{TAB}");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("정말 패스워드 초기화 작업을 하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (UpdateData() == false) return;
        }

        /// <summary>
        /// 비밀번호 초기화 함수
        /// </summary>
        /// <returns>True:초기화 작업 False:초기화 불가</returns>
        private bool UpdateData()
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PASSWORD ";
                SQL = SQL + ComNum.VBLF + "  WHERE ID =" + VB.Val(txtSabun.Text) + " ";
                SQL = SQL + ComNum.VBLF + "   AND (EDATE IS NULL OR EDATE ='') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PASSWORD SET ";
                    SQL = SQL + ComNum.VBLF + "  EDate = SYSDATE, ID_CLEAR =" + clsPublic.GnJobSabun + " ";
                    SQL = SQL + ComNum.VBLF + "  WHERE ID =" + VB.Val(txtSabun.Text) + " ";
                    SQL = SQL + ComNum.VBLF + "   AND (EDATE IS NULL OR EDATE ='') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                }

                dt.Dispose();
                dt = null;
                clsDB.setCommitTran(clsDB.DbCon);
                MessageBox.Show("패스워드 초기화 완료", "확인");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }
    }
}
