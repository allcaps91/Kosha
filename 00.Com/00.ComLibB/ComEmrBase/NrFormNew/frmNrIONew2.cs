using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;

namespace ComEmrBase
{
    public partial class frmNrIONew2 : Form
    {
        EmrForm pForm = null;
        EmrPatient AcpEmr = null;

        #region 폼에 사용하는 변수를 코딩하는 부분
        /// <summary>
        /// //JOBGB Colume
        /// </summary>
        private const int COL_DATE = 0;
        /// <summary>
        /// //ITEM Colume
        /// </summary>
        private const int COL_ITEM = 1;
        /// <summary>
        /// //그룹 이름 Colume
        /// </summary>
        private const int COL_GNAME = 2;
        /// <summary>
        /// //아이템 이름 Colume
        /// </summary>
        private const int COL_INAME = 3;

        public string mstrFormNameGb = "기록지관리";
        public string mstrFormNameWard = "임상관찰";

        private Font BoldFont = null;

        /// <summary>
        /// 날짜 + 아이템코드 매핑
        /// </summary>
        Dictionary<string, int> keyDateItem = null;
        /// <summary>
        /// Duty 매핑
        /// </summary>
        Dictionary<string, int> keyDutyItem = null;
        /// <summary>
        /// 하루 아이템 총합
        /// </summary>
        Dictionary<string, int> keyTotItem = null;
        #endregion

        public frmNrIONew2(EmrPatient p)
        {
            AcpEmr = p;
            InitializeComponent();
        }
         
        private void frmNrIONew_Load(object sender, EventArgs e)
        {
            if(AcpEmr == null)
            {
                Close();
            }

            chkAsc.Checked = clsEmrPublic.bOrderAsc;

            BoldFont = new Font("굴림", 10, FontStyle.Bold);
            ssIoTot_Sheet1.FrozenRowCount = 1;

            Height = Screen.FromControl(this).WorkingArea.Height - 10;

            Text = string.Format("등록번호 :{0} 이름 :{1}", AcpEmr.ptNo, AcpEmr.ptName);

            ssIoTot_Sheet1.Columns[1].Visible = false;

            keyDateItem = new Dictionary<string, int>();
            keyDutyItem = new Dictionary<string, int>();
            keyTotItem  = new Dictionary<string, int>();

            pForm = clsEmrChart.ClearEmrForm();
            pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "3150");

            dtpFrDateTot2.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            dtpFrDateTot.Value = dtpFrDateTot2.Value.AddDays(-3);

            btnPrint.Visible = clsType.User.AuAPRINTIN.Equals("1");

            GetIoTot();

        }

