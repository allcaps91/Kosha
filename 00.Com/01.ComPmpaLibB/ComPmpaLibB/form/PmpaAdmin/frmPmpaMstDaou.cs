using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 신용카드 수동등록(다우데이터)
/// Author : 박병규
/// Create Date : 2017.09.13
/// </summary>
/// <history>
/// </history>
/// <seealso cref="frm신용카드등록_다우.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaMstCardDaou : Form
    {
        clsPmpaFunc CPF = null;
        ComFunc CF = null;
        clsUser CU = null;
        clsSpread CS = null;
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        DataTable Dt = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        string FstrRowID = string.Empty;
        string FstrRowID_Del = string.Empty; //당일삭제건
        string FstrActDate_Del = string.Empty; //당일삭제건

        public frmPmpaMstCardDaou()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eFrm_Load);

            //SelectedIndexChanged
            this.cboInIO.SelectedIndexChanged += new EventHandler(eCtl_Selected);
            this.cboInIO.MouseWheel += new MouseEventHandler(eCtl_MouseWheel);
            this.cboInDept.SelectedIndexChanged += new EventHandler(eCtl_Selected);
            this.cboInDept.MouseWheel += new MouseEventHandler(eCtl_MouseWheel);
            this.cboInFiCode.SelectedIndexChanged += new EventHandler(eCtl_Selected);
            this.cboInFiCode.MouseWheel += new MouseEventHandler(eCtl_MouseWheel);

            //KeyPress 이벤트
            this.txtPtno.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtPart.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.dtpFdate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.dtpTdate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            this.txtInPtno.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.dtpInDate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtInPart.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtInCardNo.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtInPer.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.dtpInTrsDate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.dtpInTrsTime.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboInMonth.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboInMonth.MouseWheel += new MouseEventHandler(eCtl_MouseWheel);
            this.txtInAmt.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboInFiCode.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtInOriginNo.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboInIO.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboInDept.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            //Click 이벤트
            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnPrint.Click += new EventHandler(eCtl_Click);
            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnDelete.Click += new EventHandler(eCtl_Click);
            this.btnCancel.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_MouseWheel(object sender, MouseEventArgs e)
        {
            ComboBox CB = sender as ComboBox;

            if (CB.Focused == false)
                ((HandledMouseEventArgs)e).Handled = true;
        }

        private void eCtl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.dtpInTrsTime)
                dtpInTrsTime.Text = string.Format("{0:00:00}", dtpInTrsTime.Text);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch) { READ_PROCESS(clsDB.DbCon); }
            else if (sender == this.btnPrint) { PRINT_PROCESS(); }
            else if (sender == this.btnSave) { SAVE_PROCESS(clsDB.DbCon); }
            else if (sender == this.btnDelete) { DELETE_PROCESS(clsDB.DbCon); }
            else if (sender == this.btnCancel)
            {
                TEXT_CLEAR();
                txtInPtno.Focus();
            }
            else if (sender == this.btnExit)
                this.Close();
        }

        private void DELETE_PROCESS(PsmhDb pDbCon)
        {
            if (ComQuery.IsJobAuth(this, "D", pDbCon) == false) { return; }     //권한확인

            if (FstrActDate_Del != clsPublic.GstrSysDate)
            {
                ComFunc.MsgBox("당일건은 당일 삭제만 가능합니다.", "확인");
                return;
            }

            if (FstrRowID_Del == "")
            {
                ComFunc.MsgBox("삭제 대상을 선택하시기 바랍니다.", "확인");
                return;
            }

            if (DialogResult.Yes == ComFunc.MsgBoxQ("선택한 수동카드내역을 삭제하시겠습니까?", "선택", MessageBoxDefaultButton.Button1))
            {
                Cursor.Current = Cursors.WaitCursor;
                 
                clsDB.setBeginTran(pDbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "CARD_APPROV_DEL ";
                    SQL += ComNum.VBLF + "        (CARDSEQNO, ACTDATE, CDSEQNO, ";
                    SQL += ComNum.VBLF + "         PANO, JUMIN, SNAME, ";
                    SQL += ComNum.VBLF + "         GBIO, DEPTCODE, PCODE, ";
                    SQL += ComNum.VBLF + "         PART, SEQNO, TRANHEADER, ";
                    SQL += ComNum.VBLF + "         TEMINALID, TRANDATE, INPUTMETHOD, ";
                    SQL += ComNum.VBLF + "         INSTPERIOD, TRADEAMT, TRXAMT, ";
                    SQL += ComNum.VBLF + "         SERVICEAMT, APPROVDATE, FINAME, ";
                    SQL += ComNum.VBLF + "         ACCEPTERNAME, FICODE, ACCOUNTNO, ";
                    SQL += ComNum.VBLF + "         ORIGINNO, ORIGINDATE, CardNo, ";
                    SQL += ComNum.VBLF + "         PERIOD, FULLCARDNO, Remark, ";
                    SQL += ComNum.VBLF + "         ENTSABUN, ENTERDATE, Gubun, ";
                    SQL += ComNum.VBLF + "         CashBun, OgAmt, PTGUBUN)  ";
                    SQL += ComNum.VBLF + "  SELECT CARDSEQNO, ACTDATE, CDSEQNO, ";
                    SQL += ComNum.VBLF + "         PANO, JUMIN, SNAME, ";
                    SQL += ComNum.VBLF + "         GBIO, DEPTCODE, PCODE, ";
                    SQL += ComNum.VBLF + "         PART, SEQNO, TRANHEADER, ";
                    SQL += ComNum.VBLF + "         TEMINALID, TRANDATE, INPUTMETHOD, ";
                    SQL += ComNum.VBLF + "         INSTPERIOD, TRADEAMT, TRXAMT, ";
                    SQL += ComNum.VBLF + "         SERVICEAMT, APPROVDATE, FINAME, ";
                    SQL += ComNum.VBLF + "         ACCEPTERNAME, FICODE, ACCOUNTNO, ";
                    SQL += ComNum.VBLF + "         ORIGINNO, ORIGINDATE, CardNo, ";
                    SQL += ComNum.VBLF + "         PERIOD, FULLCARDNO, Remark, ";
                    SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + ", SYSDATE, Gubun, ";
                    SQL += ComNum.VBLF + "         CashBun, OgAmt, PTGUBUN ";
                    SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
                    SQL += ComNum.VBLF + "   WHERE 1        = 1 ";
                    SQL += ComNum.VBLF + "     AND ROWID    = '" + FstrRowID_Del + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + " DELETE ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
                    SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
                    SQL += ComNum.VBLF + "    AND ROWID         = '" + FstrRowID_Del + "' ";
                    SQL += ComNum.VBLF + "    AND ACTDATE       = TO_DATE('" + FstrActDate_Del + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND INPUTMETHOD   = 'T' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);
                    ComFunc.MsgBox("삭제되었습니다.", "알림");
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            READ_PROCESS(pDbCon);
        }

        private void SAVE_PROCESS(PsmhDb pDbCon)
        {
            string strBun = string.Empty;
            string strPCode = string.Empty;
            string strTranHead = string.Empty;
            long nAmt = 0;
            long nCardSeqNo = 0;
            long nSeqNo = 0;

            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            if (txtInPtno.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 입력하시기 바랍니다.", "확인");
                txtInPtno.Focus();
                return;
            }

            if (rdoInBun0.Checked == false && rdoInBun1.Checked == false)
            {
                ComFunc.MsgBox("카드 또는 현금을 선택하시기 바랍니다.", "확인");
                return;
            }

            if (string.Compare(dtpInDate.Text, clsPublic.GstrSysDate) < 0)
            {
                ComFunc.MsgBox("작업일자는 현재날짜보다 작을수 없습니다.", "확인");
                dtpInDate.Focus();
                return;
            }

            if (txtInPart.Text.Trim() == "")
            {
                ComFunc.MsgBox("수납조를 입력하시기 바랍니다.", "확인");
                txtInPart.Focus();
                return;
            }

            if (chkOg.Checked == true && cboInDept.Text.Trim() != "OG")
            {
                ComFunc.MsgBox("산전금액 승인은 산부인과만 가능합니다.", "확인");
                return;
            }

            if (rdoInBun0.Checked == true)
                strBun = "1";
            else
                strBun = "2";

            nAmt = Convert.ToInt64(VB.Val(VB.Replace(txtInAmt.Text, ",","")));
            if (nAmt > 0)
            {
                strPCode = "SUP+";
                strTranHead = "1";
            }
            else
            {
                strPCode = "SUP-";
                strTranHead = "2";
                nAmt = nAmt * -1;
            }

            Cursor.Current = Cursors.WaitCursor;
             
            clsDB.setBeginTran(pDbCon);

            try
            {
                if (FstrRowID == "")
                {
                    nCardSeqNo = CPF.GET_NEXT_CARDSEQNO(pDbCon);
                    nSeqNo = CPF.GET_NEXT_CDSEQNO(pDbCon);

                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "CARD_APPROV ";
                    SQL += ComNum.VBLF + "        (CARDSEQNO, ACTDATE, CDSEQNO, ";
                    SQL += ComNum.VBLF + "         PANO, JUMIN, SNAME, ";
                    SQL += ComNum.VBLF + "         GBIO, DEPTCODE, PCODE, ";
                    SQL += ComNum.VBLF + "         PART, SEQNO, TRANHEADER, ";
                    SQL += ComNum.VBLF + "         TEMINALID, TRANDATE, INPUTMETHOD,";
                    SQL += ComNum.VBLF + "         INSTPERIOD, TRADEAMT, TRXAMT, ";
                    SQL += ComNum.VBLF + "         SERVICEAMT, APPROVDATE, FINAME, ";
                    SQL += ComNum.VBLF + "         ACCEPTERNAME, FICODE, ACCOUNTNO, ";
                    SQL += ComNum.VBLF + "         ORIGINNO, ORIGINDATE, CardNo, ";
                    SQL += ComNum.VBLF + "         PERIOD, FULLCARDNO, Remark,";

                    if (chkOg.Checked == true)
                    {
                        SQL += ComNum.VBLF + "     ENTSABUN, ENTERDATE, Gubun, ";
                        SQL += ComNum.VBLF + "     CashBun, OgAmt, PtGubun) VALUES (";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "     ENTSABUN, ENTERDATE, Gubun, ";
                        SQL += ComNum.VBLF + "     CashBun,PtGubun ) VALUES (";

                    }

                    SQL += ComNum.VBLF + "         " + nCardSeqNo + ", ";
                    SQL += ComNum.VBLF + "         TO_DATE('" + dtpInDate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + txtInPtno.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtInJumin.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtInSname.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + cboInIO.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + cboInDept.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + strPCode + "', ";
                    SQL += ComNum.VBLF + "         '" + txtInPart.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + strTranHead + "', ";
                    SQL += ComNum.VBLF + "         '', ";
                    SQL += ComNum.VBLF + "         TO_DATE( '" + dtpInTrsDate.Text + dtpInTrsTime.Text + "','YYYY-MM-DD HH24:MI'), ";
                    SQL += ComNum.VBLF + "         'T', ";
                    SQL += ComNum.VBLF + "         '" + cboInMonth.Text + "', ";
                    SQL += ComNum.VBLF + "         " + nAmt + ", ";
                    SQL += ComNum.VBLF + "         0, ";
                    SQL += ComNum.VBLF + "         0, ";
                    SQL += ComNum.VBLF + "         TO_DATE( '" + dtpInTrsDate.Text + dtpInTrsTime.Text + "','YYYY-MM-DD HH24:MI'), ";
                    SQL += ComNum.VBLF + "         '', ";
                    SQL += ComNum.VBLF + "         '" + txtInFiName.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + cboInFiCode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '', ";
                    SQL += ComNum.VBLF + "         '" + txtInOriginNo.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         TO_DATE('" + dtpInTrsDate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         '" + txtInCardNo.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtInPer.Text.Trim() + "', ";

                    if (chkOg.Checked == true)
                    {
                        SQL += ComNum.VBLF + "     '', ";
                        SQL += ComNum.VBLF + "     '', ";
                        SQL += ComNum.VBLF + "     '" + clsType.User.Sabun + "', ";
                        SQL += ComNum.VBLF + "     SYSDATE, ";
                        SQL += ComNum.VBLF + "     '" + strBun + "', ";
                        SQL += ComNum.VBLF + "     '', ";
                        SQL += ComNum.VBLF + "     " + nAmt + ", ";
                        SQL += ComNum.VBLF + "     '3' ) ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "     '', ";
                        SQL += ComNum.VBLF + "     '', ";
                        SQL += ComNum.VBLF + "     '" + clsType.User.Sabun + "', ";
                        SQL += ComNum.VBLF + "     SYSDATE, ";
                        SQL += ComNum.VBLF + "     '" + strBun + "', ";
                        SQL += ComNum.VBLF + "     '', ";
                        SQL += ComNum.VBLF + "     '3' ) ";
                    }
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "CARD_APPROV ";
                    SQL += ComNum.VBLF + "    SET GBIO          = '" + cboInIO.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        DEPTCODE      = '" + cboInDept.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PCODE         = '" + strPCode + "', ";
                    SQL += ComNum.VBLF + "        ACTDATE       = TO_DATE('" + dtpInDate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "        TRANHEADER    = '" + strTranHead + "', ";
                    SQL += ComNum.VBLF + "        TRANDATE      = TO_DATE( '" + dtpInTrsDate.Text + dtpInTrsTime.Text + "','YYYY-MM-DD HH24:MI'), ";
                    SQL += ComNum.VBLF + "        INSTPERIOD    = '" + cboInMonth.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        TRADEAMT      = " + nAmt + ", ";
                    SQL += ComNum.VBLF + "        APPROVDATE    = TO_DATE( '" + dtpInTrsDate.Text + dtpInTrsTime.Text + "','YYYY-MM-DD HH24:MI'), ";
                    SQL += ComNum.VBLF + "        ACCEPTERNAME  = '" + txtInFiName.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        FICODE        = '" + cboInFiCode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        ORIGINNO      = '" + txtInOriginNo.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        ORIGINDATE    = TO_DATE('" + dtpInTrsDate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "        CardNo        = '" + txtInCardNo.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PERIOD        = '" + txtInPer.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        ENTERDATE     = SYSDATE  ";
                    SQL += ComNum.VBLF + "  WHERE ROWID         = '" + FstrRowID + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

                TEXT_CLEAR();
                txtInPtno.Focus();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void PRINT_PROCESS()
        {
            string strTitle = string.Empty;
            string strHeader = string.Empty;
            string strFooter = string.Empty;
            bool PrePrint = true;
            string strGubun = string.Empty;
            string strPartName = string.Empty;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) { return; }     //권한확인

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                if (rdoBun0.Checked == true)
                    strGubun = "카드";
                else
                    strGubun = "현금";

                if (txtPart.Text != "")
                    strPartName = CF.Read_SabunName(clsDB.DbCon, txtPart.Text);

                if (strPartName == "")
                    strTitle = strGubun + "승인 리스트";
                else
                    strTitle = strGubun + "승인 리스트 [작업조:" + strPartName + "]";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("작업일자 : " + dtpFdate.Text + " ~ " + dtpTdate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);

            }
        }

        private void READ_PROCESS(PsmhDb pDbCon)
        {
            long nTotAmt;

            nTotAmt = 0;

            if (ComQuery.IsJobAuth(this, "R", pDbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(TRANDATE,'YYYY-MM-DD') TRANDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(TRANDATE,'HH24:MI') TRANTIME, ";
            SQL += ComNum.VBLF + "        PANO, ADMIN.FC_BAS_PATIENT_SNAME(PANO) SNAME, GBIO, ";
            SQL += ComNum.VBLF + "        DEPTCODE, CARDNO, PERIOD, ";
            SQL += ComNum.VBLF + "        INSTPERIOD, ";
            SQL += ComNum.VBLF + "        DECODE(TRANHEADER,'2',TRADEAMT * -1,TRADEAMT) TRADEAMT, ";
            SQL += ComNum.VBLF + "        FICODE, ACCEPTERNAME, ORIGINNO, ";
            SQL += ComNum.VBLF + "        ROWID, ";
            SQL += ComNum.VBLF + "        TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND ACTDATE       >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND ACTDATE       <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND INPUTMETHOD   = 'T' "; //수동만
            SQL += ComNum.VBLF + "    AND PtGubun       = '3' ";

            if (rdoBun0.Checked == true)
                SQL += ComNum.VBLF + "AND GUBUN         = '1' ";
            else
                SQL += ComNum.VBLF + "AND GUBUN         = '2' ";

            if (txtPart.Text.Trim() != "")
                SQL += ComNum.VBLF + "AND PART          = '" + txtPart.Text.Trim() + "' ";

            if (txtPtno.Text.Trim() != "")
                SQL += ComNum.VBLF + "AND PANO          = '" + txtPtno.Text.Trim() + "' ";

            SQL += ComNum.VBLF + " ORDER BY TRANDATE, PANO ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count + 1;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["TRANDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["TRANTIME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["GBIO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["CARDNO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["PERIOD"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["INSTPERIOD"].ToString().Trim();
                ssList_Sheet1.Cells[i, 9].Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[i]["TRADEAMT"]));
                nTotAmt += Convert.ToInt64(Dt.Rows[i]["TRADEAMT"].ToString().Trim());
                ssList_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["FICODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["ACCEPTERNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 12].Text = Dt.Rows[i]["ORIGINNO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 13].Text = Dt.Rows[i]["ACTDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 14].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            ssList_Sheet1.Cells[ssList_Sheet1.RowCount-1, 8].Text = "합  계";
            ssList_Sheet1.Cells[ssList_Sheet1.RowCount-1, 9].Text = string.Format("{0:#,##0}", nTotAmt);

            Cursor.Current = Cursors.Default;

            FstrRowID_Del = "";
            FstrActDate_Del = "";
            btnDelete.Enabled = false;
        }

        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (Char)13) { btnSearch.Focus(); }
            if (sender == this.txtPart && e.KeyChar == (Char)13) { btnSearch.Focus(); }
            if (sender == this.dtpFdate && e.KeyChar == (Char)13) { dtpTdate.Focus(); }
            if (sender == this.dtpTdate && e.KeyChar == (Char)13) { btnSearch.Focus(); }

            if (sender == this.txtInPtno && e.KeyChar == (Char)13)
            {
                if (txtInPtno.Text != "")
                    txtInPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtInPtno.Text.Trim()));

                DataTable DtBP = CPF.Get_BasPatient(clsDB.DbCon, txtInPtno.Text);

                if (DtBP.Rows.Count == 0)
                {
                    ComFunc.MsgBox("환자정보가 없음. 등록번호 확인요망", "알림");
                    DtBP.Dispose();
                    DtBP = null;
                    return;
                }

                txtInSname.Text = DtBP.Rows[0]["SNAME"].ToString().Trim();
                txtInJumin.Text = DtBP.Rows[0]["JUMIN1"].ToString().Trim() + "*******";

                txtInCardNo.Focus();
            }
            if (sender == this.dtpInDate && e.KeyChar == (Char)13) { txtInPart.Focus(); }
            if (sender == this.txtInPart && e.KeyChar == (Char)13) { txtInCardNo.Focus(); }
            if (sender == this.txtInCardNo && e.KeyChar == (Char)13) { txtInPer.Focus(); }
            if (sender == this.txtInPer && e.KeyChar == (Char)13) { dtpInTrsDate.Focus(); }
            if (sender == this.dtpInTrsDate && e.KeyChar == (Char)13) { dtpInTrsTime.Focus(); }
            if (sender == this.dtpInTrsTime && e.KeyChar == (Char)13)
            {
                dtpInTrsTime.Text = string.Format("{0:00:00}", dtpInTrsTime.Text);
                cboInMonth.Focus();
            }
            if (sender == this.cboInMonth && e.KeyChar == (Char)13) { txtInAmt.Focus(); }
            if (sender == this.txtInAmt && e.KeyChar == (Char)13) { cboInFiCode.Focus(); }
            if (sender == this.cboInFiCode && e.KeyChar == (Char)13) { txtInOriginNo.Focus(); }
            if (sender == this.txtInOriginNo && e.KeyChar == (Char)13) { cboInIO.Focus(); }
            if (sender == this.cboInIO && e.KeyChar == (Char)13) { cboInDept.Focus(); }
            if (sender == this.cboInDept && e.KeyChar == (Char)13) { btnSave.Focus(); }
        }

        private void eCtl_Selected(object sender, EventArgs e)
        {
            if (sender == this.cboInIO)
            {
                if (cboInIO.Text.Trim() == "I")
                    txtInIO.Text = "입원";
                else
                    txtInIO.Text = "외래";
            }

            if (sender == this.cboInFiCode)
            {
                switch (cboInFiCode.Text.Trim())
                {
                    case "01": txtInFiName.Text = "비씨카드"; break;
                    case "02": txtInFiName.Text = "국민카드"; break;
                    case "03": txtInFiName.Text = "외환카드"; break;     //사용안함(2021-09-29) =>하나카드
                    case "04": txtInFiName.Text = "삼성카드"; break;
                    case "05": txtInFiName.Text = "엘지카드"; break;
                    case "08": txtInFiName.Text = "현대카드"; break;
                    case "09": txtInFiName.Text = "롯데아멕스"; break;   //사용안함(2021-09-29) => 롯데카드
                    case "10": txtInFiName.Text = "신한은행"; break;     //사용안함(2021-09-29) => 신한카드
                    case "11": txtInFiName.Text = "NH카드"; break;
                    case "12": txtInFiName.Text = "신한카드"; break;
                    case "13": txtInFiName.Text = "롯데카드"; break;
                    case "14": txtInFiName.Text = "하나카드"; break;
                    default: txtInFiName.Text = "Error"; break;
                }
            }

            if (sender == this.cboInDept)
            {
                txtInDeptName.Text = CF.READ_DEPTNAMEK(clsDB.DbCon, cboInDept.Text.Trim());
            }

        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            CPF = new clsPmpaFunc();
            CF = new ComFunc();
            CU = new clsUser();
            CS = new clsSpread();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlLeftTop);
            ComFunc.SetAllControlClear(pnlLeft);
            ComFunc.SetAllControlClear(pnlRightTop);
            ComFunc.SetAllControlClear(pnlRight);
            ComFunc.ReadSysDate(clsDB.DbCon);

            cboInMonth.Items.Clear();
            cboInMonth.Items.Add("0");
            for (int i = 2; i <= 12; i++)
                cboInMonth.Items.Add(i);
            cboInMonth.SelectedIndex = 0;

            cboInFiCode.Items.Clear();
            cboInFiCode.Items.Add("");
            cboInFiCode.Items.Add("01");
            cboInFiCode.Items.Add("02");
            //cboInFiCode.Items.Add("03");
            cboInFiCode.Items.Add("04");
            cboInFiCode.Items.Add("05");
            cboInFiCode.Items.Add("08");
            //cboInFiCode.Items.Add("09");
            //cboInFiCode.Items.Add("10");
            cboInFiCode.Items.Add("11");
            cboInFiCode.Items.Add("12");
            cboInFiCode.Items.Add("13");
            cboInFiCode.Items.Add("14");
            cboInFiCode.SelectedIndex = 0;

            cboInIO.Items.Clear();
            cboInIO.Items.Add("");
            cboInIO.Items.Add("I");
            cboInIO.Items.Add("O");
            cboInIO.SelectedIndex = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DeptCode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ClinicDept ";
            SQL += ComNum.VBLF + "  ORDER BY PrintRanking ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return;
            }

            cboInDept.Items.Clear();
            cboInDept.Items.Add("");

            for (int i = 0; i < Dt.Rows.Count; i++)
                cboInDept.Items.Add(Dt.Rows[i]["DeptCode"].ToString().Trim());

            Dt.Dispose();
            Dt = null;
            cboInDept.SelectedIndex = 0;

            TEXT_CLEAR();

            dtpInDate.Text = clsPublic.GstrSysDate;
            dtpInTrsDate.Text = clsPublic.GstrSysDate;
            txtPart.Text = clsType.User.IdNumber;
            txtInPart.Text = clsType.User.IdNumber;
            dtpFdate.Text = clsPublic.GstrSysDate;
            dtpTdate.Text = clsPublic.GstrSysDate;
            cboInIO.Text = clsPmpaPb.GstrCardIO;
            eCtl_Selected(cboInIO, null);
        }

        private void TEXT_CLEAR()
        {
            txtPtno.Text = "";
            txtInPtno.Text = "";
            txtInSname.Text = "";
            txtInJumin.Text = "";
            txtInCardNo.Text = "";
            txtInPer.Text = "";
            dtpInTrsTime.Text = "";
            cboInMonth.SelectedIndex = 0;
            txtInAmt.Text = "";
            cboInFiCode.SelectedIndex = 0;
            txtInFiName.Text = "";
            txtInOriginNo.Text = "";
            cboInIO.SelectedIndex = 0;
            txtInIO.Text = "";
            cboInDept.SelectedIndex = 0;
            txtInDeptName.Text = "";

            FstrRowID = "";
            FstrRowID_Del = "";
            FstrActDate_Del = "";
            btnDelete.Enabled = false;
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPtno = string.Empty;
            string strDate = string.Empty;

            strPtno = ssList_Sheet1.Cells[e.Row, 2].Text.Trim();
            strDate = ssList_Sheet1.Cells[e.Row, 13].Text.Trim();
            FstrActDate_Del = strDate;
            FstrRowID = ssList_Sheet1.Cells[e.Row, 14].Text.Trim();
            FstrRowID_Del = FstrRowID;

            DataTable DtBP = CPF.Get_BasPatient(clsDB.DbCon, strPtno);

            txtInPtno.Text = strPtno;
            txtInSname.Text = DtBP.Rows[0]["SNAME"].ToString().Trim();
            txtInJumin.Text = DtBP.Rows[0]["JUMIN1"].ToString().Trim() + "*******";

            DtBP.Dispose();
            DtBP = null;

            if (rdoBun0.Checked == true)
                rdoInBun0.Checked = true;
            else
                rdoInBun1.Checked = true;

            SELECT_CARD(strPtno, strDate);

            btnDelete.Enabled = true;
        }

        private void SELECT_CARD(string ArgPtno, string ArgDate)
        {

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, CardNo, PERIOD,                             --등록번호,카드번호,유효기간";
            SQL += ComNum.VBLF + "        TO_CHAR(TRANDATE,'YYYY-MM-DD') TRANDATE,          --거래일자";
            SQL += ComNum.VBLF + "        TO_CHAR(TRANDATE,'HH24:MI') TRANTIME,             --거래시간";
            SQL += ComNum.VBLF + "        INSTPERIOD, TRADEAMT, FICODE,                     --할부개월,총금액,매입처코드";
            SQL += ComNum.VBLF + "        ACCEPTERNAME, ORIGINNO, GBIO,                     --매입사명,승인번호,입원/외래";
            SQL += ComNum.VBLF + "        DEPTCODE, OgAmt, ROWID,                           --진료과목,산전지원승인금액,rowid";
            SQL += ComNum.VBLF + "        TRANHEADER                                        --거래구분(1.승인요청및응답 2.취소승인및응답)";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND PANO          = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE       = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND INPUTMETHOD   = 'T'                               --입력구분(S.Swipe, K:Key_in)";
            SQL += ComNum.VBLF + "    AND PtGubun       = '3'                               --1.코세스 2.나이스 3.다우 4.UBPay";

            if (FstrRowID != "")
                SQL += ComNum.VBLF + "   AND ROWID      = '" + FstrRowID + "' ";
            else
            {
                if (rdoBun0.Checked == true)
                    SQL += ComNum.VBLF + "  AND GUBUN   = '1'                               --카드";
                else
                    SQL += ComNum.VBLF + "  AND GUBUN   = '2'                               --현금";
            }
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                txtInCardNo.Text = Dt.Rows[0]["CardNo"].ToString().Trim();
                txtInPer.Text = Dt.Rows[0]["PERIOD"].ToString().Trim();
                dtpInTrsDate.Text = Dt.Rows[0]["TRANDATE"].ToString().Trim();
                dtpInTrsTime.Text = Dt.Rows[0]["TRANTIME"].ToString().Trim();
                cboInMonth.Text = Dt.Rows[0]["INSTPERIOD"].ToString().Trim();

                if (Dt.Rows[0]["TRANHEADER"].ToString().Trim() == "1")
                    txtInAmt.Text = string.Format("{0:#,##0}", VB.Val(Dt.Rows[0]["TRADEAMT"].ToString()));
                else
                    txtInAmt.Text = string.Format("{0:#,##0}", VB.Val(Dt.Rows[0]["TRADEAMT"].ToString()) * -1);

                cboInFiCode.Text = Dt.Rows[0]["FICODE"].ToString().Trim();
                txtInOriginNo.Text = Dt.Rows[0]["ORIGINNO"].ToString().Trim();
                cboInIO.Text = Dt.Rows[0]["GBIO"].ToString().Trim();
                cboInDept.Text = Dt.Rows[0]["DEPTCODE"].ToString().Trim();

                chkOg.Checked = false;
                if (Convert.ToInt64(VB.Val(Dt.Rows[0]["OgAmt"].ToString())) > 0 && Dt.Rows[0]["DEPTCODE"].ToString().Trim() == "OG")
                    chkOg.Checked = true;

                FstrRowID = Dt.Rows[0]["ROWID"].ToString().Trim();
                eCtl_Selected(cboInFiCode, null);
                eCtl_Selected(cboInDept, null);
                eCtl_Selected(cboInIO, null);
            }

            Dt.Dispose();
            Dt = null;

            FstrRowID = "";

            Cursor.Current = Cursors.Default;

        }
    }
}
