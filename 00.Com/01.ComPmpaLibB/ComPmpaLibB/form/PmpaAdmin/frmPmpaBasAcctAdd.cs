using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public partial class frmPmpaBasAcctAdd : Form
    {
        clsSpread cSpd = new clsSpread();
        clsPmpaPb cPb = new clsPmpaPb();
        clsComPmpaSpd cPmpaSpd = new clsComPmpaSpd();
        clsBasAcct cBAcct = new clsBasAcct();

        clsPmpaType.cBas_Add cBAdd = null;
        
        string FstrTable = string.Empty;
        string FstrGbn = string.Empty;
        string FstrChild = string.Empty;

        public frmPmpaBasAcctAdd()
        {
            InitializeComponent();
            SetEvent();
        }

        private void SetEvent()
        {
            this.Load                           += new EventHandler(eFormLoad);

            this.cboBun.SelectedIndexChanged    += new EventHandler(Set_Spread);
            this.cboChild.SelectedIndexChanged  += new EventHandler(Set_Spread);
            this.chkDel.CheckedChanged          += new EventHandler(Set_Spread);

            this.cboBun.MouseWheel              += new MouseEventHandler(eCboWheel);
            this.cboChild.MouseWheel            += new MouseEventHandler(eCboWheel);
            
            this.btnExit.Click                  += new EventHandler(eBtnClick);
            this.btnSearch.Click                += new EventHandler(eBtnClick);
            this.btnSave.Click                  += new EventHandler(eBtnClick);

            this.SS1.KeyPress                   += new KeyPressEventHandler(eSpdKeyPress);
            this.SS1.EditChange                 += new EditorNotifyEventHandler(eSpreadEditChange);
            this.SS1.CellClick                  += new CellClickEventHandler(eSpdClick);
            this.SS1.LeaveCell                  += new LeaveCellEventHandler(eSpdLeaveCell);
            this.SS1.CellDoubleClick            += new CellClickEventHandler(eSpdDblClick);
            this.SS1.Change                     += new ChangeEventHandler(eSpdChange);
        }

        private void eSpdLeaveCell(object sender, LeaveCellEventArgs e)
        {
            SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].ResetBackColor();
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.LightGreen;
        }

        private void eCboWheel(object sender, MouseEventArgs e)
        {
            ComboBox CB = sender as ComboBox;

            if (CB.Focused == false)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            //if (e.Row < 0 || e.Column < (int)clsPmpaPb.enmPmpaAdd.ADD1 || e.Column > (int)clsPmpaPb.enmPmpaAdd.ADD6) { return; }

            //if (SS1.ActiveSheet.Cells[e.Row, e.Column].ToString().Trim() == "")
            //{
            //    SS1.ActiveSheet.Cells[e.Row, e.Column].Text = "Y";
            //}
            //else
            //{
            //    SS1.ActiveSheet.Cells[e.Row, e.Column].Text = "";
            //}
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            #region ComboBox Data Set
            cboBun.Items.Clear();
            cboBun.Items.Add("01.진찰료");
            cboBun.Items.Add("02.약제조제료");
            cboBun.Items.Add("03.주사수기료");
            cboBun.Items.Add("04.마취료");
            cboBun.Items.Add("05.처치수술료");
            cboBun.Items.Add("06.검사료");
            cboBun.Items.Add("07.영상진단료");
            
            cboChild.Items.Clear();
            cboChild.Items.Add("*.전체");
            cboChild.Items.Add("0.성인");
            cboChild.Items.Add("1.신생아");
            cboChild.Items.Add("2.만1세미만");
            cboChild.Items.Add("3.만1세이상-만6세미만");
            cboChild.Items.Add("4.만6세미만");
            cboChild.Items.Add("5.만70세이상");
            cboChild.Items.Add("7.신생아0세");
            #endregion

            Screen_Clear();

            cPmpaSpd.sSpd_enmPmpaADD(SS1, cPb.sSpdPmpaAdd, cPb.nSpdPmpaAdd, 10, 0, chkDel.Checked, "");
        }

        private void Set_Spread(object sender, EventArgs e)
        {
            Screen_Clear();
            
            FstrGbn = VB.Left(cboBun.Text.Trim(), 2).Trim();
            FstrChild = VB.Left(cboChild.Text.Trim(), 1).Trim();

            switch (FstrGbn)
            {
                case "01":
                    cPmpaSpd.sSpd_enmPmpaADD(SS1, cPb.sSpdPmpaAdd_Jin, cPb.nSpdPmpaAdd_Jin, 10, 0, chkDel.Checked, FstrGbn);
                    FstrTable = "_JIN"; break;
                case "02":
                    cPmpaSpd.sSpd_enmPmpaADD(SS1, cPb.sSpdPmpaAdd_Drug, cPb.nSpdPmpaAdd_Drug, 10, 0, chkDel.Checked, FstrGbn);
                    FstrTable = "_DRUG"; break;
                case "03":
                    cPmpaSpd.sSpd_enmPmpaADD(SS1, cPb.sSpdPmpaAdd_Jusa, cPb.nSpdPmpaAdd_Jusa, 10, 0, chkDel.Checked, FstrGbn);
                    FstrTable = "_JUSA"; break;
                case "04":
                    cPmpaSpd.sSpd_enmPmpaADD(SS1, cPb.sSpdPmpaAdd_AN, cPb.nSpdPmpaAdd_AN, 10, 0, chkDel.Checked, FstrGbn);
                    FstrTable = "_AN"; break;
                case "05":
                    cPmpaSpd.sSpd_enmPmpaADD(SS1, cPb.sSpdPmpaAdd_OP, cPb.nSpdPmpaAdd_OP, 10, 0, chkDel.Checked, FstrGbn);
                    FstrTable = "_OP"; break;
                case "06":
                    cPmpaSpd.sSpd_enmPmpaADD(SS1, cPb.sSpdPmpaAdd_Exam, cPb.nSpdPmpaAdd_Exam, 10, 0, chkDel.Checked, FstrGbn);
                    FstrTable = "_EXAM"; break;
                case "07":
                    cPmpaSpd.sSpd_enmPmpaADD(SS1, cPb.sSpdPmpaAdd_Xray, cPb.nSpdPmpaAdd_Xray, 10, 0, chkDel.Checked, FstrGbn);
                    FstrTable = "_XRAY"; break;
                default:
                    cPmpaSpd.sSpd_enmPmpaADD(SS1, cPb.sSpdPmpaAdd, cPb.nSpdPmpaAdd, 10, 0, chkDel.Checked, FstrGbn);
                    FstrTable = "_JIN"; break;
            }

            Screen_Display(clsDB.DbCon, FstrGbn, FstrChild);
            SS1.Select();
        }

        private void eSpdKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                cSpd.setEnterKey((FarPoint.Win.Spread.FpSpread)sender, clsSpread.enmSpdEnterKey.Right);
            }
            else if (e.KeyChar == (char)Keys.Down)
            {
                cSpd.setEnterKey((FarPoint.Win.Spread.FpSpread)sender, clsSpread.enmSpdEnterKey.Down);
            }
        }
        
        private void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmPmpaAdd.SDATE].Text == "")
            {
                SS1.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmPmpaAdd.SDATE].Text = clsPublic.GstrSysDate;
                if (e.Column != (int)clsPmpaPb.enmPmpaAdd.GBCHILD)
                {
                    clsSpread.gSdCboItemFindLeft(SS1, e.Row, (int)clsPmpaPb.enmPmpaAdd.GBCHILD, 1, "0");
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)             //닫기
            {
                this.Close();
            }
            else if (sender == this.btnSearch)      //조회
            {
                Screen_Display(clsDB.DbCon, FstrGbn, FstrChild);
            }
            else if (sender == this.btnSave)        //등록
            {
                eSaveData(clsDB.DbCon, FstrGbn);
                Screen_Display(clsDB.DbCon, FstrGbn, FstrChild);
            }
        }

        private void eSpreadEditChange(object sender, EditorNotifyEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            FpSpread o = (FpSpread)sender;

            if (e.Column == (int)clsPmpaPb.enmPmpaAdd.EDATE)
            {
                o.ActiveSheet.Cells[e.Row, (int)clsPmpaPb.enmPmpaAdd.Change].Text = "Y";
            }
        }

        private void Screen_Clear()
        {
            cSpd.Spread_All_Clear(SS1);
        }

        private void Screen_Display(PsmhDb pDbCon, string strGbn, string strChild)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            SS1.ActiveSheet.Rows.Count = 0;

            if (strGbn == "")
            {
                return;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.ROWID, ";
                SQL += ComNum.VBLF + "        TO_CHAR(a.EntDate, 'YYYY-MM-DD HH24:MI') ENTDATE, ";
                SQL += ComNum.VBLF + "        a.*                                               ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ADD a                   ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1                                             ";
                SQL += ComNum.VBLF + "    AND GUBUN = '" + strGbn + "'                          ";
                
                if (chkindata.Checked == true)
                {
                    SQL += ComNum.VBLF + "    AND (EDATE IS NULL OR DELDATE = '')              ";
                }

                if (chkDel.Checked == false)
                {
                    SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE = '')              ";
                }
                if (strChild != "*" && strChild != "")
                {
                    SQL += ComNum.VBLF + "    AND GBCHILD = '" + strChild + "'                  ";
                }
                SQL += ComNum.VBLF + "  ORDER By a.GBCHILD,a.PCODE                                         ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    SS1.ActiveSheet.Rows.Count += 10;
                    return;
                }
                else
                {
                    Display_Data(pDbCon, SS1, Dt, strGbn);
                }

                Dt.Dispose();
                Dt = null;

                SS1.ActiveSheet.Rows.Count += 30;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }
        }
        
        private void Display_Data(PsmhDb pDbCon, FpSpread Spd, DataTable Dt, string strGbn)
        {
            int i = 0, nRow = 0, nREAD = 0;

            if (Dt == null)
            {
                return;
            }

            Spd.ActiveSheet.Rows.Count = 0;

            nREAD = Dt.Rows.Count;

            for (i = 0; i < nREAD; i++)
            {
                nRow += 1;
                if (Spd.ActiveSheet.Rows.Count < nRow)
                {
                    Spd.ActiveSheet.Rows.Count = nRow;
                }

                clsSpread.gSdCboItemFindLeft(Spd, i, (int)clsPmpaPb.enmPmpaAdd.GBCHILD, 1, Dt.Rows[i]["GBCHILD"].ToString().Trim());
                
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.PCODE].Text    = Dt.Rows[i]["PCODE"].ToString().Trim();
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.NIGHT].Value   = Dt.Rows[i]["NIGHT"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.MIDNIGHT].Value = Dt.Rows[i]["MIDNIGHT"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.GBER].Value    = Dt.Rows[i]["GBER"].ToString().Trim() == "Y" ? "True" : "False"; 
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.HOLIDAY].Value = Dt.Rows[i]["HOLIDAY"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD1].Value    = Dt.Rows[i]["ADD1"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD2].Value    = Dt.Rows[i]["ADD2"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD3].Value    = Dt.Rows[i]["ADD3"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD4].Value    = Dt.Rows[i]["ADD4"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD5].Value    = Dt.Rows[i]["ADD5"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD6].Value    = Dt.Rows[i]["ADD6"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD7].Value    = Dt.Rows[i]["ADD7"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD8].Value    = Dt.Rows[i]["ADD8"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD9].Value    = Dt.Rows[i]["ADD9"].ToString().Trim() == "Y" ? "True" : "False";
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.SDATE].Text    = Dt.Rows[i]["SDATE"].ToString().Trim();
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.EDATE].Text    = Dt.Rows[i]["EDATE"].ToString().Trim();
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.DELDATE].Text  = Dt.Rows[i]["DELDATE"].ToString().Trim();
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ENTDATE].Text  = Dt.Rows[i]["ENTDATE"].ToString().Trim();
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ENTSABUN].Text = Dt.Rows[i]["ENTSABUN"].ToString().Trim();
                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ROWID].Text    = Dt.Rows[i]["ROWID"].ToString().Trim();

                Spd.ActiveSheet.Cells[i, (int)clsPmpaPb.enmPmpaAdd.GBCHILD].Locked = true;
                
                if (Dt.Rows[i]["DELDATE"].ToString().Trim() != "")
                {
                    Spd.ActiveSheet.Cells[i, 0].Locked = true;
                }
            }
            
            Spd.ActiveSheet.Rows.Count += 10;

        }
        
        private void eSaveData(PsmhDb pDbCon, string strGbn)
        {
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            int i = 0;
            int nLastRow = -1;
            int nstartRow = 0;

            cBAdd = new clsPmpaType.cBas_Add();

            Cursor.Current = Cursors.WaitCursor;

            nLastRow = SS1.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1;
            
            clsDB.setBeginTran(pDbCon);

            try
            {
                for (i = nstartRow; i < nLastRow; i++)
                {
                    cBAdd.GUBUN     = strGbn;
                    cBAdd.PCODE     = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.PCODE].Text;
                    cBAdd.GBCHILD   = VB.Left(SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.GBCHILD].Text, 1);
                    cBAdd.NIGHT     = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.NIGHT].Text == "True" ? "Y" : "";
                    cBAdd.MIDNIGHT  = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.MIDNIGHT].Text == "True" ? "Y" : "";
                    cBAdd.GBER      = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.GBER].Text == "True" ? "Y" : "";
                    cBAdd.HOLIDAY   = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.HOLIDAY].Text == "True" ? "Y" : "";
                    cBAdd.ADD1      = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD1].Text == "True" ? "Y" : "";
                    cBAdd.ADD2      = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD2].Text == "True" ? "Y" : "";
                    cBAdd.ADD3      = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD3].Text == "True" ? "Y" : "";
                    cBAdd.ADD4      = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD4].Text == "True" ? "Y" : "";
                    cBAdd.ADD5      = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD5].Text == "True" ? "Y" : "";
                    cBAdd.ADD6      = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD6].Text == "True" ? "Y" : "";
                    cBAdd.ADD7      = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD7].Text == "True" ? "Y" : "";
                    cBAdd.ADD8      = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD8].Text == "True" ? "Y" : "";
                    cBAdd.ADD9      = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ADD9].Text == "True" ? "Y" : "";
                    cBAdd.SDATE     = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.SDATE].Text;
                    cBAdd.EDATE     = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.EDATE].Text;
                    cBAdd.DELDATE   = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.DELDATE].Text;
                    cBAdd.ENTSABUN  = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ENTSABUN].Text;
                    cBAdd.ROWID     = SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ROWID].Text;
                  
                    if (SS1_Sheet1.Cells[i, 0].Text == "True")
                    {
                        SqlErr = cBAcct.del_BasAdd(pDbCon, cBAdd.ROWID, FstrTable, ref intRowAffected);
                    }
                    else if (SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.ROWID].Text == "")
                    {
                        #region 자료값 점검
                        if (cBAdd.PCODE == "")
                        {
                            ComFunc.MsgBox("산정코드 값이 없습니다.", "확인");
                            clsDB.setRollbackTran(pDbCon);
                            return;
                        }
                        else if (cBAdd.GBCHILD == "")
                        {
                            ComFunc.MsgBox("나이구분 값이 없습니다.", "확인");
                            clsDB.setRollbackTran(pDbCon);
                            return;
                        }
                        else if (cBAdd.SDATE == "")
                        {
                            ComFunc.MsgBox("적용일자 값이 없습니다.", "확인");
                            clsDB.setRollbackTran(pDbCon);
                            return;
                        }
                        #endregion

                        SqlErr = cBAcct.ins_BasAdd(pDbCon, cBAdd, ref intRowAffected);
                    }
                    else if (SS1_Sheet1.Cells[i, (int)clsPmpaPb.enmPmpaAdd.Change].Text.Trim() == "Y")
                    {
                        SqlErr = cBAcct.up_BasAdd(pDbCon, cBAdd, ref intRowAffected);
                    }

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }
    }
}
