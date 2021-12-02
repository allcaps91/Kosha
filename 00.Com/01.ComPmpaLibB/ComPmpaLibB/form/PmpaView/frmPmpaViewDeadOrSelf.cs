using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewSakBanCheck.cs
    /// Description     : 사망 및 자의퇴원환자 명단
    /// Author          : 박창욱
    /// Create Date     : 2017-09-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\ilrepb\Frm사망및자의환자명단.frm(Frm사망및자의환자명단.frm) >> frmPmpaViewDeadOrSelf.cs 폼이름 재정의" />	
    public partial class frmPmpaViewDeadOrSelf : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        public frmPmpaViewDeadOrSelf()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "사망 및 자의 퇴원환자 명단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("퇴원일자 : " + dtpFDate.Value + " ~ " + dtpTDate.Value, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strFDate = "";
            string strTDate = "";

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.PANO, A.SNAME, C.JUMIN1 || C.JUMIN2 JUMIN,";
                SQL = SQL + ComNum.VBLF + "       A.DEPTCODE, D.DRNAME, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, B.AMSET5 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_PATIENT C, ";
                SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "IPD_TRANS B, " + ComNum.DB_PMPA + "BAS_DOCTOR D ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = C.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = D.DRCODE ";
                SQL = SQL + ComNum.VBLF + "   AND A.LASTTRS = B.TRSNO ";
                SQL = SQL + ComNum.VBLF + "   AND B.AMSET5  IN ('2','3') ";
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY OUTDATE, PANO ";

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

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JUMIN"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    switch (dt.Rows[i]["AMSET5"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[i, 7].Text = "완쾌";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 7].Text = "자의";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 7].Text = "사망";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[i, 7].Text = "전원";
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 7].Text = "";
                            break;
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

        private void frmPmpaViewDeadOrSelf_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpFDate.Value = Convert.ToDateTime(VB.Left(strSysDate, 8) + "01");
            dtpTDate.Value = Convert.ToDateTime(strSysDate);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
