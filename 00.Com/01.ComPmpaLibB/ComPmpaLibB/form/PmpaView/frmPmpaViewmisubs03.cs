using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewmisubs03.cs
    /// Description     : 퇴원환자진료비정산표
    /// Author          : 김효성
    /// Create Date     : 2017-08-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "psmh\misu\misubs\misubs03.frm(FrmDailyMagan) >> frmPmpaViewmisubs03.cs 폼이름 재정의" />	

    public partial class frmPmpaViewmisubs03 : Form
    {

        double[,] FnAmt = new double[4, 13];   //'1.개인 2.종류별 3.합계
        string FstrFDate = "";      //'시작일
        string FstrTDate = "";      //'종료일

        public frmPmpaViewmisubs03(double[,] nAmt, string strFDate, string strTDate)
        {
            FnAmt = nAmt;
            FstrFDate = strFDate;
            FstrTDate = strTDate;

            InitializeComponent();
        }

        public frmPmpaViewmisubs03()
        {
            InitializeComponent();
        }

        private void frmPmpaViewmisubs03_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strDTP = "";
            strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpFdate.Value = DateTime.Parse(strDTP);
            dtpTdate.Value = DateTime.Parse(strDTP);
            btnPrint.Enabled = false;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int j = 0;
            int nRow = 0;
            string strOldData = "";
            string strNewData = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            //'Sheet Clear
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;
            ssView_Sheet1.Cells[0, ssView_Sheet1.ColumnCount - 1, 0, ssView_Sheet1.ColumnCount - 1].Text = "";

            //누적할 배열을 Clear
            for (i = 1; i <= 3; i++)
            {
                for (j = 1; j <= 12; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }

            //작업일자 SET
            FstrFDate = dtpFdate.Value.ToString("yyyy-MM-dd");
            FstrTDate = dtpTdate.Value.ToString("yyyy-MM-dd");

            try
            {
                //'자료를 SELECT
                SQL = "";
                SQL = SQL + ComNum.VBLF + "	    SELECT Bi,TO_CHAR(ActDate,'YYYYMMDD') ActDate,Pano";
                SQL = SQL + ComNum.VBLF + "		,SName,TotAmt,Junggan";
                SQL = SQL + ComNum.VBLF + "		,Johap,Halin,Bojung";
                SQL = SQL + ComNum.VBLF + "		,EtcMisu,Sunap,Dansu";
                SQL = SQL + ComNum.VBLF + "		,DeptCode, TO_CHAR(JepDate,'YYYYMMDD') JepDate,JepNo,JepJAmt";
                SQL = SQL + ComNum.VBLF + "		,Remark ";
                SQL = SQL + ComNum.VBLF + "	    FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                SQL = SQL + ComNum.VBLF + "	        WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "		AND ActDate>=TO_DATE('" + FstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "		AND ActDate<=TO_DATE('" + FstrTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "		AND Gubun = '1' ";//'퇴원자
                SQL = SQL + ComNum.VBLF + "		ORDER BY Bi,ActDate,Pano ";
                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                nRow = 0;

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

                strOldData = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = dt.Rows[i]["BI"].ToString().Trim();
                    FnAmt[1, 1] = VB.Val(dt.Rows[i]["TOTAMT"].ToString().Trim()); //총진료비
                    FnAmt[1, 2] = VB.Val(dt.Rows[i]["Junggan"].ToString().Trim());     //  '중간청구 조합부담액
                    FnAmt[1, 3] = VB.Val(dt.Rows[i]["Johap"].ToString().Trim());     //  '조합부담액
                    FnAmt[1, 4] = VB.Val(dt.Rows[i]["Halin"].ToString().Trim());     //  '감액
                    FnAmt[1, 5] = VB.Val(dt.Rows[i]["Bojung"].ToString().Trim());     //  '보증금대체
                    FnAmt[1, 6] = VB.Val(dt.Rows[i]["EtcMisu"].ToString().Trim());     //  '개인미수
                    FnAmt[1, 7] = VB.Val(dt.Rows[i]["Sunap"].ToString().Trim());     //  '퇴원수납액
                    FnAmt[1, 8] = VB.Val(dt.Rows[i]["Dansu"].ToString().Trim());     //  '10원미만 단수액
                    FnAmt[1, 11] = VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim());     // '청구접수액
                    FnAmt[1, 12] = FnAmt[1, 3] - FnAmt[1, 11];     //  '청구차액=조합부담액-청구접수액

                    if (strOldData == "")
                    {
                        strOldData = strNewData;
                    }

                    if (strOldData != strNewData)
                    {
                        CmdView_SubTotal(ref nRow, ref strOldData, ref strNewData, ref FnAmt);
                    }

                    nRow = nRow + 1;

                    if (nRow > ssView_Sheet1.Rows.Count)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["BI"].ToString();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["ActDate"].ToString();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Pano"].ToString();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SName"].ToString();

                    for (j = 1; j <= 12; j++)
                    {
                        if (!(j == 9 || j == 10))
                        {
                            ssView_Sheet1.Cells[nRow - 1, (j + 4) - 1].Text = FnAmt[1, j].ToString("###,###,###,##0");
                        }
                    }
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["JepDate"].ToString();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["JepNo"].ToString();
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = FnAmt[1, 11].ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = FnAmt[1, 12].ToString("###,###,###,##0");

                    //소계 합계에 ADD

                    for (j = 1; j <= 12; j++)
                    {
                        if (!(j == 9 || j == 10))
                        {
                            FnAmt[2, j] = FnAmt[2, j] + FnAmt[1, j];
                            FnAmt[3, j] = FnAmt[3, j] + FnAmt[1, j];
                            FnAmt[1, j] = 0;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                CmdView_SubTotal(ref nRow, ref strOldData, ref strNewData, ref FnAmt);
                //'합계를 Display
                nRow = nRow + 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 합계 **";

                for (i = 1; i <= 12; i++)
                {
                    if (!(i == 9 || i == 10))
                    {
                        ssView_Sheet1.Cells[nRow - 1, i + 3].Text = FnAmt[3, i].ToString("###,###,###,##0");
                    }
                }

                ssView_Sheet1.Cells[nRow - 1, 14].Text = FnAmt[3, 11].ToString("###,###,###,##0");
                ssView_Sheet1.Cells[nRow - 1, 15].Text = FnAmt[3, 12].ToString("###,###,###,##0");

                ssView_Sheet1.RowHeader.Cells[nRow - 1, 0].ForeColor = Color.FromArgb(0, 0, 0);
                //ssView_Sheet1.RowHeader.Cells[nRow - 1, 0].BackColor = Color.FromArgb(131, 182, 243);

                ssView.ActiveSheet.Rows[nRow - 1].BackColor = Color.FromArgb(131, 182, 243);
                Cursor.Current = Cursors.Default;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void CmdView_SubTotal(ref int nRow, ref string strOldData, ref string strNewData, ref double[,] FnAmt)//  '소계를 인쇄
        {
            int i = 0;
            int j = 0;

            nRow = nRow + 1;

            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }
            ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 소계 **";

            for (j = 1; j <= 12; j++)
            {
                if (!(j == 9 || j == 10))
                {
                    ssView_Sheet1.Cells[nRow - 1, j + 3].Text = FnAmt[2, j].ToString("###,###,###,##0");
                    FnAmt[2, j] = 0;
                }
            }

            strOldData = strNewData;
            ssView_Sheet1.RowHeader.Cells[nRow - 1, 0].ForeColor = Color.FromArgb(0, 0, 0);
           // ssView_Sheet1.RowHeader.Cells[nRow - 1, 0].BackColor = Color.FromArgb(131, 182, 243);

            ssView.ActiveSheet.Rows[nRow - 1].BackColor = Color.FromArgb(131, 182, 243);

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int j = 0;
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";


            ssView_Sheet1.Columns[12].Visible = true;   //접수일자
            ssView_Sheet1.Columns[13].Visible = true;   //접수번호
            ssView_Sheet1.Columns[15].Visible = true;   //청구차액

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fs2";

            strHead1 = "/c/f1" + " 퇴원환자 진료비 정산표" + "/n/n";
            strHead2 = "/f2" + "작업기간 : " + dtpFdate.Value.ToString("yyyy-MM-dd") + "부터 " + dtpTdate.Value.ToString("yyyy-MM-dd") + "까지";

            strHead2 = strHead2 + VB.Space(20) + "인쇄일자: " + mdtp;

            btnPrint.Enabled = false;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;


            ssView_Sheet1.Columns[12].Visible = false;   //접수일자
            ssView_Sheet1.Columns[13].Visible = false;   //접수번호
            ssView_Sheet1.Columns[15].Visible = false;   //청구차액

            Cursor.Current = Cursors.Default;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
