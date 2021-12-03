using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmMSelf_I
    /// File Name : frmMSelf_I.cs
    /// Title or Description : 입원 제한사항 등록
    /// Author : 박창욱
    /// Create Date : 2017-06-13
    /// Update Histroy :  2018-07-28(삭제, 저장 수정)
    /// </summary>  
    /// <history>  
    /// VB\BuCode40.frm(FrmMSelf_I) -> frmMSelf_I.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\bucode\BuCode40.frm(FrmMSelf_I)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\bucode\\bucode.vbp
    /// </vbp>
    public partial class frmMSelf_I : Form
    {
        public frmMSelf_I()
        {
            InitializeComponent();
        }

        private void SCREEN_CLEAR()
        {
            cboJob.Enabled = true;
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;

            ssView.Enabled = false;
        }

        private void Sheet_Title_SET()
        {
            string strTitle = "";

            switch (VB.Left(cboJob.Text, 2))
            {
                case "11":
                case "12":
                case "83":
                    ssView_Sheet1.Columns[ 2].Label = "나이";
                    ssView_Sheet1.Columns[ 3].Label = " ";
                    break;
                case "21":
                case "22":
                    ssView_Sheet1.Columns[ 2].Label = "진료과";
                    ssView_Sheet1.Columns[ 3].Label = " ";
                    break;
                case "32":
                    ssView_Sheet1.Columns[ 2].Label = "성분분류";   
                    ssView_Sheet1.Columns[ 3].Label = "제한수량";
                    break;
                case "41":
                case "42":
                    ssView_Sheet1.Columns[ 2].Label = "상병코드";
                    ssView_Sheet1.Columns[ 3].Label = "진료과";
                    break;
                case "52":
                    ssView_Sheet1.Columns[ 2].Label = "환자종류";
                    ssView_Sheet1.Columns[ 3].Label = "상병코드";
                    break;
                case "53":
                    ssView_Sheet1.Columns[ 2].Label = "환자종류";
                    ssView_Sheet1.Columns[ 3].Label = " ";
                    break;
                case "62":
                case "71":
                case "72":
                    ssView_Sheet1.Columns[ 2].Label = "진료과";
                    ssView_Sheet1.Columns[ 3].Label = " ";
                    break;
                case "A2":
                    ssView_Sheet1.Columns[ 2].Label = "진료과";
                    ssView_Sheet1.Columns[ 3].Label = " ";
                    break;
                case "80":
                case "81":
                    ssView_Sheet1.Columns[2].Label = "동시불가코드";
                    ssView_Sheet1.Columns[3].Label = " ";
                    break;
                case "82":
                    ssView_Sheet1.Columns[2].Label = "배수함량코드";
                    ssView_Sheet1.Columns[3].Label = "배수(1회투약량) ";
                    break;
                case "84":
                    ssView_Sheet1.Columns[2].Label = "일수";
                    ssView_Sheet1.Columns[3].Label = " ";
                    break;
                case "85":
                    ssView_Sheet1.Columns[2].Label = "수량(개수)";
                    ssView_Sheet1.Columns[3].Label = " ";
                    break;
                case "86":
                case "89":
                    ssView_Sheet1.Columns[2].Label = "일수";
                    ssView_Sheet1.Columns[3].Label = " ";
                    break;
                case "91":
                    ssView_Sheet1.Columns[2].Label = "성별제한";
                    ssView_Sheet1.Columns[3].Label = " ";
                    break;
            }

            strTitle = ssView_Sheet1.Columns[3].Label.Trim();

            FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();

            if (strTitle == "")
            {
                textCellType.WordWrap = false;

                ssView_Sheet1.Columns[3].CellType = textCellType;
                ssView_Sheet1.Columns[3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                ssView_Sheet1.Columns[3].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssView_Sheet1.Columns[3].Label = "";
            }
            else
            {
                textCellType.CharacterCasing = CharacterCasing.Upper;
                textCellType.CharacterSet = FarPoint.Win.Spread.CellType.CharacterSet.Ascii;
                textCellType.Multiline = false;

                switch (strTitle)
                {
                    case "진료과":
                        textCellType.MaxLength = 2;
                        break;
                    case "상병코드":
                        textCellType.MaxLength = 6;
                        break;
                    case "제한수량":
                        textCellType.MaxLength = 2;
                        break;
                    default:
                        textCellType.MaxLength = 6;
                        break;
                }

                ssView_Sheet1.Columns[3].CellType = textCellType;
                ssView_Sheet1.Columns[3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            }
        }

 

        private void searchData()
        {
            int i = 0;
            string strGubunA = "";
            string strGubunB = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt2 = null;

            strGubunA = VB.Left(cboJob.Text, 1);
            strGubunB = VB.Mid(cboJob.Text, 2, 1);

            try
            {
                //최종 자료를 Sheet에 Display
                SQL = "";
                SQL = "SELECT a.SuCode,a.FieldA,a.FieldB,TO_CHAR(a.EntDate,'YYYY-MM-DD') EntDate,";
                SQL = SQL + ComNum.VBLF + "      a.ROWID,b.SuNameK ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_MSELF_I a,ADMIN.BAS_SUN b ";
                SQL = SQL + ComNum.VBLF + "WHERE a.GubunA = '" + strGubunA + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.GubunB = '" + strGubunB + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.SuCode = b.SuNext(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.SuCode ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count + 50;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = "";
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FieldA"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FieldB"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    if (VB.Left(cboJob.Text, 2) == "80" || VB.Left(cboJob.Text, 2) == "81" ||
                       VB.Left(cboJob.Text, 2) == "82" || VB.Left(cboJob.Text, 2) == "89")
                    {
                        SQL = "";
                        SQL = "SELECT SuNameK FROM ADMIN.BAS_SUN ";
                        SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + dt.Rows[i]["FieldA"].ToString().Trim() + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt2.Rows.Count != 0)
                        {
                            ssView_Sheet1.Cells[i, 5].Text = ComFunc.LeftH(ssView_Sheet1.Cells[i, 5].Text + VB.Space(30), 30)
                                                            + "/" + dt2.Rows[0]["SuNameK"].ToString().Trim();
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }

                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = "";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMSelf_I_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR ();

            ssView_Sheet1.Columns[6].Visible = false;
            ssView_Sheet1.Columns[7].Visible = false;

            //작업종류를 ComboBox에 SET
            cboJob.Items.Clear();
            cboJob.Items.Add("09.DUR 병용금기(약제과관리)");
            cboJob.Items.Add("10.DUR 연령금기(약제과관리)");
            cboJob.Items.Add("11.xx세 이하는 사용금지(심사청구)");
            cboJob.Items.Add("12.xx세 이상은 사용금지(심사청구)");
            cboJob.Items.Add("21.특정과만 급여");
            cboJob.Items.Add("22.특정과는 비급여");
            cboJob.Items.Add("71.남자 특정과 비급여");
            cboJob.Items.Add("72.여자 특정과 비급여");
            cboJob.Items.Add("81.동시처방불가(약제)-심사");
            cboJob.Items.Add("85.입원 OCS 1일 처방당 일용량(갯수)제한");
            cboJob.Items.Add("86.외래/입원 OCS 분할처방 금지");
            cboJob.Items.Add("89.입원 OCS 재원중 기간제한");
            cboJob.Items.Add("91.입원 OCS 남자수가제한");   

            if (clsPublic.GnJobSabun == 2186)
            {
                cboJob.SelectedIndex = 13;
            }
            else
            {
                cboJob.SelectedIndex = 0;
            }
    }

        private void cboJob_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { SendKeys.Send("{Tab}"); }
        }

        private void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strData = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                if (e.Column > 0)
                {
                    ssView_Sheet1.Cells[e.Row, 7].Text = "Y";
                }
                if (e.Column == 1)
                {
                    if (VB.Left(cboJob.Text, 2) != "D1")
                    {
                        strData = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();
                        SQL = "";
                        SQL = "SELECT SuNameK FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                        SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + strData + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count == 0)
                        {
                            ssView_Sheet1.Cells[e.Row, 1].Text = "";
                            ssView_Sheet1.Cells[e.Row, 5].Text = "";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[e.Row, 5].Text = dt.Rows[0]["SuNameK"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }

                if (e.Column == 2 &&  VB.Left(cboJob.Text, 2) == "81" || VB.Left(cboJob.Text, 2) == "82")
                {
                    strData = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();
                    SQL = "";
                    SQL = "SELECT SuNameK FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                    SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + strData + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        ssView_Sheet1.Cells[e.Row, 2].Text = "";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[e.Row, 5].Text = ssView_Sheet1.Cells[e.Row, 5].Text + "/" + dt.Rows[0]["SuNameK"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboJob_Click(object sender, EventArgs e)
        {
            Sheet_Title_SET();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            ssView.Enabled = true;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;

            searchData();

            cboJob.Enabled = false;
            btnSearch.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnPrint.Enabled = true;

            btnCancel.Focus();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            bool chk = false;
            string strChange = "";
            string strROWID = "";
            string strSuCode = "";
            string strGubunA = "";
            string strGubunB = "";
            string strFieldA = "";
            string strFieldB = "";
            string strDate = "";
            string strGubun = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strGubunA = VB.Left(cboJob.Text, 1);
            strGubunB = VB.Mid(cboJob.Text, 2, 1);

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssView_Sheet1.NonEmptyRowCount; i++)
                {
                    chk = Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value);
                    strSuCode = ssView_Sheet1.Cells[i, 1].Text.ToUpper();
                    strFieldA = ssView_Sheet1.Cells[i, 2].Text;
                    strFieldB = ssView_Sheet1.Cells[i, 3].Text;
                    strDate = ssView_Sheet1.Cells[i, 4].Text;
                    strROWID = ssView_Sheet1.Cells[i, 6].Text;
                    strChange = ssView_Sheet1.Cells[i, 7].Text;

                    strGubun = "";
                    SQL = "";
                    if (chk == true)
                    {
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = "DELETE " + ComNum.DB_PMPA + "BAS_MSELF_I WHERE ROWID = '" + strROWID + "' ";
                            strGubun = "D";
                        }
                    }
                    else if (strChange == "Y")
                    {
                        if (strROWID == "" && strSuCode != "")
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_MSELF_I (SuCode,GubunA,GubunB,FieldA,FieldB,";
                            SQL = SQL + "EntDate) VALUES ('" + strSuCode + "','" + strGubunA + "'," + ComNum.VBLF;
                            SQL = SQL + "'" + strGubunB + "','" + strFieldA + "','" + strFieldB + "'," + ComNum.VBLF;
                            SQL = SQL + "TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
                            strGubun = "I";
                        }
                        else if (strROWID != "")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_MSELF_I SET FieldA='" + strFieldA + "',";
                            SQL = SQL + ComNum.VBLF + "FieldB='" + strFieldB + "',";
                            SQL = SQL + ComNum.VBLF + "EntDate=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                            strGubun = "U";
                        }
                    }

                    if (SQL != "")
                    {
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                        }
                        switch (strGubun)
                        {
                            case "U":
                            case "D":
                                SQL = "";
                                SQL = " INSERT INTO " + ComNum.DB_PMPA + "BAS_MSELF_I_HISTORY(";
                                SQL = SQL + ComNum.VBLF + " SUCODE, GUBUNA, GUBUNB, FIELDA, FIELDB, ENTDATE, GUBUN, HDATE)  ";
                                SQL = SQL + ComNum.VBLF + "SELECT SUCODE, GUBUNA, GUBUNB, FIELDA, FIELDB, ENTDATE, '" + strGubun + "', SYSDATE";
                                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_MSELF_I";
                                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                }
                                break;
                        }
                    }

                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                SCREEN_CLEAR();
                cboJob.Focus();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            cboJob.Focus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            //자료를 인쇄
            strFont1 = "/fn\"굴림체\" /fz\"15\"";
            strFont2 = "/fn\"굴림체\" /fz\"10\"";
            strHead1 = "/n" + "/l/f1" + VB.Space(16) + "입원 제한사항 코드집" + "/n";
            strHead2 = "/l/f2" + ComFunc.LeftH(cboJob.Text + VB.Space(50), 50);
            strHead2 = strHead2 + "출력일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(12) + "PAGE:" + "/p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 30;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 180;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnBY_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira01.FRM
            //FrmHira01.Show 1
            frmHira01 frmHira01 = new frmHira01();
            frmHira01.Show();
        }

        private void btnAge_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira02.FRM
            //FrmHira02.Show 1
            frmHira02 frm = new frmHira02();
            frm.Show();
        }

        private void btnStability_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira03.FRM
            //FrmHira03.Show 1
            frmHira03 frm = new frmHira03();
            frm.Show();
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira04.FRM
            //FrmHira04.Show 1
            frmHira04 frm = new frmHira04();
            frm.Show();
        }

        private void btnLow_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira06.FRM
            //FrmHira06.Show 1
            frmHira06 frm = new frmHira06();
            frm.Show();
        }

        private void btnImBu_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira07.FRM
            //FrmHira07.Show 1
            frmHira07 frm = new frmHira07();
            frm.Show();
        }

        private void ssView_ClipboardPasted(object sender, FarPoint.Win.Spread.ClipboardPastedEventArgs e)
        {
            string strData = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                if (e.CellRange.Column > 0)
                {
                    ssView_Sheet1.Cells[e.CellRange.Row, 7].Text = "Y";
                }
                if (e.CellRange.Column == 1)
                {
                    if (VB.Left(cboJob.Text, 2) != "D1")
                    {
                        strData = ssView_Sheet1.Cells[e.CellRange.Row, e.CellRange.Column].Text.Trim().ToUpper();
                        SQL = "";
                        SQL = "SELECT SuNameK FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                        SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + strData + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count == 0)
                        {
                            ssView_Sheet1.Cells[e.CellRange.Row, 1].Text = "";
                            ssView_Sheet1.Cells[e.CellRange.Row, 5].Text = "";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[e.CellRange.Row, 5].Text = dt.Rows[0]["SuNameK"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }

                if (e.CellRange.Column == 2 && VB.Left(cboJob.Text, 2) == "81" || VB.Left(cboJob.Text, 2) == "82")
                {
                    strData = ssView_Sheet1.Cells[e.CellRange.Row, e.CellRange.Column].Text.Trim().ToUpper();
                    SQL = "";
                    SQL = "SELECT SuNameK FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                    SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + strData + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        ssView_Sheet1.Cells[e.CellRange.Row, 2].Text = "";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[e.CellRange.Row, 5].Text = ssView_Sheet1.Cells[e.CellRange.Row, 5].Text + "/" + dt.Rows[0]["SuNameK"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
