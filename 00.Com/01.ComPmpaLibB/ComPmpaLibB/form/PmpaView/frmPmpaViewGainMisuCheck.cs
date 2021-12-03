using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Drawing;
using FarPoint.Win.Spread;
namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewGainMisuCheck2.cs
    /// Description     : 월별 기타 미수금 점검표
    /// Author          : 박창욱
    /// Create Date     : 2017-08-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs41.frm(FrmGainMisuCheck.frm) >> frmPmpaViewGainMisuCheck.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGainMisuCheck : Form
    {
        double[,] FnAmt = new double[11, 11];
        double[,] FnAmt2 = new double[11, 11];
        private string GstrHelpCode = "";

        public frmPmpaViewGainMisuCheck()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            int i = 0;
            int k = 0;
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            clsSpread CS = new clsSpread();
            Cursor.Current = Cursors.WaitCursor;
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            //Print Head
            if (!(string.Compare(ComFunc.LeftH(cboYYMM.Text, 4) + ComFunc.MidH(cboYYMM.Text, 8, 2), "200701") < 0))
            {
                //strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
                //strFont2 = "/fn\"굴림체\" /fz\"11\" /fs2";
                //strHead1 = "/f1/c";
                //strHead1 = strHead1 + "월별 기타 미수금 청구액 점검표";
                //strHead2 = "/f2/n";
                //strHead2 += "/f2/n";
                //strHead2 = strHead2 + "진 료 월: " + cboYYMM.Text + "/n";
                //strHead2 = strHead2 + "인쇄일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon,"A"), "A");

                for (i = 0; i < ssView2_Sheet1.RowCount; i++)
                {
                    for (k = 0; k < ssView2_Sheet1.ColumnCount; k++)
                    {
                        ssPrint_Sheet1.Cells[i + 6, k].Text = ssView2_Sheet1.Cells[i, k].Text;
                    }
                }


                strTitle = "월별 기타 미수금 청구액 점검표";

                ssPrint_Sheet1.Cells[2, 0].Text = "진 료 월: " + VB.Left(cboYYMM.Text, 4) + "년 " + VB.Mid(cboYYMM.Text, 6, 2) + "월분";
                ssPrint_Sheet1.Cells[3, 0].Text = "출력일자:" + VB.Now().ToString();

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, false, false, false, false, false);

                CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);


                //ssPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
                //ssPrint_Sheet1.PrintInfo.Margin.Left = 0;
                //ssPrint_Sheet1.PrintInfo.Margin.Right = 0;
                //ssPrint_Sheet1.PrintInfo.Margin.Top = 60;
                //ssPrint_Sheet1.PrintInfo.Margin.Bottom = 100;
                //ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                //ssPrint_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                //ssPrint_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                //ssPrint_Sheet1.PrintInfo.ShowBorder = false;
                //ssPrint_Sheet1.PrintInfo.ShowColor = true;
                //ssPrint_Sheet1.PrintInfo.ShowGrid = false;
                //ssPrint_Sheet1.PrintInfo.ShowShadows = false;
                //ssPrint_Sheet1.PrintInfo.UseMax = false;
                //ssPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
                //ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                //ssPrint.PrintSheet(0);
            }
            else if (!(string.Compare(ComFunc.LeftH(cboYYMM.Text, 4) + ComFunc.MidH(cboYYMM.Text, 8, 2), "200501") < 0))
            {
                strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
                strFont2 = "/fn\"굴림체\" /fz\"11\" /fs2";
                strHead1 = "/f1/c";
                strHead1 = strHead1 + "월별 기타 미수금 청구액 점검표";
                strHead2 = "/f2" + "진료월: " + VB.Left(cboYYMM.Text, 4) + "년 " + VB.Mid(cboYYMM.Text, 6, 2) + "월분";
                strHead2 = strHead2 + VB.Space(65) + "인쇄일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon,"A"), "A");

                ssView2_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;
                ssView2_Sheet1.PrintInfo.Margin.Left = 0;
                ssView2_Sheet1.PrintInfo.Margin.Right = 0;
                ssView2_Sheet1.PrintInfo.Margin.Top = 60;
                ssView2_Sheet1.PrintInfo.Margin.Bottom = 100;
                ssView2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ssView2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ssView2_Sheet1.PrintInfo.ShowBorder = true;
                ssView2_Sheet1.PrintInfo.ShowColor = true;
                ssView2_Sheet1.PrintInfo.ShowGrid = false;
                ssView2_Sheet1.PrintInfo.ShowShadows = false;
                ssView2_Sheet1.PrintInfo.UseMax = false;
                ssView2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
                ssView2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ssView2.PrintSheet(0);
            }
            else
            {
                strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
                strFont2 = "/fn\"굴림체\" /fz\"11\" /fs2";
                strHead1 = "/f1/c";
                strHead1 = strHead1 + "월별 기타 미수금 점검표";
                strHead2 = "/f2" + "작업월: " + cboYYMM.Text;
                strHead2 = strHead2 + VB.Space(10) + "인쇄일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon,"A"), "A");

                ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;
                ssView_Sheet1.PrintInfo.Margin.Left = 0;
                ssView_Sheet1.PrintInfo.Margin.Right = 0;
                ssView_Sheet1.PrintInfo.Margin.Top = 60;
                ssView_Sheet1.PrintInfo.Margin.Bottom = 100;
                ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ssView_Sheet1.PrintInfo.ShowBorder = true;
                ssView_Sheet1.PrintInfo.ShowColor = true;
                ssView_Sheet1.PrintInfo.ShowGrid = false;
                ssView_Sheet1.PrintInfo.ShowShadows = false;
                ssView_Sheet1.PrintInfo.UseMax = false;
                ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
                ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ssView.PrintSheet(0);
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            int j = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";
            double nAmt = 0;

            //누적할 배열을 Clear
            for (i = 1; i < 11; i++)
            {
                for (j = 1; j < 11; j++)
                {
                    FnAmt[i, j] = 0;
                    FnAmt2[i, j] = 0;
                }
            }

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Right(cboYYMM.Text,  2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));

            View_BalEtcMisu_ADD(strFDate, strTDate, ref nAmt);  //당월발생 기타미수(MISU_BALETCMISU)
            View_GainMisu_ADD(strFDate, strTDate, ref nAmt);    //당월 개인미수 합산
            View_MisuIdMst_ADD(strYYMM, ref nAmt);   //당월 기타미수 발생액 합산
            View_MisuSlip_ADD(strFDate, strTDate, ref nAmt);    //당월 기타미수 입금, 삭감액 합산

            if (!(string.Compare(strYYMM, "200501") < 0))
            {
                ssView.Visible = false;
                ssView2.Visible = true;
            }
            else
            {
                ssView.Visible = true;
                ssView2.Visible = false;
            }

            //내용을 Sheet에 Display
            //1.외래발생, 2.입원발생, 3.소계,4.미수발생(외래), 5.발생차액(외래), 6.미수발생(입원), 7.발생차액
            if (!(string.Compare(strYYMM, "200501") < 0))
            {
                for (i = 1; i < 11; i++)
                {
                    FnAmt2[i, 1] = FnAmt[i, 1];
                    FnAmt2[i, 2] = FnAmt[i, 2];
                    FnAmt2[i, 3] = FnAmt[i, 3];

                    FnAmt2[i, 4] = FnAmt[i, 4];

                    FnAmt2[i, 5] = FnAmt[i, 6];
                    FnAmt2[i, 6] = FnAmt2[i, 4] + FnAmt2[i, 5];
                    FnAmt2[i, 7] = FnAmt[i, 4] - FnAmt[i, 1];
                    FnAmt2[i, 8] = FnAmt[i, 6] - FnAmt[i, 2];
                    FnAmt2[i, 9] = FnAmt2[i, 7] + FnAmt2[i, 8];
                    for (j = 1; j < 10; j++)
                    {
                        ssView2_Sheet1.Cells[i - 1, j + 1].Text = FnAmt2[i, j].ToString("###,###,###,### ");
                    }
                }
            }
            else
            {
                for (i = 1; i < 11; i++)
                {
                    FnAmt[i, 5] = FnAmt[i, 4] - FnAmt[i, 1];
                    FnAmt[i, 7] = FnAmt[i, 6] - FnAmt[i, 2];

                    for (j = 1; j < 11; j++)
                    {
                        ssView_Sheet1.Cells[i - 1, j + 1].Text = FnAmt[i, j].ToString("###,###,###,### ");
                    }
                }
            }
        }

        //당월 기타미수 발생액
        void View_BalEtcMisu_ADD(string strFDate, string strTDate, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT IpdOpd,MisuGye,SUM(MisuAmt) Amt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALETCMISU ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY IpdOpd,MisuGye ";
                SQL = SQL + ComNum.VBLF + "ORDER BY IpdOpd,MisuGye ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToDateTime(strFDate) >= Convert.ToDateTime("2016-12-01"))
                    {
                        switch (dt.Rows[i]["MisuGye"].ToString().Trim())
                        {
                            case "Y96":
                            case "Y96B":
                                j = 1;
                                break;
                            case "Y96H":
                                j = 2;
                                break;
                            case "Y96J":
                                j = 3;
                                break;
                            case "Y96M":
                                j = 3;
                                break;
                            case "Y96K":
                                j = 4;
                                break;
                            case "Y96S":
                                j = 5;
                                break;
                            case "Y96A":
                            case "Y96C":
                                j = 7;
                                break;
                            case "Y96F":
                                j = 8;
                                break;
                            case "Y96Y":
                                j = 9;
                                break;
                            default:
                                j = 6;
                                break;
                        }
                    }
                    else
                    {
                        switch (dt.Rows[i]["MisuGye"].ToString().Trim())
                        {
                            case "Y96":
                            case "Y96B":
                                j = 1;
                                break;
                            case "Y96H":
                                j = 2;
                                break;
                            case "Y96J":
                                j = 3;
                                break;
                            case "Y96M":
                                j = 3;
                                break;
                            case "Y96K":
                                j = 4;
                                break;
                            case "Y96S":
                                j = 5;
                                break;
                            case "Y96C":
                                j = 7;
                                break;
                            case "Y96F":
                                j = 8;
                                break;
                            case "Y96Y":
                                j = 9;
                                break;
                            default:
                                j = 6;
                                break;
                        }
                    }
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "O")
                    {
                        FnAmt[j, 1] = FnAmt[j, 1] + nAmt;
                        FnAmt[j, 3] = FnAmt[j, 3] + nAmt;
                        FnAmt[10, 1] = FnAmt[10, 1] + nAmt;
                        FnAmt[10, 3] = FnAmt[10, 3] + nAmt;
                    }
                    else
                    {
                        FnAmt[j, 2] = FnAmt[j, 2] + nAmt;
                        FnAmt[j, 3] = FnAmt[j, 3] + nAmt;
                        FnAmt[10, 2] = FnAmt[10, 2] + nAmt;
                        FnAmt[10, 3] = FnAmt[10, 3] + nAmt;
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

        //당월 개인미수 합산
        void View_GainMisu_ADD(string strFDate, string strTDate, ref double nAmt)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT Gubun1,SUBSTR(MISUDTL,1,1) IO,SUM(Amt) Amt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND BDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY Gubun1, SUBSTR(MISUDTL,1,1) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    if (dt.Rows[i]["Gubun1"].ToString().Trim() == "1")  //미수발생
                    {
                        if (dt.Rows[i]["IO"].ToString().Trim() == "I")  //입원
                        {
                            FnAmt[1, 6] = FnAmt[1, 6] + nAmt;
                            FnAmt[10, 6] = FnAmt[10, 6] + nAmt;
                        }
                        else
                        {
                            FnAmt[1, 4] = FnAmt[1, 4] + nAmt;
                            FnAmt[10, 4] = FnAmt[10, 4] + nAmt;
                        }
                    }
                    else if (dt.Rows[i]["Gubun1"].ToString().Trim() == "2") //입금
                    {
                        FnAmt[1, 8] = FnAmt[1, 8] + nAmt;
                        FnAmt[10, 8] = FnAmt[10, 8] + nAmt;
                    }
                    else if (dt.Rows[i]["Gubun1"].ToString().Trim() == "5") //삭감
                    {
                        FnAmt[1, 9] = FnAmt[1, 9] + nAmt;
                        FnAmt[10, 9] = FnAmt[10, 9] + nAmt;
                    }
                    else if (dt.Rows[i]["Gubun1"].ToString().Trim() == "4" || dt.Rows[i]["Gubun1"].ToString().Trim() == "3") //감액 반송
                    {
                        FnAmt[1, 10] = FnAmt[1, 10] + nAmt;
                        FnAmt[10, 10] = FnAmt[10, 10] + nAmt;
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

        //당월 기타미수 발생액 합산
        void View_MisuIdMst_ADD(string strYYMM, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT Class,IPDOPD, SUM(Amt2) Amt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND MirYYMM='" + strYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "  AND Class >= '08' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY Class, IPDOPD ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["Class"].ToString().Trim())
                    {
                        case "11":
                            j = 2;
                            break;  //보훈청미수
                        case "10":
                            j = 3;
                            break;  //산재공상계약서
                        case "08":
                            j = 3;
                            break;  //계약서
                        case "13":
                            j = 4;
                            break;  //심신장애진단비
                        case "09":
                            j = 5;
                            break;  //헌혈미수
                        case "16":
                            j = 7;
                            break;  //노인장기요양소견서
                        case "17":
                            j = 8;
                            break;  //가정방문소견서
                        case "18":
                            j = 9;
                            break;  //치매검사
                        default:
                            j = 6;
                            break;  //시각장애자,장애인보장구
                    }
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    if (dt.Rows[i]["IPDOPD"].ToString().Trim() == "I")  //입원
                    {
                        FnAmt[j, 6] = FnAmt[j, 6] + nAmt;
                        FnAmt[10, 6] = FnAmt[10, 6] + nAmt;
                    }
                    else
                    {
                        FnAmt[j, 4] = FnAmt[j, 4] + nAmt;
                        FnAmt[10, 4] = FnAmt[10, 4] + nAmt;
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

        //당월 기타미수 발생액
        void View_MisuSlip_ADD(string strFDate, string strTDate, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT Class,Gubun,SUM(Amt) Amt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_SLIP ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND BDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND Class >= '08' ";
                SQL = SQL + ComNum.VBLF + "  AND Gubun >= '21' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY Class,Gubun ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["Class"].ToString().Trim())
                    {
                        case "11":
                            j = 2;
                            break;  //보훈청미수
                        case "10":
                            j = 3;
                            break;  //산재공상계약서
                        case "08":
                            j = 3;
                            break;  //계약서
                        case "13":
                            j = 4;
                            break;  //심신장애진단비
                        case "09":
                            j = 5;
                            break;  //헌혈미수
                        case "16":
                            j = 7;
                            break;  //장기요양
                        case "17":
                            j = 8;
                            break;  //가정방문소견서
                        case "18":
                            j = 9;
                            break;  //치매검사
                        default:
                            j = 6;
                            break;  //계약처,시각장애자,장애인보장구
                    }
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    switch (dt.Rows[i]["Gubun"].ToString().Trim())
                    {
                        case "21":
                        case "22":
                        case "23":
                        case "24":
                        case "25":
                        case "26":
                        case "27":
                        case "28":
                        case "29":  //당월 입금액
                            FnAmt[j, 8] += nAmt;
                            FnAmt[10, 8] += nAmt;
                            break;
                        case "31":  //당월 삭감액
                            FnAmt[j, 9] += nAmt;
                            FnAmt[10, 9] += nAmt;
                            break;
                        case "32":
                        case "33":
                        case "34":
                        case "35":
                        case "36":
                        case "37":
                        case "38":
                        case "39":  //기타입금
                            FnAmt[j, 10] += nAmt;
                            FnAmt[10, 10] += nAmt;
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

        private void cboYYMM_SelectedIndexChanged(object sender, EventArgs e)
        {
            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            ssView2_Sheet1.Cells[0, 1, ssView2_Sheet1.RowCount - 1, ssView2_Sheet1.ColumnCount - 1].Text = "";
        }

        private void frmPmpaViewGainMisuCheck_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView.Dock = DockStyle.Fill;
            ssView2.Dock = DockStyle.Fill;

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "0");
            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }
        }

        private void ssView2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView2_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            //ssView2_Sheet1.Cells[0, 0, ssView2_Sheet1.RowCount - 1, ssView2_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            //ssView2_Sheet1.Cells[e.Row, 0, e.Row, ssView2_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            string strYYMM = "";
            if (e.Row == -1 || e.Column == -1 || e.Row == 9)
            {
                return;
            }
            if (e.Column != 8 && e.Column != 9)
            {
                return;
            }
            strYYMM = ComFunc.LeftH(cboYYMM.Text, 4) + ComFunc.MidH(cboYYMM.Text, 6, 2);

            GstrHelpCode = strYYMM + "/" + (e.Row+1);

            switch (e.Row)
            {
                case 0:
                    GstrHelpCode += "/개인미수";
                    break;
                case 1:
                    GstrHelpCode += "/보훈청미수";
                    break;
                case 2:
                    GstrHelpCode += "/계약처";
                    break;
                case 3:
                    GstrHelpCode += "/심신장애진단비";
                    break;
                case 4:
                    GstrHelpCode += "/헌혈미수";
                    break;
                case 5:
                    GstrHelpCode += "/기타미수";
                    break;
                case 6:
                    GstrHelpCode += "/장기요양";
                    break;
                case 7:
                    GstrHelpCode += "/가정간호소견";
                    break;
                case 8:
                    GstrHelpCode += "/치매검사";
                    break;
            }

            GstrHelpCode += "/" + (e.Column == 8 ? "O" : "I");

            frmPmpaViewGainMisuCheck2 frm = new frmPmpaViewGainMisuCheck2(GstrHelpCode);
            frm.ShowDialog();
        }

    }
}
