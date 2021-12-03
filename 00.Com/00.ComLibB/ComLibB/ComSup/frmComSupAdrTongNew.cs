using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmComSupAdrTongNew : Form
    {
        public frmComSupAdrTongNew()
        {
            InitializeComponent();
        }

        private void frmComSupAdrTongNew_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpSDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddMonths(-1);
            dtpEDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            ssView_Sheet1.RowCount = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        
        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (clsSpread CS = new clsSpread())
            {
                CS.ExportToXLS(ssView);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {            
            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            //strFont1 = "/fn\"맑은 고딕\" /fz\"17\" /fb1 /fi0 /fu0 /fk0 /fs1";
            //strFont2 = "/fn\"맑은 고딕\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";

            //strHead1 = "/c/f1" + "원내약품 수가결정 요청서" + "/f1/n";
            //strHead2 = "/l/f2" + "※ 문서번호 : " + VB.Left(GstrJDATE, 4) + "-" + GstrWRTNO + "/f2/n";
            //strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 작성일자 : " + GstrJDATE + "/f2/n";
            //strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 작 성 자 : " + GstrJNAME + "/f2/n";
            //strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 약제팀장확인 : " + GstrYDATE + "/f2/n";
            //strHead2 = strHead2 + "/l/f2" + VB.Space(11) + "※ 보험심사팀님확인 : " + GstrSDATE + "/f2/n";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.7f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 90;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.Margin.Header = 10;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetData()
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;
            int j = 0;
            //int iRow = 0;
            //int jRow = 0;

            string strSEQ = "";
            string strTemp = "";
            string strSDate = dtpSDATE.Value.ToShortDateString();
            string strEDate = dtpEDATE.Value.ToShortDateString();

            ssView_Sheet1.RowCount = 0;

            try
            {
                #region // 보고서 리스트 조회 쿼리
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT T1.SEQNO,                                                                         ";
                SQL = SQL + ComNum.VBLF + "       T1.BUNCODE,                                                                       ";    
                SQL = SQL + ComNum.VBLF + "       T1.CLASSNAME,                                                                     ";
                SQL = SQL + ComNum.VBLF + "       T2.BUNRANK,                                                                       ";
                SQL = SQL + ComNum.VBLF + "       T1.SUCODE,                                                                        ";
                SQL = SQL + ComNum.VBLF + "       T3.CODERANK,                                                                      ";
                SQL = SQL + ComNum.VBLF + "       T1.SUNAMEK,                                                                       ";
                SQL = SQL + ComNum.VBLF + "       T1.SNAME,                                                                         ";
                SQL = SQL + ComNum.VBLF + "       T1.PTNO,                                                                          ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(T1.WDATE, 'YYYY-MM-DD') WDATE                                             ";
                SQL = SQL + ComNum.VBLF + "  FROM(SELECT A.SEQNO,                                                                   ";
                SQL = SQL + ComNum.VBLF + "               C.BUNCODE,                                                                ";
                SQL = SQL + ComNum.VBLF + "               D.CLASSNAME,                                                              ";
                SQL = SQL + ComNum.VBLF + "               B.SUCODE,                                                                 ";
                SQL = SQL + ComNum.VBLF + "               B.SUNAMEK,                                                                ";
                SQL = SQL + ComNum.VBLF + "               A.SNAME,                                                                  ";
                SQL = SQL + ComNum.VBLF + "               A.PTNO,                                                                   ";
                SQL = SQL + ComNum.VBLF + "               WDATE                                                                     ";
                SQL = SQL + ComNum.VBLF + "          FROM ADMIN.DRUG_ADR1 A                                                    ";
                SQL = SQL + ComNum.VBLF + "               INNER JOIN ADMIN.DRUG_ADR1_ORDER B                                   ";
                SQL = SQL + ComNum.VBLF + "                       ON A.SEQNO = B.SEQNO                                              ";
                SQL = SQL + ComNum.VBLF + "               INNER JOIN ADMIN.OCS_DRUGINFO_NEW C                                  ";
                SQL = SQL + ComNum.VBLF + "                       ON B.SUCODE = C.SUNEXT                                            ";
                SQL = SQL + ComNum.VBLF + "               INNER JOIN ADMIN.BAS_CLASS D                                        ";
                SQL = SQL + ComNum.VBLF + "                       ON C.BUNCODE = D.CLASSCODE                                        ";
                SQL = SQL + ComNum.VBLF + "         WHERE A.WDATE >= To_date('" + strSDate + "', 'YYYY-MM-DD')                      ";
                SQL = SQL + ComNum.VBLF + "           AND A.WDATE <= To_date('" + strEDate + "', 'YYYY-MM-DD')                      ";
                SQL = SQL + ComNum.VBLF + "        UNION ALL                                                                        ";
                SQL = SQL + ComNum.VBLF + "        SELECT A.SEQNO,                                                                  ";
                SQL = SQL + ComNum.VBLF + "               C.BUNCODE,                                                                ";
                SQL = SQL + ComNum.VBLF + "               D.CLASSNAME,                                                              ";
                SQL = SQL + ComNum.VBLF + "               B.SUCODE,                                                                 ";
                SQL = SQL + ComNum.VBLF + "               B.SUNAMEK,                                                                ";
                SQL = SQL + ComNum.VBLF + "               A.SNAME,                                                                  ";
                SQL = SQL + ComNum.VBLF + "               A.PTNO,                                                                   ";
                SQL = SQL + ComNum.VBLF + "               WDATE                                                                     ";
                SQL = SQL + ComNum.VBLF + "          FROM ADMIN.DRUG_ADR1 A                                                    ";
                SQL = SQL + ComNum.VBLF + "               INNER JOIN ADMIN.DRUG_ADR1_ORDER_JO B                                ";
                SQL = SQL + ComNum.VBLF + "                       ON A.SEQNO = B.SEQNO                                              ";
                SQL = SQL + ComNum.VBLF + "               INNER JOIN ADMIN.OCS_DRUGINFO_NEW C                                  ";
                SQL = SQL + ComNum.VBLF + "                       ON B.SUCODE = C.SUNEXT                                            ";
                SQL = SQL + ComNum.VBLF + "               INNER JOIN ADMIN.BAS_CLASS D                                        ";
                SQL = SQL + ComNum.VBLF + "                       ON C.BUNCODE = D.CLASSCODE                                        ";
                SQL = SQL + ComNum.VBLF + "         WHERE A.WDATE >= To_date('" + strSDate + "', 'YYYY-MM-DD')                      ";
                SQL = SQL + ComNum.VBLF + "           AND A.WDATE <= To_date('" + strEDate + "', 'YYYY-MM-DD')) T1                  ";
                SQL = SQL + ComNum.VBLF + "       INNER JOIN(SELECT BUNCODE,                                                        ";
                SQL = SQL + ComNum.VBLF + "                          Count(*) BUNRANK                                               ";
                SQL = SQL + ComNum.VBLF + "                     FROM(SELECT C.BUNCODE                                               ";
                SQL = SQL + ComNum.VBLF + "                             FROM ADMIN.DRUG_ADR1 A                                 ";
                SQL = SQL + ComNum.VBLF + "                                   INNER JOIN ADMIN.DRUG_ADR1_ORDER B               ";
                SQL = SQL + ComNum.VBLF + "                                           ON A.SEQNO = B.SEQNO                          ";
                SQL = SQL + ComNum.VBLF + "                                   INNER JOIN ADMIN.OCS_DRUGINFO_NEW C              ";
                SQL = SQL + ComNum.VBLF + "                                           ON B.SUCODE = C.SUNEXT                        ";
                SQL = SQL + ComNum.VBLF + "                            WHERE A.WDATE >= To_date('" + strSDate + "', 'YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "                              AND A.WDATE <= To_date('" + strEDate + "', 'YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "                           UNION ALL                                                     ";
                SQL = SQL + ComNum.VBLF + "                           SELECT C.BUNCODE                                              ";
                SQL = SQL + ComNum.VBLF + "                             FROM ADMIN.DRUG_ADR1 A                                 ";
                SQL = SQL + ComNum.VBLF + "                                   INNER JOIN ADMIN.DRUG_ADR1_ORDER_JO B            ";
                SQL = SQL + ComNum.VBLF + "                                           ON A.SEQNO = B.SEQNO                          ";
                SQL = SQL + ComNum.VBLF + "                                   INNER JOIN ADMIN.OCS_DRUGINFO_NEW C              ";
                SQL = SQL + ComNum.VBLF + "                                           ON B.SUCODE = C.SUNEXT                        ";
                SQL = SQL + ComNum.VBLF + "                            WHERE A.WDATE >= To_date('" + strSDate + "', 'YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "                              AND A.WDATE <= To_date('" + strEDate + "', 'YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "                          )                                                              ";
                SQL = SQL + ComNum.VBLF + "                    GROUP BY BUNCODE) T2                                                 ";
                SQL = SQL + ComNum.VBLF + "               ON T1.BUNCODE = T2.BUNCODE                                                ";
                SQL = SQL + ComNum.VBLF + "       INNER JOIN (SELECT SUCODE,                                                        ";
                SQL = SQL + ComNum.VBLF + "                          Count(*) CODERANK                                              ";
                SQL = SQL + ComNum.VBLF + "                     FROM(SELECT B.SUCODE                                                ";
                SQL = SQL + ComNum.VBLF + "                             FROM ADMIN.DRUG_ADR1 A                                 ";
                SQL = SQL + ComNum.VBLF + "                                  INNER JOIN ADMIN.DRUG_ADR1_ORDER B                ";
                SQL = SQL + ComNum.VBLF + "                                          ON A.SEQNO = B.SEQNO                           ";
                SQL = SQL + ComNum.VBLF + "                                  INNER JOIN ADMIN.OCS_DRUGINFO_NEW C               ";
                SQL = SQL + ComNum.VBLF + "                                           ON B.SUCODE = C.SUNEXT                        ";
                SQL = SQL + ComNum.VBLF + "                            WHERE A.WDATE >= To_date('" + strSDate + "', 'YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "                              AND A.WDATE <= To_date('" + strEDate + "', 'YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "                           UNION ALL                                                     ";
                SQL = SQL + ComNum.VBLF + "                           SELECT B.SUCODE                                               ";
                SQL = SQL + ComNum.VBLF + "                             FROM ADMIN.DRUG_ADR1 A                                 ";
                SQL = SQL + ComNum.VBLF + "                                  INNER JOIN ADMIN.DRUG_ADR1_ORDER_JO B             ";
                SQL = SQL + ComNum.VBLF + "                                          ON A.SEQNO = B.SEQNO                           ";
                SQL = SQL + ComNum.VBLF + "                                  INNER JOIN ADMIN.OCS_DRUGINFO_NEW C               ";
                SQL = SQL + ComNum.VBLF + "                                           ON B.SUCODE = C.SUNEXT                        ";
                SQL = SQL + ComNum.VBLF + "                            WHERE A.WDATE >= To_date('" + strSDate + "', 'YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "                              AND A.WDATE <= To_date('" + strEDate + "', 'YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "                          )                                                              ";
                SQL = SQL + ComNum.VBLF + "                    GROUP BY SUCODE) T3                                                  ";
                SQL = SQL + ComNum.VBLF + "               ON T1.SUCODE = T3.SUCODE                                                  ";
                SQL = SQL + ComNum.VBLF + " ORDER BY BUNRANK DESC,                                                                  ";
                SQL = SQL + ComNum.VBLF + "          CODERANK DESC,                                                                 ";
                SQL = SQL + ComNum.VBLF + "          WDATE                                                                          ";
                
                #endregion

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        //ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                        strSEQ = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BUNCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CLASSNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["WDATE"].ToString().Trim();


                        #region // 의심증상
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "    B1.GUBUN, B1.GUBUN2, B1.GUBUN3, B1.CODE, B1.NAME                                                     ";
                        SQL = SQL + ComNum.VBLF + "    FROM ADMIN.BAS_BCODE B1                                                                        ";
                        SQL = SQL + ComNum.VBLF + "INNER JOIN                                                                                               ";
                        SQL = SQL + ComNum.VBLF + "(SELECT * FROM                                                                                           ";
                        SQL = SQL + ComNum.VBLF + "(SELECT SEQNO,                                                                                           ";
                        SQL = SQL + ComNum.VBLF + "        RACT_A1, RACT_A2, RACT_A3, RACT_A4, RACT_A5, RACT_A6,                                            ";
                        SQL = SQL + ComNum.VBLF + "        RACT_B1, RACT_B2, RACT_B3, RACT_B4, RACT_B5, RACT_B6, RACT_B7, RACT_B8, RACT_B9,                 ";
                        SQL = SQL + ComNum.VBLF + "        RACT_C1, RACT_C2, RACT_C3, RACT_C4, RACT_C5, RACT_C6, RACT_C7, RACT_C8,                          ";
                        SQL = SQL + ComNum.VBLF + "        RACT_D1, RACT_D2, RACT_D3, RACT_D4, RACT_D5, RACT_D6, RACT_D7,                                   ";
                        SQL = SQL + ComNum.VBLF + "        RACT_E1, RACT_E2, RACT_E3, RACT_E4, RACT_E5, RACT_E6, RACT_E7,                                   ";
                        SQL = SQL + ComNum.VBLF + "        RACT_F1, RACT_F2,                                                                                ";
                        SQL = SQL + ComNum.VBLF + "        RACT_G1, RACT_G2, RACT_G3,                                                                       ";
                        SQL = SQL + ComNum.VBLF + "        RACT_H1, RACT_H2, RACT_H3, RACT_H4,                                                              ";
                        SQL = SQL + ComNum.VBLF + "        RACT_I1, RACT_I2, RACT_I3, RACT_I4,                                                              ";
                        SQL = SQL + ComNum.VBLF + "        RACT_J1, RACT_J2, RACT_J3, RACT_J4, RACT_J5, RACT_J6, RACT_J7, RACT_J8, RACT_J9, RACT_J10,       ";
                        SQL = SQL + ComNum.VBLF + "        RACT_J11, RACT_J12, RACT_J13, RACT_J14, RACT_J15, RACT_J16,                                      ";
                        SQL = SQL + ComNum.VBLF + "        RACT_K1, RACT_K2, RACT_K3, RACT_K4, RACT_K5, RACT_K6, RACT_K7,                                   ";
                        SQL = SQL + ComNum.VBLF + "        RACT_L1, RACT_L2, RACT_L3                                                                        ";
                        SQL = SQL + ComNum.VBLF + "    FROM ADMIN.DRUG_ADR1 T1                                                                         ";
                        SQL = SQL + ComNum.VBLF + "    WHERE T1.SEQNO = '" + strSEQ + "') TEMP                                                              ";
                        SQL = SQL + ComNum.VBLF + "UNPIVOT(VAL FOR CODE IN(RACT_A1, RACT_A2, RACT_A3, RACT_A4, RACT_A5, RACT_A6,                            ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_B1, RACT_B2, RACT_B3, RACT_B4, RACT_B5, RACT_B6, RACT_B7, RACT_B8, RACT_B9, ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_C1, RACT_C2, RACT_C3, RACT_C4, RACT_C5, RACT_C6, RACT_C7, RACT_C8,          ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_D1, RACT_D2, RACT_D3, RACT_D4, RACT_D5, RACT_D6, RACT_D7,                   ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_E1, RACT_E2, RACT_E3, RACT_E4, RACT_E5, RACT_E6, RACT_E7,                   ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_F1, RACT_F2,                                                                ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_G1, RACT_G2, RACT_G3,                                                       ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_H1, RACT_H2, RACT_H3, RACT_H4,                                              ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_I1, RACT_I2, RACT_I3, RACT_I4,                                              ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_J1, RACT_J2, RACT_J3, RACT_J4, RACT_J5,                                     ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_J6, RACT_J7, RACT_J8, RACT_J9, RACT_J10,                                    ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_J11, RACT_J12, RACT_J13, RACT_J14, RACT_J15, RACT_J16,                      ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_K1, RACT_K2, RACT_K3, RACT_K4, RACT_K5, RACT_K6, RACT_K7,                   ";
                        SQL = SQL + ComNum.VBLF + "                        RACT_L1, RACT_L2, RACT_L3))) B2                                                  ";
                        SQL = SQL + ComNum.VBLF + "    ON B1.CODE = B2.CODE                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "WHERE B1.GUBUN = 'ETC_ADR_이상반응'                                                                      ";
                        SQL = SQL + ComNum.VBLF + "    AND B2.VAL = '1'                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY CODE                                                                                            ";
                        #endregion

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strTemp = "";
                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                //iRow = iRow + j;
                                strTemp = strTemp + dt1.Rows[j]["NAME"].ToString().Trim() + ComNum.VBLF;
                            }

                            ssView_Sheet1.Cells[i, 4].Text = strTemp;
                        }
                        dt1.Dispose();
                        dt1 = null;

                        //iRow = iRow + 1;
                        byte[] a = System.Text.Encoding.Default.GetBytes(ssView_Sheet1.Cells[i, 4].Text);
                        int intHeight = Convert.ToInt32(a.Length / 10);
                        ssView_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + (intHeight * 22));
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

    }
}
