using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 기타환불내역 등록관리
/// Author : 박병규
/// Create Date : 2017.07.19
/// </summary>
/// <history>
/// </history>
/// <seealso cref="frm환불내역수동등록.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaMasterRefundEtc : Form
    {
        clsPmpaFunc CPF = null;
        ComFunc CF = null;
        clsUser CU = null;
        clsSpread CS = null;
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        string strRowID = string.Empty;

        public frmPmpaMasterRefundEtc()
        {
            InitializeComponent();

            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eForm_Load);

            //LostFocus 이벤트
            this.txtPtno.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtPtno2.LostFocus += new EventHandler(eControl_LostFocus);

            //KeyPress 이벤트
            this.txtPtno.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpFdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.btnSearch.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txtPtno2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.cboDept.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpBdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtRemark.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtNotRemark.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            //Changed 이벤트
            this.dtpFdate.ValueChanged += new EventHandler(eControl_Changed);
            this.dtpTdate.ValueChanged += new EventHandler(eControl_Changed);
        }

        private void eControl_Changed(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlLeft);
            ComFunc.SetAllControlClear(pnlRight);
        }

        private void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (Char)13)
            {
                Get_Patient(txtPtno.Text, this.txtPtno);
                btnSearch.Focus();
            }

            if (sender == this.dtpFdate && e.KeyChar == (Char)13) { dtpTdate.Focus(); }
            if (sender == this.dtpTdate && e.KeyChar == (Char)13) { btnSearch.Focus(); }
            if (sender == this.btnSearch && e.KeyChar == (Char)13) { btnPrint.Focus(); }

            if (sender == this.txtPtno2 && e.KeyChar == (Char)13)
            {
                Get_Patient(txtPtno2.Text, this.txtPtno2);
                cboDept.Focus();
            }
            if (sender == this.cboDept && e.KeyChar == (Char)13) { dtpBdate.Focus(); }
            if (sender == this.dtpBdate && e.KeyChar == (Char)13) { txtRemark.Focus(); }
            if (sender == this.txtRemark && e.KeyChar == (Char)13) { txtNotRemark.Focus(); }
            if (sender == this.txtNotRemark && e.KeyChar == (Char)13) { btnSave.Focus(); }
        }

        private void Get_Patient(string ArgPtno, object sender)
        {
            DataTable Dt = new DataTable();
            DataTable DtPat = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strGubun = string.Empty;

            //eForm_Clear();

            ArgPtno = string.Format("{0:D8}", Convert.ToInt32(ArgPtno));
            
            Cursor.Current = Cursors.WaitCursor;

            DtPat = CPF.Get_BasPatient(clsDB.DbCon, ArgPtno);

            if (DtPat.Rows.Count == 0)
            {
                ComFunc.MsgBox("등록번호를 다시 입력하시기 바랍니다.", "오류");
                DtPat.Dispose();
                DtPat = null;

                return;
            }
            else
            {
                if (sender == txtPtno)
                    txtSname.Text = DtPat.Rows[0]["SNAME"].ToString().Trim() + " " + DtPat.Rows[0]["SEX"].ToString().Trim() + " " + DtPat.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
                else
                    txtSname2.Text = DtPat.Rows[0]["SNAME"].ToString().Trim() + " " + DtPat.Rows[0]["SEX"].ToString().Trim() + " " + DtPat.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
            }
            DtPat.Dispose();
            DtPat = null;

            Cursor.Current = Cursors.Default;
        }

        private void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPtno)
            {
                if (txtPtno.Text.Length == 0) return;
                txtPtno.Text = VB.Val(txtPtno.Text).ToString("00000000");
            }

            if (sender == this.txtPtno2)
            {
                if (txtPtno2.Text.Length == 0) return;
                txtPtno2.Text = VB.Val(txtPtno2.Text).ToString("00000000");
            }
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            CPF = new clsPmpaFunc();
            CF = new ComFunc();
            CU = new clsUser();
            CS = new clsSpread();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaMasterRefundEtc frm = new frmPmpaMasterRefundEtc();
            ComFunc.Form_Center(frm);

            ComFunc.SetAllControlClear(pnlLeftTop);
            ComFunc.SetAllControlClear(pnlLeft);
            ComFunc.SetAllControlClear(pnlRightTop);
            ComFunc.SetAllControlClear(pnlRight);

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFdate.Text = VB.DateAdd("D", -30, clsPublic.GstrSysDate).ToString("yyyy-MM-dd");
            dtpTdate.Text = clsPublic.GstrSysDate;
            dtpBdate.Text = clsPublic.GstrSysDate;
            dtpBdate.Checked = true;

            CF.COMBO_DEPT_SET(clsDB.DbCon, cboDept, "1", "1");

            txtPtno.Focus();
        }

        void eForm_Clear()
        {
            txtPtno2.Text = "";
            txtSname2.Text = "";
            cboDept.SelectedIndex = 0;
            dtpBdate.Checked = true;
            txtAmt.Text = "";
            txtRemark.Text = "";
            txtNotRemark.Text = "";
            strRowID = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_DataLoad(clsDB.DbCon);
        }

        private void Get_DataLoad(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (ComQuery.IsJobAuth(this, "R", pDbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlLeft);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, PANO, SNAME, ";
            SQL += ComNum.VBLF + "        DEPTCODE, BI, YEAR, ";
            SQL += ComNum.VBLF + "        CREMARK, TO_CHAR(BDATE, 'YYYY-MM-DD') BDATE, CAMT, ";
            SQL += ComNum.VBLF + "        CSABUN, KOSMOS_OCS.FC_BAS_PASS_NAME(CSABUN) CNAME, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RDATE, 'YYYY-MM-DD') RDATE, RAMT, ";
            SQL += ComNum.VBLF + "        RSABUN, KOSMOS_OCS.FC_BAS_PASS_NAME(RSABUN) RNAME, ";
            SQL += ComNum.VBLF + "        RREMARK, NOTREMARK, ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_REFUND_ETC ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ACTDATE   >= TO_DATE('" + dtpFdate.Text + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND ACTDATE   <= TO_DATE('" + dtpTdate.Text + "', 'YYYY-MM-DD') ";
            if (txtPtno.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "AND PANO      = '" + txtPtno.Text.Trim() + "' ";
            }
            SQL += ComNum.VBLF + "  ORDER BY RDATE DESC, ACTDATE DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["YEAR"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["ACTDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["BDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = string.Format("{0:#,##0}", Dt.Rows[i]["CAMT"].ToString().Trim());
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["CNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["CREMARK"].ToString().Trim();
                ssList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["RDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 10].Text = string.Format("{0:#,##0}", Dt.Rows[i]["RAMT"].ToString().Trim());
                ssList_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["RNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 12].Text = Dt.Rows[i]["RREMARK"].ToString().Trim();
                ssList_Sheet1.Cells[i, 13].Text = Dt.Rows[i]["NOTREMARK"].ToString().Trim();
                ssList_Sheet1.Cells[i, 14].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void ssList_CellClick(object sender, CellClickEventArgs e)
        {
            ComFunc.SetAllControlClear(pnlRight);

            txtPtno2.Text = ssList_Sheet1.Cells[e.Row, 2].Text.Trim();
            txtSname2.Text = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();

            string strDept = ssList_Sheet1.Cells[e.Row, 4].Text.Trim();
            for (int i = 0; i < cboDept.Items.Count; i++)
            {
                cboDept.SelectedIndex = i;
                if (VB.Left(cboDept.Text.Trim(),2) == strDept)
                    break;
                else
                    cboDept.SelectedIndex = 0;
            }

            dtpBdate.Text =  ssList_Sheet1.Cells[e.Row, 5].Text.Trim();
            dtpBdate.Checked = false;
            txtAmt.Text = ssList_Sheet1.Cells[e.Row, 6].Text.Trim();
            txtRemark.Text = ssList_Sheet1.Cells[e.Row, 8].Text.Trim();
            txtNotRemark.Text = ssList_Sheet1.Cells[e.Row, 13].Text.Trim();

            strRowID = ssList_Sheet1.Cells[e.Row, 14].Text.Trim();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            eForm_Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save_Process(clsDB.DbCon);
        }

        private void Save_Process(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            DataTable DtPat = new DataTable();

            string strDept = string.Empty;
            string strSname = string.Empty;
            string strBi = string.Empty;
            long nAmt = 0;

            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            if (txtPtno2.Text == "") { ComFunc.MsgBox("등록번호를 입력 바랍니다."); txtPtno2.Focus(); return; }

            DtPat = CPF.Get_BasPatient(pDbCon, txtPtno2.Text.Trim());

            if (DtPat.Rows.Count == 0)
            {
                ComFunc.MsgBox("등록번호를 다시 입력하시기 바랍니다.", "오류");
                DtPat.Dispose();
                DtPat = null;

                return;
            }
            else
            {
                strSname = DtPat.Rows[0]["SNAME"].ToString().Trim();
                strBi = DtPat.Rows[0]["BI"].ToString().Trim();
            }

            DtPat.Dispose();
            DtPat = null;

            strDept = VB.Left(cboDept.Text, 2);
            nAmt = Convert.ToInt64(VB.Replace(txtAmt.Text, ",", ""));

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                if (strRowID == "")
                {
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_REFUND_ETC ";
                    SQL += ComNum.VBLF + "        (ACTDATE, BDATE, PANO, ";
                    SQL += ComNum.VBLF + "         DEPTCODE, DRCODE, BI, ";
                    SQL += ComNum.VBLF + "         SNAME, PART, SABUN, ";
                    SQL += ComNum.VBLF + "         CAMT, CSABUN, CPART,";
                    SQL += ComNum.VBLF + "         CREMARK, ENTDATE, YEAR, ";
                    SQL += ComNum.VBLF + "         NOTREMARK) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'),   --예약부도정리일자";
                    SQL += ComNum.VBLF + "         TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD'),          --처방일자";
                    SQL += ComNum.VBLF + "         '" + txtPtno2.Text + "',                                 --등록번호";
                    SQL += ComNum.VBLF + "         '" + strDept + "',                                       --진료과";
                    SQL += ComNum.VBLF + "         '',                                                      --의사코드";
                    SQL += ComNum.VBLF + "         '" + strBi + "',                                         --보험유형";
                    SQL += ComNum.VBLF + "         '" + strSname + "',                                      --환자성명";
                    SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "',                          --";
                    SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + ",                            --";
                    SQL += ComNum.VBLF + "         " + nAmt + ",                                            --보관금액";
                    SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + ",                            --보관 작업자사번";
                    SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "',                          --보관 처리자 작업조";
                    SQL += ComNum.VBLF + "         '" + txtRemark.Text + "',                                --보관사유";
                    SQL += ComNum.VBLF + "         SYSDATE,                                                 --작업시간";
                    SQL += ComNum.VBLF + "         '" + VB.Left(clsPublic.GstrSysDate, 4) + "',             --";
                    SQL += ComNum.VBLF + "         '" + txtNotRemark.Text.Trim() + "')                       --미환불사유";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_REFUND_ETC ";
                    SQL += ComNum.VBLF + "    SET BDate     = TO_DATE('" + dtpBdate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "        DEPTCODE  = '" + strDept + "', ";
                    SQL += ComNum.VBLF + "        CREMARK   = '" + txtRemark.Text + "', ";
                    SQL += ComNum.VBLF + "        NotRemark = '" + txtNotRemark.Text + "' ";
                    SQL += ComNum.VBLF + "  WHERE ROWID     = '" + strRowID + "' ";
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

                eForm_Clear();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Delete_Process(clsDB.DbCon);
        }

        private void Delete_Process(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            if (txtPtno2.Text == "") { ComFunc.MsgBox("등록번호를 입력 바랍니다."); return; }
            if (strRowID == "") { ComFunc.MsgBox("삭제할 환자를 선택바랍니다."); return; }

            if (ComFunc.MsgBoxQ(txtSname.Text + " 님의 삭제를 진행하시겠습니까?",
            "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO BAS_GAMFSINGA_HIS ";
                SQL += ComNum.VBLF + "        (DELDATE, DELSABUN, GAMDATE, GAMPANO, ";
                SQL += ComNum.VBLF + "         GAMJUMIN, GAMJUMIN_NEW, GAMSNAME, GAMROMAN, ";
                SQL += ComNum.VBLF + "         GAMGUBUN, GAMSABUN, ENTDATE) ";
                SQL += ComNum.VBLF + " SELECT TO_DATE('" + clsPublic.GstrSysDate + clsPublic.GstrSysTime + "','YYYY-MM-DDHH24:MI'), ";
                SQL += ComNum.VBLF + "        '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "        GAMDATE, GAMPANO, GAMJUMIN, GAMJUMIN_new, GAMSNAME, ";
                SQL += ComNum.VBLF + "        GAMROMAN, GAMGUBUN, GAMSABUN, ENTDATE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMFSINGA ";
                SQL += ComNum.VBLF + "  WHERE ROWID  = '" + strRowID + "' ";
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
                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "BAS_GAMFSINGA ";
                SQL += ComNum.VBLF + "  WHERE ROWID = '" + strRowID + "' ";
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

                eForm_Clear();
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) { return; }     //권한확인

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strTitle = "환불대상자 명단";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("보관일자 : " + dtpFdate.Text + " ~ " + dtpTdate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);

            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(strRowID.Trim()))
            {
            }
            else
            {
                DataBackup(clsDB.DbCon);
            }
        }


        private void  DataBackup(PsmhDb pDbCon) 
        {
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            if (strRowID == "") { ComFunc.MsgBox("삭제할 환자를 선택바랍니다."); return; }


            if (ComFunc.MsgBoxQ(txtSname.Text + "해당 기타환불내역을 삭제하시겠습니까?",
                "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }



            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = " INSERT INTO KOSMOS_PMPA.OPD_REFUND_ETC_DEL ( ";
                SQL += ComNum.VBLF + "        ACTDATE, BDATE, PANO, DEPTCODE, ";
                SQL += ComNum.VBLF + "        DRCODE, BI, SNAME, PART, ";
                SQL += ComNum.VBLF + "        SABUN, CAMT, CSABUN, CPART, ";
                SQL += ComNum.VBLF + "        RDATE, RAMT, RSABUN, RPART, ";
                SQL += ComNum.VBLF + "        RREMARK, CARDSEQNO, RETCARDSEQNO, ENTDATE, ";
                SQL += ComNum.VBLF + "        YEAR, CREMARK, NOTREMARK, DELDATE, DELSABUN ) ";
                SQL += ComNum.VBLF + "        SELECT ";
                SQL += ComNum.VBLF + "        ACTDATE, BDATE, PANO, DEPTCODE, ";
                SQL += ComNum.VBLF + "        DRCODE, BI, SNAME, PART, ";
                SQL += ComNum.VBLF + "        SABUN, CAMT, CSABUN, CPART, ";
                SQL += ComNum.VBLF + "        RDATE, RAMT, RSABUN, RPART, ";
                SQL += ComNum.VBLF + "        RREMARK, CARDSEQNO, RETCARDSEQNO, ENTDATE, ";
                SQL += ComNum.VBLF + "        YEAR, CREMARK, NOTREMARK, SYSDATE,'" + clsType.User.Sabun + "'  ";
                SQL += ComNum.VBLF + "        FROM KOSMOS_PMPA.OPD_REFUND_ETC ";
                SQL += ComNum.VBLF + "        WHERE ROWID = '" + strRowID + "' ";
                SQL += ComNum.VBLF + "          AND ACTDATE = TRUNC(SYSDATE) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("기타환불 등록 내역 삭제 중 에러가 발생하였습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = " DELETE KOSMOS_PMPA.OPD_REFUND_ETC ";
                SQL += ComNum.VBLF + "        WHERE ROWID = '" + strRowID + "' ";
                SQL += ComNum.VBLF + "          AND ACTDATE = TRUNC(SYSDATE)";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("기타환불 등록 내역 삭제 중 에러가 발생하였습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                if (intRowCnt > 0)
                {
                    ComFunc.MsgBox("기타환불 등록 내역을 삭제하였습니다.");
                }
                else
                {
                    ComFunc.MsgBox("삭제조건을 확인하시기 바랍니다.(ex. 보관일자)");
                }
                eForm_Clear();

                Get_DataLoad(clsDB.DbCon);
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
    }
}
