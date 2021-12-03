using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win;
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public partial class frmEmrBaseViewVitalandActing : Form
    {
        #region // 변수선언
        EmrPatient AcpEmr = null;
        string mstrPTNO = "";
        string mstrINOUTCLS = "";
        string mstrMEDFRDATE = "";
        string mstrDEPTCD = "";

        /// <summary>
        /// 대분류 폰트
        /// </summary>
        Font boldFont = null;

        /// <summary>
        /// 대분류
        /// </summary>
        List<string> KeyRow = null;

        /// <summary>
        /// 대분류
        /// </summary>
        List<int> lstRow = null;

        /// <summary>
        /// 바이탈
        /// </summary>
        Dictionary<string, int> keyVSRow = null;
        /// <summary>
        /// 간호활동
        /// </summary>
        Dictionary<string, int> keyActRow = null;
        /// <summary>
        /// 산소/인공호흡
        /// </summary>
        Dictionary<string, int> keySpo2 = null;
        /// <summary>
        /// 상처/중심정맥관/욕창간호활동
        /// </summary>
        Dictionary<string, int> keyRecord3 = null;

        /// <summary>
        /// IO 합계폼
        /// </summary>
        frmNrIONew2 frmNrIONewX = null;

        /// <summary>
        /// 대분류 구분
        /// </summary>
        ComplexBorderSide borderSide = null;
        /// <summary>
        /// 대분류 구분
        /// </summary>
        ComplexBorder complexBorder = null;
        #endregion // 변수선언

        #region 
        //private struct ChartTime
        //{
        //    public string CHARTDATE;
        //    public string CHARTTIME;
        //    public string EMRNO;
        //}
        #endregion

        #region // 생성자
        public frmEmrBaseViewVitalandActing()
        {
            InitializeComponent();
        }

        public frmEmrBaseViewVitalandActing(string strPTNO, string strINOUTCLS, string strMEDFRDATE, string strDEPTCD)
        {
            InitializeComponent();
            mstrPTNO = strPTNO;
            mstrINOUTCLS = strINOUTCLS;
            mstrMEDFRDATE = strMEDFRDATE;
            mstrDEPTCD = strDEPTCD;
        }

        private void frmEmrBaseViewVitalandActing_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            lstRow = new List<int>();
            KeyRow = new List<string>();
            keyVSRow = new Dictionary<string, int>();
            keyActRow = new Dictionary<string, int>();
            keySpo2 = new Dictionary<string, int>();
            keyRecord3 = new Dictionary<string, int>();

            borderSide = new ComplexBorderSide(Color.Black, 1);
            complexBorder = new ComplexBorder(null, null, null, borderSide, null, false, false);

            ssVital_Sheet1.RowCount = 0;
            ssVital_Sheet1.Columns.Count = 3;
            ssAct_Sheet1.RowCount = 0;
            ssAct_Sheet1.Columns.Count = 3;
            ssSpo2RR_Sheet1.RowCount = 0;
            ssSpo2RR_Sheet1.Columns.Count = 3;

            AcpEmr = clsEmrChart.ClearPatient();
            AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, mstrPTNO, mstrINOUTCLS, mstrMEDFRDATE, mstrDEPTCD);

            SetPatientInfo();

            if (AcpEmr != null)
            {
                boldFont = new Font("굴림체", 10, FontStyle.Bold);
                GetData(btnSearchAll);
            }
        }
        #endregion // 생성자

        #region // Functions       
        private void SetPatientInfo()
        {
            if (AcpEmr == null) return;

            //dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(AcpEmr.medFrDate, "D"));

            //if (AcpEmr.medEndDate == "" || AcpEmr.medEndDate == "99991231" || AcpEmr.medEndDate == "99981231")
            //{
            //    dtpEndDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            //}
            //else
            //{
            //    dtpEndDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(AcpEmr.medEndDate, "D"));
            //}

            Text = "등록번호 : " + AcpEmr.ptNo + " 이름 : " + AcpEmr.ptName;

            dtpFrDate.Value = Convert.ToDateTime(clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, AcpEmr.inOutCls, AcpEmr.medDeptCd, clsType.User.IdNumber,
                        DateTime.ParseExact(AcpEmr.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd")));

            dtpEndDate.Value = Convert.ToDateTime(AcpEmr.medEndDate != "" ? ComFunc.FormatStrToDate(AcpEmr.medEndDate, "D") : ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
        }

        /// <summary>
        /// 데이타를 조회한다
        /// </summary>
        private void GetData(object sender)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return;

            if (AcpEmr == null)
            {
                return;
            }

            if (sender.Equals(btnSearchFilter) &&
                (ssAct_Sheet1.RowCount > 0 || ssRecord3_Sheet1.RowCount > 0 ||
                ssVital_Sheet1.RowCount > 0 || ssSpo2RR_Sheet1.RowCount > 0))
            {
                List<string> strFilter1 = new List<string>();
                List<string> strFilter2 = new List<string>();
                List<string> strFilter3 = new List<string>();
                List<string> strFilter4 = new List<string>();

                if (ssVital_Sheet1.RowCount > 0)
                {
                    for (int i = 0; i < ssVital_Sheet1.RowCount; i++)
                    {
                        if (ssVital_Sheet1.RowFilter.IsRowFilteredOut(i) == false &&
                            !string.IsNullOrWhiteSpace(ssVital_Sheet1.Cells[i, 0].Text.Trim()))
                        {
                            strFilter1.Add("'" + ssVital_Sheet1.Cells[i, 0].Text.Trim() + "'");
                        }
                    }
                }

                if (ssAct_Sheet1.RowCount > 0)
                {
                    for (int i = 0; i < ssAct_Sheet1.RowCount; i++)
                    {
                        if (ssAct_Sheet1.RowFilter.IsRowFilteredOut(i) == false &&
                            !string.IsNullOrWhiteSpace(ssAct_Sheet1.Cells[i, 0].Text.Trim()))
                        {
                            strFilter2.Add("'" + ssAct_Sheet1.Cells[i, 0].Text.Trim() + "'");
                        }
                    }
                }

                if (ssSpo2RR_Sheet1.RowCount > 0)
                {

                    for (int i = 0; i < ssSpo2RR_Sheet1.RowCount; i++)
                    {
                        if (ssSpo2RR_Sheet1.RowFilter.IsRowFilteredOut(i) == false &&
                            !string.IsNullOrWhiteSpace(ssSpo2RR_Sheet1.Cells[i, 0].Text.Trim()))
                        {
                            strFilter3.Add("'" + ssSpo2RR_Sheet1.Cells[i, 0].Text.Trim() + "'");
                        }
                    }
                }


                if (ssRecord3_Sheet1.RowCount > 0)
                {
                    for (int i = 0; i < ssRecord3_Sheet1.RowCount; i++)
                    {
                        if (ssRecord3_Sheet1.RowFilter.IsRowFilteredOut(i) == false &&
                            !string.IsNullOrWhiteSpace(ssRecord3_Sheet1.Cells[i, 0].Text.Trim()))
                        {
                            strFilter4.Add("'" + ssRecord3_Sheet1.Cells[i, 0].Text.Trim() + "'");
                        }
                    }
                }

                if (strFilter1.Count > 0 || strFilter2.Count > 0 || strFilter3.Count > 0 || strFilter4.Count > 0)
                {
                    FormSearchITEM(strFilter1, strFilter2, strFilter3, strFilter4);
                    GetDataValue(strFilter1, strFilter2, strFilter3, strFilter4);
                    return;
                }
            }


            FormSearchITEM();
            //FormSearchIVT();
            //FormSearchAct();
            //FormSearchSpo2RR();
            //FormSearchRecord3();
        }

        private void FormSearchITEM(List<string> Filter1 = null, List<string> Filter2 = null, List<string> Filter3 = null, List<string> Filter4 = null)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;

            int i = 0;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssVital_Sheet1.RowCount = 0;
            ssVital_Sheet1.Columns.Count = 3;

            ssAct_Sheet1.RowCount = 0;
            ssAct_Sheet1.Columns.Count = 3;

            ssSpo2RR_Sheet1.RowCount = 0;
            ssSpo2RR_Sheet1.Columns.Count = 3;

            ssRecord3_Sheet1.RowCount = 0;
            ssRecord3_Sheet1.Columns.Count = 3;


            #region 쿼리
            SQL.AppendLine("WITH ROW_LIST AS  ");
            SQL.AppendLine("( ");
            SQL.AppendLine("   SELECT C.FORMNO, R.ITEMCD");
            SQL.AppendLine("     FROM KOSMOS_EMR.AEMRCHARTMST C ");
            SQL.AppendLine("       INNER JOIN KOSMOS_EMR.AEMRCHARTROW R ");
            SQL.AppendLine("          ON C.EMRNO = R.EMRNO ");
            SQL.AppendLine("         AND C.EMRNOHIS = R.EMRNOHIS");
            SQL.AppendLine("         AND R.ITEMVALUE IS NOT NULL"); // OR R.ITEMVALUE1 IS NOT NULL) ");SQL = SQL + ComNum.VBLF + "       WHERE C.ACPNO = " + AcpEmr.acpNo;
            SQL.AppendLine("    WHERE C.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ");
            SQL.AppendLine("      AND C.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ");
            SQL.AppendLine("      AND C.FORMNO IN(1575, 3150, 2135, 1935, 1969, 2431, 2431, 1725, 2638, 1573) ");
            SQL.AppendLine("      AND C.PTNO = '" + AcpEmr.ptNo + "'");
            SQL.AppendLine("      AND C.CHARTUSEID <> '합계' ");
            SQL.AppendLine(") ");
            SQL.AppendLine("SELECT ");
            SQL.AppendLine("     1 AS GBN");
            SQL.AppendLine("     , CASE ");
            SQL.AppendLine("         WHEN B.UNITCLS = '임상관찰' THEN 0 ");
            SQL.AppendLine("         WHEN B.UNITCLS = '섭취배설' THEN 1 ");
            SQL.AppendLine("         WHEN B.UNITCLS = '특수치료' THEN 2 ");
            SQL.AppendLine("         ELSE 3 ");
            SQL.AppendLine("     END AS GRPSORT ");
            SQL.AppendLine("     , (SELECT BB.DISSEQNO FROM " + ComNum.DB_EMR + "AEMRBASCD BB");
            SQL.AppendLine("         WHERE BB.BASCD = B.VFLAG1 ");
            SQL.AppendLine("         AND BB.BSNSCLS = '기록지관리'");
            SQL.AppendLine("         AND BB.UNITCLS = B.UNITCLS || '그룹') AS GRPSEQ");
            SQL.AppendLine("     ,B.BASCD, B.BASNAME, B.BASEXNAME ");
            SQL.AppendLine("     ,B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, B.DISSEQNO ");
            SQL.AppendLine("  FROM " + ComNum.DB_EMR + "AEMRBASCD B");
            SQL.AppendLine(" WHERE B.BSNSCLS = '기록지관리'  ");
            SQL.AppendLine("   AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')");
            SQL.AppendLine("   AND B.USECLS = '0' ");
            if (Filter1 != null && Filter1.Count > 0)
            {
                SQL.AppendLine("   AND B.BASCD IN");
                SQL.AppendLine(" (");
                SQL.AppendLine(string.Join(",", Filter1));
                SQL.AppendLine(" )");
            }

            SQL.AppendLine("   AND EXISTS (SELECT 1");
            SQL.AppendLine("                 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U");
            SQL.AppendLine("                 INNER JOIN ROW_LIST C ");
            SQL.AppendLine("                    ON C.FORMNO = 3150");
            SQL.AppendLine("                   AND C.ITEMCD = U.ITEMCD");
            SQL.AppendLine("                WHERE U.JOBGB = 'IVT'");
            SQL.AppendLine("                  AND U.USEGB IN('" + clsType.User.IdNumber + "', " + " '" + clsType.User.BuseCode + "')");
            SQL.AppendLine("                  AND U.ITEMCD = B.BASCD");
            SQL.AppendLine("              )");

            SQL.AppendLine("UNION ALL ");
            SQL.AppendLine("SELECT ");
            SQL.AppendLine("     2 AS GBN, 0 AS GRPSORT, 0 AS GRPSEQ, B.BASCD, B.BASNAME, B.BASEXNAME,  ");
            SQL.AppendLine("     B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, BB.DISSEQNO ");
            SQL.AppendLine("FROM " + ComNum.DB_EMR + "AEMRBASCD B");
            SQL.AppendLine("INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB");
            SQL.AppendLine("     ON B.VFLAG1 = BB.BASCD ");
            SQL.AppendLine("     AND BB.BSNSCLS = '기록지관리'  ");
            SQL.AppendLine("     AND BB.UNITCLS = '간호활동그룹'"); //간호활동항목
            SQL.AppendLine("     AND BB.USECLS = '0' ");
            SQL.AppendLine("WHERE B.BSNSCLS = '기록지관리'  ");
            SQL.AppendLine("  AND B.UNITCLS = '간호활동항목'"); //간호활동항목
            SQL.AppendLine("  AND B.USECLS = '0' ");
            if (Filter2 != null && Filter2.Count > 0)
            {
                SQL.AppendLine("   AND B.BASCD IN");
                SQL.AppendLine(" (");
                SQL.AppendLine(string.Join(",", Filter2));
                SQL.AppendLine(" )");
            }

            SQL.AppendLine("   AND EXISTS (SELECT 1");
            SQL.AppendLine("                 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U");
            SQL.AppendLine("                 INNER JOIN ROW_LIST C ");
            SQL.AppendLine("                    ON C.FORMNO = 1575");
            SQL.AppendLine("                   AND C.ITEMCD = U.ITEMCD");
            SQL.AppendLine("                WHERE U.JOBGB = 'ACT'");
            SQL.AppendLine("                  AND U.USEGB IN('" + clsType.User.IdNumber + "', " + " '" + clsType.User.BuseCode + "')");
            SQL.AppendLine("                  AND U.ITEMCD = B.BASCD");
            SQL.AppendLine("              )");

            #region SP
            SQL.AppendLine("UNION ALL ");
            SQL.AppendLine("SELECT ");
            SQL.AppendLine("     3 AS GBN, 0 AS GRPSORT, 0 AS GRPSEQ, B.BASCD, B.BASNAME, B.BASEXNAME,  ");
            SQL.AppendLine("     B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, BB.DISSEQNO ");
            SQL.AppendLine(" FROM " + ComNum.DB_EMR + "AEMRBASCD B");
            SQL.AppendLine("   INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB");
            SQL.AppendLine("      ON B.VFLAG1 = BB.BASCD ");
            SQL.AppendLine("     AND BB.BSNSCLS = '기록지관리'  ");
            SQL.AppendLine("     AND BB.UNITCLS > CHR(0)");
            //SQL.AppendLine("     AND BB.BASCD > CHR(0)");
            SQL.AppendLine("     AND BB.APLFRDATE > CHR(0) ");
            SQL.AppendLine("     AND BB.BASNAME IN('호흡간호', '인공기도') ");
            SQL.AppendLine("     AND BB.USECLS = '0' ");
            SQL.AppendLine("WHERE B.BSNSCLS = '기록지관리'  ");
            SQL.AppendLine("  AND B.UNITCLS > CHR(0)");
            SQL.AppendLine("  AND B.USECLS = '0' ");
            SQL.AppendLine("  AND B.APLFRDATE > CHR(0) ");
            SQL.AppendLine("  AND B.BASEXNAME IN('호흡간호', '인공기도') ");
            if (Filter3 != null && Filter3.Count > 0)
            {
                SQL.AppendLine("   AND B.BASCD IN");
                SQL.AppendLine(" (");
                SQL.AppendLine(string.Join(",", Filter3));
                SQL.AppendLine(" )");
            }

            SQL.AppendLine("  AND EXISTS (SELECT 1");
            SQL.AppendLine("                 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U");
            SQL.AppendLine("                 INNER JOIN ROW_LIST C ");
            SQL.AppendLine("                    ON C.FORMNO  = 1575");
            SQL.AppendLine("                   AND C.ITEMCD = U.ITEMCD");
            SQL.AppendLine("                WHERE U.JOBGB = 'SP'");
            SQL.AppendLine("                  AND U.USEGB IN('" + clsType.User.IdNumber + "', " + " '" + clsType.User.BuseCode + "')");
            SQL.AppendLine("                  AND U.ITEMCD = B.BASCD");
            SQL.AppendLine("              )");

            SQL.AppendLine("UNION ALL ");
            SQL.AppendLine("SELECT ");
            SQL.AppendLine("     3 AS GBN, 0 AS GRPSORT, 0 AS GRPSEQ, B.BASCD, B.BASNAME, B.BASEXNAME,  ");
            SQL.AppendLine("     B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, BB.DISSEQNO ");
            SQL.AppendLine(" FROM " + ComNum.DB_EMR + "AEMRBASCD B");
            SQL.AppendLine("   INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB");
            SQL.AppendLine("      ON B.VFLAG1 = BB.BASCD ");
            SQL.AppendLine("     AND BB.BSNSCLS = '기록지관리'  ");
            SQL.AppendLine("     AND BB.UNITCLS > CHR(0)");
            //SQL.AppendLine("     AND BB.BASCD > CHR(0)");
            SQL.AppendLine("     AND BB.APLFRDATE > CHR(0) ");
            SQL.AppendLine("     AND BB.BASNAME IN('산소요법', '인공호흡기' , '흡인간호(Suction)') ");
            SQL.AppendLine("     AND BB.USECLS = '0' ");
            SQL.AppendLine("WHERE B.BSNSCLS = '기록지관리'  ");
            SQL.AppendLine("  AND B.UNITCLS > CHR(0)");
            SQL.AppendLine("  AND B.USECLS = '0' ");
            SQL.AppendLine("  AND B.APLFRDATE > CHR(0) ");
            SQL.AppendLine("  AND B.BASEXNAME IN('산소요법', '인공호흡기' , '흡인간호', '흡인간호(Suction)') ");
            if (Filter3 != null && Filter3.Count > 0)
            {
                SQL.AppendLine("   AND B.BASCD IN");
                SQL.AppendLine(" (");
                SQL.AppendLine(string.Join(",", Filter3));
                SQL.AppendLine(" )");
            }


            SQL.AppendLine("  AND EXISTS (SELECT 1");
            SQL.AppendLine("                 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U");
            SQL.AppendLine("                 INNER JOIN ROW_LIST C ");
            SQL.AppendLine("                    ON C.FORMNO = 3150");
            SQL.AppendLine("                   AND C.ITEMCD = U.ITEMCD");
            SQL.AppendLine("                WHERE U.JOBGB = 'SP2'");
            SQL.AppendLine("                  AND U.USEGB IN('" + clsType.User.IdNumber + "', " + " '" + clsType.User.BuseCode + "')");
            SQL.AppendLine("                  AND U.ITEMCD = B.BASCD");
            SQL.AppendLine("              )");
            #endregion


            SQL.AppendLine("UNION ALL ");
            SQL.AppendLine("SELECT ");
            SQL.AppendLine("     4 AS GBN, 0 AS GRPSORT, 0 AS GRPSEQ, B.ITEMNO AS BASCD, B.ITEMNAME AS BASNAME, A.FORMNAME  AS BASEXNAME,  ");
            SQL.AppendLine("     DECODE(A.FORMNO, 1725, 0, 2638, 1, 1573, 2) AS NFLAG1, B.ITEMNUMBER AS NFLAG2, 0 AS NFLAG2, 0 AS NFLAG3, 0 AS DISSEQNO");
            SQL.AppendLine("  FROM KOSMOS_EMR.AEMRFORM A");
            SQL.AppendLine("    INNER JOIN KOSMOS_EMR.AEMRFLOWXML B");
            SQL.AppendLine("       ON A.FORMNO = B.FORMNO");
            SQL.AppendLine("      AND A.UPDATENO = B.UPDATENO");
            SQL.AppendLine("      AND B.ITEMNUMBER >= 0");
            if (Filter4 != null && Filter4.Count > 0)
            {
                SQL.AppendLine("      AND B.ITEMNO IN");
                SQL.AppendLine(" (");
                SQL.AppendLine(string.Join(",", Filter4));
                SQL.AppendLine(" )");
            }

            SQL.AppendLine(" WHERE A.FORMNO IN (1725, 2638, 1573)");
            SQL.AppendLine("   AND A.UPDATENO > 0");
            SQL.AppendLine("   AND A.USECHECK = 1");
            SQL.AppendLine("   AND EXISTS (SELECT 1");
            SQL.AppendLine("                 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U");
            SQL.AppendLine("                 INNER JOIN ROW_LIST C ");
            SQL.AppendLine("                    ON C.ITEMCD = U.ITEMCD");
            SQL.AppendLine("                WHERE U.JOBGB IN('WD', 'BD', 'CVC') ");
            SQL.AppendLine("                  AND U.USEGB IN('" + clsType.User.IdNumber + "', " + " '" + clsType.User.BuseCode + "')");
            SQL.AppendLine("                  AND U.ITEMCD = B.ITEMNO");
            SQL.AppendLine("                  AND C.FORMNO = A.FORMNO");
            SQL.AppendLine("              )");

            SQL.AppendLine("ORDER BY GBN, GRPSORT, GRPSEQ, BASEXNAME, NFLAG1, BASVAL, NFLAG2, NFLAG3, DISSEQNO");
            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strBASEXNAME = "";
            int intS = 0;

            keyVSRow.Clear();
            keyActRow.Clear();
            keySpo2.Clear();
            keyRecord3.Clear();
            KeyRow.Clear();
            lstRow.Clear();

            SheetView sheetView = null;
            int nRow = -1;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                string Key = dt.Rows[i]["BASEXNAME"].ToString().Trim() + dt.Rows[i]["BASCD"].ToString().Trim();
                if (dt.Rows[i]["GBN"].ToString().Equals("1"))
                {
                    sheetView = ssVital_Sheet1;
                    if (keyVSRow.ContainsKey(Key) == false)
                    {
                        //intS = 0;
                        sheetView.RowCount += 1;
                        keyVSRow.Add(Key, sheetView.RowCount - 1);
                    }
                }
                else if (dt.Rows[i]["GBN"].ToString().Equals("2"))
                {
                    if (sheetView.Equals(ssAct_Sheet1) == false && sheetView.RowCount > 0 && sheetView.RowCount - intS > 0)
                    {
                        sheetView.AddSpanCell(intS, 1, sheetView.RowCount - intS, 1);
                    }
                    sheetView = ssAct_Sheet1;
                    if (keyActRow.ContainsKey(Key) == false)
                    {
                        //intS = 0;
                        sheetView.RowCount += 1;
                        keyActRow.Add(Key, sheetView.RowCount - 1);
                    }
                }
                else if (dt.Rows[i]["GBN"].ToString().Equals("3"))
                {
                    if (sheetView.Equals(ssSpo2RR_Sheet1) == false && sheetView.RowCount > 0 && sheetView.RowCount - intS > 0)
                    {
                        sheetView.AddSpanCell(intS, 1, sheetView.RowCount - intS, 1);
                    }
                    sheetView = ssSpo2RR_Sheet1;
                    if (keySpo2.ContainsKey(Key) == false)
                    {
                        //intS = 0;
                        sheetView.RowCount += 1;
                        keySpo2.Add(Key, sheetView.RowCount - 1);
                    }
                }
                else if (dt.Rows[i]["GBN"].ToString().Equals("4"))
                {
                    if (sheetView.Equals(ssRecord3_Sheet1) == false && sheetView.RowCount > 0 && sheetView.RowCount - intS > 0)
                    {
                        if (sheetView.RowCount > 0 && nRow - intS > 0)
                        {
                            sheetView.AddSpanCell(intS, 1, sheetView.RowCount - intS, 1);
                        }

                    }
                    sheetView = ssRecord3_Sheet1;
                    if (keyRecord3.ContainsKey(Key) == false)
                    {
                        //intS = 0;
                        sheetView.RowCount += 1;
                        keyRecord3.Add(Key, sheetView.RowCount - 1);
                    }
                }

                nRow = sheetView.RowCount - 1;

                sheetView.Cells[nRow, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                sheetView.Cells[nRow, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                sheetView.Cells[nRow, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();

                //2021-04-13
                if (sheetView.GetPreferredRowHeight(nRow) > ComNum.SPDROWHT)
                {
                    sheetView.SetRowHeight(nRow, Convert.ToInt32((sheetView.GetPreferredRowHeight(nRow) + 5)));
                }
                else
                {
                    sheetView.SetRowHeight(nRow, ComNum.SPDROWHT);
                }

                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    sheetView.Cells[nRow, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    sheetView.Cells[nRow, 1].Font = boldFont;
                    sheetView.Cells[nRow, 1].Border = complexBorder;

                    if (nRow != 0)
                    {
                        KeyRow.Add(dt.Rows[i]["GBN"].ToString().Trim() + "|" + (nRow - 1));
                        if (sheetView.RowCount > 0 && nRow - intS > 0)
                        {
                            sheetView.AddSpanCell(intS, 1, nRow - intS, 1);
                        }
                    }
                    intS = nRow;

                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }

            if (nRow > 0 && sheetView.RowCount - intS > 0)
            {
                sheetView.AddSpanCell(intS, 1, sheetView.RowCount - intS, 1);
            }

            sheetView.SetRowHeight(-1, 40);
            dt.Dispose();
            dt = null;

            GetDataValue();
            Cursor.Current = Cursors.Default;
        }

        private void FormSearchIVT()
        {
            string SQL = "";

            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssVital_Sheet1.RowCount = 0;
            ssVital_Sheet1.Columns.Count = 3;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "     GRPSORT, GRPSEQ, BASCD, BASNAME, BASEXNAME, ";
            SQL = SQL + ComNum.VBLF + "     NFLAG1, BASVAL, NFLAG2, NFLAG3, DISSEQNO ";
            SQL = SQL + ComNum.VBLF + "FROM (";
            SQL = SQL + ComNum.VBLF + "     SELECT ";
            SQL = SQL + ComNum.VBLF + "         CASE ";
            SQL = SQL + ComNum.VBLF + "             WHEN B.UNITCLS = '임상관찰' THEN 0 ";
            SQL = SQL + ComNum.VBLF + "             WHEN B.UNITCLS = '섭취배설' THEN 1 ";
            SQL = SQL + ComNum.VBLF + "             WHEN B.UNITCLS = '특수치료' THEN 2 ";
            SQL = SQL + ComNum.VBLF + "             ELSE 3 ";
            SQL = SQL + ComNum.VBLF + "         END AS GRPSORT, ";
            SQL = SQL + ComNum.VBLF + "         B.BASCD, B.BASNAME, B.BASEXNAME, ";
            SQL = SQL + ComNum.VBLF + "         B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, B.DISSEQNO, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT BB.DISSEQNO FROM " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "             WHERE BB.BASCD = B.VFLAG1 ";
            SQL = SQL + ComNum.VBLF + "             AND BB.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "             AND BB.UNITCLS = B.UNITCLS || '그룹') AS GRPSEQ";
            SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "     WHERE B.BSNSCLS = '기록지관리'  ";
            SQL = SQL + ComNum.VBLF + "          AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')";
            SQL = SQL + ComNum.VBLF + "          AND B.USECLS = '0' ";
            //SQL = SQL + ComNum.VBLF + "          AND EXISTS (SELECT U.ITEMCD FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U";
            //SQL = SQL + ComNum.VBLF + "                      WHERE U.JOBGB = 'IVT'";
            //SQL = SQL + ComNum.VBLF + "                            AND U.USEGB = '" + clsType.User.IdNumber + "'";
            //SQL = SQL + ComNum.VBLF + "                            AND U.ITEMCD = B.BASCD";
            //SQL = SQL + ComNum.VBLF + "                      UNION ";
            //SQL = SQL + ComNum.VBLF + "                      SELECT U.ITEMCD FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U";
            //SQL = SQL + ComNum.VBLF + "                      WHERE U.JOBGB = 'IVT'";
            //SQL = SQL + ComNum.VBLF + "                            AND U.USEGB = '" + clsType.User.BuseCode + "'";
            //SQL = SQL + ComNum.VBLF + "                            AND U.ITEMCD = B.BASCD";
            //SQL = SQL + ComNum.VBLF + "                     )";
            SQL = SQL + ComNum.VBLF + "           AND B.BASCD IN (SELECT R.ITEMCD FROM KOSMOS_EMR.AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "                           INNER JOIN KOSMOS_EMR.AEMRCHARTROW R ";
            SQL = SQL + ComNum.VBLF + "                               ON C.EMRNO = R.EMRNO ";
            SQL = SQL + ComNum.VBLF + "                               AND C.EMRNOHIS  = R.EMRNOHIS ";
            SQL = SQL + ComNum.VBLF + "                               AND R.ITEMCD IN (SELECT U.ITEMCD FROM KOSMOS_EMR.AEMRUSERITEMVS U ";
            SQL = SQL + ComNum.VBLF + "                                                WHERE U.JOBGB = 'IVT' ";
            SQL = SQL + ComNum.VBLF + "                                                       AND U.USEGB = '" + clsType.User.IdNumber + "' ";
            SQL = SQL + ComNum.VBLF + "                                                UNION ";
            SQL = SQL + ComNum.VBLF + "                                                SELECT U.ITEMCD FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U";
            SQL = SQL + ComNum.VBLF + "                                                WHERE U.JOBGB = 'IVT'";
            SQL = SQL + ComNum.VBLF + "                                                      AND U.USEGB = '" + clsType.User.BuseCode + "'";
            SQL = SQL + ComNum.VBLF + "                                                )";
            SQL = SQL + ComNum.VBLF + "            AND R.ITEMVALUE IS NOT NULL"; // OR R.ITEMVALUE1 IS NOT NULL) ";
            SQL = SQL + ComNum.VBLF + "                       WHERE C.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "                           AND C.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "                           AND C.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "                           AND C.FORMNO IN (3150, 2135, 1935, 1969, 2431, 2431) -- 임상관찰,회복실, Angio, 응급실, 진정, 내시경 ";
            SQL = SQL + ComNum.VBLF + "                           AND C.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "                           AND C.CHARTUSEID <> '합계' ";
            SQL = SQL + ComNum.VBLF + "                       GROUP BY R.ITEMCD ";
            SQL = SQL + ComNum.VBLF + "                      ) ";
            SQL = SQL + ComNum.VBLF + "     )";
            SQL = SQL + ComNum.VBLF + "ORDER BY GRPSORT, GRPSEQ, NFLAG1, BASVAL, NFLAG2, NFLAG3, DISSEQNO";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strBASEXNAME = "";
            int intS = 0;


            ssVital_Sheet1.RowCount = dt.Rows.Count;
            keyVSRow.Clear();
            lstRow.Clear();

            for (i = 0; i < dt.Rows.Count; i++)
            {
                keyVSRow.Add(dt.Rows[i]["BASCD"].ToString().Trim(), i);
                ssVital_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssVital_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                ssVital_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                ssVital_Sheet1.SetRowHeight(i, ComNum.SPDROWHT);
                //sheetView.SetRowHeight(i, Convert.ToInt32((sheetView.GetPreferredRowHeight(i) + 2)));
                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    ssVital_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        ssVital_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
                        lstRow.Add(i - 1);
                    }
                    intS = i;
                    ssVital_Sheet1.Cells[i, 1].Font = boldFont;
                    ssVital_Sheet1.Cells[i, 1].Border = complexBorder;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            ssVital_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
            ssVital_Sheet1.SetRowHeight(-1, 40);
            dt.Dispose();
            dt = null;

            GetVitalValue();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 모든 기록지 데이터 가져오기
        /// </summary>
        private void GetDataValue(List<string> Filter1 = null, List<string> Filter2 = null, List<string> Filter3 = null, List<string> Filter4 = null)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.MaxLength = 1000;
            TypeText.Multiline = true;
            TypeText.WordWrap = true;

            Cursor.Current = Cursors.WaitCursor;
            ssVital_Sheet1.Columns.Count = 3;
            ssAct_Sheet1.ColumnCount = 3;
            ssRecord3_Sheet1.ColumnCount = 3;
            ssSpo2RR_Sheet1.ColumnCount = 3;



            SQL.AppendLine("WITH ROW_LIST AS  ");
            SQL.AppendLine("( ");
            SQL.AppendLine("   SELECT ");
            SQL.AppendLine("           C.FORMNO");
            SQL.AppendLine("         , C.CHARTDATE");
            SQL.AppendLine("         , C.CHARTTIME");
            SQL.AppendLine("         , C.CHARTUSEID");
            SQL.AppendLine("         , C.EMRNO");
            SQL.AppendLine("         , R.ITEMCD");
            SQL.AppendLine("         , R.ITEMNO");
            SQL.AppendLine("         , R.ITEMVALUE");
            SQL.AppendLine("         , R.ITEMVALUE1 ");
            SQL.AppendLine("     FROM KOSMOS_EMR.AEMRCHARTMST C ");
            SQL.AppendLine("       INNER JOIN KOSMOS_EMR.AEMRCHARTROW R ");
            SQL.AppendLine("          ON C.EMRNO = R.EMRNO ");
            SQL.AppendLine("         AND C.EMRNOHIS = R.EMRNOHIS");
            SQL.AppendLine("         AND R.ITEMVALUE IS NOT NULL");
            SQL.AppendLine("    WHERE C.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ");
            SQL.AppendLine("      AND C.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ");
            SQL.AppendLine("      AND C.FORMNO IN(1575, 3150, 2135, 1935, 1969, 2431, 2431, 1725, 2638, 1573) ");
            SQL.AppendLine("      AND C.PTNO = '" + AcpEmr.ptNo + "'");
            SQL.AppendLine("      AND C.CHARTUSEID <> '합계' ");
            SQL.AppendLine(") ");

            #region VITAL 
            SQL.AppendLine("SELECT ");
            SQL.AppendLine("    1 AS  GBN");
            //SQL.AppendLine("         , C.FORMNO");
            SQL.AppendLine("         , C.CHARTDATE");
            SQL.AppendLine("         , C.CHARTTIME");
            SQL.AppendLine("         , C.CHARTUSEID");
            SQL.AppendLine("         , C.EMRNO");
            SQL.AppendLine("         , C.ITEMCD");
            SQL.AppendLine("         , C.ITEMNO");
            SQL.AppendLine("         , C.ITEMVALUE");
            SQL.AppendLine("         , C.ITEMVALUE1 ");
            SQL.AppendLine("         , B.BASEXNAME");
            SQL.AppendLine("         , B.BASNAME ");
            SQL.AppendLine("  FROM ROW_LIST C ");
            SQL.AppendLine("    INNER JOIN KOSMOS_EMR.AEMRBASCD B ");
            SQL.AppendLine("       ON C.ITEMCD = B.BASCD ");
            SQL.AppendLine("      AND C.FORMNO IN (3150, 2135, 1935, 1969, 2431, 2431) -- 임상관찰,회복실, Angio, 응급실, 진정, 내시경 ");
            SQL.AppendLine("      AND B.BSNSCLS = '기록지관리' ");
            SQL.AppendLine("      AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호') ");
            if (Filter1 != null && Filter1.Count > 0)
            {
                SQL.AppendLine(" WHERE ITEMCD IN");
                SQL.AppendLine(" (");
                SQL.AppendLine(string.Join(",", Filter1));
                SQL.AppendLine(" )");
            }
            else
            {
                SQL.AppendLine(" WHERE EXISTS");
                SQL.AppendLine(" (");
                SQL.AppendLine("   SELECT 1 FROM KOSMOS_EMR.AEMRUSERITEMVS U ");
                SQL.AppendLine("    WHERE U.JOBGB IN('IVT') ");
                SQL.AppendLine("      AND U.USEGB IN('" + clsType.User.IdNumber + "', '" + clsType.User.BuseCode + "')");
                SQL.AppendLine("      AND C.ITEMCD = U.ITEMCD");
                SQL.AppendLine(" )");
            }
            #endregion

            #region 간호활동
            SQL.AppendLine("UNION ALL");
            SQL.AppendLine("SELECT ");
            SQL.AppendLine("      2 AS GBN ");
            //SQL.AppendLine("          , C.FORMNO");
            SQL.AppendLine("          , C.CHARTDATE");
            SQL.AppendLine("          , C.CHARTTIME");
            SQL.AppendLine("          , C.CHARTUSEID");
            SQL.AppendLine("          , C.EMRNO");
            SQL.AppendLine("          , C.ITEMCD");
            SQL.AppendLine("          , C.ITEMNO");
            SQL.AppendLine("          , C.ITEMVALUE");
            SQL.AppendLine("          , C.ITEMVALUE1 ");
            SQL.AppendLine("          , B.BASEXNAME");
            SQL.AppendLine("          , B.BASNAME ");
            SQL.AppendLine("FROM ROW_LIST C ");
            SQL.AppendLine("  INNER JOIN KOSMOS_EMR.AEMRBASCD B ");
            SQL.AppendLine("     ON C.ITEMCD = B.BASCD ");
            SQL.AppendLine("     AND C.FORMNO = 1575");
            SQL.AppendLine("    AND B.BSNSCLS = '기록지관리' ");
            SQL.AppendLine("    AND B.UNITCLS IN ('간호활동항목') ");
            if (Filter2 != null && Filter2.Count > 0)
            {
                SQL.AppendLine(" WHERE ITEMCD IN");
                SQL.AppendLine(" (");
                SQL.AppendLine(string.Join(",", Filter2));
                SQL.AppendLine(" )");
            }
            else
            {
                SQL.AppendLine(" WHERE EXISTS");
                SQL.AppendLine(" (");
                SQL.AppendLine("   SELECT 1 FROM KOSMOS_EMR.AEMRUSERITEMVS U ");
                SQL.AppendLine("    WHERE U.JOBGB IN('ACT') ");
                SQL.AppendLine("      AND U.USEGB IN('" + clsType.User.IdNumber + "', '" + clsType.User.BuseCode + "')");
                SQL.AppendLine("      AND C.ITEMCD = U.ITEMCD");
                SQL.AppendLine(" )");
            }
            #endregion

            #region 3개 합침
            SQL.AppendLine("UNION ALL");
            SQL.AppendLine("SELECT ");
            SQL.AppendLine("      3 AS GBN ");
            //SQL.AppendLine("          , C.FORMNO");
            SQL.AppendLine("          , C.CHARTDATE");
            SQL.AppendLine("          , C.CHARTTIME");
            SQL.AppendLine("          , C.CHARTUSEID");
            SQL.AppendLine("          , C.EMRNO");
            SQL.AppendLine("          , C.ITEMCD");
            SQL.AppendLine("          , C.ITEMNO");
            SQL.AppendLine("          , C.ITEMVALUE");
            SQL.AppendLine("          , C.ITEMVALUE1 ");
            SQL.AppendLine("          , B.BASEXNAME");
            SQL.AppendLine("          , B.BASNAME ");
            SQL.AppendLine("FROM ROW_LIST C ");
            SQL.AppendLine("  INNER JOIN KOSMOS_EMR.AEMRBASCD B                                      ");
            SQL.AppendLine("     ON B.BSNSCLS = '기록지관리'                                            ");
            SQL.AppendLine("    AND B.UNITCLS > CHR(0)                                               ");
            SQL.AppendLine("    AND B.USECLS = '0'                                                   ");
            SQL.AppendLine("    AND B.APLFRDATE > CHR(0)                                             ");
            SQL.AppendLine("    AND B.BASEXNAME IN( '호흡간호', '인공기도') ");
            SQL.AppendLine("    AND C.ITEMCD = B.BASCD");
            SQL.AppendLine("    AND C.FORMNO IN (1575)");
            SQL.AppendLine("  INNER JOIN KOSMOS_EMR.AEMRBASCD BB                                     ");
            SQL.AppendLine("     ON B.VFLAG1 = BB.BASCD                                              ");
            SQL.AppendLine("    AND BB.BSNSCLS = '기록지관리'                                           ");
            SQL.AppendLine("    AND BB.UNITCLS > CHR(0)                                              ");
            SQL.AppendLine("    AND BB.BASCD > CHR(0)                                                ");
            SQL.AppendLine("    AND BB.APLFRDATE > CHR(0)                                            ");
            SQL.AppendLine("    AND BB.BASNAME IN('호흡간호', '인공기도')  ");
            SQL.AppendLine("    AND BB.USECLS = '0'                                                  ");
            if (Filter3 != null && Filter3.Count > 0)
            {
                SQL.AppendLine(" WHERE ITEMCD IN");
                SQL.AppendLine(" (");
                SQL.AppendLine(string.Join(",", Filter3));
                SQL.AppendLine(" )");
            }
            else
            {
                SQL.AppendLine(" WHERE EXISTS");
                SQL.AppendLine(" (");
                SQL.AppendLine("   SELECT 1 FROM KOSMOS_EMR.AEMRUSERITEMVS U ");
                SQL.AppendLine("    WHERE U.JOBGB IN('SP') ");
                SQL.AppendLine("      AND U.USEGB IN('" + clsType.User.IdNumber + "', '" + clsType.User.BuseCode + "')");
                SQL.AppendLine("      AND C.ITEMCD = U.ITEMCD");
                SQL.AppendLine(" )");
            }
            #endregion

            #region 3개 합침
            SQL.AppendLine("UNION ALL");
            SQL.AppendLine("SELECT ");
            SQL.AppendLine("      3 AS GBN ");
            //SQL.AppendLine("          , C.FORMNO");
            SQL.AppendLine("          , C.CHARTDATE");
            SQL.AppendLine("          , C.CHARTTIME");
            SQL.AppendLine("          , C.CHARTUSEID");
            SQL.AppendLine("          , C.EMRNO");
            SQL.AppendLine("          , C.ITEMCD");
            SQL.AppendLine("          , C.ITEMNO");
            SQL.AppendLine("          , C.ITEMVALUE");
            SQL.AppendLine("          , C.ITEMVALUE1 ");
            SQL.AppendLine("          , B.BASEXNAME");
            SQL.AppendLine("          , B.BASNAME ");
            SQL.AppendLine("FROM ROW_LIST C ");
            SQL.AppendLine("  INNER JOIN KOSMOS_EMR.AEMRBASCD B                                      ");
            SQL.AppendLine("     ON B.BSNSCLS = '기록지관리'                                            ");
            SQL.AppendLine("    AND B.UNITCLS > CHR(0)                                               ");
            SQL.AppendLine("    AND B.USECLS = '0'                                                   ");
            SQL.AppendLine("    AND B.APLFRDATE > CHR(0)                                             ");
            SQL.AppendLine("    AND B.BASEXNAME IN('산소요법', '인공호흡기', '흡인간호(Suction)') ");
            SQL.AppendLine("    AND C.ITEMCD = B.BASCD");
            SQL.AppendLine("    AND C.FORMNO IN (3150)");
            SQL.AppendLine("  INNER JOIN KOSMOS_EMR.AEMRBASCD BB                                     ");
            SQL.AppendLine("     ON B.VFLAG1 = BB.BASCD                                              ");
            SQL.AppendLine("    AND BB.BSNSCLS = '기록지관리'                                           ");
            SQL.AppendLine("    AND BB.UNITCLS > CHR(0)                                              ");
            SQL.AppendLine("    AND BB.BASCD > CHR(0)                                                ");
            SQL.AppendLine("    AND BB.APLFRDATE > CHR(0)                                            ");
            SQL.AppendLine("    AND BB.BASNAME IN('산소요법', '인공호흡기', '흡인간호(Suction)')  ");
            SQL.AppendLine("    AND BB.USECLS = '0'                                                  ");
            if (Filter3 != null && Filter3.Count > 0)
            {
                SQL.AppendLine(" WHERE ITEMCD IN");
                SQL.AppendLine(" (");
                SQL.AppendLine(string.Join(",", Filter3));
                SQL.AppendLine(" )");
            }
            else
            {
                SQL.AppendLine(" WHERE EXISTS");
                SQL.AppendLine(" (");
                SQL.AppendLine("   SELECT 1 FROM KOSMOS_EMR.AEMRUSERITEMVS U ");
                SQL.AppendLine("    WHERE U.JOBGB IN('SP2') ");
                SQL.AppendLine("      AND U.USEGB IN('" + clsType.User.IdNumber + "', '" + clsType.User.BuseCode + "')");
                SQL.AppendLine("      AND C.ITEMCD = U.ITEMCD");
                SQL.AppendLine(" )");
            }
            #endregion

            #region 상처/욕창등등
            SQL.AppendLine("UNION ALL");
            SQL.AppendLine("SELECT ");
            SQL.AppendLine("      4 AS GBN ");
            //SQL.AppendLine("          , C.FORMNO");
            SQL.AppendLine("          , C.CHARTDATE");
            SQL.AppendLine("          , C.CHARTTIME");
            SQL.AppendLine("          , C.CHARTUSEID");
            SQL.AppendLine("          , C.EMRNO");
            SQL.AppendLine("          , C.ITEMCD");
            SQL.AppendLine("          , C.ITEMNO");
            SQL.AppendLine("          , C.ITEMVALUE");
            SQL.AppendLine("          , C.ITEMVALUE1 ");
            SQL.AppendLine("          , (SELECT FORMNAME FROM KOSMOS_EMR.AEMRFORM WHERE FORMNO = C.FORMNO AND USECHECK = '1' AND ROWNUM = 1) AS ASD");
            SQL.AppendLine("          , ''");
            SQL.AppendLine("  FROM ROW_LIST C ");
            SQL.AppendLine(" WHERE C.FORMNO IN (1725, 2638, 1573) ");
            if (Filter4 != null && Filter4.Count > 0)
            {
                SQL.AppendLine("   AND ITEMCD IN");
                SQL.AppendLine(" (");
                SQL.AppendLine(string.Join(",", Filter4));
                SQL.AppendLine(" )");
            }
            else
            {
                SQL.AppendLine("   AND EXISTS");
                SQL.AppendLine("   (");
                SQL.AppendLine("     SELECT 1 FROM KOSMOS_EMR.AEMRUSERITEMVS U ");
                SQL.AppendLine("      WHERE U.JOBGB IN('WD', 'BD', 'CVC') ");
                SQL.AppendLine("        AND U.USEGB IN('" + clsType.User.IdNumber + "', '" + clsType.User.BuseCode + "')");
                SQL.AppendLine("        AND C.ITEMCD = U.ITEMCD");
                SQL.AppendLine("   )");
            }
            #endregion

            if (chkAsc.Checked == true)
            {
                SQL.AppendLine("ORDER BY GBN, CHARTDATE ASC, CHARTTIME ASC, EMRNO ASC");
            }
            else
            {
                SQL.AppendLine("ORDER BY GBN, CHARTDATE DESC, CHARTTIME DESC, EMRNO DESC");
            }

            SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (reader.HasRows == false)
            {
                reader.Dispose();
                Cursor.Current = Cursors.Default;
                return;
            }

            string strEMRNO = "";
            string strCHARTDATE = "";
            int intDay = 0;
            Color DayColor = ComNum.SPDESELCOLOR;

            SheetView sheetView = null;
            List<string> lstChartDT = new List<string>();

            while (reader.Read())
            {
                string GBN = reader.GetValue(0).ToString().Trim();
                string CHARTDATE = reader.GetValue(1).ToString().Trim();
                string CHARTTIME = reader.GetValue(2).ToString().Trim();
                string CHARTUSEID = reader.GetValue(3).ToString().Trim();
                string EMRNO = reader.GetValue(4).ToString().Trim();
                string ITEMCD = reader.GetValue(5).ToString().Trim();
                string ITEMNO = reader.GetValue(6).ToString().Trim();
                string ITEMVALUE = reader.GetValue(7).ToString().Trim();
                string ITEMVALUE1 = reader.GetValue(8).ToString().Trim();
                string EXENAME = reader.GetValue(9).ToString().Trim();

                if (GBN.Equals("1"))
                {
                    sheetView = ssVital_Sheet1;
                }
                else if (GBN.Equals("2"))
                {
                    if (sheetView.Equals(ssAct_Sheet1) == false)
                        lstChartDT.Clear();

                    sheetView = ssAct_Sheet1;
                }
                else if (GBN.Equals("3"))
                {
                    if (sheetView.Equals(ssSpo2RR_Sheet1) == false)
                        lstChartDT.Clear();

                    sheetView = ssSpo2RR_Sheet1;
                }
                else if (GBN.Equals("4"))
                {
                    if (sheetView.Equals(ssRecord3_Sheet1) == false)
                        lstChartDT.Clear();

                    sheetView = ssRecord3_Sheet1;
                }

                if (strCHARTDATE != CHARTDATE)
                {
                    if (intDay == 0)
                    {
                        intDay = 1;
                        DayColor = ComNum.SPDESELCOLOR;
                    }
                    else
                    {
                        intDay = 0;
                        DayColor = ComNum.SPSELCOLOR;
                    }
                }
                strCHARTDATE = CHARTDATE;

                if (strEMRNO != EMRNO)
                {
                    if (lstChartDT.IndexOf(CHARTDATE + CHARTTIME + EMRNO) == -1)
                    {
                        sheetView.Columns.Count = sheetView.Columns.Count + 1;
                        sheetView.ColumnHeader.Cells[0, sheetView.Columns.Count - 1].Text = VB.Mid(CHARTDATE, 5, 2) + "-" + VB.Right(CHARTDATE, 2);
                        sheetView.ColumnHeader.Cells[1, sheetView.Columns.Count - 1].Text = ComFunc.FormatStrToDate(CHARTTIME, "M");
                        sheetView.Columns[sheetView.Columns.Count - 1].CellType = TypeText;
                        sheetView.Columns[sheetView.Columns.Count - 1].Width = 80;
                        sheetView.Columns[sheetView.Columns.Count - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                        sheetView.Columns[sheetView.Columns.Count - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        sheetView.Columns[sheetView.Columns.Count - 1].BackColor = DayColor;
                        sheetView.Columns[sheetView.Columns.Count - 1].Locked = true;



                        lstChartDT.Add(CHARTDATE + CHARTTIME + EMRNO);


                    }

                }

                strEMRNO = EMRNO;

                int intRow = FinedRow(sheetView, EXENAME + ITEMCD);

                if (intRow == -1)
                    continue;

                if (!string.IsNullOrWhiteSpace(ITEMVALUE))
                {
                    sheetView.Cells[intRow, sheetView.Columns.Count - 1].Text = ITEMVALUE + " " + ITEMVALUE1;
                }
                else
                {
                    sheetView.Cells[intRow, sheetView.Columns.Count - 1].Text = ITEMVALUE;
                }

                sheetView.Cells[intRow, sheetView.Columns.Count - 1].Tag = READ_Job_UserName(CHARTUSEID) + " / " + VB.Mid(CHARTDATE, 5, 2) + "-" + VB.Right(CHARTDATE, 2) + " / " + ComFunc.FormatStrToDate(CHARTTIME, "M");

                sheetView.Cells[intRow, sheetView.Columns.Count - 1].ForeColor = TextForeColor(sheetView.Cells[intRow, sheetView.Columns.Count - 1].Text, strCHARTDATE, ITEMCD);



                if (ITEMNO.Equals("I0000001811"))
                {
                    if (VB.Val(sheetView.Cells[intRow, sheetView.Columns.Count - 1].Text) != 0)
                    {
                        if (VB.Val(sheetView.Cells[intRow, sheetView.Columns.Count - 1].Text) <= 35)
                        {
                            sheetView.Cells[intRow, sheetView.Columns.Count - 1].ForeColor = Color.Red;
                        }
                        else if (VB.Val(sheetView.Cells[intRow, sheetView.Columns.Count - 1].Text) >= 37.5)
                        {
                            sheetView.Cells[intRow, sheetView.Columns.Count - 1].ForeColor = Color.Red;
                        }
                    }
                }
            }
            //여기여기SetAllRowHeight
            SetAllRowHeight("");




            reader.Dispose();
            lstChartDT.Clear();


            foreach (string Rows in KeyRow)
            {
                string GBN = Rows.Split('|')[0];
                int Row = int.Parse(Rows.Split('|')[1]);
                if (GBN.Equals("1"))
                {
                    sheetView = ssVital_Sheet1;
                }
                else if (GBN.Equals("2"))
                {
                    sheetView = ssAct_Sheet1;
                }
                else if (GBN.Equals("3"))
                {
                    sheetView = ssSpo2RR_Sheet1;
                }
                else if (GBN.Equals("4"))
                {
                    sheetView = ssRecord3_Sheet1;
                }

                sheetView.Cells[Row, 2, Row, sheetView.ColumnCount - 1].Border = complexBorder;
            }

            if (sheetView.RowCount > 0)
                sheetView.Cells[sheetView.RowCount - 1, 2, sheetView.RowCount - 1, sheetView.ColumnCount - 1].Border = complexBorder;

            Cursor.Current = Cursors.Default;
        }


        private void SetAllRowHeight(string arg)
        {
            for (int i = 0; ssVital_Sheet1.Rows.Count > i; i++)
            {
                if (ssVital_Sheet1.GetPreferredRowHeight(i) > ComNum.SPDROWHT)
                {
                    ssVital_Sheet1.SetRowHeight(i, Convert.ToInt32((ssVital_Sheet1.GetPreferredRowHeight(i) + 5)));
                }
            }

            for (int i = 0; ssAct_Sheet1.Rows.Count > i; i++)
            {
                if (ssAct_Sheet1.GetPreferredRowHeight(i) > ComNum.SPDROWHT)
                {
                    ssAct_Sheet1.SetRowHeight(i, Convert.ToInt32((ssAct_Sheet1.GetPreferredRowHeight(i) + 5)));
                }
            }

            for (int i = 0; ssSpo2RR_Sheet1.Rows.Count > i; i++)
            {
                if (ssSpo2RR_Sheet1.GetPreferredRowHeight(i) > ComNum.SPDROWHT)
                {
                    ssSpo2RR_Sheet1.SetRowHeight(i, Convert.ToInt32((ssSpo2RR_Sheet1.GetPreferredRowHeight(i) + 5)));
                }
            }

            for (int i = 0; ssRecord3_Sheet1.Rows.Count > i; i++)
            {
                if (ssRecord3_Sheet1.GetPreferredRowHeight(i) > ComNum.SPDROWHT)
                {
                    ssRecord3_Sheet1.SetRowHeight(i, Convert.ToInt32((ssRecord3_Sheet1.GetPreferredRowHeight(i) + 5)));
                }
            }

        }

        /// <summary>
        /// 임상관찰 값을 가져온다
        /// </summary>
        private void GetVitalValue()
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.MaxLength = 1000;
            TypeText.Multiline = true;
            TypeText.WordWrap = true;

            Cursor.Current = Cursors.WaitCursor;
            ssVital_Sheet1.Columns.Count = 3;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.EMRNO, ";
            SQL = SQL + ComNum.VBLF + "    R.ITEMCD, R.ITEMNO, R.ITEMVALUE, R.ITEMVALUE1, ";
            SQL = SQL + ComNum.VBLF + "    B.BASEXNAME, B.BASNAME ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_EMR.AEMRCHARTROW R ";
            SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = R.EMRNO ";
            SQL = SQL + ComNum.VBLF + "    AND C.EMRNOHIS  = R.EMRNOHIS ";
            SQL = SQL + ComNum.VBLF + "    AND R.ITEMCD IN (SELECT U.ITEMCD FROM KOSMOS_EMR.AEMRUSERITEMVS U ";
            SQL = SQL + ComNum.VBLF + "                     WHERE U.JOBGB = 'IVT' ";
            SQL = SQL + ComNum.VBLF + "                            AND U.USEGB = '" + clsType.User.IdNumber + "' ";
            SQL = SQL + ComNum.VBLF + "                     UNION ";
            SQL = SQL + ComNum.VBLF + "                     SELECT U.ITEMCD FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U";
            SQL = SQL + ComNum.VBLF + "                     WHERE U.JOBGB = 'IVT'";
            SQL = SQL + ComNum.VBLF + "                           AND U.USEGB = '" + clsType.User.BuseCode + "'";
            SQL = SQL + ComNum.VBLF + "                     )";
            SQL = SQL + ComNum.VBLF + "    AND (R.ITEMVALUE IS NOT NULL ) "; // OR R.ITEMVALUE1 IS NOT NULL) ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_EMR.AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    ON R.ITEMCD = B.BASCD ";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호') ";
            SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "    AND C.FORMNO IN (3150, 2135, 1935, 1969, 2431, 2431) -- 임상관찰,회복실, Angio, 응급실, 진정, 내시경 ";
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTUSEID <> '합계' ";
            if (chkAsc.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) ASC";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) DESC";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strEMRNO = "";
            string strCHARTDATE = "";
            int intDay = 0;
            Color DayColor = ComNum.SPDESELCOLOR;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strCHARTDATE != dt.Rows[i]["CHARTDATE"].ToString().Trim())
                {
                    if (intDay == 0)
                    {
                        intDay = 1;
                        DayColor = ComNum.SPDESELCOLOR;
                    }
                    else
                    {
                        intDay = 0;
                        DayColor = ComNum.SPSELCOLOR;
                    }
                }
                strCHARTDATE = dt.Rows[i]["CHARTDATE"].ToString().Trim();

                if (strEMRNO != dt.Rows[i]["EMRNO"].ToString().Trim())
                {
                    ssVital_Sheet1.Columns.Count = ssVital_Sheet1.Columns.Count + 1;
                    ssVital_Sheet1.ColumnHeader.Cells[0, ssVital_Sheet1.Columns.Count - 1].Text = VB.Mid(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 5, 2) + "-" + VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 2);
                    ssVital_Sheet1.ColumnHeader.Cells[1, ssVital_Sheet1.Columns.Count - 1].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");
                    ssVital_Sheet1.Columns[ssVital_Sheet1.Columns.Count - 1].CellType = TypeText;
                    ssVital_Sheet1.Columns[ssVital_Sheet1.Columns.Count - 1].Width = 80;
                    ssVital_Sheet1.Columns[ssVital_Sheet1.Columns.Count - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    ssVital_Sheet1.Columns[ssVital_Sheet1.Columns.Count - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    ssVital_Sheet1.Columns[ssVital_Sheet1.Columns.Count - 1].BackColor = DayColor;
                    ssVital_Sheet1.Columns[ssVital_Sheet1.Columns.Count - 1].Locked = true;
                }

                strEMRNO = dt.Rows[i]["EMRNO"].ToString().Trim();

                int intRow = FinedRow(ssVital_Sheet1, dt.Rows[i]["ITEMCD"].ToString().Trim());

                if (dt.Rows[i]["ITEMVALUE1"].ToString().Trim() != "")
                {
                    ssVital_Sheet1.Cells[intRow, ssVital_Sheet1.Columns.Count - 1].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim() + " " + dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                }
                else
                {
                    ssVital_Sheet1.Cells[intRow, ssVital_Sheet1.Columns.Count - 1].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                }

                ssVital_Sheet1.Cells[intRow, ssVital_Sheet1.Columns.Count - 1].ForeColor = TextForeColor(ssVital_Sheet1.Cells[intRow, ssVital_Sheet1.Columns.Count - 1].Text);


                if (dt.Rows[i]["ITEMNO"].ToString().Trim() == "I0000001811")
                {
                    if (VB.Val(ssVital_Sheet1.Cells[intRow, ssVital_Sheet1.Columns.Count - 1].Text) != 0)
                    {
                        if (VB.Val(ssVital_Sheet1.Cells[intRow, ssVital_Sheet1.Columns.Count - 1].Text) <= 35)
                        {
                            ssVital_Sheet1.Cells[intRow, ssVital_Sheet1.Columns.Count - 1].ForeColor = Color.Red;
                        }
                        else if (VB.Val(ssVital_Sheet1.Cells[intRow, ssVital_Sheet1.Columns.Count - 1].Text) >= 37.5)
                        {
                            ssVital_Sheet1.Cells[intRow, ssVital_Sheet1.Columns.Count - 1].ForeColor = Color.Red;
                        }
                    }
                }
            }

            dt.Dispose();
            dt = null;

            foreach (int Row in lstRow)
            {
                ssVital_Sheet1.Cells[Row, 2, Row, ssVital_Sheet1.ColumnCount - 1].Border = complexBorder;
            }

            ssVital_Sheet1.Cells[ssVital_Sheet1.RowCount - 1, 2, ssVital_Sheet1.RowCount - 1, ssVital_Sheet1.ColumnCount - 1].Border = complexBorder;

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 아이템이 있는 Row를 찾는다
        /// </summary>
        /// <param name="Spd"></param>
        /// <param name="strITEMCD"></param>
        /// <returns></returns>
        private int FinedRow(FarPoint.Win.Spread.SheetView Spd, string strITEMCD)
        {
            int rtnVal = -1;
            //int i = 0;

            //for (i = 0; i < Spd.RowCount; i++)
            //{
            //    if (Spd.Cells[i, 0].Text.Trim() == strITEMCD)
            //    {
            //        rtnVal = i;
            //        break;
            //    }
            //}

            if (Spd.Equals(ssVital_Sheet1) && ssVital_Sheet1.RowCount > 0)
            {
                keyVSRow.TryGetValue(strITEMCD, out rtnVal);
            }
            else if (Spd.Equals(ssAct_Sheet1) && ssAct_Sheet1.RowCount > 0)
            {
                keyActRow.TryGetValue(strITEMCD, out rtnVal);
            }
            else if (Spd.Equals(ssSpo2RR_Sheet1) && ssSpo2RR_Sheet1.RowCount > 0)
            {
                keySpo2.TryGetValue(strITEMCD, out rtnVal);
            }
            else if (Spd.Equals(ssRecord3_Sheet1) && ssRecord3_Sheet1.RowCount > 0)
            {
                keyRecord3.TryGetValue(strITEMCD, out rtnVal);
            }

            return rtnVal;
        }

        private Color TextForeColor(string strText, string strCHARTDATE = "", string ITEMCD = "")
        {
            if (strText.IndexOf("삽입") != -1 ||
                strText.IndexOf("시작") != -1 ||
                strText.IndexOf("종료") != -1 ||
                strText.IndexOf("마침") != -1 ||
                strText.IndexOf("교체") != -1 ||
                strText.IndexOf("교환") != -1 ||
                strText.IndexOf("변경") != -1 ||
                strText.ToUpper().IndexOf("START") != -1 ||
                strText.ToUpper().IndexOf("REMOVE") != -1 ||
                strText.IndexOf("제거") != -1 ||
                (ITEMCD.Equals("I0000037607") || ITEMCD.Equals("I0000038108")) && strText.Replace("-", "").Substring(0, 8).Equals(strCHARTDATE))
            {
                return Color.Blue;
            }

            return Color.Black;
        }

        /// <summary>
        /// 임상관찰 - 산소요법, 인공호흡기, 흡인간호 / 간호활동 - 호흡간호/인공기도 항목 가져오기.
        /// </summary>
        private void FormSearchSpo2RR()
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssSpo2RR_Sheet1.RowCount = 0;
            ssSpo2RR_Sheet1.Columns.Count = 3;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     B.BASVAL, B.BASCD, B.BASNAME, B.BASEXNAME ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "      ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "     AND BB.BSNSCLS = '기록지관리'  ";
            SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS > CHR(0)";
            SQL = SQL + ComNum.VBLF + "     AND BB.BASCD > CHR(0)";
            SQL = SQL + ComNum.VBLF + "     AND BB.APLFRDATE > CHR(0) ";
            SQL = SQL + ComNum.VBLF + "     AND BB.BASNAME IN('산소요법', '인공호흡기' , '흡인간호', '호흡간호', '인공기도') ";
            SQL = SQL + ComNum.VBLF + "     AND BB.USECLS = '0' ";
            SQL = SQL + ComNum.VBLF + "WHERE B.BSNSCLS = '기록지관리'  ";
            SQL = SQL + ComNum.VBLF + "  AND B.UNITCLS > CHR(0)";
            SQL = SQL + ComNum.VBLF + "  AND B.USECLS = '0' ";
            SQL = SQL + ComNum.VBLF + "  AND B.APLFRDATE > CHR(0) ";
            SQL = SQL + ComNum.VBLF + "  AND B.BASEXNAME IN('산소요법', '인공호흡기' , '흡인간호', '호흡간호', '인공기도') ";
            SQL = SQL + ComNum.VBLF + "  AND EXISTS";
            SQL = SQL + ComNum.VBLF + "  (";
            SQL = SQL + ComNum.VBLF + "      SELECT 1";
            SQL = SQL + ComNum.VBLF + "        FROM KOSMOS_EMR.AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "          INNER JOIN KOSMOS_EMR.AEMRCHARTROW R ";
            SQL = SQL + ComNum.VBLF + "             ON C.EMRNO = R.EMRNO ";
            SQL = SQL + ComNum.VBLF + "            AND C.EMRNOHIS = R.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "            AND R.ITEMVALUE IS NOT NULL"; // OR R.ITEMVALUE1 IS NOT NULL) ";SQL = SQL + ComNum.VBLF + "       WHERE C.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "       WHERE C.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "         AND C.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "         AND C.FORMNO IN (3150, 1575) ";
            SQL = SQL + ComNum.VBLF + "         AND C.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "         AND C.CHARTUSEID <> '합계' ";
            SQL = SQL + ComNum.VBLF + "         AND R.ITEMCD = B.BASCD ";
            SQL = SQL + ComNum.VBLF + "  )";
            SQL = SQL + ComNum.VBLF + "ORDER BY BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strBASEXNAME = "";
            int intS = 0;

            keySpo2.Clear();
            lstRow.Clear();

            ssSpo2RR_Sheet1.RowCount = dt.Rows.Count;
            ssSpo2RR_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (keySpo2.ContainsKey(dt.Rows[i]["BASCD"].ToString().Trim()) == false)
                {
                    keySpo2.Add(dt.Rows[i]["BASCD"].ToString().Trim(), i);
                }
                ssSpo2RR_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssSpo2RR_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                ssSpo2RR_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();

                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    ssSpo2RR_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        ssSpo2RR_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
                        lstRow.Add(i - 1);
                    }
                    intS = i;

                    ssSpo2RR_Sheet1.Cells[i, 1].Font = boldFont;
                    ssSpo2RR_Sheet1.Cells[i, 1].Border = complexBorder;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            ssSpo2RR_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
            ssSpo2RR_Sheet1.SetRowHeight(-1, 40);
            dt.Dispose();
            dt = null;

            GetSpo2Value();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 상처/욕창/중심정맥관 - 합치기
        /// </summary>
        private void FormSearchRecord3()
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssSpo2RR_Sheet1.RowCount = 0;
            ssSpo2RR_Sheet1.Columns.Count = 3;

            SQL = "";
            SQL = "SELECT B.ITEMNO AS BASCD";
            SQL = SQL + ComNum.VBLF + "  , A.FORMNAME AS BASEXNAME";
            SQL = SQL + ComNum.VBLF + "  , B.ITEMNAME AS BASNAME";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRFORM A";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRFLOWXML B";
            SQL = SQL + ComNum.VBLF + "       ON A.FORMNO = B.FORMNO";
            SQL = SQL + ComNum.VBLF + "      AND A.UPDATENO = B.UPDATENO";
            SQL = SQL + ComNum.VBLF + "      AND B.ITEMNUMBER >= 0";
            SQL = SQL + ComNum.VBLF + "      AND EXISTS";
            SQL = SQL + ComNum.VBLF + "      (";
            SQL = SQL + ComNum.VBLF + "          SELECT 1";
            SQL = SQL + ComNum.VBLF + "            FROM KOSMOS_EMR.AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "              INNER JOIN KOSMOS_EMR.AEMRCHARTROW R ";
            SQL = SQL + ComNum.VBLF + "                 ON C.EMRNO = R.EMRNO ";
            SQL = SQL + ComNum.VBLF + "                AND C.EMRNOHIS = R.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "                AND R.ITEMNO IN ";
            SQL = SQL + ComNum.VBLF + "                ('I0000024733', 'I0000034121', 'I0000030799', 'I0000037566', 'I0000031440', 'I0000001311'";
            SQL = SQL + ComNum.VBLF + "                ,'I0000037604', 'I0000037611', 'I0000021853', 'I0000037607'";
            SQL = SQL + ComNum.VBLF + "                ,'I0000034115', 'I0000034116', 'I0000004518')";
            SQL = SQL + ComNum.VBLF + "                AND R.ITEMVALUE IS NOT NULL";
            SQL = SQL + ComNum.VBLF + "           WHERE C.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "             AND C.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "             AND C.FORMNO = A.FORMNO ";
            SQL = SQL + ComNum.VBLF + "             AND C.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "             AND C.CHARTUSEID <> '합계' ";
            SQL = SQL + ComNum.VBLF + "             AND R.ITEMCD = B.ITEMNO ";
            SQL = SQL + ComNum.VBLF + "      )";
            SQL = SQL + ComNum.VBLF + " WHERE A.FORMNO IN (1725, 2638, 1573)";
            SQL = SQL + ComNum.VBLF + "   AND A.UPDATENO = 2";
            SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(A.FORMNO, 1725, 0, 2638, 1, 1573, 2), B.ITEMNUMBER";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strBASEXNAME = "";
            int intS = 0;

            keyRecord3.Clear();
            lstRow.Clear();

            ssRecord3_Sheet1.RowCount = dt.Rows.Count;
            ssRecord3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (keyRecord3.ContainsKey(dt.Rows[i]["BASCD"].ToString().Trim()) == false)
                {
                    keyRecord3.Add(dt.Rows[i]["BASCD"].ToString().Trim(), i);
                }
                ssRecord3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssRecord3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                ssRecord3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();

                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    ssRecord3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        ssRecord3_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
                        lstRow.Add(i - 1);
                    }
                    intS = i;

                    ssRecord3_Sheet1.Cells[i, 1].Font = boldFont;
                    ssRecord3_Sheet1.Cells[i, 1].Border = complexBorder;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            ssRecord3_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
            ssRecord3_Sheet1.SetRowHeight(-1, 40);
            dt.Dispose();
            dt = null;

            GetRecord3Value();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 상처/욕창/중심정맥관 - 값을 가져온다
        /// </summary>
        private void GetRecord3Value()
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.MaxLength = 1000;
            TypeText.Multiline = true;
            TypeText.WordWrap = true;

            Cursor.Current = Cursors.WaitCursor;
            ssRecord3_Sheet1.Columns.Count = 3;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.EMRNO, ";
            SQL = SQL + ComNum.VBLF + "    R.ITEMCD, R.ITEMVALUE, R.ITEMVALUE1";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.AEMRCHARTROW R                                   ";
            SQL = SQL + ComNum.VBLF + "     ON C.EMRNO = R.EMRNO                                                ";
            SQL = SQL + ComNum.VBLF + "    AND C.EMRNOHIS  = R.EMRNOHIS                                         ";
            SQL = SQL + ComNum.VBLF + "    AND R.ITEMNO IN                                               ";
            SQL = SQL + ComNum.VBLF + "             ('I0000024733', 'I0000034121', 'I0000030799', 'I0000037566', 'I0000031440', 'I0000001311'";
            SQL = SQL + ComNum.VBLF + "             ,'I0000037604', 'I0000037611', 'I0000021853', 'I0000037607'";
            SQL = SQL + ComNum.VBLF + "             ,'I0000034115', 'I0000034116', 'I0000004518')";
            SQL = SQL + ComNum.VBLF + "    AND R.ITEMVALUE IS NOT NULL                                           ";
            SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "  AND C.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "  AND C.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "  AND C.FORMNO IN (1725, 2638, 1573) ";
            SQL = SQL + ComNum.VBLF + "  AND C.CHARTUSEID <> '합계' ";
            if (chkAsc.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) ASC";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) DESC";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strEMRNO = "";
            string strCHARTDATE = "";
            int intDay = 0;
            Color DayColor = ComNum.SPDESELCOLOR;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strCHARTDATE != dt.Rows[i]["CHARTDATE"].ToString().Trim())
                {
                    if (intDay == 0)
                    {
                        intDay = 1;
                        DayColor = ComNum.SPDESELCOLOR;
                    }
                    else
                    {
                        intDay = 0;
                        DayColor = ComNum.SPSELCOLOR;
                    }
                }
                strCHARTDATE = dt.Rows[i]["CHARTDATE"].ToString().Trim();

                if (strEMRNO != dt.Rows[i]["EMRNO"].ToString().Trim())
                {
                    ssRecord3_Sheet1.Columns.Count = ssRecord3_Sheet1.Columns.Count + 1;
                    ssRecord3_Sheet1.ColumnHeader.Cells[0, ssRecord3_Sheet1.Columns.Count - 1].Text = VB.Mid(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 5, 2) + "-" + VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 2);
                    ssRecord3_Sheet1.ColumnHeader.Cells[1, ssRecord3_Sheet1.Columns.Count - 1].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");
                    ssRecord3_Sheet1.Columns[ssRecord3_Sheet1.Columns.Count - 1].CellType = TypeText;
                    ssRecord3_Sheet1.Columns[ssRecord3_Sheet1.Columns.Count - 1].Width = 80;
                    ssRecord3_Sheet1.Columns[ssRecord3_Sheet1.Columns.Count - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    ssRecord3_Sheet1.Columns[ssRecord3_Sheet1.Columns.Count - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    ssRecord3_Sheet1.Columns[ssRecord3_Sheet1.Columns.Count - 1].BackColor = DayColor;
                    ssRecord3_Sheet1.Columns[ssRecord3_Sheet1.Columns.Count - 1].Locked = true;
                }

                strEMRNO = dt.Rows[i]["EMRNO"].ToString().Trim();

                int intRow = FinedRow(ssRecord3_Sheet1, dt.Rows[i]["ITEMCD"].ToString().Trim());

                if (dt.Rows[i]["ITEMVALUE1"].ToString().Trim() != "")
                {
                    ssRecord3_Sheet1.Cells[intRow, ssRecord3_Sheet1.Columns.Count - 1].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim() + " " + dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                }
                else
                {
                    ssRecord3_Sheet1.Cells[intRow, ssRecord3_Sheet1.Columns.Count - 1].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                }

                ssRecord3_Sheet1.Cells[intRow, ssRecord3_Sheet1.Columns.Count - 1].ForeColor = TextForeColor(ssRecord3_Sheet1.Cells[intRow, ssRecord3_Sheet1.Columns.Count - 1].Text);
            }

            dt.Dispose();
            dt = null;

            foreach (int Row in lstRow)
            {
                ssRecord3_Sheet1.Cells[Row, 2, Row, ssRecord3_Sheet1.ColumnCount - 1].Border = complexBorder;
            }
            ssRecord3_Sheet1.Cells[ssRecord3_Sheet1.RowCount - 1, 2, ssRecord3_Sheet1.RowCount - 1, ssRecord3_Sheet1.ColumnCount - 1].Border = complexBorder;

            Cursor.Current = Cursors.Default;
        }


        /// <summary>
        /// 임상관찰 값을 가져온다
        /// </summary>
        private void GetSpo2Value()
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.MaxLength = 1000;
            TypeText.Multiline = true;
            TypeText.WordWrap = true;

            Cursor.Current = Cursors.WaitCursor;
            ssSpo2RR_Sheet1.Columns.Count = 3;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.EMRNO, ";
            SQL = SQL + ComNum.VBLF + "    R.ITEMCD, R.ITEMVALUE, R.ITEMVALUE1, ";
            SQL = SQL + ComNum.VBLF + "    B.BASEXNAME, B.BASNAME ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.AEMRBASCD B                                      ";
            SQL = SQL + ComNum.VBLF + "     ON B.BSNSCLS = '기록지관리'                                            ";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS > CHR(0)                                               ";
            SQL = SQL + ComNum.VBLF + "    AND B.USECLS = '0'                                                   ";
            SQL = SQL + ComNum.VBLF + "    AND B.APLFRDATE > CHR(0)                                             ";
            SQL = SQL + ComNum.VBLF + "    AND B.BASEXNAME IN('산소요법', '인공호흡기' , '흡인간호', '호흡간호', '인공기도') ";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.AEMRBASCD BB                                     ";
            SQL = SQL + ComNum.VBLF + "     ON B.VFLAG1 = BB.BASCD                                              ";
            SQL = SQL + ComNum.VBLF + "    AND BB.BSNSCLS = '기록지관리'                                           ";
            SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS > CHR(0)                                              ";
            SQL = SQL + ComNum.VBLF + "    AND BB.BASCD > CHR(0)                                                ";
            SQL = SQL + ComNum.VBLF + "    AND BB.APLFRDATE > CHR(0)                                            ";
            SQL = SQL + ComNum.VBLF + "    AND BB.BASNAME IN('산소요법', '인공호흡기' , '흡인간호', '호흡간호', '인공기도')  ";
            SQL = SQL + ComNum.VBLF + "    AND BB.USECLS = '0'                                                  ";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.AEMRCHARTROW R                                   ";
            SQL = SQL + ComNum.VBLF + "     ON C.EMRNO = R.EMRNO                                                ";
            SQL = SQL + ComNum.VBLF + "    AND C.EMRNOHIS  = R.EMRNOHIS                                         ";
            SQL = SQL + ComNum.VBLF + "    AND R.ITEMCD = B.BASCD                                               ";
            SQL = SQL + ComNum.VBLF + "    AND R.ITEMVALUE IS NOT NULL                                          ";
            SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "  AND C.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "  AND C.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "  AND C.FORMNO IN (3150, 1575) ";
            SQL = SQL + ComNum.VBLF + "  AND C.CHARTUSEID <> '합계' ";
            if (chkAsc.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) ASC";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) DESC";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strEMRNO = "";
            string strCHARTDATE = "";
            int intDay = 0;
            Color DayColor = ComNum.SPDESELCOLOR;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strCHARTDATE != dt.Rows[i]["CHARTDATE"].ToString().Trim())
                {
                    if (intDay == 0)
                    {
                        intDay = 1;
                        DayColor = ComNum.SPDESELCOLOR;
                    }
                    else
                    {
                        intDay = 0;
                        DayColor = ComNum.SPSELCOLOR;
                    }
                }
                strCHARTDATE = dt.Rows[i]["CHARTDATE"].ToString().Trim();

                if (strEMRNO != dt.Rows[i]["EMRNO"].ToString().Trim())
                {
                    ssSpo2RR_Sheet1.Columns.Count = ssSpo2RR_Sheet1.Columns.Count + 1;
                    ssSpo2RR_Sheet1.ColumnHeader.Cells[0, ssSpo2RR_Sheet1.Columns.Count - 1].Text = VB.Mid(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 5, 2) + "-" + VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 2);
                    ssSpo2RR_Sheet1.ColumnHeader.Cells[1, ssSpo2RR_Sheet1.Columns.Count - 1].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");
                    ssSpo2RR_Sheet1.Columns[ssSpo2RR_Sheet1.Columns.Count - 1].CellType = TypeText;
                    ssSpo2RR_Sheet1.Columns[ssSpo2RR_Sheet1.Columns.Count - 1].Width = 80;
                    ssSpo2RR_Sheet1.Columns[ssSpo2RR_Sheet1.Columns.Count - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    ssSpo2RR_Sheet1.Columns[ssSpo2RR_Sheet1.Columns.Count - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    ssSpo2RR_Sheet1.Columns[ssSpo2RR_Sheet1.Columns.Count - 1].BackColor = DayColor;
                    ssSpo2RR_Sheet1.Columns[ssSpo2RR_Sheet1.Columns.Count - 1].Locked = true;
                }

                strEMRNO = dt.Rows[i]["EMRNO"].ToString().Trim();

                int intRow = FinedRow(ssSpo2RR_Sheet1, dt.Rows[i]["ITEMCD"].ToString().Trim());

                if (dt.Rows[i]["ITEMVALUE1"].ToString().Trim() != "")
                {
                    ssSpo2RR_Sheet1.Cells[intRow, ssSpo2RR_Sheet1.Columns.Count - 1].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim() + " " + dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                }
                else
                {
                    ssSpo2RR_Sheet1.Cells[intRow, ssSpo2RR_Sheet1.Columns.Count - 1].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                }

                ssSpo2RR_Sheet1.Cells[intRow, ssSpo2RR_Sheet1.Columns.Count - 1].ForeColor = TextForeColor(ssSpo2RR_Sheet1.Cells[intRow, ssSpo2RR_Sheet1.Columns.Count - 1].Text);
            }

            dt.Dispose();
            dt = null;

            foreach (int Row in lstRow)
            {
                ssSpo2RR_Sheet1.Cells[Row, 2, Row, ssSpo2RR_Sheet1.ColumnCount - 1].Border = complexBorder;
            }
            ssSpo2RR_Sheet1.Cells[ssSpo2RR_Sheet1.RowCount - 1, 2, ssSpo2RR_Sheet1.RowCount - 1, ssSpo2RR_Sheet1.ColumnCount - 1].Border = complexBorder;

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 간호활동 항목조회
        /// </summary>
        private void FormSearchAct()
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssAct_Sheet1.RowCount = 0;
            ssAct_Sheet1.Columns.Count = 3;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     B.BASVAL, B.BASCD, B.BASNAME, B.BASEXNAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "     ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "     AND BB.BSNSCLS = '기록지관리'  ";
            SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '간호활동그룹'"; //간호활동항목
            SQL = SQL + ComNum.VBLF + "     AND BB.USECLS = '0' ";
            SQL = SQL + ComNum.VBLF + "WHERE B.BSNSCLS = '기록지관리'  ";
            SQL = SQL + ComNum.VBLF + "     AND B.UNITCLS = '간호활동항목'"; //간호활동항목
            SQL = SQL + ComNum.VBLF + "     AND B.USECLS = '0' ";
            SQL = SQL + ComNum.VBLF + "     AND EXISTS (SELECT 1 FROM " + ComNum.DB_EMR + "AEMRUSERITEMVS U";
            SQL = SQL + ComNum.VBLF + "                 WHERE U.JOBGB = 'ACT'";
            SQL = SQL + ComNum.VBLF + "                        AND U.USEGB = '" + clsType.User.IdNumber + "'";
            SQL = SQL + ComNum.VBLF + "                        AND U.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "                 )";
            SQL = SQL + ComNum.VBLF + "     AND B.BASCD IN (SELECT R.ITEMCD FROM KOSMOS_EMR.AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "                       INNER JOIN KOSMOS_EMR.AEMRCHARTROW R ";
            SQL = SQL + ComNum.VBLF + "                           ON C.EMRNO = R.EMRNO ";
            SQL = SQL + ComNum.VBLF + "                           AND C.EMRNOHIS = R.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "                           AND R.ITEMCD IN (SELECT U.ITEMCD FROM KOSMOS_EMR.AEMRUSERITEMVS U ";
            SQL = SQL + ComNum.VBLF + "                                            WHERE U.JOBGB = 'ACT' ";
            SQL = SQL + ComNum.VBLF + "                                                   AND U.USEGB = '" + clsType.User.IdNumber + "' ";
            SQL = SQL + ComNum.VBLF + "                                            UNION ";
            SQL = SQL + ComNum.VBLF + "                                            SELECT U.ITEMCD FROM KOSMOS_EMR.AEMRUSERITEMVS U ";
            SQL = SQL + ComNum.VBLF + "                                            WHERE U.JOBGB = 'ACT' ";
            SQL = SQL + ComNum.VBLF + "                                                   AND U.USEGB = '" + clsType.User.BuseCode + "' ";
            SQL = SQL + ComNum.VBLF + "                                            )";
            SQL = SQL + ComNum.VBLF + "                           AND (R.ITEMVALUE IS NOT NULL) "; // OR R.ITEMVALUE1 IS NOT NULL) ";
            SQL = SQL + ComNum.VBLF + "                       WHERE C.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "                           AND C.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "                           AND C.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "                           AND C.FORMNO IN (1575) ";
            SQL = SQL + ComNum.VBLF + "                           AND C.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "                           AND C.CHARTUSEID <> '합계' ";
            SQL = SQL + ComNum.VBLF + "                       GROUP BY R.ITEMCD";
            SQL = SQL + ComNum.VBLF + "                     )";
            SQL = SQL + ComNum.VBLF + "ORDER BY BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strBASEXNAME = "";
            int intS = 0;

            keyActRow.Clear();
            lstRow.Clear();

            ssAct_Sheet1.RowCount = dt.Rows.Count;
            ssAct_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                keyActRow.Add(dt.Rows[i]["BASCD"].ToString().Trim(), i);
                ssAct_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssAct_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                ssAct_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();

                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    ssAct_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        ssAct_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
                        lstRow.Add(i - 1);
                    }
                    intS = i;

                    ssAct_Sheet1.Cells[i, 1].Font = boldFont;
                    ssAct_Sheet1.Cells[i, 1].Border = complexBorder;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            ssAct_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
            ssAct_Sheet1.SetRowHeight(-1, 40);
            dt.Dispose();
            dt = null;

            GetActingValue();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 임상관찰 값을 가져온다
        /// </summary>
        private void GetActingValue()
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.MaxLength = 1000;
            TypeText.Multiline = true;
            TypeText.WordWrap = true;

            Cursor.Current = Cursors.WaitCursor;
            ssAct_Sheet1.Columns.Count = 3;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    C.CHARTDATE, C.CHARTTIME, C.CHARTUSEID, C.EMRNO, ";
            SQL = SQL + ComNum.VBLF + "    R.ITEMCD, R.ITEMVALUE, R.ITEMVALUE1, ";
            SQL = SQL + ComNum.VBLF + "    B.BASEXNAME, B.BASNAME ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_EMR.AEMRCHARTROW R ";
            SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = R.EMRNO ";
            SQL = SQL + ComNum.VBLF + "    AND C.EMRNOHIS  = R.EMRNOHIS ";
            SQL = SQL + ComNum.VBLF + "    AND R.ITEMCD IN (SELECT U.ITEMCD FROM KOSMOS_EMR.AEMRUSERITEMVS U ";
            SQL = SQL + ComNum.VBLF + "                     WHERE U.JOBGB = 'ACT' ";
            SQL = SQL + ComNum.VBLF + "                            AND U.USEGB = '" + clsType.User.IdNumber + "' ";
            SQL = SQL + ComNum.VBLF + "                     UNION ";
            SQL = SQL + ComNum.VBLF + "                     SELECT U.ITEMCD FROM KOSMOS_EMR.AEMRUSERITEMVS U ";
            SQL = SQL + ComNum.VBLF + "                     WHERE U.JOBGB = 'ACT' ";
            SQL = SQL + ComNum.VBLF + "                            AND U.USEGB = '" + clsType.User.BuseCode + "' ";
            SQL = SQL + ComNum.VBLF + "                     )";
            SQL = SQL + ComNum.VBLF + "    AND (R.ITEMVALUE IS NOT NULL) "; // OR R.ITEMVALUE1 IS NOT NULL) ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_EMR.AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    ON R.ITEMCD = B.BASCD ";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS IN ('간호활동항목') ";
            SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "    AND C.FORMNO IN (1575) ";
            SQL = SQL + ComNum.VBLF + "    AND C.CHARTUSEID <> '합계' ";
            if (chkAsc.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) ASC";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY (C.CHARTDATE || C.CHARTTIME) DESC";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strEMRNO = "";
            string strCHARTDATE = "";
            int intDay = 0;
            Color DayColor = ComNum.SPDESELCOLOR;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (strCHARTDATE != dt.Rows[i]["CHARTDATE"].ToString().Trim())
                {
                    if (intDay == 0)
                    {
                        intDay = 1;
                        DayColor = ComNum.SPDESELCOLOR;
                    }
                    else
                    {
                        intDay = 0;
                        DayColor = ComNum.SPSELCOLOR;
                    }
                }
                strCHARTDATE = dt.Rows[i]["CHARTDATE"].ToString().Trim();

                if (strEMRNO != dt.Rows[i]["EMRNO"].ToString().Trim())
                {
                    ssAct_Sheet1.Columns.Count = ssAct_Sheet1.Columns.Count + 1;
                    ssAct_Sheet1.ColumnHeader.Cells[0, ssAct_Sheet1.Columns.Count - 1].Text = VB.Mid(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 5, 2) + "-" + VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 2);
                    ssAct_Sheet1.ColumnHeader.Cells[1, ssAct_Sheet1.Columns.Count - 1].Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");
                    ssAct_Sheet1.Columns[ssAct_Sheet1.Columns.Count - 1].CellType = TypeText;
                    ssAct_Sheet1.Columns[ssAct_Sheet1.Columns.Count - 1].Width = 80;
                    ssAct_Sheet1.Columns[ssAct_Sheet1.Columns.Count - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    ssAct_Sheet1.Columns[ssAct_Sheet1.Columns.Count - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    ssAct_Sheet1.Columns[ssAct_Sheet1.Columns.Count - 1].BackColor = DayColor;
                    ssAct_Sheet1.Columns[ssAct_Sheet1.Columns.Count - 1].Locked = true;
                }

                strEMRNO = dt.Rows[i]["EMRNO"].ToString().Trim();

                int intRow = FinedRow(ssAct_Sheet1, dt.Rows[i]["ITEMCD"].ToString().Trim());

                if (dt.Rows[i]["ITEMVALUE1"].ToString().Trim() != "")
                {
                    ssAct_Sheet1.Cells[intRow, ssAct_Sheet1.Columns.Count - 1].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim() + " " + dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                }
                else
                {
                    ssAct_Sheet1.Cells[intRow, ssAct_Sheet1.Columns.Count - 1].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                }

                ssAct_Sheet1.Cells[intRow, ssAct_Sheet1.Columns.Count - 1].ForeColor = TextForeColor(ssAct_Sheet1.Cells[intRow, ssAct_Sheet1.Columns.Count - 1].Text);
                //ssAct_Sheet1.SetRowHeight(intRow, Convert.ToInt32((ssAct_Sheet1.GetPreferredRowHeight(intRow) + 2)));
            }

            dt.Dispose();
            dt = null;

            foreach (int Row in lstRow)
            {
                ssAct_Sheet1.Cells[Row, 2, Row, ssAct_Sheet1.ColumnCount - 1].Border = complexBorder;
            }
            ssAct_Sheet1.Cells[ssAct_Sheet1.RowCount - 1, 2, ssAct_Sheet1.RowCount - 1, ssAct_Sheet1.ColumnCount - 1].Border = complexBorder;

            Cursor.Current = Cursors.Default;
        }

        #endregion // Functions

        #region // Control Events
        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            GetData(sender);
        }

        private void btnRegItem_Click(object sender, EventArgs e)
        {
            using (frmEmrBaseViewVitalandActingItem frm = new frmEmrBaseViewVitalandActingItem())
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog(this);
            }
        }

        private void btnIO_Click(object sender, EventArgs e)
        {
            if (frmNrIONewX != null)
            {
                frmNrIONewX.Dispose();
                frmNrIONewX = null;
            }

            frmNrIONewX = new frmNrIONew2(AcpEmr);
            frmNrIONewX.StartPosition = FormStartPosition.CenterScreen;
            frmNrIONewX.FormClosed += FrmNrIONewX_FormClosed;
            frmNrIONewX.Show();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return;

            FpSpread spd = tabControl1.SelectedTab.Controls[0] as FpSpread;

            //Print Head 지정
            string strFont1 = "/fn\"굴림체\"/fz\"14\"/fb1/fi0/fu0/fk0/fs1";
            string strFont2 = "/fn\"굴림체\"/fz\"10\"/fb0/fi0/fu0/fk0/fs2";
            string strHead1 = string.Empty;
            string strHead2 = string.Empty;

            strHead1 = "/c/f1" + tabControl1.SelectedTab.Text + " 리스트" + "/f1/n/n";

            string strInOut = "내원일자";
            if (AcpEmr.inOutCls == "I")
            {
                strInOut = "입원일자";
            }

            strHead2 = "/l/f2" + "     성명 : " + AcpEmr.ptName + " [" + AcpEmr.ptNo + "]   (" + AcpEmr.age + "/" + AcpEmr.sex + ")" + "/f2/n";
            if (clsFormPrint.mstrPRINTFLAG == "1" || clsFormPrint.mstrPRINTFLAG == "2")
            {
                strHead2 = strHead2 + "/l/f2" + "     주민번호 : " + AcpEmr.ssno1 + "-" + (AcpEmr.ssno2.Length == 0 ? "X" : AcpEmr.ssno2.Substring(0, 1)) + "/f2/n";
            }
            else
            {
                strHead2 = strHead2 + "/l/f2" + "     주민번호 : " + AcpEmr.ssno1 + "-" + VB.Left(AcpEmr.ssno2, 1) + "/f2/n";
            }

            strHead2 = strHead2 + "/l/f2" + "     진료과/주치의(" + strInOut + ") : " + AcpEmr.medDeptKorName + " / " + AcpEmr.medDrName + " (" + ComFunc.FormatStrToDate(AcpEmr.medFrDate, "D") + ")" + "/f2/n";

            //if (clsCommon.gstrEXENAME != "MHEMRJOBDG.EXE")
            //{
            //    strHead2 = strHead2 + "/l/f2" + "작성일자 : " + ComFunc.FormatStrToDate(strFrDate, "D") + " ~ " + ComFunc.FormatStrToDate(strEndDate, "D") + "/f2/n";
            //}

            strHead2 = strHead2 + "/l/f2" + "     출력자(출력일자) : " + clsType.User.UserName + "(" + ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10) + ")" + "/f2/n/n";


            spd.ActiveSheet.PrintInfo.AbortMessage = "현재 출력중입니다..";
            spd.ActiveSheet.PrintInfo.HeaderHeight = 120;
            spd.ActiveSheet.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;

            spd.ActiveSheet.PrintInfo.Margin.Top = 20;
            spd.ActiveSheet.PrintInfo.Orientation = PrintOrientation.Landscape;
            spd.ActiveSheet.PrintInfo.Centering = Centering.Horizontal;
            spd.ActiveSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            spd.ActiveSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            spd.ActiveSheet.PrintInfo.ShowBorder = true;
            spd.ActiveSheet.PrintInfo.ShowColor = true;
            spd.ActiveSheet.PrintInfo.ShowGrid = true;
            spd.ActiveSheet.PrintInfo.ShowShadows = true;
            spd.ActiveSheet.PrintInfo.UseMax = false;
            spd.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            spd.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion // Control Events

        private void FrmNrIONewX_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmNrIONewX != null)
            {
                frmNrIONewX.Dispose();
                frmNrIONewX = null;
            }
        }

        private void frmEmrBaseViewVitalandActing_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmNrIONewX != null)
            {
                frmNrIONewX.Dispose();
                frmNrIONewX = null;
            }
        }

        static string READ_Job_UserName(string ArgCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT KORNAME  FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN ='" + ArgCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["KORNAME"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return rtnVal;
        }

        private void btnViewInpUser_Click(object sender, EventArgs e)
        {
            string strFlag = "보기";

            if (btnViewInpUser.Text.Trim() == "작성자 보기")
            {
                strFlag = "보기";
                btnViewInpUser.Text = "작성자 숨김";
            }
            else
            {
                strFlag = "숨김";
                btnViewInpUser.Text = "작성자 보기";
            }

            int Row = 0;
            int Column = 0;
            Font font = new Font("굴림", 10);

            for (Row = 0; Row <= ssVital_Sheet1.RowCount - 1; Row++)
            {
                for (Column = 2; Column < ssVital_Sheet1.ColumnCount; Column++)
                {
                    if (ssVital_Sheet1.Cells[Row, Column].Tag != null)
                    {
                        if (ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim() != "")
                        {
                            if (ssVital_Sheet1.Cells[Row, Column].Text.Trim() != "")
                            {
                                if (strFlag == "보기")
                                {
                                    //ssVital_Sheet1.Cells[Row, Column].Text = ssVital_Sheet1.Cells[Row, Column].Text.Trim() + ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim();
                                    //string strUseName = ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim();

                                    string strUseName = ComFunc.SptChar(ssVital_Sheet1.Cells[Row, Column].Tag.ToString(), 0, "/").Trim();
                                    strUseName = strUseName + ComNum.VBLF;
                                    strUseName = strUseName + ComFunc.SptChar(ssVital_Sheet1.Cells[Row, Column].Tag.ToString(), 1, "/").Trim() + " ";
                                    strUseName = strUseName + ComFunc.FormatStrToDate(ComFunc.SptChar(ssVital_Sheet1.Cells[Row, Column].Tag.ToString(), 2, "/").Trim(), "M");

                                    //Size TxtSize = TextRenderer.MeasureText(strUseName, font);
                                    //List<int> lstWidth = new List<int>();
                                    //lstWidth.Add(TxtSize.Width);

                                    StringBuilder strNote = new StringBuilder();
                                    strNote.AppendLine(strUseName);
                                    ////텍스트길이 계산
                                    //TxtSize = TextRenderer.MeasureText(strUseName, font);
                                    ////List에 넣기
                                    //lstWidth.Add(TxtSize.Width);

                                    ssVital_Sheet1.Cells[Row, Column].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupStickyNote;
                                    ssVital_Sheet1.Cells[Row, Column].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
                                    ssVital_Sheet1.Cells[Row, Column].NoteIndicatorSize = new Size(9, 9);
                                    ssVital_Sheet1.Cells[Row, Column].Note = strNote.ToString().Trim();
                                    string strINPUSEID = ComFunc.SptChar(ssVital_Sheet1.Cells[Row, Column].Tag.ToString(), 1, "/").Trim();
                                    if (strINPUSEID != clsType.User.IdNumber)
                                    {
                                        ssVital_Sheet1.Cells[Row, Column].NoteIndicatorColor = Color.Pink;
                                    }
                                    else
                                    {
                                        ssVital_Sheet1.Cells[Row, Column].NoteIndicatorColor = Color.SkyBlue;
                                    }
                                    FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
                                    nsinfo = ssVital_Sheet1.GetStickyNoteStyleInfo(Row, Column);

                                    nsinfo.Font = font;
                                    nsinfo.ForeColor = Color.Black;
                                    nsinfo.Width = 100; // lstWidth.Max(); //가장 긴 텍스트 길이에 맞춰서 너비 설정
                                    nsinfo.ShapeOutlineColor = Color.Red;
                                    nsinfo.ShapeOutlineThickness = 1;
                                    nsinfo.ShadowOffsetX = 3;
                                    nsinfo.ShadowOffsetY = 3;
                                    ssVital_Sheet1.SetStickyNoteStyleInfo(Row, 4, nsinfo);
                                }
                                else
                                {
                                    ssVital_Sheet1.Cells[Row, Column].Note = string.Empty;
                                    ssVital_Sheet1.Cells[Row, Column].Note = null;
                                }
                            }
                        }
                    }
                }
            }
            for (Row = 0; Row <= ssAct_Sheet1.RowCount - 1; Row++)
            {
                for (Column = 2; Column < ssAct_Sheet1.ColumnCount; Column++)
                {
                    if (ssAct_Sheet1.Cells[Row, Column].Tag != null)
                    {
                        if (ssAct_Sheet1.Cells[Row, Column].Tag.ToString().Trim() != "")
                        {
                            if (ssAct_Sheet1.Cells[Row, Column].Text.Trim() != "")
                            {
                                if (strFlag == "보기")
                                {
                                    //ssVital_Sheet1.Cells[Row, Column].Text = ssVital_Sheet1.Cells[Row, Column].Text.Trim() + ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim();
                                    //string strUseName = ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim();

                                    string strUseName = ComFunc.SptChar(ssAct_Sheet1.Cells[Row, Column].Tag.ToString(), 0, "/").Trim();
                                    strUseName = strUseName + ComNum.VBLF;
                                    strUseName = strUseName + ComFunc.SptChar(ssAct_Sheet1.Cells[Row, Column].Tag.ToString(), 1, "/").Trim() + " ";
                                    strUseName = strUseName + ComFunc.FormatStrToDate(ComFunc.SptChar(ssAct_Sheet1.Cells[Row, Column].Tag.ToString(), 2, "/").Trim(), "M");
                                    //Size TxtSize = TextRenderer.MeasureText(strUseName, font);
                                    //List<int> lstWidth = new List<int>();
                                    //lstWidth.Add(TxtSize.Width);

                                    StringBuilder strNote = new StringBuilder();
                                    strNote.AppendLine(strUseName);
                                    ////텍스트길이 계산
                                    //TxtSize = TextRenderer.MeasureText(strUseName, font);
                                    ////List에 넣기
                                    //lstWidth.Add(TxtSize.Width);

                                    ssAct_Sheet1.Cells[Row, Column].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupStickyNote;
                                    ssAct_Sheet1.Cells[Row, Column].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
                                    ssAct_Sheet1.Cells[Row, Column].NoteIndicatorSize = new Size(9, 9);
                                    ssAct_Sheet1.Cells[Row, Column].Note = strNote.ToString().Trim();
                                    string strINPUSEID = ComFunc.SptChar(ssAct_Sheet1.Cells[Row, Column].Tag.ToString(), 1, "/").Trim();
                                    if (strINPUSEID != clsType.User.IdNumber)
                                    {
                                        ssAct_Sheet1.Cells[Row, Column].NoteIndicatorColor = Color.Pink;
                                    }
                                    else
                                    {
                                        ssAct_Sheet1.Cells[Row, Column].NoteIndicatorColor = Color.SkyBlue;
                                    }
                                    FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
                                    nsinfo = ssAct_Sheet1.GetStickyNoteStyleInfo(Row, Column);

                                    nsinfo.Font = font;
                                    nsinfo.ForeColor = Color.Black;
                                    nsinfo.Width = 100; // lstWidth.Max(); //가장 긴 텍스트 길이에 맞춰서 너비 설정
                                    nsinfo.ShapeOutlineColor = Color.Red;
                                    nsinfo.ShapeOutlineThickness = 1;
                                    nsinfo.ShadowOffsetX = 3;
                                    nsinfo.ShadowOffsetY = 3;
                                    ssAct_Sheet1.SetStickyNoteStyleInfo(Row, 4, nsinfo);
                                }
                                else
                                {
                                    ssAct_Sheet1.Cells[Row, Column].Note = string.Empty;
                                    ssAct_Sheet1.Cells[Row, Column].Note = null;
                                }
                            }
                        }
                    }
                }
            }
            for (Row = 0; Row <= ssSpo2RR_Sheet1.RowCount - 1; Row++)
            {
                for (Column = 2; Column < ssSpo2RR_Sheet1.ColumnCount; Column++)
                {
                    if (ssSpo2RR_Sheet1.Cells[Row, Column].Tag != null)
                    {
                        if (ssSpo2RR_Sheet1.Cells[Row, Column].Tag.ToString().Trim() != "")
                        {
                            if (ssSpo2RR_Sheet1.Cells[Row, Column].Text.Trim() != "")
                            {
                                if (strFlag == "보기")
                                {
                                    //ssVital_Sheet1.Cells[Row, Column].Text = ssVital_Sheet1.Cells[Row, Column].Text.Trim() + ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim();
                                    //string strUseName = ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim();

                                    string strUseName = ComFunc.SptChar(ssSpo2RR_Sheet1.Cells[Row, Column].Tag.ToString(), 0, "/").Trim();
                                    strUseName = strUseName + ComNum.VBLF;
                                    strUseName = strUseName + ComFunc.SptChar(ssSpo2RR_Sheet1.Cells[Row, Column].Tag.ToString(), 1, "/").Trim() + " ";
                                    strUseName = strUseName + ComFunc.FormatStrToDate(ComFunc.SptChar(ssSpo2RR_Sheet1.Cells[Row, Column].Tag.ToString(), 2, "/").Trim(), "M");

                                    //Size TxtSize = TextRenderer.MeasureText(strUseName, font);
                                    //List<int> lstWidth = new List<int>();
                                    //lstWidth.Add(TxtSize.Width);

                                    StringBuilder strNote = new StringBuilder();
                                    strNote.AppendLine(strUseName);
                                    ////텍스트길이 계산
                                    //TxtSize = TextRenderer.MeasureText(strUseName, font);
                                    ////List에 넣기
                                    //lstWidth.Add(TxtSize.Width);

                                    ssSpo2RR_Sheet1.Cells[Row, Column].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupStickyNote;
                                    ssSpo2RR_Sheet1.Cells[Row, Column].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
                                    ssSpo2RR_Sheet1.Cells[Row, Column].NoteIndicatorSize = new Size(9, 9);
                                    ssSpo2RR_Sheet1.Cells[Row, Column].Note = strNote.ToString().Trim();
                                    string strINPUSEID = ComFunc.SptChar(ssSpo2RR_Sheet1.Cells[Row, Column].Tag.ToString(), 1, "/").Trim();
                                    if (strINPUSEID != clsType.User.IdNumber)
                                    {
                                        ssSpo2RR_Sheet1.Cells[Row, Column].NoteIndicatorColor = Color.Pink;
                                    }
                                    else
                                    {
                                        ssSpo2RR_Sheet1.Cells[Row, Column].NoteIndicatorColor = Color.SkyBlue;
                                    }
                                    FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
                                    nsinfo = ssSpo2RR_Sheet1.GetStickyNoteStyleInfo(Row, Column);

                                    nsinfo.Font = font;
                                    nsinfo.ForeColor = Color.Black;
                                    nsinfo.Width = 100; // lstWidth.Max(); //가장 긴 텍스트 길이에 맞춰서 너비 설정
                                    nsinfo.ShapeOutlineColor = Color.Red;
                                    nsinfo.ShapeOutlineThickness = 1;
                                    nsinfo.ShadowOffsetX = 3;
                                    nsinfo.ShadowOffsetY = 3;
                                    ssSpo2RR_Sheet1.SetStickyNoteStyleInfo(Row, 4, nsinfo);
                                }
                                else
                                {
                                    ssSpo2RR_Sheet1.Cells[Row, Column].Note = string.Empty;
                                    ssSpo2RR_Sheet1.Cells[Row, Column].Note = null;
                                }
                            }
                        }
                    }
                }
            }
            for (Row = 0; Row <= ssRecord3_Sheet1.RowCount - 1; Row++)
            {
                for (Column = 2; Column < ssRecord3_Sheet1.ColumnCount; Column++)
                {
                    if (ssRecord3_Sheet1.Cells[Row, Column].Tag != null)
                    {
                        if (ssRecord3_Sheet1.Cells[Row, Column].Tag.ToString().Trim() != "")
                        {
                            if (ssRecord3_Sheet1.Cells[Row, Column].Text.Trim() != "")
                            {
                                if (strFlag == "보기")
                                {
                                    //ssVital_Sheet1.Cells[Row, Column].Text = ssVital_Sheet1.Cells[Row, Column].Text.Trim() + ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim();
                                    //string strUseName = ssVital_Sheet1.Cells[Row, Column].Tag.ToString().Trim();

                                    string strUseName = ComFunc.SptChar(ssRecord3_Sheet1.Cells[Row, Column].Tag.ToString(), 0, "/").Trim();
                                    strUseName = strUseName + ComNum.VBLF;
                                    strUseName = strUseName + ComFunc.SptChar(ssRecord3_Sheet1.Cells[Row, Column].Tag.ToString(), 1, "/").Trim() + " ";
                                    strUseName = strUseName + ComFunc.FormatStrToDate(ComFunc.SptChar(ssRecord3_Sheet1.Cells[Row, Column].Tag.ToString(), 2, "/").Trim(), "M");

                                    //Size TxtSize = TextRenderer.MeasureText(strUseName, font);
                                    //List<int> lstWidth = new List<int>();
                                    //lstWidth.Add(TxtSize.Width);

                                    StringBuilder strNote = new StringBuilder();
                                    strNote.AppendLine(strUseName);
                                    ////텍스트길이 계산
                                    //TxtSize = TextRenderer.MeasureText(strUseName, font);
                                    ////List에 넣기
                                    //lstWidth.Add(TxtSize.Width);

                                    ssRecord3_Sheet1.Cells[Row, Column].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupStickyNote;
                                    ssRecord3_Sheet1.Cells[Row, Column].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
                                    ssRecord3_Sheet1.Cells[Row, Column].NoteIndicatorSize = new Size(9, 9);
                                    ssRecord3_Sheet1.Cells[Row, Column].Note = strNote.ToString().Trim();
                                    string strINPUSEID = ComFunc.SptChar(ssRecord3_Sheet1.Cells[Row, Column].Tag.ToString(), 1, "/").Trim();
                                    if (strINPUSEID != clsType.User.IdNumber)
                                    {
                                        ssRecord3_Sheet1.Cells[Row, Column].NoteIndicatorColor = Color.Pink;
                                    }
                                    else
                                    {
                                        ssRecord3_Sheet1.Cells[Row, Column].NoteIndicatorColor = Color.SkyBlue;
                                    }
                                    FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
                                    nsinfo = ssRecord3_Sheet1.GetStickyNoteStyleInfo(Row, Column);

                                    nsinfo.Font = font;
                                    nsinfo.ForeColor = Color.Black;
                                    nsinfo.Width = 100; // lstWidth.Max(); //가장 긴 텍스트 길이에 맞춰서 너비 설정
                                    nsinfo.ShapeOutlineColor = Color.Red;
                                    nsinfo.ShapeOutlineThickness = 1;
                                    nsinfo.ShadowOffsetX = 3;
                                    nsinfo.ShadowOffsetY = 3;
                                    ssRecord3_Sheet1.SetStickyNoteStyleInfo(Row, 4, nsinfo);
                                }
                                else
                                {
                                    ssRecord3_Sheet1.Cells[Row, Column].Note = string.Empty;
                                    ssRecord3_Sheet1.Cells[Row, Column].Note = null;
                                }
                            }
                        }
                    }
                }
            }

        }

    }
}