        private void btnSearchTot_Click(object sender, EventArgs e)
        {
            GetIoTot();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region IO합계함수
        private void GetIoTot()
        {
            SetIoTotDefault();

            LoadIoTot();
        }

        /// <summary>
        /// IO 합계를 구한다
        /// </summary>
        private void LoadIoTot()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            keyDutyItem.Clear();
            keyDutyItem.Add("DAY", ssIoTot_Sheet1.ColumnCount - 4);
            keyDutyItem.Add("EVE", ssIoTot_Sheet1.ColumnCount - 3);
            keyDutyItem.Add("NIG", ssIoTot_Sheet1.ColumnCount - 2);
            keyDutyItem.Add("TOT", ssIoTot_Sheet1.ColumnCount - 1);


            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + "WITH M AS";
            SQL = SQL + ComNum.VBLF + "(";
            SQL = SQL + ComNum.VBLF + "     SELECT B.ITEMCD, B.ITEMVALUE, BC.BASNAME";
            SQL = SQL + ComNum.VBLF + "            ,CASE WHEN TRIM(SUBSTR(A.CHARTTIME, 0, 4)) >= '0501' AND TRIM(SUBSTR(A.CHARTTIME, 0, 4)) <= '1300' THEN 'DAY'";
            SQL = SQL + ComNum.VBLF + "                  WHEN TRIM(SUBSTR(A.CHARTTIME, 0, 4)) >= '1301' AND TRIM(SUBSTR(A.CHARTTIME, 0, 4)) <= '2100' THEN 'EVE'";
            SQL = SQL + ComNum.VBLF + "                  WHEN TRIM(SUBSTR(A.CHARTTIME, 0, 4)) >= '2101' AND TRIM(SUBSTR(A.CHARTTIME, 0, 4)) <= '2400' THEN 'NIG'";
            SQL = SQL + ComNum.VBLF + "                  WHEN TRIM(SUBSTR(A.CHARTTIME, 0, 4)) >= '0000' AND TRIM(SUBSTR(A.CHARTTIME, 0, 4)) <= '0500' THEN 'NIG'";
            SQL = SQL + ComNum.VBLF + "             END DUTY";
            SQL = SQL + ComNum.VBLF + "            ,CASE WHEN TRIM(SUBSTR(A.CHARTTIME, 0, 4)) = '2400' OR TRIM(SUBSTR(A.CHARTTIME, 0, 4)) <= '0500' THEN";
            SQL = SQL + ComNum.VBLF + "                  TO_DATE(A.CHARTDATE, 'YYYYMMDD') - 1";
            SQL = SQL + ComNum.VBLF + "             ELSE TO_DATE(A.CHARTDATE, 'YYYYMMDD') ";
            SQL = SQL + ComNum.VBLF + "             END CDATE";
            SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_EMR + "AEMRCHARTMST A  ";
            SQL = SQL + ComNum.VBLF + "     INNER JOIN  " + ComNum.DB_EMR + "AEMRBVITALTIME TM ";
            SQL = SQL + ComNum.VBLF + "        ON  A.ACPNO = TM.ACPNO ";
            SQL = SQL + ComNum.VBLF + "       AND TM.FORMNO = " + pForm.FmFORMNO;
            SQL = SQL + ComNum.VBLF + "       AND TM.SUBGB = '0' ";
            SQL = SQL + ComNum.VBLF + "       AND (A.CHARTDATE || A.CHARTTIME) = (NVL(TM.CHARTDATE, '00000000') || NVL(TM.TIMEVALUE, '0000') || '00') ";
            SQL = SQL + ComNum.VBLF + "     INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B ";
            SQL = SQL + ComNum.VBLF + "        ON  B.EMRNO = A.EMRNO ";
            SQL = SQL + ComNum.VBLF + "       AND  B.EMRNOHIS = A.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + @"       AND REGEXP_LIKE(B.ITEMVALUE,'^-?\d*\.?\d+([eE]-?\d+)?$')"; //소수점까지 체크가능
            SQL = SQL + ComNum.VBLF + "     INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BC  ";
            SQL = SQL + ComNum.VBLF + "        ON BC.BASCD = B.ITEMCD  ";
            SQL = SQL + ComNum.VBLF + "       AND BC.BSNSCLS = '" + mstrFormNameGb + "'  ";
            SQL = SQL + ComNum.VBLF + "       AND BC.UNITCLS = '섭취배설' ";
            SQL = SQL + ComNum.VBLF + "       AND BC.VFLAG3 IN ('01.섭취', '09.섭취', '11.배설', '19.배설', '51.기타')";
            //SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO       = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "     WHERE A.ACPNO      = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "       AND A.FORMNO     = " + pForm.FmFORMNO;
            SQL = SQL + ComNum.VBLF + "       AND A.UPDATENO   = " + pForm.FmUPDATENO;
            SQL = SQL + ComNum.VBLF + "       AND A.CHARTDATE >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "       AND A.CHARTDATE <= '" + dtpFrDateTot2.Value.AddDays(1).ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "       AND A.CHARTUSEID <> '합계' ";
            SQL = SQL + ComNum.VBLF + "       AND B.ITEMCD NOT IN ('I0000030622', 'I0000030623', 'I0000022324')";

            #region 혈액(수혈) 기록지 데이터 가져오기
            SQL = SQL + ComNum.VBLF + "    UNION ALL --수혈 기록지에서 값 가저오기                                                                                                                                                                            ";
            SQL = SQL + ComNum.VBLF + "    SELECT ITEMCD, ITEMVALUE,  '혈액'";
            SQL = SQL + ComNum.VBLF + "            ,CASE WHEN TRIM(SUBSTR(A.CHARTTIME, 0, 4)) >= '0501' AND TRIM(SUBSTR(A.CHARTTIME, 0, 4)) <= '1300' THEN 'DAY'";
            SQL = SQL + ComNum.VBLF + "                  WHEN TRIM(SUBSTR(A.CHARTTIME, 0, 4)) >= '1301' AND TRIM(SUBSTR(A.CHARTTIME, 0, 4)) <= '2100' THEN 'EVE'";
            SQL = SQL + ComNum.VBLF + "                  WHEN TRIM(SUBSTR(A.CHARTTIME, 0, 4)) >= '2101' AND TRIM(SUBSTR(A.CHARTTIME, 0, 4)) <= '2400' THEN 'NIG'";
            SQL = SQL + ComNum.VBLF + "                  WHEN TRIM(SUBSTR(A.CHARTTIME, 0, 4)) >= '0000' AND TRIM(SUBSTR(A.CHARTTIME, 0, 4)) <= '0500' THEN 'NIG'";
            SQL = SQL + ComNum.VBLF + "             END DUTY";
            SQL = SQL + ComNum.VBLF + "            ,CASE WHEN TRIM(SUBSTR(A.CHARTTIME, 0, 4)) = '2400' OR TRIM(SUBSTR(A.CHARTTIME, 0, 4)) <= '0500' THEN";
            SQL = SQL + ComNum.VBLF + "                  TO_DATE(A.CHARTDATE, 'YYYYMMDD') - 1";
            SQL = SQL + ComNum.VBLF + "             ELSE TO_DATE(A.CHARTDATE, 'YYYYMMDD') ";
            SQL = SQL + ComNum.VBLF + "             END CDATE";
            SQL = SQL + ComNum.VBLF + "    FROM                                                                                                                                                                                                        ";
            SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                                                           ";
            SQL = SQL + ComNum.VBLF + "        SELECT (REPLACE(REPLACE((SELECT ITEMVALUE                                                                                                                                                               ";
            SQL = SQL + ComNum.VBLF + "                                  FROM ADMIN.AEMRCHARTROW                                                                                                                                                  ";
            SQL = SQL + ComNum.VBLF + "                                 WHERE EMRNO = A.EMRNO                                                                                                                                                          ";
            SQL = SQL + ComNum.VBLF + "                                   AND EMRNOHIS = A.EMRNOHIS                                                        ";
            SQL = SQL + ComNum.VBLF + "                                   AND ITEMCD = 'I0000037490'),'-',''),'/','')) AS CHARTDATE--수혈종료일자                                                                                                        ";
            SQL = SQL + ComNum.VBLF + "             , REPLACE((SELECT ITEMVALUE                                                                                                                                                                        ";
            SQL = SQL + ComNum.VBLF + "                          FROM ADMIN.AEMRCHARTROW                                                                                                                                                          ";
            SQL = SQL + ComNum.VBLF + "                         WHERE EMRNO = A.EMRNO                                                                                                                                                                  ";
            SQL = SQL + ComNum.VBLF + "                           AND EMRNOHIS = A.EMRNOHIS                                                        ";
            SQL = SQL + ComNum.VBLF + "                           AND ITEMCD = 'I0000037491'),':','')AS CHARTTIME  --수혈종료시간                                                                                                                        ";
            SQL = SQL + ComNum.VBLF + "             , 'I0000022324' AS ITEMCD                                                                                                                                                                          ";
            SQL = SQL + ComNum.VBLF + "             , 'I0000022324' AS ITEMNO                                                                                                                                                                          ";
            SQL = SQL + ComNum.VBLF + "             , B.ITEMVALUE                                                                                                                                                                                      ";
            SQL = SQL + ComNum.VBLF + "          FROM ADMIN.AEMRCHARTMST A                                                                                                                                                                        ";
            SQL = SQL + ComNum.VBLF + "         INNER JOIN  ADMIN.AEMRCHARTROW B                                                                                                                                                                  ";
            SQL = SQL + ComNum.VBLF + "            ON A.EMRNO = B.EMRNO                                                                                                                                                                                ";
            SQL = SQL + ComNum.VBLF + "           AND A.EMRNOHIS = B.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "         WHERE A.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "           AND A.FORMNO IN (1965, 3535)                                                                                                                                                                      ";
            SQL = SQL + ComNum.VBLF + "           AND B.ITEMCD = 'I0000013528'                                                                                                                                                                         ";
            SQL = SQL + ComNum.VBLF + "           AND A.MEDFRDATE = '" + AcpEmr.medFrDate + "'                                                                                                                                                                         ";
            SQL = SQL + ComNum.VBLF + "    )  A                                                                                                                                                                                                         ";
            SQL = SQL + ComNum.VBLF + "      WHERE A.CHARTDATE >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "        AND A.CHARTDATE <= '" + dtpFrDateTot2.Value.AddDays(1).ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + " )";
            #endregion

            SQL = SQL + ComNum.VBLF + " SELECT   ";
            SQL = SQL + ComNum.VBLF + "   CDATE  ";
            SQL = SQL + ComNum.VBLF + " , CASE WHEN DUTY IS NULL THEN 'TOT' ELSE DUTY END DUTY  ";
            SQL = SQL + ComNum.VBLF + " , NVL(ITEMCD, '') ITEMCD   ";
            SQL = SQL + ComNum.VBLF + " , SUM(DECODE(ITEMVALUE, NULL, 0, TO_NUMBER(ITEMVALUE))) AS TOTAL  ";
            SQL = SQL + ComNum.VBLF + " , CASE WHEN GROUPING_ID(CDATE, DUTY, ITEMCD) = 1 THEN '소계' ELSE MAX(BASNAME) END ITEMNAME ";
            SQL = SQL + ComNum.VBLF + " FROM M";
            SQL = SQL + ComNum.VBLF + " GROUP BY CDATE, CUBE(DUTY, ITEMCD)";

            SQL = SQL + ComNum.VBLF + "UNION ALL --섭취계산                                                                                                                                                                                                 ";
            SQL = SQL + ComNum.VBLF + " SELECT   ";
            SQL = SQL + ComNum.VBLF + "       CDATE  ";
            SQL = SQL + ComNum.VBLF + "     , CASE WHEN DUTY IS NULL THEN 'TOT' ELSE DUTY END DUTY  ";
            SQL = SQL + ComNum.VBLF + "     , 'I0000030622' AS ITEMCD                                                                                                                                                                                  ";
            SQL = SQL + ComNum.VBLF + "     , SUM(A.ITEMVALUE) AS TOTAL                                                                                                                                                                   ";
            SQL = SQL + ComNum.VBLF + "     , '총섭취량' AS ITEMNAME                                                                                                                                                                                            ";
            SQL = SQL + ComNum.VBLF + "  FROM M A                                                                                                                                                                                                      ";
            SQL = SQL + ComNum.VBLF + " INNER JOIN ADMIN.AEMRBASCD B                                                                                                                                                                              ";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD                                                                                                                                                                                       ";
            SQL = SQL + ComNum.VBLF + "   --AND A.CHARTDATE >= B.APLFRDATE                                                                                                                                                                               ";
            SQL = SQL + ComNum.VBLF + "   --AND A.CHARTDATE < B.APLENDDATE                                                                                                                                                                               ";
            SQL = SQL + ComNum.VBLF + " WHERE B.BSNSCLS = '기록지관리'                                                                                                                                                                                   ";
            SQL = SQL + ComNum.VBLF + "   AND B.UNITCLS = '섭취배설'                                                                                                                                                                                    ";
            SQL = SQL + ComNum.VBLF + "   AND B.VFLAG3 = '01.섭취'                                                                                                                                                                                      ";
            SQL = SQL + ComNum.VBLF + "   AND A.ITEMVALUE IS NOT NULL                                                                                                                                                                                  ";
            SQL = SQL + ComNum.VBLF + " GROUP BY CDATE, CUBE((B.VFLAG3, DUTY))                                                                                                                                                          ";
            SQL = SQL + ComNum.VBLF + "--GROUP BY A.EMRNO, A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME                                                                                                                                                          ";
            SQL = SQL + ComNum.VBLF + " UNION ALL -- 배설 계산                                                                                                                                                                                              ";
            SQL = SQL + ComNum.VBLF + " SELECT  ";
            SQL = SQL + ComNum.VBLF + "       CDATE  ";
            SQL = SQL + ComNum.VBLF + "     , CASE WHEN DUTY IS NULL THEN 'TOT' ELSE DUTY END DUTY  ";
            SQL = SQL + ComNum.VBLF + "     ,  'I0000030623' AS ITEMCD                                                                                                                                                                                  ";
            SQL = SQL + ComNum.VBLF + "     , SUM(A.ITEMVALUE) AS TOTAL                                                                                                                                                                   ";
            SQL = SQL + ComNum.VBLF + "     , '총배설량' AS ITEMNAME                                                                                                                                                                                            ";
            SQL = SQL + ComNum.VBLF + "  FROM M A                                                                                                                                                                                                      ";
            SQL = SQL + ComNum.VBLF + " INNER JOIN ADMIN.AEMRBASCD B                                                                                                                                                                              ";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD                                                                                                                                                                                       ";
            SQL = SQL + ComNum.VBLF + "  -- AND A.CHARTDATE >= B.APLFRDATE                                                                                                                                                                               ";
            SQL = SQL + ComNum.VBLF + "  -- AND A.CHARTDATE < B.APLENDDATE                                                                                                                                                                               ";
            SQL = SQL + ComNum.VBLF + " WHERE B.BSNSCLS = '기록지관리'                                                                                                                                                                                   ";
            SQL = SQL + ComNum.VBLF + "   AND B.UNITCLS = '섭취배설'                                                                                                                                                                                    ";
            SQL = SQL + ComNum.VBLF + "   AND B.VFLAG3 = '11.배설'                                                                                                                                                                                      ";
            SQL = SQL + ComNum.VBLF + "   AND A.ITEMVALUE IS NOT NULL                                                                                                                                                                                  ";
            SQL = SQL + ComNum.VBLF + " GROUP BY CDATE, CUBE((B.VFLAG3, DUTY))                                                                                                                                                          ";
            SQL = SQL + ComNum.VBLF + "--GROUP BY A.EMRNO, A.EMRNOHIS, A.CHARTDATE, A.CHARTTIME                                                                                                                                                          ";
            SQL = SQL + ComNum.VBLF + "--ORDER BY CHARTDATE ASC , CHARTTIME ASC , EMRNO, UNITCLS, BASVAL    ";


            if (chkAsc.Checked)
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY CDATE, DUTY";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY CDATE DESC, DUTY";
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

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strITEMCD = dt.Rows[i]["ITEMCD"].ToString().Trim();
                string strDuty = dt.Rows[i]["DUTY"].ToString().Trim();
                DateTime CDATE = dt.Rows[i]["CDATE"].To<DateTime>();
                int Row;
                int Col;

                if (keyDateItem.TryGetValue(CDATE.ToString("yyyyMMdd") + strITEMCD, out Row) && keyDutyItem.TryGetValue(strDuty, out Col))
                {
                    if (dt.Rows[i]["ITEMCD"].ToString().Trim().Equals("I0000030622") ||
                        dt.Rows[i]["ITEMCD"].ToString().Trim().Equals("I0000030623"))
                    {
                        ssIoTot_Sheet1.Cells[Row, 1, Row, ssIoTot_Sheet1.ColumnCount - 1].BackColor = Color.LightCyan;
                        ssIoTot_Sheet1.Cells[Row, 2].Font = BoldFont;
                        ssIoTot_Sheet1.Cells[Row, ssIoTot_Sheet1.ColumnCount - 1].Font = BoldFont;
                    }
                    ssIoTot_Sheet1.Cells[Row, Col].Text = dt.Rows[i]["TOTAL"].ToString().Trim();
                }

                if (i >= dt.Rows.Count) break;
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// IO 함계 스프래드 세팅
        /// </summary>
        private void SetIoTotDefault()
        {
            ssIoTot_Sheet1.RowCount = 1;
            keyDateItem.Clear();
            keyTotItem.Clear();
            //일자별 등록된 것이 있는지 파악해서 있으면 세팅을 하고
            //없으면 기본을 가지고 세팅을 한다.
            int i = 0;
            DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            string strBASEXNAME = string.Empty;
            string strCHARTDATE = string.Empty;

            int intS = 0;
            int intS2 = 0;

            Cursor.Current = Cursors.WaitCursor;
            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO, A.CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "   ON B.BASCD = A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "  AND B.BSNSCLS = '" + mstrFormNameGb + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.UNITCLS = '섭취배설'";
            SQL = SQL + ComNum.VBLF + "  AND B.VFLAG3 IN ('01.섭취', '09.섭취', '11.배설', '19.배설', '51.기타')";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "   ON B.VFLAG1 = BB.BASCD";
            SQL = SQL + ComNum.VBLF + "  AND BB.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "  AND BB.UNITCLS = '섭취배설그룹'";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO IN(1969, " + pForm.FmFORMNO + ")";
            //SQL = SQL + ComNum.VBLF + "  AND A.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.ACPNO     = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE >= '" + dtpFrDateTot.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE <= '" + dtpFrDateTot2.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.JOBGB = 'IIO'";

            if (chkAsc.Checked)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE, BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE DESC, BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";
            }

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


                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (keyDateItem.ContainsKey(dt.Rows[i]["CHARTDATE"].ToString().Trim() + dt.Rows[i]["BASCD"].ToString().Trim()))
                        continue;
                    
                    ssIoTot_Sheet1.RowCount += 1;
                    ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_DATE].Text = DateTime.ParseExact(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "yyyyMMdd", null).ToString("yyyy-MM-dd");
                    ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_ITEM].Text = dt.Rows[i]["BASCD"].ToString().Trim();

                    string strDate = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                    string strItem = ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_ITEM].Text;

                    keyDateItem.Add(strDate + strItem, ssIoTot_Sheet1.RowCount - 1);

                    if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                    {
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_GNAME].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                        if (i != 0)
                        {
                            ssIoTot_Sheet1.AddSpanCell(intS, COL_GNAME, ssIoTot_Sheet1.RowCount - 1 - intS, 1);
                        }
                        intS = ssIoTot_Sheet1.RowCount - 1;
                    }

                    if (strCHARTDATE != dt.Rows[i]["CHARTDATE"].ToString().Trim())
                    {
                        if (i != 0)
                        {
                            ssIoTot_Sheet1.AddSpanCell(intS2, COL_DATE, ssIoTot_Sheet1.RowCount - 1 - intS2, 1);
                        }
                        intS2 = ssIoTot_Sheet1.RowCount - 1;
                    }

                    strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    strCHARTDATE = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                    ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_INAME].Text = dt.Rows[i]["BASNAME"].ToString().Trim();

                    if ((dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030622") || (dt.Rows[i]["BASCD"].ToString().Trim() == "I0000030623"))
                    {
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_GNAME].BackColor = Color.LightCyan;
                        ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, COL_INAME].BackColor = Color.LightCyan;
                        //ssIoTot_Sheet1.Cells[ssIoTot_Sheet1.RowCount - 1, 0, ssIoTot_Sheet1.RowCount - 1, ssIoTot_Sheet1.ColumnCount - 1].Font = BoldFont;
                    }

                    ssIoTot_Sheet1.SetRowHeight(ssIoTot_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                }
                dt.Dispose();
                dt = null;

                ssIoTot_Sheet1.AddSpanCell(intS, 3, ssIoTot_Sheet1.RowCount - intS, 1);
                ssIoTot_Sheet1.AddSpanCell(intS2, 0, ssIoTot_Sheet1.RowCount - intS2, 1);
            }
            else
            {
                dt.Dispose();
                dt = null;
            }

        }
        #endregion IO 함수

        private void btnPrint_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return;

            //Print Head 지정
            string strFont1 = "/fn\"굴림체\"/fz\"14\"/fb1/fi0/fu0/fk0/fs1";
            string strFont2 = "/fn\"굴림체\"/fz\"10\"/fb0/fi0/fu0/fk0/fs2";
            string strHead1 = string.Empty;
            string strHead2 = string.Empty;

            strHead1 = "/c/f1 임상관찰 I/O" + "/f1/n/n";

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
            strHead2 = strHead2 + "/l/f2" + "     출력자(출력일자) : " + clsType.User.UserName + "(" + ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10) + ")" + "/f2/n/n";

            ssIoTot.ActiveSheet.PrintInfo.AbortMessage = "현재 출력중입니다..";
            ssIoTot.ActiveSheet.PrintInfo.HeaderHeight = 120;
            ssIoTot.ActiveSheet.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;

            ssIoTot.ActiveSheet.PrintInfo.Margin.Top = 20;
            ssIoTot.ActiveSheet.PrintInfo.Orientation = PrintOrientation.Landscape;
            ssIoTot.ActiveSheet.PrintInfo.Centering = Centering.Horizontal;
            ssIoTot.ActiveSheet.PrintInfo.ShowColumnHeader = PrintHeader.Hide;
            ssIoTot.ActiveSheet.PrintInfo.ShowRowHeader = PrintHeader.Hide;
            ssIoTot.ActiveSheet.PrintInfo.ShowBorder = true;
            ssIoTot.ActiveSheet.PrintInfo.ShowColor = true;
            ssIoTot.ActiveSheet.PrintInfo.ShowGrid = true;
            ssIoTot.ActiveSheet.PrintInfo.ShowShadows = true;
            ssIoTot.ActiveSheet.PrintInfo.UseMax = false;
            ssIoTot.ActiveSheet.PrintInfo.PrintType = PrintType.All;
            ssIoTot.PrintSheet(0);
        }

        private void chkAsc_CheckedChanged(object sender, EventArgs e)
        {
            clsEmrPublic.bOrderAsc = chkAsc.Checked;
        }
    }
}
