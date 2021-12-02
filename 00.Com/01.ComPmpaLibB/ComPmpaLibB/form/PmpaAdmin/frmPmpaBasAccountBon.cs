using ComBase; //기본 클래스using System;
using ComDbB;
using System.Data;
using System.Windows.Forms;
using System;
using System.Threading;
using FarPoint.Win.Spread;
using System.Drawing;

namespace ComPmpaLibB
{
    public partial class frmPmpaBasAccountBon : Form
    {
        clsSpread cSP = new clsSpread();
        clsComPmpaSpd cCPS = new clsComPmpaSpd();
        clsPmpaFunc cPF = new clsPmpaFunc();
        clsPmpaPb cPB = new clsPmpaPb();
        ComFunc CF = new ComFunc();

        int FnRow = 0;
        int FnCol = 0;

        class cBasAcctBon
        {
            public string Bi = string.Empty;
            public string IO = string.Empty;
            public string Child = string.Empty;
            public string MCode = string.Empty;
            public string VCode = string.Empty;
            public string Dept = string.Empty;
            public string HC = string.Empty;
            public string FCode = string.Empty;
            public bool All = false;
        }

        cBasAcctBon cAB = new cBasAcctBon();

        Thread thread;
        FpSpread spd;

        public frmPmpaBasAccountBon()
        {
            InitializeComponent();
            SetEvent();
            Set_Combo();
        }

        private void SetEvent()
        {
            this.dtpSDate.ValueChanged  += new EventHandler(CF.eDtpFormatSet);
            this.dtpEndDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);
            this.dtpDelDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);
            this.cboHC.MouseWheel       += new MouseEventHandler(eCboWheel);
            this.cboHC2.MouseWheel      += new MouseEventHandler(eCboWheel);
            this.cboFCode.MouseWheel    += new MouseEventHandler(eCboWheel);
            this.cboFCode2.MouseWheel   += new MouseEventHandler(eCboWheel);
            this.cboBi.MouseWheel       += new MouseEventHandler(eCboWheel);
            this.cboBi2.MouseWheel      += new MouseEventHandler(eCboWheel);
            this.cboIO.MouseWheel       += new MouseEventHandler(eCboWheel);
            this.cboMCode.MouseWheel    += new MouseEventHandler(eCboWheel);
            this.cboMCode2.MouseWheel   += new MouseEventHandler(eCboWheel);
            this.cboVCode.MouseWheel    += new MouseEventHandler(eCboWheel);
            this.cboVCode2.MouseWheel   += new MouseEventHandler(eCboWheel);
            this.cboDept.MouseWheel     += new MouseEventHandler(eCboWheel);
            this.cboDept2.MouseWheel    += new MouseEventHandler(eCboWheel);
            this.cboGbChild.MouseWheel  += new MouseEventHandler(eCboWheel);
            this.cboGbChild2.MouseWheel += new MouseEventHandler(eCboWheel);

            this.SS1.KeyPress           += new KeyPressEventHandler(eSpdKeyPress);
            this.SS1.LeaveCell          += new LeaveCellEventHandler(eSpdLeaveCell);
            this.SS1.CellClick          += new CellClickEventHandler(eSpdClick);
            this.SS1.CellDoubleClick    += new CellClickEventHandler(eSpdDblClick);
            this.SS1.KeyDown            += new KeyEventHandler(eSpdKeyDown);

            this.btnNew.Click           += new EventHandler(eBtnClick);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.btnSave2.Click         += new EventHandler(eBtnClick);
            this.btnCancel.Click        += new EventHandler(eBtnClick);
            
            this.dtpSDate.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.dtpEndDate.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.cboIO.KeyPress         += new KeyPressEventHandler(eKeyPress);
            this.cboBi.KeyPress         += new KeyPressEventHandler(eKeyPress);
            this.cboMCode.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.cboGbChild.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.cboVCode.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.cboHC.KeyPress         += new KeyPressEventHandler(eKeyPress);
            this.cboFCode.KeyPress      += new KeyPressEventHandler(eKeyPress);            
            this.txtJin.KeyPress        += new KeyPressEventHandler(eKeyPress);
            this.txtBohum.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.txtCTMRI.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.txtFood.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.txtDt1.KeyPress        += new KeyPressEventHandler(eKeyPress);
            this.txtDt2.KeyPress        += new KeyPressEventHandler(eKeyPress);
            this.txtFAmt1.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.txtFAmt2.KeyPress      += new KeyPressEventHandler(eKeyPress);

            this.txtJin.GotFocus        += new EventHandler(eSelStart);
            this.txtBohum.GotFocus      += new EventHandler(eSelStart);
            this.txtCTMRI.GotFocus      += new EventHandler(eSelStart);
            this.txtFood.GotFocus       += new EventHandler(eSelStart);
            this.txtDt1.GotFocus        += new EventHandler(eSelStart);
            this.txtDt2.GotFocus        += new EventHandler(eSelStart);
            this.txtFAmt1.GotFocus      += new EventHandler(eSelStart);
            this.txtFAmt2.GotFocus      += new EventHandler(eSelStart);

            this.txtJin.Click           += new EventHandler(eSelStart);
            this.txtBohum.Click         += new EventHandler(eSelStart);
            this.txtCTMRI.Click         += new EventHandler(eSelStart);
            this.txtFood.Click          += new EventHandler(eSelStart);
            this.txtDt1.Click           += new EventHandler(eSelStart);
            this.txtDt2.Click           += new EventHandler(eSelStart);
            this.txtFAmt1.Click         += new EventHandler(eSelStart);
            this.txtFAmt2.Click         += new EventHandler(eSelStart);

