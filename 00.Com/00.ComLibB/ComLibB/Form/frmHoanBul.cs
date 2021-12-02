using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComLibB
{
    public partial class frmHoanBul : Form
    {
        public frmHoanBul()
        {
            InitializeComponent();
        }

        private void frmHoanBul_Load(object sender, EventArgs e)
        {
            string strIndex = "";

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            ComFunc.ReadSysDate(clsDB.DbCon);

            READ_BAS_REFUND(ref strIndex);


            Control[] ctl = this.Controls.Find("optMonth_" + strIndex, true);
            Control obj = null;
            if (ctl != null)
            {
                if (ctl.Length > 0)
                {
                    obj = (RadioButton)ctl[0];
                    ((RadioButton)obj).Checked = true;
                }
            }

            txtPano.Text = "";
            txtPano1.Text = "";
            txtRemark.Text = "";
            lblPaName.Text = "";
            dtpDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            ss1_Sheet1.RowCount = 0;
            ss1_Sheet1.RowCount = 20;
        }

        private void READ_BAS_REFUND(ref string strIndex)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int nGiGan;
            string strROWID;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT GIGAN, ROWID FROM BAS_REFUND ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN  = '0' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    nGiGan = Convert.ToInt32(VB.Val(dt.Rows[0]["GIGAN"].ToString().Trim()));
                    lblROWID.Text = strROWID;
                    i = (nGiGan / 30) - 3;
                    strIndex = Convert.ToString(i);
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtPano1.Text = "";
            ss1_Sheet1.RowCount = 0;
            ss1_Sheet1.RowCount = 20;
        }

        private void btnSaveMonth_Click(object sender, EventArgs e)
        {
            saveMonth();
        }

        private bool saveMonth()
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Control[] ctl = null;
            Control obj = null;
            int nInt = 0;
            string strROWID;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                for (i = 0; i <= 6; i++)
                {
                    ctl = this.Controls.Find("optMonth_" + Convert.ToString(i), true);
                    if (ctl != null)
                    {
                        if (ctl.Length > 0)
                        {
                            obj = (RadioButton)ctl[0];
                        }
                    }
                    if (((RadioButton)obj).Checked == true)
                    {
                        nInt = Convert.ToInt32(VB.Val(VB.Left(((RadioButton)obj).Text, 1)));
                        nInt = nInt * 30;
                    }
                }

                strROWID = lblROWID.Text;

                if (strROWID == "")
                {
                    SQL = " INSERT INTO BAS_REFUND (GUBUN, GIGAN) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " '0', " + nInt + ") ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("자료 등록시 에러 발생.전산실 전화 요망");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }
                else
                {
                    SQL = " UPDATE BAS_REFUND SET GIGAN = " + nInt + " ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("자료 수정시 에러 발생.전산실 전화 요망");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            save();
        }

        private bool save()
        {
            bool rtVal = false;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Control[] ctl = null;
            Control obj = null;
            int nInt = 0;
            string strROWID = "";
            string strPano = "";
            string strDate = "";
            string strRemark = "";
            string strChk = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                strPano = txtPano.Text;
                strDate = dtpDate.Text;
                strRemark = VB.Trim(txtRemark.Text);
                if (strRemark == "")
                {
                    ComFunc.MsgBox("사유를 꼭 입력하세요.");
                    return rtVal;
                }

                for (i = 0; i <= 1; i++)
                {
                    ctl = this.Controls.Find("optBun_" + Convert.ToString(i), true);
                    if (ctl != null)
                    {
                        if (ctl.Length > 0)
                        {
                            obj = (RadioButton)ctl[0];
                        }
                    }
                    if (((RadioButton)obj).Checked == true)
                    {
                        strChk = Convert.ToString(i);
                    }
                }

                SQL = " SELECT ROWID FROM BAS_REFUND ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;


                if (strROWID == "")
                {
                    SQL = " INSERT INTO BAS_REFUND (GUBUN, PANO, ACTDATE, REMARK, BUN) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " '1','" + strPano + "', TO_DATE('" + strDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " '" + strRemark + "', '" + strChk + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("자료 등록시 에러 발생.전산실 전화 요망");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }
                else
                {
                    SQL = " UPDATE BAS_REFUND SET ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " REMARK = '" + strRemark + "', ";
                    SQL = SQL + ComNum.VBLF + " BUN = '" + strChk + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID  = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("자료 수정시 에러 발생.전산실 전화 요망");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수            
            string strPano;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                strPano = VB.Trim(txtPano1.Text);
                ss1_Sheet1.RowCount = 0;

                if (strPano == "")
                {
                    SQL = " SELECT PANO, TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, REMARK, BUN ";
                    SQL = SQL + ComNum.VBLF + " FROM BAS_REFUND ";
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '1' ";
                }
                else
                {
                    SQL = " SELECT PANO, TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, REMARK, BUN ";
                    SQL = SQL + ComNum.VBLF + " FROM BAS_REFUND ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN  = '1' ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        if (dt.Rows[i]["BUN"].ToString().Trim() == "0")
                        {
                            ss1_Sheet1.Cells[i, 3].Text = "환불";
                        }
                        else
                        {
                            ss1_Sheet1.Cells[i, 3].Text = "취소";
                        }
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Control[] ctl = null;
            Control obj = null;
            string strPano = "";
            string strDate = "";

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                strPano = ss1_Sheet1.Cells[e.Row, 0].Text;
                strDate = ss1_Sheet1.Cells[e.Row, 1].Text;

                SQL = " SELECT PANO, TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, REMARK, BUN ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_REFUND ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN  = '1' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtPano.Text = dt.Rows[0]["PANO"].ToString().Trim();
                    dtpDate.Value = Convert.ToDateTime(dt.Rows[0]["ACTDATE"].ToString().Trim());
                    txtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();

                    ctl = this.Controls.Find("optBun_" + dt.Rows[0]["BUN"].ToString().Trim(), true);
                    if (ctl != null)
                    {
                        if (ctl.Length > 0)
                        {
                            obj = (RadioButton)ctl[0];
                            ((RadioButton)obj).Checked = true;
                        }
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Control[] ctl = null;
            Control obj = null;
            string strPano = "";
            string strDate = "";
            string strChk = "";
            string strRemark = "";

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                strPano = txtPano.Text;
                strDate = dtpDate.Text;

                SQL = " SELECT REMARK, BUN FROM BAS_REFUND ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ctl = this.Controls.Find("optBun_" + dt.Rows[0]["BUN"].ToString().Trim(), true);
                    if (ctl != null)
                    {
                        if (ctl.Length > 0)
                        {
                            obj = (RadioButton)ctl[0];
                            ((RadioButton)obj).Checked = true;
                        }
                    }
                    txtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void txtPano_Enter(object sender, EventArgs e)
        {
            txtPano.Text = "";
            txtRemark.Text = "";
            lblPaName.Text = "";
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strPano = "";

            if (e.KeyData == Keys.Enter)
            {
                try
                {

                    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                    txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
                    strPano = txtPano.Text;

                    SQL = " SELECT SNAME FROM BAS_PATIENT ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        lblPaName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
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
                    }
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                }
            }
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strPano = "";
            
            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
                strPano = txtPano.Text;

                SQL = " SELECT SNAME FROM BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblPaName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void txtPano1_Enter(object sender, EventArgs e)
        {
            txtPano1.Text = "";
        }

        private void txtPano1_Leave(object sender, EventArgs e)
        {
            txtPano1.Text = VB.Val(txtPano1.Text).ToString("00000000");
        }
    }
}
