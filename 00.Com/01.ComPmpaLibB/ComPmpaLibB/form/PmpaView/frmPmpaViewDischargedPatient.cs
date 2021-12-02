using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewDischargedPatient.cs
    /// Description     : 선택진료 퇴원자 명부 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-07
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iument\Frm선택퇴원자명부.frm(Frm선택퇴원자명부.frm) >> frmPmpaViewDischargedPatient.cs 폼이름 재정의" />	
    public partial class frmPmpaViewDischargedPatient : Form
    {
        string GstrSelTemp = "";

        public frmPmpaViewDischargedPatient()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;

            try
            {
                SQL = "";
                SQL = " SELECT '1' AS GUBUN,PANO,SNAME,BI,AGE,SEX,GBSTS,IPDNO,";
                SQL = SQL + ComNum.VBLF + "        DEPTCODE,DRCODE,WARDCODE,ROOMCODE,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE =TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND (GbSPC ='1' OR GbSPC2 ='1' ) ";
                SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT '2' AS GUBUN ,PANO,SNAME,BI,AGE,SEX,GBSTS,IPDNO,";
                SQL = SQL + ComNum.VBLF + "        DEPTCODE,DRCODE,WARDCODE,ROOMCODE,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE =TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND PANO IN ( SELECT PANO FROM KOSMOS_PMPA.BAS_SELECT_MST WHERE GUBUN ='I'";
                SQL = SQL + ComNum.VBLF + "    AND (EDATE IS NULL OR EDATE =TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) AND DELDATE IS NULL GROUP BY PANO)";
                SQL = SQL + ComNum.VBLF + "  ORDER BY PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = nRead;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["SEX"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                    if (dt.Rows[i]["GBSTS"].ToString().Trim() == "9")
                    {
                        ssView_Sheet1.Cells[i, 11].Text = "취소";
                    }
                    if (dt.Rows[i]["GUBUN"].ToString().Trim() == "2")
                    {
                        ssView_Sheet1.Cells[i, 12].Text = "Y";
                    }
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewDischargedPatient_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1);
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            string strPano = "";

            strPano = ssView_Sheet1.Cells[e.Row, 0].Text;

            //GstrSelTemp = "I^^" + strPano + "^^";
            ////Frm선택진료신청new.Show 1
            //GstrSelTemp = "";
        }
    }
}
