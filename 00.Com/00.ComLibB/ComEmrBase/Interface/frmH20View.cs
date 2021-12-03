using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;

namespace ComEmrBase
{
    public partial class frmH20View : Form
    {
        string strPano = string.Empty;
        string strWard = string.Empty;
        string strDate = string.Empty;

        public frmH20View(string Tdate, string Ward, string Pano)
        {
            strDate = Tdate;
            strWard = Ward;
            strPano = Pano;
            InitializeComponent();
        }

        private void frmH20View_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            SS1_Sheet1.RowCount = 0;

            #region 쿼리
            SQL += ComNum.VBLF + "WITH M AS                                                                                                                             ";
            SQL += ComNum.VBLF + "(                                                                                                                                     ";
            SQL += ComNum.VBLF + "    SELECT A.PTNO                                                                                                                     ";
            SQL += ComNum.VBLF + "         , A.CHARTDATE || ' ' || A.CHARTTIME AS CHARTDATETIME                                                                         ";
            SQL += ComNum.VBLF + "         , R.ITEMCD                                                                                                                   ";
            SQL += ComNum.VBLF + "         , CASE                                                                                                                       ";
            SQL += ComNum.VBLF + "           WHEN (R.ITEMCD = 'I0000037245' OR R.ITEMCD = 'I0000008710_1' OR R.ITEMCD = 'I0000030047' OR R.ITEMCD = 'I0000037576')      ";
            SQL += ComNum.VBLF + "                THEN REPLACE(REPLACE(UPPER(R.ITEMVALUE),'L',''),'%','')    ";
            SQL += ComNum.VBLF + "                ELSE R.ITEMVALUE                                           ";
            SQL += ComNum.VBLF + "           END ITEMVALUE                                                   ";
            SQL += ComNum.VBLF + "      FROM ADMIN.AEMRCHARTMST A                                       ";
            SQL += ComNum.VBLF + "     INNER JOIN ADMIN.AEMRCHARTROW R                                  ";
            SQL += ComNum.VBLF + "        ON A.EMRNO = R.EMRNO                                               ";
            SQL += ComNum.VBLF + "       AND A.EMRNOHIS = R.EMRNOHIS                                         ";
            SQL += ComNum.VBLF + "     INNER JOIN ADMIN.IPD_NEW_MASTER P                               ";
            SQL += ComNum.VBLF + "        ON A.PTNO = P.PANO                                                 ";
            SQL += ComNum.VBLF + "       AND A.CHARTDATE >= TO_CHAR(P.INDATE,'YYYYMMDD')                     ";
            SQL += ComNum.VBLF + "     WHERE A.FORMNO  = 3150 --                                             ";
            SQL += ComNum.VBLF + "       AND A.CHARTDATE <= :TDATE                                           ";
            SQL += ComNum.VBLF + "       AND R.ITEMCD IN ('I0000037859','I0000037280','I0000037245', 'I0000008710_1', 'I0000030047', 'I0000037576','I0000037254')       ";
            SQL += ComNum.VBLF + "       AND R.ITEMVALUE IS NOT NULL                                                                                                    ";
            SQL += ComNum.VBLF + "       AND (P.JDATE = TO_DATE('1900-01-01') OR P.JDATE >= TO_DATE(:TDATE))                  ";
            SQL += ComNum.VBLF + "       AND P.WARDCODE = :WARDCODE                                                           ";
            SQL += ComNum.VBLF + "       AND A.PTNO = :PTNO                                                                   ";
            SQL += ComNum.VBLF + ")                                                                                           ";
            SQL += ComNum.VBLF + ", S AS                                                                                      ";
            SQL += ComNum.VBLF + "(                                                                                           ";
            SQL += ComNum.VBLF + "    SELECT A.PTNO, A.CHARTDATETIME                                                          ";
            SQL += ComNum.VBLF + "         , (SELECT 'Y'                                                                      ";
            SQL += ComNum.VBLF + "              FROM M                                                                        ";
            SQL += ComNum.VBLF + "             WHERE ITEMCD IN ('I0000037859','I0000037280','I0000037254')                    ";
            SQL += ComNum.VBLF + "               AND ITEMVALUE IN ('Remove', '마침','종료', 'Keep 이동', 'Keep이동')                                      ";
            SQL += ComNum.VBLF + "               AND SUBSTR(CHARTDATETIME, 0, 11) = SUBSTR(A.CHARTDATETIME, 0, 11)            ";
            SQL += ComNum.VBLF + "               AND ROWNUM = 1            ";
            SQL += ComNum.VBLF + "           ) AS Y                                                                           ";
            SQL += ComNum.VBLF + "      FROM M A                                                                              ";
            SQL += ComNum.VBLF + "     WHERE A.ITEMCD IN ('I0000037859','I0000037280','I0000037254')                          ";
            SQL += ComNum.VBLF + "      AND A.ITEMVALUE IN ('Start','시작')                                                    ";
            SQL += ComNum.VBLF + "    UNION                                                                                   ";
            SQL += ComNum.VBLF + "    SELECT PTNO, MIN(CHARTDATETIME) AS CHARTDATETIME , NULL                                 ";
            SQL += ComNum.VBLF + "      FROM M                                                                                ";
            SQL += ComNum.VBLF + "     WHERE ITEMCD IN ('I0000037859','I0000037280','I0000037254')                            ";
            SQL += ComNum.VBLF + "     GROUP BY PTNO                                                                          ";
            SQL += ComNum.VBLF + ")                                                                                           ";
            SQL += ComNum.VBLF + ", R AS                                                                                      ";
            SQL += ComNum.VBLF + "(                                                                                           ";
            SQL += ComNum.VBLF + "    SELECT A.PTNO, A.CHARTDATETIME                                                          ";
            SQL += ComNum.VBLF + "         , (SELECT 'Y'                                                                      ";
            SQL += ComNum.VBLF + "              FROM M                                                                        ";
            SQL += ComNum.VBLF + "             WHERE ITEMCD IN ('I0000037859','I0000037280','I0000037254')                    ";
            SQL += ComNum.VBLF + "               AND ITEMVALUE IN ('Start','시작')                                             ";
            SQL += ComNum.VBLF + "               AND SUBSTR(CHARTDATETIME, 0, 11) = SUBSTR(A.CHARTDATETIME, 0, 11)            ";
            SQL += ComNum.VBLF + "               AND ROWNUM = 1            ";
            SQL += ComNum.VBLF + "           ) AS Y                                                                           ";
            SQL += ComNum.VBLF + "      FROM M A                                                                              ";
            SQL += ComNum.VBLF + "     WHERE A.ITEMCD IN ('I0000037859','I0000037280','I0000037254')                          ";
            SQL += ComNum.VBLF + "       AND A.ITEMVALUE IN ('Remove', '마침','종료', 'Keep 이동', 'Keep이동')                                            ";
            SQL += ComNum.VBLF + ")                                                                                           ";
            
