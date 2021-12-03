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
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= d:\psmh\misu\misubs\misubs06.frm" >> frmSupLbExSTS15.cs 폼이름 재정의" />

    public partial class frmPmPaJewonMisuCheck : Form
    {

        clsPmpaMisu CPM = null;

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        double[,] FnAmt = new double[19, 7];

        public frmPmPaJewonMisuCheck()
        {
            InitializeComponent();
        }

        private void frmPmPaJewonMisuCheck_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


            ComFunc CF = new ComFunc();
            clsPmpaFunc CPF = new clsPmpaFunc();
            clsPmpaMisu CPM = new clsPmpaMisu();

            dtpFdate.Value = Convert.ToDateTime(strDTP).AddDays(-1);
            dtpTdate.Value = Convert.ToDateTime(strDTP).AddDays(-1);
            btnPrint.Enabled = false;

            this.Size = new Size(936, 590);
            panError.Visible = false;
            SS2_Sheet1.Columns[0].Label = "등록번호";

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            int j = 0;
            string strYYMM = "";
            string strYYMM_1 = "";
            string strFDate = "";
            string strTdate = "";
            string strFDate_1 = "";
            string strMSG = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            panError.Visible = false;
            SS1_Sheet1.Visible = true;

            strFDate = dtpFdate.Value.ToString("yyyy-MM-dd");
            strFDate_1 = Convert.ToDateTime(strFDate).AddDays(-1).ToString("yyyy-MM-dd");
            strTdate = dtpTdate.Value.ToString("yyyy-MM-dd");
            strYYMM = VB.Left(strFDate, 4) + VB.Mid(strFDate, 6, 2);
            strYYMM_1 = VB.Left(dtpFdate.Value.AddMonths(-1).ToString("yyyy-MM-dd"), 4) + VB.Mid(dtpFdate.Value.AddMonths(-1).ToString("yyyy-MM-dd"), 6, 2);

            btnjewonmisuError.Enabled = false;
            btnMidError.Enabled = false;
            btnmidErrorCH.Enabled = false;

            btnPrint.Enabled = true;

            //'누적할 배열을 Clear
            for (i = 1; i <= 18; i++)
            {
                for (j = 1; j <= 6; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }
            try
            {
                CmdView_Jenil_Amt(strYYMM_1, strYYMM, strFDate_1);     //'①전일금액을 계산
                CmdView_SuipAmt(strFDate, strTdate);
                //'④당일잔액 계산
                for (i = 1; i <= 18; i++)
                {
                    FnAmt[i, 4] = FnAmt[i, 1] + FnAmt[i, 2] - FnAmt[i, 3];
                }

                CmdView_Sil_JewonAmt_new(strTdate); //'⑤실재원미수금 계산

                //'⑥차액을 계산
                for (i = 1; i <= 18; i++)
                //'2005/08월까지는 임시로 중간납,중간청구 잔액점검 안함(이양재) ★★★★ 확인 해야됨
                {
                    if (dtpFdate.Value <= Convert.ToDateTime("2005-08-31"))
                    {
                        FnAmt[i, 5] = FnAmt[i, 4];
                    }

                    FnAmt[i, 6] = FnAmt[i, 4] - FnAmt[i, 5];

                    if (FnAmt[i, 6] != 0)
                    {
                        if (i <= 6)
                        {
                            btnjewonmisuError.Enabled = true;
                        }
                        else if (i <= 12)
                        {
                            i = i;
                            btnMidError.Enabled = true;
                        }
                        else
                        {
                            btnmidErrorCH.Enabled = true;
                        }
                    }
                }

                //'합계를 계산
                for (i = 1; i <= 6; i++)
                {
                    for (j = 1; j <= 5; j++)
                    {
                        FnAmt[6, i] = FnAmt[6, i] + FnAmt[j, i];
                        FnAmt[12, i] = FnAmt[12, i] + FnAmt[j + 6, i];
                        FnAmt[18, i] = FnAmt[18, i] + FnAmt[j + 12, i];
                    }
                }

                //'금액을 Display
                for (i = 1; i <= 18; i++)
                {
                    for (j = 1; j <= 6; j++)
                    {
                        SS1_Sheet1.Cells[i - 1, j + 1].Text = VB.Format(FnAmt[i, j], "###,###,###,##0");
                    }
                }

                Cursor.Current = Cursors.WaitCursor;

               // clsDB.setBeginTran(clsDB.DbCon);

                if ((btnjewonmisuError.Enabled == false && btnMidError.Enabled == false && btnmidErrorCH.Enabled == false) || dtpFdate.Value.ToString("yyyy-MM-dd") == "2005-08-06" || dtpFdate.Value.ToString("yyyy-MM-dd") == "2005-08-07")
                {
                    SQL = "";
                    SQL = "UPDATE ETC_AUTOMAGAM SET BTongCheck='Y' ";
                    SQL = SQL + ComNum.VBLF + "WHERE JobDate>=TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND JobDate<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND (BTongCheck IS NULL OR BTongCheck <> 'Y') ";

                    //clsDB.setCommitTran(clsDB.DbCon);
                    //SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                //'중간청구액 대체 오류 점검

                SQL = "";
                SQL = "CREATE OR REPLACE VIEW VIEW_JUNGGAN_CHECK AS";
                SQL = SQL + ComNum.VBLF + "  SELECT  A.ACTDATE JOBDATE ,A.PANO, A.BI, A.SUBI, SUM(A.JUNGGAN  ) JUNGGAN, 0 BUILDJAMT,  SUM( A.JUNGGAN + A.JOHAP )  JAMT ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALTEWON A ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDate>=TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ACTDate<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND GUBUN ='1' ";//  '입원쪽만 점검 3은 제외함 - 응급실6시간 2010-07-01 윤조연
                SQL = SQL + ComNum.VBLF + "  GROUP BY  A.ACTDATE ,A.PANO, A.BI, A.SUBI";
                SQL = SQL + ComNum.VBLF + "   Union";
                SQL = SQL + ComNum.VBLF + "  SELECT  A.TDATE JOBDATE ,A.PANO, A.BI, A.SUBI,  0 JUNGGAN , SUM(A.BUILDJAMT)  BUILDJAMT,  0 JAMT";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MIR_IPDID A";
                SQL = SQL + ComNum.VBLF + " WHERE TDate>=TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND TDate<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.BUILDJAMT <> 0";
                SQL = SQL + ComNum.VBLF + " GROUP BY  A.TDATE ,A.PANO, A.BI, A.SUBI ";
                SQL = SQL + ComNum.VBLF + "   Union";
                SQL = SQL + ComNum.VBLF + " SELECT  ACTDATE JOBDATE , PANO, BI, ";
                SQL = SQL + ComNum.VBLF + "         DECODE( BI, '11','1','12','1','13','1','32','1','41','1','42','1','43','1','44','1','21','2','22','2','23','2','24','2','31','3','32','3','33','3','52','4','5') SUBI, ";
                SQL = SQL + ComNum.VBLF + "         0 JUNGGAN, 0 BUILDAMT,  SUM(AMT53 + AMT52 + NVL(AMT64,0) ) * -1  JAMT ";//  '조합부담금 + 희귀난치 지원금 + 약제상한액
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDate>=TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ACTDate<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY ACTDATE, PANO, BI ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = "SELECT JOBDATE, PANO, BI, SUBI, SUM(JUNGGAN) - SUM(BUILDJAMT)  CHAMT, SUM(JAMT) JAMT ";
                SQL = SQL + ComNum.VBLF + "  FROM VIEW_JUNGGAN_CHECK ";
                SQL = SQL + ComNum.VBLF + " GROUP BY JOBDATE, PANO, BI, SUBI";
                SQL = SQL + ComNum.VBLF + " Having Sum(JUNGGAN) - Sum(BUILDJAMT) <> 0 OR  SUM(JAMT) <> 0 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strMSG = "<<  중간 청구 대체 오류 확인바랍니다. >>";
                    strMSG = strMSG + "==========================================";
                    strMSG = strMSG + " 일자   등록번호    자격    차이금액";
                    strMSG = strMSG + "==========================================";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strMSG = strMSG + dt.Rows[i]["JOBDATE"].ToString().Trim() + "    " + dt.Rows[i]["PANO"].ToString().Trim() + "  " + dt.Rows[i]["BI"].ToString().Trim() + "  " + Convert.ToDouble(dt.Rows[i]["CHAMT"].ToString().Trim()) + Convert.ToDouble(dt.Rows[i]["JAMT"].ToString().Trim());
                    }
                    strMSG = strMSG + "==========================================";
                    ComFunc.MsgBox(strMSG, "확인");
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "    SELECT ACTDATE, PANO , BI, COUNT(*) CNT FROM ADMIN.MISU_BALTEWON ";
                SQL = SQL + ComNum.VBLF + " WHERE  GUBUN ='3' ";
                SQL = SQL + ComNum.VBLF + "   AND  ACTDate>=TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND  ACTDate<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " GROUP  BY ACTDATE, PANO, BI ";
                SQL = SQL + ComNum.VBLF + " HAVING COUNT(*) > 1 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strMSG = "<<  응급실 6시간 2중 발생오류 확인바랍니다. >>";
                    strMSG = strMSG + "==========================================";
                    strMSG = strMSG + " 일자   등록번호    자격    차이금액";
                    strMSG = strMSG + "==========================================";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strMSG = strMSG + dt.Rows[i]["ACTDATE"].ToString().Trim() + "    " + dt.Rows[i]["PANO"].ToString().Trim() + "  " + dt.Rows[i]["BI"].ToString().Trim();
                    }
                    strMSG = strMSG + "==========================================";
                    ComFunc.MsgBox(strMSG, "확인");
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        #region 함수 모음

        /// <summary>
        /// ①전일금액을 계산
        /// </summary>
        private void CmdView_Jenil_Amt(string strYYMM_1, string strYYMM, string strFDate_1)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int j = 0;

            SQL = "";
            SQL = " SELECT SuBi,SUM(TotAmt) TotAmt,SUM(IpgumAmt) IpgumAmt,";
            SQL = SQL + ComNum.VBLF + "       SUM(JungAmt) JungAmt ";
            SQL = SQL + ComNum.VBLF + "  FROM MISU_BALJEWON ";
            SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM_1 + "' ";
            SQL = SQL + ComNum.VBLF + "   AND Pano<>'81000004' ";
            SQL = SQL + ComNum.VBLF + " GROUP BY SuBi ";
            SQL = SQL + ComNum.VBLF + " ORDER BY SuBi ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                j = (int)VB.Val(dt.Rows[i]["SuBi"].ToString().Trim());
                FnAmt[j, 1] = FnAmt[j, 1] + Convert.ToDouble(dt.Rows[i]["TotAmt"].ToString().Trim()); //'총진료비
                FnAmt[7, 1] = FnAmt[7, 1] + Convert.ToDouble(dt.Rows[i]["IpgumAmt"].ToString().Trim()); //'중간납+보증금
                FnAmt[j + 12, 1] = FnAmt[j + 12, 1] + Convert.ToDouble(dt.Rows[i]["JungAmt"].ToString().Trim()); //'중간청구액
            }

            dt.Dispose();
            dt = null;


            //'당월1일부터 어제까지의 중간납, 중간청구액을 계산
            SQL = "";
            SQL = " SELECT BiGbn,SUM(Amt26-Amt31) Amt26, SUM(AMT29+Amt30-Amt36) Amt29,";
            SQL = SQL + ComNum.VBLF + "        SUM(AMT45) Amt45,SUM(Amt32) Amt32 ";
            SQL = SQL + ComNum.VBLF + "  FROM MISU_BALDAILY ";
            SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >= TO_DATE('" + strYYMM + "01','YYYYMMDD') ";
            SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + strFDate_1 + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND IpdOpd='I' ";
            SQL = SQL + ComNum.VBLF + " GROUP BY BiGbn ";
            SQL = SQL + ComNum.VBLF + " ORDER BY BiGbn ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                j = (int)VB.Val(dt.Rows[i]["BiGbn"].ToString().Trim());
                FnAmt[j, 1] = FnAmt[j, 1] + Convert.ToDouble(dt.Rows[i]["Amt26"].ToString().Trim()); //'총진료비
                FnAmt[7, 1] = FnAmt[7, 1] + Convert.ToDouble(dt.Rows[i]["Amt29"].ToString().Trim()); //'중간납+보증금
                FnAmt[j + 12, 1] = FnAmt[j + 12, 1] + Convert.ToDouble(dt.Rows[i]["Amt45"].ToString().Trim()); //'중간청구액
                FnAmt[j + 12, 1] = FnAmt[j + 12, 1] - Convert.ToDouble(dt.Rows[i]["Amt32"].ToString().Trim()); //'중간청구 대체액
            }

            dt.Dispose();
            dt = null;

        }

        /// <summary>
        /// ②당일발생액,③퇴원대체액
        /// </summary>
        /// <param name="strFDate"></param>
        /// <param name="strTdate"></param>
        private void CmdView_SuipAmt(string strFDate, string strTdate)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int j = 0;


            SQL = "";
            SQL = "SELECT ACTDATE, BIGBN, SUM(AMT26) AMT26, SUM(AMT31) AMT31,";
            SQL = SQL + ComNum.VBLF + " SUM(AMT29+Amt30) AMT29, SUM(AMT36) AMT36,";
            SQL = SQL + ComNum.VBLF + " SUM(AMT32) Amt32,SUM(AMT45) Amt45 ";
            SQL = SQL + ComNum.VBLF + " FROM MISU_BALDAILY ";
            SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND IpdOpd='I' ";
            SQL = SQL + ComNum.VBLF + " GROUP BY ACTDATE, BIGBN ";
            SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE, BIGBN ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                switch (Convert.ToInt32(dt.Rows[i]["BIGBN"].ToString().Trim()))
                {
                    case 1:
                        j = 1;  //'보험
                        break;
                    case 2:
                        j = 2;  //'보호
                        break;
                    case 3:
                        j = 3;  //'산재
                        break;
                    case 4:
                        j = 4;  //'자보
                        break;
                    default:
                        j = 5;  //'기타는 보험으로
                        break;
                }
                FnAmt[j, 2] = FnAmt[j, 2] + Convert.ToDouble(dt.Rows[i]["Amt26"].ToString().Trim());
                FnAmt[j, 3] = FnAmt[j, 3] + Convert.ToDouble(dt.Rows[i]["AMT31"].ToString().Trim());

                FnAmt[7, 2] = FnAmt[7, 2] + Convert.ToDouble(dt.Rows[i]["Amt29"].ToString().Trim());     //'보증금+중간납 발생액
                FnAmt[7, 3] = FnAmt[7, 3] + Convert.ToDouble(dt.Rows[i]["AMT36"].ToString().Trim());     //'퇴원자 보증금대체액

                FnAmt[j + 12, 2] = FnAmt[j + 12, 2] + Convert.ToDouble(dt.Rows[i]["AMT45"].ToString().Trim());   //'당일 중간청구 Build
                FnAmt[j + 12, 3] = FnAmt[j + 12, 3] + Convert.ToDouble(dt.Rows[i]["AMT32"].ToString().Trim());   //'퇴원자 중간청구액 대체

            }

            dt.Dispose();
            dt = null;

        }

        /// <summary>
        /// ⑤실당일잔액 계산
        /// </summary>
        private void CmdView_Sil_JewonAmt_new(string strTdate)
        {
            // '총진료비 실잔액을 구함
            clsPmpaMisu CPM = new clsPmpaMisu();

            string SQL = "";
            DataTable dt = null;
            DataTable dtRs2 = null;
            string SqlErr = "";
            int i = 0;
            int j = 0;
            int K = 0;


            SQL = "";
            SQL = "SELECT IPDNO, TRSNO  FROM IPD_TRANS ";
            SQL = SQL + ComNum.VBLF + " WHERE (ActDate IS NULL OR ActDate>TO_DATE('" + strTdate + "','YYYY-MM-DD')) ";
            SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            progressBar1.Value = 0;
            progressBar1.Maximum = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                progressBar1.Value = (i + 1);
                SQL = "";
                SQL = " SELECT BI, SUM(Amt1+Amt2) AMT50 ";
                SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_SLIP  ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + dt.Rows[i]["IPDNO"].ToString().Trim() + "";
                SQL = SQL + ComNum.VBLF + "   AND TRSNO = " + dt.Rows[i]["TRSNO"].ToString().Trim() + "";
                SQL = SQL + ComNum.VBLF + "   AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY BI ";

                SqlErr = clsDB.GetDataTable(ref dtRs2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                for (K = 0; K < dtRs2.Rows.Count; K++)
                {
                    switch (CPM.READ_Bi_SuipTong(dtRs2.Rows[K]["bi"].ToString().Trim(), strTdate))
                    {
                        case 1:
                            j = 1;//     '보험
                            break;
                        case 2:
                            j = 2;//     '보호
                            break;
                        case 3:
                            j = 3;//     '산재
                            break;
                        case 4:
                            j = 4;//     '자보
                            break;
                        default:
                            j = 5;//     '일반
                            break;
                    }
                    FnAmt[j, 5] = FnAmt[j, 5] + Convert.ToDouble(dtRs2.Rows[K]["Amt50"].ToString().Trim());
                }

                dtRs2.Dispose();
                dtRs2 = null;
            }

            dt.Dispose();
            dt = null;

            //'중간납 잔액을 계산
            SQL = "";
            SQL = " SELECT b.BI,b.Bun,SUM(b.Amt) Amt ";
            SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_MASTER a,IPD_NEW_CASH b ";
            SQL = SQL + ComNum.VBLF + " WHERE (a.ACTDATE IS NULL OR a.ActDate>TO_DATE('" + strTdate + "','YYYY-MM-DD'))  ";
            SQL = SQL + ComNum.VBLF + "   AND a.Pano <> '81000004' ";
            SQL = SQL + ComNum.VBLF + "   AND a.IPDNO=b.IPDNO(+) ";
            SQL = SQL + ComNum.VBLF + "   AND b.Bun IN ('85','87','88') ";
            SQL = SQL + ComNum.VBLF + "   AND b.ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + " GROUP BY b.BI,b.Bun ";
            SQL = SQL + ComNum.VBLF + " ORDER BY b.BI,b.Bun ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                switch (CPM.READ_Bi_SuipTong(dt.Rows[i]["bi"].ToString().Trim(), strTdate))
                {
                    case 1:
                        j = 1;//     '보험
                        break;
                    case 2:
                        j = 2;//     '보호
                        break;
                    case 3:
                        j = 3;//     '산재
                        break;
                    case 4:
                        j = 4;//     '자보
                        break;
                    default:
                        j = 5;//     '일반
                        break;
                }

                if (dt.Rows[i]["Bun"].ToString().Trim() != "88")
                {
                    FnAmt[7, 5] = FnAmt[7, 5] + Convert.ToDouble(dt.Rows[i]["Amt"].ToString().Trim());
                }
                else
                {
                    FnAmt[7, 5] = FnAmt[7, 5] - Convert.ToDouble(dt.Rows[i]["Amt"].ToString().Trim());
                }
            }
            dt.Dispose();
            dt = null;


            //'중간청구 미대체 잔액을 계산
            SQL = "";
            SQL = " SELECT b.BI,SUM(b.BuildJAmt) Amt ";
            SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_MASTER a,MIR_IPDID b ";
            SQL = SQL + ComNum.VBLF + " WHERE (a.ACTDATE IS NULL OR a.ActDate>TO_DATE('" + strTdate + "','YYYY-MM-DD'))  ";
            SQL = SQL + ComNum.VBLF + "   AND a.Pano <> '81000004' ";
            SQL = SQL + ComNum.VBLF + "   AND a.IPDNO=b.IPDNO(+) ";
            SQL = SQL + ComNum.VBLF + "   AND b.Flag = '1' ";
            SQL = SQL + ComNum.VBLF + "   AND b.BuildDate<=TO_DATE('" + strTdate + " 23:59','YYYY-MM-DD HH24:MI') ";
            SQL = SQL + ComNum.VBLF + "   AND (b.TDate IS NULL OR b.TDate>TO_DATE('" + strTdate + "','YYYY-MM-DD')) ";
            SQL = SQL + ComNum.VBLF + " GROUP BY b.BI  ";
            SQL = SQL + ComNum.VBLF + " ORDER BY b.BI  ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {

                switch (CPM.READ_Bi_SuipTong(dt.Rows[i]["bi"].ToString().Trim(), strTdate))
                {
                    case 1:
                        j = 1;//     '보험
                        break;
                    case 2:
                        j = 2;//     '보호
                        break;
                    case 3:
                        j = 3;//     '산재
                        break;
                    case 4:
                        j = 4;//     '자보
                        break;
                    default:
                        j = 5;//     '일반
                        break;
                }
                FnAmt[j + 12, 5] = FnAmt[j + 12, 5] + Convert.ToDouble(dt.Rows[i]["Amt"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            CPM = null;

        }
        #endregion

        private void chkCheckSheetDelete_CheckedChanged(object sender, EventArgs e)
        {
            int i = 0;

            if (chkCheckSheetDelete.Checked == false)
            {
                return;
            }

            for (i = SS2_Sheet1.RowCount; i > 1; i--)
            {
                if (Convert.ToBoolean(SS2_Sheet1.Cells[i - 1, 8].Value) == false)
                {
                    SS2_Sheet1.RemoveRows(i - 1, 8);
                    SS2_Sheet1.RowCount = SS2_Sheet1.RowCount - 1;
                }


            }
        }

        private void btnjewonmisuError_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            int j = 0;
            int nRow = 0;
            string strFDate = "";
            string strFDate_1 = "";
            string strTdate = "";
            string strYYMM = "";
            string strYYMM_1 = "";
            double[] nAmt = new double[7];
            double[] nTotAmt = new double[7];
            string strPano = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;
            DataTable dtRs1 = null;

            SS1_Sheet1.Visible = false;
            this.Size = new Size(936, 750);
            panError.Visible = true;
            SS2_Sheet1.Columns[0].Label = "TRSNO";
            SS2_Sheet1.RowCount = 30;
            SS2_Sheet1.RowCount = 1;

            for (j = 1; j <= 6; j++)
            {
                nTotAmt[j] = 0;
            }

            strFDate = dtpFdate.Value.ToString("yyyy-MM-dd");
            strFDate_1 = Convert.ToDateTime(strFDate).AddDays(-1).ToString("yyyy-MM-dd");
            strTdate = dtpTdate.Value.ToString("yyyy-MM-dd");
            strYYMM = VB.Left(strFDate, 4) + VB.Mid(strFDate, 6, 2);
            strYYMM_1 = VB.Left(dtpFdate.Value.AddMonths(-1).ToString("yyyy-MM-dd"), 4) + VB.Mid(dtpFdate.Value.AddMonths(-1).ToString("yyyy-MM-dd"), 6, 2); 

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'전월말현재 잔액
                SQL = "";
                SQL = "CREATE OR REPLACE VIEW VIEW_JEWON_CHECK ";
                SQL = SQL + ComNum.VBLF + " (IPDNO,TRSNO,Pano,Bi,IwolAmt,BalAmt, DaeAmt, SilJan) as       ";
                SQL = SQL + ComNum.VBLF + " SELECT IPDNO,TRSNO,Pano,Bi,TotAmt IwolAmt,0 BalAmt,0 DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_BALJEWON ";
                SQL = SQL + ComNum.VBLF + "  WHERE YYMM='" + strYYMM_1 + "' ";
                SQL = SQL + ComNum.VBLF + "    AND PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "    AND TotAmt <> 0 ";
                //'시작일부터 어제까지 발생금액
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT IPDNO,TRSNO,Pano, Bi,(Amt1+Amt2) IwolAmt,0 BalAmt,0 DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + strYYMM + "01','YYYYMMDD') ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate <  TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND PANO <> '81000004' ";
                //'시작일부터 어제까지 퇴원자 대체액
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT IPDNO,TRSNO,Pano, Bi,(TotAmt*-1) IwolAmt,0 BalAmt,0 DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + strYYMM + "01','YYYYMMDD') ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate <  TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + " AND TotAmt <> 0 ";
                //'당일 수입발생액
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT IPDNO,TRSNO,Pano, Bi,0 IwolAmt,(Amt1+Amt2) BalAmt,0 DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND PANO <> '81000004' ";
                //'퇴원자 총진료비
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT IPDNO,TRSNO,Pano, Bi,0 IwolAmt,0 BalAmt,TotAmt DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Gubun='1' ";
                SQL = SQL + ComNum.VBLF + "   AND PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND TotAmt <> 0 ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }


                //'자료를 SELECT
                SQL = "";
                SQL = "         SELECT a.IPDNO,a.TRSNO,b.Pano,c.SName,a.Bi,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(b.ActDate,'YYYY-MM-DD') ActDate,";
                SQL = SQL + ComNum.VBLF + "       SUM(a.IwolAmt) IwolAmt,SUM(a.BalAmt) BalAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(a.DaeAmt) DaeAmt ";
                SQL = SQL + ComNum.VBLF + "  FROM VIEW_JEWON_CHECK a," + ComNum.DB_PMPA + "IPD_TRANS b," + ComNum.DB_PMPA + "IPD_NEW_MASTER c ";
                SQL = SQL + ComNum.VBLF + " WHERE a.TRSNO=b.TRSNO(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.IPDNO=c.IPDNO(+) ";
                SQL = SQL + ComNum.VBLF + " GROUP BY a.IPDNO,a.TRSNO,b.Pano,c.SName,a.Bi,b.ActDate ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.IPDNO,a.TRSNO,b.Pano,c.SName,a.Bi ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    
                    if (i == 1317)
                    {
                        strPano = strPano;
                    }

                    nAmt[1] = VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());
                    nAmt[2] = VB.Val(dt.Rows[i]["BalAmt"].ToString().Trim());
                    nAmt[3] = VB.Val(dt.Rows[i]["DaeAmt"].ToString().Trim());
                    nAmt[4] = nAmt[1] + nAmt[2] - nAmt[3];

                    if (dt.Rows[i]["ActDate"].ToString().Trim() != "" && string.Compare(dt.Rows[i]["ActDate"].ToString().Trim(), strTdate) <= 0)
                    {
                        nAmt[5] = 0;
                        nAmt[6] = nAmt[4] - nAmt[5];
                    }
                    else if (nAmt[4] == 0)
                    {
                        nAmt[5] = 0;
                        nAmt[6] = nAmt[4] - nAmt[5];
                    }
                    else
                    {//'실잔액을 계산
                        SQL = "";
                        SQL = "SELECT SUM(Amt1+Amt2) Amt ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                        SQL = SQL + ComNum.VBLF + "WHERE TRSNO=" + VB.Val(dt.Rows[i]["TRSNO"].ToString().Trim()) + " ";
                        SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.GetDataTable(ref dtRs1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        nAmt[5] = 0;

                        if (dtRs1.Rows.Count > 0)
                        {
                            nAmt[5] = Convert.ToDouble(dtRs1.Rows[0]["Amt"].ToString().Trim());
                            dtRs1.Dispose();
                            dtRs1 = null;

                            nAmt[6] = nAmt[4] - nAmt[5];
                        }
                    }

                    if (nAmt[6] != 0)
                    {
                        nRow = nRow + 1;
                        if (nRow > SS2_Sheet1.RowCount)
                        {
                            SS2_Sheet1.RowCount = nRow;
                        }
                        SS2_Sheet1.Cells[nRow - 1, 0].Text = VB.Val(dt.Rows[i]["TRSNO"].ToString().Trim()).ToString();
                        SS2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Bi"].ToString().Trim();

                        for (j = 1; j <= 6; j++)
                        {
                            SS2_Sheet1.Cells[nRow - 1, j + 2].Text = VB.Format(nAmt[j], "###,###,###,##0");
                        }
                    }
                }
                SS2_Sheet1.RowCount = nRow;

                dt.Dispose();
                dt = null;

                SQL = "DROP VIEW VIEW_JEWON_CHECK ";

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("재원미수 오류 점검 완료");
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnMidError_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            int j = 0;
            int nRow = 0;
            string strFDate = "";
            string strFDate_1 = "";
            string strTdate = "";
            string strYYMM = "";
            string strYYMM_1 = "";
            double[] nAmt = new double[7];
            double[] nTotAmt = new double[7];

            string strPano = "";
            string strBi = "";
            double nJanAmt = 0;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;
            DataTable dtRs1 = null;

            SS1_Sheet1.Visible = false;
            this.Size = new Size(936, 750);
            panError.Visible = true;
            SS2_Sheet1.RowCount = 30;
            SS2_Sheet1.RowCount = 1;

            for (j = 1; j <= 6; j++)
            {
                nTotAmt[j] = 0;
            }

            strFDate = dtpFdate.Value.ToString("yyyy-MM-dd");
            strFDate_1 = Convert.ToDateTime(strFDate).AddDays(-1).ToString("yyyy-MM-dd");
            strTdate = dtpTdate.Value.ToString("yyyy-MM-dd");
            strYYMM = VB.Left(strFDate, 4) + VB.Mid(strFDate, 6, 2);
            strYYMM_1 = Convert.ToDateTime(strYYMM).AddDays(-1).ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'전월말현재 잔액
                SQL = "";
                SQL = "CREATE VIEW VIEW_JEWON_CHECK AS ";
                SQL = SQL + ComNum.VBLF + " SELECT IPDNO,Pano,'11' Bi,IpgumAmt IwolAmt,0 BalAmt,0 DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_BALJEWON ";
                SQL = SQL + ComNum.VBLF + "  WHERE YYMM='" + strYYMM_1 + "' ";
                SQL = SQL + ComNum.VBLF + "    AND PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "    AND IpgumAmt <> 0 ";
                //'시작일부터 어제까지 발생금액(이월액에 +)
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT IPDNO,Pano,'12' Bi,SUM(Amt) IwolAmt,0 BalAmt,0 DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + strYYMM + "01','YYYYMMDD') ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate <  TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND Bun IN ('85','87') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY IPDNO,Pano ";
                //'시작일부터 어제까지 퇴원자 대체액
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT IPDNO,Pano,'13' Bi,(Amt * -1) IwolAmt,0 BalAmt,0 DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + strYYMM + "01','YYYYMMDD') ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate <  TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND Bun = '88' ";
                //'당일 중간납 입금액
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT IPDNO,Pano,'14' Bi,0 IwolAmt,Amt BalAmt,0 DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND Bun IN ('85','87') ";
                SQL = SQL + ComNum.VBLF + " AND PANO <> '81000004' ";
                //'퇴원자 보증금 대체액
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT IPDNO,Pano,'15' Bi,0 IwolAmt,0 BalAmt,Amt DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND Bun = '88' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }


                //'자료를 SELECT
                SQL = "";
                SQL = "         SELECT a.IPDNO,b.Pano,b.SName,SUM(a.IwolAmt) IwolAmt,SUM(a.BalAmt) BalAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(a.DaeAmt) DaeAmt,SUM(SilJan) SilJan ";
                SQL = SQL + ComNum.VBLF + "  FROM VIEW_JEWON_CHECK a," + ComNum.DB_PMPA + "IPD_NEW_MASTER b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.IPDNO=b.IPDNO(+) ";
                SQL = SQL + ComNum.VBLF + " GROUP BY a.IPDNO,b.Pano,b.SName ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.IPDNO,b.Pano,b.SName ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();

                    if (strPano == "03540286")
                    {
                        strPano = strPano;
                    }

                    //'중간청구액 실잔액을 계산함
                    SQL = "";
                    SQL = "SELECT TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate,";
                    SQL = SQL + ComNum.VBLF + " SUM(DECODE(b.Bun,'88',0,b.Amt)) Amt1,SUM(DECODE(b.Bun,'88',b.Amt,0)) Amt2 ";
                    SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER a,IPD_NEW_CASH b ";
                    SQL = SQL + ComNum.VBLF + "WHERE a.IPDNO=" + Convert.ToInt32(dt.Rows[i]["IPDNO"].ToString().Trim()) + " ";
                    ;
                    SQL = SQL + ComNum.VBLF + "  AND a.IPDNO=b.IPDNO(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND b.Bun IN ('85','87','88') ";
                    SQL = SQL + ComNum.VBLF + "  AND b.ActDate <= TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY a.ActDate ";

                    SqlErr = clsDB.GetDataTable(ref dtRs1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    nJanAmt = 0;

                    if (dt.Rows.Count == 0)
                    {
                        nJanAmt = Convert.ToDouble(dtRs1.Rows[0]["AMT1"].ToString().Trim()) - Convert.ToDouble(dtRs1.Rows[0]["AMT2"].ToString().Trim());

                        if (dtRs1.Rows[0]["ActDate"].ToString().Trim() != "" && string.Compare(dtRs1.Rows[0]["ActDate"].ToString().Trim(), strTdate) <= 0)
                        {
                            nJanAmt = 0;
                        }
                    }
                    dtRs1.Dispose();
                    dtRs1 = null;

                    nAmt[1] = Convert.ToDouble(dt.Rows[i]["IwolAmt"].ToString().Trim());
                    nAmt[2] = Convert.ToDouble(dt.Rows[i]["BalAmt"].ToString().Trim());
                    nAmt[3] = Convert.ToDouble(dt.Rows[i]["DaeAmt"].ToString().Trim());
                    nAmt[4] = nAmt[1] + nAmt[2] - nAmt[3];
                    nAmt[5] = nJanAmt;
                    nAmt[6] = nAmt[4] - nAmt[5];

                    if (nAmt[6] != 0)
                    {
                        nRow = nRow + 1;
                        if (nRow > SS2_Sheet1.RowCount)
                        {
                            SS2_Sheet1.RowCount = nRow;
                        }
                        SS2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 2].Text = "";

                        for (j = 1; j <= 6; j++)
                        {
                            SS2_Sheet1.Cells[nRow - 1, j + 2].Text = VB.Format(nAmt[j], "###,###,###,##0");
                            nTotAmt[j] = nTotAmt[j] + nAmt[j];
                        }
                    }
                }

                SS2_Sheet1.Cells[nRow - 1, 0].Text = "** 합계 **";

                for (j = 1; j <= 6; j++)
                {
                    SS2_Sheet1.Cells[nRow - 1, j + 2].Text = VB.Format(nAmt[j], "###,###,###,##0");
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "DROP VIEW VIEW_JEWON_CHECK ";

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("중간납 오류 점검 완료", "확인");
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnmidErrorCH_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            int j = 0;
            int nRow = 0;
            string strFDate = "";
            string strFDate_1 = "";
            string strTdate = "";
            string strYYMM = "";
            string strYYMM_1 = "";
            double[] nAmt = new double[7];
            double[] nTotAmt = new double[7];

            string strPano = "";
            string strBi = "";
            double nJanAmt = 0;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;
            DataTable dtRs1 = null;

            SS1_Sheet1.Visible = false;
            this.Size = new Size(936, 750);
            panError.Visible = true;
            SS2_Sheet1.RowCount = 30;
            SS2_Sheet1.RowCount = 1;

            for (j = 1; j <= 6; j++)
            {
                nTotAmt[j] = 0;
            }

            strFDate = dtpFdate.Value.ToString("yyyy-MM-dd");
            strFDate_1 = Convert.ToDateTime(strFDate).AddDays(-1).ToString("yyyy-MM-dd");
            strTdate = dtpTdate.Value.ToString("yyyy-MM-dd");
            strYYMM = VB.Left(strFDate, 4) + VB.Mid(strFDate, 6, 2);
            strYYMM_1 = Convert.ToDateTime(strYYMM).AddDays(-1).ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'전월말현재 잔액
                SQL = "";
                SQL = "CREATE OR REPLACE VIEW VIEW_JEWON_CHECK AS                            ";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,Bi,JungAmt IwolAmt,0 BalAmt,0 DaeAmt,0 SilJan   ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_BALJEWON                                        ";
                SQL = SQL + ComNum.VBLF + "  WHERE YYMM='" + strYYMM_1 + "'                             ";
                SQL = SQL + ComNum.VBLF + "    AND PANO <> '81000004'                                   ";
                SQL = SQL + ComNum.VBLF + "    AND JungAmt <> 0                                         ";

                //'퇴원자 중간청구 대체액
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT Pano, Bi,0 IwolAmt,0 BalAmt,Junggan DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND ActDate <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + " AND Junggan <> 0 ";
                //'당일 중간청구 Build
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT Pano, Bi,0 IwolAmt,BuildJAmt BalAmt,0 DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MIR_IPDID ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM>='200112' ";
                SQL = SQL + ComNum.VBLF + "   AND BuildDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND BuildDate <= TO_DATE('" + strTdate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND Flag='1' ";
                SQL = SQL + ComNum.VBLF + "   AND PANO <> '81000004' ";
                //'시작일부터 어제까지 중간청구 Build
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT Pano, Bi,BuildJAmt IwolAmt,0 BalAmt,0 DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MIR_IPDID ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM>='200112' ";
                SQL = SQL + ComNum.VBLF + "   AND BuildDate >= TO_DATE('" + strYYMM + "01','YYYYMMDD') ";
                SQL = SQL + ComNum.VBLF + "   AND BuildDate <  TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Flag='1' ";
                SQL = SQL + ComNum.VBLF + " AND PANO <> '81000004' ";
                //'시작일부터 어제까지 퇴원자 중간청구 대체액
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT Pano, Bi,(Junggan*-1) IwolAmt,0 BalAmt,0 DaeAmt,0 SilJan ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + strYYMM + "01','YYYYMMDD') ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate <  TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND PANO <> '81000004' ";
                SQL = SQL + ComNum.VBLF + " AND Junggan <> 0 ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //'자료를 SELECT
                SQL = "";
                SQL = "         SELECT a.Bi,a.Pano,b.SName,SUM(a.IwolAmt) IwolAmt,SUM(a.BalAmt) BalAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(a.DaeAmt) DaeAmt,SUM(SilJan) SilJan ";
                SQL = SQL + ComNum.VBLF + "  FROM VIEW_JEWON_CHECK a," + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Pano=b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + " GROUP BY a.Pano,b.SName,a.Bi ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.Pano,b.SName,a.Bi ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strBi = dt.Rows[i]["Bi"].ToString().Trim();

                    //'중간청구액 실잔액을 계산함
                    SQL = "";
                    SQL = " SELECT SUM(b.BuildJAmt) Amt ";
                    SQL = SQL = ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a," + ComNum.DB_PMPA + "MIR_IPDID b ";
                    SQL = SQL = ComNum.VBLF + " WHERE (a.ACTDATE IS NULL OR a.ActDate>TO_DATE('" + strTdate + "','YYYY-MM-DD'))  ";
                    SQL = SQL = ComNum.VBLF + "   AND a.Pano = '" + strPano + "' ";
                    SQL = SQL = ComNum.VBLF + "   AND a.IPDNO=b.IPDNO(+) ";
                    SQL = SQL = ComNum.VBLF + "   AND b.Flag = '1' ";
                    SQL = SQL = ComNum.VBLF + "   AND b.BuildDate<=TO_DATE('" + strTdate + " 23:59','YYYY-MM-DD HH24:MI') ";
                    SQL = SQL = ComNum.VBLF + "   AND (b.TDate IS NULL OR b.TDate>TO_DATE('" + strTdate + "','YYYY-MM-DD')) ";

                    SqlErr = clsDB.GetDataTable(ref dtRs1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    nJanAmt = 0;

                    if (dt.Rows.Count == 0)
                    {
                        nJanAmt = Convert.ToDouble(dtRs1.Rows[0]["AMT1"].ToString().Trim()) - Convert.ToDouble(dtRs1.Rows[0]["AMT2"].ToString().Trim());

                        if (dtRs1.Rows[0]["ActDate"].ToString().Trim() != "" && string.Compare(dtRs1.Rows[0]["ActDate"].ToString().Trim(), strTdate) <= 0)
                        {
                            nJanAmt = 0;
                        }
                    }
                    dtRs1.Dispose();
                    dtRs1 = null;

                    nAmt[1] = Convert.ToDouble(dt.Rows[i]["IwolAmt"].ToString().Trim());
                    nAmt[2] = Convert.ToDouble(dt.Rows[i]["BalAmt"].ToString().Trim());
                    nAmt[3] = Convert.ToDouble(dt.Rows[i]["DaeAmt"].ToString().Trim());
                    nAmt[4] = nAmt[1] + nAmt[2] - nAmt[3];
                    nAmt[6] = nAmt[4] - nAmt[5];

                    if (nAmt[6] != 0)
                    {
                        nRow = nRow + 1;
                        if (nRow > SS2_Sheet1.RowCount)
                        {
                            SS2_Sheet1.RowCount = nRow;
                        }
                        SS2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["bi"].ToString().Trim();

                        for (j = 1; j <= 6; j++)
                        {
                            SS2_Sheet1.Cells[nRow - 1, j + 2].Text = VB.Format(nAmt[j], "###,###,###,##0");
                            nTotAmt[j] = nTotAmt[j] + nAmt[j];
                        }
                    }
                }

                SS2_Sheet1.Cells[nRow - 1, 0].Text = "** 합계 **";

                for (j = 1; j <= 6; j++)
                {
                    SS2_Sheet1.Cells[nRow - 1, j + 2].Text = VB.Format(nAmt[j], "###,###,###,##0");
                }

                dt.Dispose();
                dt = null;

                SQL = "DROP VIEW VIEW_JEWON_CHECK ";

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("중간청구 오류 점검 완료", "확인");
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnmidCHJumgum_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            string strMSG = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

           // clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                //'중간청구액 대체 오류 점검

                SQL = "CREATE OR REPLACE VIEW VIEW_JUNGGAN_CHECK AS";
                SQL = SQL + ComNum.VBLF + "  SELECT  A.ACTDATE JOBDATE ,A.PANO, A.BI, A.SUBI, SUM(A.JUNGGAN  ) JUNGGAN, 0 BUILDJAMT,  SUM( A.JUNGGAN + A.JOHAP )  JAMT ";
                SQL = SQL + ComNum.VBLF + "  FROM MISU_BALTEWON A ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDate>=TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ACTDate<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND GUBUN ='1' ";  //'입원쪽만 점검 3은 제외함 - 응급실6시간 2010-07-01 윤조연
                SQL = SQL + ComNum.VBLF + "  GROUP BY  A.ACTDATE ,A.PANO, A.BI, A.SUBI";
                SQL = SQL + ComNum.VBLF + "   Union";
                SQL = SQL + ComNum.VBLF + "  SELECT  A.TDATE JOBDATE ,A.PANO, A.BI, A.SUBI,  0 JUNGGAN , SUM(A.BUILDJAMT)  BUILDJAMT,  0 JAMT";
                SQL = SQL + ComNum.VBLF + "  FROM MIR_IPDID A";
                SQL = SQL + ComNum.VBLF + " WHERE TDate>=TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND TDate<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.BUILDJAMT <> 0";
                SQL = SQL + ComNum.VBLF + " GROUP BY  A.TDATE ,A.PANO, A.BI, A.SUBI ";
                SQL = SQL + ComNum.VBLF + "   Union";
                SQL = SQL + ComNum.VBLF + " SELECT  ACTDATE JOBDATE , PANO, BI, ";
                SQL = SQL + ComNum.VBLF + "         DECODE( BI, '11','1','12','1','13','1','32','1','41','1','42','1','43','1','44','1','21','2','22','2','23','2','24','2','31','3','32','3','33','3','52','4','5') SUBI, ";
                SQL = SQL + ComNum.VBLF + "         0 JUNGGAN, 0 BUILDAMT,  SUM(AMT53 + AMT52 + NVL(AMT64,0) ) * -1  JAMT ";
                SQL = SQL + ComNum.VBLF + "  FROM IPD_TRANS ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDate>=TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ACTDate<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY ACTDATE, PANO, BI ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                 //   clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "SELECT JOBDATE, PANO, BI, SUBI, SUM(JUNGGAN) - SUM(BUILDJAMT)  CHAMT, SUM(JAMT) JAMT ";
                SQL = SQL + ComNum.VBLF + "  FROM VIEW_JUNGGAN_CHECK ";
                SQL = SQL + ComNum.VBLF + " GROUP BY JOBDATE, PANO, BI, SUBI";
                SQL = SQL + ComNum.VBLF + " Having Sum(JUNGGAN) - Sum(BUILDJAMT) <> 0 OR  SUM(JAMT) <> 0 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strMSG = "<<  중간 청구 대체 오류 확인바랍니다. >>";
                    strMSG = strMSG + "==========================================";
                    strMSG = strMSG + " 일자   등록번호    자격    차이금액";
                    strMSG = strMSG + "==========================================";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strMSG = strMSG + dt.Rows[i]["JOBDATE"].ToString().Trim() + "    " + dt.Rows[i]["PANO"].ToString().Trim() + "  " + dt.Rows[i]["BI"].ToString().Trim() + "  " + Convert.ToDouble(dt.Rows[i]["CHAMT"].ToString().Trim()) + Convert.ToDouble(dt.Rows[i]["JAMT"].ToString().Trim());
                    }
                    strMSG = strMSG + "==========================================";
                    ComFunc.MsgBox(strMSG, "확인");
                }

                ComFunc.MsgBox("점검 완료  오류건수: ( 0 )", "확인");
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
             //   clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //프린트 버튼
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            strTitle = "일자별 재원미수금 점검표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("등록기간 : " + dtpFdate.Text + " ~ " + dtpTdate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}