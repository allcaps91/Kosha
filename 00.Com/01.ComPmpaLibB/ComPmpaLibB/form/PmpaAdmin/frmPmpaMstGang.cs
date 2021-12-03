using ComLibB;
using System.Windows.Forms;
using System;
using System.Data;
using System.Drawing;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 장애인 등록관리
/// Author : 박병규
/// Create Date : 2017.06.16
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaMasterGang : Form
    {
        clsUser CU = null;
        ComFunc CF = null;
        ComQuery CQ = null;
        clsSpread CS = null;
        clsPmpaFunc CPF = null;
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        string strRowID = string.Empty;

        public frmPmpaMasterGang()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eForm_Load);

            //KeyPress 이벤트
            this.txtPtno.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtPtno2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtJumin.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.cboBun.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpGangDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpHDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpEntDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtSabun.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpFDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpDelDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnDelete.Click += new EventHandler(eCtl_Click);
            this.btnCancel.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
                Get_DataLoad(clsDB.DbCon);
            else if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnDelete)
                Delete_Process(clsDB.DbCon);
            else if (sender == this.btnCancel)
            {
                PnlRight_Clear();
                txtPtno2.Focus();
            }

        }

        private void Delete_Process(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            if (txtPtno2.Text == "") { ComFunc.MsgBox("등록번호를 입력 바랍니다."); return; }
            if (strRowID == "") { ComFunc.MsgBox("삭제할 장애환자를 선택바랍니다."); return; }

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
                SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_GANGMST";
                SQL += ComNum.VBLF + "   SET DELDATE    = TO_DATE('" + dtpDelDate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " WHERE ROWID      = '" + strRowID + "' ";
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

                Get_DataLoad(pDbCon);
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
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            if (txtPtno2.Text == "") { ComFunc.MsgBox("등록번호를 입력 바랍니다."); return; }

            if (dtpFDate.Checked == false || dtpTDate.Checked == false)
            {
                if (ComFunc.MsgBoxQ("유효기간 설정이 없사오니 확인하시기 바랍니다."
                + ComNum.VBLF + "계속 등록을 진행하시겠습니까?",
                "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            String strJumin = string.Empty;
            strJumin = VB.Left(txtJumin.Text, 6) + VB.Right(txtJumin.Text, 7);

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                if (strRowID == "")
                {
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_GANGMST ";
                    SQL += ComNum.VBLF + "       (BDATE, PANO, JUMIN,JUMIN_NEW, SNAME, GANGBUN, GANGDATE, ";
                    SQL += ComNum.VBLF + "        ENTDATE, SABUN, HDATE, SDATE, EDATE) ";
                    SQL += ComNum.VBLF + " VALUES(TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "                '" + txtPtno2.Text + "', ";
                    SQL += ComNum.VBLF + "                '" + VB.Left(strJumin, 7) + "******', ";
                    SQL += ComNum.VBLF + "                '" + clsAES.AES(strJumin) + "', ";
                    SQL += ComNum.VBLF + "                '" + txtSname.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "                '" + VB.Left(cboBun.Text.Trim(), 1) + "', ";

                    if (dtpGangDate.Checked == true)
                        SQL += ComNum.VBLF + "                TO_DATE('" + dtpGangDate.Text + "','YYYY-MM-DD'), ";
                    else
                        SQL += ComNum.VBLF + "            '', ";

                    SQL += ComNum.VBLF + "                SYSDATE, '" + clsType.User.Sabun + "', ";

                    if (dtpHDate.Checked == true)
                        SQL += ComNum.VBLF + "                TO_DATE('" + dtpHDate.Text + "','YYYY-MM-DD'),";
                    else
                        SQL += ComNum.VBLF + "            '', ";

                    if (dtpFDate.Checked == true)
                        SQL += ComNum.VBLF + "                TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD'),";
                    else
                        SQL += ComNum.VBLF + "            '', ";

                    if (dtpTDate.Checked == true)
                        SQL += ComNum.VBLF + "                TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')) ";
                    else
                        SQL += ComNum.VBLF + "            '') ";

                }
                else
                {
                    SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_GANGMST";
                    SQL += ComNum.VBLF + "   SET PANO       = '" + txtPtno2.Text + "',  ";
                    SQL += ComNum.VBLF + "       SNAME      = '" + txtSname.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "       JUMIN      = '" + VB.Left(strJumin, 7) + "******', ";
                    SQL += ComNum.VBLF + "       JUMIN_NEW  = '" + clsAES.AES(strJumin) + "', ";
                    SQL += ComNum.VBLF + "       GANGBUN    = '" + VB.Left(cboBun.Text.Trim(), 1) + "'  ";

                    if (dtpGangDate.Checked == true)
                        SQL += ComNum.VBLF + "       , GANGDATE   = TO_DATE('" + dtpGangDate.Text + "','YYYY-MM-DD') ";

                    if (dtpHDate.Checked == true)
                        SQL += ComNum.VBLF + "       , HDATE      = TO_DATE('" + dtpHDate.Text + "','YYYY-MM-DD') ";

                    if (dtpFDate.Checked == true)
                        SQL += ComNum.VBLF + "       , SDATE      = TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";

                    if (dtpTDate.Checked == true)
                        SQL += ComNum.VBLF + "       , EDATE      = TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";

                    SQL += ComNum.VBLF + " WHERE ROWID      = '" + strRowID + "' ";
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

                Get_DataLoad(pDbCon);
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
            if (sender == this.txtPtno && e.KeyChar == (Char)13)
            {
                if (txtPtno.Text != "") 
                    txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));

                btnSearch.Select();
            }
            if (sender == this.txtJumin && e.KeyChar == (Char)13) { cboBun.Select(); }
            if (sender == this.cboBun && e.KeyChar == (Char)13) { dtpGangDate.Select(); }
            if (sender == this.dtpGangDate && e.KeyChar == (Char)13) { dtpHDate.Select(); }
            if (sender == this.dtpHDate && e.KeyChar == (Char)13) { dtpEntDate.Select(); }
            if (sender == this.dtpEntDate && e.KeyChar == (Char)13) { dtpFDate.Select(); }
            if (sender == this.dtpFDate && e.KeyChar == (Char)13) { dtpTDate.Select(); }
            if (sender == this.dtpTDate && e.KeyChar == (Char)13) { dtpDelDate.Select(); }
            if (sender == this.dtpDelDate && e.KeyChar == (Char)13) { btnSave.Select(); }

            if (sender == this.txtPtno2 && e.KeyChar == (Char)13)
            {
                if (txtPtno2.Text != "")
                    txtPtno2.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno2.Text));

                Get_GangMst();
                txtJumin.Select();
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

            frmPmpaMasterGang frm = new frmPmpaMasterGang();
            ComFunc.Form_Center(frm);

            ComFunc.SetAllControlClear(pnlLeftTop);
            ComFunc.SetAllControlClear(pnlLeft);
            ComFunc.SetAllControlClear(pnlRight);

            txtSabun.Text = clsType.User.Sabun;
            txtSabunName.Text = clsType.User.JobName;
            CQ.Set_BaseCode_ComboBox(clsDB.DbCon, this.cboBun, "환자장애등급", "");

            txtPtno2.Select();
        }


        private void Get_DataLoad(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (ComQuery.IsJobAuth(this, "R", pDbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;

            CS.Spread_All_Clear(ssList);
            ComFunc.SetAllControlClear(pnlRight);

            if (txtPtno.Text == "") { return; }
            txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, SNAME, JUMIN, JUMIN_NEW, GANGBUN, ";
            SQL += ComNum.VBLF + "        TO_CHAR(GANGDATE,'YYYY-MM-DD') GANGDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EDATE,'YYYY-MM-DD') EDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, ";
            SQL += ComNum.VBLF + "        SABUN, ROWID, ";
            SQL += ComNum.VBLF + "        TO_CHAR(HDATE,'YYYY-MM-DD') HDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE, ";
            SQL += ComNum.VBLF + "        (SELECT NAME FROM ADMIN.BAS_BCODE WHERE GUBUN = '환자장애등급' AND CODE = A.GANGBUN AND ROWNUM = 1) GANGNAME ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GANGMST A ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";

            if (txtPtno.Text.Trim() != "")
                SQL += ComNum.VBLF + "AND PANO = '" + txtPtno.Text + "' ";
 
            if (chkDel.Checked == true)
                SQL += ComNum.VBLF + " AND DELDATE IS NOT NULL ";
 
            SQL += ComNum.VBLF + " ORDER BY SNAME";
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
                ssList_Sheet1.Cells[i, 2].Text = VB.Left( clsAES.DeAES( Dt.Rows[i]["JUMIN_NEW"].ToString().Trim()),6) + "-" + VB.Right(clsAES.DeAES(Dt.Rows[i]["JUMIN_NEW"].ToString().Trim()), 7);
                //ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["GANGBUN"].ToString().Trim() + " 급";
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["GANGNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["GANGDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["SABUN"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["ENTDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["HDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["SDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["EDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["DELDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                ssList_Sheet1.Cells[i, 12].Text = Dt.Rows[i]["GANGBUN"].ToString().Trim();

                if (Dt.Rows[i]["DELDATE"].ToString().Trim() != "")
                {
                    CS.setSpdForeColor(ssList, i, 0, i, ssList_Sheet1.ColumnCount-1, System.Drawing.Color.Red);
                }
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        void PnlRight_Clear()
        {
            txtPtno2.Text = "";
            txtSname.Text = "";
            txtJumin.Text = "";
            cboBun.SelectedIndex = 0;
            dtpGangDate.Checked = false;
            dtpHDate.Checked = false;
            dtpEntDate.Checked = false;
            dtpEntDate.Enabled = true;
            dtpFDate.Checked = false;
            dtpTDate.Checked = false;
            dtpDelDate.Checked = false;
            dtpDelDate.Enabled = true;
            strRowID = "";
        }


        private void Get_GangMst()
        {
            DataTable Dt = new DataTable();
            DataTable DtPat = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            txtSname.Text = "";
            txtJumin.Text = "";
            cboBun.SelectedIndex = 0;
            dtpGangDate.Checked = false;
            dtpHDate.Checked = false;
            dtpEntDate.Checked = false;
            dtpEntDate.Enabled = true;
            dtpFDate.Checked = false;
            dtpTDate.Checked = false;
            dtpDelDate.Checked = false;
            dtpDelDate.Enabled = true;
            strRowID = "";

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, SNAME, JUMIN, JUMIN_NEW, GANGBUN, ";
            SQL += ComNum.VBLF + "        TO_CHAR(GANGDATE,'YYYY-MM-DD') GANGDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(EDATE,'YYYY-MM-DD') EDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, ";
            SQL += ComNum.VBLF + "        SABUN, ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GANGMST ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND PANO = '" + txtPtno2.Text + "' ";
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
                txtSname.Text = Dt.Rows[0]["SNAME"].ToString().Trim();
                txtJumin.Text = VB.Left(clsAES.DeAES(Dt.Rows[0]["JUMIN_NEW"].ToString().Trim()), 6) + "-" + VB.Right(clsAES.DeAES(Dt.Rows[0]["JUMIN_NEW"].ToString().Trim()), 7);
                dtpGangDate.Text = Dt.Rows[0]["GANGDATE"].ToString().Trim();
                ComFunc.ComboFind(cboBun, "L", 1, Dt.Rows[0]["GANGBUN"].ToString().Trim());
                dtpEntDate.Text = Dt.Rows[0]["ENTDATE"].ToString().Trim();
                dtpEntDate.Checked = false;
                dtpEntDate.Enabled = false;
                txtSabun.Text = Dt.Rows[0]["SABUN"].ToString().Trim();
                txtSabunName.Text = CF.Read_SabunName(clsDB.DbCon, Dt.Rows[0]["SABUN"].ToString().Trim());
                dtpFDate.Text = Dt.Rows[0]["SDATE"].ToString().Trim();
                dtpTDate.Text = Dt.Rows[0]["EDATE"].ToString().Trim();

                dtpDelDate.Checked = false;
                
                strRowID = Dt.Rows[0]["ROWID"].ToString().Trim();
            }
            else
            {
                DtPat = CPF.Get_BasPatient(clsDB.DbCon, txtPtno2.Text);

                if (DtPat.Rows.Count == 0)
                {
                    ComFunc.MsgBox("환자마스터에 등록후 장애인 등록관리를 하시기 바랍니다.");
                }
                else
                {
                    txtSname.Text = DtPat.Rows[0]["SNAME"].ToString().Trim();
                    txtJumin.Text = DtPat.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
                }
                DtPat.Dispose();
                DtPat = null;
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
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
                strTitle = "장애인 환자 등록 명단";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);

            }
        }

        private void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            PnlRight_Clear();

            txtPtno2.Text = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            txtSname.Text = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
            txtJumin.Text = ssList_Sheet1.Cells[e.Row, 2].Text.Trim();
            //ComFunc.ComboFind(cboBun, "L", 1, ssList_Sheet1.Cells[e.Row, 3].Text.Trim());
            ComFunc.ComboFind(cboBun, "L", 1, ssList_Sheet1.Cells[e.Row, 12].Text.Trim());
            dtpGangDate.Text = ssList_Sheet1.Cells[e.Row, 4].Text.Trim();
            dtpHDate.Text = ssList_Sheet1.Cells[e.Row, 7].Text.Trim();
            dtpEntDate.Text = ssList_Sheet1.Cells[e.Row, 6].Text.Trim();
            txtSabun.Text = ssList_Sheet1.Cells[e.Row, 5].Text.Trim();
            txtSabunName.Text = CF.Read_SabunName(clsDB.DbCon, ssList_Sheet1.Cells[e.Row, 5].Text.Trim());

            dtpFDate.Text = ssList_Sheet1.Cells[e.Row, 8].Text.Trim();
            dtpTDate.Text = ssList_Sheet1.Cells[e.Row, 9].Text.Trim();
            dtpDelDate.Text = ssList_Sheet1.Cells[e.Row, 10].Text.Trim();

            strRowID = ssList_Sheet1.Cells[e.Row, 11].Text.Trim();
        }




        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
