using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    public partial class frmFix : Form
    {

        SheetView[] SheetRoom = new SheetView[10];
        ContextMenu PopupMenu = null;

        int mintRow = 0;
        int mintsIdx = 0;

        public frmFix()
        {
            InitializeComponent();
        }

        private void frmFix_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


            SetSheet();

            SetPopupMenu();

            ssCopy.Visible = false;
            lblPtData.Text = "";

            dtpOpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(1);

            //2019-10-01 전종윤 팀장 요청사항 (31320 삭제, 15344 추가)
            //if (clsType.User.Sabun == "08662" || clsType.User.Sabun == "04349" || clsType.User.Sabun == "19835" || clsType.User.Sabun == "27031" || clsType.User.Sabun == "31320" || clsType.User.Sabun == "48345")
            if (clsType.User.Sabun == "08662" || clsType.User.Sabun == "04349" || clsType.User.Sabun == "19835" || clsType.User.Sabun == "27031" || clsType.User.Sabun == "15344" || clsType.User.Sabun == "48345" || clsType.User.Sabun == "27345")
            {
                btnSave.Visible = true;
                btnSave1.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
                btnSave1.Visible = false;
            }

            ssCopy_Sheet1.RowCount = 0;
        }

        private void SetPopupMenu()
        {
            PopupMenu = new ContextMenu();
            PopupMenu.MenuItems.Add("잘라내기", new System.EventHandler(mnuCut_Click));
            PopupMenu.MenuItems.Add("붙여넣기", new System.EventHandler(mnuPaste_Click));

            ssRoom0.ContextMenu = PopupMenu;
            ssRoom1.ContextMenu = PopupMenu;
            ssRoom2.ContextMenu = PopupMenu;
            ssRoom3.ContextMenu = PopupMenu;
            ssRoom4.ContextMenu = PopupMenu;
            ssRoom5.ContextMenu = PopupMenu;
            ssRoom6.ContextMenu = PopupMenu;
            ssRoom7.ContextMenu = PopupMenu;
            ssRoom8.ContextMenu = PopupMenu;
            ssRoom9.ContextMenu = PopupMenu;
        }

        private void mnuCut_Click(object sender, EventArgs e)
        {
            int intsIdx = mintsIdx;
            int intSRow = 0;
            int intERow = 0;
            int i = 0;
            int h = 0;

            if (SheetRoom[intsIdx].RowCount == 0) return;

            if (SheetRoom[intsIdx].IsBlockSelected == true)
            {
                intSRow = SheetRoom[intsIdx].GetSelection(0).Row;
                intERow = intSRow + SheetRoom[intsIdx].GetSelection(0).RowCount - 1;

                for (i = intSRow; i <= intERow; i++)
                {
                    ssCopy_Sheet1.RowCount = ssCopy_Sheet1.RowCount + 1;

                    for (h = 0; h < ssCopy_Sheet1.ColumnCount; h++)
                    {
                        ssCopy_Sheet1.Cells[ssCopy_Sheet1.RowCount - 1, h].Text = SheetRoom[intsIdx].Cells[i, h].Text.Trim();
                    }
                }

                SheetRoom[intsIdx].RemoveRows(intSRow, SheetRoom[intsIdx].GetSelection(0).RowCount);
            }
            else
            {
                intSRow = mintRow;

                ssCopy_Sheet1.RowCount = ssCopy_Sheet1.RowCount + 1;

                for (h = 0; h < ssCopy_Sheet1.ColumnCount; h++)
                {
                    ssCopy_Sheet1.Cells[ssCopy_Sheet1.RowCount - 1, h].Text = SheetRoom[intsIdx].Cells[intSRow, h].Text.Trim();
                }

                SheetRoom[intsIdx].RemoveRows(intSRow, 1);
            }
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            if (ssCopy_Sheet1.RowCount == 0)
            {
                return;
            }
            int h = 0;
            int i = 0;
            int intsIdx = mintsIdx;
            int intRow = mintRow;

            if (intRow != SheetRoom[intsIdx].RowCount)
            {
                if (intRow == 0) intRow = 1;
                SheetRoom[intsIdx].AddRows(intRow, ssCopy_Sheet1.RowCount);
            }
            else
            {
                SheetRoom[intsIdx].RowCount = SheetRoom[intsIdx].RowCount + ssCopy_Sheet1.RowCount;
            }

            for (i = 0; i < ssCopy_Sheet1.RowCount; i++)
            {
                for (h = 0; h < ssCopy_Sheet1.ColumnCount; h++)
                {
                    SheetRoom[intsIdx].Cells[intRow + i, h].Text = ssCopy_Sheet1.Cells[i, h].Text.Trim();
                }
            }

            ssCopy_Sheet1.RowCount = 0;
        }

        private void SetSheet()
        {
            SheetRoom[0] = ssRoom0_Sheet1;
            SheetRoom[1] = ssRoom1_Sheet1;
            SheetRoom[2] = ssRoom2_Sheet1;
            SheetRoom[3] = ssRoom3_Sheet1;
            SheetRoom[4] = ssRoom4_Sheet1;
            SheetRoom[5] = ssRoom5_Sheet1;
            SheetRoom[6] = ssRoom6_Sheet1;
            SheetRoom[7] = ssRoom7_Sheet1;
            SheetRoom[8] = ssRoom8_Sheet1;
            SheetRoom[9] = ssRoom9_Sheet1;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strPtNo = "";
            int i = 0;
            int h = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ssCopy_Sheet1.RowCount != 0)
            {
                ComFunc.MsgBox("아직 붙쳐 넣지 않은 환자가 있습니다." + ComNum.VBLF + " 확인 후 진행 하십시오.");
                return;
            }

            if (ComFunc.MsgBoxQ("마감을 취소 하시겠습니까? ", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
               

                for (i = 0; i < SheetRoom.Length; i++)
                {
                    strPtNo = "";

                    for (h = 0; h < SheetRoom[i].RowCount; h++)
                    {
                        strPtNo = SheetRoom[i].Cells[h, 4].Text.Trim();

                        SQL = "";
                        SQL = "UPDATE KOSMOS_OCS.OCS_OPSCHE SET ";
                        SQL = SQL + ComNum.VBLF + "GBMAGAM = '' ";
                        SQL = SQL + ComNum.VBLF + "WHERE OPDATE = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND   PANO   = '" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND (  GBDEL <> '*' OR GBDEL IS NULL ) ";
                        SQL = SQL + ComNum.VBLF + "   AND ( GBANGIO IS NULL OR GBANGIO='N') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                btnSearch_Click(null, null);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strPtNo = "";
            int i = 0;
            int h = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (ssCopy_Sheet1.RowCount != 0)
            {
                ComFunc.MsgBox("아직 붙쳐 넣지 않은 환자가 있습니다." + ComNum.VBLF + " 확인 후 진행 하십시오.");
                return;
            }


            if (ComFunc.MsgBoxQ("변경된 자료를 등록하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < SheetRoom.Length; i++)
                {
                    strPtNo = "";

                    for (h = 0; h < SheetRoom[i].RowCount; h++)
                    {
                        strPtNo = SheetRoom[i].Cells[h, 4].Text.Trim();

                        SQL = "UPDATE KOSMOS_OCS.OCS_OPSCHE SET ";
                        SQL = SQL + ComNum.VBLF + " OPROOM  = '" + Convert.ToString(i + 1) + "', ";
                        SQL = SQL + ComNum.VBLF + " GBANGIO ='',";
                        SQL = SQL + ComNum.VBLF + " OPSEQ   = '" + Convert.ToString(h + 1) + "', ";
                        SQL = SQL + ComNum.VBLF + " GBMAGAM = '*', ";
                        SQL = SQL + ComNum.VBLF + " MDATE = SYSDATE ";
                        SQL = SQL + ComNum.VBLF + " WHERE OPDATE = TO_DATE('" + dtpOpDate.Value.ToString("yyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND   PANO   = '" + strPtNo + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND (  GBDEL <> '*' OR GBDEL IS NULL ) ";
                        SQL = SQL + ComNum.VBLF + "     AND  ( GBANGIO IS NULL OR GBANGIO='N') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                btnSearch_Click(null, null);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return; //권한 확인
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int n = 0;
            int i = 0;
            long nWRTNO = 0;

            string strOpSeq = "";// *2
            string strDeptCode = "";// *2
            string strSname = "";// *9
            string strOpTime = "";// *8
            string strPtNo = "";// *8
            string strSex = "";// *1
            string strAge = "";// *3
            string strGbMagam = "";// *1
            string strEntDate = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;


            Cursor.Current = Cursors.WaitCursor;

            for (i = 0; i < SheetRoom.Length; i++)
            {
                SheetRoom[i].RowCount = 0;
                SheetRoom[i].SetRowHeight(-1, ComNum.SPDROWHT);
            }

            for (i = 0; i < ssPrint_Sheet1.ColumnCount; i++)
            {
                ssPrint_Sheet1.Cells[1, i].Text = "";
                ssPrint_Sheet1.Cells[3, i].Text = "";
            }

            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT O.*, TO_CHAR(O.ENTDATE,'YYYY-MM-DD HH24:MI') ENT1 FROM KOSMOS_OCS.OCS_OPSCHE O ";
                SQL = SQL + ComNum.VBLF + " WHERE  OPDATE = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND  ( GBDEL <> '*' OR GBDEL IS NULL ) ";
                SQL = SQL + ComNum.VBLF + " AND  ( GBANGIO IS NULL OR GBANGIO='N') ";
                SQL = SQL + ComNum.VBLF + " AND   OPROOM NOT IN ('*','N') ";  //'DSC 제외  NSBLOCK 제외;
                SQL = SQL + ComNum.VBLF + " ORDER BY OPROOM, OPSEQ ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("검색된 자료가 없습니다.", "확인");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["OPROOM"].ToString().Trim() == "G")
                        {
                            n = 9;
                        }
                        else
                        {
                            n = Convert.ToInt32(VB.Val(dt.Rows[i]["OPROOM"].ToString().Trim())) - 1;
                            strGbMagam = dt.Rows[i]["GBMAGAM"].ToString().Trim();
                        }

                        if (n < 0)
                            n = 8;

                        strOpSeq = dt.Rows[i]["OPSEQ"].ToString().Trim();
                        strDeptCode = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        strSname = dt.Rows[i]["SNAME"].ToString().Trim();
                        strOpTime = dt.Rows[i]["OPTIME"].ToString().Trim();
                        strPtNo = dt.Rows[i]["PANO"].ToString();
                        strSex = dt.Rows[i]["SEX"].ToString().Trim();
                        strAge = dt.Rows[i]["AGE"].ToString().Trim();
                        strEntDate = dt.Rows[i]["ENT1"].ToString().Trim();

                        SheetRoom[n].RowCount = SheetRoom[n].RowCount + 1;
                        SheetRoom[n].Cells[SheetRoom[n].RowCount - 1, 0].Text = strOpSeq;
                        SheetRoom[n].Cells[SheetRoom[n].RowCount - 1, 1].Text = strDeptCode;
                        SheetRoom[n].Cells[SheetRoom[n].RowCount - 1, 2].Text = strSname;
                        SheetRoom[n].Cells[SheetRoom[n].RowCount - 1, 3].Text = strOpTime;
                        SheetRoom[n].Cells[SheetRoom[n].RowCount - 1, 4].Text = strPtNo;
                        SheetRoom[n].Cells[SheetRoom[n].RowCount - 1, 5].Text = strSex;
                        SheetRoom[n].Cells[SheetRoom[n].RowCount - 1, 6].Text = strAge;
                        SheetRoom[n].Cells[SheetRoom[n].RowCount - 1, 7].Text = strEntDate;

                        if (n <= 4)
                        {
                            ssPrint_Sheet1.Cells[1, n].Text = ssPrint_Sheet1.Cells[1, n].Text + VB.Space(2 - VB.Len(VB.Trim(strOpSeq))) + VB.Trim(strOpSeq) + "." + strDeptCode + " " + ComFunc.RPAD(strSname, 10, " ") + ComFunc.RPAD(strOpTime, 10, " ") + ComNum.VBLF;
                        }
                        else
                        {
                            ssPrint_Sheet1.Cells[3, n - 5].Text = ssPrint_Sheet1.Cells[3, n - 5].Text + VB.Space(2 - VB.Len(VB.Trim(strOpSeq))) + VB.Trim(strOpSeq) + "." + strDeptCode + " " + ComFunc.RPAD(strSname, 10, " ") + ComFunc.RPAD(strOpTime, 10, " ") + ComNum.VBLF;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                lblCheck.Text = (strGbMagam == "*" ? "확인" : "미확인");


                //'-----------------------------------------
                //'  수술스케쥴에 WRTNO가 없으면 Update함
                //'-----------------------------------------

                SQL = "SELECT ROWID,WRTNO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ORAN_MASTER ";
                SQL = SQL + ComNum.VBLF + "WHERE OPDATE=TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND (WRTNO IS NULL OR WRTNO = 0) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nWRTNO = clsOpMain.READ_New_JepsuNo(clsDB.DbCon);
                        SQL = "UPDATE KOSMOS_PMPA.ORAN_MASTER SET WRTNO=" + nWRTNO + " ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID ='" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

}

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "수술실 스케쥴관리";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("수술일자 : " + dtpOpDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 60, 20, 100, 100);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void ssRoom_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (((FpSpread)sender).ActiveSheet.RowCount == 0)
            {
                return;
            }

            lblPtData.Text = ((FpSpread)sender).ActiveSheet.Cells[e.Row, 4].Text.Trim()
                            + " " + ((FpSpread)sender).ActiveSheet.Cells[e.Row, 2].Text.Trim()
                            + " " + ((FpSpread)sender).ActiveSheet.Cells[e.Row, 5].Text.Trim()
                            + "/" + ((FpSpread)sender).ActiveSheet.Cells[e.Row, 6].Text.Trim()
                            + " " + ((FpSpread)sender).ActiveSheet.Cells[e.Row, 7].Text.Trim();
        }

        private void ssRoom_Enter(object sender, EventArgs e)
        {
            if (((FpSpread)sender).ActiveSheet.RowCount == 0)
            {
                mintRow = 0;
            }
            else
            {
                mintRow = ((FpSpread)sender).ActiveSheet.RowCount;
            }
            mintsIdx = Convert.ToInt32(VB.Val(((FpSpread)sender).Name.Replace("ssRoom", "")));
        }

        private void ssRoom_CellClick(object sender, CellClickEventArgs e)
        {
            mintRow = ((FpSpread)sender).ActiveSheet.ActiveRowIndex;
            mintsIdx = Convert.ToInt32(VB.Val(((FpSpread)sender).Name.Replace("ssRoom", "")));
        }
    }
}
