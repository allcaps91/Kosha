using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// RRS 병동환자 
    /// </summary>
    public partial class frmEmrBaseRRSWardView : Form
    {
        public frmEmrBaseRRSWardView()
        {
            InitializeComponent();
        }


        private void frmEmrBaseRRSWardView_Load(object sender, EventArgs e)
        {
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            ssUserChart_Sheet1.DefaultStyle.Border = complexBorder2;
            ssUserChart_Sheet1.SheetCornerStyle.Border = complexBorder2;
            ssUserChart_Sheet1.ColumnHeader.DefaultStyle.Border = complexBorder2;
            ssUserChart_Sheet1.RowHeader.DefaultStyle.Border = complexBorder2;

            ssUserChart_Sheet1.RowCount = 0;

            #region ComboWard_SET()
            string SQL = string.Empty;
            OracleDataReader reader = null;

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            int sIndex = -1;
            int sCount = 0;

            try
            {
                SQL = " SELECT NAME WARDCODE, MATCH_CODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(0).ToString().Trim() != "ER" && reader.GetValue(0).ToString().Trim() != "HD")
                        {
                            cboWard.Items.Add(reader.GetValue(0).ToString().Trim());
                            if (reader.GetValue(1).ToString().Trim().Equals(clsType.User.BuseCode))
                            {
                                sIndex = sCount;
                            }
                            sCount += 1;
                        }
                    }
                }

                //cboWard.Items.Add("HD");
                //cboWard.Items.Add("ER");
                //cboWard.Items.Add("OP");
                //cboWard.Items.Add("ENDO");
                //cboWard.Items.Add("외래수혈");

                cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex + 1;

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }


            cboWard.Items.Add("HD");
            cboWard.Items.Add("ER");
            cboWard.Items.Add("OP");
            cboWard.Items.Add("AG");
            cboWard.Items.Add("ENDO");
            cboWard.Items.Add("외래수혈");
            cboWard.Items.Add("CT");
            cboWard.Items.Add("MRI");
            cboWard.Items.Add("RI");
            cboWard.Items.Add("SONO");
            #endregion

            string WardCodes = string.Empty;
            if (VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                WardCodes = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            if (NURSE_Manager_Check(clsType.User.IdNumber) == true || WardCodes.Equals("ER") || clsVbfunc.GetBCodeCODE(clsDB.DbCon, "NUR_간호부관리자사번IP", "").Equals(clsCompuInfo.gstrCOMIP))
            {
                cboWard.Enabled = true;
            }
        }

        private bool NURSE_Manager_Check(string strSABUN)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "SELECT Code FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + " WHERE Gubun='NUR_간호부관리자사번' ";
            SQL = SQL + ComNum.VBLF + "   AND Code= " + VB.Val(strSABUN) + " ";
            SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL    ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return false;
            }

            dt.Dispose();
            dt = null;
            return true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetPatList();
        }

        /// <summary>
        /// 환자리스트를 가지고 온다
        /// </summary>
        void GetPatList()
        {
            DataTable dt = null;
            string SQL = string.Empty;

            try
            {
                ssUserChart_Sheet1.RowCount = 0;

                string strPriDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-1).ToShortDateString();
                string strToDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(0).ToShortDateString();
                string strNextDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(1).ToShortDateString();

                if (cboWard.Text.Trim() != "HD")
                {
                    SQL = "SELECT WardCode, RoomCode, Pano, SName, Sex, AGE, InDate, OUTDATE, DEPTCODE, DRCODE, DRNAME";
                    SQL = SQL + ComNum.VBLF + ", PR, PRSCORE, SBP, SBPSCORE, RR, RRSCORE, BT, BTSCORE, AVPU, AVPUSCORE";
                    SQL = SQL + ComNum.VBLF + ", (NVL(PRSCORE, 0) + NVL(SBPSCORE, 0) + NVL(RRSCORE, 0) + NVL(BTSCORE, 0) + NVL(AVPUSCORE, 0)) AS TOTAL";
                    SQL = SQL + ComNum.VBLF + "FROM";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + "  SELECT M.WardCode, M.RoomCode, M.Pano, M.SName, M.Sex, M.Age,";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,";
                    SQL = SQL + ComNum.VBLF + "  M.DeptCode,M.DrCode,D.DrName ";

                    SQL = SQL + ComNum.VBLF + "  , B.ITEMVALUE AS PR";
                    SQL = SQL + ComNum.VBLF + "  , CASE WHEN TO_NUMBER(REPLACE(B.ITEMVALUE, '-', '0')) <= 40 OR(TO_NUMBER(REPLACE(B.ITEMVALUE, '-', '0')) >= 111 AND TO_NUMBER(REPLACE(B.ITEMVALUE, '-', '0')) <= 130) THEN 2                              ";
                    SQL = SQL + ComNum.VBLF + "         WHEN(TO_NUMBER(REPLACE(B.ITEMVALUE, '-', '0')) >= 41 AND TO_NUMBER(REPLACE(B.ITEMVALUE, '-', '0')) <= 50) OR TO_NUMBER(REPLACE(B.ITEMVALUE, '-', '0'))  >= 101 AND TO_NUMBER(REPLACE(B.ITEMVALUE, '-', '0'))  <= 110 THEN 1    ";
                    SQL = SQL + ComNum.VBLF + "         WHEN TO_NUMBER(REPLACE(B.ITEMVALUE, '-', '0')) >= 131 THEN 3                                                                                               ";
                    SQL = SQL + ComNum.VBLF + "         ELSE 0                                                                                                                                  ";
                    SQL = SQL + ComNum.VBLF + "     END PRSCORE                                                                                                                                  ";

                    SQL = SQL + ComNum.VBLF + "   , B2.ITEMVALUE AS SBP";
                    SQL = SQL + ComNum.VBLF + "   , CASE WHEN(TO_NUMBER(REGEXP_REPLACE(B2.ITEMVALUE, '^[a-zA-Z]+$', '')) >= 71 AND TO_NUMBER(REGEXP_REPLACE(B2.ITEMVALUE, '^[a-zA-Z]+$', '')) <= 80) OR TO_NUMBER(REGEXP_REPLACE(B2.ITEMVALUE, '^[a-zA-Z]+$', '')) >= 200 THEN 2                          ";
                    SQL = SQL + ComNum.VBLF + "          WHEN TO_NUMBER(REGEXP_REPLACE(B2.ITEMVALUE, '^[a-zA-Z]+$', '')) >= 81 AND TO_NUMBER(REGEXP_REPLACE(B2.ITEMVALUE, '^[a-zA-Z]+$', '')) <= 100 THEN 1                                                            ";
                    SQL = SQL + ComNum.VBLF + "          WHEN TO_NUMBER(REGEXP_REPLACE(B2.ITEMVALUE, '^[a-zA-Z]+$', '')) <= 70 THEN 3                                                                                               ";
                    SQL = SQL + ComNum.VBLF + "          ELSE 0                                                                                                                                  ";
                    SQL = SQL + ComNum.VBLF + "     END SBPSCORE                                                                                                                                 ";

                    SQL = SQL + ComNum.VBLF + "   , B3.ITEMVALUE AS RR";
                    SQL = SQL + ComNum.VBLF + "   , CASE WHEN TO_NUMBER(REPLACE(B3.ITEMVALUE, '-', '0'))  <= 8 OR (TO_NUMBER(REPLACE(B3.ITEMVALUE, '-', '0')) >= 21 AND TO_NUMBER(REPLACE(B3.ITEMVALUE, '-', '0')) <= 29) THEN 2                           ";
                    SQL = SQL + ComNum.VBLF + "          WHEN TO_NUMBER(REPLACE(B3.ITEMVALUE, '-', '0')) >= 15 AND TO_NUMBER(REPLACE(B3.ITEMVALUE, '-', '0')) <= 20 THEN 1                                                             ";
                    SQL = SQL + ComNum.VBLF + "          WHEN TO_NUMBER(REPLACE(B3.ITEMVALUE, '-', '0')) <= 30 THEN 3                                                                                               ";
                    SQL = SQL + ComNum.VBLF + "          ELSE 0                                                                                                                                  ";
                    SQL = SQL + ComNum.VBLF + "     END RRSCORE                                                                                                                                  ";

                    SQL = SQL + ComNum.VBLF + "   , B4.ITEMVALUE AS BT";
                    SQL = SQL + ComNum.VBLF + "   , CASE WHEN TO_NUMBER(REPLACE(REPLACE(B4.ITEMVALUE, '-', '0'), '..', '.')) <= 35.0 THEN 2                                                                                             ";
                    SQL = SQL + ComNum.VBLF + "          WHEN(TO_NUMBER(REPLACE(REPLACE(B4.ITEMVALUE, '-', '0'), '..', '.')) >= 35.1 AND TO_NUMBER(REPLACE(B4.ITEMVALUE, '-', '0')) <= 36.0) OR TO_NUMBER(REPLACE(B4.ITEMVALUE, '-', '0')) >= 37.5 THEN 1                     ";
                    SQL = SQL + ComNum.VBLF + "          ELSE 0                                                                                                                                  ";
                    SQL = SQL + ComNum.VBLF + "     END BTSCORE                                                                                                                                  ";

                    SQL = SQL + ComNum.VBLF + "   , B5.ITEMVALUE AS AVPU";
                    SQL = SQL + ComNum.VBLF + "   , CASE WHEN B5.ITEMVALUE = 'Verbal Response' THEN 1                                                                                            ";
                    SQL = SQL + ComNum.VBLF + "          WHEN B5.ITEMVALUE = 'Pain Response' THEN 2                                                                                              ";
                    SQL = SQL + ComNum.VBLF + "          WHEN B5.ITEMVALUE = 'Unconsciousness' THEN 3                                                                                            ";
                    SQL = SQL + ComNum.VBLF + "          ELSE 0                                                                                                                                  ";
                    SQL = SQL + ComNum.VBLF + "     END AVPUSCORE                                                                                                                           ";


                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER  M ";
                    SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.BAS_DOCTOR  D ";
                    SQL = SQL + ComNum.VBLF + "       ON M.DRCODE = D.DRCODE  ";
                    SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRCHARTROW B";
                    SQL = SQL + ComNum.VBLF + "       ON B.EMRNO =";
                    SQL = SQL + ComNum.VBLF + "       (";
                    SQL = SQL + ComNum.VBLF + "        SELECT MAX(A.EMRNO)                                    ";
                    SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST A                       ";        
                    SQL = SQL + ComNum.VBLF + "          WHERE A.PTNO = M.PANO                                ";
                    SQL = SQL + ComNum.VBLF + "              AND A.MEDFRDATE = TO_CHAR(M.INDATE, 'YYYYMMDD')  ";
                    SQL = SQL + ComNum.VBLF + "              AND A.FORMNO IN(3150, 2135, 1935, 2431, 1969)";
                    SQL = SQL + ComNum.VBLF + "              AND (CHARTDATE || RPAD(CHARTTIME, 6, '0')) = ";
                    SQL = SQL + ComNum.VBLF + "                  (SELECT MAX(CHARTDATE || RPAD(CHARTTIME, 6, '0'))";
                    SQL = SQL + ComNum.VBLF + "                     FROM KOSMOS_EMR.AEMRCHARTMST AA";
                    SQL = SQL + ComNum.VBLF + "                       INNER JOIN KOSMOS_EMR.AEMRCHARTROW B             ";
                    SQL = SQL + ComNum.VBLF + "                          ON AA.EMRNO = B.EMRNO                         ";
                    SQL = SQL + ComNum.VBLF + "                         AND AA.EMRNOHIS = B.EMRNOHIS                      ";
                    SQL = SQL + ComNum.VBLF + "                         AND B.ITEMCD = 'I0000002009'                     ";
                    SQL = SQL + ComNum.VBLF + "                         AND B.ITEMVALUE > CHR(0)                         ";
                    SQL = SQL + ComNum.VBLF + "                       INNER JOIN KOSMOS_EMR.AEMRCHARTROW B2             ";
                    SQL = SQL + ComNum.VBLF + "                          ON AA.EMRNO = B2.EMRNO                         ";
                    SQL = SQL + ComNum.VBLF + "                         AND AA.EMRNOHIS = B2.EMRNOHIS                      ";
                    SQL = SQL + ComNum.VBLF + "                         AND B2.ITEMCD = 'I0000024733'   --구분            ";
                    SQL = SQL + ComNum.VBLF + "                         AND NVL(B2.ITEMVALUE, ' ') <> '퇴원'                     ";
                    SQL = SQL + ComNum.VBLF + "                    WHERE PTNO = A.PTNO ";
                    SQL = SQL + ComNum.VBLF + "                      AND FORMNO IN(3150, 2135, 1935, 2431, 1969)";
                    SQL = SQL + ComNum.VBLF + "                      AND MEDFRDATE = A.MEDFRDATE";
                    SQL = SQL + ComNum.VBLF + "                  )";
                    SQL = SQL + ComNum.VBLF + "       )";
                    SQL = SQL + ComNum.VBLF + "      AND B.EMRNOHIS > 0";
                    SQL = SQL + ComNum.VBLF + "      AND B.ITEMCD = 'I0000014815' -- PR";
                    SQL = SQL + ComNum.VBLF + "      LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B2   ";
                    SQL = SQL + ComNum.VBLF + "        ON B2.EMRNO = B.EMRNO                      ";
                    SQL = SQL + ComNum.VBLF + "       AND B2.EMRNOHIS = B.EMRNOHIS                ";
                    SQL = SQL + ComNum.VBLF + "       AND B2.ITEMCD = 'I0000002018'               ";
                    SQL = SQL + ComNum.VBLF + "      LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B3   ";
                    SQL = SQL + ComNum.VBLF + "        ON B3.EMRNO = B.EMRNO                      ";
                    SQL = SQL + ComNum.VBLF + "       AND B3.EMRNOHIS = B.EMRNOHIS                ";
                    SQL = SQL + ComNum.VBLF + "       AND B3.ITEMCD = 'I0000002009'               ";
                    SQL = SQL + ComNum.VBLF + "      LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B4   ";
                    SQL = SQL + ComNum.VBLF + "        ON B4.EMRNO = B.EMRNO                      ";
                    SQL = SQL + ComNum.VBLF + "       AND B4.EMRNOHIS = B.EMRNOHIS                ";
                    SQL = SQL + ComNum.VBLF + "       AND B4.ITEMCD = 'I0000001811'               ";
                    SQL = SQL + ComNum.VBLF + "      LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B5   ";
                    SQL = SQL + ComNum.VBLF + "        ON B5.EMRNO = B.EMRNO                      ";
                    SQL = SQL + ComNum.VBLF + "       AND B5.EMRNOHIS = B.EMRNOHIS                ";
                    SQL = SQL + ComNum.VBLF + "       AND B5.ITEMCD = 'I0000037778'               ";

                    switch (cboWard.Text.Trim())
                    {
                        case "전체": SQL = SQL + ComNum.VBLF + "WHERE M.WardCode>' ' "; break;
                        case "MICU": SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='234' "; break;
                        case "SICU": SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='233' "; break;
                        case "ND":
                        case "NR":
                            SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') "; break;
                        //'Case "3B":   SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('3B','DR') "; //'COMBOBOX 처리
                        default: SQL = SQL + ComNum.VBLF + "WHERE M.WardCode='" + cboWard.Text.Trim() + "' "; break;
                    }

                    SQL = SQL + ComNum.VBLF + " AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000' ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
                    SQL = SQL + ComNum.VBLF + ")";
                }
                else
                {
                    SQL = " SELECT '' AS WardCode, '' AS RoomCode, Pano, SName, Sex, Age, '' AS Bi,  '' AS PName,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(BDATE,'YYYY-MM-DD') AS InDate, 0 as Ilsu, 0 as IpdNo,  '' AS GBSTS,";
                    SQL = SQL + ComNum.VBLF + " '' AS OutDate, ";
                    SQL = SQL + ComNum.VBLF + "  DeptCode, DrCode,  '' AS DrName,  '' AS AmSet1, '' AS AmSet4,  '' AS AmSet6,  '' AS AmSet7 ";
                    SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.OPD_MASTER M";
                    SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = 'HD'";
                    SQL = SQL + ComNum.VBLF + "      AND JIN IN ('0','1','2','3','4','5','6','7','9','C','E','F','H','M','L','K', 'N','I','J','Q','R','S','A','B')  ";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL    ";
                    SQL = SQL + ComNum.VBLF + "SELECT B.WARDCODE AS WardCode, TO_CHAR(B.ROOMCODE) AS RoomCode, A.Pano, A.SName, A.Sex, A.Age, B.BI,  B.PNAME,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(B.INDATE,'YYYY-MM-DD') AS InDate, B.ILSU, IpdNo,  GBSTS,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(B.OutDate,'YYYY-MM-DD') OUTDATE,";
                    SQL = SQL + ComNum.VBLF + "     b.DeptCode , b.DrCode, c.DrName, AmSet1, AmSet4, AmSet6, AmSet7";
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.TONG_HD_DAILY A, KOSMOS_PMPA.IPD_NEW_MASTER B, KOSMOS_PMPA.BAS_DOCTOR C";
                    SQL = SQL + ComNum.VBLF + "WHERE TDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND IPDOPD = 'I'";
                    SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO";
                    SQL = SQL + ComNum.VBLF + "    AND TRUNC(B.INDATE) <= A.TDATE";
                    SQL = SQL + ComNum.VBLF + "    AND (B.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR B.OUTDATE >= A.TDATE)";
                    SQL = SQL + ComNum.VBLF + "    AND B.DRCODE = C.DRCODE";
                }

                SQL = SQL + ComNum.VBLF + "   ORDER BY RoomCode, SName, Indate DESC  ";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt == null)
                    return;

                if (dt.Rows.Count > 0)
                {
                    ssUserChart_Sheet1.RowCount = dt.Rows.Count;
                    ssUserChart_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < ssUserChart_Sheet1.ColumnCount; i++)
                    {
                        if (ssUserChart_Sheet1.Columns[i].Tag != null)
                        {
                            ssUserChart_Sheet1.Cells[0, i, ssUserChart_Sheet1.RowCount - 1, i].Text = ssUserChart_Sheet1.Columns[i].Tag.ToString();
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssUserChart_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 5].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PR"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 9].Text = dt.Rows[i]["PRSCORE"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 10].Text = dt.Rows[i]["SBP"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 11].Text = dt.Rows[i]["SBPSCORE"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 12].Text = dt.Rows[i]["RR"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 13].Text = dt.Rows[i]["RRSCORE"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 14].Text = dt.Rows[i]["BT"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 15].Text = dt.Rows[i]["BTSCORE"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 16].Text = dt.Rows[i]["AVPU"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 17].Text = dt.Rows[i]["AVPUSCORE"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, 18].Text = dt.Rows[i]["TOTAL"].ToString().Trim();

                        double Score = VB.Val(ssUserChart_Sheet1.Cells[i, 18].Text);
                        #region 색상
                        if (Score >= 0 && Score <= 4)
                        {
                            ssUserChart_Sheet1.Rows[i].BackColor = Color.LightGreen;
                        }
                        else if (Score >= 5 && Score <= 6)
                        {
                            ssUserChart_Sheet1.Rows[i].BackColor = Color.Orange;
                        }
                        else if (Score >= 7)
                        {
                            ssUserChart_Sheet1.Rows[i].BackColor = Color.Red;
                        }
                        #endregion
                    }
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ssUserChart_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano = ssUserChart_Sheet1.Cells[e.Row, 1].Text.Trim();
            string strIndate = ssUserChart_Sheet1.Cells[e.Row, 5].Text.Trim().Replace("-", "");
            string strMedDeptCd = ssUserChart_Sheet1.Cells[e.Row, 6].Text.Trim();

            EmrPatient AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPano, "I", strIndate, strMedDeptCd);

            if (AcpEmr == null)
                return;

            using (frmEmrBaseRRSDetail frmEmrBaseRRSDetailX = new frmEmrBaseRRSDetail(AcpEmr))
            {
                frmEmrBaseRRSDetailX.StartPosition = FormStartPosition.CenterScreen;
                frmEmrBaseRRSDetailX.ShowDialog(this);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strHeader = string.Empty;
            using (Font font = new Font("굴림체", 20, FontStyle.Bold))
            {
                using (Font font2 = new Font("굴림체", 11))
                {
                    using (clsSpread cls = new clsSpread())
                    {
                        strHeader += cls.setSpdPrint_String(cboWard.Text + "병동 RRS 목록", font, clsSpread.enmSpdHAlign.Center, false, true);
                        strHeader += cls.setSpdPrint_String("     출력자 : "   + clsType.User.UserName, font2, clsSpread.enmSpdHAlign.Left, false, false);
                        strHeader += cls.setSpdPrint_String("출력시간 : " + ComQuery.CurrentDateTime(clsDB.DbCon, "S") + "      ", font2, clsSpread.enmSpdHAlign.Right, false, false);
                    }
                }
            }
            

            ssUserChart_Sheet1.PrintInfo.Header = strHeader;
            ssUserChart_Sheet1.PrintInfo.Margin.Top = 40;
            ssUserChart_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssUserChart_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssUserChart_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssUserChart_Sheet1.PrintInfo.Preview = false;
            ssUserChart_Sheet1.PrintInfo.UseMax = false;
            ssUserChart.PrintSheet(0);
        }
    }
}
