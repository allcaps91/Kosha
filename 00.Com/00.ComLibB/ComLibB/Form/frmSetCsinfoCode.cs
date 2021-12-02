using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 고객정보코드설정
/// Author : 박병규
/// Create Date : 2017.05.29
/// </summary>
/// <history>
/// </history>

namespace ComLibB
{
    public partial class frmSetCsinfoCode : Form
    {
        ComQuery CQ = new ComQuery();
        ComFunc CF = new ComFunc();

        string strGubun = "";

        public frmSetCsinfoCode()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_DataLoad();
        }

        private void Get_DataLoad()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            if (cboGubun.Text == "선택")
            {
                MessageBox.Show("구분코드를 선택하시기 바랍니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strGubun = VB.Left(cboGubun.Text, 1);

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComFunc.SetAllControlClear(pnlBody);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT CODE, NAME, SORT, ROWID";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.ETC_CSINFO_CODE";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND GUBUN = '" + strGubun + "' ";
                SQL += ComNum.VBLF + "  ORDER BY CODE";
                SqlErr  = clsDB.GetDataTable(ref Dt,  SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
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

                ssList_Sheet1.RowCount = Dt.Rows.Count + 50;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = "";
                    ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["CODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["NAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["SORT"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = "";
                    ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                }
                Dt.Dispose();
                Dt = null;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void frmSetCsinfoCode_Load(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();
            string strCode = "";
            string strCodeName = "";

            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등


            frmSetCsinfoCode frm = new frmSetCsinfoCode();
            ComFunc.Form_Center(frm);

            cboGubun.Items.Clear();
            cboGubun.Items.Add("선택");

            //구분값 하드코딩 부분 DB화
            Dt = CQ.Get_CsinfoCode(clsDB.DbCon, "A", "");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                strCode = Dt.Rows[i]["CODE"].ToString().Trim();
                strCodeName = Dt.Rows[i]["NAME"].ToString().Trim();
                cboGubun.Items.Add(strCode + ".   " + strCodeName);
            }
            Dt.Dispose();
            Dt = null;

            cboGubun.SelectedIndex = 0;

            ComFunc.SetAllControlClear(pnlBody);
        }

        private void cboGubun_Click(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboGubun_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) SendKeys.Send("{TAB}");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;      //변경된 Row받는 변수
            string strDel = "";
            string strCode = "";
            string strName = "";
            int intSort = 0;
            string strChange = "";
            string strRowid = "";

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) { return; }     //권한확인

            if (strGubun == "")
            {
                MessageBox.Show("선택된 구분코드의 데이터 조회후 저장하시기 바랍니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                strDel = ssList_Sheet1.Cells[i, 0].Text.Trim();
                strCode = ssList_Sheet1.Cells[i, 1].Text.Trim();
                strName = ssList_Sheet1.Cells[i, 2].Text.Trim();
                intSort = Convert.ToInt16(VB.Val( ssList_Sheet1.Cells[i, 3].Text));

                if (strDel == "")
                {
                    if (strCode != "" && strName == "")
                    {
                        MessageBox.Show("코드명칭이 공란입니다..", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);
            

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strCode = ssList_Sheet1.Cells[i, 1].Text.Trim();

                    if (strCode != "")
                    {
                        strDel = ssList_Sheet1.Cells[i, 0].Text.Trim();
                        strName = ssList_Sheet1.Cells[i, 2].Text.Trim();
                        intSort = Convert.ToInt16(VB.Val( ssList_Sheet1.Cells[i, 3].Text.Trim()));
                        strChange = ssList_Sheet1.Cells[i, 4].Text.Trim();
                        strRowid = ssList_Sheet1.Cells[i, 5].Text.Trim();

                        if (strRowid == "" && strDel == "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_CSINFO_CODE";
                            SQL += ComNum.VBLF + "        (GUBUN, CODE, NAME, SORT, ENTSABUN, ENTDATE)";
                            SQL += ComNum.VBLF + " VALUES ('" + strGubun + "',";
                            SQL += ComNum.VBLF + "         '" + strCode + "',";
                            SQL += ComNum.VBLF + "         '" + strName + "',";
                            SQL += ComNum.VBLF + "         " + intSort + ",";
                            SQL += ComNum.VBLF + "         '" + clsPublic.GstrJobSabun + "',";
                            SQL += ComNum.VBLF + "         SYSDATE)";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
                        }
                        else
                        {
                            if (strDel != "")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "ETC_CSINFO_CODE ";
                                SQL += ComNum.VBLF + "  WHERE ROWID = '" + strRowid + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
                            }
                            else if (strChange == "Y")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_CSINFO_CODE";
                                SQL += ComNum.VBLF + "    SET CODE = '" + strCode + "',";
                                SQL += ComNum.VBLF + "        NAME = '" + strName + "',";
                                SQL += ComNum.VBLF + "        SORT = " + intSort + ",";
                                SQL += ComNum.VBLF + "        EntSabun = '" + clsPublic.GstrJobSabun + "',";
                                SQL += ComNum.VBLF + "        EntDate = SYSDATE ";
                                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                                SQL += ComNum.VBLF + "    AND ROWID = '" + strRowid + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
                            }
                        }
                    }
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장되었습니다");
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
            cboGubun.Focus();

        }

        private void ssList_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            ssList_Sheet1.Cells[e.Row, 4].Text = "Y";
        }

        private void ssList_EditModeOff(object sender, EventArgs e)
        {
            String strCode = "";
            String strChkCode = "";

            if (ssList_Sheet1.ActiveColumnIndex != 1) return;

            strCode = ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim();
            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text = strCode;

            if (strCode == "")
                ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 2].Text = "";
            else
            {
                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strChkCode = ssList_Sheet1.Cells[i, 1].Text.Trim();

                    if (strChkCode == strCode)
                    {
                        if (i != ssList_Sheet1.ActiveRowIndex && strChkCode != "")
                        {
                            MessageBox.Show(i + 1 + "번째줄의 코드와 중복입니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 0].Text = "";
                            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text = "";
                            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 2].Text = "";
                            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 3].Text = "";
                            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 4].Text = "";

                            ssList_Sheet1.SetActiveCell(ssList_Sheet1.ActiveRowIndex, 1);
                            ssList.Focus();
                            return;
                        }
                    }
                }

                if (strGubun == "5")
                {
                    ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 2].Text = CF.Read_SabunName(clsDB.DbCon, strCode);
                }
            }
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 0) { ssList_Sheet1.Cells[e.Row, 4].Text = "Y"; }
        }
    }
}
