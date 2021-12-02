using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaViewDischargeSTS : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmpaViewDischargeSTS.cs
        /// Description     : 퇴원 예고 및 퇴원 과별 통계
        /// Author          : 김효성
        /// Create Date     : 2017-08-13
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "\IPD\ilrepb\ilrepb.vbp(Frmc퇴원예고및퇴원과별통계) >> frmPmpaViewDischargeSTS.cs 폼이름 재정의" />	


        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewDischargeSTS()
        {
            InitializeComponent();
        }

        private void frmPmpaViewDischargeSTS_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            dtpFdate.Value = DateTime.Parse(VB.Left(strDTP, 8) + "01");
            dtpTdate.Value = DateTime.Parse(strDTP);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "	SELECT DEPTCODE, PRINTRANKING ";
                SQL = SQL + ComNum.VBLF + "	  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "	  WHERE DEPTCODE NOT IN ('II','R6','HC','OM','LM','PT','HD','PC','OC','HR','AN')";
                SQL = SQL + ComNum.VBLF + "	ORDER BY PRINTRANKING";

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
                ssView_Sheet1.ColumnCount = 10;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.ColumnCount = ssView_Sheet1.ColumnCount + 1;
                    ssView_Sheet1.SetColumnWidth(ssView_Sheet1.ColumnCount - 1, 30);
                    ssView_Sheet1.Columns[ssView_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    ssView_Sheet1.Columns[ssView_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    ssView_Sheet1.Cells[0, ssView_Sheet1.ColumnCount - 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.ColumnCount = ssView_Sheet1.ColumnCount + 1;
                ssView_Sheet1.SetColumnWidth(ssView_Sheet1.ColumnCount - 1, 50);
                ssView_Sheet1.Columns[ssView_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssView_Sheet1.Columns[ssView_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssView_Sheet1.Cells[0, ssView_Sheet1.ColumnCount - 1].Text = "합   계";

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
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int[] nOutCnt = new int[12];
            int i = 0;
            int k = 0;
            int nRead = 0;
            int nFDD = 0;
            int nTDD = 0;
            double nDept_Tot = 0;
            double nSe_tot = 0;
            double nSe_Sum = 0;
            string strDate = "";
            string strDept = "";
            string strFDate = "";
            string strTdate = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //'noutcnt(1-7): 1.퇴원등록 2.8시이전퇴원등록 3.8시이후퇴원등록 4.사망자
            //'5.자의퇴원 6.12시이전퇴원완료자 7.12시이후퇴원완료자


            Cursor.Current = Cursors.WaitCursor;


            try
            {

                for (i = 1; i <= 11; i++)
                {
                    nOutCnt[i] = 0;
                }

                nFDD = int.Parse(VB.Right(dtpFdate.Value.ToString("yyyy-MM-dd"), 2));
                nTDD = int.Parse(VB.Right(dtpTdate.Value.ToString("yyyy-MM-dd"), 2));
                ssView_Sheet1.RowCount = 1;

                for (i = nFDD; i <= nTDD; i++)
                {
                    strDate = VB.Left(dtpFdate.Value.ToString("yyyy-MM-dd"), 8) + VB.Format(i, "00");

                    for (k = 1; k <= 11; k++)
                    {
                        nOutCnt[k] = 0;
                    }

                    //'8시이전
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT COUNT(PANO) CNT ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE OUTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "   AND GBSTS = '7' ";
                    SQL = SQL + ComNum.VBLF + "   AND GBIPD = '1' ";
                    SQL = SQL + ComNum.VBLF + "   AND AMSET5 IN ('2','3') ";
                    SQL = SQL + ComNum.VBLF + "   AND TO_CHAR(ROUTDATE,'HH24:MI')>='00:00' AND TO_CHAR(ROUTDATE,'HH24:MI')<='08:00'";

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

                    nOutCnt[8] = int.Parse(dt.Rows[0]["CNT"].ToString().Trim());
                    dt.Dispose();
                    dt = null;

                    //  '8시이후
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT COUNT(PANO) CNT ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE OUTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "   AND GBSTS = '7' ";
                    SQL = SQL + ComNum.VBLF + "   AND GBIPD = '1' ";
                    SQL = SQL + ComNum.VBLF + "   AND AMSET5 IN ('2','3') ";
                    SQL = SQL + ComNum.VBLF + "   AND TO_CHAR(ROUTDATE,'HH24:MI')>='08:01' AND TO_CHAR(ROUTDATE,'HH24:MI')<='23:59'";

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

                    nOutCnt[9] = int.Parse(dt.Rows[0]["CNT"].ToString().Trim());
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT COUNT(CASE WHEN TO_CHAR(ROUTDATE,'HH24:MI') >= '00:00' AND TO_CHAR(ROUTDATE,'HH24:MI') <= '08:00' THEN PANO END) CNT2, ";
                    SQL = SQL + ComNum.VBLF + " COUNT(CASE WHEN TO_CHAR(ROUTDATE,'HH24:MI') >= '08:01' AND TO_CHAR(ROUTDATE,'HH24:MI') <= '23:59' THEN PANO END) CNT3 ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE OUTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND ACTDATE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "    AND GBSTS = '7' ";
                    SQL = SQL + ComNum.VBLF + "    AND GBIPD = '1' ";
                    SQL = SQL + ComNum.VBLF + "    AND AMSET5 IN ('1')";

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

                    nOutCnt[2] = int.Parse(dt.Rows[0]["CNT2"].ToString().Trim());
                    nOutCnt[3] = int.Parse(dt.Rows[0]["CNT3"].ToString().Trim());
                    nOutCnt[1] = nOutCnt[2] + nOutCnt[3];
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT AMSET5, COUNT(PANO) CNT ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE OUTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND AMSET5 IN ('2','3')";
                    SQL = SQL + ComNum.VBLF + "    AND ACTDATE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "    AND GBSTS = '7' ";
                    SQL = SQL + ComNum.VBLF + "    AND GBIPD = '1' ";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY AMSET5 ";

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


                    for (k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["AMSET5"].ToString().Trim() == "2")    //자의퇴원
                        {
                            nOutCnt[5] = int.Parse(dt.Rows[k]["CNT"].ToString().Trim());
                        }
                        else if (dt.Rows[k]["AMSET5"].ToString().Trim() == "3")   //사망
                        {
                            nOutCnt[4] = int.Parse(dt.Rows[k]["CNT"].ToString().Trim());
                        }

                    }
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT COUNT(CASE WHEN TO_CHAR(SUNAPTIME,'HH24:MI') >= '00:00' AND TO_CHAR(SUNAPTIME,'HH24:MI') <= '12:00' THEN PANO END) CNT6, ";
                    SQL = SQL + ComNum.VBLF + " COUNT(CASE WHEN TO_CHAR(SUNAPTIME,'HH24:MI') >= '12:01' AND TO_CHAR(SUNAPTIME,'HH24:MI') <= '23:59' THEN PANO END) CNT7 ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE OUTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND ACTDATE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "    AND GBSTS = '7' ";
                    SQL = SQL + ComNum.VBLF + "    AND GBIPD = '1' ";
                    SQL = SQL + ComNum.VBLF + "    AND AMSET5 NOT IN ('8') ";

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

                    nOutCnt[6] = int.Parse(dt.Rows[0]["CNT6"].ToString().Trim());
                    nOutCnt[7] = int.Parse(dt.Rows[0]["CNT7"].ToString().Trim());

                    dt.Dispose();
                    dt = null;

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strDate;

                    nOutCnt[2] = nOutCnt[2];  //- nOutCnt(8)   '8시이전 퇴원 - 자의
                    nOutCnt[3] = nOutCnt[3];  //- nOutCnt(9)   '8시이후 퇴원 - 사망
                    nOutCnt[1] = nOutCnt[1];  //- nOutCnt(4) - nOutCnt(5)

                    if (nOutCnt[1] < 0)
                    {
                        nOutCnt[1] = 0;
                    }

                    for (k = 1; k <= 5; k++)
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Text = VB.IIf(nOutCnt[k] == 0, "", nOutCnt[k]).ToString();

                        if (k == 1 && nOutCnt[1] == 0)
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Text = 0.ToString();
                        }

                        if (k == 4 || k == 5)
                        {
                            //'If j <> 1 Then SS1.Text = SS1.Text & "(" & IIf(nOutCnt(j) = 0, "", Format(nOutCnt(j) / nOutCnt(1) * 100, "###")) & "%)"
                        }
                        else
                        {
                            if (k != 1)
                            {
                                if (nOutCnt[1] > 0)
                                {
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Text + "(" + VB.IIf(nOutCnt[k] == 0, "", (Convert.ToDouble(nOutCnt[k]) / nOutCnt[1] * 100).ToString("###")) + "%)";

                                    if (ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Text == "(%)")
                                    {
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Text = "";
                                    }
                                }
                            }

                        }
                    }

                    //전체인원
                    nOutCnt[10] = 0;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = (nOutCnt[6] + nOutCnt[7]).ToString();
                    nOutCnt[10] = (nOutCnt[6] + nOutCnt[7]);

                    for (k = 6; k <= 7; k++)
                    {
                        nOutCnt[10] = (nOutCnt[10] == 0 ? 1 : nOutCnt[10]);

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Text = VB.IIf(nOutCnt[k] == 0, "", nOutCnt[k]).ToString();

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Text + "(" + VB.IIf(nOutCnt[k] == 0, "", (Convert.ToDouble(nOutCnt[k]) / nOutCnt[10] * 100).ToString("###")) + "%)";
                        if (ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Text == "(%)")
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Text = "";
                        }
                    }

                    nDept_Tot = 0;
                    for (k = 11; k < ssView_Sheet1.ColumnCount; k++)
                    {
                        strDept = ssView_Sheet1.Cells[0, k - 1].Text;

                        SQL = SQL + ComNum.VBLF + " SELECT COUNT(DEPTCODE) CNT FROM IPD_TRANS ";
                        SQL = SQL + ComNum.VBLF + " WHERE OUTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDept + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GBSTS  = '7' ";
                        SQL = SQL + ComNum.VBLF + "   AND GBIPD = '1' ";

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

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k - 1].Text = VB.IIf(int.Parse(dt.Rows[0]["CNT"].ToString().Trim()) == 0, "", dt.Rows[0]["CNT"].ToString().Trim()).ToString();
                        nDept_Tot = nDept_Tot + double.Parse(dt.Rows[0]["CNT"].ToString().Trim());

                        dt.Dispose();
                        dt = null;
                    }
                    ssView_Sheet1.Cells[0, ssView_Sheet1.ColumnCount - 1].Text = "합  계";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = VB.IIf(nDept_Tot == 0, "", nDept_Tot).ToString();
                }

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "합 계";

                //세로 합계
                nSe_tot = 0;
                for (i = 2; i <= 6; i++)
                {
                    for (k = 2; k < ssView_Sheet1.RowCount; k++)
                    {
                        nSe_tot = nSe_tot + VB.Val(ssView_Sheet1.Cells[k - 1, i - 1].Text);
                    }
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, i - 1].Text = VB.IIf(nSe_tot == 0, "", nSe_tot).ToString();
                    nSe_tot = 0;
                }

                nSe_tot = 0;
                nSe_Sum = VB.Val(ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text);

                for (i = 3; i <= 8; i++)
                {
                    if (i != 5 && i != 6)
                    {
                        for (k = 2; k < ssView_Sheet1.RowCount; k++)
                        {
                            nSe_tot = nSe_tot + VB.Val(ssView_Sheet1.Cells[k - 1, i - 1].Text);
                        }
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, i - 1].Text = VB.IIf(nSe_tot == 0, "", nSe_tot).ToString() + "(" + (nSe_tot / nSe_Sum * 100).ToString("###") + "%)";
                        nSe_tot = 0;
                    }
                }

                nSe_tot = 0;
                for (i = 11; i <= ssView_Sheet1.ColumnCount; i++)
                {
                    for (k = 2; k < ssView_Sheet1.RowCount; k++)
                    {
                        nSe_tot = nSe_tot + VB.Val(ssView_Sheet1.Cells[k - 1, i - 1].Text);
                    }
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, i - 1].Text = VB.IIf(nSe_tot == 0, "", nSe_tot).ToString();
                    nSe_tot = 0;
                }
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
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";
            string strFont3 = "";
            string sFont3 = "";
            string sFoot = "";

            strFont1 = "/fn\"굴림체\" /fz\"18\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "퇴원예고 및 과별 통계" + "/f1/n";   //제목 센터
            strHead2 = "/l/f2" + "퇴원일자: " + (dtpFdate.Value).ToString("yyyy-MM-dd") + " ~" + (dtpTdate.Value).ToString("yyyy-MM-dd") + "/f2";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.78f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Footer = sFont3 + sFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
