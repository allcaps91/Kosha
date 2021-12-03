using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;


namespace ComPmpaLibB
{
    public partial class frmPmpaViewEmergencyNHTList : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : 
        /// Description     : 
        /// Author          : 김효성
        /// Create Date     : 2017-09-14
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= d:\psmh\IPD\iument\iument.vbp\Frm응급실내원입원명단.frm(응급실내원입원환자명단.frm)" >> frmSupLbExSTS15.cs 폼이름 재정의" />

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewEmergencyNHTList()
        {
            InitializeComponent();
        }

        private void frmPmpaViewEmergencyNHTList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFDate.Value = Convert.ToDateTime(strDTP);
            dtpTDate.Value = Convert.ToDateTime(strDTP);

            lblcnt.Text = "";
            lblcnt2.Text = "";
            lblcnt3.Text = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }


            int i = 0;
            int nRead = 0;
            int nErCnt = 0;
            string strFDate = "";
            string strTDate = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Value.AddDays(1).ToString().Trim(); //.Text) , 1)

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT * FROM " + ComNum.DB_PMPA + "BBASCD";  //BAS_BASCD

                SQL = "";
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR (INDATE,'YYYY-MM-DD') INDATE, ";
                SQL = SQL + ComNum.VBLF + " PANO, SNAME, DEPTCODE, WARDCODE, ROOMCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE < TO_DATE('" + Convert.ToDateTime(strTDate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND GBSTS <> '9' ";
                SQL = SQL + ComNum.VBLF + "   AND AMSET7 IN ('3','4','5') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY INDATE, PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                nRead = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = " ";
                }
                dt.Dispose();
                dt = null;

                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "COUNT(PANO) CNT FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = 'ER' ";

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                nErCnt = (int)VB.Val(dt.Rows[0]["CNT"].ToString().Trim());

                dt.Dispose();
                dt = null;

                lblcnt.Text = nErCnt.ToString();
                lblcnt2.Text = nRead.ToString();
                lblcnt3.Text = (nRead / nErCnt * 100).ToString("###");

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "응급실내원(입원명단)";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회일자 : " + dtpFDate.Text + " ~ " + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
        }
    }
}
