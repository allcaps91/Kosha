using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaViewOutRoomTagetST : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmpaViewOutRoomTagetST.cs
        /// Description     : 퇴원등록 대상자 확인
        /// Author          : 김효성
        /// Create Date     : 2017-08-24
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "\IPD\ipdSim2\iusent.vbp(Frm퇴원등록대상자조회) >> frmPmpaViewOutRoomTagetST.cs 폼이름 재정의" />	

        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewOutRoomTagetST()
        {
            InitializeComponent();
        }

        private void frmPmpaViewOutRoomTagetST_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }

        private void Search()
        {

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int nREAD = 0;
            int nRow = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.Pano, b.SName, b.Bi";
                SQL = SQL + ComNum.VBLF + "	, c .GBIPD, b.WardCode, b.RoomCode";
                SQL = SQL + ComNum.VBLF + "	, c.DeptCode, c.DrCode, c.GbSPC, c.GBDRG";
                SQL = SQL + ComNum.VBLF + "	, TO_CHAR(c.InDate,'YYYY-MM-DD') INDATE";
                SQL = SQL + ComNum.VBLF + "	, TO_CHAR(c.OutDate,'YYYY-MM-DD') OUTDATE, c.Ilsu, c.TRSNO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TORDER_CHK a, " + ComNum.DB_PMPA + "IPD_NEW_MASTER b, " + ComNum.DB_PMPA + "IPD_TRANS c ";
                SQL = SQL + ComNum.VBLF + " WHERE a.ACTDATE>=TO_DATE('" + DateTime.Parse(strdtP).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND (a.CHK IS NULL OR a.CHK = '') ";
                SQL = SQL + ComNum.VBLF + "   AND a.Pano = b.Pano ";
                SQL = SQL + ComNum.VBLF + "   AND b.IPDNO = c.IPDNO ";
                SQL = SQL + ComNum.VBLF + "   AND a.IPDNO = c.IPDNO ";
                SQL = SQL + ComNum.VBLF + "   AND a.TRSNO = c.TRSNO ";
                SQL = SQL + ComNum.VBLF + "   AND b.GBSTS <> '9' ";
                SQL = SQL + ComNum.VBLF + "   AND c.GBIPD <> 'D' ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = VB.IIf(dt.Rows[i]["GBIPD"].ToString().Trim() == "9", "지병", "").ToString();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["GbSPC"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["GBDRG"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Ilsu"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["TRSNO"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (Save() == true)
            {
                this.Close();
            }
        }

        private bool Save()
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return false;//권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            bool rtnVal = false;
            int nTRSNO = 0;
            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    nTRSNO = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 13].Text));

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " UPDATE IPD_TORDER_CHK SET CHK = 'Y' ";
                    SQL = SQL + ComNum.VBLF + "  WHERE TRSNO = " + nTRSNO + " ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
