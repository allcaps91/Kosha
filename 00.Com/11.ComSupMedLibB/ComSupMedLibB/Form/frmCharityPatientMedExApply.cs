using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupMedLibB
{
    public partial class frmCharityPatientMedExApply : Form
    {
        private frmCharityPatientMedExApplyTong frmCharityPatientMedExApplyTongX;

        public frmCharityPatientMedExApply()
        {
            InitializeComponent();
        }

        private void frmCharityPatientMedExApply_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            Screen_Clear();

            if (frmCharityPatientMedExApplyTongX != null)
            {
                frmCharityPatientMedExApplyTongX.Dispose();
                frmCharityPatientMedExApplyTongX = null;
            }

            frmCharityPatientMedExApplyTongX = new frmCharityPatientMedExApplyTong();
            frmCharityPatientMedExApplyTongX.rGetPatientInfo += new frmCharityPatientMedExApplyTong.GetPatientInfo(frmCharityPatientMedExApplyTongX_rGetPatientInfo);
            frmCharityPatientMedExApplyTongX.rEventClosed += new frmCharityPatientMedExApplyTong.EventClosed(FrmCharityPatientMedExApplyTongX_rEventClosed);


            frmCharityPatientMedExApplyTongX.ShowDialog();
            frmCharityPatientMedExApplyTongX = null;
        }

        private void FrmCharityPatientMedExApplyTongX_rEventClosed()
        {
            frmCharityPatientMedExApplyTongX.Dispose();
            frmCharityPatientMedExApplyTongX = null;
        }

        private void frmCharityPatientMedExApplyTongX_rGetPatientInfo(string strPtno)
        {
            if (frmCharityPatientMedExApplyTongX == null)
            {
                frmCharityPatientMedExApplyTongX.Dispose();
                frmCharityPatientMedExApplyTongX = null;
            }

            GetPatientInfo(strPtno);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            Screen_Clear();
        }

        private void SaveData()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strPtno = "";
            string strActiveNo = "";

            Cursor.Current = Cursors.WaitCursor;

            if (ComFunc.MsgBoxQ("저장하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            SQL = "";
            SQL = "SELECT * FROM KOSMOS_ADM.SOCIAL_CHARITY_PATIENT";
            SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + txtPtno.Text + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strPtno = dt.Rows[0]["PTNO"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strPtno == "")
                {
                    SQL = "SELECT KOSMOS_ADM.SEQ_SOCIAL_CHARITY_PATIENT.NEXTVAL FROM DUAL";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strActiveNo = dt.Rows[0]["NEXTVAL"].ToString().Trim();
                    }

                    txtSeqno.Text = strActiveNo;

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "INSERT INTO KOSMOS_ADM.SOCIAL_CHARITY_PATIENT";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + "     SEQNO, PTNO, PTNAME, BIRTHDATE, PHONENO, DEPTCODE, ILLNAME, STAY, GUBUN, TMEDFEE, BONFEE, SUPPORTFEE, WELFAREOP";
                    SQL = SQL + ComNum.VBLF + ")";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + "     '" + txtSeqno.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtPtno.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtBirth.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtTel.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtDept.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtIlls.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtJaewon.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtGubun.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtTotalFee.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtBonFee.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtHuwonFee.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtEtc.Text + "'";
                    SQL = SQL + ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = "UPDATE KOSMOS_ADM.SOCIAL_CHARITY_PATIENT SET";
                    SQL = SQL + ComNum.VBLF + "     PTNO = '" + txtPtno.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     PTNAME = '" + txtName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     BIRTHDATE = '" + txtBirth.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     PHONENO = '" + txtTel.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     DEPTCODE = '" + txtDept.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     ILLNAME = '" + txtIlls.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     STAY = '" + txtJaewon.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     GUBUN = '" + txtGubun.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     TMEDFEE = '" + txtTotalFee.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     BONFEE = '" + txtBonFee.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     SUPPORTFEE = '" + txtHuwonFee.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     WELFAREOP = '" + txtEtc.Text + "'";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtno + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = "";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            CS = null;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetPatientInfo(string strPtno)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT SEQNO, PTNO, PTNAME, BIRTHDATE, PHONENO, DEPTCODE, ILLNAME, STAY, GUBUN, TMEDFEE, BONFEE, SUPPORTFEE, WELFAREOP";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.SOCIAL_CHARITY_PATIENT";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtno + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtSeqno.Text = dt.Rows[0]["SEQNO"].ToString().Trim();
                    txtPtno.Text = dt.Rows[0]["PTNO"].ToString().Trim();
                    txtName.Text = dt.Rows[0]["PTNAME"].ToString().Trim();
                    txtBirth.Text = dt.Rows[0]["BIRTHDATE"].ToString().Trim();
                    txtTel.Text = dt.Rows[0]["PHONENO"].ToString().Trim();
                    txtDept.Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    txtIlls.Text = dt.Rows[0]["ILLNAME"].ToString().Trim();
                    txtJaewon.Text = dt.Rows[0]["STAY"].ToString().Trim();
                    txtGubun.Text = dt.Rows[0]["GUBUN"].ToString().Trim();
                    txtTotalFee.Text = dt.Rows[0]["TMEDFEE"].ToString().Trim();
                    txtBonFee.Text = dt.Rows[0]["BONFEE"].ToString().Trim();
                    txtHuwonFee.Text = dt.Rows[0]["SUPPORTFEE"].ToString().Trim();
                    txtEtc.Text = dt.Rows[0]["WELFAREOP"].ToString().Trim();

                    ssView_Sheet1.Cells[4, 2].Text = txtPtno.Text;
                    ssView_Sheet1.Cells[4, 5].Text = txtName.Text;
                    ssView_Sheet1.Cells[5, 2].Text = txtBirth.Text;
                    ssView_Sheet1.Cells[5, 5].Text = txtTel.Text;
                    ssView_Sheet1.Cells[6, 2].Text = txtDept.Text;
                    ssView_Sheet1.Cells[6, 5].Text = txtIlls.Text;
                    ssView_Sheet1.Cells[7, 2].Text = txtJaewon.Text;
                    ssView_Sheet1.Cells[7, 5].Text = txtGubun.Text;
                    ssView_Sheet1.Cells[9, 2].Text = txtTotalFee.Text;
                    ssView_Sheet1.Cells[9, 5].Text = txtBonFee.Text;
                    ssView_Sheet1.Cells[10, 2].Text = txtHuwonFee.Text;
                    ssView_Sheet1.Cells[13, 1].Text = txtEtc.Text;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void txtPtno_Leave(object sender, EventArgs e)
        {
            string strPtno = "";

            strPtno = txtPtno.Text;

            txtPtno.Text = ComFunc.LPAD(strPtno, 8, "0");
        }

        private void Screen_Clear()
        {
            txtSeqno.Text = "";
            txtPtno.Text = "";
            txtName.Text = "";
            txtBirth.Text = "";
            txtTel.Text = "";
            txtDept.Text = "";
            txtIlls.Text = "";
            txtJaewon.Text = "";
            txtGubun.Text = "";
            txtTotalFee.Text = "";
            txtBonFee.Text = "";
            txtHuwonFee.Text = "";
            txtEtc.Text = "";
        }
    }
}
