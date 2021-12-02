using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 수납관련 예외설정
/// Author : 박병규
/// Create Date : 2017.06.01
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaSetDataDic : Form
    {
        ComQuery CQ = null;
        ComFunc CF = null;
        clsSpread SPR = null;
        DataTable Dt = new DataTable();
        string SQL = "";
        string SqlErr = "";
        int intRowCnt = 0;

        bool blnUpdate_YN = false;

        public frmPmpaSetDataDic()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);
            this.btnSave.Click += new EventHandler(eCtl_Click);

        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
                Save_Process(clsDB.DbCon);
        }

        private void Save_Process(PsmhDb pDbCon)
        {
            string strDel = "";
            string strCode = "";
            string strName = "";
            string strJDate = "";
            string strDelDate = "";
            string strChange = "";
            string strRowid = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            try
            {
                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strCode = ssList_Sheet1.Cells[i, 1].Text.Trim();
                    if (strCode != "")
                    {
                        strDel = ssList_Sheet1.Cells[i, 0].Text.Trim();
                        strName = ssList_Sheet1.Cells[i, 2].Text.Trim();
                        strJDate = ssList_Sheet1.Cells[i, 3].Text.Trim();
                        strDelDate = ssList_Sheet1.Cells[i, 4].Text.Trim();
                        strChange = ssList_Sheet1.Cells[i, 5].Text.Trim();
                        strRowid = ssList_Sheet1.Cells[i, 6].Text.Trim();

                        if (strRowid == "" && strDel != "True")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_BCODE";
                            SQL += ComNum.VBLF + "        (GUBUN, CODE, NAME, JDATE, DELDATE,";
                            SQL += ComNum.VBLF + "         ENTSABUN, ENTDATE )";
                            SQL += ComNum.VBLF + " VALUES ('" + cboGubun.Text.Trim() + "',";
                            SQL += ComNum.VBLF + "         '" + strCode + "',";
                            SQL += ComNum.VBLF + "         '" + strName + "',";
                            SQL += ComNum.VBLF + "         TRUNC(SYSDATE),";
                            SQL += ComNum.VBLF + "         TO_DATE('" + strDel + "'),";
                            SQL += ComNum.VBLF + "         '" + clsPublic.GstrJobSabun + "',";
                            SQL += ComNum.VBLF + "         SYSDATE)";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);
                        }
                        else
                        {
                            if (strDel == "True")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "BAS_BCODE";
                                SQL += ComNum.VBLF + "  WHERE ROWID = '" + strRowid + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);
                            }
                            else if (strChange == "Y")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_BCODE";
                                SQL += ComNum.VBLF + "    SET CODE = '" + strCode + "',";
                                SQL += ComNum.VBLF + "        NAME = '" + strName + "',";
                                SQL += ComNum.VBLF + "        JDATE = TO_DATE('" + strJDate + "', 'YYYY-MM-DD'),";
                                SQL += ComNum.VBLF + "        DELDATE = TO_DATE('" + strDelDate + "', 'YYYY-MM-DD'),";
                                SQL += ComNum.VBLF + "        ENTSABUN = '" + clsPublic.GstrJobSabun + "',";
                                SQL += ComNum.VBLF + "        ENTDATE = SYSDATE ";
                                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                                SQL += ComNum.VBLF + "    AND ROWID = '" + strRowid + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);
                            }
                        }
                    }

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
                blnUpdate_YN = false;

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

            blnUpdate_YN = false;
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            CQ = new ComQuery();
            CF = new ComFunc();
            SPR = new clsSpread();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaSetDataDic frm = new frmPmpaSetDataDic();
            ComFunc.Form_Center(frm);

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);

            if (clsPublic.GstrHelpCode == "1")
            {
                cboGubun.Items.Add("접수등록번호예외");
                txtInfo.Text = "접수시 예외번호 등록(ex:의료급여+계약처 등)";
            }
            else if (clsPublic.GstrHelpCode == "2")
            {
                cboGubun.Items.Add("입원수납_Y96J허용");
                txtInfo.Text = "퇴원수납시 Y96J 허용 등록";
            }
            else if (clsPublic.GstrHelpCode == "3")
            {
                cboGubun.Items.Add("외래_효도감액강제대상");
                txtInfo.Text = "외래_효도감액 강제 대상 등록";
                cboGubun.Items.Add("외래_효도감액강제예외대상");
                txtInfo.Text = "외래_효도감액강제예외대상 등록";
            }
            else if (clsPublic.GstrHelpCode == "4")
            {
                cboGubun.Items.Add("외래_가정간호선택병원강제대상");
                txtInfo.Text = "외래_가정간호 선택병원 강제 대상";
            }
            else if (clsPublic.GstrHelpCode == "5")
            {
                cboGubun.Items.Add("원무강제퇴사자감액");
                txtInfo.Text = "원무 강제 퇴사자 감액(인사등록예외)";
            }

            if (cboGubun.Items.Count > 0)
            {
                cboGubun.SelectedIndex = 0;
            }

            if (clsPublic.GstrHelpCode == "3")
            {
                cboGubun.Enabled = true;
            }
            else
            {
                cboGubun.Enabled = false;
            }

           

            switch (clsPublic.GstrHelpCode)
            {
                case "1":
                case "2":
                case "4":
                    ssList_Sheet1.Columns[1].Label = "등록번호";
                    ssList_Sheet1.Columns[2].Label = "참고사항";
                    break;

                case "3":
                    ssList_Sheet1.Columns[1].Label = "주민번호";
                    ssList_Sheet1.Columns[2].Label = "참고사항";
                    break;

                case "5":
                    ssList_Sheet1.Columns[1].Label = "주민번호";
                    ssList_Sheet1.Columns[2].Label = "성명";
                    break;
            }

            Get_DataLoad();
        }


        private void Get_DataLoad()
        {
            if (cboGubun.Text == "")
            {
                ComFunc.MsgBox("구분란이 공란입니다.", "알림");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlBody);

            Dt = CQ.Get_BasBcode(clsDB.DbCon, cboGubun.Text.Trim(), "");

            ssList_Sheet1.RowCount = Dt.Rows.Count;

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = "";
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["CODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["NAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["JDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DELDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = "";
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }

            ssList_Sheet1.RowCount += 50;

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_DataLoad();
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            clsPublic.GstrMsgList = "";
            clsPublic.GstrMsgList += "저장하지 않은 데이터가 있습니다." + '\r';
            clsPublic.GstrMsgList += "저장하지 않고 닫을경우 현재 작성된 데이터는 삭제됩니다." + '\r' + '\r';
            clsPublic.GstrMsgList += "계속하려면 [YES]을 클릭하고, 현재 화면에 있으려면 [NO]를 클릭하십시오";

            if (blnUpdate_YN == true)
            {
                if (ComFunc.MsgBoxQ (clsPublic.GstrMsgList, "확인요망", MessageBoxDefaultButton.Button1) == DialogResult.No)
                    return;
            }

            this.Close();
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ComFunc.SelectRowColor(ssList_Sheet1, e.Row);

            if (e.Column == 0) { ssList_Sheet1.Cells[e.Row, 5].Text = "Y"; blnUpdate_YN = true;}
        }

        private void ssList_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            ssList_Sheet1.Cells[e.Row, 5].Text = "Y";
            blnUpdate_YN = true;

        }

    }
}
