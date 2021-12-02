using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : ComPmPaGwaSakPrintSTS.cs
    /// Description     : 진료과별 삭감율
    /// Author          : 김효성
    /// Create Date     : 2017-09-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\psmh\misu\misuta.vbp\FrmGwaSakPrint(MISUT211.FRM)  >> ComPmPaGwaSakPrintSTS.cs 폼이름 재정의" />	
    /// <seealso cref= "\psmh\misu\misupo.vbp\FrmGwaSakPrint(MISUR211.FRM)  >> ComPmPaGwaSakPrintSTS.cs 폼이름 재정의" />

    public partial class ComPmPaGwaSakPrintSTS : Form
    {
        // Flag 1. 계약처별 과별 진료비 삭감 현황
        // Flag 2. 자보 손보사별 과별 진료비 삭감 현황
        int Flag = 0;
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public ComPmPaGwaSakPrintSTS(int lag)
        {
            Flag = lag;
            InitializeComponent();
        }

        public ComPmPaGwaSakPrintSTS()
        {
            InitializeComponent();
        }

        private void ComPmPaGwaSakPrintSTS_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            nYY = (int)VB.Val(VB.Left(strDTP, 4));
            nMM = (int)VB.Val(VB.Mid(strDTP, 6, 2));

            for (i = 1; i <= 12; i++)
            {
                cboFdate.Items.Add(nYY.ToString("0000") + "-" + nMM.ToString("00"));
                cboTdate.Items.Add(nYY.ToString("0000") + "-" + nMM.ToString("00"));
                nMM = nMM - 1;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY = nYY - 1;
                }
            }
            cboFdate.SelectedIndex = 1;
            cboTdate.SelectedIndex = 1;

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "	    SELECT MiaCode GelCode, MiaName                 ";
                SQL = SQL + ComNum.VBLF + "	    FROM " + ComNum.DB_PMPA + "BAS_MIA            ";
                if (Flag == 1)
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE (MiaCode LIKE 'JK%' OR MiaCode LIKE 'KB%') ";
                }
                else if (Flag == 2)
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE MiaCode >= 'H001' AND MiaCode <= 'H999'    ";
                }
                SQL = SQL + ComNum.VBLF + "	    ORDER BY MiaCode                                ";

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

                CboGel.Items.Clear();
                CboGel.Items.Add("****.전체계약처");

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    CboGel.Items.Add(dt.Rows[i]["GelCode"].ToString().Trim() + "." + dt.Rows[i]["MiaName"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                CboGel.SelectedIndex = 0;

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

            int i = 0;
            int nRow = 0;
            double nMAmt = 0;
            double nIAmt = 0;
            double nTotMAmt = 0;
            double nTotIAmt = 0;
            double nTotSMAmt = 0;
            double nTotSSAmt = 0;
            double nSMAmt = 0;
            double nSSAmt = 0;
            double nSakRATE = 0;
            string strFYYMM = "";
            string strTYYMM = "";
            string strGelCode = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strFYYMM = VB.Left(cboFdate.Text, 4) + VB.Right(cboFdate.Text, 2);
            strTYYMM = VB.Left(cboTdate.Text, 4) + VB.Right(cboTdate.Text, 2);
            strGelCode = VB.Left(CboGel.Text, 4);

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                //' 해당월 마감여부 Checking
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Count(*) Cnt";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_MONTHLY ";
                SQL = SQL + ComNum.VBLF + "  WHERE YYMM  = '" + strTYYMM + "'     ";
                if (Flag == 1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Class = '07'                   ";
                }
                else if (Flag == 2)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Class = '08'                   ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("통계 형성중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당월의 통계형성에 실패 하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                if (Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim()) == 0)
                {
                    ComFunc.MsgBox("해당월의 통계형성에 실패 하였습니다.");
                    return;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                btnView.Enabled = false;
                btnPrint.Enabled = false;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT b.DeptCode,SUM(a.MisuAmt) MAmt,SUM(a.IpgumAmt) IAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(a.JanAmt,0,b.Amt2,0)) SMAmt,                ";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(a.JanAmt,0,b.Amt4,0)) SSAmt                 ";
                SQL = SQL + ComNum.VBLF + "   FROM MISU_MONTHLY a,MISU_IDMST b                            ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.YYMM >= '" + strFYYMM + "'                           ";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM <= '" + strTYYMM + "'                           ";
                SQL = SQL + ComNum.VBLF + "    AND a.Class = '07'                                         ";

                if (strGelCode != "****")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.GelCode = '" + strGelCode + "'                   ";
                }
                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO                                      ";

                if (Flag == 1)
                {
                    SQL = SQL + ComNum.VBLF + "    AND b.Class = '07'                                         ";
                }
                else if (Flag == 2)
                {
                    SQL = SQL + ComNum.VBLF + "    AND b.Class = '08'                                         ";
                }

                SQL = SQL + ComNum.VBLF + "  GROUP BY b.DeptCode                                          ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY b.DeptCode                                          ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                nTotSMAmt = 0;
                nTotSSAmt = 0;
                nTotMAmt = 0;
                nTotIAmt = 0;
                nRow = 0;

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nMAmt = VB.Val(dt.Rows[i]["MAmt"].ToString().Trim());
                    nIAmt = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim());
                    nSSAmt = VB.Val(dt.Rows[i]["SSAmt"].ToString().Trim());
                    nSMAmt = VB.Val(dt.Rows[i]["SMAmt"].ToString().Trim());

                    if (nMAmt != 0 || nIAmt != 0 || nSSAmt != 0)
                    {
                        nRow = nRow + 1;
                        switch (dt.Rows[i]["DEPTCODE"].ToString().Trim())
                        {

                            case "MD":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "내 과";
                                break;
                            case "GS":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "일반외과";
                                break;
                            case "CS":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "흉부외과";
                                break;
                            case "OS":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "정형외과";
                                break;
                            case "NS":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "신경외과";
                                break;
                            case "DN":
                            case "DT":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "치 과";
                                break;
                            //'~1
                            case "EM":
                            case "ER":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "응급실";
                                break;
                            //'~1
                            case "NP":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "신경정신과";
                                break;
                            case "OT":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "안 과";
                                break;
                            case "UR":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "비뇨기과";
                                break;
                            case "DM":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "피부과";
                                break;
                            case "EN":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "이비인후과";
                                break;
                            case "OB":
                            case "OG":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "산부인과";
                                break;
                            case "PD":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "소아과";
                                break;
                            case "RT":
                            case "PC":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "통증치료";
                                break;
                            case "PT":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "물리치료";
                                break;
                            default:
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "기타과";
                                break;
                        }
                        if (nSMAmt != 0 && nSSAmt != 0)
                        {
                            nSakRATE = nSSAmt / nSMAmt * 100;
                        }
                        else
                        {
                            nSakRATE = 0;
                        }

                        ssView_Sheet1.Cells[nRow - 1, 1].Text = nMAmt.ToString("###,###,###,##0 ");
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = nIAmt.ToString("###,###,###,##0 ");
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = nSSAmt.ToString("###,###,###,##0 ");
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = nSakRATE.ToString("###0.00") + "(%)";

                        nTotMAmt = nTotMAmt + nMAmt;
                        nTotIAmt = nTotIAmt + nIAmt;
                        nTotSMAmt = nTotSMAmt + nSMAmt;
                        nTotSSAmt = nTotSSAmt + nSSAmt;
                    }
                }
                dt.Dispose();
                dt = null;

                nRow = nRow + 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.Cells[nRow - 1, 0].Text = "** 합 계 **";
                nSakRATE = 0;

                if (nTotSMAmt != 0 && nTotSSAmt != 0)
                {
                    nSakRATE = nTotSSAmt / nTotSMAmt * 100;
                }

                ssView_Sheet1.Cells[nRow - 1, 1].Text = nTotMAmt.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 2].Text = nTotIAmt.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 3].Text = nTotSSAmt.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 4].Text = nSakRATE.ToString("###0.00") + "(%) ";

                btnView.Enabled = true;
                btnPrint.Enabled = true;

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
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";
            string sFont3 = "";
            string sFoot = "";

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            strFont1 = "/c/fn\"굴림체\" /fz\"15\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";


            if (Flag == 1)
            {
                strHead1 = "/c/f1" + "계약처별 과별 진료비 삭감 현황" + "/f1/n";   //제목 센터
            }
            else if (Flag == 2)
            {
                strHead1 = "/c/f1" + "자보 손보사별 과별 진료비 삭감 현황" + "/f1/n";   //제목 센터
            }
            strHead2 = "/l/f2" + "작업년월 : " + cboFdate.Text + " ~" + cboTdate.Text + "/f2";
            strHead2 = strHead2 + VB.Space(10) + "계 약 처: " + CboGel.Text;


            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Footer = sFont3 + sFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