            SQL += ComNum.VBLF + ", DATA_LIST AS                                                                                                                                 ";
            SQL += ComNum.VBLF + "(                                                                                                                                ";
            SQL += ComNum.VBLF + "SELECT                                                                   ";
            SQL += ComNum.VBLF + "                   A.PTNO                                                ";
            SQL += ComNum.VBLF + "                 , A.CHARTDATETIME AS STIME                              ";
            SQL += ComNum.VBLF + "                 , (SELECT MIN(CHARTDATETIME)                            ";
            SQL += ComNum.VBLF + "                      FROM R                                             ";
            SQL += ComNum.VBLF + "                     WHERE PTNO = A.PTNO                                 ";
            SQL += ComNum.VBLF + "                       AND CHARTDATETIME >= A.CHARTDATETIME              ";
            SQL += ComNum.VBLF + "--                       AND Y IS NULL                                   ";
            SQL += ComNum.VBLF + "                    ) AS ETIME                                           ";
            SQL += ComNum.VBLF + "              FROM S A                                                   ";
            SQL += ComNum.VBLF + "--             WHERE Y IS NULL                                           ";
            SQL += ComNum.VBLF + ")                                                                                                       ";

            SQL += ComNum.VBLF + "SELECT                                                                                                       ";
            SQL += ComNum.VBLF + "                   A.PTNO                                                                                    ";
            SQL += ComNum.VBLF + "                 , A.STIME                                                                  ";
            SQL += ComNum.VBLF + "                 , ETIME                                                                ";
            SQL += ComNum.VBLF + "                 , CASE WHEN                                                                                  ";
            SQL += ComNum.VBLF + "                             (SELECT COUNT(ETIME)                                                                                   ";
            SQL += ComNum.VBLF + "                                FROM DATA_LIST                                                                                   ";
            SQL += ComNum.VBLF + "                               WHERE PTNO = A.PTNO                                                                                ";
            SQL += ComNum.VBLF + "                                 AND ETIME = A.ETIME                                                                                  ";
            SQL += ComNum.VBLF + "                              ) > 1 THEN  '1' END DUPTIME     ";
            SQL += ComNum.VBLF + "              FROM DATA_LIST A                                                                                       ";
            #endregion

            ClsParameter clsParameter = new ClsParameter();
            clsParameter.Add("TDATE", strDate);
            clsParameter.Add("TDATE", strDate);
            clsParameter.Add("WARDCODE", strWard);
            clsParameter.Add("PTNO", strPano);

            SqlErr = clsDB.GetDataTableRExP(ref dt, SQL, clsDB.DbCon, clsParameter);
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
                return;
            }

            SS1_Sheet1.RowCount = dt.Rows.Count;

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PTNO"].ToString();
                SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["STIME"].ToString().NotEmpty() ? DateTime.ParseExact(dt.Rows[i]["STIME"].ToString(), "yyyyMMdd HHmmss", null).ToString("yyyy-MM-dd HH:mm") : "";
                SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ETIME"].ToString().NotEmpty() ? DateTime.ParseExact(dt.Rows[i]["ETIME"].ToString(), "yyyyMMdd HHmmss", null).ToString("yyyy-MM-dd HH:mm") : "";

                if (dt.Rows[i]["DUPTIME"].ToString().Trim().Equals("1"))
                {
                    SS1_Sheet1.Cells[i, 2].ForeColor = System.Drawing.Color.Red;
                }
            }

            dt.Dispose();
        }
    }
}
