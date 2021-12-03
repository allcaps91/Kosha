using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public partial class frmDaySugaInterface : Form
    {
        public frmDaySugaInterface()
        {
            InitializeComponent();
        }

        private void frmDaySugaInterface_Load(object sender, EventArgs e)
        {
            SS1_Sheet1.RowCount = 0;
            WardList();
        }

        private void WardList()
        {
            #region ComboWard_SET()
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            int sIndex = -1;
            int sCount = 0;

            string WardCodes = string.Empty;
            if (VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                WardCodes = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

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
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
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
                            if (reader.GetValue(0).ToString().Trim().Equals(WardCodes))
                            {
                                sIndex = sCount;
                            }
                            sCount += 1;
                        }
                    }
                }

                cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex + 1;
                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "BST Interface WardList()", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion

            bool Manager = NURSE_Manager_Check(clsType.User.IdNumber);
            if (Manager == true || clsVbfunc.GetBCodeCODE(clsDB.DbCon, "NUR_간호부관리자사번IP", "").Equals(clsCompuInfo.gstrCOMIP))
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
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            READ_DATA();
        }

        #region 조회 함수
        /// <summary>
        /// 데이터 가져오기
        /// </summary>
        void READ_DATA()
        {
            SS1_Sheet1.RowCount = 0;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL += ComNum.VBLF + "WITH M AS                                                                                                            ";
                SQL += ComNum.VBLF + "(                                                                                                                    ";
                SQL += ComNum.VBLF + "    SELECT A.PTNO                                                                                                    ";
                SQL += ComNum.VBLF + "         , A.CHARTDATE || ' ' || A.CHARTTIME AS CHARTDATETIME                                                        ";
                SQL += ComNum.VBLF + "         , R.ITEMCD                                                                                                  ";
                SQL += ComNum.VBLF + "         , CASE";
                SQL += ComNum.VBLF + "           WHEN(R.ITEMCD = 'I0000037245' OR R.ITEMCD = 'I0000008710_1' OR R.ITEMCD = 'I0000030047' OR R.ITEMCD = 'I0000037576')   ";
                SQL += ComNum.VBLF + "                 THEN REPLACE(REPLACE(UPPER(R.ITEMVALUE),'L',''),'%','')";
                SQL += ComNum.VBLF + "                 ELSE UPPER(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(UPPER(R.ITEMVALUE),'시작', 'Start'),'유지', 'Keep'),'제거', 'Remove'),'종료', 'Remove'),'퇴원', 'Remove'))";
                SQL += ComNum.VBLF + "           END ITEMVALUE                                                                                                          ";
                SQL += ComNum.VBLF + "      FROM KOSMOS_EMR.AEMRCHARTMST A                                                                                 ";
                SQL += ComNum.VBLF + "     INNER JOIN KOSMOS_EMR.AEMRCHARTROW R                                                                            ";
                SQL += ComNum.VBLF + "        ON A.EMRNO = R.EMRNO                                                                                         ";
                SQL += ComNum.VBLF + "       AND A.EMRNOHIS = R.EMRNOHIS                                                                                   ";
                SQL += ComNum.VBLF + "     INNER JOIN KOSMOS_PMPA.IPD_NEW_MASTER P                                                                         ";
                SQL += ComNum.VBLF + "        ON A.PTNO = P.PANO                                                                                           ";
                SQL += ComNum.VBLF + "       AND A.CHARTDATE >= TO_CHAR(P.INDATE, 'YYYYMMDD')";
                SQL += ComNum.VBLF + "     INNER JOIN KOSMOS_EMR.AEMRBVITALSET V                                                                        ";
                SQL += ComNum.VBLF + "        ON A.PTNO = V.PTNO                                                                                           ";
                SQL += ComNum.VBLF + "       AND V.FORMNO = 3150";
                SQL += ComNum.VBLF + "       AND V.JOBGB IN ('IIO', 'IVT', 'IBN', 'IST')";
                SQL += ComNum.VBLF + "       AND V.CHARTDATE = '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "     WHERE A.FORMNO  = 3150 --                                                                                       ";
                SQL += ComNum.VBLF + "       AND A.CHARTDATE <= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "       AND A.MEDFRDATE = TO_CHAR(P.INDATE, 'YYYYMMDD')";
                SQL += ComNum.VBLF + "       AND R.ITEMCD IN ('I0000037245', 'I0000037254', 'I0000037859','I0000037280', 'I0000008710_1', 'I0000030047', 'I0000037576','I0000024733')     ";
                SQL += ComNum.VBLF + "       AND V.ITEMCD IN ('I0000037245', 'I0000037254', 'I0000037859','I0000037280', 'I0000008710_1', 'I0000030047', 'I0000037576','I0000024733')     ";
                SQL += ComNum.VBLF + "       AND R.ITEMVALUE IS NOT NULL                                                                                   ";
                SQL += ComNum.VBLF + "       AND (P.JDATE = TO_DATE('1900-01-01') OR P.JDATE >= TO_DATE('" + dtpSDATE.Value.ToString("yyyyMMdd") + "'))         ";
                SQL += ComNum.VBLF + "       AND P.WARDCODE = '" + cboWard.Text.Trim() + "'";
                SQL += ComNum.VBLF + ")                                                                                                                    ";
                SQL += ComNum.VBLF + ", S AS                                                                           ";
                SQL += ComNum.VBLF + "(                                                                                ";
                SQL += ComNum.VBLF + "    SELECT A.PTNO                                                                ";
                SQL += ComNum.VBLF + "         , A.CHARTDATETIME                                                       ";
                SQL += ComNum.VBLF + "         , (SELECT 'Y'                                                           ";
                SQL += ComNum.VBLF + "              FROM M                                                             ";
                SQL += ComNum.VBLF + "             WHERE ITEMCD IN ('I0000037859','I0000037280', 'I0000037254','I0000024733')        ";
                SQL += ComNum.VBLF + "               AND ITEMVALUE IN ('REMOVE', '마침', 'KEEP이동', 'KEEP 이동')          ";
                SQL += ComNum.VBLF + "               AND SUBSTR(CHARTDATETIME, 0, 11) = SUBSTR(A.CHARTDATETIME, 0, 11) ";
                SQL += ComNum.VBLF + "               AND SUBSTR(CHARTDATETIME, 0, 13) <= SUBSTR(A.CHARTDATETIME, 0, 13) ";
                SQL += ComNum.VBLF + "               AND PTNO = A.PTNO                                            ";
                SQL += ComNum.VBLF + "               AND ROWNUM = 1                                                    ";
                SQL += ComNum.VBLF + "           ) AS Y                                                                ";
                SQL += ComNum.VBLF + "      FROM                                                                       ";
                SQL += ComNum.VBLF + "           (SELECT PTNO, CHARTDATETIME                                           ";
                SQL += ComNum.VBLF + "              FROM M                                                             ";
                SQL += ComNum.VBLF + "             WHERE ITEMCD IN ('I0000037859','I0000037280', 'I0000037254')        ";
                SQL += ComNum.VBLF + "               AND ITEMVALUE = 'START'                                           ";
                SQL += ComNum.VBLF + "            UNION                                                                ";
                SQL += ComNum.VBLF + "            SELECT PTNO, MIN(CHARTDATETIME) AS CHARTDATETIME                     ";
                SQL += ComNum.VBLF + "              FROM M                                                             ";
                SQL += ComNum.VBLF + "             WHERE ITEMCD IN ('I0000037859','I0000037280', 'I0000037254')        ";
                SQL += ComNum.VBLF + "             GROUP BY PTNO                                                       ";
                SQL += ComNum.VBLF + "          ) A                                                                    ";
                SQL += ComNum.VBLF + ")                                                                                ";
                SQL += ComNum.VBLF + ", R AS                                                                                     ";
                SQL += ComNum.VBLF + "(                                                                                          ";
                SQL += ComNum.VBLF + "    SELECT A.PTNO, A.CHARTDATETIME                                                         ";
                SQL += ComNum.VBLF + "         , (SELECT 'Y'                                                                     ";
                SQL += ComNum.VBLF + "              FROM M                                                                       ";
                SQL += ComNum.VBLF + "             WHERE ITEMCD IN ('I0000037859','I0000037280', 'I0000037254')                                 ";
                SQL += ComNum.VBLF + "               AND ITEMVALUE = 'START'                                                     ";
                SQL += ComNum.VBLF + "               AND SUBSTR(CHARTDATETIME, 0, 11) = SUBSTR(A.CHARTDATETIME, 0, 11)           ";
                SQL += ComNum.VBLF + "               AND SUBSTR(CHARTDATETIME, 0, 13) >= SUBSTR(A.CHARTDATETIME, 0, 13)           ";
                SQL += ComNum.VBLF + "               AND PTNO = A.PTNO                                            ";
                SQL += ComNum.VBLF + "               AND ROWNUM = 1                                                              ";
                SQL += ComNum.VBLF + "           ) AS Y                                                                          ";
                SQL += ComNum.VBLF + "      FROM M A                                                                             ";
                SQL += ComNum.VBLF + "     WHERE A.ITEMCD IN ('I0000037859','I0000037280', 'I0000037254','I0000024733')                                       ";
                SQL += ComNum.VBLF + "       AND A.ITEMVALUE IN ('REMOVE', '마침', 'KEEP이동', 'KEEP 이동')                                    ";
                SQL += ComNum.VBLF + "     GROUP BY A.PTNO, A.CHARTDATETIME                                                      ";
                SQL += ComNum.VBLF + ")                                                                                          ";
                SQL += ComNum.VBLF + ", TERM1 AS                                                                          ";
                SQL += ComNum.VBLF + "(                                                                                   ";
                SQL += ComNum.VBLF + "    SELECT                                                                          ";
                SQL += ComNum.VBLF + "           PTNO                                                                     ";
                SQL += ComNum.VBLF + "         , CASE                                                                     ";
                SQL += ComNum.VBLF + "           WHEN SUBSTR(AA.STIME,0,8) < '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "                THEN '0000'                                                         ";
                SQL += ComNum.VBLF + "                ELSE SUBSTR(AA.STIME,10,4)                                          ";
                SQL += ComNum.VBLF + "           END STIME                                                                ";
                SQL += ComNum.VBLF + "         , CASE                                                                     ";
                SQL += ComNum.VBLF + "           WHEN AA.ETIME IS NULL                                                    ";
                SQL += ComNum.VBLF + "                THEN (CASE                                                          ";
                SQL += ComNum.VBLF + "                      WHEN TO_CHAR(SYSDATE,'YYYYMMDD') = '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "                           THEN TO_CHAR(SYSDATE,'HH24MI')                           ";
                SQL += ComNum.VBLF + "                           ELSE '23'                                                ";
                SQL += ComNum.VBLF + "                      END)                                                          ";
                SQL += ComNum.VBLF + "                --ELSE LPAD(TO_CHAR(SUBSTR(AA.ETIME,10,2) -1), 2, '0')              ";
                SQL += ComNum.VBLF + "                ELSE SUBSTR(AA.ETIME,10,4)                                          ";
                SQL += ComNum.VBLF + "           END ETIME                                                                ";
                SQL += ComNum.VBLF + "      FROM (SELECT                                                                  ";
                SQL += ComNum.VBLF + "                   A.PTNO                                                           ";
                SQL += ComNum.VBLF + "                 , A.CHARTDATETIME AS STIME                                         ";
                SQL += ComNum.VBLF + "                 , (SELECT MIN(CHARTDATETIME)                                       ";
                SQL += ComNum.VBLF + "                      FROM R                                                        ";
                SQL += ComNum.VBLF + "                     WHERE PTNO = A.PTNO                                            ";
                SQL += ComNum.VBLF + "                       AND CHARTDATETIME >= A.CHARTDATETIME                         ";
                SQL += ComNum.VBLF + "                       AND Y IS NULL                                                ";
                SQL += ComNum.VBLF + "                    ) AS ETIME                                                      ";
                SQL += ComNum.VBLF + "              FROM S A                                                              ";
                SQL += ComNum.VBLF + "             WHERE Y IS NULL) AA                                                    ";
                SQL += ComNum.VBLF + "     WHERE (SUBSTR(AA.ETIME,0,8)  = '" + dtpSDATE.Value.ToString("yyyyMMdd") + "' OR AA.ETIME IS NULL)                     ";
                SQL += ComNum.VBLF + ")                                                                                   ";
                SQL += ComNum.VBLF + ",TERM AS                                                                            ";
                SQL += ComNum.VBLF + "(                                                                                   ";
                SQL += ComNum.VBLF + "    SELECT PTNO                                                                     ";
                SQL += ComNum.VBLF + "         , SUBSTR(STIME,0,2) AS STIME                                               ";
                SQL += ComNum.VBLF + "         , CASE                                                                     ";
                SQL += ComNum.VBLF + "           WHEN SUBSTR(STIME,3,2) >= SUBSTR(ETIME,3,2)                              ";
                SQL += ComNum.VBLF + "                THEN LPAD(SUBSTR(ETIME,0,2)-1, 2, '0')                                ";
                SQL += ComNum.VBLF + "                ELSE SUBSTR(ETIME,0,2)                                              ";
                SQL += ComNum.VBLF + "           END ETIME                                                                ";
                SQL += ComNum.VBLF + "      FROM TERM1                                                                    ";
                SQL += ComNum.VBLF + ")                                                                                   ";
                SQL += ComNum.VBLF + ", VAL AS                                                                                                             ";
                SQL += ComNum.VBLF + "(                                                                                                                    ";
                SQL += ComNum.VBLF + "     SELECT PTNO, VALDATE, VALTIME , MAX(VALUE) AS VALUE                                                              ";
                SQL += ComNum.VBLF + "      FROM                                                                                                           ";
                SQL += ComNum.VBLF + "           (SELECT A.PTNO                                                                                            ";
                SQL += ComNum.VBLF + "                 , SUBSTR(A.CHARTDATETIME,0,8) AS VALDATE                                                            ";
                SQL += ComNum.VBLF + "                 , SUBSTR(A.CHARTDATETIME,10,4) AS VALTIME                                                           ";
                SQL += ComNum.VBLF + "                 , CASE                                                                                              ";
                SQL += ComNum.VBLF + "                   WHEN A.ITEMCD = 'I0000037576'                                                                     ";
                SQL += ComNum.VBLF + "                        THEN (SELECT VFLAG2                                                                          ";
                SQL += ComNum.VBLF + "                                FROM KOSMOS_EMR.AEMRBASCD                                                            ";
                SQL += ComNum.VBLF + "                               WHERE BSNSCLS = '기록지관리'                                                             ";
                SQL += ComNum.VBLF + "                                 AND UNITCLS = '산소관리'                                                               ";
                SQL += ComNum.VBLF + "                                 AND VFLAG1 = A.ITEMVALUE)                                                           ";
                SQL += ComNum.VBLF + "                        ELSE (SELECT VFLAG2                                                                          ";
                SQL += ComNum.VBLF + "                                FROM KOSMOS_EMR.AEMRBASCD                                                            ";
                SQL += ComNum.VBLF + "                               WHERE BSNSCLS = '기록지관리'                                                             ";
                SQL += ComNum.VBLF + "                                 AND UNITCLS = '산소관리'                                                               ";
                SQL += ComNum.VBLF + "                                 AND BASCD = A.ITEMVALUE)                                                            ";
                SQL += ComNum.VBLF + "                   END VALUE                                                                                         ";
                SQL += ComNum.VBLF + "              FROM M A                                                                                               ";
                SQL += ComNum.VBLF + "             WHERE A.ITEMCD IN('I0000037245', 'I0000008710_1', 'I0000030047', 'I0000037576')                         ";
                SQL += ComNum.VBLF + "           )                                                                                                         ";
                SQL += ComNum.VBLF + "     WHERE VALUE IS NOT NULL                                                                                         ";
                SQL += ComNum.VBLF + "     GROUP BY PTNO, VALDATE, VALTIME                                                                                 ";
                SQL += ComNum.VBLF + ")                                                                                                                    ";
                SQL += ComNum.VBLF + ", TIMELIST AS                                                                                                        ";
                SQL += ComNum.VBLF + "(                                                                                                                    ";
                SQL += ComNum.VBLF + "    SELECT A.PTNO, B.CHARTTIME                                                                                       ";
                SQL += ComNum.VBLF + "      FROM TERM A                                                                                                    ";
                SQL += ComNum.VBLF + "     INNER JOIN (SELECT LPAD(ATIME, 2, '0') AS CHARTTIME                                                             ";
                SQL += ComNum.VBLF + "      FROM (SELECT TIEM1 + LEVEL -1 AS ATIME                                                                         ";
                SQL += ComNum.VBLF + "             FROM (SELECT 0 AS TIEM1 , 23 AS TIEM2                                                                   ";
                SQL += ComNum.VBLF + "                     FROM DUAL A)                                                                                    ";
                SQL += ComNum.VBLF + "                  CONNECT BY LEVEL <= TIEM2 - TIEM1 + 1                                                              ";
                SQL += ComNum.VBLF + "           )) B                                                                                                      ";
                SQL += ComNum.VBLF + "        ON B.CHARTTIME BETWEEN A.STIME AND A.ETIME                                                                   ";
                SQL += ComNum.VBLF + ")                                                                                                                    ";
                SQL += ComNum.VBLF + ", MM AS                                                                                                              ";
                SQL += ComNum.VBLF + "(                                                                                                                    ";
                SQL += ComNum.VBLF + "    SELECT A.PTNO                                                                                                    ";
                SQL += ComNum.VBLF + "         , A.CHARTTIME                                                                                               ";
                SQL += ComNum.VBLF + "         , COALESCE((SELECT MAX(VALUE)                                                                                    ";
                SQL += ComNum.VBLF + "                       FROM VAL                                                                                      ";
                SQL += ComNum.VBLF + "                      WHERE VALDATE = '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "                        AND PTNO = A.PTNO                                                                            ";
                SQL += ComNum.VBLF + "                        AND SUBSTR(VALTIME,0,2) = A.CHARTTIME                                                                    ";
                SQL += ComNum.VBLF + "                    ),                                                                                               ";
                SQL += ComNum.VBLF + "                    (SELECT VALUE                                                                                    ";
                SQL += ComNum.VBLF + "                       FROM VAL                                                                                      ";
                SQL += ComNum.VBLF + "                      WHERE PTNO = A.PTNO                                                                            ";
                SQL += ComNum.VBLF + "                        AND VALDATE || VALTIME = (SELECT MAX(AA.VALDATE || AA.VALTIME)                               ";
                SQL += ComNum.VBLF + "                                                    FROM VAL AA                                                      ";
                SQL += ComNum.VBLF + "                                                   WHERE AA.PTNO = A.PTNO                                            ";
                SQL += ComNum.VBLF + "                                                     AND AA.VALDATE || AA.VALTIME < '" + dtpSDATE.Value.ToString("yyyyMMdd") + "' || A.CHARTTIME)           ";
                SQL += ComNum.VBLF + "                    )                                                                                                ";
                SQL += ComNum.VBLF + "                   ) AS VALUE                                                                                        ";
                SQL += ComNum.VBLF + "      FROM TIMELIST A                                                                                                ";
                SQL += ComNum.VBLF + ")                                                                                                                    ";
                SQL += ComNum.VBLF + "SELECT P.ROOMCODE, P.SNAME, PTNO, VALUE, COUNT(PTNO || VALUE) AS QTY                                                                      ";
                SQL += ComNum.VBLF + " , E.BASNAME AS ORDERNM                    ";
                SQL += ComNum.VBLF + " , E.BASEXNAME AS SUCODE                    ";
                SQL += ComNum.VBLF + " , E.VFLAG1 AS ORDERCODE                    ";
                SQL += ComNum.VBLF + " , TO_CHAR(P.INDATE, 'YYYYMMDD') MEDFRDATE ";
                SQL += ComNum.VBLF + " , P.DEPTCODE ";
                SQL += ComNum.VBLF + " , (SELECT SUM(QTY)         ";
                SQL += ComNum.VBLF + "      FROM KOSMOS_OCS.OCS_IORDER                          ";
                SQL += ComNum.VBLF + "     WHERE PTNO      = A.PTNO                                  ";
                SQL += ComNum.VBLF + "       AND BDATE     = TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')    ";
                SQL += ComNum.VBLF + "       AND SUCODE    = RPAD(E.BASEXNAME, 8, ' ')          ";
                SQL += ComNum.VBLF + "       AND ORDERCODE = RPAD(E.VFLAG1, 8, ' ')             ";
                SQL += ComNum.VBLF + "       AND GBSTATUS  = ' '";
                SQL += ComNum.VBLF + "       AND (GBIOE NOT IN ('E', 'EI') OR GBIOE IS NULL)";
                //SQL += ComNum.VBLF + "     HAVING SUM(NAL) > 0";
                SQL += ComNum.VBLF + "   ) AS OCSQTY                                           ";
                //SQL += ComNum.VBLF + " , CASE WHEN EXISTS  ";
                //SQL += ComNum.VBLF + "   ( ";
                //SQL += ComNum.VBLF + "   SELECT 1  ";
                //SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.OCS_IORDER";
                //SQL += ComNum.VBLF + "    WHERE PTNO = A.PTNO";
                //SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                //SQL += ComNum.VBLF + "      AND SUCODE    = RPAD(E.BASEXNAME, 8, ' ')";
                //SQL += ComNum.VBLF + "      AND ORDERCODE = RPAD(E.VFLAG1, 8, ' ')";
                //SQL += ComNum.VBLF + "     GROUP BY ORDERNO";
                //SQL += ComNum.VBLF + "     HAVING SUM(NAL) > 0";
                //SQL += ComNum.VBLF + "   ) THEN 1 END ORDERSEND ";
                SQL += ComNum.VBLF + "  FROM MM A                                                                                                           ";
                SQL += ComNum.VBLF + "     INNER JOIN KOSMOS_PMPA.IPD_NEW_MASTER P                                                                         ";
                SQL += ComNum.VBLF + "        ON A.PTNO = P.PANO                                                                                           ";
                SQL += ComNum.VBLF + "      LEFT OUTER JOIN KOSMOS_EMR.AEMRBASCD E                                                                         ";
                SQL += ComNum.VBLF + "        ON E.BSNSCLS = '기록지관리'    ";
                SQL += ComNum.VBLF + "       AND E.UNITCLS = '산소오더관리'   ";
                SQL += ComNum.VBLF + "       AND E.BASCD  =  A.VALUE         ";
                SQL += ComNum.VBLF + "  WHERE (P.JDATE = TO_DATE('1900-01-01') OR P.JDATE >= TO_DATE('" + dtpSDATE.Value.ToString("yyyyMMdd") + "'))         ";
                SQL += ComNum.VBLF + "    AND P.WARDCODE = '" + cboWard.Text.Trim() + "'";
                SQL += ComNum.VBLF + " GROUP BY P.ROOMCODE, P.SNAME, PTNO, TO_CHAR(P.INDATE, 'YYYYMMDD'), P.DEPTCODE,  VALUE, E.BASNAME, E.BASEXNAME, E.VFLAG1                                                                                                ";
                SQL += ComNum.VBLF + " ORDER BY P.ROOMCODE, P.SNAME, PTNO, VALUE                                                                                                ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.RowCount = dt.Rows.Count;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["OCSQTY"].To<int>() > 0)
                        {
                            SS1_Sheet1.Rows[i].BackColor = Color.Pink;
                            SS1_Sheet1.Cells[i, 0].Locked = true;

                            if (dt.Rows[i]["OCSQTY"].Equals(dt.Rows[i]["QTY"]) == false)
                            {
                                SS1_Sheet1.Cells[i, 13].Text = "처방과 기록지의 수량이 다릅니다!!";
                                SS1_Sheet1.Cells[i, 0].Locked = !(dt.Rows[i]["OCSQTY"].To<int>() < dt.Rows[i]["QTY"].To<int>());
                            }
                        }
                        else
                        {
                            if (dt.Rows[i]["QTY"].To<int>() > 24)
                            {
                                SS1_Sheet1.Cells[i, 13].Text = "START/REMOVE확인필요";
                            }
                        }

                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["MEDFRDATE"].ToString().Trim();
                        //SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ITEMNM"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["VALUE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["OCSQTY"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ORDERNM"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        //SS1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["CHANGEVAL"].ToString().Trim();

                    }
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }



        #endregion

        private void btnSend_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            using (Ftpedt FtpedtX = new Ftpedt())
            {
                string strFile = @"C:\PSMHEXE\Manual_Emr_Suga.pdf";
                string strHost = "/psnfs/psmhexe/manual";
                string strHostFile = "Manual_Emr_Suga.pdf";

                if (FtpedtX.FtpDownload("192.168.100.35", "pcnfs", "pcnfs1", strFile, strHostFile, strHost) == true)
                {
                    System.Diagnostics.Process.Start(strFile);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0)
                return;

            if (ComFunc.MsgBoxQEx(this, "정말 전송하시겠습니까?\r\n산소 시간을 다시 점검하시고 맞다면 예를 눌러주세요.") == DialogResult.No)
                return;


            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            int nOrderNo = 0;
            int intRowAffected = 0;

            EmrPatient pAcp = null;
            OracleDataReader reader = null;

            DateTime dtpSys = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (int i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    if (SS1_Sheet1.Cells[i, 0].Text.Equals("True"))
                    {
                        string strPano = SS1_Sheet1.Cells[i, 2].Text.Trim();
                        string strDeptCode = SS1_Sheet1.Cells[i, 4].Text.Trim();
                        string strMedFrDate = SS1_Sheet1.Cells[i, 5].Text.Trim();

                        pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPano, "I", strMedFrDate, strDeptCode);

                        if (pAcp == null)
                            continue;

                        int nQty = SS1_Sheet1.Cells[i, 7].Text.To<int>();
                        int OcsQty = SS1_Sheet1.Cells[i, 9].Text.To<int>();
                        if (nQty > OcsQty && SS1_Sheet1.Cells[i, 13].Text.IndexOf("수량") != -1)
                        {
                            nQty -= OcsQty;
                        }

                        string strGBInfo = SS1_Sheet1.Cells[i, 10].Text.Trim();
                        string strSabun = string.Empty;

                        string strSucode = SS1_Sheet1.Cells[i, 11].Text.Trim();
                        string strORDERCODE = SS1_Sheet1.Cells[i, 12].Text.Trim();

                        string strDosCode = string.Empty;

                        #region GET SABUN
                        SQL = " SELECT SABUN FROM KOSMOS_OCS.OCS_DOCTOR ";
                        SQL = SQL + ComNum.VBLF + " WHERE DRCODE = '" + pAcp.medDrCd + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GBOUT  = 'N' ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            return;
                        }

                        if (reader.HasRows && reader.Read())
                        {
                            strSabun = reader.GetValue(0).ToString().Trim();
                        }

                        reader.Dispose();
                        #endregion

                        #region DOSCODE
                        SQL = " SELECT SPECCODE  FROM KOSMOS_OCS.OCS_ORDERCODE ";
                        SQL = SQL + ComNum.VBLF + " WHERE ORDERCODE ='" + strORDERCODE + "' ";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            return;
                        }

                        if (reader.HasRows && reader.Read())
                        {
                            strDosCode = reader.GetValue(0).ToString().Trim();
                        }

                        reader.Dispose();
                        #endregion

                        #region 오더등록
                        //'<오더등록>---------------------------------------------------------------------------------------
                        SQL = "SELECT KOSMOS_OCS.SEQ_ORDERNO.NextVal nNEXTVAL FROM DUAL";
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            nOrderNo = (int)VB.Val(dt.Rows[0]["nNEXTVAL"].ToString().Trim());
                        }
                        dt.Dispose();
                        dt = null;


                        SQL = "INSERT INTO KOSMOS_OCS.OCS_IORDER ( Ptno, BDate,     Seqno,     DeptCode,  ";
                        SQL = SQL + ComNum.VBLF + "  DrCode,  StaffID,    Slipno,    OrderCode, SuCode,  Bun,      ";
                        SQL = SQL + ComNum.VBLF + "  GbOrder, Contents,   BContents, RealQty,   Qty,     RealNal,  ";
                        SQL = SQL + ComNum.VBLF + "  Nal,     DosCode,    GbInfo,    GbSelf,    GbSpc,   GbNgt,    ";
                        SQL = SQL + ComNum.VBLF + "  GbER,    GbPRN,      GbDiv,     GbBoth,    GbAct,   GbTFlag,  ";
                        SQL = SQL + ComNum.VBLF + "  GbSend,  GbPosition, GbStatus,  NurseID,   EntDate, WardCode, ";
                        SQL = SQL + ComNum.VBLF + "  RoomCode, Bi,        OrderNo,   Remark, ";
                        SQL = SQL + ComNum.VBLF + "  ActDate, GbGroup,    GbPort,    OrderSite, GBPICKUP, PICKUPSABUN, ";
                        SQL = SQL + ComNum.VBLF + "  PICKUPDATE, SUBUL_WARD, HIGHRISK, ";
                        SQL = SQL + ComNum.VBLF + "  CORDERCODE, CSUCODE, CBUN ) VALUES     ";
                        SQL = SQL + ComNum.VBLF + "( '" + strPano + "',     TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),  ";
                        SQL = SQL + ComNum.VBLF + "    999 ,     '" + strDeptCode + "',    ";
                        SQL = SQL + ComNum.VBLF + "  '" + strSabun + "',   '" + pAcp.medDrCd + "',    'A5',   ";
                        SQL = SQL + ComNum.VBLF + "  '" + strORDERCODE + "','" + strSucode + "',     '29',      ";
                        SQL = SQL + ComNum.VBLF + "  '',  0,0,";
                        SQL = SQL + ComNum.VBLF + "  " + nQty + ",   " + nQty + ",1,   ";
                        SQL = SQL + ComNum.VBLF + "   1, '" + strDosCode + "',    '" + strGBInfo + "',   ";
                        SQL = SQL + ComNum.VBLF + "  '',   '',      '',    ";
                        SQL = SQL + ComNum.VBLF + "  '',     ' ',   1 ,    ";
                        SQL = SQL + ComNum.VBLF + "  '0',   '',      '',  ";
                        SQL = SQL + ComNum.VBLF + "  '*',   '', ' ', ";
                        SQL = SQL + ComNum.VBLF + "  '" + clsType.User.IdNumber.PadLeft(5, '0') + "',  SysDate,   '" + cboWard.Text + "', ";
                        SQL = SQL + ComNum.VBLF + "  '" + pAcp.room + "', '" + pAcp.bi + "', " + nOrderNo + ", '', ";
                        SQL = SQL + ComNum.VBLF + "  TRUNC(SYSDATE),   '',  ";
                        SQL = SQL + ComNum.VBLF + "  '',   'TEL' , '*',  '" + clsType.User.IdNumber.PadLeft(5, '0') + "', SYSDATE,";
                        SQL = SQL + ComNum.VBLF + "  '', '', ";
                        SQL = SQL + ComNum.VBLF + "  '" + strORDERCODE + "','" + strSucode + "','29' ) ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(this, "처방자료를 저장하는데 오류가 발생되었습니다");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        #endregion

                        #region 이전 로직 비내산소 1일당
                        //int AMT = 0;

                        //SQL = "SELECT 1";
                        //SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER";
                        //SQL = SQL + ComNum.VBLF + "WHERE SUCODE = 'M0040'";
                        //SQL = SQL + ComNum.VBLF + "  AND BDATE  = TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                        //SQL = SQL + ComNum.VBLF + "  AND PTNO   = '" + pAcp.ptNo + "'";

                        //SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        //if (SqlErr.Length > 0)
                        //{
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        //    ComFunc.MsgBoxEx(this, SqlErr);
                        //    return;
                        //}

                        //if (reader.HasRows)
                        //{
                        //    AMT = 1;
                        //}

                        //reader.Dispose();

                        //if (AMT > 0)
                        //{
                        //    continue;
                        //}

                        ////'<오더등록>---------------------------------------------------------------------------------------
                        //SQL = "SELECT KOSMOS_OCS.SEQ_ORDERNO.NextVal nNEXTVAL FROM DUAL";
                        //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        //if (SqlErr != "")
                        //{
                        //    clsDB.setRollbackTran(clsDB.DbCon);
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        //    return;
                        //}

                        //if (dt.Rows.Count > 0)
                        //{
                        //    nOrderNo = (int)VB.Val(dt.Rows[0]["nNEXTVAL"].ToString().Trim());
                        //}
                        //dt.Dispose();
                        //dt = null;


                        //SQL = "INSERT INTO KOSMOS_OCS.OCS_IORDER ( Ptno, BDate,     Seqno,     DeptCode,  ";
                        //SQL = SQL + ComNum.VBLF + "  DrCode,  StaffID,    Slipno,    OrderCode, SuCode,  Bun,      ";
                        //SQL = SQL + ComNum.VBLF + "  GbOrder, Contents,   BContents, RealQty,   Qty,     RealNal,  ";
                        //SQL = SQL + ComNum.VBLF + "  Nal,     DosCode,    GbInfo,    GbSelf,    GbSpc,   GbNgt,    ";
                        //SQL = SQL + ComNum.VBLF + "  GbER,    GbPRN,      GbDiv,     GbBoth,    GbAct,   GbTFlag,  ";
                        //SQL = SQL + ComNum.VBLF + "  GbSend,  GbPosition, GbStatus,  NurseID,   EntDate, WardCode, ";
                        //SQL = SQL + ComNum.VBLF + "  RoomCode, Bi,        OrderNo,   Remark, ";
                        //SQL = SQL + ComNum.VBLF + "  ActDate, GbGroup,    GbPort,    OrderSite, GBPICKUP, PICKUPSABUN, ";
                        //SQL = SQL + ComNum.VBLF + "  PICKUPDATE, SUBUL_WARD, HIGHRISK, ";
                        //SQL = SQL + ComNum.VBLF + "  CORDERCODE, CSUCODE, CBUN ) VALUES     ";
                        //SQL = SQL + ComNum.VBLF + "( '" + strPano + "',     TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),  ";
                        //SQL = SQL + ComNum.VBLF + "    999 ,     '" + strDeptCode + "',    ";
                        //SQL = SQL + ComNum.VBLF + "  '" + strSabun + "',   '" + pAcp.medDrCd + "',    'A5',   ";
                        //SQL = SQL + ComNum.VBLF + "  'M0040','M0040',     '28',      ";
                        //SQL = SQL + ComNum.VBLF + "  '',  0,0,";
                        //SQL = SQL + ComNum.VBLF + "  1,   1,1,   ";
                        //SQL = SQL + ComNum.VBLF + "   1, '',    '',   ";
                        //SQL = SQL + ComNum.VBLF + "  '',   '',      '',    ";
                        //SQL = SQL + ComNum.VBLF + "  '',     ' ',   1 ,    ";
                        //SQL = SQL + ComNum.VBLF + "  '0',   '',      '',  ";
                        //SQL = SQL + ComNum.VBLF + "  '*',   '', ' ', ";
                        //SQL = SQL + ComNum.VBLF + "  '" + clsType.User.IdNumber.PadLeft(5, '0') + "',  SysDate,   '" + cboWard.Text + "', ";
                        //SQL = SQL + ComNum.VBLF + "  '" + pAcp.room + "', '" + pAcp.bi + "', " + nOrderNo + ", '', ";
                        //SQL = SQL + ComNum.VBLF + "  TRUNC(SYSDATE),   '',  ";
                        //SQL = SQL + ComNum.VBLF + "  '',   'TEL' , '*',  '" + clsType.User.IdNumber.PadLeft(5, '0') + "', SYSDATE,";
                        //SQL = SQL + ComNum.VBLF + "  '', '', ";
                        //SQL = SQL + ComNum.VBLF + "  'M0040','M0040','28' ) ";

                        //SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        //if (SqlErr != "")
                        //{
                        //    clsDB.setRollbackTran(clsDB.DbCon);
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //    ComFunc.MsgBoxEx(this, "처방자료를 저장하는데 오류가 발생되었습니다");
                        //    Cursor.Current = Cursors.Default;
                        //    return;
                        //}
                        #endregion
                        
                        #region 신규 로직
                        SQL = "    WITH M AS                                                                                                                                                                ";
                        SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "        SELECT A.PTNO                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "             , A.CHARTDATE || ' ' || A.CHARTTIME AS CHARTDATETIME                                                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "             , R.ITEMCD                                                                                                                                                           ";
                        SQL = SQL + ComNum.VBLF + "             , R.ITEMVALUE                                                                                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST A                                                                                                                                          ";
                        SQL = SQL + ComNum.VBLF + "         INNER JOIN KOSMOS_EMR.AEMRCHARTROW R                                                                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "            ON A.EMRNO = R.EMRNO                                                                                                                                                  ";
                        SQL = SQL + ComNum.VBLF + "           AND A.EMRNOHIS = R.EMRNOHIS                                                                                                                                            ";
                        SQL = SQL + ComNum.VBLF + "         WHERE A.FORMNO = 3150-- 임상관찰 기록                                                                                                                                      ";
                        SQL = SQL + ComNum.VBLF + "           AND A.MEDFRDATE = '" + strMedFrDate + "'                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "           AND A.CHARTDATE <= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "           AND R.ITEMCD IN('I0000001239_9', 'I0000031111_1', 'I0000037254', 'I0000037280')                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "           AND R.ITEMVALUE IS NOT NULL                                                                                                                                            ";
                        SQL = SQL + ComNum.VBLF + "           AND A.PTNO = '" + strPano + "'";
                        SQL = SQL + ComNum.VBLF + "    )                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "    , MM AS                                                                                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "        SELECT                                                                                                                                                                    ";
                        SQL = SQL + ComNum.VBLF + "               A.PTNO                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "             , A.CHARTDATETIME                                                                                                                                                    ";
                        SQL = SQL + ComNum.VBLF + "             , (CASE                                                                                                                                                              ";
                        SQL = SQL + ComNum.VBLF + "                WHEN B.ITEMCD = 'I0000001239_9'                                                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "                     THEN (CASE                                                                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "                           WHEN B.ITEMVALUE = 'High-flow'                                                                                                                         ";
                        SQL = SQL + ComNum.VBLF + "                                THEN 2--'하이플로어'                                                                                                                                ";
                        SQL = SQL + ComNum.VBLF + "                                ELSE 1--'비내산소'                                                                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "                           END)                                                                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "                     ELSE 3--'인공호흡기'                                                                                                                                           ";
                        SQL = SQL + ComNum.VBLF + "                 END) AS GUBUN                                                                                                                                                    ";
                        SQL = SQL + ComNum.VBLF + "             , (CASE                                                                                                                                                              ";
                        SQL = SQL + ComNum.VBLF + "                WHEN B.ITEMCD = 'I0000001239_9'                                                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "                     THEN(SELECT ITEMVALUE FROM M WHERE PTNO = A.PTNO AND CHARTDATETIME = A.CHARTDATETIME AND ITEMCD = 'I0000037280' AND ROWNUM = 1)                              ";
                        SQL = SQL + ComNum.VBLF + "                     ELSE(SELECT ITEMVALUE FROM M WHERE PTNO = A.PTNO AND CHARTDATETIME = A.CHARTDATETIME AND ITEMCD = 'I0000037254' AND ROWNUM = 1)--'인공호흡기'                  ";
                        SQL = SQL + ComNum.VBLF + "                 END) AS VALUE                                                                                                                                                    ";
                        SQL = SQL + ComNum.VBLF + "          FROM(SELECT PTNO, CHARTDATETIME FROM M GROUP BY PTNO, CHARTDATETIME) A                                                                                                  ";
                        SQL = SQL + ComNum.VBLF + "        INNER JOIN M B                                                                                                                                                            ";
                        SQL = SQL + ComNum.VBLF + "           ON A.PTNO = B.PTNO                                                                                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "           AND A.CHARTDATETIME = B.CHARTDATETIME                                                                                                                                  ";
                        SQL = SQL + ComNum.VBLF + "         WHERE B.ITEMCD IN('I0000001239_9','I0000031111_1')                                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "    )                                                                                                                                                                             ";

                        SQL = SQL + ComNum.VBLF + "	, MM2 AS                                                                                                      ";
                        SQL = SQL + ComNum.VBLF + "	(                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "		SELECT PTNO, CHARTDATETIME, GUBUN, VALUE                                                                  ";
                        SQL = SQL + ComNum.VBLF + "		  FROM MM                                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "		  WHERE EXISTS                                                                                            ";
                        SQL = SQL + ComNum.VBLF + "		  (                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "		     SELECT 1                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "		       FROM MM                                                                                            ";
                        SQL = SQL + ComNum.VBLF + "		      WHERE CHARTDATETIME >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + " 000000'                                                            ";
                        SQL = SQL + ComNum.VBLF + "		        AND CHARTDATETIME <= '" + dtpSDATE.Value.ToString("yyyyMMdd") + " 005959'                                                            ";
                        SQL = SQL + ComNum.VBLF + "		  )                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "		  AND  CHARTDATETIME >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + " 000000'                                                                 ";
                        SQL = SQL + ComNum.VBLF + "		UNION                                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "		SELECT PTNO, '" + dtpSDATE.Value.ToString("yyyyMMdd") + " 000000' AS CHARTDATETIME, GUBUN, VALUE                                             ";
                        SQL = SQL + ComNum.VBLF + "		  FROM MM                                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "		  WHERE NOT EXISTS                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "		  (                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "		     SELECT 1                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "		       FROM MM                                                                                            ";
                        SQL = SQL + ComNum.VBLF + "		      WHERE CHARTDATETIME >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + " 000000'                                                            ";
                        SQL = SQL + ComNum.VBLF + "		        AND CHARTDATETIME <= '" + dtpSDATE.Value.ToString("yyyyMMdd") + " 005959'                                                            ";
                        SQL = SQL + ComNum.VBLF + "		  )                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "		  AND CHARTDATETIME = (SELECT MAX(CHARTDATETIME) FROM MM WHERE CHARTDATETIME < '" + dtpSDATE.Value.ToString("yyyyMMdd") + " 000000')         ";
                        SQL = SQL + ComNum.VBLF + "		UNION                                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "		SELECT PTNO, CHARTDATETIME, GUBUN, VALUE                                                                  ";
                        SQL = SQL + ComNum.VBLF + "		  FROM MM                                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "		  WHERE NOT EXISTS                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "		  (                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "		     SELECT 1                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "		       FROM MM                                                                                            ";
                        SQL = SQL + ComNum.VBLF + "		      WHERE CHARTDATETIME >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + " 000000'                                                            ";
                        SQL = SQL + ComNum.VBLF + "		        AND CHARTDATETIME <= '" + dtpSDATE.Value.ToString("yyyyMMdd") + " 005959'                                                            ";
                        SQL = SQL + ComNum.VBLF + "		  )                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "		  AND  CHARTDATETIME >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + " 000000'                                                                 ";
                        SQL = SQL + ComNum.VBLF + "	)";
                        SQL = SQL + ComNum.VBLF + "    , S AS                                                                                                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "        SELECT*                                                                                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "          FROM                                                                                                                                                                    ";
                        SQL = SQL + ComNum.VBLF + "               (SELECT A.PTNO, A.CHARTDATETIME, A.GUBUN                                                                                                                           ";
                        SQL = SQL + ComNum.VBLF + "                  FROM MM2 A                                                                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "                 WHERE A.VALUE IN ('START', 'Start','시작')                                                                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "                 UNION ALL                                                                                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "                SELECT A.PTNO, MIN(A.CHARTDATETIME) AS CHARTDATETIME, A.GUBUN                                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "                 FROM MM2 A                                                                                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "                 GROUP BY A.PTNO, A.GUBUN)                                                                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "         GROUP BY PTNO, CHARTDATETIME, GUBUN                                                                                                                                      ";
                        SQL = SQL + ComNum.VBLF + "    )                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "    , R AS                                                                                                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "        SELECT A.PTNO, A.CHARTDATETIME, A.GUBUN                                                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "          FROM MM2 A                                                                                                                                                               ";
                        SQL = SQL + ComNum.VBLF + "         WHERE A.VALUE IN ('Remove', '마침','종료')                                                                                                                                ";
                        SQL = SQL + ComNum.VBLF + "    )                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "    , MMM AS                                                                                                                                                                      ";
                        SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "        SELECT                                                                                                                                                                    ";
                        SQL = SQL + ComNum.VBLF + "               A.PTNO                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "             , A.CHARTDATETIME AS SDATE                                                                                                                                           ";
                        SQL = SQL + ComNum.VBLF + "             , COALESCE((SELECT MIN(CHARTDATETIME) FROM R WHERE PTNO = A.PTNO AND CHARTDATETIME > A.CHARTDATETIME),TO_CHAR(SYSDATE, 'YYYYMMDD HH24MISS')) AS RDATE                ";
                        SQL = SQL + ComNum.VBLF + "             , A.GUBUN                                                                                                                                                            ";
                        SQL = SQL + ComNum.VBLF + "          FROM S A                                                                                                                                                                ";
                        SQL = SQL + ComNum.VBLF + "    )                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "    , MMMM AS                                                                                                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "        SELECT                                                                                                                                                                    ";
                        SQL = SQL + ComNum.VBLF + "               PTNO                                                                                                                                                               ";
                        SQL = SQL + ComNum.VBLF + "             , GUBUN                                                                                                                                                              ";
                        SQL = SQL + ComNum.VBLF + "             , (CASE                                                                                                                                                              ";
                        SQL = SQL + ComNum.VBLF + "                WHEN SUBSTR(SDATE,0,8) = '" + dtpSDATE.Value.ToString("yyyyMMdd") + "' AND SUBSTR(RDATE, 0, 8) = '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'                                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "                       THEN TO_NUMBER(SUBSTR(RDATE, 10, 2)) - TO_NUMBER(SUBSTR(SDATE, 10, 2))                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "                WHEN SUBSTR(SDATE,0,8) != '" + dtpSDATE.Value.ToString("yyyyMMdd") + "' AND SUBSTR(RDATE, 0, 8) = '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'                                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "                       THEN TO_NUMBER(SUBSTR(RDATE, 10, 2))                                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "                WHEN SUBSTR(SDATE,0,8) = '" + dtpSDATE.Value.ToString("yyyyMMdd") + "' AND SUBSTR(RDATE, 0, 8) != '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'                                                                                                ";
                        SQL = SQL + ComNum.VBLF + "                       THEN 24 - TO_NUMBER(SUBSTR(SDATE, 10, 2))                                                                                                                  ";
                        SQL = SQL + ComNum.VBLF + "                WHEN SUBSTR(SDATE,0,8) != '" + dtpSDATE.Value.ToString("yyyyMMdd") + "' AND SUBSTR(RDATE, 0, 8) != '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'                                                                                                ";
                        SQL = SQL + ComNum.VBLF + "                       THEN 24                                                                                                                                                    ";
                        SQL = SQL + ComNum.VBLF + "                END)                                                                                                                                                              ";
                        SQL = SQL + ComNum.VBLF + "             +(CASE--같은시간 보정                                                                                                                                                  ";
                        SQL = SQL + ComNum.VBLF + "                WHEN SUBSTR(SDATE,10,2) = SUBSTR(RDATE, 10, 2)                                                                                                                    ";
                        SQL = SQL + ComNum.VBLF + "                     THEN 1                                                                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "                     ELSE 0                                                                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "                END)                                                                                                                                                              ";
                        SQL = SQL + ComNum.VBLF + "             +(CASE--분이 다른 시간 보정                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "                WHEN SUBSTR(SDATE,12,2) >= SUBSTR(RDATE, 12, 2)                                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "                     THEN - 1                                                                                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "                     ELSE 0                                                                                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "                END) AS T                                                                                                                                                         ";
                        SQL = SQL + ComNum.VBLF + "          FROM MMM                                                                                                                                                                ";
                        SQL = SQL + ComNum.VBLF + "          WHERE '" + dtpSDATE.Value.ToString("yyyyMMdd") + "' BETWEEN SUBSTR(SDATE, 0, 8) AND SUBSTR(RDATE,0,8)                                                                                                          ";
                        SQL = SQL + ComNum.VBLF + "    )                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "    , MMMMM AS                                                                                                                                                                    ";
                        SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "        SELECT PTNO, GUBUN, SUM(T) AS T                                                                                                                                           ";
                        SQL = SQL + ComNum.VBLF + "          FROM MMMM                                                                                                                                                               ";
                        SQL = SQL + ComNum.VBLF + "         GROUP BY PTNO, GUBUN                                                                                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "    )                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "    , MMMMM2 AS                                                                                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "    SELECT PTNO                                                                                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "         , CASE                                                                                                                                                                  ";
                        SQL = SQL + ComNum.VBLF + "            WHEN GUBUN = 1 THEN RPAD('M0040', 8, ' ')-- 비내산소                                                                                                                                ";
                        SQL = SQL + ComNum.VBLF + "            WHEN GUBUN = 2 THEN RPAD('M0046A', 8, ' ')--(가온가습고유량비강캐뉼라요법) - 하이플로어                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "            WHEN GUBUN = 3 THEN(CASE                                                                                                                                              ";
                        SQL = SQL + ComNum.VBLF + "                                 WHEN T <= 3 THEN RPAD('M5850', 8, ' ')--인공호흡 3시간까지                                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "                                 WHEN T > 3 AND T <= 8 THEN RPAD('M5857', 8, ' ')--3시간 초과 8시간까지                                                                                            ";
                        SQL = SQL + ComNum.VBLF + "                                 WHEN T > 8 AND T <= 12 THEN RPAD('M5858', 8, ' ')--8시간 초과 12시간까지                                                                                          ";
                        SQL = SQL + ComNum.VBLF + "                                 WHEN T > 12 THEN RPAD('M5860', 8, ' ')--인공호흡 12시간초과 1일당                                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "                                 END)                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "           END AS ORDERCODE                                                                                                                                                          ";
                        SQL = SQL + ComNum.VBLF + "         , CASE                                                                                                                                                                  ";
                        SQL = SQL + ComNum.VBLF + "            WHEN GUBUN = 1 THEN RPAD('M0040', 8, ' ')-- 비내산소                                                                                                                                ";
                        SQL = SQL + ComNum.VBLF + "            WHEN GUBUN = 2 THEN RPAD('M0046', 8, ' ')--(가온가습고유량비강캐뉼라요법) - 하이플로어                                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "            WHEN GUBUN = 3 THEN(CASE                                                                                                                                              ";
                        SQL = SQL + ComNum.VBLF + "                                 WHEN T <= 3 THEN RPAD('M5850', 8, ' ')--인공호흡 3시간까지                                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "                                 WHEN T > 3 AND T <= 8 THEN RPAD('M5857', 8, ' ')--3시간 초과 8시간까지                                                                                            ";
                        SQL = SQL + ComNum.VBLF + "                                 WHEN T > 8 AND T <= 12 THEN RPAD('M5858', 8, ' ')--8시간 초과 12시간까지                                                                                          ";
                        SQL = SQL + ComNum.VBLF + "                                 WHEN T > 12 THEN RPAD('M5860', 8, ' ')--인공호흡 12시간초과 1일당                                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "                                 END)                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "           END AS SUCODE                                                                                                                                                          ";
                        SQL = SQL + ComNum.VBLF + "      FROM MMMMM A                                                                                                                                                                 ";
                        SQL = SQL + ComNum.VBLF + "    )                                                                                                                                                                             ";
                        SQL = SQL + ComNum.VBLF + "     SELECT  PTNO";
                        SQL = SQL + ComNum.VBLF + "         ,   ORDERCODE";
                        SQL = SQL + ComNum.VBLF + "         ,   SUCODE";
                        SQL = SQL + ComNum.VBLF + "       FROM MMMMM2 A";
                        SQL = SQL + ComNum.VBLF + "      WHERE NOT EXISTS";
                        SQL = SQL + ComNum.VBLF + "      (";
                        SQL = SQL + ComNum.VBLF + "          SELECT 1";
                        SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_OCS.OCS_IORDER";
                        SQL = SQL + ComNum.VBLF + "          WHERE SUCODE = A.SUCODE";
                        SQL = SQL + ComNum.VBLF + "            AND BDATE  = TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "            AND PTNO   = '" + pAcp.ptNo + "'";
                        SQL = SQL + ComNum.VBLF + "            AND GBSTATUS = ' '";
                        SQL = SQL + ComNum.VBLF + "      )";


                        DataTable dt2 = null;
                        SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr.NotEmpty())
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                #region '<오더등록>---------------------------------------------------------------------------------------
                                SQL = "SELECT KOSMOS_OCS.SEQ_ORDERNO.NextVal nNEXTVAL FROM DUAL";
                                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt.Rows.Count > 0)
                                {
                                    nOrderNo = (int)VB.Val(dt.Rows[0]["nNEXTVAL"].ToString().Trim());
                                }
                                dt.Dispose();
                                dt = null;

                                strORDERCODE = dt2.Rows[j]["ORDERCODE"].ToString().Trim();
                                strSucode = dt2.Rows[j]["SUCODE"].ToString().Trim();


                                SQL = "INSERT INTO KOSMOS_OCS.OCS_IORDER ( Ptno, BDate,     Seqno,     DeptCode,  ";
                                SQL = SQL + ComNum.VBLF + "  DrCode,  StaffID,    Slipno,    OrderCode, SuCode,  Bun,      ";
                                SQL = SQL + ComNum.VBLF + "  GbOrder, Contents,   BContents, RealQty,   Qty,     RealNal,  ";
                                SQL = SQL + ComNum.VBLF + "  Nal,     DosCode,    GbInfo,    GbSelf,    GbSpc,   GbNgt,    ";
                                SQL = SQL + ComNum.VBLF + "  GbER,    GbPRN,      GbDiv,     GbBoth,    GbAct,   GbTFlag,  ";
                                SQL = SQL + ComNum.VBLF + "  GbSend,  GbPosition, GbStatus,  NurseID,   EntDate, WardCode, ";
                                SQL = SQL + ComNum.VBLF + "  RoomCode, Bi,        OrderNo,   Remark, ";
                                SQL = SQL + ComNum.VBLF + "  ActDate, GbGroup,    GbPort,    OrderSite, GBPICKUP, PICKUPSABUN, ";
                                SQL = SQL + ComNum.VBLF + "  PICKUPDATE, SUBUL_WARD, HIGHRISK, ";
                                SQL = SQL + ComNum.VBLF + "  CORDERCODE, CSUCODE, CBUN ) VALUES     ";
                                SQL = SQL + ComNum.VBLF + "( '" + strPano + "',     TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),  ";
                                SQL = SQL + ComNum.VBLF + "    999 ,     '" + strDeptCode + "',    ";
                                SQL = SQL + ComNum.VBLF + "  '" + strSabun + "',   '" + pAcp.medDrCd + "',    'A5',   ";
                                SQL = SQL + ComNum.VBLF + "  '" + strORDERCODE + "','" + strSucode + "',     '28',      ";
                                SQL = SQL + ComNum.VBLF + "  '',  0,0,";
                                SQL = SQL + ComNum.VBLF + "  1,   1,1,   ";
                                SQL = SQL + ComNum.VBLF + "   1, '',    '',   ";
                                SQL = SQL + ComNum.VBLF + "  '',   '',      '',    ";
                                SQL = SQL + ComNum.VBLF + "  '',     ' ',   1 ,    ";
                                SQL = SQL + ComNum.VBLF + "  '0',   '',      '',  ";
                                SQL = SQL + ComNum.VBLF + "  '*',   '', ' ', ";
                                SQL = SQL + ComNum.VBLF + "  '" + clsType.User.IdNumber.PadLeft(5, '0') + "',  SysDate,   '" + cboWard.Text + "', ";
                                SQL = SQL + ComNum.VBLF + "  '" + pAcp.room + "', '" + pAcp.bi + "', " + nOrderNo + ", '', ";
                                SQL = SQL + ComNum.VBLF + "  TRUNC(SYSDATE),   '',  ";
                                SQL = SQL + ComNum.VBLF + "  '',   'TEL' , '*',  '" + clsType.User.IdNumber.PadLeft(5, '0') + "', SYSDATE,";
                                SQL = SQL + ComNum.VBLF + "  '', '', ";
                                SQL = SQL + ComNum.VBLF + "  '" + strORDERCODE + "','" + strSucode + "','28' ) ";

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBoxEx(this, "처방자료를 저장하는데 오류가 발생되었습니다");
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                #endregion
                            }

                        }

                        dt2.Dispose();
                        #endregion
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception EX)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(EX.Message, "", clsDB.DbCon);
            }

            READ_DATA();
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            using (Form frm = new frmH20View(dtpSDATE.Value.ToString("yyyyMMdd"), cboWard.Text.Trim(), SS1_Sheet1.Cells[e.Row, 2].Text.Trim()))
            {
                frm.Location = new Point(Cursor.Position.X + 200, e.Y + 200);
                frm.StartPosition = FormStartPosition.Manual;
                frm.ShowDialog(this);
            }
        }
    }
}
