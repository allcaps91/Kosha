using ComBase;
using System;
using System.Windows.Forms;
using ComDbB;
using System.Data;
using System.Threading;
using FarPoint.Win.Spread;
using System.Drawing;
using ComBase.Controls;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcCode.cs
/// Description     : 건진센터 기초코드 관리
/// Author          : 김민철
/// Create Date     : 2018-08-27
/// Update History  : 
/// </summary>
/// <history>  
/// 기존 FrmCodeEntry(HaCode09.frm) / FrmGCodeEntry(HcCode02.frm) 통합
/// </history>
/// <seealso cref= "HaCode09.frm(FrmCodeEntry) / HcCode02.frm(FrmGCodeEntry)" />
namespace ComHpcLibB
{
    public partial class frmHcCode : Form
    {
        int FnREAD = 0;
        string FstrTable = "HIC_CODE";       //기본 Table 
        string FstrGubun = "";
        string FstrKeyWord = "";

        Thread thread;
        FpSpread spd;

        public frmHcCode()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcCode(string argTable, string argGubun, string argKeyWord = "")
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FstrTable = argTable;
            FstrGubun = argGubun;
            FstrKeyWord = argKeyWord;
        }

        void SetControl()
        {
            try
            {
                ComFunc CF = new ComFunc();
                clsHcFunc cHF = new clsHcFunc();

                cboJong.Items.Clear();
                cboJong.Items.Add("00.기초코드");
                cHF.Combo_HCode_SET(clsDB.DbCon, cboJong, "00", false, 1, FstrTable, "N");
                
                cboJong.SelectedIndex = 0;

                SetAutoKeyWords(txtName, VB.Left(cboJong.Text, 2));

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            
        }

        void SetEvent()
        {
            this.Load                           += new EventHandler(eFormLoad);
            this.tabCtrl.SelectedIndexChanged   += new EventHandler(eChangeRdo);
            this.SS1.KeyDown                    += new KeyEventHandler(eSpdKeyDown);
            this.SS1.CellClick                  += new CellClickEventHandler(eSpdClick);
            this.SS1.EditModeOff                += new EventHandler(eSpdEditChange);
            this.SS1.ButtonClicked              += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.ClipboardPasted            += new ClipboardPastedEventHandler(eSpdClipBoard);      //Spread 복사 붙여넣기 Event
            this.btnExit.Click                  += new EventHandler(eBtnClick);
            this.btnCancel.Click                += new EventHandler(eBtnClick);
            this.btnSave.Click                  += new EventHandler(eBtnClick);
            this.btnSearch.Click                += new EventHandler(eBtnClick);
            this.txtName.KeyPress               += new KeyPressEventHandler(eKeyPress);
            this.lblCount.LinkClicked           += new LinkLabelLinkClickedEventHandler(eLabelClick);
            this.cboJong.SelectedIndexChanged   += new EventHandler(eCboChage);
        }

        void eCboChage(object sender, EventArgs e)
        {
            if (sender == cboJong)
            {
                if (cboJong.Text.Trim() == "")
                {
                    return;
                }

                SetAutoKeyWords(txtName, VB.Left(cboJong.Text, 2));
            }
            
        }

        void SetAutoKeyWords(TextBox txtName, string ArgGubun)
        {
            if (ArgGubun.Trim() == "")
            {
                return;
            }

            try
            {
                AutoCompleteStringCollection _acsCollection = new AutoCompleteStringCollection();

                _acsCollection.AddRange(sel_HcBasCodeAuto(clsDB.DbCon, ArgGubun));

                txtName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtName.AutoCompleteCustomSource = _acsCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void eLabelClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (FnREAD == 0)
            {
                return;
            }

            SS1.ActiveSheet.SetActiveCell(FnREAD, (int)clsHcSpd.enmHcCode.CODE);
            SS1.ShowActiveCell(VerticalPosition.Center, HorizontalPosition.Center);
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column == (int)clsHcSpd.enmHcCode.chk01)
            {
                clsSpread cSpd = new clsSpread();

                if (SS1.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcCode.chk01].Text == "True")
                {
                    cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.Red);
                }
                else
                {
                    if (SS1.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcCode.CHANGE].Text == "Y")
                    {
                        cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.Blue);
                    }
                    else
                    {
                        cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.Black);
                    }
                }
            }
        }

        void eSpdClipBoard(object sender, ClipboardPastedEventArgs e)
        {
            int nRow = 0;
            int nRow2 = 0;

            nRow = e.CellRange.Row;
            nRow2 = e.CellRange.Row + e.CellRange.RowCount - 1;

            //변경된 Cell로 마킹
            SS1.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcCode.CHANGE, nRow2, (int)clsHcSpd.enmHcCode.CHANGE].Text = "Y";
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread cSpd = new clsSpread();
                cSpd.setSpdSort(SS1, e.Column, true);
            }
        }

        void eSpdEditChange(object sender, EventArgs e)
        {
            int nRow = SS1.ActiveSheet.ActiveRowIndex;

            if (SS1.ActiveSheet.Cells[nRow, 6].Text.Trim() != "Y")
            {
                clsSpread cSpd = new clsSpread();

                SS1.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcCode.CHANGE].Text = "Y";
                cSpd.setSpdForeColor(SS1, nRow, 0, nRow, SS1_Sheet1.ColumnCount - 1, Color.Blue);
                cSpd = null;
            }
        }
        
        void eSpdKeyDown(object sender, KeyEventArgs e)
        {
            clsSpread cSpd = new clsSpread();

            if (e.KeyCode == Keys.Enter)
            {
                cSpd.setEnterKey(SS1, clsSpread.enmSpdEnterKey.Right);
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtName)
            {
                if (e.KeyChar == (char)13)
                {
                    btnSearch.Focus();
                }
            }
        }
        
        void eBtnClick(object sender, EventArgs e)
        {
            try
            {
                if (sender == btnExit)
                {
                    this.Close();
                    return;
                }
                else if (sender == btnCancel)
                {
                    Screen_Clear();
                }
                else if (sender == btnSave)
                {
                    eSave(clsDB.DbCon, SS1);
                }
                else if (sender == btnSearch)
                {
                    FstrGubun = VB.Left(cboJong.Text, 2);
                    FstrKeyWord = txtName.Text.Trim();

                    if (FstrGubun != "")
                    {
                        Screen_Display(clsDB.DbCon, SS1);
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }
        
        void eSave(PsmhDb pDbCon, FpSpread Spd)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int i               = 0;
            int nRow            = 0;

            string strDel       = "";
            string strCode      = "";
            string strName      = "";
            string strChk       = "";
            string strSort      = "";
            string strGCode     = "";
            string strGCode1    = "";
            string strGCode2    = "";
            string strROWID     = "";
            string strChange    = "";
            string strMsg       = "";

            string strGubun1 = "";
            string strGubun2 = "";
            string strGubun3 = "";

            if (SS1.ActiveSheet.RowCount == 0)
            {
                return;
            }
            
            try
            {
                strMsg = Check_Data_Error(SS1, FstrTable);

                if (strMsg != "")
                {
                    ComFunc.MsgBox(strMsg, "자료점검");
                    return;
                }

                this.btnSave.Enabled = false;
                this.btnCancel.Enabled = false;
                this.SS1.Enabled = false;

                if (FstrTable == "HIC_CODE")
                {
                    strGubun1 = "GCODE";
                    strGubun2 = "GCODE1";
                    strGubun3 = "GCODE2";
                }
                else
                {
                    strGubun1 = "GUBUN1";
                    strGubun2 = "GUBUN2";
                    strGubun3 = "GUBUN3";
                }

                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(pDbCon);

                nRow = Spd.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1;

                for (i = 0; i < nRow; i++)
                {
                    strChk      = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.chk01].Text.Trim();
                    strCode     = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.CODE].Text.Trim();
                    strName     = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.NAME].Text.Trim();
                    strDel      = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.GBDEL].Text.Trim();
                    strSort     = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.SORT].Text.Trim();
                    strGCode    = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.GUBUN1].Text.Trim();
                    strGCode1   = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.GUBUN2].Text.Trim();
                    strGCode2   = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.GUBUN3].Text.Trim();
                    strROWID    = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.ROWID].Text.Trim();
                    strChange   = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.CHANGE].Text.Trim();

                    if (strName != "")
                    {
                        strName = strName.Replace("'", "`");
                    }

                    if (strROWID == "")
                    {
                        if (strChk != "True")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + FstrTable + "                   ";
                            SQL += ComNum.VBLF + "        (Gubun, Code, Name, Sort, GbDel,                          ";
                            SQL += ComNum.VBLF + "        " + strGubun1 + "," + strGubun2 + "," + strGubun3 + ")    ";
                            SQL += ComNum.VBLF + " VALUES (                                                         ";
                            SQL += ComNum.VBLF + "        '" + FstrGubun + "', '" + strCode + "', '" + strName + "',";
                            SQL += ComNum.VBLF + "        '" + strSort + "', '" + strDel + "','" + strGCode + "',   ";
                            SQL += ComNum.VBLF + "        '" + strGCode1 + "','" + strGCode2 + "')                  ";
                        }
                    }
                    else
                    {
                        if (strChk == "True")
                        {
                            SQL = "DELETE " + ComNum.DB_PMPA + FstrTable + " WHERE ROWID = '" + strROWID + "' ";
                        }
                        else if (strChange == "Y")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + FstrTable + "        ";
                            SQL += ComNum.VBLF + "    SET Code = '" + strCode + "',                 ";
                            SQL += ComNum.VBLF + "        Name='" + strName + "',                   ";
                            SQL += ComNum.VBLF + "        Sort ='" + strSort + "',                  ";
                            SQL += ComNum.VBLF + "       " + strGubun1 + " = '" + strGCode + "',    ";
                            SQL += ComNum.VBLF + "       " + strGubun2 + " = '" + strGCode1 + "',   ";
                            SQL += ComNum.VBLF + "       " + strGubun3 + " = '" + strGCode2 + "',   ";
                            SQL += ComNum.VBLF + "        GbDel = '" + strDel + "'                  ";
                            SQL += ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "'                ";
                        }
                    }

                    if (SQL != "")
                    {
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            this.btnSave.Enabled = true;
                            this.btnCancel.Enabled = true;
                            this.SS1.Enabled = true;
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }


                clsDB.setCommitTran(pDbCon);

                ComFunc.MsgBox("저장하였습니다.");
                
                Cursor.Current = Cursors.Default;

                Screen_Clear();

                cboJong.Focus();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        string Check_Data_Error(FpSpread Spd, string ArgGbn)
        {
            int i           = 0;
            int nRow        = 0;

            string strDel   = "";
            string strCode  = "";
            string strName  = "";
            string rtnVal   = "";

            try
            {
                nRow = Spd.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1;

                for (i = 0; i < nRow; i++)
                {
                    strCode = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.CODE].Text;
                    strName = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.NAME].Text;
                    strDel  = Spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.GBDEL].Text;

                    if (strCode == "")
                    {
                        rtnVal = (i + 1).ToString() + "번째 줄 코드가 공란입니다.";
                        break;
                    }

                    if (strName == "")
                    {
                        rtnVal = (i + 1).ToString() + "번째 줄 코드명칭이 공란입니다.";
                        break;
                    }

                    if (strDel != "Y" && strDel != "")
                    {
                        rtnVal = (i + 1).ToString() + "번째 줄 삭제 시 Y를 입력하세요.";
                        break;
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }    
        }

        void eChangeRdo(object sender, EventArgs e)
        {
            try
            {
                if (tabCtrl.SelectedTab == tabPage1)
                {
                    FstrTable = "HIC_CODE";
                }
                else
                {
                    FstrTable = "HEA_CODE";
                }
                
                cboJong.Text = "";

                SetControl();

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            try
            {
                clsHcSpd cHSpd = new clsHcSpd();

                ComFunc.ReadSysDate(clsDB.DbCon);

                cHSpd.sSpd_enmHcCode(SS1, cHSpd.sHcCode, cHSpd.nHcCode, 10, 0);

                //cboJong.Text = "";
                txtName.Text = "";

                Screen_Clear();

                if (!FstrGubun.IsNullOrEmpty())
                {
                    cboJong.Text = FstrGubun;
                    FstrGubun = VB.Pstr(FstrGubun, ".", 1);
                    Screen_Display(clsDB.DbCon, SS1);
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Screen_Clear()
        {
            try
            {
                clsSpread cSpd = new clsSpread();
                
                cSpd.Spread_All_Clear(SS1);

                FnREAD = 0;
                lblCount.Text = FnREAD.ToString() + " 건";

                this.btnSave.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnSearch.Enabled = true;
                this.cboJong.Enabled = true;
                this.tabCtrl.Enabled = true;
                this.txtName.Enabled = true;
                this.SS1.Enabled = false;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        delegate void threadSpdTypeDelegate(FpSpread spd, DataTable dt);

        void Screen_Display(PsmhDb pDbCon, FpSpread Spd)
        {
            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            spd = SS1;

            try
            {
                //스레드 시작
                thread = new Thread(tProcess);
                thread.Start();

                Cursor.Current = Cursors.Default;

                this.btnSave.Enabled = true;
                this.btnCancel.Enabled = true;
                this.btnSearch.Enabled = false;
                this.cboJong.Enabled = false;
                this.tabCtrl.Enabled = false;
                this.txtName.Enabled = false;
                this.SS1.Enabled = true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            
        }

        void tProcess()
        {
            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            //this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));

            DataTable dt = null;
            
            dt = sel_HcBasCode(clsDB.DbCon, FstrGubun, FstrKeyWord);

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));
            //this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));
        }

        void tShowSpread(FpSpread spd, DataTable Dt)
        {
            int i = 0;
            
            if (Dt == null)
            {
                return;
            }

            try
            {
                if (Dt.Rows.Count > 0)
                {
                    FnREAD = Dt.Rows.Count;
                    lblCount.Text = FnREAD.ToString() + " 건";
                    spd.ActiveSheet.RowCount = Dt.Rows.Count;

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.CODE].Text     = Dt.Rows[i]["CODE"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.NAME].Text     = Dt.Rows[i]["NAME"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.GUBUN1].Text   = Dt.Rows[i]["GUBUN1"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.GUBUN2].Text   = Dt.Rows[i]["GUBUN2"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.GUBUN3].Text   = Dt.Rows[i]["GUBUN3"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.SORT].Text     = Dt.Rows[i]["SORT"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.GBDEL].Text    = Dt.Rows[i]["GBDEL"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcCode.ROWID].Text    = Dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                Dt.Dispose();
                Dt = null;

                spd.ActiveSheet.RowCount += 300;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);

                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }
            }
        }

        delegate void threadProcessDelegate(bool b);

        void trunCircular(bool b)
        {
            this.Progress.Visible = b;
            this.Progress.IsRunning = b;
        }

        DataTable sel_HcBasCode(PsmhDb pDbCon, string strGubun, string strKey)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                if (FstrTable == "HIC_CODE")
                {
                    SQL = SQL + ComNum.VBLF + "SELECT Sort,Code,Name,GBDEL,ROWID,                   ";
                    SQL = SQL + ComNum.VBLF + "       GCODE Gubun1, GCODE1 Gubun2, GCODE2 Gubun3    ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_CODE                ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "SELECT Sort,Code,Name,Gubun1,Gubun2,Gubun3,GBDEL,ROWID ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HEA_CODE                ";
                }
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                                             ";
                if (strKey != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND NAME LIKE '%" + strKey + "%'                ";
                }
                SQL = SQL + ComNum.VBLF + "   AND Gubun = '" + strGubun + "'                        ";
                SQL = SQL + ComNum.VBLF + "   AND (GBDEL IS NULL OR GBDEL = '')                     ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Sort,Code                                       ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

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

        string[] sel_HcBasCodeAuto(PsmhDb pDbCon, string strGubun)
        {
            string[] rtnVal = new string[0];

            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int i = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Name                   ";
                if (FstrTable == "HIC_CODE")
                {
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_CODE                ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HEA_CODE                ";
                }
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                                             ";                
                SQL = SQL + ComNum.VBLF + "   AND Gubun = '" + strGubun + "'                        ";
                SQL = SQL + ComNum.VBLF + "   AND (GBDEL IS NULL OR GBDEL = '')                     ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Sort,Code                                       ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        if ((i + 1) > rtnVal.Length)
                        {
                            Array.Resize<string>(ref rtnVal, (i + 1));
                        }

                        rtnVal[i] = Dt.Rows[i]["NAME"].ToString().Trim();
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                return rtnVal;
            }
        }
        
    }
}
