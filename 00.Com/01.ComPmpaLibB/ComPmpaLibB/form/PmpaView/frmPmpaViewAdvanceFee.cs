using ComLibB;
using System.Windows.Forms;
using System;
using System.Data;
using System.Drawing;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 예약검사 선수금 명단조회
/// Author : 박병규
/// Create Date : 2017.07.24
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaViewAdvanceFee : Form
    {
        clsUser CU = null;
        ComQuery CQ = null;
        clsSpread CS = null;
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        DataTable Dt = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;


        public delegate void CHOICE_PTNO(string cValue);

        public event CHOICE_PTNO CHOICE_Event;


        public frmPmpaViewAdvanceFee()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eForm_Load);

            this.txtPtno.LostFocus += new EventHandler(eCtl_LostFocus);
            this.dtpFdate.ValueChanged += new EventHandler(eForm_Clear);
            this.dtpTdate.ValueChanged += new EventHandler(eForm_Clear);

            this.dtpFdate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.dtpTdate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            this.btnSave.Click += new EventHandler(eCtl_Click);
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
        }

        private void eCtl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPtno)
                txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));
        }

        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpFdate && e.KeyChar == (Char)13) { dtpTdate.Focus(); }
            if (sender == this.dtpTdate && e.KeyChar == (Char)13) { btnSave.Focus(); }
            if (sender == this.txtPtno && e.KeyChar == (Char)13) { btnSave.Focus(); }
        }

        private void eForm_Clear(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
        }


        private void eForm_Load(object sender, EventArgs e)
        {
            CU = new clsUser();
            CQ = new ComQuery();
            CS = new clsSpread();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);

            dtpFdate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpTdate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            btnSave.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_DataLoad();
        }

        private void Get_DataLoad()
        {

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlBody);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Pano, DeptCode, SName,                                        --등록번호,진료과,성명";
            SQL += ComNum.VBLF + "        TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,                        --회계일자";
            SQL += ComNum.VBLF + "        Amt6, GbEnd, Remark,                                          --영수금액,종료여부,참고사항";
            SQL += ComNum.VBLF + "        TO_CHAR(TransDate,'YYYY-MM-DD') TransDate, TransAmt,          --대체일자, 대체금액";
            SQL += ComNum.VBLF + "        TO_CHAR(Date3,'YYYY-MM-DD') Date3,                            --예약일자시간(최종)";
            SQL += ComNum.VBLF + "        ROWID                                                         --";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM                       --예약검사테이블";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            if (chkNo.Checked == true)
            {
                SQL += ComNum.VBLF + "    AND (gbend is null or gbend = 'N') ";
                SQL += ComNum.VBLF + "    AND (date1 is null or date1 = '' )                                --완료일자/취소일자";
                SQL += ComNum.VBLF + "    AND (date3 is null or  date3 <= to_date('" + clsPublic.GstrSysDate + "','yyyy-mm-dd') ) ";
                SQL += ComNum.VBLF + "    AND actdate < to_date('" + clsPublic.GstrSysDate + "','yyyy-mm-dd') ";
            }
            else
            {

                if (rdoGb0.Checked == true)
                {
                    if (txtPtno.Text.Trim() != "")
                        SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text.Trim() + "' ";
                    else
                    {
                        SQL += ComNum.VBLF + "    AND ActDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND ActDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                    }
                }
                else if (rdoGb1.Checked == true)
                {
                    if (txtPtno.Text.Trim() != "")
                        SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text.Trim() + "' ";
                    else
                    {
                        SQL += ComNum.VBLF + "    AND TRUNC(TransDATE) >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND TRUNC(TransDATE) <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                    }
                }
                else if (rdoGb2.Checked == true)
                    if (txtPtno.Text.Trim() != "")
                        SQL += ComNum.VBLF + "    AND Pano = '" + txtPtno.Text.Trim() + "' ";

                    else
                    {
                        SQL += ComNum.VBLF + "    AND TRUNC(DATE3) >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND TRUNC(DATE3) <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                    }
            }

            SQL += ComNum.VBLF + "  ORDER BY ACTDATE, PANO, ENTDATE ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
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

                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["Pano"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SName"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["ActDate"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = string.Format("{0:#,##0}", Dt.Rows[i]["Amt6"].ToString().Trim());
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["GbEnd"].ToString().Trim();

                if (Dt.Rows[i]["GbEnd"].ToString().Trim() == "Y")
                    ssList.ActiveSheet.Rows[i].BackColor = Color.FromArgb(128, 255, 128);
                else
                    ssList.ActiveSheet.Rows[i].BackColor = Color.FromArgb(255, 255, 255);

                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["TransDate"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = string.Format("{0:#,##0}", Dt.Rows[i]["TransAmt"].ToString().Trim());
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["Date3"].ToString().Trim();
                ssList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["Remark"].ToString().Trim();
                ssList_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                ssList_Sheet1.Cells[i, 11].Text = "";
            }
            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssList_Change(object sender, ChangeEventArgs e)
        {
            ssList_Sheet1.Cells[e.Row, 11].Text = "Y";
        }

        private void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strPtno = string.Empty;

            strPtno = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();

            CHOICE_Event(strPtno);

            this.Close();
        }


        private void Save_Process(PsmhDb pDbCon)
        {
            string strRemark = string.Empty;
            string strRowID = string.Empty;

            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                if (ssList_Sheet1.Cells[i, 11].Text.Trim() == "Y")
                {
                    strRemark = ssList_Sheet1.Cells[i, 9].Text.Trim();
                    strRowID = ssList_Sheet1.Cells[i, 10].Text.Trim();

                    if (strRowID != "")
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        clsDB.setBeginTran(pDbCon);

                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM_BACKUP  ";
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
                            SQL += ComNum.VBLF + "         JIN,AMT1,AMT2, ";
                            SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                            SQL += ComNum.VBLF + "         AMT6, PART, BOHUN, ";
                            SQL += ComNum.VBLF + "         GELCODE, TRANSDATE, TRANSAMT, ";
                            SQL += ComNum.VBLF + "         TRANSPART, RETDATE, RETAMT, ";
                            SQL += ComNum.VBLF + "         RETPART, MCode, VCode, ";
                            SQL += ComNum.VBLF + "         Remark || ' 내역수정', SMSBUILD, WRTNO, ";
                            SQL += ComNum.VBLF + "         GBEND, MANUAL, Gubun, ";
                            SQL += ComNum.VBLF + "         SYSDATE, DATE1, DATE2, ";
                            SQL += ComNum.VBLF + "         DATE3 ";
                            SQL += ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                            SQL += ComNum.VBLF + "   WHERE 1     = 1 ";
                            SQL += ComNum.VBLF + "     AND ROWID = '" + strRowID + "' ) ";
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
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_EXAM ";
                            SQL += ComNum.VBLF + "    SET REMARK = '" + strRemark + "' ";
                            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                            SQL += ComNum.VBLF + "    AND ROWID = '" + strRowID + "' ";
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
                    }
                }
            }
        }
    }
}
