using ComBase;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmIO_D : Form
    {
        #region 변수
        /// <summary>
        /// 등롭건호
        /// </summary>
        string mstrPano = string.Empty;

        /// <summary>
        /// 이름
        /// </summary>
        string mstrName = string.Empty;

        /// <summary>
        /// 성별
        /// </summary>
        string mstrSex = string.Empty;

        /// <summary>
        /// 나이
        /// </summary>
        string mstrAge = string.Empty;

        /// <summary>
        /// 주민1
        /// </summary>
        string mstrJumin1 = string.Empty;

        /// <summary>
        /// 주민2
        /// </summary>
        string mstrJumin2 = string.Empty;
        #endregion

        public frmIO_D()
        {
            InitializeComponent();
        }


        public frmIO_D(string strPano)
        {
            mstrPano = strPano;
            InitializeComponent();
        }

        private void FrmIO_D_Load(object sender, EventArgs e)
        {
            Read_PatInfo();

            SSPatientInfo_Sheet1.Cells[0, 0].Text = mstrPano;
            SSPatientInfo_Sheet1.Cells[0, 1].Text = mstrName;

            dtpEDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            dtpSDATE.Value = dtpEDATE.Value.AddDays(-5);

            btnPrint.Enabled = false;

            if (clsEmrPublic.gUserGrade.Equals("SIMSA") || clsEmrPublic.gUserGrade.Equals("WRITE"))
            {
                btnPrint.Enabled = true;
            }

            btnSearch.PerformClick();
        }

        /// <summary>
        /// 환자정보
        /// </summary>
        /// <returns></returns>
        void Read_PatInfo()
        {
            string SQL = string.Empty;
            OracleDataReader dataReader = null;

            try
            {
                SQL = " SELECT SNAME, SEX, JUMIN1, JUMIN2, JUMIN3";
                SQL += ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_PATIENT";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + mstrPano + "'";

                string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dataReader.HasRows && dataReader.Read())
                {
                    mstrName = dataReader.GetValue(0).ToString().Trim();
                    mstrSex = dataReader.GetValue(1).ToString().Trim().Equals("M") ? "남" : "여";
                    mstrAge = (ComFunc.AgeCalcEx(dataReader.GetValue(2).ToString().Trim() + clsAES.DeAES(dataReader.GetValue(4).ToString().Trim()), ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10))).ToString();
                    mstrJumin1 = dataReader.GetValue(2).ToString().Trim();
                    mstrJumin2 = dataReader.GetValue(3).ToString().Trim();
                }

                dataReader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            READ_IO_TOTAL();
        }


        /// <summary>
        /// IO 조회 함수
        /// </summary>
        void READ_IO_TOTAL()
        {
            string SQL = string.Empty;
            DataTable dt = null;

            SSIO_Sheet1.RowCount = 3;
            FarPoint.Win.ComplexBorder border = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            string strOLDCHARTDATE = string.Empty;
            string strOLDCHARTTIME = string.Empty;

            int nItotal = 0;
            int nOtotal = 0;
            int nRow = 3;
            int nCol = 0;

            string SqlErr = string.Empty;

            try
            {
                #region NEW -------
                SQL = SQL + ComNum.VBLF + "SELECT ITEM, TOTAL";
                SQL = SQL + ComNum.VBLF + "       ,CASE WHEN TRIM(SUBSTR(CDATE, 9, 4)) = '2400' THEN TO_DATE(SUBSTR(CDATE, 0, 8), 'YYYYMMDD') + 1";
                SQL = SQL + ComNum.VBLF + "        ELSE TO_DATE(SUBSTR(CDATE, 0, 8) || ' ' || TRIM(SUBSTR(CDATE, 9, 4)), 'YYYYMMDD HH24MI') END CDATE";
                SQL = SQL + ComNum.VBLF + "       ,CASE WHEN TRIM(SUBSTR(CDATE, 9, 4)) >= 0501 AND TRIM(SUBSTR(CDATE, 9, 4)) <= 1300 THEN 'DAY'";
                SQL = SQL + ComNum.VBLF + "            WHEN TRIM(SUBSTR(CDATE, 9, 4)) >= 1301 AND TRIM(SUBSTR(CDATE, 9, 4)) <= 2100 THEN 'EVE'";
                SQL = SQL + ComNum.VBLF + "            WHEN TRIM(SUBSTR(CDATE, 9, 4)) >= 2101 AND TRIM(SUBSTR(CDATE, 9, 4)) <= 2400 THEN 'NIG'";
                SQL = SQL + ComNum.VBLF + "            WHEN TRIM(SUBSTR(CDATE, 9, 4)) >= 0000 AND TRIM(SUBSTR(CDATE, 9, 4)) <= 0500 THEN 'NIG'";
                SQL = SQL + ComNum.VBLF + "        END DUTY";
                SQL = SQL + ComNum.VBLF + "FROM (";
                SQL = SQL + ComNum.VBLF + "SELECT SUBSTR((CHARTDATE || CHARTTIME), 0 , 12) AS CDATE, b1.remark1 AS ITEM, SUM(R.ITEMVALUE) AS TOTAL ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST A                                              ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRBASCD B1                                         ";
                SQL = SQL + ComNum.VBLF + "        ON B1.BSNSCLS = '기록지관리'                                              ";
                SQL = SQL + ComNum.VBLF + "       AND B1.UNITCLS = '섭취배설그룹'                                             ";
                SQL = SQL + ComNum.VBLF + "       AND B1.BASCD  > CHR(0)                                                 ";
                SQL = SQL + ComNum.VBLF + "       AND B1.APLFRDATE > CHR(0)                                              ";
                SQL = SQL + ComNum.VBLF + "       AND B1.REMARK1  IN('경구', '배액관', 'Stool', '혈액', '수액', 'Urine')        ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRBASCD B                                          ";
                SQL = SQL + ComNum.VBLF + "        ON B.BSNSCLS = '기록지관리'                                               ";
                SQL = SQL + ComNum.VBLF + "       AND B.UNITCLS = '섭취배설'                                                ";
                SQL = SQL + ComNum.VBLF + "       AND B.BASCD  > CHR(0)                                                  ";
                SQL = SQL + ComNum.VBLF + "       AND B.APLFRDATE > CHR(0)                                               ";
                SQL = SQL + ComNum.VBLF + "       AND B.VFLAG1 = B1.BASCD                                                ";
                SQL = SQL + ComNum.VBLF + "       AND B.VFLAG3 IN('11.배설', '01.섭취')                                               ";
                SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_EMR.AEMRCHARTROW R                                     ";
                SQL = SQL + ComNum.VBLF + "        ON A.EMRNO  = R.EMRNO                                                 ";
                SQL = SQL + ComNum.VBLF + "       AND A.EMRNOHIS = R.EMRNOHIS                                            ";
                SQL = SQL + ComNum.VBLF + "       AND R.ITEMCD  = B.BASCD                                                ";
                SQL = SQL + ComNum.VBLF + "       AND REGEXP_INSTR(REPLACE(R.ITEMVALUE, '.', ''),'[^0-9]') = 0                             ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + mstrPano + "'                                                      ";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO = 3150                                                          ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "   AND CHARTUSEID <> '합계'                                                     ";
                SQL = SQL + ComNum.VBLF + " GROUP BY SUBSTR((CHARTDATE || CHARTTIME), 0 , 12), B1.REMARK1                ";
                SQL = SQL + ComNum.VBLF + ")";
                SQL = SQL + ComNum.VBLF + "  ORDER BY TO_CHAR(CDATE, 'YYYYMMDD') DESC, TO_CHAR(CDATE, 'HH24MI'), DECODE(DUTY, 'DAY', 0, 'EVE', 1, 'NIG', 2)";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    DateTime dtpOldChartDate = Convert.ToDateTime(dt.Rows[0]["CDATE"].ToString().Trim());
                    string strDuty = string.Empty;
                    string strOldDuty = dt.Rows[0]["DUTY"].ToString().Trim();
                    SSIO_Sheet1.RowCount += 1;
                    nRow = SSIO_Sheet1.RowCount - 1;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DateTime dtpChartDate = Convert.ToDateTime(dt.Rows[i]["CDATE"].ToString().Trim());
                        strDuty = dt.Rows[i]["DUTY"].ToString().Trim();

                        if (dtpChartDate.Date == dtpOldChartDate.Date && strDuty.Equals(strOldDuty) == false ||
                            (dtpOldChartDate.Date - dtpChartDate.Date).TotalDays == 1 && strDuty.Equals("NIG") && strOldDuty.Equals("NIG") ||
                            (dtpOldChartDate.Date - dtpChartDate.Date).TotalDays == 1 && strDuty.Equals("NIG") && strOldDuty.Equals("NIG") == false ||
                            (dtpOldChartDate.Date - dtpChartDate.Date).TotalHours > 8 ||
                            (dtpOldChartDate.Date - dtpChartDate.Date).TotalDays > 1)
                        {
                            SSIO_Sheet1.RowCount += 1;
                            nRow = SSIO_Sheet1.RowCount - 1;

                            SSIO_Sheet1.Cells[nRow, 0].Text = "총 량";
                            SSIO_Sheet1.AddSpanCell(nRow, 0, 1, 2);

                            SSIO_Sheet1.Cells[nRow, 3].Text = nItotal.ToString();
                            SSIO_Sheet1.AddSpanCell(nRow, 3, 1, 3);

                            SSIO_Sheet1.Cells[nRow, 7].Text = nOtotal.ToString();
                            SSIO_Sheet1.AddSpanCell(nRow, 7, 1, 3);

                            nItotal = 0;
                            nOtotal = 0;
                        }

                        if (dtpOldChartDate != dtpChartDate)
                        {
                            SSIO_Sheet1.RowCount += 1;
                            nRow = SSIO_Sheet1.RowCount - 1;
                        }

                        SSIO_Sheet1.Cells[nRow, 0].Text = dtpChartDate.ToString("MM") + "/" + dtpChartDate.ToString("dd") + " " + dtpChartDate.ToString("HH:mm");
                        SSIO_Sheet1.Cells[nRow, 1].Text = dt.Rows[i]["DUTY"].ToString().Trim();

                        switch (dt.Rows[i]["ITEM"].ToString().Trim())
                        {
                            case "수액":
                            case "혈액":
                            case "경구":
                                nItotal += (int)VB.Val(dt.Rows[i]["TOTAL"].ToString().Trim());
                                break;
                            case "Stool":
                            case "Urine":
                            case "배액관":
                                nOtotal += (int)VB.Val(dt.Rows[i]["TOTAL"].ToString().Trim());
                                break;
                        }

                        switch (dt.Rows[i]["ITEM"].ToString().Trim())
                        {
                            case "경구":
                                nCol = 3;
                                break;
                            case "수액":
                                nCol = 4;
                                break;
                            case "혈액":
                                nCol = 5;
                                break;
                            case "Urine":
                                nCol = 7;
                                break;
                            case "Stool":
                                nCol = 8;
                                break;
                            case "배액관":
                                nCol = 9;
                                break;
                        }

                        SSIO_Sheet1.Cells[nRow, nCol].Text = dt.Rows[i]["TOTAL"].ToString().Trim();
                        dtpOldChartDate = Convert.ToDateTime(dt.Rows[i]["CDATE"].ToString().Trim());
                        strOldDuty = dt.Rows[i]["DUTY"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                SSIO_Sheet1.RowCount += 1;
                nRow = SSIO_Sheet1.RowCount - 1;

                SSIO_Sheet1.Cells[nRow, 0].Text = "총 량";
                SSIO_Sheet1.AddSpanCell(nRow, 0, 1, 2);

                SSIO_Sheet1.Cells[nRow, 3].Text = nItotal.ToString();
                SSIO_Sheet1.AddSpanCell(nRow, 3, 1, 3);

                SSIO_Sheet1.Cells[nRow, 7].Text = nOtotal.ToString();
                SSIO_Sheet1.AddSpanCell(nRow, 7, 1, 3);

                nItotal = 0;
                nOtotal = 0;
                #endregion


                SQL = " SELECT 'WARD' GBN, CHARTDATE, TRIM(CHARTTIME) CHARTTIME,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it64') AS DUTY,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it2') 구강1,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it5') 구강2 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it8') 구강3,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it11') 수액1,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it14') 수액2 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it17') 수액3 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it20') 수액4 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it23') 수액5 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it66') 수액6,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it26') 혈액1 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it29') 혈액2,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it32') 혈액3 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it35') 혈액4,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it38') 섭취총량,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it41') URINE,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it44') STOOL,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it47') 기타배설1 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it50') 기타배설2,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it53') 기타배설3 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it56') 기타배설4 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it59') 기타배설5,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it62') 배설총량";
                SQL = SQL + ComNum.VBLF + "     From KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT EMRNO FROM KOSMOS_EMR.EMRXMLMST WHERE FORMNO = 1970";
                SQL = SQL + ComNum.VBLF + "      AND PTNO = '" + (mstrPano) + "'";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "')";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "   SELECT 'ICU' GBN, CHARTDATE, TRIM(CHARTTIME) CHARTTIME,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta1') AS DUTY,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta3') 구강1,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta6') 구강2,";
                SQL = SQL + ComNum.VBLF + "   '0' 구강3,";
                SQL = SQL + ComNum.VBLF + "   '0' 수액1,";
                SQL = SQL + ComNum.VBLF + "   '0' 수액2,";
                SQL = SQL + ComNum.VBLF + "   '0' 수액3,";
                SQL = SQL + ComNum.VBLF + "   '0' 수액4,";
                SQL = SQL + ComNum.VBLF + "   '0' 수액5,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta9') 수액6,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta12') 혈액1,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta15') 혈액2 ,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta18') 혈액3 ,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta21') 혈액4,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta24') 섭취총량,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta27') URINE,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta223') STOOL,";
                SQL = SQL + ComNum.VBLF + "   '0' 기타배설1,";
                SQL = SQL + ComNum.VBLF + "   '0' 기타배설2,";
                SQL = SQL + ComNum.VBLF + "   '0' 기타배설3,";
                SQL = SQL + ComNum.VBLF + "   '0' 기타배설4,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta502') 기타배설5,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta724') 배설총량";
                SQL = SQL + ComNum.VBLF + "     From KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "   WHERE EMRNO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT EMRNO";
                SQL = SQL + ComNum.VBLF + "    From KOSMOS_EMR.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + (mstrPano) + "'";
                SQL = SQL + ComNum.VBLF + " AND CHARTDATE >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + " AND CHARTDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "  AND FORMNO = 1795)";             

                SQL = SQL + ComNum.VBLF + "   ORDER BY CHARTDATE DESC, CHARTTIME DESC";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }


                if (dt.Rows.Count > 0)
                {
                    nCol = 2;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SSIO_Sheet1.RowCount += 1;
                        nRow = SSIO_Sheet1.RowCount - 1;

                        string strCHARTDATE = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                        string strCHARTTIME = dt.Rows[i]["CHARTTIME"].ToString().Trim();

                        if (strOLDCHARTDATE == strCHARTDATE && VB.Val(strCHARTTIME) <= 700 &&
                            strOLDCHARTDATE.Length > 0 && strCHARTDATE.Length > 0)
                        {
                            SSIO_Sheet1.RowCount += 1;
                            nRow = SSIO_Sheet1.RowCount - 1;

                            SSIO_Sheet1.Cells[nRow, 0].Text = "총 량";
                            SSIO_Sheet1.AddSpanCell(nRow, 0, 1, 2);

                            SSIO_Sheet1.Cells[nRow, 3].Text = nItotal.ToString();
                            SSIO_Sheet1.AddSpanCell(nRow, 3, 1, 3);

                            SSIO_Sheet1.Cells[nRow, 7].Text = nOtotal.ToString();
                            SSIO_Sheet1.AddSpanCell(nRow, 7, 1, 3);

                            nItotal = 0;
                            nOtotal = 0;

                            SSIO_Sheet1.RowCount += 1;
                            nRow = SSIO_Sheet1.RowCount - 1;
                        }

                        SSIO_Sheet1.Cells[nRow, 0].Text = VB.Left(VB.Right(strCHARTDATE, 4), 2) + "/" + VB.Right(strCHARTDATE, 2) + " " +
                            VB.Left(strCHARTTIME, 2) + ":" + VB.Right(strCHARTTIME, 2);

                        SSIO_Sheet1.Cells[nRow, 1].Text = dt.Rows[i]["DUTY"].ToString().Trim();

                        SSIO_Sheet1.Cells[nRow, 3].Text = (VB.Val(dt.Rows[i]["구강1"].ToString().Trim()) +
                            VB.Val(dt.Rows[i]["구강2"].ToString().Trim()) +
                            VB.Val(dt.Rows[i]["구강3"].ToString().Trim())).ToString();

                        nItotal += (int)VB.Val(SSIO_Sheet1.Cells[nRow, 3].Text.Trim());

                        string[] strTemp = dt.Rows[i]["수액6"].ToString().Trim().Split(Environment.NewLine.ToCharArray());
                        int n수액 = 0;
                        for (int jj = 0; jj < strTemp.Length; jj++)
                        {
                            n수액 += (int)VB.Val(strTemp[jj]);
                        }

                        strTemp = dt.Rows[i]["기타배설5"].ToString().Trim().Split(Environment.NewLine.ToCharArray());
                        int n기타배설 = 0;
                        for (int jj = 0; jj < strTemp.Length; jj++)
                        {
                            n기타배설 += (int)VB.Val(strTemp[jj]);
                        }

                        SSIO_Sheet1.Cells[nRow, 4].Text = (VB.Val(dt.Rows[i]["수액1"].ToString().Trim()) +
                            VB.Val(dt.Rows[i]["수액2"].ToString().Trim()) + VB.Val(dt.Rows[i]["수액3"].ToString().Trim()) +
                            VB.Val(dt.Rows[i]["수액4"].ToString().Trim()) + VB.Val(dt.Rows[i]["수액5"].ToString().Trim()) + n수액).ToString();
                        nItotal += (int)VB.Val(SSIO_Sheet1.Cells[nRow, 4].Text.Trim());

                        SSIO_Sheet1.Cells[nRow, 5].Text = (VB.Val(dt.Rows[i]["혈액1"].ToString().Trim()) +
                            VB.Val(dt.Rows[i]["혈액2"].ToString().Trim()) + VB.Val(dt.Rows[i]["혈액3"].ToString().Trim()) +
                            VB.Val(dt.Rows[i]["혈액4"].ToString().Trim())).ToString();
                        nItotal += (int)VB.Val(SSIO_Sheet1.Cells[nRow, 5].Text.Trim());

                        SSIO_Sheet1.Cells[nRow, 7].Text = dt.Rows[i]["URINE"].ToString().Trim();
                        nOtotal += (int)VB.Val(SSIO_Sheet1.Cells[nRow, 7].Text.Trim());

                        SSIO_Sheet1.Cells[nRow, 8].Text = dt.Rows[i]["STOOL"].ToString().Trim();
                        nOtotal += (int)VB.Val(SSIO_Sheet1.Cells[nRow, 8].Text.Trim());


                        SSIO_Sheet1.Cells[nRow, 9].Text = (VB.Val(dt.Rows[i]["기타배설1"].ToString().Trim()) +
                            VB.Val(dt.Rows[i]["기타배설2"].ToString().Trim()) + VB.Val(dt.Rows[i]["기타배설3"].ToString().Trim()) +
                            VB.Val(dt.Rows[i]["기타배설4"].ToString().Trim()) + n기타배설).ToString();
                        nOtotal += (int)VB.Val(SSIO_Sheet1.Cells[nRow, 9].Text.Trim());


                        if (dt.Rows[i]["gbn"].ToString().Trim() == "ICU")
                        {
                            SSIO_Sheet1.Rows[nRow].ForeColor = System.Drawing.Color.FromArgb(0, 0, 255);
                        }

                        strOLDCHARTDATE = strCHARTDATE;
                        strOLDCHARTTIME = strCHARTTIME;

                        //SSIO_Sheet1.Cells[nRow, nCol].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        //SSIO_Sheet1.Cells[nRow, nCol].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                        //SSIO_Sheet1.Columns[nCol].Width = 50;
                        //SSIO_Sheet1.Columns[nCol].Width = SSIO_Sheet1.Columns[nCol].GetPreferredWidth() + 5;
                    }

                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return;
        }


        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string strFormName = "섭취배설 History";

            //'Print Head 지정
            string strFont1 = @"/fn""바탕체"" /fz""14"" /fb1 /fi0 /fu0 /fk0 /fs1";
            string strFont2 = @"/fn""바탕체"" /fz""8"" /fb0 /fi0 /fu0 /fk0 /fs2";
            string strHead1 = @"/c/f1" + strFormName + "/f1/n/n";
            string strHead2 = @"/n/l/f2" + "환자번호 : " + mstrPano +
                              VB.Space(5) + "환자성명 : " + mstrName +
                              VB.Space(5) + "성별/나이 : " + mstrSex + "/" + mstrAge + "세" +
                              "              주민번호 : " + mstrJumin1 + "-" + mstrJumin2 + "/n/n";
            string strFooter = "/n/l/f2" + "포항성모병원" + VB.Space(10) + "출력일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + VB.Space(10) + "출력자 : " + clsType.User.UserName;
            strFooter += "/n/l/f2" + "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.";
            strFooter += "/n/l/f2" + "This is an electronically authorized offical medical record";


            SSIO_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            SSIO_Sheet1.PrintInfo.Footer = strFooter;
            SSIO_Sheet1.PrintInfo.Margin.Left = 20;
            SSIO_Sheet1.PrintInfo.Margin.Right = 20;
            SSIO_Sheet1.PrintInfo.Margin.Top = 20;
            SSIO_Sheet1.PrintInfo.Margin.Bottom = 20;
            SSIO_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            SSIO_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            SSIO_Sheet1.PrintInfo.ShowBorder = true;
            SSIO_Sheet1.PrintInfo.ShowColor = false;
            SSIO_Sheet1.PrintInfo.ShowGrid = true;
            SSIO_Sheet1.PrintInfo.ShowShadows = false;
            SSIO_Sheet1.PrintInfo.UseMax = false;
            SSIO_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            SSIO_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            SSIO_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            SSIO.PrintSheet(0);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
