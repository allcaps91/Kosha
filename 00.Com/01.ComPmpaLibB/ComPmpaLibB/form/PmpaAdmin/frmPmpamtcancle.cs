using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpamtcancle : Form
    {
        clsPmpaFunc CPF = null;
        ComFunc CF = null;
        clsUser CU = null;
        clsSpread CS = null;

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        string strRowID = string.Empty;

        public frmPmpamtcancle()
        {
            InitializeComponent();
            setParam();
        }
        private void setParam()
        {
            this.Load += new EventHandler(eForm_Load);

            //LostFocus 이벤트
          
            this.txtPtno2.LostFocus += new EventHandler(eControl_LostFocus);

            //KeyPress 이벤트
          
            this.dtpFdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.btnSearch.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txtPtno2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
           // this.txtJumin.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtDept.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtRemark.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            //Changed 이벤트
            this.dtpFdate.ValueChanged += new EventHandler(eControl_Changed);
            this.dtpTdate.ValueChanged += new EventHandler(eControl_Changed);

            this.ssList.CellClick += new CellClickEventHandler(Spread_CellClick);
            
            this.btnExit.Click += new EventHandler(eCtl_Click);
            this.btnCancel.Click += new EventHandler(eCtl_Click);
            this.btnPrint.Click += new EventHandler(eCtl_Click);
            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnDelete.Click += new EventHandler(eCtl_Click);
        }
        private void Spread_CellClick(object sender, CellClickEventArgs e)
        {
            int Col = 0;
            int Row = 0;

            if (sender == this.ssList)
            {
                ComFunc.SetAllControlClear(pnlRight);
                txtPtno2.Text = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
                txtSname.Text = ssList_Sheet1.Cells[e.Row, 2].Text.Trim();

                txtDept.Text = ssList_Sheet1.Cells[e.Row, 3].Text.Trim();
                txtRemark.Text = ssList_Sheet1.Cells[e.Row, 4].Text.Trim();
                dtpDate.Text = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
                dtpDate.Checked = true;

                strRowID = ssList_Sheet1.Cells[e.Row, 6].Text.Trim();

            }
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnDelete)
                Delete_Process(clsDB.DbCon);
            else if (sender == this.btnSearch)
                Get_DataLoad();
            else if (sender == this.btnExit)
                this.Close();
            else if (sender == this.btnCancel)
                eForm_Clear(); 
            else if (sender == this.btnPrint)
                Get_DataPrint();
            
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
                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "OPD_REJUPSUMST ";
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
                Get_DataLoad();
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

          
           
            if (txtPtno2.Text == "") { ComFunc.MsgBox("등록번호를 입력 바랍니다."); txtPtno2.Focus(); return; }
            if (txtDept.Text == "") { ComFunc.MsgBox("진료과 입력 바랍니다."); txtDept.Focus(); return; }
            if (txtRemark.Text == "") { ComFunc.MsgBox("비고를 입력 바랍니다."); txtRemark.Focus(); return; }
            

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                if (strRowID == "")
                {
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_REJUPSUMST ";
                    SQL += ComNum.VBLF + "        (JDATE, PANO, SNAME, DEPTCODE, DRCODE,SABUN, ";
                    SQL += ComNum.VBLF + "         REMARK, ENTDATE ) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         '" + txtPtno2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtSname.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "         '" + txtDept.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '', ";
                    SQL += ComNum.VBLF + "         '" + clsType.User.Sabun + "', ";
                    SQL += ComNum.VBLF + "         '" + txtRemark.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         SYSDATE) ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_REJUPSUMST";
                    SQL += ComNum.VBLF + "    SET DEPTCODE        = '" + txtDept.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        REMARK       = '" + txtRemark.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        JDATE   = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD'), ";
                   
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
                Get_DataLoad();
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
          
            if (sender == this.dtpFdate && e.KeyChar == (Char)13) { dtpTdate.Focus(); }
            if (sender == this.dtpTdate && e.KeyChar == (Char)13) { btnSearch.Focus(); }
            if (sender == this.btnSearch && e.KeyChar == (Char)13) { btnPrint.Focus(); }

           
            if (sender == this.txtDept && e.KeyChar == (Char)13) { txtRemark.Focus(); }
            if (sender == this.txtRemark && e.KeyChar == (Char)13) { dtpDate.Focus(); }
            if (sender == this.dtpDate && e.KeyChar == (Char)13) { btnSave.Focus(); }

            if (sender == this.txtPtno2 && e.KeyChar == (Char)13)
            {
                Get_GamfSinga(clsDB.DbCon);
     
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
        
            txtDept.Text = "";
            txtRemark.Text = "";
            dtpDate.Text = clsPublic.GstrSysDate;
            dtpDate.Checked = true;
           
            strRowID = "";

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT    SNAME       ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT A ";

            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND PANO = '" + txtPtno2.Text.Trim() + "' ";
     
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
               
            }
            

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void eControl_LostFocus(object sender, EventArgs e)
        {
            //if (sender == this.txtPtno)
            //{
            //    if (txtPtno.Text.Trim() == "") { return; }
            //    txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));
            //}

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

            
            frmPmpaMasterGamek frm = new frmPmpaMasterGamek();
            ComFunc.Form_Center(frm);

            ComFunc.SetAllControlClear(pnlLeftTop);
            ComFunc.SetAllControlClear(pnlLeft);
            ComFunc.SetAllControlClear(pnlRightTop);
            ComFunc.SetAllControlClear(pnlRight);

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFdate.Text = VB.DateAdd("D", -30, clsPublic.GstrSysDate).ToString("yyyy-MM-dd");
            dtpTdate.Text = clsPublic.GstrSysDate;
            dtpDate.Text = clsPublic.GstrSysDate;
            dtpDate.Checked = true;

           

            txtPtno2.Select();
        }

        void eForm_Clear()
        {
            txtPtno2.Text = "";
            txtSname.Text = "";
           
            txtDept.Text = "";
            txtRemark.Text = "";
            dtpDate.Text = clsPublic.GstrSysDate;
            dtpDate.Checked = true;
           
            strRowID = "";
        }

      

        private void Get_DataLoad()
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인
            CS.Spread_All_Clear(ssList);
            Cursor.Current = Cursors.WaitCursor;
            //  ComFunc.SetAllControlClear(pnlLeft);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE, A.PANO, ";
            SQL += ComNum.VBLF + "        A.DEPTCODE, A.DRCODE, B.SNAME, ";
            SQL += ComNum.VBLF + "        A.SABUN, A.REMARK, A.ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_REJUPSUMST A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT B ";
            SQL += ComNum.VBLF + " WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "   AND A.JDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND A.JDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";


            SQL += ComNum.VBLF + "   AND A.PANO = B.PANO(+) ";
            SQL += ComNum.VBLF + " ORDER BY A.JDATE, A.PANO ";
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
                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["JDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();

                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SNAME"].ToString().Trim();

                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["REMARK"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["SABUN"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
               
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

    

        private void btnCancel_Click(object sender, EventArgs e)
        {
            eForm_Clear();
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Get_DataPrint()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

           

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strTitle = " 취소후 예약자 관리 명 단";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("등록기간 : " + dtpFdate.Text + " ~ " + dtpTdate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
               

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
