using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmDocuRemark : Form
    {
        string GstrRetValue = "";

        public frmDocuRemark()
        {
            InitializeComponent();
        }

        public frmDocuRemark(string strRetValue)
        {
            InitializeComponent();
            GstrRetValue = strRetValue;
        }

        private void frmDocuRemark_Load(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ComFunc.ReadSysDate(clsDB.DbCon);
            ss1.ActiveSheet.RowCount = 0;

            SQL = "SELECT CODE, CODENAME, ROWID";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_CODE1";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + GstrRetValue + "'";
            SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";
            SQL = SQL + ComNum.VBLF + " ORDER BY CODE";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ss1.ActiveSheet.RowCount = 0;
                ss1.ActiveSheet.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["CODENAME"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strOK = string.Empty;
            string strCode = string.Empty;
            string strName = string.Empty;
            string strROWID = string.Empty;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < ss1.ActiveSheet.RowCount; i++)
                {
                    strOK = ss1.ActiveSheet.Cells[i, 0].Text;
                    strCode = ss1.ActiveSheet.Cells[i, 1].Text;
                    strName = ss1.ActiveSheet.Cells[i, 2].Text;
                    strROWID = ss1.ActiveSheet.Cells[i, 3].Text;

                    strName = strName.Replace("'", "`");


                    if (strOK == "True")    //'코드삭제
                    {
                        if (strROWID != "")
                        {
                            SQL = "UPDATE KOSMOS_ADM.INSA_DOCU_CODE1 SET";
                            SQL = SQL + "\r\n" + "       DELDATE = SYSDATE";
                            SQL = SQL + "\r\n" + " WHERE ROWID='" + strROWID + "'";
                        }
                    }
                    else
                    {
                        if (VB.Trim(strName) != "")
                        {
                            if (strROWID == "" && VB.Trim(strName) != "")
                            {
                                SQL = "INSERT INTO KOSMOS_ADM.INSA_DOCU_CODE1";
                                SQL = SQL + "\r\n" + "       (CODE, CODENAME, ENTDATE,, GUBUN) ";
                                SQL = SQL + "\r\n" + "VALUES ('" + strCode + "', '" + strName + "', SYSDATE, '" + GstrRetValue + "') ";
                            }
                            else if (strROWID != "")
                            {
                                SQL = "UPDATE KOSMOS_ADM.INSA_DOCU_CODE1 SET";
                                SQL = SQL + "\r\n" + "       CODE = '" + strCode + "',";
                                SQL = SQL + "\r\n" + "       CODENAME = '" + strName + "',";
                                SQL = SQL + "\r\n" + "       ENTDATE = SYSDATE";
                                SQL = SQL + "\r\n" + " WHERE ROWID = '" + strROWID + "'";
                            }
                        }
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void BtnSeqNo_Click(object sender, EventArgs e)
        {
            if (ss1.ActiveSheet.RowCount == 0) return;

            for (int i = 0; i < ss1.ActiveSheet.RowCount; i++)
            {
                ss1.ActiveSheet.Cells[i, 1].Text = string.Format("{0:0000}", i + 1);
            }
        }

        private void frmDocuRemark_Activated(object sender, EventArgs e)
        {

        }

        private void frmDocuRemark_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
