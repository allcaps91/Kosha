using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrPeritonealDialysis : Form
    {
        EmrPatient AcpEmr = null;

        public frmEmrPeritonealDialysis(EmrPatient p)
        {
            AcpEmr = p;
            InitializeComponent();
        }

        private void frmEmrPeritonealDialysis_Load(object sender, EventArgs e)
        {
            ss1_Sheet1.RowCount = 0;
            
            Text = "등록번호 : " + AcpEmr.ptNo + " 환자이름 : " + AcpEmr.ptName;
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            dtpFrDate.Value = DateTime.ParseExact(AcpEmr.medFrDate, "yyyyMMdd", null);
            dtpEndDate.Value = DateTime.ParseExact(string.IsNullOrWhiteSpace(AcpEmr.medEndDate) ? strCurDate : AcpEmr.medEndDate, "yyyyMMdd", null);
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            GetRecordData();
        }

        /// <summary>
        /// 기록지 가져오기.
        /// </summary>
        private void GetRecordData()
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                #region 쿼리
                SQL.AppendLine("SELECT");
                SQL.AppendLine("         A.CHARTDATE");
                SQL.AppendLine("       , A.CHARTTIME");
                SQL.AppendLine("       , (R2.ITEMVALUE || R3.ITEMVALUE) AS GBN");
                SQL.AppendLine("       , R4.ITEMVALUE AS INJECTIONVAL");
                SQL.AppendLine("       , R.ITEMNO");
                SQL.AppendLine("       , R.ITEMVALUE");

                #region 총제수량 일의 마지막에 합계 표시
                SQL.AppendLine("       , CASE WHEN R.ITEMNO = 'I0000013209' AND CHARTTIME = ");
                SQL.AppendLine("            (SELECT MAX(CHARTTIME)");
                SQL.AppendLine("               FROM ADMIN.AEMRCHARTMST");
                SQL.AppendLine("              WHERE MEDFRDATE  = A.MEDFRDATE ");
                SQL.AppendLine("                AND FORMNO  = A.FORMNO ");
                SQL.AppendLine("                AND CHARTDATE = A.CHARTDATE");
                SQL.AppendLine("            )");
                SQL.AppendLine("         THEN");
                SQL.AppendLine("             (SELECT SUM(RR.ITEMVALUE)                                                     ");
                SQL.AppendLine("                FROM ADMIN.AEMRCHARTMST AA                                            ");
                SQL.AppendLine("                  INNER JOIN ADMIN.AEMRCHARTROW RR                                  ");
                SQL.AppendLine("                     ON AA.EMRNO = RR.EMRNO                                             ");
                SQL.AppendLine("                    AND AA.EMRNOHIS = RR.EMRNOHIS                                          ");
                SQL.AppendLine("                    AND RR.ITEMNO = 'I0000013209'                                          ");
                SQL.AppendLine("                  INNER JOIN ADMIN.AEMRCHARTROW RR2                                 ");
                SQL.AppendLine("                     ON AA.EMRNO = RR2.EMRNO                                            ");
                SQL.AppendLine("                    AND AA.EMRNOHIS = RR2.EMRNOHIS                                         ");
                SQL.AppendLine("                    AND RR2.ITEMNO = 'I0000013354'                                         ");
                SQL.AppendLine("                  INNER JOIN ADMIN.AEMRCHARTROW RR3                                 ");
                SQL.AppendLine("                     ON AA.EMRNO = RR3.EMRNO                                            ");
                SQL.AppendLine("                    AND AA.EMRNOHIS = RR3.EMRNOHIS                                         ");
                SQL.AppendLine("                    AND RR3.ITEMNO = 'I0000013353'                                         ");
                SQL.AppendLine("                WHERE AA.MEDFRDATE = A.MEDFRDATE                                           ");
                SQL.AppendLine("                  AND AA.FORMNO = A.FORMNO                                               ");
                SQL.AppendLine("                  AND AA.CHARTDATE = A.CHARTDATE                                         ");
                SQL.AppendLine("                  AND REPLACE(RR2.ITEMVALUE || RR3.ITEMVALUE, '%', '') = REPLACE(R2.ITEMVALUE || R3.ITEMVALUE, '%', '')");
                SQL.AppendLine("             )                                                                         ");
                SQL.AppendLine("         END MAXVAL");
                #endregion

                SQL.AppendLine("  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A");
                SQL.AppendLine("    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R");
                SQL.AppendLine("       ON A.EMRNO    = R.EMRNO");
                SQL.AppendLine("      AND A.EMRNOHIS = R.EMRNOHIS");
                SQL.AppendLine("      AND R.ITEMNO IN ('I0000013293', 'I0000013058', 'I0000013209')");

                SQL.AppendLine("    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R2");
                SQL.AppendLine("       ON A.EMRNO    = R2.EMRNO");
                SQL.AppendLine("      AND A.EMRNOHIS = R2.EMRNOHIS");
                SQL.AppendLine("      AND R2.ITEMNO IN ('I0000013354') -- 투석액종류");

                SQL.AppendLine("    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R3");
                SQL.AppendLine("       ON A.EMRNO    = R3.EMRNO");
                SQL.AppendLine("      AND A.EMRNOHIS = R3.EMRNOHIS");
                SQL.AppendLine("      AND R3.ITEMNO IN ('I0000013353') -- 투석액농도");

                SQL.AppendLine("    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R4");
                SQL.AppendLine("       ON A.EMRNO    = R4.EMRNO");
                SQL.AppendLine("      AND A.EMRNOHIS = R4.EMRNOHIS");
                SQL.AppendLine("      AND R4.ITEMNO IN ('I0000013293') -- 주입량");

                SQL.AppendLine(" WHERE MEDFRDATE = '" + AcpEmr.medFrDate + "'");
                SQL.AppendLine("   AND PTNO = '" + AcpEmr.ptNo + "'");
                SQL.AppendLine("   AND FORMNO = 1571 --복막투석");
                SQL.AppendLine("   AND CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'");
                SQL.AppendLine("   AND CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "'");
                //SQL.AppendLine(" GROUP BY A.CHARTDATE, CUBE(((R2.ITEMVALUE || R3.ITEMVALUE), R4.ITEMVALUE, R.ITEMNO))--복막투석");
                SQL.AppendLine(" ORDER BY (A.CHARTDATE || A.CHARTTIME) DESC ");
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                ss1_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    string beforeDuplication = dt.Rows[0]["CHARTDATE"].ToString().Trim() +
                                               dt.Rows[0]["CHARTTIME"].ToString().Trim() + 
                                               dt.Rows[0]["GBN"].ToString().Trim() +
                                               dt.Rows[0]["INJECTIONVAL"].ToString().Trim();
                    ss1_Sheet1.RowCount += 1;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string nowDuplication = dt.Rows[i]["CHARTDATE"].ToString().Trim() +
                                                dt.Rows[i]["CHARTTIME"].ToString().Trim() +
                                                dt.Rows[i]["GBN"].ToString().Trim() +
                                                dt.Rows[i]["INJECTIONVAL"].ToString().Trim();

                        if (string.IsNullOrWhiteSpace(dt.Rows[i]["GBN"].ToString().Trim()))
                            continue;

                        if (beforeDuplication.Equals(nowDuplication) == false)
                        {
                            ss1_Sheet1.RowCount += 1;
                            beforeDuplication = nowDuplication;
                        }

                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 0].Text = DateTime.ParseExact(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "yyyyMMdd", null).ToString("yyyy-MM-dd");
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 1].Text = DateTime.ParseExact(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "HHmmss", null).ToString("HH:mm");
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["GBN"].ToString().Trim();

                        //주입량
                        if (dt.Rows[i]["ITEMNO"].ToString().Trim().Equals("I0000013293"))
                        {
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();

                        }
                        //배액량
                        else if (dt.Rows[i]["ITEMNO"].ToString().Trim().Equals("I0000013058"))
                        {
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();

                        }
                        //제수량
                        else if (dt.Rows[i]["ITEMNO"].ToString().Trim().Equals("I0000013209"))
                        {
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                        }

                        if (!string.IsNullOrWhiteSpace(dt.Rows[i]["MAXVAL"].ToString().Trim()))
                        {
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["MAXVAL"].ToString().Trim();
                        }
                    }

                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message, "에러");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
