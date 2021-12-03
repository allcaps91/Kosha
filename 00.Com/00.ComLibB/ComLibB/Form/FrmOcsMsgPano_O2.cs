using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComLibB
{
    public partial class FrmOcsMsgPano_O2 : Form
    {
        public delegate void REventExit(object sender, EventArgs e);
        public static event REventExit rEventExit;

        ComFunc CF = new ComFunc();
        
        frmSpecialText frmSpecialTextX = null;
        frmOcsMsgPanoHyperlipidemia frmOcsMsgPanoHyperlipidemiaX = null;
        frmOcsMsgPanoLiver frmOcsMsgPanoLiverX = null;
        frmOcsMsgPanoO4Build frmOcsMsgPanoO4BuildX = null;

        string FGstrROWID = "";
        string FstrPANO = "";
        string FstrSNAME = "";
        double FdblWRTNO = 0;
        string FstrJOB = "";
        string GstrHelpCode = "";
        string gstrPANO = "";
        string GstrSaBun = "";

        public FrmOcsMsgPano_O2()
        {
            InitializeComponent();
        }

        public FrmOcsMsgPano_O2(string strHelpCode, string sabun, string strPano, string SuChk)
        {
            InitializeComponent();

            GstrHelpCode = strHelpCode;
            gstrPANO = strPano;
            GstrSaBun = clsType.User.IdNumber;
        }

        public FrmOcsMsgPano_O2(string strHelpCode)
        {
            InitializeComponent();

            GstrHelpCode = strHelpCode;
            GstrSaBun = clsType.User.IdNumber;
        }

        public FrmOcsMsgPano_O2(string strHelpCode, string strPano)
        {
            InitializeComponent();

            GstrHelpCode = strHelpCode;
            gstrPANO = strPano;
            GstrSaBun = clsType.User.IdNumber;
        }

        private void SCREEN_CLEAR()
        {
            txtInfoJin.Text = "";
            txtInfoSim.Text = "";
            txtPano.Text = "";
            txtName.Text = "";
            FGstrROWID = "";

            btnSave.Enabled = false;
            btnDelete.Enabled = false;

            panMain.Enabled = false;
        }

        private void FrmOcsMsgPano_O2_Load(object sender, EventArgs e)
        {
            FstrJOB = GstrHelpCode;

            if (FstrJOB == "고지혈")
            {
                this.Text = "심사과 환자메세지등록(외래환자)- 고지혈증";
                lblTitle.Text = "심사과 환자메세지등록(외래환자)- 고지혈증";
                lblTitleSub0.Text = "심사과 환자메세지등록(외래환자)- 고지혈증";
                btnGoji.Visible = true;
                btnGanjang.Visible = false;
                btnChime.Visible = false;
                lblInfo.Text = "고지혈증";
            }
            else if (FstrJOB == "간장용제")
            {
                this.Text = "심사과 환자메세지등록(외래환자)- 간장용제";
                lblTitle.Text = "심사과 환자메세지등록(외래환자)- 간장용제";
                lblTitleSub0.Text = "심사과 환자메세지등록(외래환자)- 간장용제";
                btnGoji.Visible = false;
                btnGanjang.Visible = true;
                btnChime.Visible = false;
                lblInfo.Text = "간장용제";
            }
            else if (FstrJOB == "항암제")
            {
                this.Text = "심사과 환자메세지등록(외래환자)- 항암제";
                lblTitle.Text = "심사과 환자메세지등록(외래환자)- 항암제";
                lblTitleSub0.Text = "심사과 환자메세지등록(외래환자)- 항암제";
                btnGoji.Visible = false;
                btnGanjang.Visible = false;
                btnChime.Visible = false;
                lblInfo.Text = "항암제";
            }
            else if (FstrJOB == "치매약제")
            {
                this.Text = "심사과 환자메세지등록(외래환자)- 치매약제";
                lblTitle.Text = "심사과 환자메세지등록(외래환자)- 치매약제";
                lblTitleSub0.Text = "심사과 환자메세지등록(외래환자)- 치매약제";
                btnGoji.Visible = false;
                btnGanjang.Visible = false;
                btnChime.Visible = true;
                lblInfo.Text = "치매약제";
            }
            else
            {
                this.Text = "심사과 환자메세지등록(외래환자)- 진료과용";
                lblTitle.Text = "심사과 환자메세지등록(외래환자)- 진료과용";
                lblTitleSub0.Text = "심사과 환자메세지등록(외래환자)- 진료과용";
                btnGoji.Visible = false;
                btnGanjang.Visible = false;
                btnChime.Visible = false;
                lblInfo.Text = "진료과용";
            }

            SCREEN_CLEAR();
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept);
            setCboSabun(cboSabun);

            if (gstrPANO != "")
            {
                txtPano.Text = gstrPANO;
                viewData();
            }
        }

        private void setCboSabun(ComboBox cbo)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT SABUN, KORNAME  FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE TOIDAY IS NULL ";     //'재직자
                SQL = SQL + ComNum.VBLF + "   AND BUSE ='078201' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboSabun.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    cboSabun.Items.Add("******.전체");
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboSabun.Items.Add(dt.Rows[i]["SABUN"].ToString().Trim() + "." + dt.Rows[i]["KORNAME"].ToString().Trim());
                    }
                }
                cboSabun.SelectedIndex = 0;

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

        private void btnSelect_Click(object sender, EventArgs e) 
        {
            DialogResult dr = this.fontDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtInfoJin.SelectionFont = fontDialog1.Font;
                //txtInfoJin.SelectionFont = new System.Drawing.Font("맑은 고딕", 14); 
                txtInfoJin.SelectionColor = fontDialog1.Color;
            }

            txtInfoJin.Focus();

         
        }

        private void btnSpecial_Click(object sender, EventArgs e)
        {
            if (frmSpecialTextX != null)
            {
                frmSpecialTextX.Dispose();
                frmSpecialTextX = null;
            }
            frmSpecialTextX = new frmSpecialText();
            frmSpecialTextX.rSendText += new frmSpecialText.SendText(GetText);
            frmSpecialTextX.rEventExit += new frmSpecialText.EventExit(frmSpecialTextX_rEventExit);
            frmSpecialTextX.Show();
        }

        private void frmSpecialTextX_rEventExit()
        {
            frmSpecialTextX.Dispose();
            frmSpecialTextX = null;
        }

        private void GetText(string strText)
        {
            txtInfoJin.Text += strText;
        }


        private void btnResult_Click(object sender, EventArgs e)
        {
            if (FstrPANO == "") return;
            GstrHelpCode = FstrPANO;

            frmViewResult frmViewResultX = new frmViewResult(GstrHelpCode);
            frmViewResultX.Show();
        }

        private void btnOutData_Click(object sender, EventArgs e)
        {
            if (FstrPANO == "") return;
            GstrHelpCode = FstrPANO;

            frmSlipView frmSlipView = new frmSlipView(GstrHelpCode);
            frmSlipView.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            viewData();
        }

        private void viewData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT A.PANO APANO , A.SNAME,  B.PANO BPANO, A.BI,B.DEPTCODE,B.MEMO,TO_CHAR(B.SDATE,'YYYY-MM-DD') SDATE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT A, KOSMOS_PMPA.BAS_OCSMEMO_O2 B ";
                if (rdoRegPerson.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO";
                    SQL = SQL + ComNum.VBLF + "   AND B.DDATE IS  NULL  ";
                }

                if (rdoDelPerson.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE A.PANO(+) =  B.PANO";
                    SQL = SQL + ComNum.VBLF + "   AND B.DDATE IS NOT NULL  ";
                }

                if (cboSabun.SelectedIndex != 0) SQL = SQL + ComNum.VBLF + "  AND B.ENTSABUN = '" + cboSabun.Text.Split('.')[0] + "' ";

                if (VB.Trim(txtPano.Text) != "") SQL = SQL + ComNum.VBLF + " AND A.PANO = '" + txtPano.Text + "' ";

                if (FstrJOB == "고지혈")
                { SQL = SQL + ComNum.VBLF + "   AND B.GBJOB = '2'  "; }
                else if (FstrJOB == "간장용제")
                { SQL = SQL + ComNum.VBLF + "   AND B.GBJOB = '3'  "; }
                else if (FstrJOB == "항암제")
                { SQL = SQL + ComNum.VBLF + "   AND B.GBJOB = '4'  "; }
                else
                { SQL = SQL + ComNum.VBLF + "   AND B.GBJOB = '1'  "; }

                if (rdoNum.Checked == true) SQL = SQL + ComNum.VBLF + " ORDER BY A.PANO, A.SNAME,   B.PANO ";
                if (rdoName.Checked == true) SQL = SQL + ComNum.VBLF + " ORDER BY A.SNAME, A.PANO,   B.PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssView_Sheet1.RowCount = 0;
                ssMsg_Sheet1.RowCount = 0;

                if (dt.Rows.Count == 0)
                {
                    btnSave.Enabled = true;
                    txtInfoJin.Text = "";
                    panMain.Enabled = true;
                    FstrPANO = txtPano.Text;
                    FstrSNAME = txtName.Text;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    txtInfoJin.Text = "";
                    txtInfoSim.Text = "";
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["bi"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["APANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SDate"].ToString().Trim();
                        if (dt.Rows[i]["BPANO"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 233, 233);
                        }
                    }
                }
                 
                dt.Dispose();
                dt = null;

                ssView.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssView.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (string.IsNullOrEmpty(FstrPANO) || string.IsNullOrEmpty(FstrSNAME))
            {
                txtPano.Focus();
                ComFunc.MsgBox("등록번호를 확인해주세요.");
                return;
            }


            saveData();
        }

        private bool saveData()
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strData = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                strData = VB.Replace(txtInfoJin.Rtf, "'", "`");

                if (FGstrROWID == "")
                {
                    SQL = " SELECT MAX(WRTNO) MWRTNO FROM KOSMOS_PMPA.BAS_OCSMEMO_O2 ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                    }



                    FdblWRTNO = VB.Val(dt.Rows[0]["MWRTNO"].ToString().Trim()) + 1;
                    dt.Dispose();
                    dt = null;

                    SQL = " INSERT INTO KOSMOS_PMPA.BAS_OCSMEMO_O2 (  PANO, SNAME, MEMO, SDATE, DDATE, WRTNO, GBJOB, MEMO2 , ENTSABUN, DEPTCODE ) VALUES (";
                    SQL = SQL + ComNum.VBLF + " '" + FstrPANO + "', '" + FstrSNAME + "', :MEMO, TRUNC(SYSDATE), '' , '" + FdblWRTNO + "', ";

                    if (FstrJOB == "고지혈")
                    {
                        SQL = SQL + ComNum.VBLF + "    '2'  ";
                    }
                    else if (FstrJOB == "간장용제")
                    {
                        SQL = SQL + ComNum.VBLF + "    '3'  ";
                    }
                    else if (FstrJOB == "항암제")
                    {
                        SQL = SQL + ComNum.VBLF + "    '4'  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    '1'  ";
                    }
                    SQL = SQL + ComNum.VBLF + ", '" + txtInfoSim.Text + "', " + clsType.User.Sabun + ",  '" + VB.Left(cboDept.Text, 2) + "' ) ";

                    SqlErr = clsDB.ExecuteLongQuery(SQL, txtInfoJin.Rtf, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }

                    
                    //SQL = "UPDATE KOSMOS_PMPA.BAS_OCSMEMO_O2 SET ";
                    //SQL = SQL + ComNum.VBLF + "  MEMO = :MEMO ";
                    //SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPANO + "' ";
                    //SQL = SQL + ComNum.VBLF + "   AND WRTNO = '" + FdblWRTNO + "' ";

                    //PsmhDb pDbCon = clsDB.DbCon;
                    //OracleCommand cmd = new OracleCommand(SQL, pDbCon.Con);

                    //cmd.Parameters.Add("MEMO", txtInfoJin.Rtf);
                    //cmd.ExecuteNonQuery();



                    SQL = " SELECT ROWID FROM KOSMOS_PMPA.BAS_OCSMEMO_O2 WHERE PANO = '" + FstrPANO + "'  AND WRTNO = '" + FdblWRTNO + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                    }

                    FGstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }
                else
                {
                    SQL = " UPDATE KOSMOS_PMPA.BAS_OCSMEMO_O2";
                    SQL = SQL + ComNum.VBLF + " SET";
                    SQL = SQL + ComNum.VBLF + " MEMO2 = '" + txtInfoSim.Text + "',";
                    SQL = SQL + ComNum.VBLF + " MEMO = :MEMO,";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN = " + GstrSaBun + ",";
                    SQL = SQL + ComNum.VBLF + " DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrROWID + "' ";

                    SqlErr = clsDB.ExecuteLongQuery(SQL, txtInfoJin.Rtf, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }

                    //SQL = "UPDATE KOSMOS_PMPA.BAS_OCSMEMO_O2 SET ";
                    //SQL = SQL + ComNum.VBLF + "  MEMO = :MEMO ";
                    //SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrROWID + "' ";

                    //PsmhDb pDbCon = clsDB.DbCon;
                    //OracleCommand cmd = new OracleCommand(SQL, pDbCon.Con);

                    //cmd.Parameters.Add("MEMO", txtInfoJin.Rtf);
                    //cmd.ExecuteNonQuery();                    
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                ssMsg_Sheet1.RowCount = 0;
                btnCancelClick();
                btnNew.Enabled = false;

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


        private void btnGoji_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //닫는 이벤트 내용
            if (frmOcsMsgPanoHyperlipidemiaX != null)
            {
                frmOcsMsgPanoHyperlipidemiaX.Dispose();
                frmOcsMsgPanoHyperlipidemiaX = null;
            }
            frmOcsMsgPanoHyperlipidemiaX = new frmOcsMsgPanoHyperlipidemia();
            frmOcsMsgPanoHyperlipidemiaX.rSetHelpCode += frmOcsMsgPanoHyperlipidemiaX_rSetHelpCode;
            frmOcsMsgPanoHyperlipidemiaX.rEventClose += frmOcsMsgPanoHyperlipidemiaX_rEventClose;
            frmOcsMsgPanoHyperlipidemiaX.ShowDialog();

            if (GstrHelpCode != "")
            {
                try
                {

                    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                    SCREEN_CLEAR();

                    SQL = " SELECT ROWID FROM KOSMOS_PMPA.BAS_OCSMEMO_O2 ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + GstrHelpCode + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND GBJOB ='2'  "; //'고지혈증

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
                        Cursor.Current = Cursors.Default;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        rdoRegPerson.Checked = true;
                    }
                    else
                    {
                        rdoDelPerson.Checked = true;
                    }
                    dt.Dispose();
                    dt = null;

                    txtPano.Text = GstrHelpCode;

                    txtPanoKeyDown(Keys.Enter);
                    viewData();
                    ssViewCellClick(0, 0);
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

        private void frmOcsMsgPanoHyperlipidemiaX_rSetHelpCode(string strHelpCode)
        {
            if (strHelpCode != "")
            {
                GstrHelpCode = strHelpCode;
            }
        }

        private void frmOcsMsgPanoHyperlipidemiaX_rEventClose()
        {
            //닫는 이벤트 내용
            if (frmOcsMsgPanoHyperlipidemiaX != null)
            {
                frmOcsMsgPanoHyperlipidemiaX.Dispose();
                frmOcsMsgPanoHyperlipidemiaX = null;
            }
        }
        
        private void btnGanjang_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            
            //닫는 이벤트 내용
            if (frmOcsMsgPanoLiverX != null)
            {
                frmOcsMsgPanoLiverX.Dispose();
                frmOcsMsgPanoLiverX = null;
            }
            frmOcsMsgPanoLiverX = new frmOcsMsgPanoLiver();
            frmOcsMsgPanoLiverX.rSetHelpCode += frmOcsMsgPanoLiverX_rSetHelpCode;
            frmOcsMsgPanoLiverX.rEventClose += frmOcsMsgPanoLiverX_rEventClose;
            frmOcsMsgPanoLiverX.ShowDialog();
            
            if (GstrHelpCode != "")
            {
                try
                {

                    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                    SCREEN_CLEAR();

                    SQL = " SELECT ROWID FROM KOSMOS_PMPA.BAS_OCSMEMO_O2 ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + GstrHelpCode + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND GBJOB ='3'  "; //'간장용제

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
                        Cursor.Current = Cursors.Default;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        rdoRegPerson.Checked = true;
                    }
                    else
                    {
                        rdoDelPerson.Checked = true;
                    }
                    dt.Dispose();
                    dt = null;

                    txtPano.Text = GstrHelpCode;

                    txtPanoKeyDown(Keys.Enter);
                    viewData();
                    ssViewCellClick(0, 0);
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

        private void frmOcsMsgPanoLiverX_rEventClose()
        {
            //닫는 이벤트 내용
            if (frmOcsMsgPanoLiverX != null)
            {
                frmOcsMsgPanoLiverX.Dispose();
                frmOcsMsgPanoLiverX = null;
            }
        }

        private void frmOcsMsgPanoLiverX_rSetHelpCode(string strHelpCode)
        {
            
            if (strHelpCode != "")
            {
                GstrHelpCode = strHelpCode;
            }
        }

        private void btnChime_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            GstrHelpCode = "";
                 
            //닫는 이벤트 내용
            if (frmOcsMsgPanoO4BuildX != null)
            {
                frmOcsMsgPanoO4BuildX.Dispose();
                frmOcsMsgPanoO4BuildX = null;
            }
            frmOcsMsgPanoO4BuildX = new frmOcsMsgPanoO4Build();            
            frmOcsMsgPanoO4BuildX.Show();

            
            if (GstrHelpCode != "")
            {
                try
                {

                    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                    SCREEN_CLEAR();

                    SQL = " SELECT ROWID FROM KOSMOS_PMPA.BAS_OCSMEMO_O2 ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + GstrHelpCode + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND GBJOB ='5'  "; //'치매약제

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
                        Cursor.Current = Cursors.Default;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        rdoRegPerson.Checked = true;
                    }
                    else
                    {
                        rdoDelPerson.Checked = true;
                    }
                    dt.Dispose();
                    dt = null;

                    txtPano.Text = GstrHelpCode;

                    txtPanoKeyDown(Keys.Enter);
                    viewData();
                    ssViewCellClick(0, 0);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancelClick();
        }

        private void btnCancelClick()
        {
            SCREEN_CLEAR();
            btnNew.Enabled = false;
            panMain.Enabled = false;
            ssMsg_Sheet1.RowCount = 0;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnDeleteClick();
        }

        private bool btnDeleteClick()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (FGstrROWID == "")
            {
                ComFunc.MsgBox("전산오류입니다.", "확인");
                return rtVal;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                SQL = " UPDATE KOSMOS_PMPA.BAS_OCSMEMO_O2  SET DDATE = TRUNC(SYSDATE) WHERE ROWID = '" + FGstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제 하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                panMain.Enabled = false;

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (rEventExit != null)
            {
                rEventExit(sender, e);
            }

            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            btnNewClick();
        }

        private void btnNewClick()
        {
            SCREEN_CLEAR();
            btnSave.Enabled = true;
            panMain.Enabled = true;
            txtInfoJin.Focus();
        }

        private void rdoView_CheckedChanged(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            txtPanoKeyDown(e.KeyData);
        }

        private void txtPanoKeyDown(Keys keyData)
        {
            if (keyData != Keys.Enter) return;

            btnSearch.Focus();
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (txtPano.Text == "")
            {
                txtName.Text = "";
                return;
            }

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT PANO , SNAME FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";

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
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                }
                else
                {
                    txtPano.Text = "";
                    txtName.Text = "";
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

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssViewCellClick(e.Row, e.Column);
        }

        private void ssViewCellClick(int iRow, int iCol)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            FstrPANO = ssView_Sheet1.Cells[iRow, 1].Text;
            FstrSNAME = ssView_Sheet1.Cells[iRow, 2].Text;

            if (FstrPANO == "") return;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT PANO, SNAME, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(DDATE,'YYYY-MM-DD') DDATE,";
                SQL = SQL + ComNum.VBLF + " WRTNO, ROWID";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_OCSMEMO_O2 ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + FstrPANO + "'";
                if (FstrJOB == "고지혈")
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBJOB = '2'  ";
                }
                else if (FstrJOB == "간장용제")
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBJOB = '3'  ";
                }
                else if (FstrJOB == "항암제")
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBJOB = '4'  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBJOB = '1'  ";
                }

                if (cboSabun.SelectedIndex != 0)
                {
                    SQL = SQL + ComNum.VBLF + "  AND ENTSABUN = '" + cboSabun.Text.Split('.')[0] + "' ";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY SDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssMsg_Sheet1.RowCount = 0;
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    btnNewClick();
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssMsg_Sheet1.RowCount = dt.Rows.Count;
                    ssMsg_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssMsg_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                        ssMsg_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DDATE"].ToString().Trim();
                        ssMsg_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssMsg_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssMsg_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                        ssMsg_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }
                //FGstrROWID = ssMsg_Sheet1.Cells[i, 5].Text.Trim();
                FGstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                dt.Dispose();
                dt = null;
                GetMemo();

                btnNew.Enabled = true;
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

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssView)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
            }

            if (e.Row >= 0 && e.Column == 4)
            {
                gstrPANO = ssView_Sheet1.Cells[e.Row, 1].Text;
            }

            frmSlipView frmSlipViewX = new frmSlipView(gstrPANO);
            frmSlipViewX.Show();

            gstrPANO = "";
            GstrHelpCode = "";
        }

        private void ssMsg_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssMsg_Sheet1.RowCount == 0) return;

           // FGstrROWID = ssMsg_Sheet1.Cells[e.Row, 5].Text;
            if (FGstrROWID == "") return;

            if (e.RowHeader == true)
            {
                if (ComFunc.MsgBoxQ("완전삭제 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

                deleteData();
                btnCancelClick();
            }

           // GetMemo();
        }


        void GetMemo()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT MEMO, MEMO2 , DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_OCSMEMO_O2 ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FGstrROWID + "' ";

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
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    txtInfoJin.Rtf = VB.Replace(dt.Rows[0]["MEMO"].ToString().Trim(), "`", "'");
                    txtInfoSim.Text = dt.Rows[0]["MEMO2"].ToString().Trim();
                    cboDept.Text = dt.Rows[0]["DeptCode"].ToString().Trim() + "." + CF.READ_DEPTNAMEK(clsDB.DbCon, dt.Rows[0]["DeptCode"].ToString().Trim());
                }

                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                panMain.Enabled = true;

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


        private bool deleteData()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                SQL = " DELETE KOSMOS_PMPA.BAS_OCSMEMO_O2 WHERE ROWID = '" + FGstrROWID + "'       ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
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
    }
}
