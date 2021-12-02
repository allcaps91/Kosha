using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

/// <summary>
/// Description : 외래 청구상병 자동발생 등록
/// Author : 김형범
/// Create Date : 2017.06.30
/// </summary>
/// <history>
/// 완료
/// </history>
/// <summary> 외래 청구상병 자동발생 등록 </summary>
namespace ComLibB
{
    public partial class frmMill : Form
    {
        public frmMill()
        {
            InitializeComponent();
        }

        void frmMill_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ScreenDisplay();
        }

        void ScreenDisplay()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string strDeptCode = "";
            string strIllCode1 = "";
            string strIllCode2 = "";
            string strIllCode3 = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ssClear(ssView);

                SQL = "";
                SQL = "SELECT a.SuCode,b.SuNameK,a.DeptCode,a.IllCode1,a.IllCode2,a.illCode3, A.RO1, A.RO2, A.RO3, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(a.EntDate,'YYYY-MM-DD') EntDate,a.ROWID, A.SEX, A.FAGE, A.TAGE ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_MILL a, " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.SuCode = b.SuNext(+) ";
                SQL = SQL + ComNum.VBLF + "  AND a.GBIO ='O' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.SuCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count + 15;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                    strIllCode1 = dt.Rows[i]["ILLCODE1"].ToString().Trim();
                    strIllCode2 = dt.Rows[i]["ILLCODE2"].ToString().Trim();
                    strIllCode3 = dt.Rows[i]["ILLCODE3"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 0].Text = "";
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = strDeptCode;

