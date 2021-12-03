using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirEndoView.cs
    /// Description     : 내시경 판독 조회
    /// Author          : 박성완
    /// Create Date     : 2017-12-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\ENDOVIEW.FRM
    public partial class frmComMirEndoView : Form
    {
        private string FstrPano = "";

        private string FstrPano_B = "";
        private string FstrJinDate_B = "";
        private string FstrOutDate_B = "";
        private string FstrSName_B = "";
        private string FstrBi_B = "";

        public frmComMirEndoView()
        {
            InitializeComponent();

            SetEvent();
        }

        public frmComMirEndoView(string GstrPano)
        {
            FstrPano = GstrPano;

            InitializeComponent();

            SetEvent();
        }
        public frmComMirEndoView(string GstrPano_B, string GstrJinDate_B, string GstrOutDate_B, string GstrSName_B, string GstrBi_B)
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
            this.Load += FrmComMirXrayView_Load;

            this.btnSearch.Click += BtnSearch_Click;
            this.btnExit.Click += BtnExit_Click;

            this.ssMain.EnterCell += SsMain_EnterCell;
            this.txtPano.KeyPress += TxtPano_KeyPress;
        }

        private void TxtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
                return;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            txtPano.Text = txtPano.Text.PadLeft(8, '0');

            SQL = "";
            SQL += "SELECT SNAME, BI FROM KOSMOS_PMPA.BAS_PATIENT ";
            SQL += " WHERE PANO = '" + txtPano.Text + "' ";
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

        private void SsMain_EnterCell(object sender, EnterCellEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인	

            double cSeqno = 0;
            string cROWID = "";

            txtRemark.Text = "";
            txtResult.Text = "";

            cSeqno = Convert.ToDouble(ssMain.ActiveSheet.Cells[e.Row, 2].Text.Trim());
            cROWID = ssMain.ActiveSheet.Cells[e.Row, 5].Text.Trim();

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //Read_Result
                string strRemark = "";
                string strKorName = "";
                string strResultDrCode = "";

                SQL = "";
                SQL += "SELECT A.Remark1, A.Remark2, A.Remark3, A.Remark4, I.KORNAME," + ComNum.VBLF;
                SQL += "       A.Remark5, A.Remark6, A.PicXY, B.RESULTDRCODE " + ComNum.VBLF;
                SQL += "  FROM KOSMOS_OCS.ENDO_RESULT  A, " + ComNum.VBLF;
                SQL += "       KOSMOS_OCS.ENDO_JUPMST  B," + ComNum.VBLF;
                SQL += "       KOSMOS_ADM.INSA_MST     I" + ComNum.VBLF;
                SQL += " WHERE A.Seqno  = " + cSeqno + " " + ComNum.VBLF;
                SQL += "   AND A.SEQNO = B.SEQNO " + ComNum.VBLF;
                SQL += "   AND B.RESULTDRCODE = I.SABUN(+)" + ComNum.VBLF;
                SQL += "   AND ROWNUM = 1  " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count == 1)
                {
                    strResultDrCode = dt.Rows[0]["RESULTDRCODE"].ToString().Trim();
                    strKorName = dt.Rows[0]["KORNAME"].ToString().Trim();

                    switch (ssMain.ActiveSheet.Cells[e.Row, 3].Text.Trim())
                    {
                        case "1":
                            {
                                strRemark += "◈ Vocal Cord ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark1"].ToString() + ComNum.VBLF;
                                strRemark += "◈ Carina ◈ " + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark2"].ToString() + ComNum.VBLF;
                                strRemark += "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString() + ComNum.VBLF;
                                strRemark += "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;
                                break;
                            }
                        case "2":
                            {
                                strRemark += "◈ Esophagus ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark1"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ Stomach ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark2"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ Duodenum ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ Endoscopic Diagnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark4"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark5"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ Endoscopic Biposy ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark6"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;
                                break;
                            }
                        case "3":
                            {
                                strRemark += "◈ Endoscopic Findings ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark1"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ Endoscopic Disgnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark2"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ Endoscopic Biposy ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark6"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;
                                break;
                            }
                        case "4":
                            {
                                strRemark += "◈ ERCP Finding ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark1"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ Disgnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark2"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ Paln + Tx ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString() + ComNum.VBLF + ComNum.VBLF;
                                strRemark += "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + ssMain.ActiveSheet.Cells[e.Row, 3].Text.Trim() + ComNum.VBLF;
                                strRemark += "◈ 처치의사 : " + strResultDrCode + "  " + strKorName;
                                break;
                            }
                    }
                }

                dt.Dispose();
                dt = null;

                txtResult.Text = strRemark;

                //Read_Remark
                SQL = "" + ComNum.VBLF;
                SQL += "SELECT RemarkC, RemarkX, RemarkP, RemarkD  " + ComNum.VBLF;
                SQL += "  FROM KOSMOS_OCS.ENDO_JUPMST A,   " + ComNum.VBLF;
                SQL += "       KOSMOS_OCS.ENDO_REMARK B    " + ComNum.VBLF;
                SQL += " WHERE A.ROWID      = '" + cROWID + "' " + ComNum.VBLF;
                SQL += "   AND A.Ptno       = B.Ptno      " + ComNum.VBLF;
                SQL += "   AND A.JDate      = B.JDate     " + ComNum.VBLF;
                SQL += "   AND A.OrderCode  = B.OrderCode " + ComNum.VBLF;
                SQL += "   AND ROWNUM       = 1 " + ComNum.VBLF;
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                strRemark = "";

                if (dt.Rows.Count == 1)
                {
                    strRemark = strRemark + "◈ Chief Complaints : " + dt.Rows[0]["RemarkC"].ToString().Trim() + ComNum.VBLF;
                    strRemark = strRemark + "◈ Clinical Diagnosis : " + dt.Rows[0]["RemarkD"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                txtRemark.Text = strRemark;

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
                SQL = "" + ComNum.VBLF;
                SQL += "SELECT TO_CHAR(JDate,'YYYY-MM-DD') BDate1, OrderCode, GbJob, " + ComNum.VBLF;
                SQL += "       Seqno,  Remark, PacsUID,  ROWID " + ComNum.VBLF;
                SQL += "  FROM KOSMOS_OCS.ENDO_JUPMST " + ComNum.VBLF;
                SQL += " WHERE Ptno     = '" + txtPano.Text + "' " + ComNum.VBLF;
                SQL += "   AND JDATE >= TO_DATE('" + dtpSDate.Text + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += "   AND JDATE <= TO_DATE('" + dtpEDate.Text + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += "   AND GbSunap <> '*'  " + ComNum.VBLF;
                SQL += " ORDER BY 1 DESC   " + ComNum.VBLF;

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
                    ssMain.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["BDate1"].ToString();
                    ssMain.ActiveSheet.Cells[i, 1].Text = ReadOrderName(dt.Rows[i]["OrderCode"].ToString());
                    ssMain.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["Seqno"].ToString();
                    ssMain.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["GbJob"].ToString();
                    if (VB.Mid(dt.Rows[i]["Remark"].ToString().Trim(), 1, 50) != "")
                    {
                        ssMain.ActiveSheet.Cells[i, 4].Text = VB.Mid(dt.Rows[i]["Remark"].ToString().Trim(), 1, 50);
                    }
                    if (VB.Mid(dt.Rows[i]["Remark"].ToString().Trim(), 51, 50) != "")
                    {
                        ssMain.ActiveSheet.Cells[i, 4].Text += ",  " + VB.Mid(dt.Rows[i]["Remark"].ToString().Trim(), 51, 50);
                    }
                    if (VB.Mid(dt.Rows[i]["Remark"].ToString().Trim(), 101, 50) != "")
                    {
                        ssMain.ActiveSheet.Cells[i, 4].Text += ",  " + VB.Mid(dt.Rows[i]["Remark"].ToString().Trim(), 101, 50);
                    }

                    ssMain.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString();
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

        private string ReadOrderName(string OrderCode)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            string rtnVal = "";

            SQL = "";
            SQL += "SELECT OrderName FROM KOSMOS_OCS.OCS_ORDERCODE ";
            SQL += " WHERE OrderCode = '" + OrderCode.Trim() + "'   ";
            SQL += "   AND (SendDept <> 'N' OR SendDept IS NULL) ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["OrderName"].ToString().Trim();
            }
            else
            {
                rtnVal = "";
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        private void FrmComMirXrayView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회                           
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPano.Text = "";
            if (FstrPano_B == "")
            {
                txtPano.Text = FstrPano;
                
                dtpSDate.Text = "2018-01-01";
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

            ssMain.ActiveSheet.Columns[5].Visible = false;
            txtRemark.Text = "";
            txtResult.Text = "";
            if (txtPano.Text != "")  btnSearch.PerformClick();

            lblName.Text = "";
            lblBi.Text = "";
        }

    }     
}
