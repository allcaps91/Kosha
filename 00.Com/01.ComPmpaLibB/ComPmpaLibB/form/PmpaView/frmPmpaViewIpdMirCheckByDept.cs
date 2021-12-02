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
    /// File Name       : frmPmpaViewIpdMirCheckByDept.cs
    /// Description     : 과별 월별 입원 청구액 점검표
    /// Author          : 박창욱
    /// Create Date     : 2017-10-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs51_1.frm(FrmTongIpdMirCheck_1.frm) >> frmPmpaViewIpdMirCheckByDept.cs 폼이름 재정의" />	
    public partial class frmPmpaViewIpdMirCheckByDept : Form
    {
        clsPmpaMisu cpm = new clsPmpaMisu();

        double[,] FnAmt = new double[16, 6];

        public frmPmpaViewIpdMirCheckByDept()
        {
            InitializeComponent();
        }

        void SS_SET()
        {
            //건강보험
            if (chkOpt0.Checked == true)
            {
                ssView_Sheet1.Rows[0].Visible = true;
                ssView_Sheet1.Rows[5].Visible = true;
                ssView_Sheet1.Rows[10].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[0].Visible = false;
                ssView_Sheet1.Rows[5].Visible = false;
                ssView_Sheet1.Rows[10].Visible = false;
            }

            //의료급여
            if (chkOpt1.Checked == true)
            {
                ssView_Sheet1.Rows[1].Visible = true;
                ssView_Sheet1.Rows[6].Visible = true;
                ssView_Sheet1.Rows[11].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[1].Visible = false;
                ssView_Sheet1.Rows[6].Visible = false;
                ssView_Sheet1.Rows[11].Visible = false;
            }

            //자보
            if (chkOpt2.Checked == true)
            {
                ssView_Sheet1.Rows[3].Visible = true;
                ssView_Sheet1.Rows[8].Visible = true;
                ssView_Sheet1.Rows[13].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[3].Visible = false;
                ssView_Sheet1.Rows[8].Visible = false;
                ssView_Sheet1.Rows[13].Visible = false;
            }

            //산재
            if (chkOpt2.Checked == true)
            {
                ssView_Sheet1.Rows[2].Visible = true;
                ssView_Sheet1.Rows[7].Visible = true;
                ssView_Sheet1.Rows[12].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[2].Visible = false;
                ssView_Sheet1.Rows[7].Visible = false;
                ssView_Sheet1.Rows[12].Visible = false;
            }

            ssView_Sheet1.Columns[0].Visible = false;

            ssView_Sheet1.Cells[0, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

        }

        private void chkOpt_CheckedChanged(object sender, EventArgs e)
        {
            SS_SET();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            int i = 0;
            int k = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                for (k = 2; k < ssView_Sheet1.ColumnCount; k++)
                {
                    ssPrint_Sheet1.Cells[i + 6, k].Text = ssView_Sheet1.Cells[i, k].Text;
                }
            }

            strTitle = "월별 입원청구액 점검표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("[" + cboYYMM.Text + "~" + cboYYMM2.Text + "]", new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            if (chkGel.Checked == false)
            {
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 200, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, false, false, false, false);
                CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else
            {
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 120, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);
                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int j = 0;
            int nRead = 0;
            int nBiNo = 0;
            double nAmt = 0;
            string strYYMM = "";
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTDate = "";
            string strBi = "";
            string strBiGbn = "";
            string strJungFDate = "";
            string strJungTDate = "";
            string strOK = "";

            //누적할 배열을 Clear
            for (i = 0; i < 16; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            strFYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";

            strTYYMM = VB.Left(cboYYMM2.Text, 4) + VB.Mid(cboYYMM2.Text, 7, 2);
            strTDate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strTYYMM, 4)), Convert.ToInt32(VB.Right(strTYYMM, 2)));

            //jjy(2003-01-13) '통계 remark 등록 공용변수
            //GstrYYMM = strFYYMM;
            //GstrMenu = "4";
            //GstrSMenu = "1";

            try
            {
                //퇴원자 조합부담금
                #region Slip_ADD
                if (strYYMM != "200112")
                {
                    //입원 당월퇴원 조합부담 발생액을 MISU_BALDAILY에서 READ
                    //BIGBN   종류(1.보험 2.보호 3.산재 4.자보 5.일반)
                    strBiGbn = "''";
                    if (chkOpt0.Checked == true)
                    {
                        strBiGbn += ",'1','5'";
                    }
                    if (chkOpt1.Checked == true)
                    {
                        strBiGbn += ",'2'";
                    }
                    if (chkOpt3.Checked == true)
                    {
                        strBiGbn += ",'3'";
                    }
                    if (chkOpt2.Checked == true)
                    {
                        strBiGbn += ",'4'";
                    }

                    SQL = SQL + ComNum.VBLF + "SELECT BiGbn, SUM(Amt33) Amt ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ActDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND IpdOpd='I' ";
                    if (strBiGbn != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND BIGBN IN (" + strBiGbn + " ) ";
                    }
                    SQL = SQL + ComNum.VBLF + "GROUP BY BiGbn ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    nRead = dt.Rows.Count;
                    for (i = 0; i < nRead; i++)
                    {
                        nBiNo = (int)VB.Val(dt.Rows[i]["BiGbn"].ToString().Trim());
                        nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        if (nAmt != 0)
                        {
                            switch (nBiNo)
                            {
                                case 1:
                                    j = 1;  //보험
                                    break;
                                case 2:
                                    j = 2;  //보호
                                    break;
                                case 3:
                                    j = 3;  //산재
                                    break;
                                case 4:
                                    j = 4;  //자보
                                    break;
                                default:
                                    j = 1;  //기타는 보험으로
                                    break;
                            }
                            FnAmt[j, 1] += nAmt;
                            FnAmt[5, 1] += nAmt;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                #endregion


                //중간청구 조합부담금
                #region JungganMir_ADD
                strJungFDate = Convert.ToDateTime(clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strFYYMM, 4)), Convert.ToInt32(VB.Right(strFYYMM, 2)))).AddDays(1).ToString("yyyy-MM-dd");
                strJungTDate = Convert.ToDateTime(clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strTYYMM, 4)), Convert.ToInt32(VB.Right(strTYYMM, 2)))).AddDays(1).ToString("yyyy-MM-dd");
                strJungTDate = Convert.ToDateTime(clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strJungTDate, 4)), Convert.ToInt32(VB.Mid(strJungTDate, 6, 2)))).AddDays(1).ToString("yyyy-MM-dd");

                //SUBI     구분(1.보험 2.보호 3.산재 4.자보 5.일반)
                strBiGbn = "''";
                if (chkOpt0.Checked == true)
                {
                    strBiGbn += ",'1','5'";
                }
                if (chkOpt1.Checked == true)
                {
                    strBiGbn += ",'2'";
                }
                if (chkOpt3.Checked == true)
                {
                    strBiGbn += ",'3'";
                }
                if (chkOpt2.Checked == true)
                {
                    strBiGbn += ",'4'";
                }

                //일월 1일부터 말일까지 중간청구 Build 조합부담 발생액을 Read
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Bi, SUBI, SUM(BuildJAmt) Amt, SUM(JepJAmt) JepJAmt ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MIR_IPDID";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND YYMM>='200112'";
                SQL = SQL + ComNum.VBLF + "   AND BuildDate>=TO_DATE('" + strJungFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND BuildDate< TO_DATE('" + strJungTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Flag='1' ";      //청구Build한 내역
                if (strBiGbn != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND SUBI IN (" + strBiGbn + " ) ";
                }
                SQL = SQL + ComNum.VBLF + "GROUP BY Bi, SUBI, Pano ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    strOK = "NO";
                    nBiNo = (int)VB.Val(dt.Rows[i]["SUBI"].ToString().Trim());
                    if (dt.Rows[i]["SUBI"].ToString().Trim() == "")
                    {
                        nBiNo = cpm.READ_Bi_SuipTong(dt.Rows[i]["Bi"].ToString().Trim(), strJungFDate);
                    }
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());

                    if (strYYMM == "200306" || nAmt != 0)
                    {
                        strOK = "OK";
                    }
                    if (strYYMM == "200412" && nAmt == 0)
                    {
                        strOK = "OK";
                    }
                    if (strYYMM == "201103" || nAmt == 0)
                    {
                        strOK = "OK";
                    }
                    //경리과 요청(2005-02-02) 김혜향 요청

                    if (strOK == "OK")
                    {
                        switch (nBiNo)
                        {
                            case 1:
                                j = 6;  //보험
                                break;
                            case 2:
                                j = 7;  //보호
                                break;
                            case 3:
                                j = 8;  //산재
                                break;
                            case 4:
                                j = 9;  //자보
                                break;
                            default:
                                j = 6;  //기타는 보험으로
                                break;
                        }

                        //중간청구 대상 금액
                        FnAmt[j, 1] += nAmt;
                        FnAmt[10, 1] += nAmt;

                        //중간청구 접수금액
                        nAmt = VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim());
                        FnAmt[j, 2] += nAmt;
                        FnAmt[10, 2] += nAmt;
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion


                //응급실 6시간 이상, NP 낮 병동
                #region Em6TimeOver_ADD

                if (strFYYMM != "200112")
                {
                    //응급실6시간이상,NP낮병동 조합부담액 ADD
                    //SUBI    CHAR(1)     구분(1.보험 2.보호 3.산재 4.자보 5.일반)
                    strBiGbn = "''";
                    if (chkOpt0.Checked == true)
                    {
                        strBiGbn += ",'1','5'";
                    }
                    if (chkOpt1.Checked == true)
                    {
                        strBiGbn += ",'2'";
                    }
                    if (chkOpt3.Checked == true)
                    {
                        strBiGbn += ",'3'";
                    }
                    if (chkOpt2.Checked == true)
                    {
                        strBiGbn += ",'4'";
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(ACTDATE,'YYYYMM') YYYYMM, SuBi, SUM(Johap) Amt, SUM(JepJAmt) JepJAmt ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND ActDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND Gubun='3'";  //응급6시간초과
                    if (strBiGbn != "")
                    {
                        SQL = SQL + ComNum.VBLF + "   AND SUBI IN (" + strBiGbn + " ) ";
                    }
                    SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(ACTDATE,'YYYYMM'), SuBi ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    nRead = dt.Rows.Count;
                    for (i = 0; i < nRead; i++)
                    {
                        nBiNo = (int)VB.Val(dt.Rows[i]["SuBi"].ToString().Trim());
                        switch (nBiNo)
                        {
                            case 1:
                                j = 11;  //보험
                                break;
                            case 2:
                                j = 12;  //보호
                                break;
                            case 3:
                                j = 13;  //산재
                                break;
                            case 4:
                                j = 14;  //자보
                                break;
                            default:
                                j = 11;  //기타는 보험으로
                                break;
                        }
                        if (nBiNo == 1 && string.Compare(dt.Rows[i]["YYYYMM"].ToString().Trim(), "201601") >= 0)
                        {
                            nAmt = VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim());
                            FnAmt[1, 2] -= nAmt;
                            FnAmt[5, 2] -= nAmt;
                        }
                        else if (nBiNo == 2 && string.Compare(dt.Rows[i]["YYYYMM"].ToString().Trim(), "201602") >= 0)
                        {
                            nAmt = VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim());
                            FnAmt[2, 2] -= nAmt;
                            FnAmt[5, 2] -= nAmt;
                        }
                        nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        FnAmt[j, 1] += nAmt;
                        FnAmt[15, 1] += nAmt;
                        nAmt = VB.Val(dt.Rows[i]["JepJAmt"].ToString().Trim());
                        FnAmt[j, 2] += nAmt;
                        FnAmt[15, 2] += nAmt;
                    }
                    dt.Dispose();
                    dt = null;
                }

                #endregion


                //당월분 실청구액
                #region SilMir_ADD

                //미수관리에서 입원 청구액을 ADD
                //** 미수종류(Class) **
                //01.공단 02.직장 03.지역 04.보호 05.산재 07.자보
                strBiGbn = "''";
                if (chkOpt0.Checked == true)
                {
                    strBiGbn += ",'01','02','03'";
                }
                if (chkOpt1.Checked == true)
                {
                    strBiGbn += ",'04'";
                }
                if (chkOpt3.Checked == true)
                {
                    strBiGbn += ",'05'";
                }
                if (chkOpt2.Checked == true)
                {
                    strBiGbn += ",'07'";
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Class, TongGbn, SUM(Amt2) Amt";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND MirYYMM >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND MirYYMM <= '" + strTYYMM + "'";
                if (strBiGbn != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND CLASS IN (" + strBiGbn + " )";
                }
                SQL = SQL + ComNum.VBLF + "   AND TongGbn = '1'";        //퇴원청구
                SQL = SQL + ComNum.VBLF + "   AND IpdOpd='I'";
                SQL = SQL + ComNum.VBLF + " GROUP BY Class,TongGbn";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    strBi = dt.Rows[i]["Class"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    switch (strBi)
                    {
                        case "01":
                        case "02":
                        case "03":
                            j = 1;  //보험
                            break;
                        case "04":
                            j = 2;  //보호
                            break;
                        case "05":
                            j = 3;  //산재
                            break;
                        case "07":
                            j = 4;  //자보
                            break;
                        default:
                            j = 1;  //기타는 보험으로
                            break;
                    }

                    if (dt.Rows[i]["TongGbn"].ToString().Trim() == "1") //퇴원
                    {
                        if (strYYMM != "200112")
                        {
                            FnAmt[j, 2] += nAmt;
                            FnAmt[5, 2] += nAmt;
                        }
                    }
                    else  //중간청구
                    {
                        FnAmt[j + 5, 2] += nAmt;
                        FnAmt[10, 2] += nAmt;
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion


                //내용을 Sheet에 Display
                for (i = 1; i < 16; i++)
                {
                    //청구차액 = 미수발생액 - 발생미수액
                    FnAmt[i, 3] = FnAmt[i, 2] - FnAmt[i, 1];

                    //EDI차액 = 발생미수액 - EDI접수액
                    FnAmt[i, 5] = FnAmt[i, 2] - FnAmt[i, 4];

                    for (j = 1; j < 6; j++)
                    {
                        ssView_Sheet1.Cells[i - 1, j + 1].Text = FnAmt[i, j].ToString("###,###,###,##0 ");
                    }
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewIpdMirCheckByDept_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 24, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM2, 24, "", "1");
            SS_SET();

            ssView2_Sheet1.Columns[3].Visible = false;
            ssView2_Sheet1.Columns[4].Visible = false;
            ssView2_Sheet1.Columns[5].Visible = false;
            ssView2_Sheet1.Columns[6].Visible = false;
            ssView2_Sheet1.Columns[7].Visible = false;
            ssView2_Sheet1.Columns[8].Visible = false;
            ssView2_Sheet1.Columns[9].Visible = false;
            ssView2_Sheet1.Columns[14].Visible = false;
            ssView2_Sheet1.Columns[15].Visible = false;
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int j = 0;
            int nRead = 0;
            int nRead2 = 0;
            int nRow = 0;
            int nDept = 0;

            double nSumTot1 = 0;
            double nSumTot2 = 0; //퇴원합
            double nSumTot3 = 0;
            double nSumTot4 = 0; //중간합
            double nSumTot5 = 0; //응급
            double nSumTot6 = 0;
            double nSumTot7 = 0;
            double nSumTot8 = 0;
            double nSubSum1 = 0;
            double nTempMisuAmt = 0;
            double nTempCnt = 0;
            double nTempAmt = 0;
            double nTempAmt2 = 0;
            double nTempAmt3 = 0;
            double nTempAmt4 = 0;
            double nTempAmt5 = 0;
            double nTempAmt6 = 0;
            double nTempAmt7 = 0;
            double nTempAmt8 = 0;

            string strYYMM = "";
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTDate = "";
            string strBiGbn = "";
            string strDeptRow = "";
            string strOK = "";
            string strDept = "";
            string strJungFDate = "";
            string strJungTDate = "";

            double[,] nSumDept = new double[51, 201];
            double[,] nSumCnt = new double[51, 201];

            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 1;


            //누적할 배열을 Clear
            for (i = 0; i < 16; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

            strFYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";

            strTYYMM = VB.Left(cboYYMM2.Text, 4) + VB.Mid(cboYYMM2.Text, 7, 2);
            strTDate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strTYYMM, 4)), Convert.ToInt32(VB.Right(strTYYMM, 2)));

            strJungFDate = Convert.ToDateTime(clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strFYYMM, 4)), Convert.ToInt32(VB.Right(strFYYMM, 2)))).AddDays(1).ToString("yyyy-MM-dd");
            strJungTDate = Convert.ToDateTime(clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strTYYMM, 4)), Convert.ToInt32(VB.Right(strTYYMM, 2)))).AddDays(1).ToString("yyyy-MM-dd");
            strJungTDate = Convert.ToDateTime(clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strJungTDate, 4)), Convert.ToInt32(VB.Mid(strJungTDate, 6, 2)))).AddDays(1).ToString("yyyy-MM-dd");

            nSumTot1 = 0;
            nSumTot2 = 0;
            nSumTot3 = 0;
            nSumTot4 = 0;
            nSumTot5 = 0;
            nSumTot6 = 0;
            nSumTot7 = 0;
            nSumTot8 = 0;

            nTempAmt = 0;
            nTempMisuAmt = 0;
            nSubSum1 = 0;

            for (i = 0; i < 51; i++)
            {
                for (j = 0; j < 201; j++)
                {
                    nSumDept[i, j] = 0;
                    nSumCnt[i, j] = 0;
                }
            }

            try
            {
                //과배열 세팅
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DeptCode";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "bas_clinicdept";
                SQL = SQL + ComNum.VBLF + " WHERE deptcode not in ('II','PT','AN','HC','OM','LM')";
                SQL = SQL + ComNum.VBLF + " ORDER BY printranking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;
                strDeptRow = "";

                for (i = 0; i < nRead; i++)
                {
                    strDeptRow += dt.Rows[i]["DeptCode"].ToString().Trim() + "," + (i + 1) + ";";
                }

                dt.Dispose();
                dt = null;

                strDept = "";
                nDept = 0;
                nTempMisuAmt = 0;
                nTempCnt = 0;

                strBiGbn = "''";

                if (chkOpt0.Checked == true)
                {
                    strBiGbn += ",'01','02','03'";
                }
                if (chkOpt1.Checked == true)
                {
                    strBiGbn += ",'04'";
                }
                if (chkOpt3.Checked == true)
                {
                    strBiGbn += ",'05'";
                }
                if (chkOpt2.Checked == true)
                {
                    strBiGbn += ",'07'";
                }


                #region 미수발생
                //미수발생
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT class, ipdopd, deptcode,";
                SQL = SQL + ComNum.VBLF + "       edimirno, sum(amt2) misuamt, sum(qty1) cnt1";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND MirYYMM >= '" + strFYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND MirYYMM <= '" + strTYYMM + "'";
                if (strBiGbn != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND CLASS IN (" + strBiGbn + " ) ";
                }
                SQL = SQL + ComNum.VBLF + "   AND TongGbn = '1'";
                SQL = SQL + ComNum.VBLF + "   AND IpdOpd='I'";
                SQL = SQL + ComNum.VBLF + " GROUP BY class,ipdopd,deptcode,edimirno ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    for (i = 0; i < nRead; i++)
                    {
                        switch (dt.Rows[i]["Class"].ToString().Trim())
                        {
                            case "07":  //자보 5
                                strDept = "";
                                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nDept = 0;
                                nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                if (strDept != "" && nDept > 0)
                                {
                                    nSumDept[nDept, 5] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());
                                    nTempMisuAmt += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());
                                    nSumCnt[nDept, 5] += VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim());
                                    nTempCnt += VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim());
                                }
                                break;
                            case "05":  //산재 3
                                strDept = "";
                                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nDept = 0;
                                nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                if (strDept != "" && nDept > 0)
                                {
                                    nSumDept[nDept, 3] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());
                                    nTempMisuAmt += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());
                                    nSumCnt[nDept, 3] += VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim());
                                    nTempCnt += VB.Val(dt.Rows[i]["Cnt1"].ToString().Trim());
                                }
                                break;
                            case "04":  //의급 2
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "SELECT deptcode1, sum(nvl(edijamt,0) + nvl(ediuamt100,0) + nvl(edigamt,0) + nvl(ediboamt,0)) jamt, count(wrtno) cnt1";
                                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "mir_insid";
                                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "   AND edimirno= '" + dt.Rows[i]["EdiMirNo"].ToString().Trim() + "'";
                                SQL = SQL + ComNum.VBLF + "   AND ipdopd ='I' ";
                                SQL = SQL + ComNum.VBLF + "   AND johap ='5' ";
                                SQL = SQL + ComNum.VBLF + " GROUP BY deptcode1";
                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                nRead2 = dt1.Rows.Count;

                                if (nRead2 > 0)
                                {
                                    for (j = 0; j < nRead2; j++)
                                    {
                                        strDept = "";
                                        strDept = dt1.Rows[j]["DeptCode1"].ToString().Trim();
                                        nDept = 0;
                                        nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                        if (strDept != "" && nDept > 0)
                                        {
                                            nSumDept[nDept, 2] += VB.Val(dt1.Rows[j]["JAmt"].ToString().Trim());
                                            nTempMisuAmt += VB.Val(dt1.Rows[j]["JAmt"].ToString().Trim());
                                            nSumCnt[nDept, 2] += VB.Val(dt1.Rows[j]["Cnt1"].ToString().Trim());
                                            nTempCnt += VB.Val(dt1.Rows[j]["Cnt1"].ToString().Trim());
                                        }
                                    }
                                }
                                dt1.Dispose();
                                dt1 = null;
                                break;
                            case "01":
                            case "02":
                            case "03":      //건보 1
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "SELECT deptcode1, sum(nvl(edijamt,0) + nvl(ediuamt100,0) + nvl(edigamt,0) + nvl(ediboamt,0)) jamt, count(wrtno) cnt1";
                                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "mir_insid";
                                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "   AND edimirno= '" + dt.Rows[i]["EdiMirNo"].ToString().Trim() + "'";
                                SQL = SQL + ComNum.VBLF + "   AND ipdopd ='I' ";
                                SQL = SQL + ComNum.VBLF + "   AND johap <> '5' ";
                                SQL = SQL + ComNum.VBLF + " GROUP BY deptcode1";
                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                nRead2 = dt1.Rows.Count;

                                if (nRead2 > 0)
                                {
                                    for (j = 0; j < nRead2; j++)
                                    {
                                        strDept = "";
                                        strDept = dt1.Rows[j]["DeptCode1"].ToString().Trim();
                                        nDept = 0;
                                        nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                        if (strDept != "" && nDept > 0)
                                        {
                                            nSumDept[nDept, 1] += VB.Val(dt1.Rows[j]["JAmt"].ToString().Trim());
                                            nTempMisuAmt += VB.Val(dt1.Rows[j]["JAmt"].ToString().Trim());
                                            nSumCnt[nDept, 1] += VB.Val(dt1.Rows[j]["Cnt1"].ToString().Trim());
                                            nTempCnt += VB.Val(dt1.Rows[j]["Cnt1"].ToString().Trim());
                                        }
                                    }
                                }
                                dt1.Dispose();
                                dt1 = null;
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                #endregion

                #region 발생금액
                //발생금액
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DeptCode";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "bas_clinicdept";
                SQL = SQL + ComNum.VBLF + " WHERE deptcode not in ('II','PT','AN','HC','OM','LM')";
                SQL = SQL + ComNum.VBLF + " ORDER BY printranking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;

                    for (i = 0; i < nRead; i++)
                    {
                        for (j = 1; j < 6; j++)
                        {
                            strOK = "";

                            if (chkOpt0.Checked == true && j == 1)
                            {
                                strOK = "OK";
                            }
                            if (chkOpt1.Checked == true && j == 2)
                            {
                                strOK = "OK";
                            }
                            if (chkOpt3.Checked == true && j == 3)
                            {
                                strOK = "OK";
                            }
                            if (chkOpt2.Checked == true && j == 5)
                            {
                                strOK = "OK";
                            }

                            if (j == 4)
                            {
                                strOK = "";
                            }

                            if (strOK == "OK")
                            {
                                nRow += 1;
                                ssView2_Sheet1.RowCount = nRow;
                                switch (j)
                                {
                                    case 1:
                                        ssView2_Sheet1.Cells[nRow - 1, 1].Text = "건보";
                                        break;
                                    case 2:
                                        ssView2_Sheet1.Cells[nRow - 1, 1].Text = "의급";
                                        break;
                                    case 3:
                                        ssView2_Sheet1.Cells[nRow - 1, 1].Text = "산재";
                                        break;
                                    case 5:
                                        ssView2_Sheet1.Cells[nRow - 1, 1].Text = "자보";
                                        break;
                                }
                                ssView2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();

                                nTempAmt = 0;
                                nTempAmt2 = 0;
                                nTempAmt3 = 0;
                                nTempAmt4 = 0;
                                nTempAmt5 = 0;
                                nTempAmt6 = 0;
                                nTempAmt7 = 0;
                                nTempAmt8 = 0;


                                //발생
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "SELECT substr(trim(bi),1,1) bi, deptcode, SUM(TotAmt) TotAmt,";
                                SQL = SQL + ComNum.VBLF + "       SUM(Junggan) Junggan, SUM(Johap) Johap, SUM(Halin) Halin,";
                                SQL = SQL + ComNum.VBLF + "       SUM(Bojung) Bojung, SUM(EtcMisu) EtcMisu, SUM(SuNap) SuNap,";
                                SQL = SQL + ComNum.VBLF + "       SUM(Dansu) Dansu, SUM(DRUGAMT) DRUGAMT, SUM(Johap+junggan) totamt2";
                                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALTEWON";
                                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "   AND deptcode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'";
                                SQL = SQL + ComNum.VBLF + "   AND actdate >=to_date('" + strFDate + "','yyyy-mm-dd')";
                                SQL = SQL + ComNum.VBLF + "   AND actdate <=to_date('" + strTDate + "','yyyy-mm-dd')";
                                SQL = SQL + ComNum.VBLF + "   AND gubun in ('1')";
                                SQL = SQL + ComNum.VBLF + "   AND substr(trim(bi),1,1) ='" + j + "'";
                                SQL = SQL + ComNum.VBLF + " GROUP BY substr(trim(bi),1,1),deptcode";
                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    ssView2_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dt1.Rows[0]["totamt2"].ToString().Trim()).ToString("###,###,###,##0 ");
                                    nTempAmt = VB.Val(dt1.Rows[0]["totamt2"].ToString().Trim());
                                    nSumTot1 += VB.Val(dt1.Rows[0]["totamt2"].ToString().Trim());
                                }
                                dt1.Dispose();
                                dt1 = null;


                                //응급실 6시간
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "SELECT substr(trim(bi),1,1) bi, deptcode, SUM(TotAmt) TotAmt,";
                                SQL = SQL + ComNum.VBLF + "       SUM(Junggan) Junggan, SUM(Johap) Johap, SUM(Halin) Halin,";
                                SQL = SQL + ComNum.VBLF + "       SUM(Bojung) Bojung, SUM(EtcMisu) EtcMisu, SUM(SuNap) SuNap,";
                                SQL = SQL + ComNum.VBLF + "       SUM(Dansu) Dansu, SUM(DRUGAMT) DRUGAMT, SUM(Johap+junggan) totamt2,";
                                SQL = SQL + ComNum.VBLF + "       SUM(JepJAmt) JepJAmt ";
                                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALTEWON";
                                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "   AND deptcode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'";
                                SQL = SQL + ComNum.VBLF + "   AND actdate >=to_date('" + strFDate + "','yyyy-mm-dd')";
                                SQL = SQL + ComNum.VBLF + "   AND actdate <=to_date('" + strTDate + "','yyyy-mm-dd')";
                                SQL = SQL + ComNum.VBLF + "   AND gubun in ('3')  ";
                                SQL = SQL + ComNum.VBLF + "   AND substr(trim(bi),1,1) ='" + j + "' ";
                                SQL = SQL + ComNum.VBLF + " GROUP BY substr(trim(bi),1,1),deptcode ";
                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    ssView2_Sheet1.Cells[nRow - 1, 9].Text = VB.Val(dt1.Rows[0]["TotAmt2"].ToString().Trim()).ToString("###,###,###,##0 ");
                                    nTempAmt5 = VB.Val(dt1.Rows[0]["TotAmt2"].ToString().Trim());
                                    nSumTot5 += VB.Val(dt1.Rows[0]["TotAmt2"].ToString().Trim());

                                    nTempAmt7 = VB.Val(dt1.Rows[0]["JepJAmt"].ToString().Trim());
                                    nSumTot7 += VB.Val(dt1.Rows[0]["JepJAmt"].ToString().Trim());
                                    ssView2_Sheet1.Cells[nRow - 1, 15].Text = dt1.Rows[0]["JepJAmt"].ToString().Trim();
                                }
                                dt1.Dispose();
                                dt1 = null;

                                //중간청구 발생1
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "SELECT deptcode, substr(trim(bi),1,1) bi, SUM(BuildJAmt) Junggan,";
                                SQL = SQL + ComNum.VBLF + "        SUM(BuildJAmt) Amt, SUM(JepJAmt) JepJAmt ";
                                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MIR_IPDID";
                                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "   AND deptcode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'";
                                SQL = SQL + ComNum.VBLF + "   AND TDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                                SQL = SQL + ComNum.VBLF + "   AND TDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                                SQL = SQL + ComNum.VBLF + "   AND substr(trim(bi),1,1) ='" + j + "' ";
                                SQL = SQL + ComNum.VBLF + " GROUP BY deptcode,substr(trim(bi),1,1) ";
                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    ssView2_Sheet1.Cells[nRow - 1, 4].Text = (nTempAmt - VB.Val(dt1.Rows[0]["Junggan"].ToString().Trim())).ToString("###,###,###,##0 ");
                                    nTempAmt2 = VB.Val(dt1.Rows[0]["Junggan"].ToString().Trim());
                                    nSumTot2 += (nTempAmt - VB.Val(dt1.Rows[0]["Junggan"].ToString().Trim()));
                                }
                                else
                                {
                                    nSumTot2 += nTempAmt;
                                }
                                dt1.Dispose();
                                dt1 = null;

                                //중간청구 발생2
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "SELECT deptcode, substr(trim(bi),1,1) bi, SUM(BuildJAmt) Junggan,";
                                SQL = SQL + ComNum.VBLF + "       SUM(BuildJAmt) Amt, SUM(JepJAmt) JepJAmt";
                                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MIR_IPDID";
                                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                                SQL = SQL + ComNum.VBLF + "   AND deptcode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'";
                                SQL = SQL + ComNum.VBLF + "   AND BuildDate>=TO_DATE('" + strJungFDate + "','YYYY-MM-DD')";
                                SQL = SQL + ComNum.VBLF + "   AND BuildDate< TO_DATE('" + strJungTDate + "','YYYY-MM-DD')";
                                SQL = SQL + ComNum.VBLF + "   AND YYMM>='200112'";
                                SQL = SQL + ComNum.VBLF + "   AND substr(trim(bi),1,1) ='" + j + "' ";
                                SQL = SQL + ComNum.VBLF + " GROUP BY deptcode,substr(trim(bi),1,1)";
                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    ssView2_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt1.Rows[0]["Junggan"].ToString().Trim()).ToString("###,###,###,##0 ");
                                    nTempAmt4 = VB.Val(dt1.Rows[0]["Junggan"].ToString().Trim());
                                    nSumTot4 += VB.Val(dt1.Rows[0]["Junggan"].ToString().Trim());

                                    nTempAmt6 = VB.Val(dt1.Rows[0]["JepJAmt"].ToString().Trim());
                                    nSumTot6 += VB.Val(dt1.Rows[0]["JepJAmt"].ToString().Trim());
                                    ssView2_Sheet1.Cells[nRow - 1, 14].Text = dt1.Rows[0]["JepJAmt"].ToString().Trim();
                                }
                                dt1.Dispose();
                                dt1 = null;

                                strDept = "";
                                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nDept = 0;
                                nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                if (nDept > 0)
                                {
                                    ssView2_Sheet1.Cells[nRow - 1, 5].Text = nSumDept[nDept, j].ToString("###,###,###,##0 ");
                                    ssView2_Sheet1.Cells[nRow - 1, 12].Text = (nTempAmt6 + nTempAmt7 + nSumDept[nDept, j]).ToString("###,###,###,##0 ");
                                    nTempAmt8 = (nTempAmt6 + nTempAmt7 + nSumDept[nDept, j]) - (nTempAmt - nTempAmt2 + nTempAmt4 + nTempAmt5);
                                    nSumTot8 += nTempAmt8;
                                    ssView2_Sheet1.Cells[nRow - 1, 16].Text = nSumCnt[nDept, j].ToString("###,###,###,##0 ");
                                }
                                else
                                {
                                    ssView2_Sheet1.Cells[nRow - 1, 12].Text = (nTempAmt6 + nTempAmt7).ToString("###,###,###,##0 ");
                                    nTempAmt8 = (nTempAmt6 + nTempAmt7) - (nTempAmt - nTempAmt2 + nTempAmt4 + nTempAmt5);
                                    nSumTot8 += nTempAmt8;
                                }

                                ssView2_Sheet1.Cells[nRow - 1, 11].Text = (nTempAmt - nTempAmt2 + nTempAmt4 + nTempAmt5).ToString("###,###,###,##0 ");
                                nSubSum1 = nSubSum1 + (nTempAmt - nTempAmt2 + nTempAmt4 + nTempAmt5);
                                ssView2_Sheet1.Cells[nRow - 1, 13].Text = nTempAmt8.ToString("###,###,###,##0 ");
                            }
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion

                ssView2_Sheet1.RowCount = nRow + 1;
                ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 3].Text = nSumTot1.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 4].Text = nSumTot2.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 5].Text = nTempMisuAmt.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 6].Text = (nTempMisuAmt - nSumTot2).ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 8].Text = nSumTot4.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 9].Text = nSumTot5.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 11].Text = nSubSum1.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 12].Text = (nTempMisuAmt + nSumTot6 + nSumTot7).ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 13].Text = nSumTot8.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 14].Text = nSumTot6.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 15].Text = nSumTot7.ToString("###,###,###,##0 ");
                ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 16].Text = nTempCnt.ToString("###,###,###,##0 ");

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
