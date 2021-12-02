using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirAnatView.cs
    /// Description     : 임상병리조회
    /// Author          : 박성완
    /// Create Date     : 2017-12-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\ANATVIEW.frm
    public partial class frmComMirAnatView : Form
    {
        string FstrPano = "";
        string FstrPano_B = "";
        string FstrJinDate_B = "";
        string FstrOutDate_B = "";
        string FstrSName_B = "";
        string FstrBi_B = "";

        public frmComMirAnatView(string GstrPano)
        {
            FstrPano = GstrPano;

            InitializeComponent();

            SetEvent();
        }

        public frmComMirAnatView(string GstrPano_B, string GstrJinDate_B, string GstrOutDate_B, string GstrSName_B, string GstrBi_B)
        {
            FstrPano_B = GstrPano_B;
            FstrJinDate_B = GstrJinDate_B;
            FstrOutDate_B = GstrOutDate_B;
            FstrSName_B = GstrSName_B;
            FstrBi_B = GstrBi_B;

            InitializeComponent();

            SetEvent();
        }

        private void SetEvent()
        {
            this.Load += FrmComMirAnatView_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExit.Click += BtnExit_Click;
            this.txtPano.KeyPress += TxtPano_KeyPress;
            this.ssMain.CellClick += SsMain_CellClick;
        }

        private void SsMain_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인	

            string strRowId = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                strRowId = ssMain.ActiveSheet.Cells[e.Row, 3].Text.Trim();

                SQL = " SELECT RESULT1, RESULT2, BDATE, TO_CHAR(RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE ";
                SQL += " FROM KOSMOS_OCS.EXAM_ANATMST ";
                SQL += " WHERE ROWID = '" + strRowId + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                txtResult.Text = "▶처방일: " + dt.Rows[0]["BDATE"].ToString() + ComNum.VBLF + "▶결과일: " + dt.Rows[0]["RESULTDATE"].ToString() + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Result1"].ToString() + ComNum.VBLF + dt.Rows[0]["Result2"].ToString();

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void TxtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
            {
                return;
            }

            txtPano.Text = txtPano.Text.PadLeft(8, '0');

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT SNAME, BI FROM KOSMOS_PMPA.BAS_PATIENT " + ComNum.VBLF;
            SQL += " WHERE PANO = '" + txtPano.Text + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                lblName.Text = dt.Rows[0]["SNAME"].ToString();
                lblBi.Text = dt.Rows[0]["BI"].ToString();
            }
            else
            {
                MessageBox.Show("해당하는 환자가 없습니다.");
                lblName.Text = "";
                lblBi.Text = "";
            }
            dt.Dispose();
            dt = null;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        private void SearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인	

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT TO_CHAR(a.BDate,'YYYY-MM-DD') BDate, a.GbIO,a.DeptCode, b.OrderName, a.ROWID " + ComNum.VBLF;
                SQL += "  FROM KOSMOS_OCS.EXAM_ANATMST a, KOSMOS_OCS.OCS_ORDERCODE b ,KOSMOS_OCS.EXAM_SPECMST c  " + ComNum.VBLF;
                SQL += " WHERE a.Ptno     = '" + txtPano.Text + "' " + ComNum.VBLF;
                SQL += "   AND a.SpecNo =c.SpecNo " + ComNum.VBLF;
                SQL += "   AND a.OrderCode = b.OrderCode       " + ComNum.VBLF;
                SQL += "   AND A.GBJOB ='V'" + ComNum.VBLF; //JJY(2005-06-17) 해부병리과장님요
                SQL += " ORDER BY a.BDate DESC " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssMain_Sheet1.Rows.Count = dt.Rows.Count;
                ssMain_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssMain.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString();
                    ssMain.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["OrderName"].ToString();
                    ssMain.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString();
                    ssMain.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void FrmComMirAnatView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회                           
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPano.Text = "";
            lblName.Text = "";
            lblBi.Text = "";

            if (FstrPano_B == "")
            {
                txtPano.Text = FstrPano;
                dtpSDate.Text = "2005-01-01";
                dtpEDate.Text = DateTime.Now.ToShortDateString();
            }
            else
            {
                txtPano.Text = FstrPano_B;
                dtpSDate.Text = FstrJinDate_B;
                dtpEDate.Text = FstrOutDate_B;
                lblName.Text = FstrSName_B;
                lblBi.Text = FstrBi_B;
            }

            if (txtPano.Text != "")
            {
                TxtPano_KeyPress(txtPano, new KeyPressEventArgs('\r'));
                btnSearch.PerformClick();
            }

            ssMain.ActiveSheet.Columns[2].Visible = false;
            ssMain.ActiveSheet.Columns[3].Visible = false;
            ssMain.ActiveSheet.Columns[4].Visible = false;
            ssMain.ActiveSheet.Columns[5].Visible = false;
            ssMain.ActiveSheet.Columns[6].Visible = false;
            ssMain.ActiveSheet.Columns[7].Visible = false;
        }
    }
}
