using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrNrCatheterView : Form
    {
        EmrPatient p = null;

        public frmEmrNrCatheterView(EmrPatient patient)
        {
            p = patient;
            InitializeComponent();
        }

        private void frmEmrNrCatheterView_Load(object sender, EventArgs e)
        {
            GetData(ssChart_Sheet1);
        }

        /// <summary>
        /// 데이타를 조회한다
        /// </summary>
        /// <param name="Spd"></param>
        private void GetData(FarPoint.Win.Spread.SheetView Spd)
        {
            if (p == null) return;

            int i = 0;
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            string curDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            Spd.RowCount = 0;

            #region 도뇨관 리스트
            SQL.AppendLine("WITH ITEM_LIST AS");
            SQL.AppendLine("(");
            SQL.AppendLine("SELECT    A.CHARTDATE");
            SQL.AppendLine("        , R.ITEMNO");
            SQL.AppendLine("        , R.ITEMVALUE");
            SQL.AppendLine("	        FROM ADMIN.AEMRCHARTMST A ");
            SQL.AppendLine("	   	   INNER JOIN ADMIN.AEMRCHARTROW R ");
            SQL.AppendLine("	   	      ON A.EMRNO = R.EMRNO ");
            SQL.AppendLine("	   		 AND A.EMRNOHIS = R.EMRNOHIS ");
            SQL.AppendLine("	   		 AND EXISTS");
            SQL.AppendLine("	   		 (");
            SQL.AppendLine("	   		    SELECT 1");
            SQL.AppendLine("                  FROM ADMIN.AEMRBASCD");
            SQL.AppendLine("                 WHERE BSNSCLS = '기록지관리'");
            SQL.AppendLine("                   AND UNITCLS = '도뇨관관리'");
            SQL.AppendLine("                   AND BASCD = R.ITEMNO");
            SQL.AppendLine("	   		 )");
            SQL.AppendLine("	       WHERE A.MEDFRDATE = '" + p.medFrDate + "'");
            SQL.AppendLine("	   	     AND A.PTNO = '" + p.ptNo + "'");
            SQL.AppendLine("	   	     AND A.FORMNO IN (1575, 3150)");
            SQL.AppendLine(")");
            #endregion

            #region 인공기도 리스트
            SQL.AppendLine(", AA_LIST AS");
            SQL.AppendLine("(");
            SQL.AppendLine("SELECT  ");
            SQL.AppendLine("          R.ITEMVALUE AS KINDS");
            SQL.AppendLine("        , MAX(REGEXP_REPLACE(R2.ITEMVALUE, '[^0-9]+')) AS INSERTDAY");
            SQL.AppendLine("        , MAX(TO_NUMBER(REGEXP_REPLACE(R3.ITEMVALUE, '[^0-9]+'))) AS USEDAY");
            SQL.AppendLine("  FROM ADMIN.AEMRCHARTMST A ");
            SQL.AppendLine("    INNER JOIN ADMIN.AEMRCHARTROW R              ");
            SQL.AppendLine("       ON A.EMRNO = R.EMRNO                  ");
            SQL.AppendLine("   	  AND A.EMRNOHIS = R.EMRNOHIS            ");
            SQL.AppendLine("   	  AND R.ITEMNO = 'I0000037878' --종류     ");
            SQL.AppendLine("   	  AND R.ITEMVALUE > CHR(0)               ");
            SQL.AppendLine("    INNER JOIN ADMIN.AEMRCHARTROW R2    ");
            SQL.AppendLine("       ON A.EMRNO = R2.EMRNO                 ");
            SQL.AppendLine("   	  AND A.EMRNOHIS = R2.EMRNOHIS           ");
            SQL.AppendLine("   	  AND R2.ITEMNO = 'I0000037660' --삽입일   ");
            SQL.AppendLine("    INNER JOIN ADMIN.AEMRCHARTROW R3    ");
            SQL.AppendLine("       ON A.EMRNO = R3.EMRNO                 ");
            SQL.AppendLine("   	  AND A.EMRNOHIS = R3.EMRNOHIS           ");
            SQL.AppendLine("   	  AND R3.ITEMNO = 'I0000037743' --유지일   ");
            SQL.AppendLine(" WHERE A.MEDFRDATE = '" + p.medFrDate + "'");
            SQL.AppendLine("   AND A.PTNO = '" + p.ptNo + "'");
            SQL.AppendLine("   AND A.FORMNO = 2639");
            SQL.AppendLine(" GROUP BY R.ITEMVALUE                     ");

            SQL.AppendLine(")");
            #endregion

            #region 중심정맥관 리스트
            SQL.AppendLine(", CVC_LIST AS");
            SQL.AppendLine("(");
            SQL.AppendLine("SELECT  ");
            SQL.AppendLine("          R.ITEMVALUE AS KINDS");
            SQL.AppendLine("        , MAX(R2.ITEMVALUE) AS INSERTDAY");
            //SQL.AppendLine("        , MAX(REGEXP_REPLACE(R2.ITEMVALUE, '[^0-9]+')) AS INSERTDAY");
            SQL.AppendLine("        , MAX(TO_NUMBER(REGEXP_REPLACE(R3.ITEMVALUE, '[^0-9]+'))) AS USEDAY");
            SQL.AppendLine("  FROM ADMIN.AEMRCHARTMST A ");
            SQL.AppendLine("    INNER JOIN ADMIN.AEMRCHARTROW R              ");
            SQL.AppendLine("       ON A.EMRNO = R.EMRNO                  ");
            SQL.AppendLine("   	  AND A.EMRNOHIS = R.EMRNOHIS            ");
            SQL.AppendLine("   	  AND R.ITEMNO = 'I0000037611' --종류     ");
            SQL.AppendLine("   	  AND R.ITEMVALUE > CHR(0)               ");
            SQL.AppendLine("    INNER JOIN ADMIN.AEMRCHARTROW R2    ");
            SQL.AppendLine("       ON A.EMRNO = R2.EMRNO                 ");
            SQL.AppendLine("   	  AND A.EMRNOHIS = R2.EMRNOHIS           ");
            SQL.AppendLine("   	  AND R2.ITEMNO = 'I0000016403' --삽입일   ");
            SQL.AppendLine("    INNER JOIN ADMIN.AEMRCHARTROW R3    ");
            SQL.AppendLine("       ON A.EMRNO = R3.EMRNO                 ");
            SQL.AppendLine("   	  AND A.EMRNOHIS = R3.EMRNOHIS           ");
            SQL.AppendLine("   	  AND R3.ITEMNO = 'I0000037605' --유지일   ");
            SQL.AppendLine(" WHERE A.MEDFRDATE = '" + p.medFrDate + "'");
            SQL.AppendLine("   AND A.PTNO = '" + p.ptNo + "'");
            SQL.AppendLine("   AND A.FORMNO = 2227");
            SQL.AppendLine(" GROUP BY R.ITEMVALUE                     ");
            SQL.AppendLine(")");
            #endregion

            #region 유치도뇨관 제거, 유지, 등등
            SQL.AppendLine("SELECT   ");
            SQL.AppendLine("         BASNAME AS ITEMNAME");
            SQL.AppendLine("       , '' AS KINDS");
            SQL.AppendLine("       , DISSEQNO");
            SQL.AppendLine("	   , ( SELECT ");
            SQL.AppendLine("	      	        MAX(CHARTDATE)");
            SQL.AppendLine("	         FROM ITEM_LIST R");
            SQL.AppendLine("	      	 WHERE I.BASCD = R.ITEMNO");
            SQL.AppendLine("	      	   AND (UPPER(R.ITEMVALUE) LIKE '%START%' OR R.ITEMVALUE LIKE '%삽입%' OR  R.ITEMVALUE LIKE '%교체%' OR R.ITEMVALUE LIKE '%교환%' OR R.ITEMVALUE LIKE '%at%')");
            SQL.AppendLine("	     ) AS INSERTVAL");

            #region 유지일 - 제거일이 있다면(삽입일이 제거일과 같거나 같을경우),  없다면(현재날짜 - 마지막 삽입일)
            SQL.AppendLine("	   , CASE WHEN");
            SQL.AppendLine("	        (SELECT MAX(CHARTDATE)");
            SQL.AppendLine("	           FROM ITEM_LIST R ");
            SQL.AppendLine("	          WHERE I.BASCD = R.ITEMNO ");
            SQL.AppendLine("	            AND (R.ITEMVALUE LIKE '%제거%' OR UPPER(R.ITEMVALUE) LIKE '%REMOVE%')");
            SQL.AppendLine("	        ) IS NULL THEN ");
            SQL.AppendLine("	            (  SELECT ");
            SQL.AppendLine("	             	        CASE WHEN MAX(CHARTDATE) IS NOT NULL THEN (TRUNC(SYSDATE + 1) - TO_DATE(MAX(CHARTDATE), 'YYYY-MM-DD')) END A");
            SQL.AppendLine("	                 FROM ITEM_LIST R");
            SQL.AppendLine("	             	  WHERE I.BASCD = R.ITEMNO");
            SQL.AppendLine("	             		AND (UPPER(R.ITEMVALUE) LIKE '%START%' OR R.ITEMVALUE LIKE '%삽입%' OR R.ITEMVALUE LIKE '%교체%' OR R.ITEMVALUE LIKE '%교환%' OR R.ITEMVALUE LIKE '%at%')");
            SQL.AppendLine("	            )");
            SQL.AppendLine("	     ELSE ");
            SQL.AppendLine("	            (  SELECT ");
            SQL.AppendLine("	             	        CASE WHEN MAX(CHARTDATE) IS NOT NULL THEN (TRUNC(SYSDATE + 1) - TO_DATE(MAX(CHARTDATE), 'YYYY-MM-DD')) END A");
            SQL.AppendLine("	                 FROM ITEM_LIST R");
            SQL.AppendLine("	             	  WHERE I.BASCD = R.ITEMNO");
            SQL.AppendLine("	             		AND R.CHARTDATE >= ");
            SQL.AppendLine("	             		(");
            SQL.AppendLine("	                         SELECT MAX(CHARTDATE)");
            SQL.AppendLine("	                           FROM ITEM_LIST R ");
            SQL.AppendLine("	                          WHERE I.BASCD = R.ITEMNO ");
            SQL.AppendLine("	                            AND (R.ITEMVALUE LIKE '%제거%' OR UPPER(R.ITEMVALUE) LIKE '%REMOVE%')");
            SQL.AppendLine("	             		)");
            SQL.AppendLine("	             		AND (UPPER(R.ITEMVALUE) LIKE '%START%' OR R.ITEMVALUE LIKE '%삽입%' OR R.ITEMVALUE LIKE '%교체%' OR R.ITEMVALUE LIKE '%교환%' OR R.ITEMVALUE LIKE '%at%')");
            SQL.AppendLine("	            )");
            SQL.AppendLine("	     END USEVAL");
            #endregion

            #region 가장 최근 제거일
            SQL.AppendLine("	   , (  SELECT ");
            SQL.AppendLine("	   	        MAX(CHARTDATE)");
            SQL.AppendLine("	       FROM ITEM_LIST R");
            SQL.AppendLine("	   	  WHERE I.BASCD = R.ITEMNO");
            SQL.AppendLine("	   		AND (R.ITEMVALUE LIKE '%제거%' OR UPPER(R.ITEMVALUE) LIKE '%REMOVE%')");
            SQL.AppendLine("	     ) AS DELVAL");
            #endregion

            SQL.AppendLine("     FROM ADMIN.AEMRBASCD I");
            SQL.AppendLine("    WHERE BSNSCLS = '기록지관리'");
            SQL.AppendLine("      AND UNITCLS = '도뇨관관리'");

            #endregion

            #region 폴리
            SQL.AppendLine("UNION ALL");
            SQL.AppendLine("SELECT  '유치도뇨관'");
            SQL.AppendLine("      , ''");
            SQL.AppendLine("      , 7 AS DISSEQNO");
            SQL.AppendLine("	  , (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037645') AS INSERTDAY");
            SQL.AppendLine("	  , TO_NUMBER(NVL((SELECT REGEXP_REPLACE(ITEMVALUE, '[^0-9]+') FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037742'), 0)) AS USEDAY");
            //SQL.AppendLine("	  , (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037742') AS USEDAY");
            SQL.AppendLine("	  , (  SELECT                                                          ");
            SQL.AppendLine("	   	   MAX(CHARTDATE) ");
            SQL.AppendLine("	       FROM ADMIN.AEMRCHARTMST A                                  ");
            SQL.AppendLine("	         INNER JOIN ADMIN.AEMRCHARTROW R                          ");
            SQL.AppendLine("	            ON A.EMRNO = R.EMRNO                                       ");
            SQL.AppendLine("	           AND A.EMRNOHIS = R.EMRNOHIS                                 ");
            SQL.AppendLine("	           AND R.ITEMCD = 'I0000037521'                                ");
            SQL.AppendLine("	           AND R.ITEMVALUE  = '제거'                                    ");
            SQL.AppendLine("          WHERE A.MEDFRDATE = '" + p.medFrDate + "'");
            SQL.AppendLine("            AND A.PTNO = '" + p.ptNo + "'");
            SQL.AppendLine("            AND A.FORMNO = 1575");
            SQL.AppendLine("	    ) AS DELVAL                                                        ");
            SQL.AppendLine(" FROM ADMIN.AEMRCHARTMST A");
            SQL.AppendLine("WHERE A.MEDFRDATE = '" + p.medFrDate + "'");
            SQL.AppendLine("  AND A.PTNO = '" + p.ptNo + "'");
            SQL.AppendLine("  AND A.FORMNO = 2641");
            SQL.AppendLine("  AND (A.CHARTDATE || RPAD(CHARTTIME, 6, '0')) = ");
            SQL.AppendLine("  (");
            SQL.AppendLine("     SELECT MAX(CHARTDATE || CHARTTIME)");
            SQL.AppendLine("     FROM ADMIN.AEMRCHARTMST");
            SQL.AppendLine("    WHERE MEDFRDATE = '" + p.medFrDate + "'");
            SQL.AppendLine("      AND PTNO = '" + p.ptNo + "'");
            SQL.AppendLine("      AND FORMNO = 2641");
            SQL.AppendLine("  )");
            #endregion

            #region 인공기도
            SQL.AppendLine("UNION ALL");
            SQL.AppendLine("SELECT  '인공기도'");
            SQL.AppendLine("      , KINDS");
            SQL.AppendLine("      , 8 AS DISSEQNO");
            SQL.AppendLine("      , INSERTDAY");
            SQL.AppendLine("      , USEDAY");
            SQL.AppendLine("	  , ''");
            SQL.AppendLine("  FROM AA_LIST AA");
            #endregion

            #region 중심정맥관
            SQL.AppendLine("UNION ALL");
            SQL.AppendLine("SELECT  '중심정맥관'");
            SQL.AppendLine("      , KINDS");
            SQL.AppendLine("      , 9 AS DISSEQNO");
            SQL.AppendLine("      , INSERTDAY");
            SQL.AppendLine("      , USEDAY");
            SQL.AppendLine("	  , (  SELECT                                                          ");
            SQL.AppendLine("	   	   MAX(CHARTDATE) ");
            SQL.AppendLine("	       FROM ADMIN.AEMRCHARTMST A                                  ");
            SQL.AppendLine("	         INNER JOIN ADMIN.AEMRCHARTROW R                          ");
            SQL.AppendLine("	            ON A.EMRNO = R.EMRNO                                       ");
            SQL.AppendLine("	           AND A.EMRNOHIS = R.EMRNOHIS                                 ");
            SQL.AppendLine("	           AND R.ITEMCD = 'I0000037604'                                ");
            SQL.AppendLine("	           AND R.ITEMVALUE  = '제거'                                    ");
            SQL.AppendLine("   	         INNER JOIN ADMIN.AEMRCHARTROW R2                         ");
            SQL.AppendLine("	            ON A.EMRNO = R2.EMRNO                                      ");
            SQL.AppendLine("	           AND A.EMRNOHIS = R2.EMRNOHIS                                ");
            SQL.AppendLine("	           AND R2.ITEMCD = 'I0000037611'                               ");
            SQL.AppendLine("          WHERE A.MEDFRDATE = '" + p.medFrDate + "'");
            SQL.AppendLine("            AND A.PTNO = '" + p.ptNo + "'");
            SQL.AppendLine("            AND A.FORMNO = 2227");
            SQL.AppendLine("		    AND R2.ITEMVALUE = AA.KINDS	                                   ");
            SQL.AppendLine("	    ) AS DELVAL                                                        ");
            SQL.AppendLine("  FROM CVC_LIST AA");
            #endregion

            SQL.AppendLine("ORDER BY DISSEQNO");

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString(), clsDB.DbCon);
            if (!string.IsNullOrWhiteSpace(SqlErr))
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count != 0)
            {
                Spd.RowCount = 0;
                Spd.SetRowHeight(-1, 38);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(dt.Rows[i]["INSERTVAL"].ToString().Trim()) ||
                        !string.IsNullOrWhiteSpace(dt.Rows[i]["USEVAL"].ToString().Trim()) ||
                        !string.IsNullOrWhiteSpace(dt.Rows[i]["DELVAL"].ToString().Trim()))
                    {
                        Spd.RowCount += 1;
                        Spd.Cells[Spd.RowCount - 1, 0].Text = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                        Spd.Cells[Spd.RowCount - 1, 1].Text = dt.Rows[i]["KINDS"].ToString().Trim();
                        Spd.Cells[Spd.RowCount - 1, 2].Text = dt.Rows[i]["INSERTVAL"].ToString().Trim();
                        Spd.Cells[Spd.RowCount - 1, 3].Text = dt.Rows[i]["USEVAL"].ToString().Trim();
                        Spd.Cells[Spd.RowCount - 1, 4].Text = dt.Rows[i]["DELVAL"].ToString().Trim();
                    }
                }

            }
            dt.Dispose();
            dt = null;
        }
    }
}
