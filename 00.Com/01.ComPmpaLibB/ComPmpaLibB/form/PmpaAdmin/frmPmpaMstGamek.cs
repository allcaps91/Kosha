using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 천주교신자 감액대상자 등록관리
/// Author : 박병규
/// Create Date : 2017.06.22
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaMasterGamek : Form
    {
        clsPmpaFunc CPF = null;
        ComFunc CF = null;
        clsUser CU = null;
        clsSpread CS = null;

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        string strRowID = string.Empty;

        public frmPmpaMasterGamek()
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
            this.cboBun.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpFdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.btnSearch.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txtPtno2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtJumin.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtRoman.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtSeName.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            //Changed 이벤트
            this.dtpFdate.ValueChanged += new EventHandler(eControl_Changed);
            this.dtpTdate.ValueChanged += new EventHandler(eControl_Changed);
            this.cboBun.SelectedIndexChanged += new EventHandler(eControl_Changed);

            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnDelete.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnDelete)
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
                SQL += ComNum.VBLF + "        '" + clsType.User.Sabun + "', ";
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

        private void Save_Process(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string strGubun = string.Empty;

            if (rdoGubun0.Checked == true)
                strGubun = "0";
            else if (rdoGubun1.Checked == true)
                strGubun = "1";
            else if (rdoGubun2.Checked == true)
                strGubun = "2";
            else
                strGubun = "";

            string strJumin = VB.Left(txtJumin.Text, 6) + VB.Mid(txtJumin.Text, 8, 1) + "******";
            string strJumin_New = VB.Left(txtJumin.Text, 6) + VB.Right(txtJumin.Text, 7);

            if (txtPtno2.Text == "") { ComFunc.MsgBox("등록번호를 입력 바랍니다."); txtPtno2.Focus(); return; }
            if (txtRoman.Text == "") { ComFunc.MsgBox("소속성당을 입력 바랍니다."); txtRoman.Focus(); return; }
            if (txtSeName.Text == "") { ComFunc.MsgBox("세례명을 입력 바랍니다."); txtSeName.Focus(); return; }
            if (strGubun == "") { ComFunc.MsgBox("증빙서류를 입력 바랍니다."); return; }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                if (strRowID == "")
                {
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_GAMFSINGA ";
                    SQL += ComNum.VBLF + "        (GAMDATE, GAMPANO, GAMJUMIN, GAMJUMIN_new, GAMSNAME, ";
                    SQL += ComNum.VBLF + "         GAMROMAN, GAMGUBUN, GAMSABUN, ENTDATE ) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         '" + txtPtno2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + strJumin + "',";
                    SQL += ComNum.VBLF + "         '" + clsAES.AES(strJumin_New) + "', ";
                    SQL += ComNum.VBLF + "         '" + txtSeName.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtRoman.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + strGubun + "', ";
                    SQL += ComNum.VBLF + "         '" + clsType.User.Sabun + "', ";
                    SQL += ComNum.VBLF + "         SYSDATE) ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_GAMFSINGA";
                    SQL += ComNum.VBLF + "    SET GAMPANO        = '" + txtPtno2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        GAMJUMIN       = '" + strJumin + "', ";
                    SQL += ComNum.VBLF + "        GAMJUMIN_NEW   = '" + clsAES.AES(strJumin_New) + "', ";
                    SQL += ComNum.VBLF + "        GAMSNAME       = '" + txtSeName.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        GAMROMAN       = '" + txtRoman.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        GAMGUBUN       = '" + strGubun + "', ";
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

        private void eControl_Changed(object sender, EventArgs e)
        {

            CS.Spread_All_Clear(ssList);

            ComFunc.SetAllControlClear(pnlRight);
        }

        private void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (Char)13) { cboBun.Focus(); }
            if (sender == this.cboBun && e.KeyChar == (Char)13) { dtpFdate.Focus(); }
            if (sender == this.dtpFdate && e.KeyChar == (Char)13) { dtpTdate.Focus(); }
            if (sender == this.dtpTdate && e.KeyChar == (Char)13) { btnSearch.Focus(); }
            if (sender == this.btnSearch && e.KeyChar == (Char)13) { btnPrint.Focus(); }

            if (sender == this.txtJumin && e.KeyChar == (Char)13) { txtRoman.Focus(); }
            if (sender == this.txtRoman && e.KeyChar == (Char)13) { txtSeName.Focus(); }
            if (sender == this.txtSeName && e.KeyChar == (Char)13) { dtpDate.Focus(); }
            if (sender == this.dtpDate && e.KeyChar == (Char)13) { btnSave.Focus(); }

            if (sender == this.txtPtno2 && e.KeyChar == (Char)13)
            {
                Get_GamfSinga(clsDB.DbCon);
                txtJumin.Focus();
            }

        }

        private void Get_GamfSinga(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            DataTable DtP = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strGubun = string.Empty;

            txtSname.Text = "";
            txtJumin.Text = "";
            txtRoman.Text = "";
            txtSeName.Text = "";
            dtpDate.Text = clsPublic.GstrSysDate;
            dtpDate.Checked = true;
            rdoGubun0.Checked = false;
            rdoGubun1.Checked = false;
            rdoGubun2.Checked = false;
            strRowID = "";
            
            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.GAMDATE, A.GAMPANO, A.GAMSNAME,         --감액등록번호, 세례명, ";
            SQL += ComNum.VBLF + "        A.GAMROMAN, A.GAMGUBUN, B.SNAME,          --소속성당, 증명서구분, 환자성명";
            SQL += ComNum.VBLF + "        B.JUMIN1, B.JUMIN2, B.JUMIN3, A.ROWID     --주민번호1, 주민번호2, 주민번호3, ROWID";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMFSINGA A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT B ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND A.GAMPANO = '" + txtPtno2.Text.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND A.GAMPANO = B.PANO ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count >= 1)
            {
                txtSname.Text = Dt.Rows[0]["SNAME"].ToString().Trim();
                txtJumin.Text = Dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString().Trim());
                txtRoman.Text = Dt.Rows[0]["GAMROMAN"].ToString().Trim();
                txtSeName.Text = Dt.Rows[0]["GAMSNAME"].ToString().Trim();

                dtpDate.Text = Dt.Rows[0]["GAMDATE"].ToString().Trim();
                dtpDate.Checked = false;

                strGubun = Dt.Rows[0]["GAMGUBUN"].ToString().Trim();
                switch (strGubun)
                {
                    case "0":
                        rdoGubun0.Checked = true;
                        break;
                    case "1":
                        rdoGubun1.Checked = true;
                        break;
                    case "2":
                        rdoGubun2.Checked = true;
                        break;
                    default:
                        ComFunc.MsgBox("증병서류가 없습니다.", "확인");
                        rdoGubun0.Checked = false;
                        rdoGubun1.Checked = false;
                        rdoGubun2.Checked = false;
                        break;
                }

                strRowID = Dt.Rows[0]["ROWID"].ToString().Trim();

                ComFunc.MsgBox("등록된 자료 내역입니다.", "알림");
            }
            else
            {
                DtP = CPF.Get_BasPatient(pDbCon, string.Format("{0:D8}", Convert.ToInt32(txtPtno2.Text)));

                if (DtP.Rows.Count == 0)
                    ComFunc.MsgBox("환자마스터에 등록후 감액 등록관리를 하시기 바랍니다.");
                else
                {
                    txtSname.Text = DtP.Rows[0]["SNAME"].ToString().Trim();
                    txtJumin.Text = DtP.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(DtP.Rows[0]["JUMIN3"].ToString().Trim());
                }

                DtP.Dispose();
                DtP = null;
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPtno)
            {
                if (txtPtno.Text.Trim() == "") { return; }
                txtPtno.Text =  string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));
            }

            if (sender == this.txtPtno2)
            {
                if (txtPtno2.Text.Trim() == "") { return; }
                txtPtno2.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno2.Text));
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

            frmPmpaMasterGamek frm = new frmPmpaMasterGamek();
            ComFunc.Form_Center(frm);

            ComFunc.SetAllControlClear(pnlLeftTop);
            ComFunc.SetAllControlClear(pnlLeft);
            ComFunc.SetAllControlClear(pnlRightTop);
            ComFunc.SetAllControlClear(pnlRight);

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFdate.Text = VB.DateAdd("D",-30, clsPublic.GstrSysDate).ToString("yyyy-MM-dd");
            dtpTdate.Text = clsPublic.GstrSysDate;
            dtpDate.Text = clsPublic.GstrSysDate;
            dtpDate.Checked = true;

            cboBun.Items.Clear();
            cboBun.Items.Add("*.전체");
            cboBun.Items.Add("0.세례증서");
            cboBun.Items.Add("1.확인서");
            cboBun.Items.Add("2.기타");
            cboBun.SelectedIndex = 0;

            txtPtno2.Select();
        }

        void eForm_Clear()
        {
            txtPtno2.Text = "";
            txtSname.Text = "";
            txtJumin.Text = "";
            txtRoman.Text = "";
            txtSeName.Text = "";
            dtpDate.Text = clsPublic.GstrSysDate;
            dtpDate.Checked = true;
            rdoGubun0.Checked = false;
            rdoGubun1.Checked = false;
            rdoGubun2.Checked = false;
            strRowID = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_DataLoad();
        }

        private void Get_DataLoad()
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
          //  ComFunc.SetAllControlClear(pnlLeft);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.GAMPANO, A.GAMSNAME, ";
            SQL += ComNum.VBLF + "        A.GAMROMAN, A.GAMGUBUN, B.SNAME, ";
            SQL += ComNum.VBLF + "        A.GAMJUMIN, A.GAMJUMIN_NEW, A.ROWID, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.GAMDATE,'YYYY-MM-DD') GAMDATE, A.GAMSABUN, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_USER_NAME(A.GAMSABUN) AS SABUNNAME ";                                                       
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMFSINGA A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT B ";
            SQL += ComNum.VBLF + " WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "   AND A.GAMDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND A.GAMDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";

            if (txtPtno.Text.Trim() != "")
                SQL += ComNum.VBLF + "AND A.GAMPANO = '" + txtPtno.Text.Trim() + "' ";

            if (VB.Left(cboBun.Text.Trim(), 1) != "*")
                SQL += ComNum.VBLF + "AND A.GAMGUBUN = '" + VB.Left(cboBun.Text.Trim(), 1) + "' ";

            SQL += ComNum.VBLF + "   AND A.GAMPANO = B.PANO(+) ";
            SQL += ComNum.VBLF + " ORDER BY GAMDATE, GAMPANO ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
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
                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["GAMPANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SNAME"].ToString().Trim();

                string strJumin_New = clsAES.DeAES(Dt.Rows[i]["GAMJUMIN_NEW"].ToString().Trim());
                ssList_Sheet1.Cells[i, 2].Text = VB.Left(strJumin_New, 6) + "-" + VB.Right(strJumin_New, 7);

                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["GAMROMAN"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["GAMSNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["GAMGUBUN"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["GAMDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["SABUNNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ComFunc.SetAllControlClear(pnlRight);

            txtPtno2.Text = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            txtSname.Text = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
            txtJumin.Text = ssList_Sheet1.Cells[e.Row, 2].Text.Trim();
            txtRoman.Text = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();
            txtSeName.Text = ssList_Sheet1.Cells[e.Row, 4].Text.Trim();
            dtpDate.Text = ssList_Sheet1.Cells[e.Row, 6].Text.Trim();
            dtpDate.Checked = false;

            switch(ssList_Sheet1.Cells[e.Row, 5].Text.Trim())
            {
                case "0":
                    rdoGubun0.Checked = true;
                    break;
                case "1":
                    rdoGubun1.Checked = true;
                    break;
                case "2":
                    rdoGubun2.Checked = true;
                    break;
                default:
                    ComFunc.MsgBox("증병서류가 없습니다.", "확인");
                    rdoGubun0.Checked = false;
                    rdoGubun1.Checked = false;
                    rdoGubun2.Checked = false;
                    break;
            }

            strRowID = ssList_Sheet1.Cells[e.Row, 8].Text.Trim();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            eForm_Clear();
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
                strTitle = "신 자 감 액 명 단";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("등록기간 : " + dtpFdate.Text + " ~ " + dtpTdate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("증빙서류 : " + cboBun.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);

            }
        }
    }
}
