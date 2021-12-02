using ComBase;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// 신규 개발(스캔 변환 조회)
    /// </summary>
    public partial class frmDietScanView : Form, MainFormMessage
    {
        #region //MainFormMessage
        string mPara1 = string.Empty;
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage

        /// <summary>
        /// 스캔 뷰어
        /// </summary>
        frmScanImageViewNew3 frmScanImageView = null;

        #region 생성자
        public frmDietScanView()
        {
            InitializeComponent();
        }

        public frmDietScanView(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmDietScanView(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;

        }
        #endregion
        private void frmDietScanView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboGBN.Items.Add("의뢰서");
            cboGBN.Items.Add("협진");
            cboGBN.SelectedIndex = clsType.User.IsNurse.Equals("OK") ? 1 : 0;
            ssView_Sheet1.RowCount = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        private void GetSearchData()
        {
            ssView_Sheet1.RowCount = 0;
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }


            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "SELECT B.TREATNO, A.PANO, P.SNAME, U.NAME, B.CLINCODE, GBN, TO_CHAR(REQUESTDATE, 'YYYY-MM-DD') REQUESTDATE, B.INDATE, COUNT(PAGENO) CNT";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_NST_CVT_HISTORY A";
            SQL += ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "EMR_TREATT B";
            SQL += ComNum.VBLF + "     ON A.TREATNO = B.TREATNO";
            SQL += ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "EMR_USERT U";
            SQL += ComNum.VBLF + "     ON B.DOCCODE = U.USERID";
            SQL += ComNum.VBLF + "  INNER JOIN " + ComNum.DB_PMPA + "BAS_PATIENT P";
            SQL += ComNum.VBLF + "     ON P.PANO = A.PANO";
            SQL += ComNum.VBLF + "WHERE REQUESTDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND REQUESTDATE <= TO_DATE('" + dtpDate2.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";

            if (txtPano.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "  AND A.PANO = '" + txtPano.Text.Trim() + "'";
            }

            if (cboGBN.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "  AND A.GBN = '" + cboGBN.Text.Trim() + "'";
            }

            SQL += ComNum.VBLF + "GROUP BY A.PANO, P.SNAME, U.NAME, B.CLINCODE, B.TREATNO, REQUESTDATE, B.INDATE, GBN";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            try
            {
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 0].Tag  = dt.Rows[i]["TREATNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["CLINCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GBN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["REQUESTDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = int.Parse(dt.Rows[i]["INDATE"].ToString().Trim()).ToString("0000-00-00");
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    }
                }

                dt.Dispose();
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
                return;

            string TreatNo = ssView_Sheet1.Cells[e.Row, 0].Tag.ToString().Trim();
            if (frmScanImageView != null)
            {
                frmScanImageView.gPatientinfoRecive(TreatNo, "109", "");
                return;
            }

            frmScanImageView = new frmScanImageViewNew3(this, TreatNo, "109", "0");
            frmScanImageView.TopLevel = false;
            frmScanImageView.Dock = DockStyle.Fill;
            frmScanImageView.FormBorderStyle = FormBorderStyle.None;
            frmScanImageView.Parent = panScan;
            frmScanImageView.Show();
            panScan.Controls.Add(frmScanImageView);
        }

        private void frmDietScanView_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmDietScanView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmScanImageView != null)
            {
                frmScanImageView.Dispose();
                frmScanImageView = null;
            }

            string mstrViewPath = @"C:\PSMHEXE\ScanTmp\Formname" + "\\" + ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            if (Directory.Exists(mstrViewPath))
            {
                clsImgcvt.DelAllFile(mstrViewPath);
            }

            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void frmDietScanView_Resize(object sender, EventArgs e)
        {
            if (frmScanImageView != null)
            {
                frmScanImageView.Width = panScan.Width;
                frmScanImageView.Height = panScan.Height;
            }
        }
    }
}
