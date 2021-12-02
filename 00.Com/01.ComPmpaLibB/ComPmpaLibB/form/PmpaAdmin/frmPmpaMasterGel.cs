using ComLibB;
using System.Windows.Forms;
using System;
using System.Data;
using System.Drawing;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스



/// <summary>
/// Description : 개인별 계약처 등록 관리
/// Author : 박병규
/// Create Date : 2017.06.22
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaMasterGel : Form
    {
        clsUser CU = null;
        ComFunc CF = null;
        ComQuery CQ = null;
        clsSpread CS = null;
        clsPmpaFunc CPF = null;
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        string strRowID = "";
        string FstrPtno = "";

        public frmPmpaMasterGel()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaMasterGel(string ArgPtno)
        {
            InitializeComponent();
            FstrPtno = ArgPtno;
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            this.ssGel_Sheet1.Columns[0].AllowAutoSort = true;
            this.ssGel_Sheet1.Columns[1].AllowAutoSort = true;

            //LostFocus 이벤트
            this.txtPtno.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtPtno2.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtGelcode.LostFocus += new EventHandler(eControl_LostFocus);

            //KeyPress 이벤트
            this.txtPtno.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txtPtno2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtGelcode.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtSabun.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpDelDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtDel.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txtLtdName.KeyPress += new KeyPressEventHandler(eControl_KeyPress);


            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnPrint.Click += new EventHandler(eCtl_Click);
            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnDelete.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);
            this.btnSearchGel.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
                Get_DataLoad(clsDB.DbCon);
            else if (sender == this.btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) { return; }     //권한확인

                string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

                if (strPrintName != "")
                {
                    strTitle = "개인별 계약처 명단";

                    strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                    strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                    strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                    setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                    setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                    CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                    ComFunc.Delay(200);

                }

            }
            else if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnDelete)
                Delete_Process(clsDB.DbCon);
            else if (sender == this.btnCancel)
                eForm_Clear();
            else if (sender == this.btnExit)
                this.Close();
            else if (sender == this.btnSearchGel)
                SearchGel_Process(clsDB.DbCon);

        }

        private void SearchGel_Process(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

            ((FarPoint.Win.Spread.FpSpread)ssGel).ActiveSheet.RowCount = 0;
            CS.Spread_Clear(ssGel, ssGel_Sheet1.RowCount, ssGel_Sheet1.ColumnCount);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT MiaName, MiaCode  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND MIACLASS  = '90' ";
            SQL += ComNum.VBLF + "    AND ( DelDate IS NULL OR DelDate ='') ";

            if (txtLtdName.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "    AND MiaName LIKE '%" + txtLtdName.Text.Trim() + "%' ";
            }

            SQL += ComNum.VBLF + "  ORDER BY MIANAME ";
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

            ssGel_Sheet1.RowCount = Dt.Rows.Count;
            ssGel_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssGel_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["MiaCode"].ToString().Trim();
                ssGel_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["MiaName"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void Delete_Process(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            if (txtPtno2.Text == "") { ComFunc.MsgBox("등록번호를 입력 바랍니다."); return; }
            if (txtDel.Text == "") { ComFunc.MsgBox("삭제 참고사항을 입력 바랍니다."); return; }
            if (strRowID == "") { ComFunc.MsgBox("삭제할 환자를 선택바랍니다."); return; }

            if (ComFunc.MsgBoxQ(txtSname2.Text + " 님의 삭제를 진행하시겠습니까?",
            "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT_GEL  ";
                SQL += ComNum.VBLF + "    SET DELDATE   = TO_DATE('" + dtpDelDate.Text + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "        DELREMARK = '" + txtDel.Text + "', ";
                SQL += ComNum.VBLF + "        DELPART   = '" + clsType.User.Sabun + "' ";
                SQL += ComNum.VBLF + "  WHERE PANO      = '" + txtPtno2.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND GELCODE   = '" + txtGelcode.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND SDATE     = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
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

        private void Save_Process(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            if (ComQuery.IsJobAuth(this, "C", pDbCon) == false) { return; }     //권한확인

            if (txtPtno2.Text == "") { ComFunc.MsgBox("등록번호를 입력 바랍니다."); txtPtno2.Focus(); return; }
            if (txtGelcode.Text == "") { ComFunc.MsgBox("계약처코드를 입력 바랍니다."); txtGelcode.Focus(); return; }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                if (strRowID == "")
                {
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_PATIENT_GEL ";
                    SQL += ComNum.VBLF + "        (SDATE, PANO, GELCODE, PART, ENTDATE) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         '" + txtPtno2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtGelcode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtSabun.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         SYSDATE) ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT_GEL";
                    SQL += ComNum.VBLF + "    SET GELCODE        = '" + txtGelcode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        ENTDATE        = SYSDATE ";
                    SQL += ComNum.VBLF + "  WHERE ROWID          = '" + strRowID + "' ";
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

        private void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (Char)13) { btnSearch.Focus(); }

            if (sender == this.txtPtno2 && e.KeyChar == (Char)13) { txtGelcode.Focus(); }
            if (sender == this.txtGelcode && e.KeyChar == (Char)13) { dtpDate.Focus(); }
            if (sender == this.dtpDate && e.KeyChar == (Char)13) { txtSabun.Focus(); }
            if (sender == this.txtSabun && e.KeyChar == (Char)13) { dtpDelDate.Focus(); }
            if (sender == this.dtpDelDate && e.KeyChar == (Char)13) { txtDel.Focus(); }
            if (sender == this.txtDel && e.KeyChar == (Char)13) { btnSave.Focus(); }

            if (sender == this.txtLtdName && e.KeyChar == (Char)13) { btnSearchGel.Focus(); }

        }

        private void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPtno)
            {
                if (txtPtno.Text != "")
                {
                    txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));

                    txtSname.Text = CF.Read_Patient(clsDB.DbCon, txtPtno.Text, "2");

                    if (txtSname.Text.Trim() == "")
                    {
                        ComFunc.MsgBox("등록번호가 없습니다. 확인바랍니다.", "확인");
                        txtPtno.Text = "";
                        txtPtno.Focus();
                        return;
                    }
                }
            }

            if (sender == this.txtPtno2)
            {
                if (txtPtno2.Text != "")
                {
                    txtPtno2.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno2.Text));
                    txtSname2.Text = CF.Read_Patient(clsDB.DbCon, txtPtno2.Text, "2");

                    if (txtSname2.Text.Trim() == "")
                    {
                        ComFunc.MsgBox("등록번호가 없습니다. 확인바랍니다.", "확인");
                        txtPtno2.Text = "";
                        txtPtno2.Focus();
                        return;
                    }
                }
            }

            if (sender == this.txtGelcode)
            {
                if (txtGelcode.Text != "")
                {
                    txtGelName.Text = CF.Read_MiaName(clsDB.DbCon, txtGelcode.Text.Trim(), true);

                    if (txtGelName.Text.Trim() == "")
                    {
                        ComFunc.MsgBox("계약처코드가 없습니다. 확인바랍니다.", "확인");
                        txtGelcode.Text = "";
                        txtGelcode.Focus();
                        return;
                    }
                }
            }
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            CU = new clsUser();
            CF = new ComFunc();
            CQ = new ComQuery();
            CS = new clsSpread();
            CPF = new clsPmpaFunc();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlLeftTop);
            ComFunc.SetAllControlClear(pnlLeft);
            ComFunc.SetAllControlClear(pnlRightTop);
            ComFunc.SetAllControlClear(pnlRight);

            eForm_Clear();

            txtSabun.Text = clsType.User.IdNumber;
            txtSabunName.Text = clsType.User.JobName;


            if (FstrPtno != "")
            {
                txtPtno.Text = FstrPtno;
                txtSname.Text = CF.Read_Patient(clsDB.DbCon,txtPtno.Text, "2");
                Get_DataLoad(clsDB.DbCon);
            }

            txtPtno2.Select();
        }

        void eForm_Clear()
        {
            txtPtno2.Text = "";
            txtSname2.Text = "";
            txtGelcode.Text = "";
            txtGelName.Text = "";
            dtpDate.Text = clsPublic.GstrSysDate;
            dtpDate.Checked = false;
            dtpDate.Enabled = true;
            txtSabun.Text = "";
            txtSabunName.Text = "";
            dtpDelDate.Checked = false;
            dtpDelDate.Enabled = true;
            txtDel.Text = "";
            txtLtdName.Text = "";
        }

        private void Get_DataLoad_Person(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,            --등록일자";
            SQL += ComNum.VBLF + "        PANO, GELCODE, DELREMARK,                     --등록번호, 계약처코드, 삭제참고사항";
            SQL += ComNum.VBLF + "        TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE, ROWID  --삭제일자";
            SQL += ComNum.VBLF + "   FROM BAS_PATIENT_GEL ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND PANO = '" + txtPtno2.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DelDate ='') ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                dtpDate.Text = Dt.Rows[0]["SDATE"].ToString().Trim();
                dtpDate.Checked = false;
                dtpDate.Enabled = false;
                txtGelcode.Text = Dt.Rows[0]["GELCODE"].ToString().Trim();
                txtGelName.Text = CF.Read_MiaName(pDbCon, Dt.Rows[0]["GELCODE"].ToString().Trim(), true);

                if (Dt.Rows[0]["DELDATE"].ToString().Trim() != "")
                {
                    dtpDelDate.Text =  Dt.Rows[0]["DELDATE"].ToString().Trim();
                    dtpDelDate.Checked = false;
                    dtpDelDate.Enabled = false;
                    txtDel.Text = Dt.Rows[0]["DELREMARK"].ToString().Trim();
                }

                strRowID = Dt.Rows[0]["ROWID"].ToString().Trim();
            }
            else
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                eForm_Clear();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void Get_DataLoad(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (ComQuery.IsJobAuth(this, "R", pDbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.PANO, B.SNAME, C.MIANAME, A.GELCODE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.SDATE,'YYYY-MM-DD') SDATE, ";
            SQL += ComNum.VBLF + "        A.PART, A.DELPART, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.DELDATE,'YYYY-MM-DD') DELDATE, ";
            SQL += ComNum.VBLF + "        DELREMARK, A.Rowid ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_GEL A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT B, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_MIA C ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND C.MIACLASS  = '90' ";

            if (txtPtno.Text.Trim() != "")
                SQL += ComNum.VBLF + "  AND A.PANO = '" + txtPtno.Text.Trim() + "' ";

            if (chkDel.Checked == false)
                SQL += ComNum.VBLF + "  AND (A.DELDATE IS NULL OR A.DelDate ='')  ";

            if (chkDay.Checked == true)
                SQL += ComNum.VBLF + "  AND TRUNC(A.EntDate) = TRUNC(SYSDATE)  ";

            SQL += ComNum.VBLF + "    AND A.PANO = B.PANO(+) ";
            SQL += ComNum.VBLF + "    AND A.GELCODE = C.MIACODE(+) ";
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
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["GELCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["MIANAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["SDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["PART"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["DELDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["DELREMARK"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            ComFunc.SetAllControlClear(pnlRight);
            eForm_Clear();

            txtPtno2.Text = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            txtSname2.Text = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
            txtGelcode.Text = ssList_Sheet1.Cells[e.Row, 2].Text.Trim();
            txtGelName.Text = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();
            dtpDate.Text = ssList_Sheet1.Cells[e.Row, 4].Text.Trim();
            dtpDate.Checked = false;
            dtpDate.Enabled = false;
            txtSabun.Text = ssList_Sheet1.Cells[e.Row, 5].Text.Trim();
            txtSabunName.Text = CF.Read_SabunName(clsDB.DbCon,ssList_Sheet1.Cells[e.Row, 5].Text.Trim());
            dtpDelDate.Text = ssList_Sheet1.Cells[e.Row, 6].Text.Trim();
            dtpDelDate.Checked = false;
            dtpDelDate.Enabled = true;
            txtDel.Text = ssList_Sheet1.Cells[e.Row, 7].Text.Trim();
            strRowID = ssList_Sheet1.Cells[e.Row, 8].Text.Trim();
        }


        private void ssGel_CellClick(object sender, CellClickEventArgs e)
        {
            txtGelcode.Text = ssGel_Sheet1.Cells[e.Row, 0].Text;
            txtGelName.Text = ssGel_Sheet1.Cells[e.Row, 1].Text;
        }

    }
}