                    if (strDeptCode != "" && strDeptCode != "**")
                    {
                        ssView_Sheet1.Cells[i, 4].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strDeptCode);
                    }

                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["RO1"].ToString().Trim();

                    if (dt.Rows[i]["RO1"].ToString().Trim() == "*")
                    {
                        ssView_Sheet1.Cells[i, 6].BackColor = Color.Blue;
                    }

                    ssView_Sheet1.Cells[i, 7].Text = strIllCode1;

                    if (strIllCode1 != "")
                    {
                        ssView_Sheet1.Cells[i, 8].Text =clsVbfunc. READ_ILLName(clsDB.DbCon, strIllCode1);
                    }

                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["RO2"].ToString().Trim();

                    if (dt.Rows[i]["RO2"].ToString().Trim() == "*")
                    {
                        ssView_Sheet1.Cells[i, 9].BackColor = Color.Blue;
                    }

                    ssView_Sheet1.Cells[i, 10].Text = strIllCode2;

                    if (strIllCode2 != "")
                    {
                        ssView_Sheet1.Cells[i, 11].Text =clsVbfunc. READ_ILLName(clsDB.DbCon, strIllCode2);
                    }

                    ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["RO3"].ToString().Trim();

                    if (dt.Rows[i]["RO3"].ToString().Trim() == "*")
                    {
                        ssView_Sheet1.Cells[i, 12].BackColor = Color.Blue;
                    }

                    ssView_Sheet1.Cells[i, 13].Text = strIllCode3;

                    if (strIllCode3 != "")
                    {
                        ssView_Sheet1.Cells[i, 14].Text =clsVbfunc. READ_ILLName(clsDB.DbCon, strIllCode3);
                    }

                    ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 16].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 17].Text = "";

                    SQL = " SELECT DELDATE ";
                    SQL = SQL + ComNum.VBLF + "  FROM VIEW_SUGA_CODE ";
                    SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + dt.Rows[i]["SuCode"].ToString().Trim() + "'";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[i, 18].Text = dt1.Rows[0]["DelDate"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //2020-07-22 김준수 주임 요청으로 나이 조건 추가작업
                    ssView_Sheet1.Cells[i, 19].Text = dt.Rows[i]["FAGE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 20].Text = dt.Rows[i]["TAGE"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void ssClear(FpSpread Spread)
        {
            Spread.ActiveSheet.Cells[0, 0, Spread.ActiveSheet.Rows.Count - 1, Spread.ActiveSheet.Columns.Count - 1].Text = "";
            Spread.ActiveSheet.SetActiveCell(0, 0);
            Spread.ActiveSheet.RowCount = 0;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strChange = "";
            string strROWID = "";
            bool strChk = false;

            string strSuCode = "";
            string strDeptCode = "";
            string strIllCode1 = "";
            string strIllCode2 = "";
            string strIllCode3 = "";
            string strSex = "";
            string strRO1 = "";
            string strRO2 = "";
            string strRO3 = "";

            int strFAGE = 0;
            int strTAGE = 0;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
                {
                    strChk = Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value);
                    strSuCode = ssView_Sheet1.Cells[i, 1].Text;
                    strDeptCode = ssView_Sheet1.Cells[i, 3].Text;
                    strSex = ssView_Sheet1.Cells[i, 5].Text;
                    strRO1 = ssView_Sheet1.Cells[i, 6].Text;
                    strIllCode1 = ssView_Sheet1.Cells[i, 7].Text;
                    strRO2 = ssView_Sheet1.Cells[i, 9].Text;
                    strIllCode2 = ssView_Sheet1.Cells[i, 10].Text;
                    strRO2 = ssView_Sheet1.Cells[i, 12].Text;
                    strIllCode3 = ssView_Sheet1.Cells[i, 13].Text;
                    strROWID = ssView_Sheet1.Cells[i, 16].Text;
                    strChange = ssView_Sheet1.Cells[i, 17].Text;

                    strFAGE = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 19].Text));
                    strTAGE = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 20].Text));

                    SQL = "";

                    if (strChk == true)
                    {
                        if (strROWID != "")
                        {

                            SQL = "";
                            SQL = "DELETE " + ComNum.DB_PMPA + "BAS_MILL WHERE ROWID = '" + strROWID + "' ";
                        }
                    }
                    else if (strChange == "Y")
                    {
                        if (strROWID == "" && strSuCode != "")
                        {

                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_MILL (SuCode,DeptCode,IllCode1,IllCode2,IllCode3,EntDate,SEX, RO1, RO2, RO3,GBIO,FAGE,TAGE) ";
                            SQL = SQL + ComNum.VBLF + "VALUES ('" + strSuCode + "','" + strDeptCode + "',";
                            SQL = SQL + ComNum.VBLF + "'" + strIllCode1 + "','" + strIllCode2 + "','" + strIllCode3 + "',SYSDATE," + "'" + strSex + "', ";
                            SQL = SQL + ComNum.VBLF + " '" + strRO1 + "', '" + strRO2 + "','" + strRO3 + "','O','" + strFAGE + "','" + strTAGE + "') ";
                        }
                        else if (strROWID != "")
                        {

                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_MILL SET ";
                            SQL = SQL + ComNum.VBLF + "SuCode='" + strSuCode + "',";
                            SQL = SQL + ComNum.VBLF + "DeptCode='" + strDeptCode + "',";
                            SQL = SQL + ComNum.VBLF + "IllCode1='" + strIllCode1 + "',";
                            SQL = SQL + ComNum.VBLF + " RO1 = '" + strRO1 + "',";
                            SQL = SQL + ComNum.VBLF + "IllCode2='" + strIllCode2 + "',";
                            SQL = SQL + ComNum.VBLF + " RO2 = '" + strRO2 + "',";
                            SQL = SQL + ComNum.VBLF + "IllCode3='" + strIllCode3 + "',";
                            SQL = SQL + ComNum.VBLF + " RO3 = '" + strRO3 + "',";
                            SQL = SQL + ComNum.VBLF + "SEX     ='" + strSex + "',";
                            SQL = SQL + ComNum.VBLF + "FAGE     ='" + strFAGE + "',";
                            SQL = SQL + ComNum.VBLF + "TAGE     ='" + strTAGE + "'";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                        }
                    }
                    
                    if(SQL.Length > 0)
                    {
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
                clsDB.setCommitTran(clsDB.DbCon);
                ScreenDisplay();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"15\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strHead1 = "/c/f1" + "외래 청구상병 자동발생 내역(BAS_MILL)" + "/n";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb1 /fi0 /fu0 /fk0 /fs2";
            strHead2 = "/l/f2" + "출력일자 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "/r" + "PAGE : " + "/p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 30;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = PrintType.All;
            ssView.PrintSheet(0);
        }

        void ssView_Change(object sender, ChangeEventArgs e)
        {

            string strData = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                if (e.Column > 0)
                {
                    ssView_Sheet1.Cells[e.Row, 17].Text = "Y";
                }

                strData = VB.UCase(ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim());
                ssView_Sheet1.Cells[e.Row, e.Column].Text = strData;

                switch (e.Column)
                {
                    case 1:
                        SQL = "";
                        SQL = "SELECT SuNameK ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                        SQL = SQL + ComNum.VBLF + "WHERE SuNext='" + strData + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[e.Row, 2].Text = dt.Rows[0]["SuNameK"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[e.Row, 1].Text = "";
                            ssView_Sheet1.Cells[e.Row, 2].Text = "";
                            ComFunc.MsgBox(" 수가코드 등록 않됨", "확인");
                        }

                        dt.Dispose();
                        dt = null;
                        break;
                    case 3:
                        ssView_Sheet1.Cells[e.Row, 4].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strData);
                        break;
                    case 5:
                        switch (strData)
                        {
                            case "M":
                            case "F":
                            case "*":
                                break;
                            default:
                                ComFunc.MsgBox("해당하는란에는 M F * 값중에서 하나선택해주세요", "확인");
                                ssView_Sheet1.Cells[e.Row, e.Column].Text = "*";
                                break;
                        }
                        break;
                    case 7:
                    case 10:
                    case 13:
                        ssView_Sheet1.Cells[e.Row, e.Column + 1].Text = clsVbfunc.READ_ILLName(clsDB.DbCon, strData);
                        break;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.Column == 6 || e.Column == 9 || e.Column == 12)
            {
                if (ssView_Sheet1.Cells[e.Row, e.Column].Text == "")
                {
                    ssView_Sheet1.Cells[e.Row, e.Column].Text = "*";
                    ssView_Sheet1.Cells[e.Row, e.Column].BackColor = Color.Blue;
                    ssView_Sheet1.Cells[e.Row, 17].Text = "Y";
                }
                else
                {
                    ssView_Sheet1.Cells[e.Row, e.Column].Text = "";
                    ssView_Sheet1.Cells[e.Row, e.Column].BackColor = Color.White;
                    ssView_Sheet1.Cells[e.Row, 17].Text = "Y";
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(rdoDown.Checked)
            {
                ssView_Sheet1.Rows.Add(ssView_Sheet1.RowCount, (int)VB.Val(txtLine.Text));
            }
            else if(rdoUp.Checked)
            {
                ssView_Sheet1.Rows.Add(0, (int)VB.Val(txtLine.Text));
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) return;

            int foundRow = -1;
            int foundColum = -1;

            ssView.Search(0, txtSearch.Text.Trim(), false, false, false, false, 0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1, ref foundRow, ref foundColum);
            ssView_Sheet1.SetActiveCell(foundRow, foundColum);
            ssView.ShowRow(0, foundRow, VerticalPosition.Nearest);
            ssView.ShowColumn(0, foundColum, HorizontalPosition.Nearest);
        }

        private void ssView_ClipboardPasted(object sender, ClipboardPastedEventArgs e)
        {
            string strData = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                if (e.CellRange.Column > 0)
                {
                    ssView_Sheet1.Cells[e.CellRange.Row, 17].Text = "Y";
                }

                strData = ssView_Sheet1.Cells[e.CellRange.Row, e.CellRange.Column].Text.Trim().ToUpper();
                ssView_Sheet1.Cells[e.CellRange.Row, e.CellRange.Column].Text = strData;

                switch (e.CellRange.Column)
                {
                    case 1:
                        SQL = "";
                        SQL = "SELECT SuNameK ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                        SQL = SQL + ComNum.VBLF + "WHERE SuNext='" + strData + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[e.CellRange.Row, 2].Text = dt.Rows[0]["SuNameK"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[e.CellRange.Row, 1].Text = "";
                            ssView_Sheet1.Cells[e.CellRange.Row, 2].Text = "";
                            ComFunc.MsgBox(" 수가코드 등록 않됨", "확인");
                        }

                        dt.Dispose();
                        dt = null;
                        break;
                    case 3:
                        ssView_Sheet1.Cells[e.CellRange.Row, 4].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strData);
                        break;
                    case 5:
                        switch (strData)
                        {
                            case "M":
                            case "F":
                            case "*":
                                break;
                            default:
                                ComFunc.MsgBox("해당하는란에는 M F * 값중에서 하나선택해주세요", "확인");
                                ssView_Sheet1.Cells[e.CellRange.Row, e.CellRange.Column].Text = "*";
                                break;
                        }
                        break;
                    case 7:
                    case 10:
                    case 13:
                        ssView_Sheet1.Cells[e.CellRange.Row, e.CellRange.Column + 1].Text = clsVbfunc.READ_ILLName(clsDB.DbCon, strData);
                        break;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }
    }
}
