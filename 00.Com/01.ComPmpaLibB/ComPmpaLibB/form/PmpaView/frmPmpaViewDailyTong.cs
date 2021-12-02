using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewDailyTong.cs
    /// Description     : 수납집계표
    /// Author          : 박창욱
    /// Create Date     : 2017-09-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\FrmDailyTong_New.frm(FrmDailyTong_New.frm) >> frmPmpaViewDailyTong.cs 폼이름 재정의" />	
    public partial class frmPmpaViewDailyTong : Form
    {
        string GstrMsgList = "";

        //2011/03/22 추가 50: 약제산한액, 51:선택진료비 , 52:예약검사 53:예약검사대체, 54:부가세, 55:부가세 원단위 절사액
        double[,] FnAmt = new double[56, 7];

        public frmPmpaViewDailyTong()
        {
            InitializeComponent();
        }

        void VIEW_NEW_EXAM_ALL()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int k = 0;
            int nRead = 0;
            int nBi = 0;
            int nRow = 0;

            double nAmt = 0;
            double nChaAmt = 0;
            string strFDate = "";
            string strTDate = "";
            string strCha = "";
            long[] nTempAmt = new long[10];

            for (i = 0; i < nTempAmt.Length; i++)
            {
                nTempAmt[i] = 0;
            }

            if (dtpFDate.Value >= Convert.ToDateTime("2011-04-01"))
            {
                ssView_Sheet1.Rows[24].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[24].Visible = false;
            }

            ssView_Sheet1.Rows[51].Visible = false;

            //입원인 경우 "일별 재원미수금 점검표" 점검여부를 확인함
            Etc_AutoMagam_Check();


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(JOBDATE,'YYYY-MM-DD HH24:MI') JOBDATE, WHY";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALDAILY_HISTORY ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(JOBDATE,'YYYY-MM-DD HH24:MI'), WHY ";
                SQL = SQL + ComNum.VBLF + " ORDER BY JOBDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssView2_Sheet1.RowCount = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView2_Sheet1.RowCount += 1;
                    ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 0].Text = "재빌더 일자 : " + dt.Rows[i]["JOBDATE"].ToString().Trim();
                    ssView2_Sheet1.SetRowHeight(ssView2_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    if (clsType.User.Sabun == "4349" || clsType.User.Sabun == "25108" || clsType.User.Sabun == "23417" || clsType.User.Sabun == "36863" ||
                       clsType.User.Sabun == "25420" || clsType.User.Sabun == "30761")
                    {
                        ssView2_Sheet1.RowCount += 1;
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 0].Text = "재빌더 사유 : " + dt.Rows[i]["WHY"].ToString().Trim();
                        ssView2_Sheet1.SetRowHeight(ssView2_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    }
                }

                dt.Dispose();
                dt = null;


                Cursor.Current = Cursors.WaitCursor;

                //Sheet Clear
                ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

                //배열을 Clear
                for (i = 0; i < 56; i++)
                {
                    for (k = 0; k < 7; k++)
                    {
                        FnAmt[i, k] = 0;
                    }
                }

                //자료를 SELECT
                SQL = "";
                SQL = "SELECT BiGbn,";
                for (i = 1; i < 44; i++)
                {
                    SQL = SQL + ComNum.VBLF + "SUM(Amt" + i.ToString("00") + ") Amt" + i.ToString("00") + ",";
                }
                SQL = SQL + ComNum.VBLF + " SUM(Amt44) Amt44, SUM(AMT46) AMT46, SUM(AMT47) AMT47, SUM(AMT48) AMT48 , SUM(AMT50) AMT50, ";
                SQL = SQL + ComNum.VBLF + " SUM(Amt51) Amt51, SUM(AMT52) AMT52, SUM(AMT54) AMT54,SUM(AMT55) AMT55  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND ActDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                if (rdoIO0.Checked == true)
                {
                    SQL = ComNum.VBLF + SQL + "  AND IpdOpd = 'I' "; //입원
                }
                else
                {
                    SQL = ComNum.VBLF + SQL + "  AND IpdOpd = 'O' "; //외래
                }
                SQL = ComNum.VBLF + SQL + "GROUP BY BiGbn ";
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

                for (i = 0; i < nRead; i++)
                {
                    nBi = (int)VB.Val(dt.Rows[i]["BiGbn"].ToString().Trim());
                    for (k = 1; k < 45; k++)
                    {
                        nAmt = VB.Val(dt.Rows[i]["Amt" + k.ToString("00")].ToString().Trim());
                        switch (k)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                                nRow = k;
                                break;
                            case 17:
                            case 18:
                            case 19:
                                nRow = k - 1;
                                break;
                            case 20:
                            case 21:
                                nRow = k + 3;
                                break;
                            case 22:
                            case 23:
                                nRow = k - 3;
                                break;
                            case 24:
                                nRow = 26;
                                break;
                            case 26:
                                nRow = 28; //총계
                                break;
                            case 27:
                            case 28:
                                nRow = k + 2;
                                break;
                            case 29:
                            case 30:
                                nRow = k + 3;
                                break;
                            case 31:
                            case 32:
                            case 33:
                            case 34:
                            case 35:
                            case 36:
                            case 37:
                            case 38:
                            case 39:
                            case 40:
                            case 41:
                            case 42:
                            case 43:
                                nRow = k + 4;
                                break;
                            case 44:
                                nRow = k + 5;
                                break;
                            case 16:
                            case 25:
                                nRow = 0;
                                nAmt = 0;       //소계는 사용 안 함. 2004년 1월 1일부터
                                break;
                            default:
                                nRow = 0;
                                nAmt = 0;
                                break;
                        }

                        if (nRow <= 20)
                        {
                            FnAmt[21, nBi] += nAmt;     //소계1
                            FnAmt[21, 6] += nAmt;       //소계2
                        }

                        if (nRow >= 22 && nRow <= 26)
                        {
                            FnAmt[27, nBi] += nAmt;     //소계1
                            FnAmt[27, 6] += nAmt;       //소계2
                        }

                        FnAmt[nRow, nBi] += nAmt;
                        FnAmt[nRow, 6] += nAmt;
                    }

                    //수탁검사 2004년 1월 1일부터 추가
                    nAmt = VB.Val(dt.Rows[i]["AMT46"].ToString().Trim());
                    FnAmt[22, nBi] += nAmt; //수탁검사료 2004년 1월 1일부터 추가
                    FnAmt[22, 6] += nAmt;

                    FnAmt[27, nBi] += nAmt;     //소계2
                    FnAmt[27, 6] += nAmt;

                    //2007-06-26 추가 AMT47 : 의료급여 본인부담금
                    nAmt = VB.Val(dt.Rows[i]["AMT47"].ToString().Trim());
                    FnAmt[48, nBi] += nAmt;
                    FnAmt[48, 6] += nAmt;

                    FnAmt[51, nBi] += VB.Val(dt.Rows[i]["AMT50"].ToString().Trim());
                    FnAmt[51, 6] += VB.Val(dt.Rows[i]["AMT50"].ToString().Trim());

                    //2014-03-06 부가세 추가
                    nAmt = VB.Val(dt.Rows[i]["AMT54"].ToString().Trim());
                    FnAmt[54, nBi] += nAmt;
                    FnAmt[54, 6] += nAmt;

                    nAmt = VB.Val(dt.Rows[i]["AMT55"].ToString().Trim());
                    FnAmt[55, nBi] += nAmt;
                    FnAmt[55, 6] += nAmt;

                    //약세상한 2011년 4월 1일부터 사용
                    nAmt = VB.Val(dt.Rows[i]["AMT48"].ToString().Trim());
                    if (nAmt != 0)
                    {
                        FnAmt[25, nBi] += nAmt;     //약제상한
                        FnAmt[25, 6] += nAmt;

                        FnAmt[27, nBi] += nAmt;     //소계2
                        FnAmt[27, 6] += nAmt;

                        //투약료에서 제외
                        FnAmt[3, nBi] -= nAmt;     //투약료
                        FnAmt[3, 6] -= nAmt;       //투약료

                        FnAmt[21, nBi] -= nAmt;     //소계1
                        FnAmt[21, 6] -= nAmt;       //소계1
                    }

                    //예약검사비 2011-09-27 시행
                    nAmt = VB.Val(dt.Rows[i]["AMT51"].ToString().Trim());
                    FnAmt[29, nBi] += nAmt;     //예약검사비
                    FnAmt[29, 6] += nAmt;       //예약검사비계

                    nAmt = VB.Val(dt.Rows[i]["AMT52"].ToString().Trim());
                    FnAmt[30, nBi] += nAmt;     //예약검사대체
                    FnAmt[30, 6] += nAmt;       //예약검사대체계
                }
                dt.Dispose();
                dt = null;

                strFDate = "";
                strTDate = "";


                //2005년 08월 05일 시점:퇴원자 중간청구 대체액을 원래 청구한 보험자격으로 처리
                if (rdoIO1.Checked == false)
                {
                    //조합미수금 = 조합미수금 + 중간청구대체액으로 변경함
                    for (i = 1; i < 7; i++)
                    {
                        FnAmt[37, i] += FnAmt[36, i];
                        FnAmt[36, i] = 0;
                    }

                    if (dtpFDate.Value <= Convert.ToDateTime("2005-08-05"))
                    {
                        strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
                        if (dtpTDate.Value <= Convert.ToDateTime("2005-08-05"))
                        {
                            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");
                            View_JungMirAmt_Add2(strFDate, strTDate, ref nBi);     //기존방식의 중간청구액 READ
                        }
                        else
                        {
                            strTDate = "2005-08-05";
                            View_JungMirAmt_Add2(strFDate, strTDate, ref nBi);     //기존방식의 중간청구액 READ
                            strFDate = "2005-08-06";
                            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");
                            View_JungMirAmt_Add1(strFDate, strTDate, ref nBi);     //기존방식의 중간청구액 READ
                        }
                    }
                    else
                    {
                        strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
                        strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");
                        View_JungMirAmt_Add1(strFDate, strTDate, ref nBi);     //직접 중간청구 테이블에 가서 DATA READ
                    }
                }

                //외래보증금을 입원에서 (-)금액으로 작업하는 곳
                if (rdoIO0.Checked == true) //입원
                {
                    SQL = "";
                    SQL = "SELECT BiGbn, SUM(Amt42) Amt42 ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "  AND ActDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND IpdOpd = 'O' ";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY BiGbn ";
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
                        nBi = (int)VB.Val(dt.Rows[i]["BiGbn"].ToString().Trim());
                        for (k = 46; k < 48; k++)
                        {
                            nAmt = VB.Val(dt.Rows[i]["Amt42"].ToString().Trim());
                            FnAmt[k, nBi] -= nAmt;
                            FnAmt[k, 6] -= nAmt;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                nChaAmt = 0;


                //현금 마감
                for (k = 1; k < 7; k++)
                {
                    FnAmt[50, k] = FnAmt[47, k] - FnAmt[48, k] - FnAmt[49, k];
                }



                //자료를 Display
                for (i = 1; i < 52; i++)
                {
                    if (i >= 47)
                    {
                        for (k = 1; k < 7; k++)
                        {
                            ssView_Sheet1.Cells[i, k].Text = FnAmt[i, k].ToString("###,###,###,##0 ");
                            if (i == 43 && k < 6)
                            {
                                nChaAmt += FnAmt[i, k];
                            }
                        }
                    }
                    else
                    {
                        for (k = 1; k < 7; k++)
                        {
                            ssView_Sheet1.Cells[i - 1, k].Text = FnAmt[i, k].ToString("###,###,###,##0 ");
                            if (i == 43 && k < 6)
                            {
                                nChaAmt += FnAmt[i, k];
                            }
                        }
                    }
                }

                //부가가치세 추가 2014-03-06
                for (i = 1; i < 7; i++)
                {
                    ssView_Sheet1.Cells[46, i].Text = FnAmt[54, i].ToString("###,###,###,##0 ");
                }

                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;

                ssView_Sheet1.Rows[29].BackColor = Color.FromArgb(255, 255, 255);
                ssView_Sheet1.Rows[36].BackColor = Color.FromArgb(255, 255, 255);
                ssView_Sheet1.Rows[39].BackColor = Color.FromArgb(255, 255, 255);
                ssView_Sheet1.Rows[43].BackColor = Color.FromArgb(255, 255, 255);

                if (rdoIO1.Checked == true)
                {
                    if (FnAmt[30, 6] - FnAmt[40, 6] != 0)
                    {
                        ComFunc.MsgBox("예약비 대체액 오류");
                        ssView_Sheet1.Rows[29].BackColor = Color.FromArgb(255, 200, 200);
                        ssView_Sheet1.Rows[39].BackColor = Color.FromArgb(255, 200, 200);
                        if (clsType.User.Sabun != "4349" && clsType.User.Sabun != "23417" && clsType.User.Sabun != "25420")
                        {
                            btnPrint.Enabled = false;
                        }
                    }
                }

                if (FnAmt[37, 5] != 0)
                {
                    ComFunc.MsgBox("발생주의 일반자격에 조합부담금 발생 오류");
                    ssView_Sheet1.Rows[35].BackColor = Color.FromArgb(255, 200, 200);
                    if (clsType.User.Sabun != "4349" && clsType.User.Sabun != "23417" && clsType.User.Sabun != "25420")
                    {
                        btnPrint.Enabled = false;
                    }
                }


                //입원만
                if (rdoIO0.Checked == true)
                {
                    if (FnAmt[35, 6] != (FnAmt[36, 6] + FnAmt[37, 6] + FnAmt[38, 6]))
                    {
                        ComFunc.MsgBox("퇴원총진료비 = 중간청구액 + 조합미수 + 본인부담 비교점검");
                    }
                }

                if (nChaAmt >= 7000 || nChaAmt <= -7000)
                {
                    ComFunc.MsgBox("해당일자에 차액금액이 있습니다. 확인 요망");
                    ssView_Sheet1.Rows[42].BackColor = Color.FromArgb(200, 200, 255);
                }

                if (dtpFDate.Value >= Convert.ToDateTime("2009-05-08"))
                {
                    //외래에서 입원 보증금 점검로직
                    strCha = "";
                    SQL = "";
                    SQL = " CREATE OR REPLACE VIEW VIEW_IPD_OPD";
                    SQL = SQL + ComNum.VBLF + " AS ";
                    SQL = SQL + ComNum.VBLF + " SELECT PANO,  SUM( -1* AMT1) AMT1 ,  0 AMT2 FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND ActDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ActDate<=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND PART ='#'";
                    SQL = SQL + ComNum.VBLF + "   AND BUN IN('99') ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY PANO";
                    SQL = SQL + ComNum.VBLF + " UNION ALL ";
                    SQL = SQL + ComNum.VBLF + " SELECT PANO ,  0  AMT1, SUM(AMT) AMT2   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND ActDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ActDate<=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND PART ='#'";
                    SQL = SQL + ComNum.VBLF + " GROUP BY PANO";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = " SELECT  PANO,  SUM(  AMT1 - AMT2)  CHA";
                    SQL = SQL + ComNum.VBLF + "   FROM  VIEW_IPD_OPD";
                    SQL = SQL + ComNum.VBLF + " GROUP BY PANO";
                    SQL = SQL + ComNum.VBLF + " HAVING  SUM( AMT1 - AMT2) <> 0";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strCha += dt.Rows[i]["PANO"].ToString().Trim() + " 대체금액 : " + dt.Rows[i]["CHA"].ToString().Trim() + ComNum.VBLF;
                        }
                    }
                    dt.Dispose();
                    dt = null;

                    if (strCha != "")
                    {
                        ComFunc.MsgBox("외래 입원 보증금 대체금액 오류" + ComNum.VBLF + ComNum.VBLF + strCha);
                    }
                }

                //예약비관련 일일점검추가 2011-01-31
                //일일집계표에서 예약비대체액,수납액을 읽음
                if (dtpFDate.Value >= Convert.ToDateTime("2011-01-01"))
                {
                    SQL = "";
                    SQL = "SELECT SUM(Amt27) Amt27,SUM(Amt28) Amt28 ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
                    SQL = SQL + ComNum.VBLF + "WHERE ActDate=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND IpdOpd='O' ";
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
                        nTempAmt[1] = (long)VB.Val(dt.Rows[i]["Amt27"].ToString().Trim());
                        nTempAmt[2] = (long)VB.Val(dt.Rows[i]["Amt28"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;


                    //SLIP의 발생액
                    SQL = "";
                    SQL = "SELECT SUM(Amt7) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                    SQL = SQL + ComNum.VBLF + "WHERE Date1=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND Amt7 <> 0 ";
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
                        nTempAmt[3] = (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;


                    //SLIP 환불액
                    SQL = "";
                    SQL = "SELECT SUM(RetAmt) Amt ";
                    SQL = SQL + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                    SQL = SQL + "WHERE RetDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + "  AND RetDate<=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
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
                        nTempAmt[4] = nTempAmt[3] + (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;


                    //SLIP 대체액
                    SQL = "";
                    SQL = "SELECT SUM(Amt1+Amt2) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                    SQL = SQL + ComNum.VBLF + "WHERE ActDate=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND Bun='99' "; //현금입금
                    SQL = SQL + ComNum.VBLF + "  AND SeqNo= -1 ";
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
                        nTempAmt[5] = (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;


                    //SLIP의 발생액
                    SQL = "";
                    SQL = "SELECT SUM(Amt7) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                    SQL = SQL + ComNum.VBLF + "WHERE TransDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND TransDate<=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
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
                        nTempAmt[6] = (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;


                    //예약부도자 금액
                    SQL = "";
                    SQL = "SELECT SUM(Amt7) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESV_DEL ";
                    SQL = SQL + ComNum.VBLF + "WHERE ActDate=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND TO_CHAR(Date3,'YYYY-MM-DD')=TO_CHAR(ActDate,'YYYY-MM-DD') ";
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
                        nTempAmt[7] = nTempAmt[6] - (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;

                    if (nTempAmt[1] != nTempAmt[4])
                    {
                        ComFunc.MsgBox("예약금관련 점검오류. 전산실 연락 요망");
                    }

                    if ((nTempAmt[2] != nTempAmt[5]) || (nTempAmt[2] != nTempAmt[7]))
                    {
                        ComFunc.MsgBox("예약금관련 점검오류. 전산실 연락 요망");
                    }
                }



                //예약검사 점검
                if (dtpFDate.Value >= Convert.ToDateTime("2011-09-27"))
                {
                    for (i = 0; i < nTempAmt.Length; i++)
                    {
                        nTempAmt[i] = 0;
                    }

                    SQL = "";
                    SQL = "SELECT SUM(Amt51) Amt51,SUM(Amt52) Amt52 ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
                    SQL = SQL + ComNum.VBLF + "WHERE ActDate=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND IpdOpd='O' ";
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
                        nTempAmt[1] = (long)VB.Val(dt.Rows[i]["Amt51"].ToString().Trim());      //수납액
                        nTempAmt[2] = (long)VB.Val(dt.Rows[i]["Amt52"].ToString().Trim());      //대체액
                    }
                    dt.Dispose();
                    dt = null;

                    //SLIP의 발생액
                    SQL = "";
                    SQL = "SELECT SUM(Amt6) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                    SQL = SQL + ComNum.VBLF + "WHERE ActDate=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND Amt6 <> 0 ";
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
                        nTempAmt[3] = (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;


                    //SLIP 환불액
                    SQL = "";
                    SQL = "SELECT SUM(RetAmt) Amt ";
                    SQL = SQL + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                    SQL = SQL + "WHERE RetDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + "  AND RetDate<=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
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
                        nTempAmt[4] = nTempAmt[3] + (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;


                    //예약대체 발생액
                    SQL = "";
                    SQL = "SELECT SUM(TransAmt) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                    SQL = SQL + ComNum.VBLF + "WHERE TransDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND TransDate<=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
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
                        nTempAmt[5] = (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;


                    //진찰경우 예약테이블trans 일자는 반드시 있고 opd_slip 발생안되니 밑에서 빼주는데 예약검사는 예약테이블 trans 안들어감
                    //SLIP의 발생액
                    SQL = "";
                    SQL = "SELECT SUM(TransAmt) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                    SQL = SQL + ComNum.VBLF + "WHERE TransDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND TransDate<=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
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
                        nTempAmt[6] = (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;


                    //예약부도자 금액
                    SQL = "";
                    SQL = "SELECT SUM(Amt7) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_REFUND_EXAM ";
                    SQL = SQL + ComNum.VBLF + "WHERE ActDate=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND cPart ='555'  ";
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
                        nTempAmt[7] = nTempAmt[6] - (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        nTempAmt[9] = (long)VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    }
                    dt.Dispose();
                    dt = null;


                    if (nTempAmt[1] != nTempAmt[4])
                    {
                        ComFunc.MsgBox("예약검사금관련 점검오류. 전산실 연락 요망");
                    }

                    if (nTempAmt[2] != nTempAmt[5])
                    {
                        ComFunc.MsgBox("예약검사금관련 점검오류. 전산실 연락 요망");
                    }
                }
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //입원인 경우 "일별 재원미수금 점검표" 점검여부를 확인함
        void Etc_AutoMagam_Check()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(JobDate,'YYYY-MM-DD') JobDate ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "ETC_AUTOMAGAM ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND JobDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND JobDate<=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND (BTongCheck='N' OR BTongCheck IS NULL) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrMsgList = "통계점검을 하지않아 조회가 불가능합니다." + ComNum.VBLF + ComNum.VBLF;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        GstrMsgList += dt.Rows[i]["JobDate"].ToString().Trim() + "일" + ComNum.VBLF;
                    }
                    ComFunc.MsgBox(GstrMsgList, "발생주의 통계점검 안 함");
                    dt.Dispose();
                    dt = null;
                    return;
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


        void View_JungMirAmt_Add2(string strFDate, string strTDate, ref int nBi)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT BiGbn, SUM(AMT32) AMT32 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND IpdOpd = 'I' "; //입원
                SQL = SQL + ComNum.VBLF + "GROUP BY BiGbn ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nBi = (int)VB.Val(dt.Rows[i]["BiGbn"].ToString().Trim());
                        FnAmt[36, nBi] += VB.Val(dt.Rows[i]["Amt32"].ToString().Trim());
                        FnAmt[37, nBi] -= VB.Val(dt.Rows[i]["Amt32"].ToString().Trim());
                        FnAmt[36, 6] += VB.Val(dt.Rows[i]["Amt32"].ToString().Trim());
                        FnAmt[37, 6] -= VB.Val(dt.Rows[i]["Amt32"].ToString().Trim());
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


        //퇴원자 중간청구 대체액을 원래 청구한 보험자격으로 처리
        void View_JungMirAmt_Add1(string strFDate, string strTDate, ref int nBi)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                //작업일자의 중간청구 퇴원대체액을 환자종류별로 읽음
                SQL = "";
                SQL = "SELECT Bi,SUM(BuildJAmt) JAmt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_IPDID ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND TDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND TDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND Flag='1' "; //청구
                SQL = SQL + ComNum.VBLF + "GROUP BY Bi ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Bi ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nBi = READ_Bi_SuipTong(dt.Rows[i]["Bi"].ToString().Trim(), dtpTDate.Value.ToString("yyyy-MM-dd"));
                        //35:중간청구 대체액 36:퇴원자 조합부담액
                        FnAmt[36, nBi] += VB.Val(dt.Rows[i]["JAmt"].ToString().Trim());
                        FnAmt[37, nBi] -= VB.Val(dt.Rows[i]["JAmt"].ToString().Trim());
                        FnAmt[36, 6] += VB.Val(dt.Rows[i]["JAmt"].ToString().Trim());
                        FnAmt[37, 6] -= VB.Val(dt.Rows[i]["JAmt"].ToString().Trim());
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

        //환자종류로 수입통계에서 사용하는 환자구분으로 변환
        int READ_Bi_SuipTong(string argBi, string argJobDate)
        {
            int rtnval = 0;

            if (string.Compare(argJobDate, "2003-11-03") >= 0)
            {
                switch (argBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "32":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                        rtnval = 1;     //보험
                        break;
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        rtnval = 2;     //보호
                        break;
                    case "31":
                    case "33":
                        rtnval = 3;     //산재
                        break;
                    case "52":
                        rtnval = 4;     //자보
                        break;
                    default:
                        rtnval = 5;     //일반
                        break;
                }
            }
            else
            {
                switch (argBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                        rtnval = 1;     //보험
                        break;
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        rtnval = 2;     //보호
                        break;
                    case "31":
                    case "32":
                    case "33":
                        rtnval = 3;     //산재
                        break;
                    case "52":
                        rtnval = 4;     //자보
                        break;
                    default:
                        rtnval = 5;     //일반
                        break;
                }
            }
            return rtnval;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strFont1 = "";
            string strHead1 = "";

            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            //ssPrint_Sheet1.RowCount = 6;

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"15\" /fs1";
            strHead1 = "/f1" + VB.Space(20);

            if (rdoIO0.Checked == true)
            {
                strHead1 += "    입 원  일 일  집 계 표  ";
            }
            else
            {
                strHead1 += "    외 래  일 일  집 계 표  ";
            }

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
               // ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
              //  ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT);
               // ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].ColumnSpan = 2;
              //  ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 6].ColumnSpan = 2;
              //  ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].ColumnSpan = 2;

                ssPrint_Sheet1.Cells[i + 6, 0].Text = ssView_Sheet1.Cells[i, 0].Text;
                ssPrint_Sheet1.Cells[i + 6, 1].Text = ssView_Sheet1.Cells[i, 1].Text;
                ssPrint_Sheet1.Cells[i + 6, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                ssPrint_Sheet1.Cells[i + 6, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                ssPrint_Sheet1.Cells[i + 6, 4].Text = ssView_Sheet1.Cells[i, 4].Text;
                ssPrint_Sheet1.Cells[i + 6, 5].Text = ssView_Sheet1.Cells[i, 5].Text;
                ssPrint_Sheet1.Cells[i + 6, 6].Text = ssView_Sheet1.Cells[i, 6].Text;
            }
            ssPrint_Sheet1.Cells[2, 0].Text = "작업기간: " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd") + "일";
            ssPrint_Sheet1.Cells[3, 0].Text = "인쇄일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");

            ssPrint_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1;
            ssPrint_Sheet1.PrintInfo.Footer = "/fn\"굴림체\" /fz\"11\" /fs2" + VB.Space(65) + "▶재무회계팀 확인 :";
            ssPrint_Sheet1.PrintInfo.Margin.Left = 10;
            ssPrint_Sheet1.PrintInfo.Margin.Right = 0;
            ssPrint_Sheet1.PrintInfo.Margin.Top = 40;
            ssPrint_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPrint_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.ShowBorder = false;
            ssPrint_Sheet1.PrintInfo.ShowColor = false;
            ssPrint_Sheet1.PrintInfo.ShowGrid = false;
            ssPrint_Sheet1.PrintInfo.ShowShadows = false;
            ssPrint_Sheet1.PrintInfo.UseMax = false;
            ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrint.PrintSheet(0);

            Cursor.Current = Cursors.Default;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            VIEW_NEW_EXAM_ALL();    //예약검사 포함 2011-09-27 시행
        }

        private void frmPmpaViewDailyTong_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1);
            dtpTDate.Value = dtpFDate.Value;
            btnPrint.Enabled = false;

            ssView2_Sheet1.RowCount = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoIO_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoIO0.Checked == true)
            {
                ssView_Sheet1.Cells[28, 0].Text = "";
                ssView_Sheet1.Cells[29, 0].Text = "";
                ssView_Sheet1.Cells[39, 0].Text = "중 간 납 대 체";
            }
            else
            {
                ssView_Sheet1.Cells[28, 0].Text = "예약 수납+환불";
                ssView_Sheet1.Cells[29, 0].Text = "예 약 비 대 체";
                ssView_Sheet1.Cells[39, 0].Text = "예 약 비 대 체";
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ssView_Sheet1.Cells[e.Row, 0].Text.Trim() == "예약 수납+환불")
                {
                    SQL = "";
                    SQL = " SELECT SUM(AMT27) AMT27,SUM(AMT28) AMT28,SUM(AMT51) AMT51,SUM(AMT52) AMT52 ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE >=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE <=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    ComFunc.MsgBox("예약진찰수납+환불 : " + VB.Val(dt.Rows[0]["AMT27"].ToString().Trim()).ToString("###,###,###,##0") +
                                "   예약검사수납+환불 : " + VB.Val(dt.Rows[0]["AMT51"].ToString().Trim()).ToString("###,###,###,##0") + ComNum.VBLF + ComNum.VBLF +
                                   "합계 : " + (VB.Val(dt.Rows[0]["AMT27"].ToString().Trim()) + VB.Val(dt.Rows[0]["AMT51"].ToString().Trim())).ToString("###,###,###,##0"));
                    dt.Dispose();
                    dt = null;
                }
                else if (ssView_Sheet1.Cells[e.Row, 0].Text.Trim() == "예 약 비 대 체")
                {
                    SQL = "";
                    SQL = " SELECT SUM(AMT27) AMT27,SUM(AMT28) AMT28,SUM(AMT51) AMT51,SUM(AMT52) AMT52 ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE >=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE <=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    ComFunc.MsgBox("예약진찰대체 : " + VB.Val(dt.Rows[0]["AMT28"].ToString().Trim()).ToString("###,###,###,##0") +
                                "   예약검사대체 : " + VB.Val(dt.Rows[0]["AMT52"].ToString().Trim()).ToString("###,###,###,##0") + ComNum.VBLF + ComNum.VBLF +
                                   "합계 : " + (VB.Val(dt.Rows[0]["AMT28"].ToString().Trim()) + VB.Val(dt.Rows[0]["AMT52"].ToString().Trim())).ToString("###,###,###,##0"));
                    dt.Dispose();
                    dt = null;
                }



            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void dtpFDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtpTDate.Focus();
            }
        }

        private void dtpTDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void ssPrint_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }
    }
}
