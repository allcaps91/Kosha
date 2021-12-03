using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
/// <summary>
/// Class Name      : ComPmpaLibB
/// File Name       : 
/// Description     : 
/// Author          : 김효성
/// Create Date     : 2017-09-29
/// Update History  : 
/// </summary>
/// <history>  
/// 
/// </history>
/// <seealso cref =D:\psmh\IPD\iument\iument.vbp\Frm일일수술명단.frm(일일수술센터명단조회)" >> frmSupLbExSTS15.cs 폼이름 재정의" />
{
    public partial class frmPmpaViewTodayOperlationSearch : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewTodayOperlationSearch()
        {
            InitializeComponent();
        }

        private void frmPmpaViewTodayOperlationSearch_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFDate.Value = Convert.ToDateTime(strDTP);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE,WARDCODE,ROOMCODE,GBSTS, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(INDATE,'YYYY-MM-DD') INDATE,TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE   ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE ( ActDate IS NULL OR ActDate=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ) ";
                SQL = SQL + ComNum.VBLF + "   AND GBSTS <> '9' "; //  '취소제외
                SQL = SQL + ComNum.VBLF + "   AND GBSUDAY ='Y' ";// '일일수술센터

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "IPD_입원상태", dt.Rows[i]["GBSTS"].ToString().Trim());

                    if (VB.DateDiff("d", dtpFDate.Value, Convert.ToDateTime(dt.Rows[i]["INDATE"].ToString().Trim())) > 0)
                    {
                        ssView_Sheet1.Cells[i, 7].Text = "일수확인";
                    }
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "일일 수술 센터 명단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회일자 : " + dtpFDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
