using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace ComBase
{
    /// <summary>
    /// Class Name      : SupDiet
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\mtsEmr\CADEX\Frm영양초기평가명단" >> frmBasDietEvaluationList.cs 폼이름 재정의" />

    public partial class frmBasDietEvaluationList : Form
    {
        public delegate void EventClose();
        public event EventClose rEventClose;

        string FnIPDNO = "";
        string Gstrname = "";

        public frmBasDietEvaluationList(string FnIPDNOX)
        {
            InitializeComponent();

            FnIPDNO = FnIPDNOX;

        }

        public frmBasDietEvaluationList()
        {
            InitializeComponent();
        }

        private void frmBasDietEvaluationList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            READ_HISTORY(FnIPDNO);

        }

        private void READ_HISTORY(string ArgIpdNo)
        {

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ss_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(BUILDDATE, 'YYYY-MM-DD') BDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(CERTIDATE, 'YYYY-MM-DD') CDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(NEED_DATE, 'YYYY-MM-DD') NDATE, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_RATING_PATIENT_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIpdNo;
                SQL = SQL + ComNum.VBLF + " ORDER BY BUILDDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {

                    ss_Sheet1.Rows.Count = dt.Rows.Count;
                    ss_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ss_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CDATE"].ToString().Trim();
                        ss_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NDATE"].ToString().Trim();
                        ss_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }

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

        private void ss_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            int i = 0;
            string strRE = "";

            if (ss_Sheet1.RowCount == 0)
            {
                return;
            }

            FnIPDNO = ss_Sheet1.Cells[e.Row, 3].Text.Trim();

            if (ss_Sheet1.Cells[e.Row, e.Column].Text == "")
            {
                return;
            }

            switch (e.Column)
            {
                case 1:
                case 2:
                    for (i = ss_Sheet1.RowCount; i <= 1; i++)
                    {
                        if (ss_Sheet1.Cells[i - 1, 1].Text != "")
                        {
                            break;
                        }
                    }

                    if (i > e.Row)
                    {
                        Gstrname = "RE";
                    }
                    else
                    {
                        Gstrname = "";
                    }
                    //TODO 첫번째 인수 프로그램 이름 
                    //frmDietEvaluationNew f = new frmDietEvaluationNew(, FnIPDNO, Gstrname);
                    //f.ShowDialog();
                    break;
                case 3:
                    //Frm영양불량평가지New.Show 1
                    break;

            }
            FnIPDNO = "";
            Gstrname = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClose();
        }
    }
}
