using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmMSelf
    /// File Name : frmMSelf.cs
    /// Title or Description : 외래 제한사항 등록
    /// Author : 박창욱
    /// Create Date : 2017-06-08
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    /// </summary>  
    /// <history>  
    /// VB\BuCode42.frm(frmMSelf) -> frmMSelf.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\bucode\BuCode42.frm(frmMSelf)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\bucode\\bucode.vbp
    /// </vbp>
    public partial class frmMSelf : Form
    {
        public frmMSelf()
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

            ssView_Sheet1.Columns[4].Label = "등록일자";
            ssView_Sheet1.Columns[9].Visible = false;

            switch (VB.Left(cboJob.Text, 2))
            {
                case "81":
                    ssView_Sheet1.Columns[8].Visible = true;
                    break;
                case "88":
                    ssView_Sheet1.Columns[3].Visible = false;
                    break;
                default:
                    ssView_Sheet1.Columns[8].Visible = false;
                    ssView_Sheet1.Columns[3].Visible = true;
                    break;
            }

            switch (VB.Left(cboJob.Text, 2))
            {
                case "07":
                    ssView_Sheet1.Columns[1].Label= "";
                    ssView_Sheet1.Columns[3].Label= "제한구분 ";
                    ssView_Sheet1.Columns[2].Label= "";
                    ssView_Sheet1.Columns[3].Label= "제한구분 ";
                    break;
                case "08":
                    ssView_Sheet1.Columns[2].Label= "나이";
                    ssView_Sheet1.Columns[3].Label= "제한구분 ";
                    break;
                case "09":
                    ssView_Sheet1.Columns[2].Label= "동시불가코드";
                    ssView_Sheet1.Columns[3].Label= " ";
                    ssView_Sheet1.Columns[4].Label= "고시일자";
                    break;
                case "10":
                    ssView_Sheet1.Columns[2].Label= "나이";
                    ssView_Sheet1.Columns[3].Label= "제한구분";
                    ssView_Sheet1.Columns[4].Label= "고시일자";
                    break;
                case "11":
                    ssView_Sheet1.Columns[2].Label= "나이";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "12":
                    ssView_Sheet1.Columns[2].Label= "나이";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "83":
                    ssView_Sheet1.Columns[2].Label= "나이";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "21":
                    ssView_Sheet1.Columns[2].Label= "진료과";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "22":
                    ssView_Sheet1.Columns[2].Label= "진료과";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "23":
                    ssView_Sheet1.Columns[2].Label= "진료과";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "32":
                    ssView_Sheet1.Columns[2].Label= "성분분류";
                    ssView_Sheet1.Columns[3].Label= "제한수량";
                    break;
                case "41":
                    ssView_Sheet1.Columns[2].Label= "상병코드";
                    ssView_Sheet1.Columns[3].Label= "진료과";
                    break;
                case "42":
                    ssView_Sheet1.Columns[2].Label= "상병코드";
                    ssView_Sheet1.Columns[3].Label= "진료과";
                    break;
                case "43":
                    ssView_Sheet1.Columns[2].Label= "상병코드";
                    ssView_Sheet1.Columns[3].Label= "진료과";
                    break;
                case "52":
                    ssView_Sheet1.Columns[2].Label= "환자종류";
                    ssView_Sheet1.Columns[3].Label= "상병코드";
                    break;
                case "53":
                    ssView_Sheet1.Columns[2].Label= "환자종류";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "62":
                    ssView_Sheet1.Columns[2].Label= "진료과";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "71":
                    ssView_Sheet1.Columns[2].Label= "진료과";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "72":
                    ssView_Sheet1.Columns[2].Label= "진료과";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "A2":
                    ssView_Sheet1.Columns[2].Label= "진료과";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "81":
                    ssView_Sheet1.Columns[2].Label= "동시불가코드";
                    ssView_Sheet1.Columns[3].Label= "진료과";
                    break;
                case "80":
                    ssView_Sheet1.Columns[2].Label= "동시불가코드";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "82":
                    ssView_Sheet1.Columns[2].Label= "배수함량코드";
                    ssView_Sheet1.Columns[3].Label= "배수(1회투약량) ";
                    break;
                case "84":
                    ssView_Sheet1.Columns[2].Label= "일수";
                    ssView_Sheet1.Columns[3].Label= "과";
                    break;
                case "85":
                    ssView_Sheet1.Columns[2].Label= "수량(개수)";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "86":
                    ssView_Sheet1.Columns[2].Label= " ";
                    ssView_Sheet1.Columns[3].Label= " ";
                    break;
                case "87":
                    ssView_Sheet1.Columns[9].Visible = true;
                    ssView_Sheet1.Columns[2].Label= "기간(일)";
                    ssView_Sheet1.Columns[2].Label= "기간";
                    ssView_Sheet1.Columns[3].Label = "개수";
                    ssView_Sheet1.Columns[9].Label= "과";
                    break;
                case "88":
                    ssView_Sheet1.Columns[9].Visible = true;
                    ssView_Sheet1.Columns[3].Visible = false;
                    ssView_Sheet1.Columns[3].Label = "";
                    ssView_Sheet1.Columns[2].Label = "기간(일)";
                    ssView_Sheet1.Columns[9].Label = "과";
                    break;
                case "C1":
                    ssView_Sheet1.Columns[2].Label= "수량";
                    ssView_Sheet1.Columns[3].Label= "변환수가";
                    break;
                case "D1":
                    ssView_Sheet1.Columns[1].Label= "상병코드";
                    ssView_Sheet1.Columns[2].Label= "과";
                    ssView_Sheet1.Columns[3].Label= "";
                    ssView_Sheet1.Columns[5].Label= "참고사항";
                    break;
                case "J1":
                    ssView_Sheet1.Columns[1].Label= "수가코드";
                    ssView_Sheet1.Columns[2].Label= "";
                    ssView_Sheet1.Columns[3].Label= "";
                    ssView_Sheet1.Columns[5].Label= "참고사항";
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
                    case "변환수가":
                        textCellType.MaxLength = 8;
                        break;
                    default:
                        textCellType.MaxLength = 6;
                        break;

                }

                ssView_Sheet1.Columns[3].CellType = textCellType;
                ssView_Sheet1.Columns[3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            cboJob.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string strChk = "";
            string strChange = "";
            string strROWID = "";
            string strSuCode = "";
            string strGubunA = "";
            string strGubunB = "";
            string strFieldA = "";
            string strFieldB = "";
            string strFieldC = "";
            string strDate = "";
            string strRemark = "";
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
                for (i = 1; i < ssView_Sheet1.RowCount; i++)
                {
                    strChk = ssView_Sheet1.Cells[i - 1, 0].Text;
                    strSuCode = ssView_Sheet1.Cells[i - 1, 1].Text.ToUpper();
                    strFieldA = ssView_Sheet1.Cells[i - 1, 2].Text;
                    strFieldB = ssView_Sheet1.Cells[i - 1, 3].Text;
                    strDate = ssView_Sheet1.Cells[i - 1, 4].Text;
                    strRemark = ssView_Sheet1.Cells[i - 1, 5].Text;
                    strROWID = ssView_Sheet1.Cells[i - 1, 6].Text;
                    strChange = ssView_Sheet1.Cells[i - 1, 7].Text;
                    strFieldC = ssView_Sheet1.Cells[i - 1, 9].Text;

                    if (strGubunA == "8" && strGubunB == "4")
                    {
                        strFieldB = strFieldB.ToUpper();
                    }

                    strGubun = "";
                    SQL = "";
                    if (strChk == "True" )
                    {
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = "DELETE KOSMOS_PMPA.BAS_MSELF WHERE ROWID = '" + strROWID + "' ";
                            strGubun = "D";
                        }
                    }
                    else if (strChange == "Y")
                    {
                        if (strROWID == "" && strSuCode != "")
                        {
                            if (strGubunA == "D" && strGubunB == "1")
                            {
                                SQL = "";
                                SQL = "INSERT INTO KOSMOS_PMPA.BAS_MSELF (SuCode,GubunA,GubunB,FieldA,FieldB,FieldC,";
                                SQL = SQL + ComNum.VBLF + "EntDate,Remark) VALUES ('" + strSuCode + "','" + strGubunA + "',";
                                SQL = SQL + ComNum.VBLF + " '" + strGubunB + "','" + strFieldA + "','" + strFieldB + "','" + strFieldC + "',";
                                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strDate + "','YYYY-MM-DD') ,'" + strRemark + "' ) ";
                                strGubun = "I";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "INSERT INTO KOSMOS_PMPA.BAS_MSELF (SuCode,GubunA,GubunB,FieldA,FieldB,FieldC,";
                                SQL = SQL + ComNum.VBLF + "EntDate) VALUES ('" + strSuCode + "','" + strGubunA + "',";
                                SQL = SQL + ComNum.VBLF + "'" + strGubunB + "','" + strFieldA + "','" + strFieldB + "','" + strFieldC + "',";
                                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
                                strGubun = "I";
                            }
                        }
                        else if (strROWID != "")
                        {
                            if (strGubunA == "D" && strGubunB == "1")
                            {
                                SQL = "";
                                SQL = "UPDATE KOSMOS_PMPA.BAS_MSELF SET FieldA='" + strFieldA + "',";
                                SQL = SQL + ComNum.VBLF + " Remark ='" + strRemark + "', ";
                                SQL = SQL + ComNum.VBLF + "FieldB='" + strFieldB + "',";
                                SQL = SQL + ComNum.VBLF + "FieldC='" + strFieldC + "',";
                                SQL = SQL + ComNum.VBLF + "EntDate=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                                strGubun = "U";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE KOSMOS_PMPA.BAS_MSELF SET FieldA='" + strFieldA + "',";
                                SQL = SQL + ComNum.VBLF + "FieldB='" + strFieldB + "',";
                                SQL = SQL + ComNum.VBLF + "FieldC='" + strFieldC + "',";
                                SQL = SQL + ComNum.VBLF + "EntDate=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                                strGubun = "U";
                            }
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
                            return;
                        }

                        switch (strGubun)
                        {
                            case "U":
                                SQL = "";
                                SQL = " INSERT INTO KOSMOS_PMPA.BAS_MSELF_HISTORY(";
                                SQL = SQL + ComNum.VBLF + " SUCODE, GUBUNA, GUBUNB, FIELDA, FIELDB, ENTDATE, CHKNO, REMARK, FIELDC, GUBUN, HDATE)  ";
                                SQL = SQL + ComNum.VBLF + "SELECT SUCODE, GUBUNA, GUBUNB, FIELDA, FIELDB, ENTDATE, CHKNO, REMARK, FIELDC, '" + strGubun + "', SYSDATE";
                                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_MSELF";
                                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                break;
                            case "D":
                                SQL = "";
                                SQL = " INSERT INTO KOSMOS_PMPA.BAS_MSELF_HISTORY(";
                                SQL = SQL + ComNum.VBLF + " SUCODE, GUBUNA, GUBUNB, FIELDA, FIELDB, ENTDATE, CHKNO, REMARK, FIELDC, GUBUN, HDATE)  ";
                                SQL = SQL + ComNum.VBLF + "SELECT SUCODE, GUBUNA, GUBUNB, FIELDA, FIELDB, ENTDATE, CHKNO, REMARK, FIELDC, '" + strGubun + "', SYSDATE";
                                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_MSELF";
                                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                    return;
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
            strHead1 = "/n" + "/l/f1" + VB.Space(16) + "제한사항 코드집(BAS_MSELF)" + "/n";
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
                SQL = "SELECT a.SuCode,a.FieldA,a.FieldB,a.FieldC,TO_CHAR(a.EntDate,'YYYY-MM-DD') EntDate,a.ChkNo,a.Remark,";
                SQL = SQL + ComNum.VBLF + "      a.ROWID,b.SuNameK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_MSELF a,KOSMOS_PMPA.BAS_SUN b ";
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
                       VB.Left(cboJob.Text, 2) == "82" || VB.Left(cboJob.Text, 2) == "09")
                    {
                        SQL = "";
                        SQL = "SELECT SuNameK FROM KOSMOS_PMPA.BAS_SUN ";
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
                    if (VB.Left(cboJob.Text, 2) == "C1")
                    {
                        SQL = "";
                        SQL = "SELECT SuNameK FROM KOSMOS_PMPA.BAS_SUN ";
                        SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + dt.Rows[i]["FieldB"].ToString().Trim() + "' ";
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
                    if (VB.Left(cboJob.Text, 2) == "D1")
                    {
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = "";
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ChkNo"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["FieldC"].ToString().Trim();
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

        private void cboJob_Click(object sender, EventArgs e)
        {
            
        }

        private void cboJob_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { SendKeys.Send("{Tab}"); }
        }

        private void frmMSelf_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR ();

            ssView_Sheet1.Columns[6].Visible = false;
            ssView_Sheet1.Columns[7].Visible = false;
            ssView_Sheet1.Columns[8].Visible = false;
            ssView_Sheet1.Columns[9].Visible = false;

            //작업종류를 ComboBox에 SET
            cboJob.Items.Clear();
            cboJob.Items.Add("08.약제과 연령금기(약제과관리)");
            cboJob.Items.Add("09.DUR 병용금기(약제과관리)");
            cboJob.Items.Add("10.DUR 연령금기(약제과관리)");
            cboJob.Items.Add("11.xx세이하는 사용금지(심사청구)");
            cboJob.Items.Add("12.xx세이상은 사용금지(심사청구)");
            cboJob.Items.Add("21.특정과만 급여");
            cboJob.Items.Add("22.특정과는 비급여");
            cboJob.Items.Add("23.특정과 사용불가");
            cboJob.Items.Add("32.동일성분 n종만 급여,나머지 비급여");
            cboJob.Items.Add("41.특정상병만 급여");
            cboJob.Items.Add("42.특정상병은 총액");
            cboJob.Items.Add("43.외래 OCS 수가별 상병체크");
            cboJob.Items.Add("52.특정환자종류,특정상병에 비급여");
            cboJob.Items.Add("53.보호환자,비급여");
            cboJob.Items.Add("62.HD환자 필수약제");
            cboJob.Items.Add("71.남자 특정과 비급여");
            cboJob.Items.Add("72.여자 특정과 비급여");
            cboJob.Items.Add("80.동시처방불가(검사)-심사");
            cboJob.Items.Add("81.동시처방불가(약제)-심사");
            cboJob.Items.Add("82.배수함량코드");
            cboJob.Items.Add("83.연령금기(미만)(심사청구)");
            cboJob.Items.Add("84.외래 OCS 1회처방 일수 제한");
            cboJob.Items.Add("85.외래 OCS 1일 처방당 일용량(갯수)제한");
            cboJob.Items.Add("86.외래/입원 OCS 분할처방 금지");
            cboJob.Items.Add("87.외래 OCS 기간별 갯수제한");
            cboJob.Items.Add("88.외래 OCS 총 투여일수 제한");
            cboJob.Items.Add("A2.약국전송 제외");
            cboJob.Items.Add("C1.청구수가변환");
            cboJob.Items.Add("D1.외래처방 상병제한");
            cboJob.Items.Add("J1.진단초음파수가");

            if (clsPublic.GnJobSabun == 2186)
            {
                cboJob.SelectedIndex = 13;
            }
            else
            {
                cboJob.SelectedIndex = 0;
            }
        }

        private void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strData = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strDEPTCODE = "";

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
                        SQL = "SELECT SuNameK FROM KOSMOS_PMPA.BAS_SUN ";
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

                if((VB.Left(cboJob.Text, 2) == "81" || VB.Left(cboJob.Text, 2) == "82")&& e.Column == 2)
                {
                    strData = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();
                    SQL = "";
                    SQL = "SELECT SuNameK FROM KOSMOS_PMPA.BAS_SUN ";
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
                        ssView_Sheet1.Cells[e.Row, 5].Text = ssView_Sheet1.Cells[e.Row, 5].Text + "/" +dt.Rows[0]["SuNameK"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }

                if(VB.Left(cboJob.Text, 2) == "C1" && e.Column == 3)
                {
                    strData = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();
                    SQL = "";
                    SQL = "SELECT SuNameK FROM KOSMOS_PMPA.BAS_SUN ";
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

                //2020-03-31 진료과 추가 작업
                if (VB.Left(cboJob.Text, 2) == "81" && e.Column == 3)
                {
                    strDEPTCODE = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();

                    if(strDEPTCODE == "")
                    {
                        ssView_Sheet1.Cells[e.Row, e.Column].Text = "**";
                    }
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strROWID = "";
            string strTemp = "";
            string strGbn = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (e.Column != 9) { return; }

            strGbn = VB.Left(cboJob.Text, 2).Trim();

            //해당 항목만 갱신 수행
            switch (strGbn)
            {
                case "81":  //병용금기
                    break;
                default:
                    ComFunc.MsgBox("이 항목은 제외할 수 없습니다.");
                    return;
            }

            strTemp = "";

            strROWID = ssView_Sheet1.Cells[e.Row, 6].Text;
            strTemp = ssView_Sheet1.Cells[e.Row, 8].Text.Trim();

            if(strTemp == "X")
            {
                strTemp = "";
                ssView_Sheet1.Cells[e.Row, 8].Text = "";
            }
            else
            {
                strTemp = "X";
                ssView_Sheet1.Cells[e.Row, 8].Text = "X";
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (strROWID != "")
                {
                    SQL = "";
                    SQL = " UPDATE KOSMOS_PMPA.BAS_MSELF SET ChkNO = '" + strTemp + "'  ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                    Cursor.Current = Cursors.Default;
                }
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnBY_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira01.FRM
            frmHira01 frmHira01 = new frmHira01();
            frmHira01.Show();
            //FrmHira01.Show 1
        }

        private void btnAge_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira02.FRM
            frmHira02 frmHira02 = new frmHira02();
            frmHira02.Show();
            //FrmHira02.Show 1
        }

        private void btnStability_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira03.FRM
            //FrmHira03.Show 1
            frmHira03 frmHira03 = new frmHira03();
            frmHira03.Show();
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira04.FRM
            //FrmHira04.Show 1
            frmHira04 frmHira04 = new frmHira04();
            frmHira04.Show();
        }

        private void btnLow_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira06.FRM
            //FrmHira06.Show 1
            frmHira06 frmHira06 = new frmHira06();
            frmHira06.Show();
        }

        private void btnImBu_Click(object sender, EventArgs e)
        {
            //C:\V60_NEW\BASIC\BUSUGA\FrmHira07.FRM
            //FrmHira07.Show 1
            frmHira07 frmHira07 = new frmHira07();
            frmHira07.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmNotSendIllOPD frm = new frmNotSendIllOPD();
            frm.Show();
        }

        private void cboJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sheet_Title_SET();
        }

        private void ssView_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strData = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            string strDEPTCODE = "";

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
                        SQL = "SELECT SuNameK FROM KOSMOS_PMPA.BAS_SUN ";
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
                            if(ssView_Sheet1.Cells[e.Row, 5].Text == "")
                            {
                                ssView_Sheet1.Cells[e.Row, 5].Text = dt.Rows[0]["SuNameK"].ToString().Trim();
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }

                if ((VB.Left(cboJob.Text, 2) == "81" || VB.Left(cboJob.Text, 2) == "82") && e.Column == 2)
                {
                    strData = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();
                    SQL = "";
                    SQL = "SELECT SuNameK FROM KOSMOS_PMPA.BAS_SUN ";
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
                        if(ssView_Sheet1.Cells[e.Row, 5].Text.Contains("/"))
                        {

                        }
                        else
                        {
                            ssView_Sheet1.Cells[e.Row, 5].Text = ssView_Sheet1.Cells[e.Row, 5].Text + "/" + dt.Rows[0]["SuNameK"].ToString().Trim();
                        }
                        
                    }
                    dt.Dispose();
                    dt = null;
                }

                if (VB.Left(cboJob.Text, 2) == "C1" && e.Column == 3)
                {
                    strData = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();
                    SQL = "";
                    SQL = "SELECT SuNameK FROM KOSMOS_PMPA.BAS_SUN ";
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

                //2020-03-31 진료과 추가 작업
                //if (VB.Left(cboJob.Text, 2) == "81" && e.Column == 3)
                //{
                //    strDEPTCODE = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();

                //    if (strDEPTCODE == "")
                //    {
                //        ssView_Sheet1.Cells[e.Row, e.Column].Text = "**";
                //    }
                //}
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
