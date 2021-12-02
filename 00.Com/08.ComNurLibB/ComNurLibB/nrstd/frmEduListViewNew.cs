using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmEduListViewNew.cs
    /// Description     : 병원교육리스트조회New
    /// Author          : 박창욱
    /// Create Date     : 2018-01-31
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm병원교육리스트New.frm(Frm병원교육리스트New.frm) >> frmEduListViewNew.cs 폼이름 재정의" />	
    public partial class frmEduListViewNew : Form
    {
        public frmEduListViewNew()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Search_Data();
        }

        void Search_Data()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT  GUBUN , JONG, Code, Name, EDUTIME,";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(FrDate,'YYYY-MM-DD') FrDate, TO_CHAR(ToDate,'YYYY-MM-DD') ToDate,";
                SQL = SQL + ComNum.VBLF + "   OptTime , STIME, MAN, OptPlace, PLACE, JUMSU, EntDate, ROWID";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "NUR_EDU_CODE ";
                SQL = SQL + ComNum.VBLF + "   WHERE FRDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     AND FRDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     AND GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "   ORDER BY FRDATE DESC";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = clsNrstd.READ_EDU_JONG(dt.Rows[i]["Jong"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FrDate"].ToString().Trim();

                    if (dt.Rows[i]["ToDate"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 2].Text += "~" + dt.Rows[i]["ToDate"].ToString().Trim();
                    }

                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["EDUTIME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PLACE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["MAN"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = Read_MYEDU(dt.Rows[i]["CODE"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        string Read_MYEDU(string argWRTNO)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT SIGN ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_EDU_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = '" + argWRTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN = '2' ";
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(SABUN) = '" + clsType.User.Sabun + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["SIGN"].ToString().Trim() != "")
                    {
                        rtnVar = "참석확인하세요";
                    }
                    else
                    {
                        rtnVar = "교육 참석";
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVar;
            }
        }

        private void frmEduListViewNew_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-40);

            Search_Data();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
