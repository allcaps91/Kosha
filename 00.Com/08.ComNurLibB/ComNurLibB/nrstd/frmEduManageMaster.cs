using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmEduManageMaster.cs
    /// Description     : 원내교육관리프로그램마스타
    /// Author          : 박창욱
    /// Create Date     : 2018-01-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO : 폼 연결
    /// TODO : pictureBox1에 이미지 추가
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm교육관리마스타.frm(Frm교육관리마스타.frm) >> frmEduManageMaster.cs 폼이름 재정의" />	
    public partial class frmEduManageMaster : Form
    {
        public frmEduManageMaster()
        {
            InitializeComponent();
        }

        private void btnBase_Click(object sender, EventArgs e)
        {
            //TODO : 폼 호출
            //Frm교육관리기초1.Show 1
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strSabun = "";

            clsPublic.GstrPassProgramID = "";
            //TODO : 폼 호출
            //FrmPassword.Show 1

            btnMaster.Enabled = false;
            btnBase.Enabled = false;

            if (VB.Val(clsType.User.Sabun) <= 99999)
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("00000");
            }
            else
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("000000");
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
                SQL = SQL + ComNum.VBLF + "  WHERE Gubun ='1'";
                SQL = SQL + ComNum.VBLF + "    AND Remark ='" + strSabun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                btnMaster.Enabled = true;
                btnBase.Enabled = true;

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                lblSabun.Text = "현재 로그인 상태 [ 성명 : " + clsType.User.JobName + "  사번 : " + clsType.User.Sabun + " ]";
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnMaster_Click(object sender, EventArgs e)
        {
            //TODO : 폼 호출
            //Frm교육관리등록1.Show 1
        }

        private void btnPersonal_Click(object sender, EventArgs e)
        {
            //TODO : 폼 호출
            //Frm교육관리등록2.Show 1
        }

        private void frmEduManageMaster_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strSabun = "";

            clsPublic.GstrPassProgramID = "";
            //TODO : 폼 호출
            //FrmPassword.Show 1

            btnMaster.Enabled = false;
            btnBase.Enabled = false;

            if (VB.Val(clsType.User.Sabun) <= 99999)
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("00000");
            }
            else
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("000000");
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
                SQL = SQL + ComNum.VBLF + "  WHERE Gubun ='1'";
                SQL = SQL + ComNum.VBLF + "    AND Remark ='" + strSabun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                btnMaster.Enabled = true;
                btnBase.Enabled = true;

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                lblSabun.Text = "현재 로그인 상태 [ 성명 : " + clsType.User.JobName + "  사번 : " + clsType.User.Sabun + " ]";
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
