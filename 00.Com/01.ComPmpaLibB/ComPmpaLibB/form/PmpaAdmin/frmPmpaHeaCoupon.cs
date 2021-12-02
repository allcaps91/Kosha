using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 종합검진 외래감액 쿠폰사용등록
/// Author : 박병규
/// Create Date : 2018.06.12
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaHeaCoupon : Form
    {
        clsPmpaFunc CPF = null;
        ComFunc CF = null;
        clsUser CU = null;
        clsSpread CS = null;

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        DataTable Dt1 = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        string FstrRowID = "";
        string FstrPtno = "";
        string FstrDept = "";

        public frmPmpaHeaCoupon()
        {
            InitializeComponent();
            setParam();
        }

        public frmPmpaHeaCoupon(string ArgPtno, string ArgDept)
        {
            InitializeComponent();
            FstrPtno = ArgPtno;
            FstrDept = ArgDept;
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eForm_Load);

            this.txtPtno.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtPtno.LostFocus += new EventHandler(eControl_LostFocus);

            this.btnSearch.Click += new EventHandler(eCtl_Click);
            this.btnSave.Click += new EventHandler(eCtl_Click);
            this.btnCancel.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
                Search_Process(clsDB.DbCon);
            if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
            if (sender == this.btnCancel)
                Cancel_Process(clsDB.DbCon);
            if (sender == this.btnExit)
                this.Close();
        }

        private void Cancel_Process(PsmhDb pDbCon)
        {
            if (FstrRowID == "" || TxtUseYn.Text != "N")
            {
                ComFunc.MsgBox("쿠폰이 선택되지 않았거나 사용되지 않은 쿠폰입니다.");
                return;
            }

            if (DialogResult.Yes == ComFunc.MsgBoxQ("종검감액쿠폰 사용을 미사용처리 하겠습니까?", "확인", MessageBoxDefaultButton.Button1))
            {
                ComFunc.ReadSysDate(pDbCon);

                clsDB.setBeginTran(pDbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE HEA_JEPSU_COUPON  ";
                    SQL += ComNum.VBLF + "    set USETIME   = '', ";
                    SQL += ComNum.VBLF + "        USEDEPT   = '' ";
                    SQL += ComNum.VBLF + "  WHERE ROWID     = '" + FstrRowID + "' ";
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
                    ComFunc.MsgBox("쿠폰을 미사용처리 했습니다.");

                    Search_Process(pDbCon);
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

        private void Search_Process(PsmhDb pDbCon)
        {

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            //ComFunc.SetAllControlClear(pnlLeft);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SNAME, SNAME2 ";
            SQL += ComNum.VBLF + "   FROM BAS_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE PANO = '" + txtPtno.Text.Trim() + "'  ";
            SqlErr = clsDB.GetDataTableEx(ref Dt1, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt1.Rows.Count > 0)
            {
                if (Dt1.Rows[0]["SNAME2"].ToString().Trim() == "")
                    txtSname.Text = Dt1.Rows[0]["SNAME"].ToString().Trim();
                else
                    txtSname.Text = Dt1.Rows[0]["SNAME2"].ToString().Trim();
            }

            Dt1.Dispose();
            Dt1 = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.WRTNO, A.PANO, A.USEDEPT, ";
            SQL += ComNum.VBLF + "        A.ROWID, TO_CHAR(A.SDATE,'YYYY-MM-DD') SDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.USETIME,'YYYY-MM-DD') USETIME, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.MDATE,'YYYY-MM-DD') MDATE ";
            SQL += ComNum.VBLF + "   FROM HEA_JEPSU_COUPON A ";
            SQL += ComNum.VBLF + "  WHERE A.SDATE   >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.SDATE   <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.PTNO    = '" + txtPtno.Text.Trim() + "'  ";
            SQL += ComNum.VBLF + "  ORDER BY SDATE  DESC ";
            SqlErr = clsDB.GetDataTableEx(ref Dt1, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt1.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt1.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = Dt1.Rows[i]["WRTNO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt1.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt1.Rows[i]["SDATE"].ToString().Trim() + "~" + Dt1.Rows[i]["MDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt1.Rows[i]["USEDEPT"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt1.Rows[i]["USETIME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt1.Rows[i]["ROWID"].ToString().Trim();
            }

            Dt1.Dispose();
            Dt1 = null;

            Cursor.Current = Cursors.Default;
        }

        private void Save_Process(PsmhDb pDbCon)
        {
            string strPtno = txtPtno.Text.Trim();
            string strDept = txtDept.Text.Trim();
            string strDate = txtDate.Text.Trim();


            if (txtPtno.Text == "") { ComFunc.MsgBox("등록번호를 입력 바랍니다."); txtPtno.Focus(); return; }
            if (txtDate.Text == "") { ComFunc.MsgBox("등록일자를 입력 바랍니다."); return; }
            if (TxtUseYn.Text == "") { ComFunc.MsgBox("사용유무가 판단안됩니다."); return; }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                if (FstrRowID == "" || TxtUseYn.Text != "Y")
                {
                    ComFunc.MsgBox("사용쿠폰이 잘못지정되었습니다.!!");
                    return;
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE HEA_JEPSU_COUPON ";
                    SQL += ComNum.VBLF + "    SET USETIME   = TO_DATE('" + strDate + "','YYYY-MM-DD') , ";
                    SQL += ComNum.VBLF + "        USEDEPT   = '" + strDept + "' ";
                    SQL += ComNum.VBLF + "  WHERE ROWID     = '" + FstrRowID + "' ";
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
                    clsPublic.GstrHelpCode = "OK";

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            this.Close();
        }

        private void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (Char)13)
            {
                if (txtPtno.Text.Trim() != "")
                {
                    txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));
                    Search_Process(clsDB.DbCon);
                }
            }
        }

        private void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPtno)
            {
                if (txtPtno.Text.Trim() == "") { return; }
                txtPtno.Text =  string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));
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

            frmPmpaHeaCoupon frm = new frmPmpaHeaCoupon();
            ComFunc.Form_Center(frm);

            ComFunc.SetAllControlClear(pnlLeftTop);
            ComFunc.SetAllControlClear(pnlLeft);
            ComFunc.SetAllControlClear(pnlRightTop);
            ComFunc.SetAllControlClear(pnlRight);

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFdate.Text = VB.DateAdd("D",-365, clsPublic.GstrSysDate).ToString("yyyy-MM-dd");
            dtpTdate.Text = clsPublic.GstrSysDate;
            txtDate.Text = clsPublic.GstrSysDate;

            txtPtno.Text = FstrPtno;
            txtDept.Text = FstrDept;

            Search_Process(clsDB.DbCon);
            txtPtno.Select();
        }

        void eForm_Clear()
        {
            txtDate.Text = clsPublic.GstrSysDate;
            txtPtno.Text = "";
            txtSname.Text = "";
            txtDept.Text = "";
            TxtUseYn.Text = "";
            FstrRowID = "";
        }


        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            TxtUseYn.Text = "N";
            FstrRowID = ssList_Sheet1.Cells[e.Row, 5].Text.Trim();

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.WRTNO, A.PANO, A.USEDEPT, ";
            SQL += ComNum.VBLF + "        A.ROWID, TO_CHAR(A.SDATE,'YYYY-MM-DD') SDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.USETIME,'YYYY-MM-DD') USETIME ";
            SQL += ComNum.VBLF + "   FROM HEA_JEPSU_COUPON A";
            SQL += ComNum.VBLF + "  WHERE PTNO  = '" + txtPtno.Text.Trim() + "'  ";
            SQL += ComNum.VBLF + "    AND ROWID = '" + FstrRowID + "' ";
            SqlErr = clsDB.GetDataTableEx(ref Dt1, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt1.Rows.Count > 0)
            {
                if (Dt1.Rows[0]["USETIME"].ToString().Trim() == "")
                    TxtUseYn.Text = "Y";
            }

            Dt1.Dispose();
            Dt1 = null;
        }

    }
}
