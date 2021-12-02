using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB;
using FarPoint.Win.Spread;
using System.Threading;
using System.Drawing;
using ComBase.Controls;

namespace ComHpcLibB
{
    /// <summary>
    /// Class Name      : ComHpcLibB
    /// File Name       : frmHcPendList.cs
    /// Description     : 보류대장
    /// Author          : 김민철
    /// Create Date     : 2018-11-26
    /// Update History  : 
    /// <seealso cref="\Hic\HcAct\Frm보류대장.frm(Frm보류대장)"/>
    /// </summary>
    public partial class frmHcPendList : Form
    {
        public delegate void SetGnWrtnoValue(long GnWrtno);
        public static event SetGnWrtnoValue rSetGnWrtnoValue;

        BasPcconfigService basPcconfigService = null;

        frmHcPanExamResultRegChg FrmHcPanExamResultRegChg = null;   //일반검진 결과 등록
        frmHaExamResultReg FrmHaExamResultReg = null;               //종검 결과 등록

        Thread thread;
        FpSpread spd;

        string FstrSName = string.Empty;
        string FstrFDate = string.Empty;
        string FstrTDate = string.Empty;
        string FstrJob = string.Empty;
        string FstrChul = string.Empty;
        string FstrMsg = string.Empty;
        string FstrGubun = string.Empty;
        bool bolSort = false;

        public frmHcPendList()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            basPcconfigService = new BasPcconfigService();

