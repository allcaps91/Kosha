using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{
    public partial class frmPmpaMstExamRes : Form
    {
        ComFunc CF = null;
        clsSpread CS = null;
        clsOumsad CPO = null;
        clsPmpaPrint CPP = null;

        DataTable Dt = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        string FstrRowID = string.Empty;
        long FnAmt6 = 0;
        long FnWrtno = 0;


        public frmPmpaMstExamRes()
        {
            InitializeComponent();

            setParam();
        }


        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

            this.txtPtno.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            this.chkEnd.Click += new EventHandler(eCtl_Click);

            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnPrint.Click += new EventHandler(eCtl_Click);
            this.btnSearchAdFee.Click += new EventHandler(eCtl_Click);
            this.btnSave.Click += new EventHandler(eCtl_Click);//예약검사비 대체
            this.btnRet.Click += new EventHandler(eCtl_Click);//예약검사비 수동취소
        }


        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
                GET_DATA(clsDB.DbCon);
            else if (sender == this.btnPrint)
                Print_Process(clsDB.DbCon);
            else if (sender == this.btnSearchAdFee)
            {
                frmPmpaViewAdvanceFee F = new ComPmpaLibB.frmPmpaViewAdvanceFee();
                F.Show();
                F.CHOICE_Event += new ComPmpaLibB.frmPmpaViewAdvanceFee.CHOICE_PTNO(CHOICE_Value);
            }
            else if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnRet)
                Ret_Process(clsDB.DbCon);
            else if (sender == this.chkEnd)
            {
                RESV_EXAM_VIEW(clsDB.DbCon, txtPtno.Text);
                CS.Spread_Clear(ssList2, ssList2_Sheet1.RowCount, ssList2_Sheet1.ColumnCount);
                RESV_ENDO_EXAM_VIEW(clsDB.DbCon, txtPtno.Text);
            }
        }

        private void Ret_Process(PsmhDb pDbCon)
        {
            string strPtno = "";
            string strSname = "";
            string strBi = "";
            string strBdate = "";
            string strDeptCode = "";
            string strDeptNamek = "";
            string strDrCode = "";
            string strGbSpc = "";
            long nWrtNo = 0;
            int nSeqNo = 0;

            if (ComQuery.IsJobAuth(this, "D", pDbCon) == false) { return; }     //권한확인

            if (FstrRowID == "" || FnWrtno == 0)
            {
                ComFunc.MsgBox("예약검사비 환불처리 항목을 먼저 선택후 작업하시기 바랍니다.", "확인");
                return;
            }

            clsPublic.GstrMsgTitle = "확인";
            clsPublic.GstrMsgList = "선택한 예약검사비용 [ " + FnAmt6 + " ] 을 환불 하시겠습니까?" + '\r' + '\r';
            clsPublic.GstrMsgList += "수동 환불처리시 내시경실과 통화 후 확인하시기 바랍니다." + '\r';

            DialogResult result = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
            if (result == DialogResult.No) { return; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, DEPTCODE, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(DEPTCODE) DEPTNAMEK, ";
            SQL += ComNum.VBLF + "        BI, DRCODE, GBSPC, ";
            SQL += ComNum.VBLF + "        TO_CHAR(BDate,'YYYY-MM-DD') BDate, WRTNO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND ROWID = '" + FstrRowID + "' ";
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
                ComFunc.MsgBox("예약검사 환불처리할 항목을 선택후 작업하시기 바랍니다.", "확인");
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }
            else
            {
                strPtno = Dt.Rows[0]["PANO"].ToString().Trim();
                strBi = Dt.Rows[0]["Bi"].ToString().Trim();
                strBdate = Dt.Rows[0]["BDate"].ToString().Trim();
                strDeptCode = Dt.Rows[0]["DeptCode"].ToString().Trim();
                strDeptNamek = Dt.Rows[0]["DEPTNAMEK"].ToString().Trim();
                strDrCode = Dt.Rows[0]["DrCode"].ToString().Trim();
                strGbSpc = Dt.Rows[0]["GbSpc"].ToString().Trim();
                nWrtNo = Convert.ToInt64(Dt.Rows[0]["WRTNO"].ToString().Trim());

                strSname = ssMst_Sheet1.Cells[0, 0].Text.Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                //BACKUP
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM_BACKUP ";
                SQL += ComNum.VBLF + "        (PANO, DEPTCODE, BI, ";
                SQL += ComNum.VBLF + "         SNAME, DRCODE, ACTDATE, ";
                SQL += ComNum.VBLF + "         BDATE, GBGAMEK, GBSPC, ";
                SQL += ComNum.VBLF + "         JIN, AMT1, AMT2, ";
                SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                SQL += ComNum.VBLF + "         AMT6, PART, BOHUN, ";
                SQL += ComNum.VBLF + "         GELCODE, TRANSDATE, TRANSAMT, ";
                SQL += ComNum.VBLF + "         TRANSPART, RETDATE, RETAMT, ";
                SQL += ComNum.VBLF + "         RETPART, MCODE, VCODE, ";
                SQL += ComNum.VBLF + "         Remark, SMSBUILD, WRTNO, ";
                SQL += ComNum.VBLF + "         GBEND, MANUAL, Gubun, ";
                SQL += ComNum.VBLF + "         ENTDATE, DATE1, DATE2, ";
                SQL += ComNum.VBLF + "         DATE3 ) ";
                SQL += ComNum.VBLF + " (SELECT PANO, DEPTCODE, BI, ";
                SQL += ComNum.VBLF + "         SNAME, DRCODE, ACTDATE, ";
                SQL += ComNum.VBLF + "         BDATE, GBGAMEK, GBSPC, ";
                SQL += ComNum.VBLF + "         JIN, AMT1, AMT2, ";
                SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                SQL += ComNum.VBLF + "         AMT6, PART, BOHUN, ";
                SQL += ComNum.VBLF + "         GELCODE, TRANSDATE, TRANSAMT, ";
                SQL += ComNum.VBLF + "         TRANSPART, RETDATE, RETAMT, ";
                SQL += ComNum.VBLF + "         RETPART, MCode, VCode, ";
                SQL += ComNum.VBLF + "         Remark, SMSBUILD, WRTNO, ";
                SQL += ComNum.VBLF + "         GBEND, MANUAL, Gubun, ";
                SQL += ComNum.VBLF + "         ENTDATE, DATE1, DATE2, ";
                SQL += ComNum.VBLF + "         DATE3 ";
                SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL += ComNum.VBLF + "   WHERE 1        = 1 ";
                SQL += ComNum.VBLF + "     AND ROWID    = '" + FstrRowID + "' ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //- 금액 입력
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL += ComNum.VBLF + "        (PANO, DEPTCODE, BI, ";
                SQL += ComNum.VBLF + "         SNAME, DRCODE, ACTDATE, ";
                SQL += ComNum.VBLF + "         BDATE, GBGAMEK, GBSPC, ";
                SQL += ComNum.VBLF + "         JIN, AMT1, AMT2, ";
                SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                SQL += ComNum.VBLF + "         AMT6, PART, BOHUN, ";
                SQL += ComNum.VBLF + "         GELCODE, TRANSDATE, TRANSAMT, ";
                SQL += ComNum.VBLF + "         TRANSPART, RETDATE, RETAMT, ";
                SQL += ComNum.VBLF + "         RETPART, MCODE, VCODE, ";
                SQL += ComNum.VBLF + "         Remark, SMSBUILD, WRTNO, ";
                SQL += ComNum.VBLF + "         GBEND, MANUAL, Gubun, ";
                SQL += ComNum.VBLF + "         ENTDATE, DATE1, DATE2, ";
                SQL += ComNum.VBLF + "         DATE3 ) ";
                SQL += ComNum.VBLF + " (SELECT PANO, DEPTCODE, BI, ";
                SQL += ComNum.VBLF + "         SNAME, DRCODE, TRUNC(SYSDATE), ";
                SQL += ComNum.VBLF + "         BDATE, GBGAMEK, GBSPC, ";
                SQL += ComNum.VBLF + "         JIN, AMT1 * -1, AMT2 * -1, ";
                SQL += ComNum.VBLF + "         AMT3 * -1, AMT4 * -1, AMT5 * -1, ";
                SQL += ComNum.VBLF + "         AMT6 * -1, " + clsType.User.IdNumber + ", BOHUN, ";
                SQL += ComNum.VBLF + "         GELCODE, TRANSDATE, TRANSAMT * -1, ";
                SQL += ComNum.VBLF + "         TRANSPART, RETDATE, RETAMT * -1, ";
                SQL += ComNum.VBLF + "         RETPART, MCode, VCode, ";
                SQL += ComNum.VBLF + "         '" + Convert.ToString(FnWrtno) + "번 예약 수동취소" + "' , SMSBUILD, WRTNO, ";
                SQL += ComNum.VBLF + "         GBEND, 'Y', Gubun, ";
                SQL += ComNum.VBLF + "         SYSDATE, SYSDATE, DATE2, ";
                SQL += ComNum.VBLF + "         DATE3 ";
                SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL += ComNum.VBLF + "   WHERE 1        = 1 ";
                SQL += ComNum.VBLF + "     AND ROWID    = '" + FstrRowID + "' ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //UPDATE(완료셋팅)
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL += ComNum.VBLF + "    SET Date1 = SYSDATE, ";
                SQL += ComNum.VBLF + "        GbEnd = 'Y' ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND WRTNO = " + FnWrtno + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //수납작업
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                SQL += ComNum.VBLF + "        (ACTDATE, PANO, AMT, ";
                SQL += ComNum.VBLF + "         PART, SEQNO, STIME, ";
                SQL += ComNum.VBLF + "         BIGO, REMARK, SEQNO2, ";
                SQL += ComNum.VBLF + "         DEPTCODE,BI,GBSPC) ";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '" + strPtno + "', ";
                SQL += ComNum.VBLF + "         " + (FnAmt6 * -1) + ", ";
                SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "         " + nSeqNo + ", ";
                SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                SQL += ComNum.VBLF + "         '번호:" + FnWrtno + "', ";
                SQL += ComNum.VBLF + "         '예약검사(취소)' , ";
                SQL += ComNum.VBLF + "         " + nSeqNo + ", ";
                SQL += ComNum.VBLF + "         '" + strDeptCode + "', ";
                SQL += ComNum.VBLF + "         '" + strBi + "', ";
                SQL += ComNum.VBLF + "         '" + strGbSpc + "' ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //UPDATE
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "OCS_OORDER ";
                SQL += ComNum.VBLF + "    SET GbSunap   = '0' ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + strBdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND Ptno      = '" + strPtno + "' ";
                SQL += ComNum.VBLF + "    AND DeptCode  = '" + strDeptCode + "' ";
                SQL += ComNum.VBLF + "    AND Res       = '1' ";
                SQL += ComNum.VBLF + "    AND GbSunap   = '1' ";
                SQL += ComNum.VBLF + "    AND WRTNO     = " + FnWrtno + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //API INSERT
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_MED + "OCS_API ";
                SQL += ComNum.VBLF + "        (ActDate, Pano, Part, ";
                SQL += ComNum.VBLF + "         SeqNo, Gubun, GbSend, ";
                SQL += ComNum.VBLF + "         EntDate, WRTNO, RES ) ";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrActDate + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '" + txtPtno.Text.Trim() + "', ";
                SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "         0, ";
                SQL += ComNum.VBLF + "         '-', ";
                SQL += ComNum.VBLF + "         ' ', ";
                SQL += ComNum.VBLF + "         SysDate, ";
                SQL += ComNum.VBLF + "         " + FnWrtno + ", ";
                SQL += ComNum.VBLF + "         '1' )  ";
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

            CPP.Report_Print_ResvReceipt_New(pDbCon, strPtno, strDeptNamek, strSname, strDrCode, strBi, strBdate, strDeptCode, pic_Sign, strGbSpc, "3", FnAmt6 * -1, nWrtNo);

            GET_DATA(pDbCon);
        }

        private void Save_Process(PsmhDb pDbCon)
        {
            string strPtno = "";
            string strSname = "";
            string strBi = "";
            string strBdate = "";
            string strDeptCode = "";
            string strDeptNamek = "";
            string strDrCode = "";
            string strGbSpc = "";
            long nWrtNo = 0;
            int nSeqNo = 0;

            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            if (FstrRowID == "" || FnAmt6 < 0)
            {
                ComFunc.MsgBox("예약검사비 대체할 항목을 먼저 선택후 작업하시기 바랍니다.", "확인");
                return;
            }

            clsPublic.GstrMsgTitle = "확인";
            clsPublic.GstrMsgList = "선택한 예약검사비용 [ " + string.Format("{0:#,##0}", FnAmt6) + " ] 을 대체하시겠습니까?" + '\r' + '\r';
            clsPublic.GstrMsgList += "대체작업후 검사를 반드시 수납처리하시기 바랍니다." + '\r';

            DialogResult result = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No) { return; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, DEPTCODE, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(DEPTCODE) DEPTNAMEK, ";
            SQL += ComNum.VBLF + "        BI, DRCODE, GBSPC, ";
            SQL += ComNum.VBLF + "        TO_CHAR(BDate,'YYYY-MM-DD') BDate, WRTNO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND ROWID = '" + FstrRowID + "' ";
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
                ComFunc.MsgBox("예약검사 대체처리할 항목을 선택후 작업하시기 바랍니다.", "확인");
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }
            else
            {
                strPtno = Dt.Rows[0]["PANO"].ToString().Trim();
                strBi = Dt.Rows[0]["Bi"].ToString().Trim();
                strBdate = Dt.Rows[0]["BDate"].ToString().Trim();
                strDeptCode = Dt.Rows[0]["DeptCode"].ToString().Trim();
                strDeptNamek = Dt.Rows[0]["DEPTNAMEK"].ToString().Trim();
                strDrCode = Dt.Rows[0]["DrCode"].ToString().Trim();
                strGbSpc = Dt.Rows[0]["GbSpc"].ToString().Trim();
                nWrtNo = Convert.ToInt64(VB.Val(Dt.Rows[0]["WRTNO"].ToString().Trim()));

                strSname = ssMst_Sheet1.Cells[0, 0].Text.Trim();
            }

            Dt.Dispose();
            Dt = null;

            if (strBdate == clsPublic.GstrSysDate)
            {
                clsPublic.GstrMsgTitle = "대체불가 확인";
                clsPublic.GstrMsgList = "예약검사 대체처리는 당일 예약검사비 발생건은 할 수 없습니다." + '\r' + '\r';
                clsPublic.GstrMsgList += "환불 후 정상수납 처리하시기 바랍니다." + '\r';
                ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                //BACKUP
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM_BACKUP ";
                SQL += ComNum.VBLF + "        (PANO, DEPTCODE, BI, ";
                SQL += ComNum.VBLF + "         SNAME, DRCODE, ACTDATE, ";
                SQL += ComNum.VBLF + "         BDATE, GBGAMEK, GBSPC, ";
                SQL += ComNum.VBLF + "         JIN, AMT1, AMT2, ";
                SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                SQL += ComNum.VBLF + "         AMT6, PART, BOHUN, ";
                SQL += ComNum.VBLF + "         GELCODE, TRANSDATE, TRANSAMT, ";
                SQL += ComNum.VBLF + "         TRANSPART, RETDATE, RETAMT, ";
                SQL += ComNum.VBLF + "         RETPART, MCODE, VCODE, ";
                SQL += ComNum.VBLF + "         Remark, SMSBUILD, WRTNO, ";
                SQL += ComNum.VBLF + "         GBEND, MANUAL, Gubun, ";
                SQL += ComNum.VBLF + "         ENTDATE, DATE1, DATE2, ";
                SQL += ComNum.VBLF + "         DATE3 ) ";
                SQL += ComNum.VBLF + " (SELECT PANO, DEPTCODE, BI, ";
                SQL += ComNum.VBLF + "         SNAME, DRCODE, ACTDATE, ";
                SQL += ComNum.VBLF + "         BDATE, GBGAMEK, GBSPC, ";
                SQL += ComNum.VBLF + "         JIN, AMT1, AMT2, ";
                SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                SQL += ComNum.VBLF + "         AMT6, PART, BOHUN, ";
                SQL += ComNum.VBLF + "         GELCODE, TRANSDATE, TRANSAMT, ";
                SQL += ComNum.VBLF + "         TRANSPART, RETDATE, RETAMT, ";
                SQL += ComNum.VBLF + "         RETPART, MCode, VCode, ";
                SQL += ComNum.VBLF + "         Remark, SMSBUILD, WRTNO, ";
                SQL += ComNum.VBLF + "         GBEND, MANUAL, Gubun, ";
                SQL += ComNum.VBLF + "         ENTDATE, DATE1, DATE2, ";
                SQL += ComNum.VBLF + "         DATE3 ";
                SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL += ComNum.VBLF + "   WHERE 1        = 1 ";
                SQL += ComNum.VBLF + "     AND ROWID    = '" + FstrRowID + "' ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //UPDATE
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                SQL += ComNum.VBLF + "    SET TRANSDATE = TRUNC(SYSDATE), ";
                SQL += ComNum.VBLF + "        TRANSAMT  = " + FnAmt6 + ",  ";
                SQL += ComNum.VBLF + "        TRANSPART = '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "        GbEnd     = 'Y', ";
                SQL += ComNum.VBLF + "        ENTDATE   = SYSDATE ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND ROWID     = '" + FstrRowID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //수납작업
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                SQL += ComNum.VBLF + "        (ACTDATE, PANO, AMT, ";
                SQL += ComNum.VBLF + "         PART, SEQNO, STIME, ";
                SQL += ComNum.VBLF + "         BIGO, REMARK, SEQNO2, ";
                SQL += ComNum.VBLF + "         DEPTCODE, BI, GBSPC) ";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '" + strPtno + "', ";
                SQL += ComNum.VBLF + "         " + (FnAmt6 * -1) + ", ";
                SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "         " + nSeqNo + ", ";
                SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                SQL += ComNum.VBLF + "         '번호:" + FnWrtno + "', ";
                SQL += ComNum.VBLF + "         '예약검사대체', ";
                SQL += ComNum.VBLF + "         " + nSeqNo + ", ";
                SQL += ComNum.VBLF + "         '" + strDeptCode + "', ";
                SQL += ComNum.VBLF + "         '" + strBi + "', ";
                SQL += ComNum.VBLF + "         '" + strGbSpc + "' ) ";
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

            //예약선수금 영수증 출력
            clsPublic.GstrMsgTitle = "확인";
            clsPublic.GstrMsgList = "예약 선수금 대체 영수증을 출력하시겠습니까?" + '\r' + '\r';
            clsPublic.GstrMsgList += "이전 선수금 영수증을 회수하여 주시기 바랍니다." + '\r';

            if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                CPP.Report_Print_ResvReceipt_New(pDbCon, strPtno, strDeptNamek, strSname, strDrCode, strBi, strBdate, strDeptCode, pic_Sign, strGbSpc, "2", FnAmt6, nWrtNo);

            GET_DATA(pDbCon);
        }

        private void Print_Process(PsmhDb pDbCon)
        {
            string strPtno = "";
            string strSname = "";
            string strDeptCode = "";
            string strDeptNamek = "";
            string strBi = "";
            string strDrCode = "";
            string strDrName = "";
            string strGbSpc = "";
            string strBdate = "";
            string strChk = "";
            long nAmt6 = 0;
            long nWrtNo = 0;


            clsPublic.GstrMsgTitle = "재출력";
            clsPublic.GstrMsgList = "선택한 예약검사 영수증을 재출력 하시겠습니까?" + '\r';

            if (DialogResult.No == ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button2))
                return; 

            strSname = ssMst_Sheet1.Cells[0, 0].Text.Trim();

            for (int i = 0; i < ssList1_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssList1_Sheet1.Cells[i, 0].Value) == true)
                {
                    strPtno = ssList1_Sheet1.Cells[i, 1].Text.Trim();
                    strDeptCode = ssList1_Sheet1.Cells[i, 2].Text.Trim();
                    strBi = ssList1_Sheet1.Cells[i, 3].Text.Trim();
                    strDrCode = ssList1_Sheet1.Cells[i, 4].Text.Trim();
                    strGbSpc = ssList1_Sheet1.Cells[i, 6].Text.Trim();
                    strBdate = ssList1_Sheet1.Cells[i, 7].Text.Trim();
                    nAmt6 = Convert.ToInt64(VB.Val(VB.Replace(ssList1_Sheet1.Cells[i, 9].Text.Trim(), ",", "")));
                    nWrtNo = Convert.ToInt64(ssList1_Sheet1.Cells[i, 20].Text.Trim());

                    strDrName = CF.READ_DrName(pDbCon, strDrCode);
                    strDeptNamek = CF.READ_DEPTNAMEK(pDbCon, strDeptCode);

                    //예약선수금 영수증 출력
                    if (nAmt6 != 0)
                    {
                        strChk = "0";

                        if (ComFunc.MsgBoxQ("사본으로 출력하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            strChk = "1";

                        CPP.Report_Print_ResvReceipt_New(pDbCon, strPtno, strDeptNamek, strSname, strDrCode, strBi, strBdate, strDeptCode, pic_Sign, strGbSpc, strChk, nAmt6, nWrtNo);

                        //예약검사 접수증
                        CPP.Report_Print_ResvExam(strPtno, strDeptNamek, strSname, strDrCode, strDrName, strBi, strBdate, strDeptCode, pic_Sign, strGbSpc, strChk, nAmt6, nWrtNo, ssResExamPrint);
                    }
                }
            }

        }


        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (Char)13)
            {
                if (CF.READ_BARCODE(txtPtno.Text.Trim()) == true)
                    txtPtno.Text = clsPublic.GstrBarPano;
                else
                {
                    if (txtPtno.Text == "") { return; }
                    txtPtno.Text = String.Format("{0:D8}", Convert.ToInt32(VB.Val(txtPtno.Text)));
                }

                eFrm_Clear();

                RESV_EXAM_MST(clsDB.DbCon, txtPtno.Text);    //환자에 대한 총예약금액, 대체금, 잔액 확인
                RESV_EXAM_VIEW(clsDB.DbCon, txtPtno.Text);


                btnSearch.Select();
            }
        }



        //환자에 대한 총예약금액, 대체금, 잔액 확인
        private void RESV_EXAM_MST(PsmhDb pDbCon, string ArgPtno)
        {
            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_Clear(ssMst, ssMst_Sheet1.RowCount, ssMst_Sheet1.ColumnCount);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, SNAME, ";
            SQL += ComNum.VBLF + "        SUM(NVL(AMT6,0) + NVL(RETAMT,0)) TAMT,                        --총금액:영수금액+환불금액";
            SQL += ComNum.VBLF + "        SUM(NVL(TRANSAMT,0)) TRANSAMT,                                --대체금액";
            SQL += ComNum.VBLF + "        SUM(NVL(AMT6,0) - NVL(TRANSAMT,0) + NVL(RETAMT,0)) JANAMT     --잔액:영수금액-대체금액+환불금액";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "  GROUP BY PANO, SNAME ";
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

            ssMst_Sheet1.RowCount = Dt.Rows.Count;
            ssList2_Sheet1.SetRowHeight(-1, 24);

            ssMst_Sheet1.Cells[0, 0].Text = Dt.Rows[0]["SNAME"].ToString().Trim();
            ssMst_Sheet1.Cells[0, 1].Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[0]["TAMT"]));
            ssMst_Sheet1.Cells[0, 2].Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[0]["TRANSAMT"])); 
            ssMst_Sheet1.Cells[0, 3].Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[0]["JANAMT"]));
            

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }


        private void RESV_EXAM_VIEW(PsmhDb pDbCon, string ArgPtno)
        {
            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_Clear(ssList1, ssList1_Sheet1.RowCount, ssList1_Sheet1.ColumnCount);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, DEPTCODE, BI,                           --";
            SQL += ComNum.VBLF + "        SNAME, DRCODE,                                --";
            SQL += ComNum.VBLF + "        TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,        --회계일자";
            SQL += ComNum.VBLF + "        TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,            --처방일자";
            SQL += ComNum.VBLF + "        TO_CHAR(TRANSDATE,'YYYY-MM-DD') TRANSDATE,    --대체일자";
            SQL += ComNum.VBLF + "        TRANSAMT, TRANSPART,                          --대체금액,대체조";
            SQL += ComNum.VBLF + "        TO_CHAR(RETDATE,'YYYY-MM-DD') RETDATE,        --환불일자";
            SQL += ComNum.VBLF + "        RETAMT, RETPART ,                             --환불금액,환불조";
            SQL += ComNum.VBLF + "        TO_CHAR(EntDate,'YY/MM/DD HH24:MI') EntDate,  --최종작업시간";
            SQL += ComNum.VBLF + "        GBGAMEK, GBSPC, JIN,                          --감액코드,특진여부,접수구분";
            SQL += ComNum.VBLF + "        AMT1, AMT2, AMT3,                             --진료비총액,선택진료비,급여총액";
            SQL += ComNum.VBLF + "        AMT4, AMT5, AMT6,                             --비급여총액,감액,영수금액";
            SQL += ComNum.VBLF + "        PART, BOHUN, GELCODE,                         --작업조,보훈,계약처코드";
            SQL += ComNum.VBLF + "        MCode, VCode, Remark,                         --특정코드,중증코드,참고사항";
            SQL += ComNum.VBLF + "        SMSBUILD, WRTNO, GbEnd,                       --문자구분,고유번호,종료여부";
            SQL += ComNum.VBLF + "        Manual, ROWID                                 --수동처리,ROWID";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";

            if (chkEnd.Checked == false)
                SQL += ComNum.VBLF + "AND GbEnd = 'N' ";

            SQL += ComNum.VBLF + "  ORDER BY EntDate, WRTNO, BDATE DESC ";
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
                ComFunc.MsgBox("예약금 상세내역 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList1_Sheet1.RowCount = Dt.Rows.Count;
            ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                if (Dt.Rows[i]["GbEnd"].ToString().Trim() == "Y")
                    CS.setColStyle(ssList1, i, 0, clsSpread.enmSpdType.Label, null, null, null, null, false);
                else
                    CS.setColStyle(ssList1, i, 0, clsSpread.enmSpdType.CheckBox, null, null, null, null, false);



                ssList1_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["Bi"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DrCode"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["GbGamek"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["GbSPC"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["BDate"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["ActDate"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 9].Text = String.Format("{0:#,##0}", Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt6"].ToString())));
                ssList1_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["Part"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["TransDate"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 12].Text = String.Format("{0:#,##0}", Convert.ToInt64(VB.Val(Dt.Rows[i]["TransAmt"].ToString())));
                ssList1_Sheet1.Cells[i, 13].Text = Dt.Rows[i]["TransPart"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 14].Text = Dt.Rows[i]["RetDate"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 15].Text = String.Format("{0:#,##0}", Convert.ToInt64(VB.Val(Dt.Rows[i]["RetAmt"].ToString())));
                ssList1_Sheet1.Cells[i, 16].Text = Dt.Rows[i]["RetPart"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 17].Text = Dt.Rows[i]["REMARK"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 18].Text = Dt.Rows[i]["MCode"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 19].Text = Dt.Rows[i]["VCode"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 20].Text = Dt.Rows[i]["WRTNO"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 21].Text = Dt.Rows[i]["GbEnd"].ToString().Trim();

                if (Dt.Rows[i]["GbEnd"].ToString().Trim() == "Y")
                {
                    CS.setColStyle(ssList1, i, 0, clsSpread.enmSpdType.Text, null, null, null, null, false);
                    ssList1.ActiveSheet.Cells[i, 21].BackColor = Color.FromArgb(255, 255, 128);
                }
                else
                    ssList1.ActiveSheet.Cells[i, 21].BackColor = Color.FromArgb(255, 255, 255);

                ssList1_Sheet1.Cells[i, 22].Text = Dt.Rows[i]["Manual"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 23].Text = Dt.Rows[i]["EntDate"].ToString().Trim();
                ssList1_Sheet1.Cells[i, 24].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }


        //내시경관리화면
        private void RESV_ENDO_EXAM_VIEW(PsmhDb pDbCon, string ArgPtno)
        {
            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_Clear(ssEndo, ssEndo_Sheet1.RowCount, ssEndo_Sheet1.ColumnCount);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PtNO Pano, DEPTCODE, SNAME,                                       --";
            SQL += ComNum.VBLF + "        DRCODE,                                                           --";
            SQL += ComNum.VBLF + "        TO_CHAR(JDATE,'YYYY-MM-DD') ACTDATE,                              --접수일자";
            SQL += ComNum.VBLF + "        TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,                                --처방일자";
            SQL += ComNum.VBLF + "        TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,                                --예약일자";
            SQL += ComNum.VBLF + "        TO_CHAR(GbReFund_Date,'YY/MM/DD HH24:MI') ReFundDate,             --환불처리요청일자";
            SQL += ComNum.VBLF + "        DECODE(RES,'1','예약','') RES,                                    --예약";
            SQL += ComNum.VBLF + "        DECODE(GbReFund,'1','부도','') GbReFund,                          --부도";
            SQL += ComNum.VBLF + "        GbReFund_Sabun,                                                   --환불처리요청사번";
            SQL += ComNum.VBLF + "        DECODE(GbJob,'1','기관지','2','위','3','대장','4','ERCP') GbJob,  --구분";
            SQL += ComNum.VBLF + "        DECODE(GbSunap,'1','접수','2','미접수','*','취소') GbSunap        --수납여부";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ENDO_JUPMST ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PtNO  = '" + ArgPtno + "' ";
            if (chkRes.Checked == true)
                SQL += ComNum.VBLF + "AND RES   ='1' ";
            SQL += ComNum.VBLF + "  ORDER BY BDate DESC, RDate DESC, GbJob  ";
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
                ComFunc.MsgBox("내시경 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssEndo_Sheet1.RowCount = Dt.Rows.Count;
            ssEndo_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssEndo_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["RES"].ToString().Trim();
                ssEndo_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssEndo_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                ssEndo_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DrCode"].ToString().Trim();
                ssEndo_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["BDate"].ToString().Trim();
                ssEndo_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["RDate"].ToString().Trim();
                ssEndo_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["GbJob"].ToString().Trim();

                ssEndo_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["GbSunap"].ToString().Trim();
                if (Dt.Rows[i]["GbSunap"].ToString().Trim() == "취소")
                    ssEndo.ActiveSheet.Cells[i, 7].BackColor = Color.FromArgb(255, 128, 128);
                else
                    ssEndo.ActiveSheet.Cells[i, 7].BackColor = Color.FromArgb(255, 255, 255);


                ssEndo_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["GbReFund"].ToString().Trim();
                if (Dt.Rows[i]["GbReFund"].ToString().Trim() == "부도")
                    ssEndo.ActiveSheet.Cells[i, 8].BackColor = Color.FromArgb(255, 128, 128);
                else
                    ssEndo.ActiveSheet.Cells[i, 8].BackColor = Color.FromArgb(255, 255, 255);

                ssEndo_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["ReFundDate"].ToString().Trim();
                ssEndo_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["GbReFund_Sabun"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            CF = new ComFunc();
            CS = new clsSpread();
            CPO = new clsOumsad();
            CPP = new clsPmpaPrint();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaMstExamRes frm = new frmPmpaMstExamRes();
            ComFunc.Form_Center(frm);

            ComFunc.SetAllControlClear(pnlTop);

            eFrm_Clear();
            chkEnd.Checked = true;

            txtPtno.Text = "";

            if (clsPmpaPb.GstrResExamPtno != "")
                txtPtno.Text = clsPmpaPb.GstrResExamPtno;

            txtPtno.Select();
        }


        private void eFrm_Clear()
        {
            ComFunc.SetAllControlClear(pnlBody);

            btnSave.Enabled = false;
            btnRet.Enabled = false;
            chkEnd.Checked = true;
            FstrRowID = "";
            FnAmt6 = 0;
            FnWrtno = 0;
        }


        private void CHOICE_Value(string cValue)
        {
            this.txtPtno.Text = cValue;
        }


        private void GET_DATA(PsmhDb pDbCon)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            if (txtPtno.Text != "")
            {
                eFrm_Clear();

                RESV_EXAM_MST(pDbCon, txtPtno.Text);
                RESV_EXAM_VIEW(pDbCon, txtPtno.Text);
                RESV_ENDO_EXAM_VIEW(pDbCon, txtPtno.Text);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            eFrm_Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPtno = string.Empty;
            string strDept = string.Empty;
            string strBdate = string.Empty;
            long nWrtno = 0;
            long nAmt = 0;

            if (e.Row > -1 && e.Column > 0)
            {
                strPtno = ssList1_Sheet1.Cells[e.Row, 1].Text.Trim();
                strDept = ssList1_Sheet1.Cells[e.Row, 2].Text.Trim();
                strBdate = ssList1_Sheet1.Cells[e.Row, 7].Text.Trim();
                nAmt =  Convert.ToInt64(VB.Replace(ssList1_Sheet1.Cells[e.Row, 9].Text.Trim(), ",", ""));
                nWrtno = Convert.ToInt64( ssList1_Sheet1.Cells[e.Row, 20].Text.Trim());

                RESV_EXAM_ORDER_VIEW(clsDB.DbCon, strPtno, strBdate, strDept, nWrtno, nAmt); //예약검사별 오더조회
            }
        }

        //예약검사별 오더조회
        private void RESV_EXAM_ORDER_VIEW(PsmhDb pDbCon, string strPtno, string strBdate, string strDept, long nWrtno, long nAmt)
        {
            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_Clear(ssList2, ssList2_Sheet1.RowCount, ssList2_Sheet1.ColumnCount);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.Ptno, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.BDate,'YYYY-MM-DD') BDate, ";
            SQL += ComNum.VBLF + "        A.DeptCode, A.DrCode, A.SuCode, ";
            SQL += ComNum.VBLF + "        A.Qty, A.Nal, A.Bun, ";
            SQL += ComNum.VBLF + "        A.Bi, A.OrderNo, A.WRTNO, ";
            SQL += ComNum.VBLF + "        B.SuNameK ";
            SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_OORDER A, ";
            SQL += ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_SUN B ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND A.PTNO        = '" + strPtno + "' ";
            SQL += ComNum.VBLF + "    AND A.BDATE       = TO_DATE('" + strBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.DEPTCODE    = '" + strDept + "' ";
            SQL += ComNum.VBLF + "    AND A.RES         = '1' ";                //예약검사
            SQL += ComNum.VBLF + "    AND A.GbSunap IN ('1','2')  ";            //예약처리
            SQL += ComNum.VBLF + "    AND A.WRTNO       = " + nWrtno + " ";

            if (nAmt >= 0)
                SQL += ComNum.VBLF + "AND A.Nal >= 0 ";
            else
                SQL += ComNum.VBLF + "AND A.Nal < 0 ";

            SQL += ComNum.VBLF + "    AND A.SuCode      = B.SuNext(+) ";
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
                ComFunc.MsgBox("예약 처방내역 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList2_Sheet1.RowCount = Dt.Rows.Count;
            ssList2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList2_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["Ptno"].ToString().Trim();
                ssList2_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["BDate"].ToString().Trim();
                ssList2_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                ssList2_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DrCode"].ToString().Trim();
                ssList2_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["Bi"].ToString().Trim();
                ssList2_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["SuCode"].ToString().Trim();
                ssList2_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["SuNameK"].ToString().Trim();
                ssList2_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["Qty"].ToString().Trim();
                ssList2_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["Nal"].ToString().Trim();
                ssList2_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["Bun"].ToString().Trim();
                ssList2_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["OrderNo"].ToString().Trim();
                ssList2_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["WRTNO"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void ssList1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            int intCnt = 0;
            string strDept = string.Empty;
            string strBi = string.Empty;
            string strBdate = string.Empty;
            string strTrsDate = string.Empty;
            string strRetDate = string.Empty;

            if (e.Column != 0) { return; }

            if (Convert.ToBoolean(ssList1_Sheet1.Cells[e.Row, 0].Value) == true)
            {
                for (int i = 0; i < ssList1_Sheet1.RowCount; i++)
                {
                    if (ssList1_Sheet1.Cells[i, 0].Text == "1") { intCnt += 1; }

                    if (intCnt > 1)
                    {
                        ComFunc.MsgBox("예약검사를 하나만 선택후 작업하시기 바랍니다.", "확인");
                        ssList1_Sheet1.Cells[e.Row, 0].Text = "0";
                        return;
                    }
                }

                ssList1_Sheet1.Cells[e.Row, 1, e.Row, ssList1_Sheet1.ColumnCount-1].BackColor = System.Drawing.Color.FromArgb(202, 255, 202);

                FstrRowID = ssList1_Sheet1.Cells[e.Row, 24].Text.Trim();
                strDept = ssList1_Sheet1.Cells[e.Row, 2].Text.Trim();
                strBi = ssList1_Sheet1.Cells[e.Row, 3].Text.Trim();
                strBdate = ssList1_Sheet1.Cells[e.Row, 7].Text.Trim();
                FnAmt6 = Convert.ToInt64(VB.Replace(ssList1_Sheet1.Cells[e.Row, 9].Text.Trim(), ",", ""));
                FnWrtno = Convert.ToInt64(ssList1_Sheet1.Cells[e.Row, 20].Text.Trim());
                strTrsDate = ssList1_Sheet1.Cells[e.Row, 11].Text.Trim();
                if (strTrsDate == "") { btnSave.Enabled = true; }

                strRetDate = ssList1_Sheet1.Cells[e.Row, 14].Text.Trim();

                if (strTrsDate == "")
                {
                    if (strBi == "21")
                    {
                        if (FnAmt6 >= 0) { btnRet.Enabled = true; }
                    }
                    else
                    {
                        if (FnAmt6 > 0) { btnRet.Enabled = true; }
                    }
                }

                RESV_EXAM_ORDER_VIEW(clsDB.DbCon, txtPtno.Text.Trim(), strBdate, strDept, FnWrtno, FnAmt6);
                RESV_ENDO_EXAM_VIEW(clsDB.DbCon, txtPtno.Text.Trim());
            }
            else
            {
                ssList1_Sheet1.Cells[e.Row, 1, e.Row, ssList1_Sheet1.ColumnCount-1].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                FstrRowID = "";
                FnAmt6 = 0;

                CS.Spread_Clear(ssList2, ssList2_Sheet1.RowCount, ssList2_Sheet1.ColumnCount);
                btnSave.Enabled = false;
                btnRet.Enabled = false;
            }
        }

    }
}
