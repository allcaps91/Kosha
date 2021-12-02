using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrtong\FrmNurseTong.frm >> frmNurseTong.cs 폼이름 재정의" />

    public partial class frmNurseTong : Form
    {
        public frmNurseTong()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strSDATE = "";
            string strEDATE = "";
            int i = 0;
            int j = 0;
            int k = 0;
            double nj = 0;
            double nk = 0;
            int nA = 0;
            int nB = 0;
            //int nTEMP = 0;
            string strBUSE = "";
            string strBUSE_OLD = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int nCol = 0;

            strSDATE = (cboYYMM.Text).Trim() + "-01-01";
            strEDATE = (cboYYMM.Text).Trim() + "-12-31";

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT YYMM, WARDCODE, NAME, MATCH_CODE, SUM(1) CNT, PRINTRANKING";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE1 A, KOSMOS_PMPA.NUR_CODE B, KOSMOS_ADM.INSA_MST C";
                SQL = SQL + ComNum.VBLF + " WHERE A.YYMM >= '" + VB.Left(strSDATE.Replace("-", ""), 6) + "'";
                SQL = SQL + ComNum.VBLF + " AND A.YYMM <= '" + VB.Left(strEDATE.Replace("-", ""), 6) + "'";
                SQL = SQL + ComNum.VBLF + " AND A.WARDCODE = B.CODE";
                SQL = SQL + ComNum.VBLF + " AND B.GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + " AND A.SABUN = C.SABUN";
                SQL = SQL + ComNum.VBLF + " AND C.MYEN_CODE = '31'";
                SQL = SQL + ComNum.VBLF + " AND B.CODE NOT IN ('DOCT')";
                SQL = SQL + ComNum.VBLF + " GROUP BY YYMM, WARDCODE, NAME, MATCH_CODE, PRINTRANKING";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING, WARDCODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (strBUSE_OLD != dt.Rows[i]["WARDCODE"].ToString().Trim())
                    {
                        SS1_Sheet1.RowCount = SS1_Sheet1.RowCount + 1;

                        if (i == 0)
                        {
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = dt.Rows[0]["NAME"].ToString().Trim();
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = dt.Rows[0]["MATCH_CODE"].ToString().Trim();
                        }
                        else
                        {
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = dt.Rows[i - 1]["NAME"].ToString().Trim();
                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i - 1]["MATCH_CODE"].ToString().Trim();
                        }
                        
                        if (k != 0 && j != 0)
                        {
                            nj = j;
                            nk = k;

                            SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text = (Math.Round(nk / nj, MidpointRounding.AwayFromZero)).ToString("0");
                        }

                        j = 0;
                        k = 0;

                        strBUSE_OLD = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    }

                    k = k + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    j = j + 1;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT BUSE,  Round((TOIDAY - IPSADAY)/365, 1) CNT";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_ADM.INSA_MST";
                SQL = SQL + ComNum.VBLF + " WHERE TOIDAY >= TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND TOIDAY <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND MYEN_CODE = '31'";
                SQL = SQL + ComNum.VBLF + "   AND BUSE IN  (SELECT MATCH_CODE";
                SQL = SQL + ComNum.VBLF + "   From KOSMOS_PMPA.NUR_CODE";
                SQL = SQL + ComNum.VBLF + "   WHERE GUBUN = '2')";
                SQL = SQL + ComNum.VBLF + "   ORDER BY BUSE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBUSE = dt.Rows[i]["Buse"].ToString().Trim();

                    for (j = 1; j < SS1_Sheet1.RowCount; j++)
                    {

                        if (SS1_Sheet1.Cells[j - 1, 1].Text == strBUSE)
                        {
                            SS1_Sheet1.Cells[j - 1, 3].Text = (VB.Val(SS1_Sheet1.Cells[j - 1, 3].Text) + 1).ToString();
                            nB = (int)VB.Val(SS1_Sheet1.Cells[j - 1, 3].Text);
                            nA = (int)VB.Val(SS1_Sheet1.Cells[j - 1, 2].Text);

                            if (nA > 0)
                            {
                                SS1_Sheet1.Cells[j - 1, 4].Text = ((double)nB / nA * 100).ToString("0.0");
                            }

                            if (VB.Val(dt.Rows[i]["CNT"].ToString().Trim()) < 20)
                            {
                                nCol = (7 + Convert.ToInt32(VB.Val(dt.Rows[i]["CNT"].ToString().Trim())));
                                //SS1_Sheet1.Cells[j, (int)(7 + VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()))].Text
                                //    = (VB.Val(SS1_Sheet1.Cells[j, (int)(6 + VB.Val(dt.Rows[i]["Cnt"].ToString().Trim()))].Text) + 1).ToString();
                            }
                            else
                            {
                                nCol = 27;
                                // SS1_Sheet1.Cells[j, 26].Text = (VB.Val(SS1_Sheet1.Cells[j, 26].Text) + 1).ToString();
                            }

                            SS1_Sheet1.Cells[j - 1, nCol - 1].Text = (VB.Val(SS1_Sheet1.Cells[j - 1, nCol - 1].Text) + 1).ToString();
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                SS1_Sheet1.RemoveRows(0, 1);

                //합계
                SS1_Sheet1.RowCount++;
                SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = "합계";

                for (i = 1; i < SS1_Sheet1.ColumnCount; i++)
                {
                    //퇴직율은 더하지 않는다.
                    if (i == 4)
                    {
                        continue;
                    }

                    for (j = 0; j < SS1_Sheet1.RowCount - 1; j++)
                    {
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, i].Text = (VB.Val(SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, i].Text) + VB.Val(SS1_Sheet1.Cells[j, i].Text)).ToString();
                    }
                }

                //평균 퇴직율
                SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 4].Text = (VB.Val(SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 3].Text) / VB.Val(SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 2].Text) * 100).ToString("0.0");


                ///READ_TOISAYU
                SS2_Sheet1.RowCount = 0;
                SQL = "";
                SQL = " SELECT TOISAYU, SUM(1) CNT";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE TOIDAY >= TO_DATE('" + strSDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND TOIDAY <= TO_DATE('" + strEDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND MYEN_CODE = '31'";
                SQL = SQL + ComNum.VBLF + "   AND BUSE IN  (SELECT MATCH_CODE";
                SQL = SQL + ComNum.VBLF + "   From KOSMOS_PMPA.NUR_CODE";
                SQL = SQL + ComNum.VBLF + "   WHERE GUBUN = '2')";
                SQL = SQL + ComNum.VBLF + " GROUP BY TOISAYU ";
                SQL = SQL + ComNum.VBLF + " ORDER BY CNT DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                SS2_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["TOISAYU"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CNT"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void frmNurseTong_Load(object sender, EventArgs e)
        {
            clsVbfunc.SetCboDateYY(clsDB.DbCon, cboYYMM, 12, "1");
        }
    }
}
