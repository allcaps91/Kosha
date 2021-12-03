using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : `
    /// Author          : 김효성
    /// Create Date     : 2018-02-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2014-09-01 컴파일 에러로 막음 KMC 
    /// </history>
    /// <seealso cref= d:\psmh\nurse\nrinfo\FrmSMSHistory.frm" >> frmSMSHistory.cs 폼이름 재정의" />

    public partial class frmSMSHistory : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmSMSHistory()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void READ_SMS()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //'44551 NST전담간호사
                if (clsType.User.Sabun != "44551")
                {
                    SQL = "";
                    SQL = " SELECT ENTDATE, ENTSABUN, HPHONE, SENDTIME, SENDMSG ";
                    SQL = SQL + ComNum.VBLF + "FROM ADMIN.ETC_SMS";
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN IN ( '21') ";
                    SQL = SQL + ComNum.VBLF + " AND ENTSABUN IN (SELECT SABUN FROM ADMIN.INSA_MST";
                    SQL = SQL + ComNum.VBLF + " WHERE BUSE = (";
                    SQL = SQL + ComNum.VBLF + " SELECT BUSE FROM ADMIN.INSA_MST";
                    SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + clsType.User.Sabun + "'))";
                    SQL = SQL + ComNum.VBLF + " AND JOBDATE >= TRUNC(SYSDATE-10)";
                    SQL = SQL + ComNum.VBLF + " ORDER BY ENTDATE";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        SS1_Sheet1.RowCount = dt.Rows.Count;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 1].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim());
                            SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["HPHONE"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SENDTIME"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SENDMSG"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    SS1_Sheet1.RowCount = 0;

                    SQL = "";
                    SQL = " SELECT ENTDATE, ENTSABUN, HPHONE, SENDTIME, SENDMSG ";
                    SQL = SQL + ComNum.VBLF + "FROM ADMIN.ETC_SMS";
                    SQL = SQL + ComNum.VBLF + " WHERE RETTEL =  '010-9684-0579'";
                    SQL = SQL + ComNum.VBLF + " AND JOBDATE >= TRUNC(SYSDATE-30)";
                    SQL = SQL + ComNum.VBLF + " ORDER BY ENTDATE DESC";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        SS1_Sheet1.RowCount = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 1].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim());
                            SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["HPHONE"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SENDTIME"].ToString().Trim();
                            SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SENDMSG"].ToString().Trim();
                        }
                    }
                }

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

        private void frmSMSHistory_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            lblPatient.Text = "";

            READ_SMS();
        }
    }
}
