using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 접수부도자 환불관리
/// Author : 박병규
/// Create Date : 2017.07.17
/// </summary>
/// <history>
/// </history>
/// <seealso cref="frmJepsuFailed.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaEntryJupsuFail : Form
    {
        clsSpread CS = null;
        clsPmpaFunc CPF = null;

        DataTable Dt = new DataTable();
        DataTable DtPat = new DataTable();

        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        string FstrRowID = string.Empty;
        long FnRAmt = 0;
        string FstrPtno = "";
        string FstrPGID = "";


        public frmPmpaEntryJupsuFail()
        {
            InitializeComponent();
            setParam();
        }

        public frmPmpaEntryJupsuFail(string ArgPtno = "", string ArgProgramID = "")
        {
            InitializeComponent();
            FstrPtno = ArgPtno;
            FstrPGID = ArgProgramID;
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eForm_Load);

            //KeyPress 이벤트
            this.txtPtno.KeyPress += new KeyPressEventHandler(KeyPressEvent);
            this.txtRemark.KeyPress += new KeyPressEventHandler(KeyPressEvent);

            this.btnPtnoSearch.Click += new EventHandler(eCtl_Click);
            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnCancel.Click += new EventHandler(eCtl_Click);
            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnPtnoSearch)
            {
                frmSearchPano frmPtno = new frmSearchPano();
                frmPtno.Show();

                if (clsPublic.GstrPtno != "") { txtPtno.Text = clsPublic.GstrPtno; }
            }
            else if (sender == this.btnSearch)
                Search_Process(clsDB.DbCon);
            else if (sender == this.btnCancel)
            {
                eForm_Clear();
                txtPtno.Focus();
            }
            else if (sender == this.btnSave)
                Seve_Process(clsDB.DbCon);
            else if (sender == this.btnExit)
                this.Close();

        }

        private void Seve_Process(PsmhDb pDbCon)
        {
            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            if (txtRemark.Text.Trim().Length == 0)
            {
                ComFunc.MsgBox("보관금액 반환사유가 누락되었습니다.", "오류");
                txtRemark.Focus();
                return;
            }

            if (FnRAmt != 0)
                ComFunc.MsgBox("기 환불된 환자입니다. 환불사유만 수정됩니다.", "확인");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_REFUND ";
                SQL += ComNum.VBLF + "    SET RREMARK   = '" + txtRemark.Text + "' ";
                if (FnRAmt == 0)
                {
                    SQL += ComNum.VBLF + "   ,RDATE     = TO_DATE('" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "','YYYY-MM-DD HH24:MI') ,";
                    SQL += ComNum.VBLF + "    RSABUN    = '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "    RPART     = '" + clsType.User.JobPart + "', ";
                    SQL += ComNum.VBLF + "    RAMT      = CAMT ";
                }
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND ROWID = '" + FstrRowID + "'";
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
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_REFUNDHIS ";
                SQL += ComNum.VBLF + "        (ACTDATE, PANO, DEPTCODE, ";
                SQL += ComNum.VBLF + "         DRCODE, CHOJAE, BI, ";
                SQL += ComNum.VBLF + "         SNAME, SEX, AGE, ";
                SQL += ComNum.VBLF + "         GBGAMEK, GBSPC, JIN, ";
                SQL += ComNum.VBLF + "         BOHUN, YDATE1, YDATE2, ";
                SQL += ComNum.VBLF + "         YDATE3, AMT1, AMT2, ";
                SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                SQL += ComNum.VBLF + "         AMT6, AMT7, GELCODE, ";
                SQL += ComNum.VBLF + "         CAMT, CSABUN, CPART, ";
                SQL += ComNum.VBLF + "         RDATE, RAMT, RSABUN, ";
                SQL += ComNum.VBLF + "         RPART, RREMARK, CARDSEQNO, ";
                SQL += ComNum.VBLF + "         EntDate, Jiwon, Gubun ) ";
                SQL += ComNum.VBLF + "  SELECT ACTDATE, PANO, DEPTCODE, ";
                SQL += ComNum.VBLF + "         DRCODE, CHOJAE, BI, ";
                SQL += ComNum.VBLF + "         SNAME, SEX, AGE, ";
                SQL += ComNum.VBLF + "         GBGAMEK, GBSPC, JIN, ";
                SQL += ComNum.VBLF + "         BOHUN, YDATE1, YDATE2, ";
                SQL += ComNum.VBLF + "         YDATE3, AMT1, AMT2, ";
                SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                SQL += ComNum.VBLF + "         AMT6, AMT7, GELCODE, ";
                SQL += ComNum.VBLF + "         CAMT, CSABUN, CPART, ";
                SQL += ComNum.VBLF + "         RDATE, RAMT, RSABUN, ";
                SQL += ComNum.VBLF + "         RPART, RREMARK, CARDSEQNO, ";
                SQL += ComNum.VBLF + "         EntDate, Jiwon, Gubun ";
                SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "OPD_REFUND ";
                SQL += ComNum.VBLF + "   WHERE 1        = 1 ";
                SQL += ComNum.VBLF + "     AND ROWID    = '" + FstrRowID + "' ";
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
                ComFunc.MsgBox("환불되었습니다.", "알림");
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

            Search_Process(pDbCon);
        }

        private void KeyPressEvent(object sender, KeyPressEventArgs e)
        {

            if (sender == this.txtPtno && e.KeyChar == (Char)13)
            {
                txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(VB.Val(txtPtno.Text.Trim())));

                DtPat = CPF.Get_BasPatient(clsDB.DbCon, txtPtno.Text);

                if (DtPat.Rows.Count > 0)
                {
                    txtSname.Text = DtPat.Rows[0]["SNAME"].ToString().Trim() + " ";
                    txtSname.Text += DtPat.Rows[0]["SEX"].ToString().Trim() + " ";
                    txtSname.Text += DtPat.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES( DtPat.Rows[0]["JUMIN3"].ToString().Trim()); 
                }
                else
                {
                    ComFunc.MsgBox("등록번호를 다시 입력하시기 바랍니다.", "확인");
                }

                DtPat.Dispose();
                DtPat = null;

                btnSearch.Focus();
            }

            if (sender == this.txtRemark && e.KeyChar == (Char)13) { btnSave.Focus(); }

        }

        private void eForm_Load(object sender, EventArgs e)
        {
            CS = new clsSpread();
            CPF = new clsPmpaFunc();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaEntryResFail frm = new frmPmpaEntryResFail();
            ComFunc.Form_Center(frm);

            eForm_Clear();

            if (FstrPtno != "")
            {
                txtPtno.Text = FstrPtno;
                Search_Process(clsDB.DbCon);
            }

            txtPtno.Focus();
        }

        private void eForm_Clear()
        {
            ComFunc.SetAllControlClear(pnlTop);
            CS.Spread_All_Clear(ssList);
            CS.Spread_Clear(ssFind, ssFind_Sheet1.RowCount, ssFind_Sheet1.ColumnCount);
            txtRemark.Text = "";
        }
        
        private void Search_Process(PsmhDb pDbCon)
        {
            if (txtPtno.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 입력하시기 바랍니다.", "확인");
                txtPtno.Focus();
                return;
            }

            txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(VB.Val(txtPtno.Text.Trim())));

            DtPat = CPF.Get_BasPatient(pDbCon, txtPtno.Text);

            if (DtPat.Rows.Count > 0)
            {
                txtSname.Text = DtPat.Rows[0]["SNAME"].ToString().Trim() + " ";
                txtSname.Text += DtPat.Rows[0]["SEX"].ToString().Trim() + " ";
                txtSname.Text += DtPat.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
            }
            else
            {
                txtSname.Text = "";
                ComFunc.MsgBox("등록번호를 다시 입력하시기 바랍니다.", "확인");
            }

            DtPat.Dispose();
            DtPat = null;


            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE,        --예약부도 정리일자";
            SQL += ComNum.VBLF + "        PANO , DEPTCODE, BI,                          --등록번호, 진료과, 유형";
            SQL += ComNum.VBLF + "        GUBUN,                                        ";
            SQL += ComNum.VBLF + "        TO_CHAR(YDATE1,'YYYY-MM-DD') YDATE1,          ";
            SQL += ComNum.VBLF + "        TO_CHAR(YDATE3,'YYYY-MM-DD HH24:MI') YDATE3,  ";
            SQL += ComNum.VBLF + "        NVL(AMT7, 0) AMT7, NVL(CAMT, 0) CAMT, CSABUN,                           --진찰료영수금액, 예약금보관금액, 예약금보관 사번";
            SQL += ComNum.VBLF + "        TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,            ";
            SQL += ComNum.VBLF + "        NVL(RAMT, 0) RAMT, RSABUN, RREMARK,                        ";
            SQL += ComNum.VBLF + "        CARDSEQNO, ROWID                              --예약보관금 카드승인 일련번호";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_REFUND ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + txtPtno.Text + "'";

            if (FstrPGID == "")
            {
                SQL += ComNum.VBLF + "AND GUBUN = '01' ";
            }

            SQL += ComNum.VBLF + "  ORDER BY RDATE DESC ,ACTDATE DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                //ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                if (Dt.Rows[i]["GUBUN"].ToString().Trim() == "00")
                    ssList_Sheet1.Cells[i, 0].Text = "예약접수";
                else
                    ssList_Sheet1.Cells[i, 0].Text = "당일접수";

                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["ACTDATE"].ToString().Trim();   //부도일자
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();  //진료과
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["YDATE3"].ToString().Trim();    //예약일자
                ssList_Sheet1.Cells[i, 4].Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[i]["AMT7"]));      //예약금액
                ssList_Sheet1.Cells[i, 5].Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[i]["CAMT"]));      //예약 보관금액
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["CSABUN"].ToString().Trim();    //예약 보관사번
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["RDATE"].ToString().Trim();     //보관금액 반환일자
                ssList_Sheet1.Cells[i, 8].Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[i]["RAMT"]));      //보관금액 반환금액
                ssList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["RSABUN"].ToString().Trim();    //보관금액 반환사번
                ssList_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["RREMARK"].ToString().Trim();  //보관금액 반환사유
                ssList_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["CARDSEQNO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 12].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

            CS.Spread_Clear(ssFind, ssFind_Sheet1.RowCount, ssFind_Sheet1.ColumnCount);
            txtRemark.Text = "";
        }


        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            CS.Spread_Clear(ssFind, ssFind_Sheet1.RowCount, 1);

            ssFind_Sheet1.Cells[0, 0].Text = ssList_Sheet1.Cells[e.Row, 3].Text;    //예약일자
            ssFind_Sheet1.Cells[1, 0].Text = ssList_Sheet1.Cells[e.Row, 1].Text;    //부도일자
            ssFind_Sheet1.Cells[2, 0].Text = ssList_Sheet1.Cells[e.Row, 4].Text;    //예약금액
            ssFind_Sheet1.Cells[3, 0].Text = ssList_Sheet1.Cells[e.Row, 5].Text;    //예약 보관금액
            ssFind_Sheet1.Cells[4, 0].Text = ssList_Sheet1.Cells[e.Row, 6].Text;    //예약 보관사번
            ssFind_Sheet1.Cells[5, 0].Text = ssList_Sheet1.Cells[e.Row, 7].Text;    //보관금액 반환일자
            ssFind_Sheet1.Cells[6, 0].Text = ssList_Sheet1.Cells[e.Row, 8].Text;    //보관금액 반환금액
            ssFind_Sheet1.Cells[7, 0].Text = ssList_Sheet1.Cells[e.Row, 9].Text;    //보관금액 반환사번
            ssFind_Sheet1.Cells[8, 0].Text = ssList_Sheet1.Cells[e.Row, 10].Text;   //보관금액 반환사유

            FnRAmt = Convert.ToInt32(VB.Replace(ssList_Sheet1.Cells[e.Row, 8].Text, ",",""));
            FstrRowID = ssList_Sheet1.Cells[e.Row, 12].Text;
            txtRemark.Text = ssList_Sheet1.Cells[e.Row, 10].Text;

            if (FstrRowID != "")
            {
                if (chkRemark.Checked == true)
                    txtRemark.Text = clsPublic.GstrSysDate + "접수비대체";

                txtRemark.Focus();
            }
        }

    }
}
