using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewGelSearch.cs
    /// Description     : 계약처 Search
    /// Author          : 박창욱
    /// Create Date     : 2017-10-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MUMAIN05.FRM(FrmGelSearch.frm) >> frmPmpaViewGelSearch.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGelSearch : Form
    {
        public delegate void GetData(string strCodeName, string strCode); //델리게이트 선언문
        public event GetData rGetData; //델리게이트 이벤트 선언문

        public delegate void EventClose();
        public event EventClose rEventClose;

        string GstrMiaCode = "";
        string GstrMiaName = "";

        public frmPmpaViewGelSearch()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            GstrMiaCode = "";
            GstrMiaName = "";
            this.Close();
        }

        private void frmPmpaViewGelSearch_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            txtName.Text = "";
            txtName.Enabled = true;
            txtName.Focus();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GstrMiaCode = ssView_Sheet1.Cells[e.Row, 0].Text.Trim();
            GstrMiaName = ssView_Sheet1.Cells[e.Row, 1].Text.Trim();

            rGetData(GstrMiaCode, GstrMiaName);
            rEventClose();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strCode = "";
            string strName = "";

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT MiaCode, MiaName";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND MiaName  LIKE '%" + txtName.Text.Trim() + "%'";
                if (rdoBi0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND MiaClass = '13'";
                }
                else if (rdoBi1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND MiaClass = '20'";
                }
                else if (rdoBi2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND MiaClass IN ('31','90')";
                }
                else if (rdoBi3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND MiaClass >= '90'";
                }
                else if (rdoBi4.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND GbCha ='*'";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY MiaName";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count == 1)
                {
                    GstrMiaCode = dt.Rows[0]["MiaCode"].ToString().Trim();
                    GstrMiaName = dt.Rows[0]["MiaName"].ToString().Trim();
                    ssView_Sheet1.Cells[0, 0].Text = GstrMiaCode;
                    ssView_Sheet1.Cells[0, 1].Text = GstrMiaName;
                    //ComFunc.MsgBox(GstrMiaCode + GstrMiaName);

                    rGetData(GstrMiaCode, GstrMiaName); //2019-01-02 김해수 수정
                    rEventClose();

                    dt.Dispose();
                    dt = null;
                    this.Close();
                    return;
                }
                else if (dt.Rows.Count > 1)
                {
                    ssView_Sheet1.RowCount = 0;
                    ssView_Sheet1.RowCount = 1;

                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strCode = dt.Rows[i]["MiaCode"].ToString().Trim();
                        strName = dt.Rows[i]["MiaName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 0].Text = strCode;
                        ssView_Sheet1.Cells[i, 1].Text = strName;
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}