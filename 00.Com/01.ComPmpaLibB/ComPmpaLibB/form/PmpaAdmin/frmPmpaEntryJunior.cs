using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 신생아 등록번호를 엄마 등록번호로 매칭후 연말정산 자료로 Update
/// Author : 박병규
/// Create Date : 2017.06.20
/// </summary>
/// <history>
/// 2017.06.20 : ETC_JUNSLIP으로 Update하는 변환버튼을 삭제 > 저장버튼으로 사용자UI 단순화시킴
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaEntryJunior : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        clsPmpaFunc CPF = new clsPmpaFunc();

        int intFnRow = 0;

        public frmPmpaEntryJunior()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            //KeyPress 이벤트
            this.dtpFDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            //ValueChanged 이벤트
            this.dtpFDate.ValueChanged += new EventHandler(eControl_ValueChanged);
            this.dtpTDate.ValueChanged += new EventHandler(eControl_ValueChanged);
        }

        private void eControl_ValueChanged(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
        }

        private void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpFDate && e.KeyChar == (Char)13) { dtpTDate.Focus(); }
            if (sender == this.dtpTDate && e.KeyChar == (Char)13) { btnSearch.Focus(); }
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaEntryJunior frm = new frmPmpaEntryJunior();
            ComFunc.Form_Center(frm);

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);

            CF.SetComboBoxYear(clsDB.DbCon, cboYear, 5, "2");

            //CPF.Read_SysDate();

            dtpFDate.Text =  VB.Left(cboYear.Text, 4) + "-01-01";
            dtpTDate.Text = VB.Left(cboYear.Text, 4) + "-12-31";
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtpFDate.Text = VB.Left(cboYear.Text, 4) + "-01-01";
            dtpTDate.Text = VB.Left(cboYear.Text, 4) + "-12-31";

            ComFunc.SetAllControlClear(pnlBody);
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
            ComFunc.SetAllControlClear(pnlBody);

            string strYear = VB.Left(cboYear.Text,4);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, JUMIN, SNAME,JUMIN_new ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_JUNSLIP ";
            SQL += ComNum.VBLF + "  WHERE YEAR = '" + strYear + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND ACTDATE <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND baby ='Y'  ";
           // SQL += ComNum.VBLF + "    AND (JUMIN LIKE '%0000000' OR ";
           // SQL += ComNum.VBLF + "         JUMIN LIKE '%000000' OR ";
           // SQL += ComNum.VBLF + "         JUMIN LIKE '%00000' OR ";
           // SQL += ComNum.VBLF + "         JUMIN LIKE '%000001' OR ";
           // SQL += ComNum.VBLF + "         JUMIN LIKE '%000002' OR ";
           // SQL += ComNum.VBLF + "         JUMIN LIKE '%000003' OR ";
           // SQL += ComNum.VBLF + "         JUMIN LIKE '%000004') ";
            SQL += ComNum.VBLF + "    AND PANO  NOT IN ( SELECT PANO ";
            SQL += ComNum.VBLF + "                          FROM " + ComNum.DB_PMPA + "ETC_JUNREMARK ";
            SQL += ComNum.VBLF + "                         WHERE YEAR = '" + strYear + "' ) ";
            SQL += ComNum.VBLF + "  GROUP BY PANO, JUMIN,  SNAME,JUMIN_new ";
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

            string strPtno = string.Empty;
            string strSname = string.Empty;
            string strJumin = string.Empty;
            
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                strPtno =  Dt.Rows[i]["PANO"].ToString().Trim();
                strSname  = Dt.Rows[i]["SNAME"].ToString().Trim();
                strJumin = clsAES.DeAES(Dt.Rows[i]["JUMIN_new"].ToString().Trim());

                Get_JunMatch(i, strPtno, strJumin, strYear);

                ssList_Sheet1.Cells[i, 0].Text = strPtno;
                ssList_Sheet1.Cells[i, 1].Text = strSname;
                ssList_Sheet1.Cells[i, 2].Text = VB.Left(strJumin,6);
                ssList_Sheet1.Cells[i, 3].Text = VB.Right(strJumin,7);
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

            intFnRow = 0;
        }

        private void Get_JunMatch(int intRow, string strPtno, string strJumin, string strYear)
        {
            DataTable DtJun = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.PANO2, A.JUMIN2, A.JUMIN2_NEW, B.SNAME, B.GKIHO, A.ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_JUNMATCH A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT B ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND A.PANO        ='" + strPtno + "' ";
            SQL += ComNum.VBLF + "    AND A.YEAR        ='" + strYear + "'";
            SQL += ComNum.VBLF + "    AND A.JUMIN_NEW   = '" + clsAES.AES(strJumin) + "' ";
            SQL += ComNum.VBLF + "    AND A.PANO2       = B.PANO(+) ";
            SqlErr = clsDB.GetDataTable(ref DtJun, SQL, clsDB.DbCon);

            if (DtJun.Rows.Count != 0)
            {
                ssList_Sheet1.Cells[intRow, 4].Text = DtJun.Rows[0]["GKIHO"].ToString().Trim();
                ssList_Sheet1.Cells[intRow, 5].Text = DtJun.Rows[0]["PANO2"].ToString().Trim();
                ssList_Sheet1.Cells[intRow, 6].Text = DtJun.Rows[0]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[intRow, 7].Text = VB.Left(clsAES.DeAES(DtJun.Rows[0]["JUMIN2_NEW"].ToString().Trim()),6);
                ssList_Sheet1.Cells[intRow, 8].Text = VB.Right(clsAES.DeAES(DtJun.Rows[0]["JUMIN2_NEW"].ToString().Trim()), 7);
                ssList_Sheet1.Cells[intRow, 10].Text = DtJun.Rows[0]["ROWID"].ToString().Trim();
            }

            DtJun.Dispose();
            DtJun = null;

        }

        private void ssList_EditModeOff(object sender, EventArgs e)
        {
            String strCode = string.Empty;

            if (ssList_Sheet1.ActiveColumnIndex != 0 || ssList_Sheet1.ActiveColumnIndex != 5) return;
            
            if (ssList_Sheet1.ActiveColumnIndex == 0)
                strCode = VB.Format(ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 0].Text.Trim(), "00000000");
            else if (ssList_Sheet1.ActiveColumnIndex == 5)
                strCode = VB.Format(ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 5].Text.Trim(), "00000000");

            DataTable DtPat =  CPF.Get_BasPatient(clsDB.DbCon, strCode);

            if (ssList_Sheet1.ActiveColumnIndex == 0)
            {
                ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text = DtPat.Rows[0]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 2].Text = DtPat.Rows[0]["JUMIN1"].ToString().Trim();
                ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 3].Text = clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());

            }
            else if (ssList_Sheet1.ActiveColumnIndex == 5)
            {
                ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 6].Text = DtPat.Rows[0]["SNAME"].ToString().Trim() + "의 아기";
                ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 7].Text = DtPat.Rows[0]["JUMIN1"].ToString().Trim();
                ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 8].Text = clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
            }

            DtPat.Dispose();
            DtPat = null;
        }

        private void ssList_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            DataTable Dt = new DataTable();
            DataTable DtPat = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strPtno = string.Empty;
            string strFdate = string.Empty;
            string strTdate = string.Empty;
            string strJumin1 = string.Empty;
            string strJumin2 = string.Empty;

            intFnRow = 0;

            if ( e.Column == 9)
            {
                strPtno = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
                strFdate = Convert.ToString( VB.Val(VB.Left(cboYear.Text,4))-1) + "-12-01";
                strTdate = Convert.ToString(VB.Left(cboYear.Text, 4) + "-11-30");

                CS.Spread_Clear(ssFind, 0, 0);

                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT JUPBONO ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "  WHERE PANO  = '" + strPtno + "' ";
                SQL += ComNum.VBLF + "    AND INDATE >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND INDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND JUPBONO IS NOT NULL ";
                SQL += ComNum.VBLF + "  GROUP BY JUPBONO ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (Dt.Rows.Count == 1)
                {
                    string strCode = VB.Format(Dt.Rows[0]["JUPBONO"].ToString().Trim(), "00000000");
                    DtPat = CPF.Get_BasPatient(clsDB.DbCon, strCode);

                    if (DtPat.Rows.Count != 0)
                    {
                        ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 5].Text = strCode;
                        ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 6].Text = DtPat.Rows[0]["SNAME"].ToString().Trim() + "의 아기";
                        ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 7].Text = DtPat.Rows[0]["JUMIN1"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 8].Text = clsAES.DeAES(DtPat.Rows[0]["JUMIN3"].ToString().Trim());
                    }

                    DtPat.Dispose();
                    DtPat = null;
                }
                else if (Dt.Rows.Count >= 2)
                {
                    ComFunc.MsgBox("엄마 등록번호가 두개 이상입니다.", "확인");
                }

                Dt.Dispose();
                Dt = null;

                DtPat = CPF.Get_BasPatient(clsDB.DbCon, strPtno);

                if (DtPat.Rows.Count != 0)
                {
                    string strTemp = DtPat.Rows[0]["REMARK"].ToString().Trim();
                    ssFind_Sheet1.Cells[0, 0].Text = strTemp;
                    ssFind_Sheet1.Cells[1, 0].Text = DtPat.Rows[0]["SNAME"].ToString().Trim();

                    if (strTemp != ""  && VB.I(strTemp,":") > 1 && VB.I(strTemp, "-") > 1)
                    {
                        intFnRow = e.Row;

                        strJumin1 = VB.Pstr(VB.Pstr(strTemp, ":", 2), "-", 1).Trim();
                        strJumin2 = VB.Pstr(VB.Pstr(strTemp, ":", 2), "-", 2).Trim();

                        ssFind_Sheet1.Cells[3, 0].Text = strJumin1;
                        ssFind_Sheet1.Cells[4, 0].Text = strJumin2;

                        if (VB.Len(strJumin2) <= 7)
                        {
                            strJumin2 = clsAES.AES(strJumin2);
                        }
                    }
                }
                DtPat.Dispose();
                DtPat = null;

                DtPat = CPF.Get_Jumin_BasPatient(clsDB.DbCon, strJumin1, strJumin2);

                if (DtPat.Rows.Count != 0)
                {
                    ssFind_Sheet1.Cells[2, 0].Text = DtPat.Rows[0]["PANO"].ToString().Trim();
                }
                DtPat.Dispose();
                DtPat = null;

                Cursor.Current = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            intFnRow = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
             
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strYear = string.Empty;
                string strPtno = string.Empty;
                string strJumin = string.Empty;
                string strPtno2 = string.Empty;
                string strSname2 = string.Empty;
                string strJumin2 = string.Empty;
                string strRowid = string.Empty;

                strYear = VB.Left(cboYear.Text, 4);

                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strPtno = ssList_Sheet1.Cells[i, 0].Text.Trim();
                    strJumin = ssList_Sheet1.Cells[i, 2].Text.Trim() + ssList_Sheet1.Cells[i, 3].Text.Trim();
                    strPtno2 = ssList_Sheet1.Cells[i, 5].Text.Trim();
                    strSname2 = ssList_Sheet1.Cells[i, 6].Text.Trim();
                    strJumin2 = ssList_Sheet1.Cells[i, 7].Text.Trim() + ssList_Sheet1.Cells[i, 8].Text.Trim();
                    strRowid = ssList_Sheet1.Cells[i, 10].Text.Trim();

                    if (strPtno2 != "")
                    {
                        if (strRowid == "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_JUNMATCH ";
                            SQL += ComNum.VBLF + "       (PANO, JUMIN, PANO2, JUMIN2, YEAR, JUMIN_New, JUMIN2_New) ";
                            SQL += ComNum.VBLF + " VALUES('" + strPtno + "', ";
                            SQL += ComNum.VBLF + "        '" + VB.Left(strJumin, 7) + "******', ";
                            SQL += ComNum.VBLF + "        '" + strPtno2 + "',  ";
                            SQL += ComNum.VBLF + "        '" + VB.Left(strJumin2, 1) + "******', ";
                            SQL += ComNum.VBLF + "        '" + strYear + "', ";
                            SQL += ComNum.VBLF + "        '" + clsAES.AES(strJumin) + "', ";
                            SQL += ComNum.VBLF + "        '" + clsAES.AES(strJumin2) + "' ) ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        else
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_JUNMATCH ";
                            SQL += ComNum.VBLF + "    SET PANO2         = '" + strPtno2 + "', ";
                            SQL += ComNum.VBLF + "        JUMIN2        = '" + VB.Left(strJumin2, 1) + "******', ";
                            SQL += ComNum.VBLF + "        JUMIN2_New    = '" + clsAES.AES(strJumin2) + "' ";
                            SQL += ComNum.VBLF + "  WHERE ROWID         = '" + strRowid + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }

                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_JUNSLIP ";
                        SQL += ComNum.VBLF + "    SET PANO      = '" + strPtno2 + "', ";
                        SQL += ComNum.VBLF + "        SNAME     = '" + strSname2 + "', ";
                        SQL += ComNum.VBLF + "        JUMIN     = '" + VB.Left(strJumin2, 1) + "******', ";
                        SQL += ComNum.VBLF + "        JUMIN_new = '" + clsAES.AES(strJumin2) + "', ";
                        SQL += ComNum.VBLF + "        BPANO     = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "  WHERE YEAR      = '" + strYear + "' ";
                        SQL += ComNum.VBLF + "    AND PANO      = '" + strPtno + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("자료가 저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

                Get_DataLoad();
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            string strPtno = string.Empty;
            string strSname = string.Empty;
            string strJumin1 = string.Empty;
            string strJumin2 = string.Empty;

            strPtno = ssFind_Sheet1.Cells[2, 0].Text.Trim();
            strSname = ssFind_Sheet1.Cells[1, 0].Text.Trim();
            strJumin1 = ssFind_Sheet1.Cells[3, 0].Text.Trim();
            strJumin2 = ssFind_Sheet1.Cells[4, 0].Text.Trim();

            if (VB.Len(strJumin1) == 6 && VB.Len(strJumin2) == 7 && strPtno != "" && intFnRow > 0)
            {
                ssList_Sheet1.Cells[intFnRow, 5].Text = strPtno;
                ssList_Sheet1.Cells[intFnRow, 6].Text = strSname;
                ssList_Sheet1.Cells[intFnRow, 7].Text = strJumin1;
                ssList_Sheet1.Cells[intFnRow, 8].Text = strJumin2;
            }

        }
    }
}