            this.Load               += new EventHandler(eFormLoad);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.btnSave.Click      += new EventHandler(eBtnClick);
            this.SS1.CellClick      += new CellClickEventHandler(eSpdClick);
            this.SS1.EditModeOff    += new EventHandler(eSpdEditOff);
            this.SS1.ButtonClicked  += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column == (int)clsHcSpd.enmHcPendList.chk01)
            {
                clsSpread cSpd = new clsSpread();

                if (SS1.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcPendList.chk01].Text == "True")
                {
                    cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.Red);
                }
                else
                {
                    if (SS1.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcPendList.CHANGE].Text == "Y")
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

        void eSpdEditOff(object sender, EventArgs e)
        {
            int nRow = SS1.ActiveSheet.ActiveRowIndex;
            int nCol = SS1.ActiveSheet.ActiveColumnIndex;

            if (nCol == 0)
            {
                return;
            }

            if (SS1.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPendList.CHANGE].Text.Trim() != "Y")
            {
                clsSpread cSpd = new clsSpread();

                SS1.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcPendList.CHANGE].Text = "Y";
                cSpd.setSpdForeColor(SS1, nRow, 0, nRow, SS1_Sheet1.ColumnCount - 1, Color.Blue);
                cSpd = null;
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            clsHcSpd cHSpd = new clsHcSpd();

            cHSpd.sSpd_enmHcPendList(SS1, cHSpd.sHcPendList, cHSpd.nHcPendList, 10, 0);

            ComFunc.ReadSysDate(clsDB.DbCon);

            ComFunc CF = new ComFunc();

            if (clsHcVariable.GstrHicPart == "1")   //종검
            {
                rdoGubun3.Checked = true;
                dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -365);
                dtpTDate.Text = clsPublic.GstrSysDate;

            }
            else if (clsHcVariable.GstrHicPart == "2")  //일검
            {
                rdoGubun2.Checked = true;
                dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -90);
                dtpTDate.Text = clsPublic.GstrSysDate;

            }

            txtSName.Text = "";

            cHSpd = null;
            CF = null;

            clsHcVariable.GstrHicPart = basPcconfigService.GetConfig_Code(clsCompuInfo.gstrCOMIP, "검진센터부서");

            //if (clsHcVariable.GstrHicPart == "1")   //종검
            //{
            //    rdoGubun3.Checked = true;
            //}
            //else if (clsHcVariable.GstrHicPart == "2")  //일검
            //{
            //    rdoGubun2.Checked = true;
            //}
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                Screen_Display(clsDB.DbCon, SS1);
            }
            else if (sender == btnSave)
            {
                eSave(clsDB.DbCon);
                //this.Close();
                return;
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1 && e.Column != 12)
            {
                if (SS1.ActiveSheet.Cells[e.Row, 1].Text == "종검")
                {
                    FrmHaExamResultReg = new frmHaExamResultReg(SS1.ActiveSheet.Cells[e.Row, 3].Text.To<long>());
                    FrmHaExamResultReg.WindowState = FormWindowState.Maximized;
                    FrmHaExamResultReg.StartPosition = FormStartPosition.CenterScreen;
                    FrmHaExamResultReg.ShowDialog(this);
                    
                }
                else
                {
                    FrmHcPanExamResultRegChg = new frmHcPanExamResultRegChg(SS1.ActiveSheet.Cells[e.Row, 3].Text.To<long>(),"","");
                    FrmHcPanExamResultRegChg.WindowState = FormWindowState.Maximized;
                    FrmHcPanExamResultRegChg.StartPosition = FormStartPosition.CenterScreen;
                    FrmHcPanExamResultRegChg.ShowDialog(this);
                }
            }
            else
            {
                SS1.ActiveSheet.Cells[e.Row, 12].Locked = false;
            }
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(SS1, e.Column, ref bolSort, true);
            }
        }


        void eSave(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int i = 0;
            int nRow = 0;

            string strGubun     = string.Empty;
            string strDel       = string.Empty;
            string strExamName  = string.Empty;
            string strSayu      = string.Empty;
            string strEnd       = string.Empty;
            string strEndDate   = string.Empty;
            string strSpecDate  = string.Empty;
            string strSpecSABUN = string.Empty;
            string strROWID     = string.Empty;
            string strChange    = string.Empty;

            if (SS1.ActiveSheet.RowCount == 0)
            {
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(pDbCon);

                nRow = SS1.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1;

                for (i = 0; i < nRow; i++)
                {
                    strGubun    = SS1.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.GUBUN].Text.Trim();
                    strDel      = SS1.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.chk01].Text.Trim();
                    strExamName = SS1.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.EXAMS].Text.Trim();
                    strSayu     = SS1.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.SAYU].Text.Trim();
                    strEnd      = SS1.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.END].Text.Trim();
                    strEndDate  = SS1.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.ENDDATE].Text.Trim();
                    strSpecDate = SS1.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.SPECDATE].Text.Trim();
                    strSpecSABUN= SS1.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.SPECSABUN].Text.Trim();
                    strROWID    = SS1.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.ROWID].Text.Trim();
                    strChange   = SS1.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.CHANGE].Text.Trim();

                    SQL = "";

                    if (strDel == "True")
                    {
                        SQL = "DELETE " + ComNum.DB_PMPA + "HIC_PENDING " + " WHERE ROWID = '" + strROWID + "' ";
                    }
                    else if (strChange == "Y")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "HIC_PENDING ";
                        SQL += ComNum.VBLF + "    SET ExamName='" + strExamName + "',   ";
                        if (strEnd == "True" && strEndDate == "")
                        {
                            SQL += ComNum.VBLF + "        EndDate  = SYSDATE,          ";
                            SQL += ComNum.VBLF + "        EndSabun = " + clsType.User.IdNumber + ",  ";
                        }
                        else if (strEnd == "False" && strEndDate != "")
                        {
                            SQL += ComNum.VBLF + "        EndDate  = '',                            ";
                            SQL += ComNum.VBLF + "        EndSabun = '',                            ";
                        }

                        //if (strEnd == "True" && strSpecDate == "" && strGubun =="일검")
                        //{
                        //    SQL += ComNum.VBLF + "        SPECDATE = = SYSDATE,          ";
                        //}
                        //else if (!strSpecDate.IsNullOrEmpty())
                        //{
                        //    SQL += ComNum.VBLF + "        SPECDATE = TO_DATE('" + strSpecDate + "', 'YYYY-MM-DD'), ";
                        //}

                        if (!strSpecDate.IsNullOrEmpty())
                        {
                            SQL += ComNum.VBLF + "        SPECDATE = TO_DATE('" + strSpecDate + "', 'YYYY-MM-DD'), ";
                        }

                        if (strSpecSABUN.IsNullOrEmpty())
                        {
                            SQL += ComNum.VBLF + "        SPECSABUN = " + clsType.User.IdNumber + ",  ";
                        }

                        SQL += ComNum.VBLF + "        Sayu='" + strSayu + "'            ";
                        SQL += ComNum.VBLF + "  WHERE ROWID='" + strROWID + "'          ";
                    }
                    
                    if (SQL != "")
                    {
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }


                clsDB.setCommitTran(pDbCon);

                ComFunc.MsgBox("저장하였습니다.");

                Cursor.Current = Cursors.Default;

                Screen_Display(pDbCon, SS1);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        delegate void threadSpdTypeDelegate(FpSpread spd, DataTable dt);

        void Screen_Display(PsmhDb pDbCon, FpSpread Spd)
        {
            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            spd = SS1;

            FstrSName = "";
            if (txtSName.Text.Trim() != "") { FstrSName = txtSName.Text.Trim(); }
            if (dtpFDate.Text.Trim() != "") { FstrFDate = dtpFDate.Text.Trim(); }
            if (dtpTDate.Text.Trim() != "") { FstrTDate = dtpTDate.Text.Trim(); }
            if (rdoJob0.Checked)
            {
                FstrJob = "ALL";
            }
            else if (rdoJob1.Checked)
            {
                FstrJob = "Y";
            }
            else
            {
                FstrJob = "N";
            }

            if (chkChul0.Checked && chkChul1.Checked)
            {
                FstrChul = "ALL";
            }
            else if (chkChul0.Checked)
            {
                FstrChul = "내원";
            }
            else if (chkChul1.Checked)
            {
                FstrChul = "출장";
            }

            if (rdoGubun1.Checked == true)  //전체
            {
                FstrGubun = "1";
            }
            else if (rdoGubun2.Checked == true) //일검
            {
                FstrGubun = "2";
            }
            else if (rdoGubun3.Checked == true) //종검
            {
                FstrGubun = "3";
            }


            try
            {
                //스레드 시작
                thread = new Thread(tProcess);
                thread.Start();

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        void tProcess()
        {
            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));

            DataTable dt = null;

            dt = sel_HcPendList(clsDB.DbCon, FstrGubun, FstrFDate, FstrTDate);

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));
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
                    spd.ActiveSheet.RowCount = Dt.Rows.Count;

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.GUBUN].Text      = Dt.Rows[i]["GUBUN"].ToString().Trim() == "1" ? "일검" : "종검";
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.JEPDATE].Text    = Dt.Rows[i]["JepDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.WRTNO].Text      = Dt.Rows[i]["WRTNO"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.SNAME].Text      = Dt.Rows[i]["SNAME"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.LTDNAME].Text    = Dt.Rows[i]["LtdName"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.HPHONE].Text     = Dt.Rows[i]["HTEL"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.CHUL].Text       = Dt.Rows[i]["GbChul"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.EXAMS].Text      = Dt.Rows[i]["ExamName"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.SAYU].Text       = Dt.Rows[i]["Sayu"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.END].Text        = Dt.Rows[i]["EndDate"].ToString().Trim() == "" ? "" : "True";
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.ENDDATE].Text    = Dt.Rows[i]["EndDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.SPECDATE].Text   = Dt.Rows[i]["SPECDATE"].ToString().Trim();//검체제출일
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.SPECSABUN].Text = Dt.Rows[i]["SPECSABUN"].ToString().Trim();//검체제출일
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.ENTSABUN].Text   = Dt.Rows[i]["ENTSABUN"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.ENDSABUN].Text   = Dt.Rows[i]["ENDSABUN"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.TONGBODATE].Text = Dt.Rows[i]["TONGBODATE"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.TONGBOGBN].Text  = Dt.Rows[i]["TONGBOGBN"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcPendList.ROWID].Text      = Dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                Dt.Dispose();
                Dt = null;
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

        DataTable sel_HcPendList(PsmhDb pDbCon, string argGubun, string argFDate, string argTDate)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            try
            {
                if (argGubun == "1")    //전체
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.WRTNO WRTNO,b.Pano,b.SName SName,b.LtdCode,                 ";
                    SQL += ComNum.VBLF + "        FC_HIC_LTDNAME(b.LtdCode) LtdName, DECODE(RTRIM(c.HPhone),'', c.Tel, c.HPhone) HTEL,                  ";
                    SQL += ComNum.VBLF + "        DECODE(b.GbChul, 'Y', '출장', '') GbChul, a.ExamName,a.Sayu,a.GUBUN,SPECSABUN,                        ";  
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_INSA_MST_KORNAME(a.EntSabun) ENTSABUN, TO_CHAR(ENDDATE,'YYYY-MM-DD') ENDDATE,           ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_INSA_MST_KORNAME(a.EndSabun) ENDSABUN, a.ROWID, TO_CHAR(SPECDATE,'YYYY-MM-DD') SPECDATE,";
                    SQL += ComNum.VBLF + "        TO_CHAR(b.TONGBODATE, 'YYYY-MM-DD') TONGBODATE, DECODE(WEBPRINTSEND, '','우편','알림톡') TONGBOGBN     ";
                    SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_PENDING a,                                                                   ";
                    SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "HIC_JEPSU b,                                                                     ";
                    SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "HIC_PATIENT c                                                                    ";
                    SQL += ComNum.VBLF + " WHERE 1 = 1                                                                                                  ";
                    SQL += ComNum.VBLF + " AND b.JepDate >= TO_DATE('" + argFDate + "','YYYY-MM-DD')               ";
                    SQL += ComNum.VBLF + " AND b.JepDate <= TO_DATE('" + argTDate + "','YYYY-MM-DD')               ";
                    if (FstrJob == "Y")
                    {
                        SQL += ComNum.VBLF + "   AND a.EndDate IS NOT NULL ";
                    }
                    else if (FstrJob == "N")
                    {
                        SQL += ComNum.VBLF + "   AND a.EndDate IS NULL ";
                    }
                    SQL += ComNum.VBLF + " AND a.GUBUN = '1' ";
                    SQL += ComNum.VBLF + " AND a.WRTNO=b.WRTNO(+)           ";
                    SQL += ComNum.VBLF + " AND b.Pano=c.Pano(+)             ";
                    
                    if (FstrChul == "내원")
                    {
                        SQL += ComNum.VBLF + " AND b.GbChul = 'N' ";
                    }
                    else if (FstrChul == "출장")
                    {
                        SQL += ComNum.VBLF + " AND b.GbChul = 'Y' ";
                    }

                    //if (FstrGubun == "2")   //일검
                    //{
                    //    SQL += ComNum.VBLF + " AND a.GUBUN = '1' ";
                    //}
                    //else if (FstrGubun == "3")  //종검
                    //{
                    //    SQL += ComNum.VBLF + " AND a.GUBUN = '2' ";
                    //}

                    //if (FstrSName != "") { SQL += ComNum.VBLF + " AND b.SName = '" + FstrSName + "' "; 
                    if (FstrSName != "") { SQL += ComNum.VBLF + " AND b.SName LIKE '%" + FstrSName + "%' "; }
                    SQL += ComNum.VBLF + "  UNION ALL                                                                                                ";
                    SQL += ComNum.VBLF + " SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.WRTNO WRTNO,b.Pano,b.SName SName,b.LtdCode,              ";
                    SQL += ComNum.VBLF + "        FC_HIC_LTDNAME(b.LtdCode) LtdName, DECODE(RTRIM(c.HPhone),'', c.Tel, c.HPhone) HTEL,               ";
                    SQL += ComNum.VBLF + "        '' GbChul, a.ExamName,a.Sayu,a.GUBUN, SPECSABUN,                                                   ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_INSA_MST_KORNAME(a.EntSabun) ENTSABUN, TO_CHAR(ENDDATE,'YYYY-MM-DD') ENDDATE,        ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_INSA_MST_KORNAME(a.EndSabun) ENDSABUN, a.ROWID, TO_CHAR(SPECDATE,'YYYY-MM-DD') SPECDATE,";
                    SQL += ComNum.VBLF + "        TO_CHAR(b.PRTDATE, 'YYYY-MM-DD') TONGBODATE, DECODE(WEBPRINTSEND, '','우편','알림톡') TONGBOGBN    ";
                    SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_PENDING a,                                                         ";
                    SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "HEA_JEPSU b,                                                           ";
                    SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "HIC_PATIENT c                                                          ";
                    SQL += ComNum.VBLF + " WHERE 1 = 1                                                                                        ";
                    SQL += ComNum.VBLF + " AND b.SDate >= TO_DATE('" + argFDate + "','YYYY-MM-DD')               ";
                    SQL += ComNum.VBLF + " AND b.SDate <= TO_DATE('" + argTDate + "','YYYY-MM-DD')               ";
                    if (FstrJob == "Y")
                    {
                        SQL += ComNum.VBLF + "   AND a.EndDate IS NOT NULL ";
                    }
                    else if (FstrJob == "N")
                    {
                        SQL += ComNum.VBLF + "   AND a.EndDate IS NULL ";
                    }
                    SQL += ComNum.VBLF + " AND a.GUBUN = '2' ";
                    SQL += ComNum.VBLF + " AND a.WRTNO=b.WRTNO(+)           ";
                    SQL += ComNum.VBLF + " AND b.Pano=c.Pano(+)             ";

                    //if (FstrGubun == "2")   //일검
                    //{
                    //    SQL += ComNum.VBLF + " AND a.GUBUN = '1' ";
                    //}
                    //else if (FstrGubun == "3")  //종검
                    //{
                    //    SQL += ComNum.VBLF + " AND a.GUBUN = '2' ";
                    //}

                    //if (FstrSName != "") { SQL += ComNum.VBLF + " AND b.SName = '" + FstrSName + "' "; }
                    if (FstrSName != "") { SQL += ComNum.VBLF + " AND b.SName LIKE '%" + FstrSName + "%' "; }
                    SQL += ComNum.VBLF + "ORDER BY SName, JepDate, WRTNO ";
                }
                else if (argGubun == "2")   //일검
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.WRTNO,b.Pano,b.SName,b.LtdCode,                   ";
                    SQL += ComNum.VBLF + "        FC_HIC_LTDNAME(b.LtdCode) LtdName, DECODE(RTRIM(c.HPhone),'', c.Tel, c.HPhone) HTEL,        ";
                    SQL += ComNum.VBLF + "        DECODE(b.GbChul, 'Y', '출장', '') GbChul, a.ExamName,a.Sayu,a.GUBUN,SPECSABUN,              ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_INSA_MST_KORNAME(a.EntSabun) ENTSABUN, TO_CHAR(ENDDATE,'YYYY-MM-DD') ENDDATE, ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_INSA_MST_KORNAME(a.EndSabun) ENDSABUN, a.ROWID, TO_CHAR(SPECDATE,'YYYY-MM-DD') SPECDATE, ";
                    SQL += ComNum.VBLF + "        TO_CHAR(b.TONGBODATE, 'YYYY-MM-DD') TONGBODATE, DECODE(WEBPRINTSEND, '','우편','알림톡') TONGBOGBN    ";
                    SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_PENDING a,                                                         ";
                    SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "HIC_JEPSU b,                                                           ";
                    SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "HIC_PATIENT c                                                          ";
                    SQL += ComNum.VBLF + " WHERE 1 = 1                                                                                        ";
                    SQL += ComNum.VBLF + " AND b.JepDate >= TO_DATE('" + argFDate + "','YYYY-MM-DD')               ";
                    SQL += ComNum.VBLF + " AND b.JepDate <= TO_DATE('" + argTDate + "','YYYY-MM-DD')               ";
                    if (FstrJob == "Y")
                    {
                        SQL += ComNum.VBLF + "   AND a.EndDate IS NOT NULL ";
                    }
                    else if (FstrJob == "N")
                    {
                        SQL += ComNum.VBLF + "   AND a.EndDate IS NULL ";
                    }

                    SQL += ComNum.VBLF + " AND a.WRTNO=b.WRTNO(+)           ";
                    SQL += ComNum.VBLF + " AND b.Pano=c.Pano(+)             ";

                    if (FstrChul == "내원")
                    {
                        SQL += ComNum.VBLF + " AND b.GbChul = 'N' ";
                    }
                    else if (FstrChul == "출장")
                    {
                        SQL += ComNum.VBLF + " AND b.GbChul = 'Y' ";
                    }

                    SQL += ComNum.VBLF + " AND a.GUBUN = '1' ";

                    //if (FstrSName != "") { SQL += ComNum.VBLF + " AND b.SName = '" + FstrSName + "' "; }
                    if (FstrSName != "") { SQL += ComNum.VBLF + " AND b.SName LIKE '%" + FstrSName + "%' "; }
                    SQL += ComNum.VBLF + "ORDER BY b.SName,a.JepDate,a.WRTNO ";
                }
                else if (argGubun == "3")   //종검
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.WRTNO,b.Pano,b.SName,b.LtdCode,                   ";
                    SQL += ComNum.VBLF + "        FC_HIC_LTDNAME(b.LtdCode) LtdName, DECODE(RTRIM(c.HPhone),'', c.Tel, c.HPhone) HTEL,        ";
                    SQL += ComNum.VBLF + "        '' GbChul, a.ExamName,a.Sayu,a.GUBUN,SPECSABUN,                                             ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_INSA_MST_KORNAME(a.EntSabun) ENTSABUN, TO_CHAR(ENDDATE,'YYYY-MM-DD') ENDDATE, ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_INSA_MST_KORNAME(a.EndSabun) ENDSABUN, a.ROWID,TO_CHAR(SPECDATE,'YYYY-MM-DD') SPECDATE,";
                    SQL += ComNum.VBLF + "        TO_CHAR(b.PRTDATE, 'YYYY-MM-DD') TONGBODATE, DECODE(WEBPRINTSEND, '','우편','알림톡') TONGBOGBN    ";
                    SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_PENDING a,                                                         ";
                    SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "HEA_JEPSU b,                                                           ";
                    SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "HIC_PATIENT c                                                          ";
                    SQL += ComNum.VBLF + " WHERE 1 = 1                                                                                        ";
                    SQL += ComNum.VBLF + " AND b.SDate >= TO_DATE('" + argFDate + "','YYYY-MM-DD')               ";
                    SQL += ComNum.VBLF + " AND b.SDate <= TO_DATE('" + argTDate + "','YYYY-MM-DD')               ";
                    if (FstrJob == "Y")
                    {
                        SQL += ComNum.VBLF + "   AND a.EndDate IS NOT NULL ";
                    }
                    else if (FstrJob == "N")
                    {
                        SQL += ComNum.VBLF + "   AND a.EndDate IS NULL ";
                    }

                    SQL += ComNum.VBLF + " AND a.WRTNO=b.WRTNO(+)           ";
                    SQL += ComNum.VBLF + " AND b.Pano=c.Pano(+)             ";

                    SQL += ComNum.VBLF + " AND a.GUBUN = '2' ";

                    //if (FstrSName != "") { SQL += ComNum.VBLF + " AND b.SName = '" + FstrSName + "' "; }
                    if (FstrSName != "") { SQL += ComNum.VBLF + " AND b.SName LIKE '%" + FstrSName + "%' "; }
                    SQL += ComNum.VBLF + "ORDER BY SName,JepDate,a.WRTNO ";
                }
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

        
    }
}