            this.swtBtn.ValueChanged    += new EventHandler(eBtnClick);
            
        }

        void eSpdKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Shift || e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.Insert)
                {
                    Clipboard.Clear();

                    if (sender == SS1)
                    {
                        cSP.setDel_Ins(SS1, true);
                    }
                }
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnNew)
            {
                Set_New();
            }
            else if (sender == btnExit)
            {
                this.Close();
            }
            else if (sender == btnSearch)
            {
                Search_Data();
            }
            else if (sender == swtBtn)
            {
                Search_Data();
            }
            else if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnSave2)
            {
                eSave(clsDB.DbCon);
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
        }

        void eSave(PsmhDb pDbCon)
        {
            int i = 0;
            string strIO = string.Empty;
            string strRowid = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

            ComFunc.ReadSysDate(pDbCon);

            try
            {
                clsDB.setBeginTran(pDbCon);

                for (i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strRowid = SS1.ActiveSheet.Cells[i, (int)clsPmpaPb.enmBasAcctBon.ROWID].Text.Trim();

                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        if (Data_Delete(strRowid) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox((i + 1).ToString() + "번째 줄 삭제 시 오류발생");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        if (eDataSave(pDbCon, strRowid, i) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox((i + 1).ToString() + "번째 줄 저장 시 오류발생");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(pDbCon);

                ComFunc.MsgBox("작업완료.");

                Screen_Clear();

                if (swtBtn.Value == true)
                {
                    strIO = "O";
                }
                else
                {
                    strIO = "I";
                }

                Screen_Display(strIO);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (SS1.ActiveSheet.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                cSP.setSpdSort(SS1, e.Column, true);
                return;
            }
            else
            {
                //셀 시작 Row 체크
                FnRow = e.Row;
                FnCol = e.Column;
            }
        }

        void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            int i = 0;

            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            if (SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.ROWID].Text == "")
            {
                return;
            }

            if (SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.DELDATE].Text != "")
            {
                return;
            }

            #region Control Enable
            dtpSDate.Enabled = true;
            dtpEndDate.Enabled = true;
            cboIO.Enabled = true;
            cboBi.Enabled = true;
            cboMCode.Enabled = true;
            cboGbChild.Enabled = true;
            cboVCode.Enabled = true;
            cboDept.Enabled = true;
            cboHC.Enabled = true;
            cboFCode.Enabled = true;
            txtJin.Enabled = true;
            txtBohum.Enabled = true;
            txtCTMRI.Enabled = true;
            txtFood.Enabled = true;
            txtDt1.Enabled = true;
            txtDt2.Enabled = true;
            txtFAmt1.Enabled = true;
            txtFAmt2.Enabled = true;
            #endregion

            lblWrtno.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.WRTNO].Text;
            
            if (SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.SDATE].Text.Trim() == "")
            {
                CF.dtpClear(dtpSDate);
            }
            else
            {
                dtpSDate.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.SDATE].Text;
            }

            if (SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.EDATE].Text.Trim() == "")
            {
                CF.dtpClear(dtpEndDate);
            }
            else
            {
                dtpEndDate.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.EDATE].Text;
            }

            if (SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.DELDATE].Text.Trim() == "")
            {
                CF.dtpClear(dtpDelDate);
            }
            else
            {
                dtpDelDate.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.DELDATE].Text;
            }

            for (i = 0; i < cboIO.Items.Count; i++)
            {
                if (VB.Left(cboIO.Items[i].ToString(), 1) == SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.GBIO].Text)
                {
                    cboIO.SelectedIndex = i;
                    break;
                }
            }

            for (i = 0; i < cboBi.Items.Count; i++)
            {
                if (VB.Left(cboBi.Items[i].ToString(), 2) == VB.Left(SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.BI].Text, 2))
                {
                    cboBi.SelectedIndex = i;
                    break;
                }
            }

            for (i = 0; i < cboMCode.Items.Count; i++)
            {
                if (VB.Left(cboMCode.Items[i].ToString(), 4) == SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.MCODE].Text)
                {
                    cboMCode.SelectedIndex = i;
                    break;
                }
            }

            for (i = 0; i < cboDept.Items.Count; i++)
            {
                if (VB.Left(cboDept.Items[i].ToString(), 2) == SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.DEPT].Text)
                {
                    cboDept.SelectedIndex = i;
                    break;
                }
            }

            for (i = 0; i < cboVCode.Items.Count; i++)
            {
                if (VB.Left(cboVCode.Items[i].ToString(), 4) == SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.VCODE].Text)
                {
                    cboVCode.SelectedIndex = i;
                    break;
                }
            }
            
            cboGbChild.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.GBCHILD].Text;
            cboHC.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.HC].Text;
            cboFCode.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.FCODE].Text;
            txtJin.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.JIN].Text;
            txtBohum.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.BOHUM].Text;
            txtCTMRI.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.CTMRI].Text;
            txtFood.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.FOOD].Text;
            txtDt1.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.DT1].Text;
            txtDt2.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.DT2].Text;
            txtFAmt1.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.FAMT1].Text;
            txtFAmt2.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.FAMT2].Text;
            txtRowid.Text = SS1_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.ROWID].Text;
        }

        void eSpdKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                cSP.setEnterKey((FpSpread)sender, clsSpread.enmSpdEnterKey.Right);
            }
            else if (e.KeyChar == (char)Keys.Down)
            {
                cSP.setEnterKey((FpSpread)sender, clsSpread.enmSpdEnterKey.Down);
            }
        }

        void eSpdLeaveCell(object sender, LeaveCellEventArgs e)
        {
            if (e.Column == (int)clsPmpaPb.enmBasAcctBon.DEPT)
            {
                SS1.ActiveSheet.Cells[e.Row, e.Column].Text = SS1.ActiveSheet.Cells[e.Row, e.Column].Text.ToUpper();
            }
            else if (e.Column == (int)clsPmpaPb.enmBasAcctBon.VCODE)
            {
                SS1.ActiveSheet.Cells[e.Row, e.Column].Text = SS1.ActiveSheet.Cells[e.Row, e.Column].Text.ToUpper();
            }
            else if (e.Column == (int)clsPmpaPb.enmBasAcctBon.MCODE)
            {
                SS1.ActiveSheet.Cells[e.Row, e.Column].Text = SS1.ActiveSheet.Cells[e.Row, e.Column].Text.ToUpper();
            }

            if (SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.MCODE].Text.Trim() != "")
            {
                SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.MCODE_NAME].Text = Read_MCode_Name(clsDB.DbCon, SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.MCODE].Text.Trim());
            }

            if (SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.VCODE].Text.Trim() != "")
            {
                SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.VCODE_NAME].Text = Read_VCode_Name(clsDB.DbCon, SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.VCODE].Text.Trim());
            }

            if (SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.GBCHILD].Text.Trim() != "")
            {
                string strChild = SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.GBCHILD].Text.Trim();

                if (strChild != "0" && strChild != "1" && strChild != "2" && strChild != "3" && strChild != "4" &&  strChild != "5")
                {
                    SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.GBCHILD].Text = "0";
                }

                SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.CHILDNAME].Text = Read_Child_Name(SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.GBCHILD].Text.Trim());
            }
            
            if (SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.ROWID].Text.Trim() == "" && SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.BI].Text.Trim() != "")
            {
                SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmBasAcctBon.WRTNO].Text = "신규";
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpSDate && e.KeyChar == (char)13) { cboIO.Focus(); }
            if (sender == this.cboIO && e.KeyChar == (char)13) { cboBi.Focus(); }
            if (sender == this.cboBi && e.KeyChar == (char)13) { cboMCode.Focus(); }
            if (sender == this.cboMCode && e.KeyChar == (char)13) { cboGbChild.Focus(); }
            if (sender == this.cboGbChild && e.KeyChar == (char)13) { cboVCode.Focus(); }
            if (sender == this.cboVCode && e.KeyChar == (char)13) { cboDept.Focus(); }
            if (sender == this.cboDept && e.KeyChar == (char)13) { cboHC.Focus(); }
            if (sender == this.cboHC && e.KeyChar == (char)13) { cboFCode.Focus(); }
            if (sender == this.cboFCode && e.KeyChar == (char)13) { dtpEndDate.Focus(); }
            if (sender == this.dtpEndDate && e.KeyChar == (char)13) { txtJin.Focus(); }
            if (sender == this.txtJin && e.KeyChar == (char)13) { txtBohum.Focus(); }
            if (sender == this.txtBohum && e.KeyChar == (char)13) { txtCTMRI.Focus(); }
            if (sender == this.txtCTMRI && e.KeyChar == (char)13) { txtFood.Focus(); }
            if (sender == this.txtFood && e.KeyChar == (char)13) { txtDt1.Focus(); }
            if (sender == this.txtDt1 && e.KeyChar == (char)13) { txtDt2.Focus(); }
            if (sender == this.txtDt2 && e.KeyChar == (char)13) { txtFAmt1.Focus(); }
            if (sender == this.txtFAmt1 && e.KeyChar == (char)13) { txtFAmt2.Focus(); }
            if (sender == this.txtFAmt2 && e.KeyChar == (char)13) { btnSave.Focus(); }
        }

        void eSelStart(object sender, EventArgs e)
        {
            TextBox tP = sender as TextBox;
            tP.SelectionStart = 0;
            tP.SelectionLength = tP.Text.Length;
        }

        private void eCboWheel(object sender, MouseEventArgs e)
        {
            ComboBox CB = sender as ComboBox;
           
            if (CB.Focused == false)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        private void frmPmpaBasAccountBon_Load(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpSDate.Text = clsPublic.GstrSysDate;

            Screen_Clear();
            
            cCPS.sSpd_enmBasAcctBon(SS1, cPB.sSpdBasAcctBon, cPB.nSpdBasAcctBon, 10, 0, cPB.ArgV, cPB.ArgY);

            cboBi2.SelectedIndex = 0;
            cboMCode2.SelectedIndex = 0;
            cboGbChild2.SelectedIndex = 0;
            cboVCode2.SelectedIndex = 0;
            cboDept2.SelectedIndex = 0;
            cboHC2.SelectedIndex = 0;
            cboFCode2.SelectedIndex = 0;
        }

        private void Set_Combo()
        {
            DataTable Dt = null;
            int i = 0;

            //BAS_소아면제
            Dt = cPF.sel_Bas_OgPdBun(clsDB.DbCon);
            if (Dt != null)
            {
                cPB.ArgV = new string[Dt.Rows.Count + 1];

                cPB.ArgV[0] = "";
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    cPB.ArgV[i + 1] = Dt.Rows[i]["CODE"].ToString().Trim() + "." + Dt.Rows[i]["NAME"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;
            }

            //FCODE 특정기호
            Dt = cPF.sel_Bas_BCode_FCode(clsDB.DbCon);
            if (Dt != null)
            {
                cPB.ArgY = new string[Dt.Rows.Count + 1];

                cPB.ArgY[0] = "";
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    cPB.ArgY[i + 1] = Dt.Rows[i]["CODE"].ToString().Trim() + "." + Dt.Rows[i]["NAME"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;
            }

            #region 세팅부분
            cboIO.Items.Clear();
            cboIO.Items.Add("O.외래");
            cboIO.Items.Add("I.입원");
                
            CF.Combo_BCode_SET(clsDB.DbCon, cboBi, "BAS_환자종류", true, 1, "");
            cboBi.Items.Remove("12.직장");
            cboBi.Items.Remove("13.지역");
            
            CF.Combo_BCode_SET(clsDB.DbCon, cboMCode, "BAS_의료급여본인부담", true, 1, "");

            cboGbChild.Items.Clear();
            cboGbChild.Items.Add("0.성인");
            cboGbChild.Items.Add("1.신생아");
            cboGbChild.Items.Add("2.6세미만");
            cboGbChild.Items.Add("3.6세~15세");
            cboGbChild.Items.Add("4.65세이상");

            Combo_BCode_SET_VCODE(clsDB.DbCon, cboVCode, false, true, false, true);
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "1", 2);
            CF.Combo_BCode_SET(clsDB.DbCon, cboHC, "BAS_소아면제", true, 1, "");
            CF.Combo_BCode_SET(clsDB.DbCon, cboFCode, "BAS_특정기호", true, 1, "");
            #endregion

            #region 조회부분

            Combo_BCode_SET(clsDB.DbCon, cboBi2, "BAS_환자종류", true, false);
            cboBi2.Items.Remove("12");
            cboBi2.Items.Remove("13");
                        
            Combo_BCode_SET(clsDB.DbCon, cboMCode2, "BAS_의료급여본인부담", true, true);

            cboGbChild2.Items.Clear();
            cboGbChild2.Items.Add("*.전체");
            cboGbChild2.Items.Add("0.성인");
            cboGbChild2.Items.Add("1.신생아");
            cboGbChild2.Items.Add("2.6세미만");
            cboGbChild2.Items.Add("3.6세~15세");
            cboGbChild2.Items.Add("4.65세이상");
            cboGbChild2.Items.Add("5.1세미만");

            Combo_BCode_SET_VCODE(clsDB.DbCon, cboVCode2, true, true, true, false);
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept2, "1", 2);
            Combo_BCode_SET(clsDB.DbCon, cboHC2, "BAS_소아면제", false, true);
            Combo_BCode_SET(clsDB.DbCon, cboFCode2, "BAS_특정기호", false, true);

            #endregion
        }

        private void Screen_Clear()
        {
            lblWrtno.Text = "";
            CF.dtpClear(dtpSDate);
            CF.dtpClear(dtpEndDate);
            CF.dtpClear(dtpDelDate);
            
            cboIO.SelectedIndex = 0;
            cboBi.SelectedIndex = 0;
            cboMCode.SelectedIndex = 0;
            cboGbChild.SelectedIndex = 0;
            cboVCode.SelectedIndex = 0;
            cboDept.SelectedIndex = 0;
            cboHC.SelectedIndex = 0;
            cboFCode.SelectedIndex = 0;

            txtJin.Text = "";
            txtBohum.Text = "";
            txtCTMRI.Text = "";
            txtFood.Text = "";
            txtDt1.Text = "";
            txtDt2.Text = "";
            txtFAmt1.Text = "";
            txtFAmt2.Text = "";
            txtRowid.Text = "";

            dtpSDate.Enabled = false;
            dtpEndDate.Enabled = false;
            cboIO.Enabled = false;
            cboBi.Enabled = false;
            cboMCode.Enabled = false;
            cboGbChild.Enabled = false;
            cboVCode.Enabled = false;
            cboDept.Enabled = false;
            cboHC.Enabled = false;
            cboFCode.Enabled = false;
            txtJin.Enabled = false;
            txtBohum.Enabled = false;
            txtCTMRI.Enabled = false;
            txtFood.Enabled = false;
            txtDt1.Enabled = false;
            txtDt2.Enabled = false;
            txtFAmt1.Enabled = false;
            txtFAmt2.Enabled = false;

            cSP.Spread_All_Clear(SS1);
        }
        
        private void Search_Data()
        {
            string strIO = string.Empty;

            //if (tabCntrl.SelectedTab == tabPage1)
            //{
            //    strIO = "O";
            //}
            //else if (tabCntrl.SelectedTab == tabPage2)
            //{
            //    strIO = "I";
            //}
            if (swtBtn.Value == true)
            {
                strIO = "O";
            }
            else 
            {
                strIO = "I";
            }

            Screen_Display(strIO);
        }
        
        private void Set_New()
        {
            Screen_Clear();

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpSDate.Enabled = true;
            dtpEndDate.Enabled = true;
            cboIO.Enabled = true;
            cboBi.Enabled = true;
            cboMCode.Enabled = true;
            cboGbChild.Enabled = true;
            cboVCode.Enabled = true;
            cboDept.Enabled = true;
            cboHC.Enabled = true;
            cboFCode.Enabled = true;
            txtJin.Enabled = true;
            txtBohum.Enabled = true;
            txtCTMRI.Enabled = true;
            txtFood.Enabled = true;
            txtDt1.Enabled = true;
            txtDt2.Enabled = true;
            txtFAmt1.Enabled = true;
            txtFAmt2.Enabled = true;

            lblWrtno.Text = "신규";
            dtpSDate.Text = clsPublic.GstrSysDate;
            
            txtJin.Text = "0";
            txtBohum.Text = "0";
            txtCTMRI.Text = "0";
            txtFood.Text = "0";
            txtDt1.Text = "0";
            txtDt2.Text = "0";
            txtFAmt1.Text = "0";
            txtFAmt2.Text = "0";
        }

        bool eDataSave(PsmhDb pDbCon, string strRowid, int i)
        {
            try
            {
                if (strRowid == "")
                {
                    if (Insert_Data2(pDbCon, i) == false)
                    {
                        return false;
                    }
                }
                else
                {
                    if (Update_Data2(pDbCon, i) == false)
                    {
                        return false;
                    }
                }

                return true;  
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private void Data_Save()
        {
            string strIO = string.Empty;
            
            Cursor.Current = Cursors.WaitCursor;

            if (lblWrtno.Text.Trim() == "")
            {
                ComFunc.MsgBox("관리번호가 없습니다. 신규자료인 경우 신규버튼을 클릭하세요.", "점검");
                return;
            }

            if (dtpSDate.Text.Trim() == "")
            {
                ComFunc.MsgBox("적용일자가 공란입니다.", "점검");
                return;
            }

            if (cboIO.Text.Trim() == "")
            {
                ComFunc.MsgBox("입원/외래 구분이 공란입니다.", "점검");
                return;
            }

            if (cboBi.Text.Trim() == "")
            {
                ComFunc.MsgBox("환자종류가 공란입니다.", "점검");
                return;
            }

            if (cboGbChild.Text.Trim() == "")
            {
                ComFunc.MsgBox("나이구분이 공란입니다.", "점검");
                return;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (lblWrtno.Text.Trim() == "신규")
                {
                    if (Insert_Data(clsDB.DbCon) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    if (Update_Data(clsDB.DbCon) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                Screen_Clear();

                //if (tabCntrl.SelectedTab == tabPage1)
                //{
                //    strIO = "O";
                //}
                //else if (tabCntrl.SelectedTab == tabPage2)
                //{
                //    strIO = "I";
                //}

                if (swtBtn.Value == true)
                {
                    strIO = "O";
                }
                else
                {
                    strIO = "I";
                }

                Screen_Display(strIO);

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private bool Insert_Data(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            double nWRTNO = 0;
            string strWrtno = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT MAX(WRTNO) cMaxNO FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_BON ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    nWRTNO = VB.Val(Dt.Rows[0]["cMaxNO"].ToString()) + 1;
                    strWrtno = VB.Format(nWRTNO, "00#");
                }

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_ACCOUNT_BON ";
                SQL += ComNum.VBLF + "      ( WRTNO, BI, MCODE, GBCHILD, VCODE_NAME, ";
                SQL += ComNum.VBLF + "        VCODE, JIN, BOHUM, CTMRI, FOOD, GBIO, SDATE, ENTSABUN, ";
                SQL += ComNum.VBLF + "        ENTDATE,DT1,DT2, FAMT1, FAMT2, OGPDBUN, DEPT, FCODE, ENDDATE ) ";
                SQL += ComNum.VBLF + " VALUES ";
                SQL += ComNum.VBLF + "      (   '" + strWrtno + "'                                              --WRTNO     ";
                SQL += ComNum.VBLF + "          ,'" + VB.Left(cboBi.Text, 2) + "'                               --BI        ";
                SQL += ComNum.VBLF + "          ,'" + VB.Left(cboMCode.Text, 4) + "'                            --MCODE     ";
                SQL += ComNum.VBLF + "          ,'" + VB.Left(cboGbChild.Text, 1) + "'                          --GBCHILD   ";
                SQL += ComNum.VBLF + "          ,'" + VB.SinglePiece(cboVCode.Text, ".", 2).Trim() + "'         --VCODE_NAME";
                SQL += ComNum.VBLF + "          ,'" + VB.SinglePiece(cboVCode.Text, ".", 1).Trim() + "'         --VCODE     ";
                SQL += ComNum.VBLF + "          , " + Convert.ToInt16(txtJin.Text) + "                          --JIN       ";
                SQL += ComNum.VBLF + "          , " + Convert.ToInt16(txtBohum.Text) + "                        --BOHUM     ";
                SQL += ComNum.VBLF + "          , " + Convert.ToInt16(txtCTMRI.Text) + "                        --CTMRI     ";
                SQL += ComNum.VBLF + "          , " + Convert.ToInt16(txtFood.Text) + "                         --FOOD      ";
                SQL += ComNum.VBLF + "          ,'" + VB.Left(cboIO.Text, 1) + "'                               --GBIO      ";
                SQL += ComNum.VBLF + "          , TO_DATE('" + dtpSDate.Text + "','YYYY-MM-DD')                 --SDATE     ";
                SQL += ComNum.VBLF + "          , " + clsType.User.IdNumber + "                                 --ENTSABUN  ";
                SQL += ComNum.VBLF + "          , SYSDATE                                                       --ENTDATE   ";
                SQL += ComNum.VBLF + "          , " + Convert.ToInt16(txtDt1.Text) + "                          --DT1       ";
                SQL += ComNum.VBLF + "          , " + Convert.ToInt16(txtDt2.Text) + "                          --DT2       ";
                SQL += ComNum.VBLF + "          , " + Convert.ToInt64(VB.Replace(txtFAmt1.Text, ",", "")) + "   --FAMT1     ";
                SQL += ComNum.VBLF + "          , " + Convert.ToInt64(VB.Replace(txtFAmt2.Text, ",", "")) + "   --FAMT2     ";
                SQL += ComNum.VBLF + "          ,'" + VB.SinglePiece(cboHC.Text, ".", 1).Trim() + "'            --OGPDBUN   ";
                SQL += ComNum.VBLF + "          ,'" + VB.Left(cboDept.Text, 2).Trim() + "'                      --DEPT      ";
                SQL += ComNum.VBLF + "          ,'" + VB.Left(cboFCode.Text, 2).Trim() + "'                     --FCODE     ";
                if (dtpEndDate.Text.Trim() != "")
                {
                    SQL += ComNum.VBLF + "      ,TO_DATE('" + dtpEndDate.Text + "','YYYY-MM-DD')                --ENDDATE   ";
                }
                else
                {
                    SQL += ComNum.VBLF + "      ,'' ";
                }
                SQL += ComNum.VBLF + " ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool Update_Data(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            ComFunc.ReadSysDate(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_ACCOUNT_BON ";
                SQL += ComNum.VBLF + "    SET JIN   = " + txtJin.Text + "      ";
                SQL += ComNum.VBLF + "        ,BOHUM = " + txtBohum.Text + "    ";
                SQL += ComNum.VBLF + "        ,CTMRI = " + txtCTMRI.Text + "    ";
                SQL += ComNum.VBLF + "        ,FOOD  = " + txtFood.Text + "     ";
                SQL += ComNum.VBLF + "        ,DT1   = " + txtDt1.Text + "      ";
                SQL += ComNum.VBLF + "        ,DT2   = " + txtDt2.Text + "       ";
                SQL += ComNum.VBLF + "        ,FAMT1  = " + Convert.ToInt64(VB.Replace(txtFAmt1.Text, ",", "")) + " ";
                SQL += ComNum.VBLF + "        ,FAMT2  = " + Convert.ToInt64(VB.Replace(txtFAmt2.Text, ",", "")) + " ";
                if (dtpEndDate.Text.Trim() != "")
                {
                    SQL += ComNum.VBLF + "        ,ENDDATE   = TO_DATE('" + dtpEndDate.Text + "','YYYY-MM-DD')           ";
                }
                SQL += ComNum.VBLF + "  WHERE ROWID = '" + txtRowid.Text.Trim() + "'     ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool Insert_Data2(PsmhDb pDbCon, int i)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            double nWRTNO = 0;
            string strWrtno = string.Empty;
            string strIO = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            
            if (swtBtn.Value == true)
            {
                strIO = "O";
            }
            else
            {
                strIO = "I";
            }

            try
            {
               
                if (SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.SDATE].Text.Trim() == "")
                {
                    return true;
                }

                if (SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.GBIO].Text.Trim() == "")
                {
                    return true;
                }

                if (SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.BI].Text.Trim() == "")
                {
                    return true;
                }

                if (SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.GBCHILD].Text.Trim() == "")
                {
                    return true;
                }

                SQL = "";
                SQL += ComNum.VBLF + " SELECT MAX(WRTNO) cMaxNO FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_BON ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    nWRTNO = VB.Val(Dt.Rows[0]["cMaxNO"].ToString()) + 1;
                    strWrtno = VB.Format(nWRTNO, "000#");
                }

                Dt.Dispose();
                Dt = null;

                //GBCHILD,
                //HC, 
                //FCODE, 
                    
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_ACCOUNT_BON                                                                                ";
                SQL += ComNum.VBLF + "      ( WRTNO, BI, MCODE, GBCHILD, VCODE_NAME,                                                                                    ";
                SQL += ComNum.VBLF + "        VCODE, JIN, BOHUM, CTMRI, FOOD, GBIO, SDATE, ENTSABUN,                                                                    ";
                SQL += ComNum.VBLF + "        ENTDATE,DT1,DT2, FAMT1, FAMT2, OGPDBUN, DEPT, FCODE, ENDDATE )                                                            ";
                SQL += ComNum.VBLF + " VALUES                                                                                                                           ";
                SQL += ComNum.VBLF + "     ( '" + strWrtno + "'                                                                                             --WRTNO     ";
                SQL += ComNum.VBLF + "      ,'" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.BI].Text.Trim() + "'                                     --BI        ";
                SQL += ComNum.VBLF + "      ,'" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.MCODE].Text.Trim() + "'                                  --MCODE     ";
                SQL += ComNum.VBLF + "      ,'" + VB.Left(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.GBCHILD].Text.Trim(), 1) + "'                    --GBCHILD   ";
                SQL += ComNum.VBLF + "      ,'" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.VCODE_NAME].Text.Trim() + "'                             --VCODE_NAME";
                SQL += ComNum.VBLF + "      ,'" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.VCODE].Text.Trim() + "'                                  --VCODE     ";
                SQL += ComNum.VBLF + "      , " + Convert.ToInt16(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.JIN].Text) + "                           --JIN       ";
                SQL += ComNum.VBLF + "      , " + Convert.ToInt16(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.BOHUM].Text) + "                         --BOHUM     ";
                SQL += ComNum.VBLF + "      , " + Convert.ToInt16(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.CTMRI].Text) + "                         --CTMRI     ";
                SQL += ComNum.VBLF + "      , " + Convert.ToInt16(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.FOOD].Text) + "                          --FOOD      ";
                SQL += ComNum.VBLF + "      ,'" + strIO + "'                                                                                                --GBIO      ";
                SQL += ComNum.VBLF + "      , TO_DATE('" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.SDATE].Text.Trim() + "','YYYY-MM-DD')           --SDATE     ";
                SQL += ComNum.VBLF + "      , " + clsType.User.IdNumber + "                                                                                 --ENTSABUN  ";
                SQL += ComNum.VBLF + "      , SYSDATE                                                                                                       --ENTDATE   ";
                SQL += ComNum.VBLF + "      , " + Convert.ToInt16(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.DT1].Text) + "                           --DT1       ";
                SQL += ComNum.VBLF + "      , " + Convert.ToInt16(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.DT2].Text) + "                           --DT2       ";
                SQL += ComNum.VBLF + "      , " + Convert.ToInt64(VB.Replace(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.FAMT1].Text, ",", "")) + "    --FAMT1     ";
                SQL += ComNum.VBLF + "      , " + Convert.ToInt64(VB.Replace(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.FAMT2].Text, ",", "")) + "    --FAMT2     ";
                SQL += ComNum.VBLF + "      ,'" + VB.Left(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.HC].Text.Trim(), 1) + "'                         --OGPDBUN   ";
                SQL += ComNum.VBLF + "      ,'" + VB.Left(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.DEPT].Text.Trim(), 2) + "'                       --DEPT      ";
                SQL += ComNum.VBLF + "      ,'" + VB.Left(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.FCODE].Text.Trim(), 2) + "'                      --FCODE     ";
                SQL += ComNum.VBLF + "      , TO_DATE('" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.EDATE].Text.Trim() + "','YYYY-MM-DD')           --ENDDATE   ";
                SQL += ComNum.VBLF + "      )                                                                                                                           ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool Update_Data2(PsmhDb pDbCon, int i)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_ACCOUNT_BON  SET  ";
                SQL += ComNum.VBLF + "        BI         = '" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.BI].Text.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,MCODE      = '" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.MCODE].Text.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHILD    = '" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.GBCHILD].Text.Trim().Substring(0, 1) + "' ";
                SQL += ComNum.VBLF + "       ,VCODE_NAME = '" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.VCODE_NAME].Text.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,VCODE      = '" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.VCODE].Text.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,SDATE      =      TO_DATE('" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.SDATE].Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "       ,JIN        =  " + Convert.ToInt64(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.JIN].Text) + "      ";
                SQL += ComNum.VBLF + "       ,BOHUM      =  " + Convert.ToInt64(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.BOHUM].Text) + "    ";
                SQL += ComNum.VBLF + "       ,CTMRI      =  " + Convert.ToInt64(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.CTMRI].Text) + "    ";
                SQL += ComNum.VBLF + "       ,FOOD       =  " + Convert.ToInt64(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.FOOD].Text) + "     ";
                SQL += ComNum.VBLF + "       ,DT1        =  " + Convert.ToInt64(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.DT1].Text) + "      ";
                SQL += ComNum.VBLF + "       ,DT2        =  " + Convert.ToInt64(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.DT2].Text) + "       ";
                SQL += ComNum.VBLF + "       ,FAMT1      =  " + Convert.ToInt64(VB.Replace(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.FAMT1].Text, ",", "")) + " ";
                SQL += ComNum.VBLF + "       ,FAMT2      =  " + Convert.ToInt64(VB.Replace(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.FAMT2].Text, ",", "")) + " ";
                SQL += ComNum.VBLF + "       ,OGPDBUN    = '" + VB.Left(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.HC].Text.Trim(), 1) + "' ";
                SQL += ComNum.VBLF + "       ,DEPT       = '" + VB.Left(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.DEPT].Text.Trim(), 2) + "' ";
                SQL += ComNum.VBLF + "       ,FCODE      = '" + VB.Left(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.FCODE].Text.Trim(), 2) + "' ";
                if (SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.EDATE].Text.Trim() != "")
                {
                    SQL += ComNum.VBLF + "       ,ENDDATE    = TO_DATE('" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.EDATE].Text.Trim() + "','YYYY-MM-DD') ";
                }
                SQL += ComNum.VBLF + "  WHERE ROWID      = '" + SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmBasAcctBon.ROWID].Text.Trim() + "'     ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool Data_Delete(string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_ACCOUNT_BON ";
                SQL += ComNum.VBLF + "    SET DELDATE   = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  WHERE ROWID = '" + strRowid + "'     ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                return true;   
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        #region 쓰레드적용

        delegate void threadSpdTypeDelegate(FpSpread spd, DataTable dt);

        private void Screen_Display(string strIO)
        {
            SS1.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            spd = SS1;

            #region 변수 값 세팅
            cAB.Bi      = VB.Left(cboBi2.Text, 2);
            cAB.Child   = VB.Left(cboGbChild2.Text, 1);
            cAB.MCode   = VB.Left(cboMCode2.Text, 4).Trim();
            cAB.Dept    = VB.Left(cboDept2.Text, 2).Trim();
            cAB.VCode   = VB.Left(cboVCode2.Text, 4).Trim();
            cAB.HC      = VB.Replace(VB.Left(cboHC2.Text, 2).Trim(), ".", "").Trim();
            cAB.FCode   = VB.Left(cboFCode2.Text, 2).Trim();
            cAB.IO      = strIO;
            cAB.All     = chkAll.Checked;
            #endregion

            //스레드 시작
            thread = new Thread(tProcess);
            thread.Start();

            Cursor.Current = Cursors.Default;
        }

        private void tProcess()
        { 
            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));
            
            DataTable dt = null;
            dt = sel_BasAccountBon(clsDB.DbCon, cAB);

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));
            
            Cursor.Current = Cursors.Default;
        }
        
        void tShowSpread(FpSpread spd, DataTable Dt)
        {
            int i = 0, nRead = 0, nRow = 0;

            spd.ActiveSheet.ClearRange(0, 0, spd.ActiveSheet.Rows.Count, spd.ActiveSheet.ColumnCount, false);
            spd.ActiveSheet.Rows.Count = 0;

            if (Dt == null)
            {
                return;
            }

            try
            {
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nRow += 1;
                        if (spd.ActiveSheet.Rows.Count < nRow)
                        {
                            spd.ActiveSheet.Rows.Count = nRow;
                        }
                        
                        //clsSpread.gSdCboItemFindLeft(SS1, nRow - 1, (int)clsPmpaPb.enmBasAcctBon.GBCHILD, 1, Dt.Rows[i]["GBCHILD"].ToString().Trim());
                        clsSpread.gSdCboItemFindLeft(SS1, nRow - 1, (int)clsPmpaPb.enmBasAcctBon.HC, 1, Dt.Rows[i]["OGPDBUN"].ToString().Trim());
                        clsSpread.gSdCboItemFindLeft(SS1, nRow - 1, (int)clsPmpaPb.enmBasAcctBon.FCODE, 2, Dt.Rows[i]["FCODE"].ToString().Trim());

                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.WRTNO].Text        = Dt.Rows[i]["WRTNO"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.SDATE].Text        = Dt.Rows[i]["SDATE"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.GBIO].Text         = Dt.Rows[i]["GBIO"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.BI].Text           = Dt.Rows[i]["BI"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.GBCHILD].Text      = Dt.Rows[i]["GBCHILD"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.CHILDNAME].Text    = Read_Child_Name(Dt.Rows[i]["GBCHILD"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.MCODE].Text        = Dt.Rows[i]["MCODE"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.VCODE_NAME].Text   = Dt.Rows[i]["VCODE_NAME"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.VCODE].Text        = Dt.Rows[i]["VCODE"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.DEPT].Text         = Dt.Rows[i]["DEPT"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.JIN].Text          = Dt.Rows[i]["JIN"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.BOHUM].Text        = Dt.Rows[i]["BOHUM"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.CTMRI].Text        = Dt.Rows[i]["CTMRI"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.FOOD].Text         = Dt.Rows[i]["FOOD"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.DT1].Text          = Dt.Rows[i]["DT1"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.DT2].Text          = Dt.Rows[i]["DT2"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.FAMT1].Text        = VB.Val(Dt.Rows[i]["FAMT1"].ToString().Trim()).ToString("###,###,##0");
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.FAMT2].Text        = VB.Val(Dt.Rows[i]["FAMT2"].ToString().Trim()).ToString("###,###,##0");
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.EDATE].Text        = Dt.Rows[i]["ENDDATE"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.ENTDATE].Text      = Dt.Rows[i]["ENTDATE"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.ENTSABUN].Text     = Dt.Rows[i]["ENTSABUN"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.DELDATE].Text      = Dt.Rows[i]["DELDATE"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.ROWID].Text        = Dt.Rows[i]["ROWID"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmBasAcctBon.ROWID].Locked      = true;

                        if (Dt.Rows[i]["DELDATE"].ToString().Trim() != "")
                        { 
                            spd.ActiveSheet.Cells[nRow - 1, 0, nRow -1, spd.ActiveSheet.ColumnCount - 1].Locked = true;
                            cSP.setSpdForeColor(spd, nRow - 1, 0, nRow - 1, spd.ActiveSheet.ColumnCount - 1, Color.Red);
                        }
                        
                    }
                }
                Dt.Dispose();
                Dt = null;

                if (txtSpdLen.Text.Trim() != "")
                {
                    int nCNT = spd.ActiveSheet.RowCount;
                    
                    spd.ActiveSheet.RowCount += Convert.ToInt32(txtSpdLen.Text);

                    for (i = nCNT; i < spd.ActiveSheet.RowCount; i++)
                    {

                        spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmBasAcctBon.WRTNO].Text = "";
                        spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmBasAcctBon.ROWID].Locked = true;

                        for (int j = 0; j < spd.ActiveSheet.ColumnCount; j++)
                        {
                            cSP.setColStyle(spd, i, j, clsSpread.enmSpdType.Text);
                            cSP.setColStyle(spd, i, (int)clsPmpaPb.enmBasAcctBon.FAMT1, clsSpread.enmSpdType.Text, null, null, null, null, false);
                            cSP.setColStyle(spd, i, (int)clsPmpaPb.enmBasAcctBon.FAMT2, clsSpread.enmSpdType.Text, null, null, null, null, false);
                            cSP.setColStyle(spd, i, (int)clsPmpaPb.enmBasAcctBon.MCODE, clsSpread.enmSpdType.Text, null, null, null, null, false);
                            cSP.setColStyle(spd, i, (int)clsPmpaPb.enmBasAcctBon.VCODE, clsSpread.enmSpdType.Text, null, null, null, null, false);

                            cSP.setColLength(spd, i, (int)clsPmpaPb.enmBasAcctBon.DEPT, 2);
                            cSP.setColLength(spd, i, (int)clsPmpaPb.enmBasAcctBon.GBCHILD, 1);
                            cSP.setColLength(spd, i, (int)clsPmpaPb.enmBasAcctBon.MCODE, 4);
                            cSP.setColLength(spd, i, (int)clsPmpaPb.enmBasAcctBon.VCODE, 4);
                            cSP.setColLength(spd, i, (int)clsPmpaPb.enmBasAcctBon.JIN, 3);
                            cSP.setColLength(spd, i, (int)clsPmpaPb.enmBasAcctBon.BOHUM, 3);
                            cSP.setColLength(spd, i, (int)clsPmpaPb.enmBasAcctBon.FOOD, 3);
                            cSP.setColLength(spd, i, (int)clsPmpaPb.enmBasAcctBon.CTMRI, 3);
                            cSP.setColLength(spd, i, (int)clsPmpaPb.enmBasAcctBon.DT1, 3);
                            cSP.setColLength(spd, i, (int)clsPmpaPb.enmBasAcctBon.DT2, 3);
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }
                ComFunc.MsgBox(ex.Message);
            }
        }

        string Read_Child_Name(string strChild)
        {
            string strName = string.Empty;

            switch (strChild)
            {
                case "0": strName = "성인"; break;
                case "1": strName = "신생아"; break;
                case "2": strName = "6세미만"; break;
                case "3": strName = "6세~15세"; break;
                case "4": strName = "65세이상"; break;
                case "5": strName = "1세미만"; break;
                default:  strName = "성인"; break;
            }
            return strName;
        }

        delegate void threadProcessDelegate(bool b);

        void trunCircular(bool b)
        {
            this.Progress.Visible = b;
            this.Progress.IsRunning = b;
        }

        #endregion

        private DataTable sel_BasAccountBon(PsmhDb pDbCon, cBasAcctBon cAB)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT WRTNO, BI,MCODE,GBCHILD,VCODE,JIN,BOHUM,CTMRI,FOOD,GBIO,OGPDBUN               ";
                SQL += ComNum.VBLF + "       ," + ComNum.DB_MED + "FC_BAS_VCODE_NAME(VCODE) VCODE_NAME                      ";
                SQL += ComNum.VBLF + "       ," + ComNum.DB_MED + "FC_BAS_USER_USERNAME2(ENTSABUN) ENTSABUN                 ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(SDATE, 'YYYY-MM-DD') SDate, TO_CHAR(DELDATE, 'YYYY-MM-DD') DelDate    ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(ENTDATE, 'YYYY-MM-DD HH24:MI') EntDate, DT1,DT2,FAMT1,FAMT2,FCODE,DEPT";
                SQL += ComNum.VBLF + "       ,TO_CHAR(ENDDATE, 'YYYY-MM-DD') EndDate                                        ";
                SQL += ComNum.VBLF + "       ,ROWID                                                                         ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_BON                                         ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                if (cAB.Bi != "전체")
                {
                    SQL += ComNum.VBLF + "    AND Bi = '" + cAB.Bi + "' ";
                }
                if (cAB.IO != "전체")
                {
                    SQL += ComNum.VBLF + "    AND GBIO = '" + cAB.IO + "' ";
                }
                if (cAB.Child != "*")
                {
                    SQL += ComNum.VBLF + "    AND GBCHILD = '" + cAB.Child + "' ";
                }
                if (cAB.MCode == "일반")
                {
                    SQL += ComNum.VBLF + "    AND (MCODE IS NULL OR MCODE = '' OR MCODE = ' ') ";
                }
                else if (cAB.MCode != "전체")
                {
                    SQL += ComNum.VBLF + "    AND MCODE = '" + cAB.MCode + "' ";
                }
                if (cAB.Dept != "**")
                {
                    SQL += ComNum.VBLF + "    AND DEPT = '" + cAB.Dept + "' ";
                }
                if (cAB.VCode == "일반")
                {
                    SQL += ComNum.VBLF + "    AND (VCODE IS NULL OR VCODE = '' OR VCODE = ' ') ";
                }
                else if (cAB.VCode != "전체")
                {
                    SQL += ComNum.VBLF + "    AND VCODE = '" + cAB.VCode + "' ";
                }
                if (cAB.HC != "전체")
                {
                    SQL += ComNum.VBLF + "    AND OGPDBUN = '" + cAB.HC + "' ";
                }
                if (cAB.FCode == "일반")
                {
                    SQL += ComNum.VBLF + "    AND (FCODE IS NULL OR FCODE = '' OR FCODE = ' ') ";
                }
                else if (cAB.FCode != "전체")
                {
                    SQL += ComNum.VBLF + "    AND FCODE = '" + cAB.FCode + "' ";
                }
                if (cAB.All == false)
                {
                    SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE > TRUNC(SYSDATE)) ";
                }
                SQL += ComNum.VBLF + "  ORDER By WRTNO ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null; 
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return null;
            }
        }

        /// <summary>
        /// 콤보박스에 BCode 관련 항목을 추가한다. 
        /// </summary>
        /// <param name="ArgCombobox"></param>
        /// <param name="argGubun"></param>
        /// <param name="ArgClear"></param>
        /// <param name="ArgTYPE"></param>
        /// <param name="ArgNULL"></param>
        private void Combo_BCode_SET(PsmhDb pDbCon, ComboBox ArgCombobox, string argGubun, bool chkNormal, bool bName)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            ArgCombobox.Items.Clear();

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                               ";
            SQL = SQL + ComNum.VBLF + "     Sort,Code,Name                                  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE                 ";
            SQL = SQL + ComNum.VBLF + "     WHERE 1=1                                       ";
            SQL = SQL + ComNum.VBLF + "   AND Gubun = '" + argGubun + "'                    ";
            SQL = SQL + ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Sort,Code                                   ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }

            ArgCombobox.Items.Add("전체");
            if (chkNormal)
            {
                ArgCombobox.Items.Add("일반");
            }

            if (dt.Rows.Count > 0)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (bName)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }
                    else
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                    }
                    
                }
            }
            else
            {
                ComFunc.MsgBox("조회중 문제가 발생하였습니다.");
                return;
            }
            dt.Dispose();
            dt = null;
            
        }
        
        private void Combo_BCode_SET_VCODE(PsmhDb pDbCon, ComboBox ArgCombobox, bool chkNormal, bool bName, bool chkAll, bool chkNull)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            ArgCombobox.Items.Clear();

            if (chkNull)
            {
                ArgCombobox.Items.Add("");
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT Code,Name                                     ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE               ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                           ";
            SQL = SQL + ComNum.VBLF + "    AND Gubun = 'BAS_중증암환자'                      ";
            SQL = SQL + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
            SQL = SQL + ComNum.VBLF + "  UNION                                               ";
            SQL = SQL + ComNum.VBLF + " SELECT Code,Name                                     ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE               ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                           ";
            SQL = SQL + ComNum.VBLF + "    AND Gubun = '희귀난치V_상세코드'                  ";
            SQL = SQL + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }
            
            if (chkAll)
            {
                ArgCombobox.Items.Add("전체");
            }
            
            if (chkNormal)
            {
                ArgCombobox.Items.Add("일반");
            }

            if (dt.Rows.Count > 0)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (bName)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }
                    else
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                    }

                }
            }
            else
            {
                ComFunc.MsgBox("조회중 문제가 발생하였습니다.");
                return;
            }
            dt.Dispose();
            dt = null;

        }

        private string Read_VCode_Name(PsmhDb pDbCon, string ArgVCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Name                                          ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE               ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                           ";
                SQL = SQL + ComNum.VBLF + "    AND Gubun = 'BAS_중증암환자'                      ";
                SQL = SQL + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
                SQL = SQL + ComNum.VBLF + "    AND CODE = '" + ArgVCode + "'                     ";
                SQL = SQL + ComNum.VBLF + "  UNION                                               ";
                SQL = SQL + ComNum.VBLF + " SELECT Name                                          ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE               ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                           ";
                SQL = SQL + ComNum.VBLF + "    AND Gubun = '희귀난치V_상세코드'                  ";
                SQL = SQL + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
                SQL = SQL + ComNum.VBLF + "    AND CODE = '" + ArgVCode + "'                     ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                return rtnVal;
                
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return "";
            }
        }

        private string Read_MCode_Name(PsmhDb pDbCon, string ArgMCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Name                                          ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE               ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                           ";
                SQL = SQL + ComNum.VBLF + "    AND Gubun = 'BAS_의료급여본인부담'                      ";
                SQL = SQL + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
                SQL = SQL + ComNum.VBLF + "    AND CODE = '" + ArgMCode + "'                     ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                return rtnVal;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return "";
            }
        }
    }
}
