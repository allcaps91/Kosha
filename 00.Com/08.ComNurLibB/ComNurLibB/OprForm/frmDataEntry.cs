using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmDataEntry
    /// Description     : 수술 현황판
    /// Author          : 김효성
    /// Create Date     : 2017-12-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// SMS 기능 삭제
    /// </history>
    /// <seealso cref= "D:\psmh\Ocs\oproom\opguide\frmDataEntry >> frmDataEntry.cs 폼이름 재정의" />

    public partial class frmDataEntry : Form
    {
        string strNowDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        int[,] mintRGB = null;
        string mstrOpTop = "";


        public frmDataEntry()
        {
            InitializeComponent();
        }

        public frmDataEntry(string strOpTop)
        {
            InitializeComponent();

            mstrOpTop = strOpTop; // 수녀님을 위해 만듬!!
        }


        private void frmDataEntry_Load(object sender, EventArgs e)
        {
            dtpNowDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            timer1.Enabled = false;

            DataColor_READ();

            GetData();

            if (mstrOpTop == "조회전용")
            {
                panTitleSub1.Visible = false;
                panel4.Visible = false;
                ssView2.Visible = false;
                panel3.Visible = false;
                panel2.Visible = false;
                timer1.Enabled = true;

                this.Width = ssView.Width;
            }

            
        }

        private void GetData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT A.ROOM1, A.SNAME, A.DEPTCODE, A.GUBUN, A.TOROOM,      ";
                SQL = SQL + ComNum.VBLF + "            A.TELNO, A.SMS_GB, A.OPROOM, B.DEPTNAMEK, C.NAME AS FLAGNAME, A.ROOM AS SEQ    ";
                SQL = SQL + ComNum.VBLF + "            , A.ROWID                                                                        ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.ORAN_GUIDE A     ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN BAS_CLINICDEPT B     ";
                SQL = SQL + ComNum.VBLF + "    ON A.DEPTCODE = B.DEPTCODE     ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN BAS_BCODE C     ";
                SQL = SQL + ComNum.VBLF + "    ON A.GUBUN = C.CODE     ";
                SQL = SQL + ComNum.VBLF + "    AND C.GUBUN = 'OP_상황판'     ";
                SQL = SQL + ComNum.VBLF + "WHERE A.DELDATE IS NULL    ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SNAME     ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ROOM1"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["TOROOM"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["FLAGNAME"].ToString().Trim();

                        ssView_Sheet1.Cells.Get(i, 5, i, 6).BackColor = System.Drawing.Color.FromArgb(mintRGB[Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 5].Text)) + 2, 1]
                                                                                                    , mintRGB[Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 5].Text)) + 2, 2]
                                                                                                    , mintRGB[Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 5].Text)) + 2, 3]);

                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["TELNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["OPROOM"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["SEQ"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10].Tag = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 11].Text = "";
                    }

                    if (mstrOpTop == "조회전용")
                    {
                        ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Locked = true;
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Save() == true)
            {
                GetData();
            }
        }

        private bool Save()
        {
            //bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (ssView_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        #region 기존 로직
                        //SQL = "";
                        //SQL = "DELETE KOSMOS_PMPA.ORAN_GUIDE     ";
                        //SQL = SQL + ComNum.VBLF + "WHERE ROOM = " + ssView_Sheet1.Cells[i, 10].Text.Trim() + "    ";
                        #endregion

                        #region 신규 로직(2021-04-30)
                        SQL = "";
                        SQL = "UPDATE KOSMOS_PMPA.ORAN_GUIDE     ";
                        SQL = SQL + ComNum.VBLF + " SET                                                           ";
                        SQL = SQL + ComNum.VBLF + " DELDATE = SYSDATE                                             ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + ssView_Sheet1.Cells[i, 10].Tag.ToString().Trim() + "'    ";
                        #endregion

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                    else
                    {
                        if (ssView_Sheet1.Cells[i, 2].Text.Trim() != "" && ssView_Sheet1.Cells[i, 11].Text.Trim() == "Y")
                        {
                            if (ssView_Sheet1.Cells[i, 10].Text.Trim() == "")
                            {
                                SQL = "";
                                SQL = "INSERT INTO KOSMOS_PMPA.ORAN_GUIDE       ";
                                SQL = SQL + ComNum.VBLF + "       (SNAME, DEPTCODE, GUBUN, ENTDATE, TOROOM, TELNO, OPROOM, ROOM1)      ";
                                SQL = SQL + ComNum.VBLF + " VALUES      ";
                                SQL = SQL + ComNum.VBLF + "(        ";
                                SQL = SQL + ComNum.VBLF + "'" + ssView_Sheet1.Cells[i, 2].Text.Trim() + "',";  //SNAME
                                SQL = SQL + ComNum.VBLF + "'" + ssView_Sheet1.Cells[i, 3].Text.Trim() + "',";  //DEPTCODE
                                SQL = SQL + ComNum.VBLF + "'" + ssView_Sheet1.Cells[i, 5].Text.Trim() + "',";  //GUBUN
                                SQL = SQL + ComNum.VBLF + "SYSDATE,";  //ENTDATE
                                SQL = SQL + ComNum.VBLF + "'" + ssView_Sheet1.Cells[i, 4].Text.Trim() + "',";  //TOROOM
                                SQL = SQL + ComNum.VBLF + "'" + ssView_Sheet1.Cells[i, 8].Text.Trim() + "',";  //TELNO
                                SQL = SQL + ComNum.VBLF + "'" + ssView_Sheet1.Cells[i, 9].Text.Trim() + "',";  //OPROOM
                                SQL = SQL + ComNum.VBLF + "'" + ssView_Sheet1.Cells[i, 1].Text.Trim() + "'  ";  //ROOM1
                                SQL = SQL + ComNum.VBLF + ")        ";

                                //SQL = SQL + ComNum.VBLF + ssView_Sheet1.Cells[i, 10].Text.Trim();  //ROOM       
                                //SQL = SQL + ComNum.VBLF + "(SELECT DECODE((SELECT MAX(ROOM) AS SEQ FROM ORAN_GUIDE),'',0, (SELECT MAX(ROOM) AS SEQ FROM ORAN_GUIDE) + 1) AS SEQ FROM DUAL))";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE KOSMOS_PMPA.ORAN_GUIDE        ";
                                SQL = SQL + ComNum.VBLF + "SET       ";
                                SQL = SQL + ComNum.VBLF + "    SNAME = '" + ssView_Sheet1.Cells[i, 2].Text.Trim() + "',       ";
                                SQL = SQL + ComNum.VBLF + "    DEPTCODE = '" + ssView_Sheet1.Cells[i, 3].Text.Trim() + "',       ";
                                SQL = SQL + ComNum.VBLF + "    GUBUN = '" + ssView_Sheet1.Cells[i, 5].Text.Trim() + "',        ";
                                SQL = SQL + ComNum.VBLF + "    ENTDATE = SYSDATE,        ";
                                SQL = SQL + ComNum.VBLF + "    TOROOM = '" + ssView_Sheet1.Cells[i, 4].Text.Trim() + "',        ";
                                SQL = SQL + ComNum.VBLF + "    TELNO = '" + ssView_Sheet1.Cells[i, 8].Text.Trim() + "',        ";
                                SQL = SQL + ComNum.VBLF + "    OPROOM = '" + ssView_Sheet1.Cells[i, 9].Text.Trim() + "',        ";
                                SQL = SQL + ComNum.VBLF + "    ROOM1 = '" + ssView_Sheet1.Cells[i, 1].Text.Trim() + "'       ";
                                //SQL = SQL + ComNum.VBLF + "WHERE ROOM =  " + ssView_Sheet1.Cells[i, 10].Text.Trim() + "     ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + ssView_Sheet1.Cells[i, 10].Tag.ToString().Trim() + "'    ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

            return false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
            ssView2Date();
        }

        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            int intRow = ssView_Sheet1.ActiveRowIndex;
            int intCol = ssView_Sheet1.ActiveColumnIndex;

            if (ssView_Sheet1.Cells[intRow, intCol].Text.Trim() != "")
            {
                ssView_Sheet1.Cells[intRow, 11].Text = "Y";
            }

            if (intCol == 3)
            {

                ssView_Sheet1.Cells[intRow, intCol].Text = ssView_Sheet1.Cells[intRow, intCol].Text.Trim().ToUpper();

                switch (ssView_Sheet1.Cells[intRow, intCol].Text.Trim())
                {
                    case "MD":
                        ssView_Sheet1.Cells[intRow, 7].Text = "내과";
                        break;
                    case "GS":
                        ssView_Sheet1.Cells[intRow, 7].Text = "외과";
                        break;
                    case "OG":
                        ssView_Sheet1.Cells[intRow, 7].Text = "산부인과";
                        break;
                    case "PD":
                        ssView_Sheet1.Cells[intRow, 7].Text = "소아과";
                        break;
                    case "OS":
                        ssView_Sheet1.Cells[intRow, 7].Text = "정형외과";
                        break;
                    case "NS":
                        ssView_Sheet1.Cells[intRow, 7].Text = "신경외과";
                        break;
                    case "CS":
                        ssView_Sheet1.Cells[intRow, 7].Text = "흉부외과";
                        break;
                    case "NP":
                        ssView_Sheet1.Cells[intRow, 7].Text = "정신과";
                        break;
                    case "EN":
                        ssView_Sheet1.Cells[intRow, 7].Text = "이비인후과";
                        break;
                    case "OT":
                        ssView_Sheet1.Cells[intRow, 7].Text = "안과";
                        break;
                    case "UR":
                        ssView_Sheet1.Cells[intRow, 7].Text = "비뇨기과";
                        break;
                    case "DM":
                        ssView_Sheet1.Cells[intRow, 7].Text = "피부과";
                        break;
                    case "DT":
                        ssView_Sheet1.Cells[intRow, 7].Text = "치과";
                        break;
                    case "ER":
                        ssView_Sheet1.Cells[intRow, 7].Text = "응급실";
                        break;
                    default:
                        ssView_Sheet1.Cells[intRow, 7].Text = "";
                        break;
                }
            }
            else if (intCol == 5)
            {
                if (Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[intRow, 5].Text)) > 7 || Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[intRow, 5].Text)) < 1)
                {
                    ssView_Sheet1.Cells[intRow, 5].Text = "1";
                }

                ssView_Sheet1.Cells[intRow, 6].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "OP_상황판", ssView_Sheet1.Cells[intRow, intCol].Text.Trim());
                ssView_Sheet1.Cells.Get(intRow, 5, intRow, 6).BackColor = System.Drawing.Color.FromArgb(mintRGB[Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[intRow, 5].Text)) + 2, 1]
                                                                                                    , mintRGB[Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[intRow, 5].Text)) + 2, 2]
                                                                                                    , mintRGB[Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[intRow, 5].Text)) + 2, 3]);
            }
            else if (intCol == 1)
            {
                ssView_Sheet1.Cells[intRow, intCol].Text = ssView_Sheet1.Cells[intRow, intCol].Text.Trim().ToUpper();
            }
            else if (intCol == 4)
            {
                ssView_Sheet1.Cells[intRow, intCol].Text = ssView_Sheet1.Cells[intRow, intCol].Text.Trim().ToUpper();
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int intDate = 0;

            if (mstrOpTop == "조회전용") return;

            if (e.RowHeader == true) return;

            if (e.Column == 6)
            {
                intDate = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[e.Row, 5].Text.Trim())) + 1;

                if (intDate > 7 || intDate < 1)
                {
                    ssView_Sheet1.Cells[e.Row, 5].Text = "1";
                    ssView_Sheet1.Cells[e.Row, 6].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "OP_상황판", ssView_Sheet1.Cells[e.Row, 5].Text.Trim());
                }
                else
                {
                    ssView_Sheet1.Cells[e.Row, 5].Text = Convert.ToString(intDate);
                    ssView_Sheet1.Cells[e.Row, 6].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "OP_상황판", ssView_Sheet1.Cells[e.Row, 5].Text.Trim());
                }

                ssView_Sheet1.Cells.Get(e.Row, 5, e.Row, 6).BackColor = System.Drawing.Color.FromArgb(mintRGB[Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[e.Row, 5].Text)) + 2, 1]
                                                                                                    , mintRGB[Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[e.Row, 5].Text)) + 2, 2]
                                                                                                    , mintRGB[Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[e.Row, 5].Text)) + 2, 3]);

                UpdateGubun(ssView_Sheet1.Cells[e.Row, 5].Text.Trim(), ssView_Sheet1.Cells[e.Row, 10].Tag.ToString().Trim());
                          //, ssView_Sheet1.Cells[e.Row, 2].Text.Trim()
                          //, ssView_Sheet1.Cells[e.Row, 3].Text.Trim());
                GetData();
            }
        }

        private void UpdateGubun(string strGubun, string ROWID)
        {

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //SQL = "";
                //SQL = " UPDATE KOSMOS_PMPA.ORAN_GUIDE SET ";
                //SQL = SQL + ComNum.VBLF + "  GUBUN ='" + strGubun + "' ";
                //SQL = SQL + ComNum.VBLF + "  WHERE TRIM(SNAME) ='" + strSName + "' ";
                //SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDeptCode + "' ";
                //SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                SQL = "";
                SQL = " UPDATE KOSMOS_PMPA.ORAN_GUIDE SET ";
                SQL = SQL + ComNum.VBLF + "  GUBUN ='" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID ='" + ROWID + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

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

        private void ssView2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = ssView2_Sheet1.Cells[e.Row, 4].Text.Trim();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = ssView2_Sheet1.Cells[e.Row, 1].Text.Trim();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = ssView2_Sheet1.Cells[e.Row, 2].Text.Trim();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = ssView2_Sheet1.Cells[e.Row, 4].Text.Trim();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = "1";

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "OP_상황판", ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text.Trim());

            switch (ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text.Trim())
            {
                case "MD":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "내과";
                    break;
                case "GS":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "외과";
                    break;
                case "OG":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "산부인과";
                    break;
                case "PD":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "소아과";
                    break;
                case "OS":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "정형외과";
                    break;
                case "NS":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "신경외과";
                    break;
                case "CS":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "흉부외과";
                    break;
                case "NP":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "정신과";
                    break;
                case "EN":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "이비인후";
                    break;
                case "OT":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "안과";
                    break;
                case "UR":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "비뇨기과";
                    break;
                case "DM":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "피부과";
                    break;
                case "DT":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "치과";
                    break;
                case "ER":
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "응급실";
                    break;
                default:
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "";
                    break;
            }

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = ssView2_Sheet1.Cells[e.Row, 3].Text.Trim();
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "Y";


        }

        private void dtpNowDate_ValueChanged(object sender, EventArgs e)
        {
            ssView2Date();
        }

        private void ssView2Date()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView2_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //SQL = "";
                //SQL = "SELECT A.PANO, A.SNAME, A.DEPTCODE, A.OPROOM, I.ROOMCODE ,B.OPETIME      ";
                //SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_OPSCHE A      ";
                //SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.IPD_NEW_MASTER I      ";
                //SQL = SQL + ComNum.VBLF + "    ON A.PANO = I.PANO      ";
                //SQL = SQL + ComNum.VBLF + "    AND I.OUTDATE IS NULL      ";
                //SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.ORAN_MASTER B      ";
                //SQL = SQL + ComNum.VBLF + "    ON A.WRTNO = B.WRTNO      ";
                //SQL = SQL + ComNum.VBLF + "    AND A.OPDATE = B.OPDATE      ";
                ////SQL = SQL + ComNum.VBLF + "    AND A.GBANGIO = B.GBANGIO      "; // 스케쥴 테이블 하고 마스터 테이블 하고 데이터가 달라서 안됨.
                //SQL = SQL + ComNum.VBLF + "    AND A.OPROOM = B.OPROOM      ";
                //SQL = SQL + ComNum.VBLF + "    AND A.OPDATE = B.OPDATE      ";
                //SQL = SQL + ComNum.VBLF + "WHERE A.OPDATE = TO_DATE('" + dtpNowDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')      ";
                //SQL = SQL + ComNum.VBLF + "    AND(A.GBANGIO IS NULL OR A.GBANGIO = 'N')      ";
                //SQL = SQL + ComNum.VBLF + "    AND A.OPROOM <> '*'      ";
                //SQL = SQL + ComNum.VBLF + "    AND A.OPROOM <> 'N'      ";
                //SQL = SQL + ComNum.VBLF + "ORDER BY A.SNAME      ";


                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.PANO, A.SNAME, A.DEPTCODE, A.OPROOM, I.ROOMCODE, B.OPETIME     ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_OPSCHE A     ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.ORAN_MASTER B     ";
                SQL = SQL + ComNum.VBLF + "    ON A.WRTNO = B.WRTNO     ";
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO      ";
                SQL = SQL + ComNum.VBLF + "    AND A.OPDATE = B.OPDATE     ";
                SQL = SQL + ComNum.VBLF + "    AND (B.OPETIME IS NULL OR B.OPETIME = ':')   ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.IPD_NEW_MASTER I     ";
                SQL = SQL + ComNum.VBLF + "    ON A.PANO = I.PANO     ";
                SQL = SQL + ComNum.VBLF + "    AND I.OUTDATE IS NULL     ";
                SQL = SQL + ComNum.VBLF + "WHERE A.OPDATE = TO_DATE('" + dtpNowDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')     ";
                SQL = SQL + ComNum.VBLF + "    AND(A.GBANGIO IS NULL OR A.GBANGIO = 'N')     ";
                SQL = SQL + ComNum.VBLF + "    AND A.OPROOM <> '*'     ";
                SQL = SQL + ComNum.VBLF + "    AND A.OPROOM <> 'N'     ";

                SQL = SQL + ComNum.VBLF + "ORDER BY A.SNAME     ";


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    ssView2_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["OPROOM"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["ROOMCODE"].ToString().Trim() == "" ? "OPD" : dt.Rows[i]["ROOMCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void DataColor_READ()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            mintRGB = null;
            mintRGB = new int[14, 4];

            try
            {
                SQL = "";
                SQL = "SELECT CODE, NAME, GUBUN2 AS R, GUBUN3 AS G, GUBUN4 AS B     ";
                SQL = SQL + ComNum.VBLF + "FROM BAS_BCODE       ";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = 'OP_상황판_색'     ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        mintRGB[Convert.ToInt32(dt.Rows[i]["CODE"].ToString().Trim()), 1] = Convert.ToInt32(dt.Rows[i]["R"].ToString().Trim());
                        mintRGB[Convert.ToInt32(dt.Rows[i]["CODE"].ToString().Trim()), 2] = Convert.ToInt32(dt.Rows[i]["G"].ToString().Trim());
                        mintRGB[Convert.ToInt32(dt.Rows[i]["CODE"].ToString().Trim()), 3] = Convert.ToInt32(dt.Rows[i]["B"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            GetData();

            timer1.Enabled = true;
        }

        private void ssView_EditModeOn(object sender, EventArgs e)
        {
            int row = ssView.ActiveSheet.ActiveRowIndex;
            int col = ssView.ActiveSheet.ActiveColumnIndex;

            switch (col)
            {
                case 2: // 이름
                    ssView.ActiveSheet.Cells[row, col].ImeMode = ImeMode.Hangul;
                    break;
                case 3: // 과
                    ssView.ActiveSheet.Cells[row, col].ImeMode = ImeMode.Alpha;
                    break;
            }            
        }
    }
}
