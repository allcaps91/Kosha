using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    public partial class frmDNR : Form
    {

        #region //MainFormMessage

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

        string fstrIPDNO = "";              //DNR 목록 프로그램에서 조회했을 경우에 사용 수정/삭제에 사용합니다. 
        string fstrWard = "";

        public frmDNR(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmDNR()
        {
            InitializeComponent();
            setEvent();
        }

        public frmDNR(string argIPDNO, string argWard = "", string argPANO = "")
        {
            InitializeComponent();
            setEvent();
            fstrIPDNO = argIPDNO;
            fstrWard = argWard;
            eGetData(argIPDNO, argPANO);
        }

        private void setCtrlData()
        {
        }

        private void setCtrlInit()
        {
        }

            

        private void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnNew.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);

            this.txtPano.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtPano.Leave += new EventHandler(eKeyLeave);

            this.txtDsabun.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtDsabun.Leave += new EventHandler(eKeyLeave);

        }
        private void eKeyLeave(object sender, EventArgs e)
        {
            ComFunc cf = new ComFunc();
            if (sender == this.txtPano)
            {
                txtSname.Text = cf.Read_Patient(clsDB.DbCon, txtPano.Text.Trim(), "2");
            }
            else if (sender == this.txtDsabun)
            {
                txtDname.Text = cf.Read_SabunName(clsDB.DbCon, txtDsabun.Text.Trim());
            }
        }


        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            

            if (sender == this.txtPano)
            {
                if (e.KeyChar == 13)
                {
                    txtDsabun.Focus();
                }
            }
            else if(sender == this.txtDsabun)
            {
                if (e.KeyChar == 13)
                {
                    txtRemark.Focus();
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnNew)
            {
                txtPano.Text = "";
                txtSname.Text = "";
                cboWard.SelectedIndex = -1;
                txtRemark.Text = "";
                txtDdate.Text = "";
                txtDsabun.Text = "";
                txtDname.Text = "";
                fstrIPDNO = "";
            }
            else if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSaveData(fstrIPDNO);
            }
            else if (sender == this.btnDelete)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eDelData(fstrIPDNO);
            }
        }
        private void ComboWard_SET()
        {
            int i = 0;
            //int j = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  WardCode, WardName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND WARDCODE NOT IN ('IU','NP','2W','NR','DR','IQ','ER')";
            SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
            SQL += ComNum.VBLF + "ORDER BY WardCode";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            cboWard.Items.Clear();

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;

            cboWard.SelectedIndex = 0;

            if (fstrWard != "")
            {
                cboWard.SelectedIndex = cboWard.Items.IndexOf(fstrWard);
                //cboWard.Enabled = false;
            }
            
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                ComFunc CF = new ComFunc();

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                ComFunc.ReadSysDate(clsDB.DbCon);

                string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                txtDdate.Text = strDate;

                if (fstrWard == "" && fstrIPDNO != "")
                {
                    fstrWard = ReadWard(fstrIPDNO);
                }
                ComboWard_SET();

                txtPano.Focus();
                
            }
        }

        private void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        private void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        private void eDelData(string argIPDNO)
        {
           
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            //DataTable dt = null;

            if (argIPDNO == "")
            {
                MessageBox.Show("저장된 내용이 없습니다.");
                return;
            }

            if (ComFunc.MsgBoxQ("삭제 후 복원은 불가능합니다. 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " DELETE FROM KOSMOS_PMPA.NUR_DNR WHERE IPDNO = " + argIPDNO;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                btnExit.PerformClick();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
            
        }

        private void eSaveData(string argIPDNO = "")
        {

            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            string strROWID = "";

            string strIPDNO = "";

            #region 등록 전 체크
            if (txtPano.Text.Trim() == "" || txtSname.Text.Trim() == "")
            {
                MessageBox.Show("등록번호가 공란이거나 검색된(재원중인) 환자가 없습니다.");
                return;
            }

            if (cboWard.Text.Trim() == "")
            {
                MessageBox.Show("병동이 공란입니다.");
                return;
            }

            if (txtDdate.Text.Trim() == "")
            {
                MessageBox.Show("등록일자가 공란입니다.");
                return;
            }

            if (txtDsabun.Text.Trim() == "" || txtDname.Text.Trim() == "")
            {
                MessageBox.Show("등록자사번이 공란이거나 올바르지 않은 사번입니다.");
                return;
            }
            #endregion


            SQL = " SELECT ROWID  FROM KOSMOS_PMPA.NUR_DNR ";
            SQL += ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            if (strROWID != "")
            {
                if (ComFunc.MsgBoxQ("저장된 내용을 수정하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                SQL = " SELECT IPDNO ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "  WHERE JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND PANO = '" + txtPano.Text.Trim() + "'";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strIPDNO == "")
                {
                    MessageBox.Show("등록번호가 공란이거나 검색된(재원중인) 환자가 없습니다.");
                    return;
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strROWID != "")
                {
                    SQL = " UPDATE KOSMOS_PMPA.NUR_DNR SET ";
                    SQL += ComNum.VBLF + " DDATE = TO_DATE('" + txtDdate.Text.Trim() + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + " DSABUN = " + txtDsabun.Text.Trim() + ",";
                    SQL += ComNum.VBLF + " DWARD = '" + cboWard.Text.Trim() + "',";
                    SQL += ComNum.VBLF + " WRITEDATE = SYSDATE, ";
                    SQL += ComNum.VBLF + " WRITESABUN = " + clsType.User.Sabun;
                    SQL += ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
                }
                else
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.NUR_DNR( ";
                    SQL += ComNum.VBLF + " IPDNO, PANO, DDATE, DWARD,  ";
                    SQL += ComNum.VBLF + " DSABUN, REMARK, WRITEDATE, WRITESABUN) VALUES ( ";
                    SQL += ComNum.VBLF + strIPDNO + ", '" + txtPano.Text.Trim() + "', TO_DATE('" + txtDdate.Text.Trim() + "', 'YYYY-MM-DD'), '" + cboWard.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + txtDsabun.Text.Trim() + ", '" + txtRemark.Text.Trim() + "', SYSDATE, " + clsType.User.Sabun + ") ";
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

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                MessageBox.Show("저장되었습니다.");
                btnExit.PerformClick();

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

        }

        private void eGetData(string argIPDNO, string argPano = "")
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            //fstrIPDNO = "";

            try
            {
                SQL = " SELECT A.IPDNO, A.PANO, (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PANO AND ROWNUM = 1) SNAME, ";
                SQL += ComNum.VBLF + " TO_CHAR(DDATE, 'YYYY-MM-DD') DDATE, A.DWARD,  ";
                SQL += ComNum.VBLF + " A.DSABUN, (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST WHERE SABUN3 = A.DSABUN AND ROWNUM = 1) KORNAME, A.REMARK ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_DNR A ";
                SQL += ComNum.VBLF + "  WHERE IPDNO = " + argIPDNO;
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    txtPano.Text = dt.Rows[0]["PANO"].ToString().Trim();
                    txtSname.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    cboWard.Text = dt.Rows[0]["DWARD"].ToString().Trim();
                    txtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                    txtDdate.Text = dt.Rows[0]["DDATE"].ToString().Trim();
                    txtDsabun.Text = dt.Rows[0]["DSABUN"].ToString().Trim();
                    txtDname.Text = dt.Rows[0]["KORNAME"].ToString().Trim();
                    fstrIPDNO = argIPDNO;
                }
                else
                {
                    //fstrIPDNO = "";
                    ComFunc cf = new ComFunc();
                    txtPano.Text = argPano;
                    txtSname.Text = cf.Read_Patient(clsDB.DbCon, txtPano.Text.Trim(), "2");
                    txtDsabun.Text = (Int32.Parse(clsType.User.Sabun)).ToString();
                    txtDname.Text = cf.Read_SabunName(clsDB.DbCon, txtDsabun.Text.Trim());
                    cf = null;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            Cursor.Current = Cursors.Default;

            return;

        }


        private string ReadWard(string argIPDNO)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strRtn = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT WARDCODE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "  WHERE IPDNO = " + argIPDNO;
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    fstrIPDNO = "";
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    strRtn = dt.Rows[0]["WARDCODE"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            Cursor.Current = Cursors.Default;

            return strRtn;

        }

    }
}
