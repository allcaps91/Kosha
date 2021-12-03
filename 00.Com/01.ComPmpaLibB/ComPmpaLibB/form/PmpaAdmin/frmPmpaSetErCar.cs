using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 응급 이송차량 코드설정
/// Author : 박병규
/// Create Date : 2017.05.30
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaSetErCar : Form
    {
        clsQuery CQ = new clsQuery();
        ComFunc CF = new ComFunc();

        //2017-09-21 안정수, rEventExit 델리게이트 추가

        public delegate void EventExit();
        public event EventExit rEventExit;

        public frmPmpaSetErCar()
        {
            InitializeComponent();
        }

        private void frmPmpaSetErCar_Load(object sender, EventArgs e)
        {
            //2017-09-21 안정수 테스트를 위해 권한 확인부분 주석처리

            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaSetErCar frm = new frmPmpaSetErCar();
            ComFunc.Form_Center(frm);
            Get_DataLoad();

        }

        private void Get_DataLoad()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComFunc.SetAllControlClear(pnlBody);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT CODE, NAME, TO_CHAR(ENTDATE,'YYYY-MM-DD') ENTDATE,";
                SQL += ComNum.VBLF + "        TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE, ROWID,NCODE";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.ETC_ER_CAR";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "  ORDER BY CODE ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

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

                ssList_Sheet1.RowCount = Dt.Rows.Count + 10;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = "";
                    ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["CODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["NAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["ENTDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DELDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = "";
                    ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["NCODE"].ToString().Trim();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            //2017-09-21 안정수 rEventExit 추가
            //this.Close();
            rEventExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
        }

        private void ssList_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            ssList_Sheet1.Cells[e.Row, 5].Text = "Y";
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 0) { ssList_Sheet1.Cells[e.Row, 5].Text = "Y"; }
        }

        private void ssList_EditModeOff(object sender, EventArgs e)
        {
            String strCode = "";
            String strChkCode = "";

            if (ssList_Sheet1.ActiveColumnIndex != 1) return;

            strCode = ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim().ToUpper();
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
                            ComFunc.MsgBox(i + 1 + "번째줄의 코드와 중복입니다.", "경고");
                            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 0].Text = "";
                            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text = "";
                            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 2].Text = "";
                            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 3].Text = "";
                            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 4].Text = "";
                            ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 5].Text = "";

                            ssList_Sheet1.SetActiveCell(ssList_Sheet1.ActiveRowIndex, 1);
                            ssList.Focus();
                            return;
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";     //에러문 받는 변수
            int intRowCnt = 0;      //변경된 Row받는 변수
            string strDel = "";
            string strCode = "";
            string strNCode = "";
            string strName = "";
            string strEntDate = "";
            string strDelDate = "";
            string strChange = "";
            string strRowid = "";

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) { return; }     //권한확인

            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                strDel = ssList_Sheet1.Cells[i, 0].Text.Trim();
                strCode = ssList_Sheet1.Cells[i, 1].Text.Trim();
                strName = ssList_Sheet1.Cells[i, 2].Text.Trim();
                strEntDate = ssList_Sheet1.Cells[i, 3].Text.Trim();
                strDelDate = ssList_Sheet1.Cells[i, 4].Text.Trim();
                strNCode = ssList_Sheet1.Cells[i, 7].Text.Trim();

                if (strDel == "" && strCode != "")
                {
                    if (strName == "")
                    {
                        ComFunc.MsgBox("구분란이 공란입니다..", "확인");
                        return;
                    }
                }
            }

            Cursor.Current = Cursors.WaitCursor;
             
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strCode = ssList_Sheet1.Cells[i, 1].Text.Trim();
                    if (strCode != "")
                    {
                        strDel = ssList_Sheet1.Cells[i, 0].Text.Trim();
                        strName = ssList_Sheet1.Cells[i, 2].Text.Trim();
                        strEntDate = ssList_Sheet1.Cells[i, 3].Text.Trim();
                        strDelDate = ssList_Sheet1.Cells[i, 4].Text.Trim();
                        strChange = ssList_Sheet1.Cells[i, 5].Text.Trim();
                        strRowid = ssList_Sheet1.Cells[i, 6].Text.Trim();
                        strNCode = ssList_Sheet1.Cells[i, 7].Text.Trim();
                        if (strNCode == "" )
                        {
                            strNCode = strCode; 
                        }
                        if (strRowid == "" && strDel == "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.ETC_ER_CAR";
                            SQL += ComNum.VBLF + "        (CODE, NAME,NCODE, ENTDATE)";
                            SQL += ComNum.VBLF + " VALUES ('" + strCode + "',";
                            SQL += ComNum.VBLF + "         '" + strName + "',";
                            SQL += ComNum.VBLF + "         '" + strNCode + "',";
                            SQL += ComNum.VBLF + "         TRUNC(SYSDATE))";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
                        }
                        else
                        {
                            if (strDel != "")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " UPDATE KOSMOS_PMPA.ETC_ER_CAR";
                                SQL += ComNum.VBLF + "    SET DELDATE = TRUNC(SYSDATE)";
                                SQL += ComNum.VBLF + "  WHERE ROWID = '" + strRowid + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
                            }
                            else if (strChange == "Y")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + " UPDATE KOSMOS_PMPA.ETC_ER_CAR";
                                SQL += ComNum.VBLF + "    SET Code = '" + strCode + "',";
                                SQL += ComNum.VBLF + "        NCODE = '" + strNCode + "',";
                                SQL += ComNum.VBLF + "        Name = '" + strName + "'";
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
                ComFunc.MsgBox("저장되었습니다.", "알림");
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_DataLoad();
        }
    }
}
