using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmNearMissReport.cs
    /// Description     : 근접오류 보고서
    /// Author          : 이현종
    /// Create Date     : 2018-08-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\etc\kms\Frm근접오류보고서(Frm근접오류보고서.frm) >> frmNearMissReport.cs 폼이름 재정의" />
    public partial class frmNearMissReport : Form
    {
        string strDepartment = string.Empty;
        string strRowid  = string.Empty;
        string FstrSabun = string.Empty;
        string FstrBuse = string.Empty;

        public frmNearMissReport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// QI전용
        /// </summary>
        /// <param name="strDepartment">적정관리실</param>
        /// <param name="strRowid">ROWID</param>
        public frmNearMissReport(string strDepartment, string strRowid)
        {
            InitializeComponent();
            this.strDepartment = strDepartment;
            this.strRowid = strRowid;
        }

        private void frmNearMissReport_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssList_Sheet1.RowCount = 0;

            if (strDepartment == "적정관리실")
            {
                label1.Visible = false;
                label2.Visible = false;
                READ_DATA(strRowid);
            }
            else
            {
                btnSearch.PerformClick();
                btnNew.PerformClick();
                //GetSearchData();
            }
        }

        void SCREEN_CLEAR()
        {
            //ss1_Sheet1.Cells[3, 4].Value = false;

            ss1_Sheet1.Cells[4, 2].Text = "";
            ss1_Sheet1.Cells[4, 7].Text = "";
            ss1_Sheet1.Cells[4, 13].Text = "";
            ss1_Sheet1.Cells[4, 20].Text = "";

            ss1_Sheet1.Cells[7, 1].Value = false;
            ss1_Sheet1.Cells[7, 3].Value = false;
            ss1_Sheet1.Cells[7, 5].Value = false;
            ss1_Sheet1.Cells[7, 7].Value = false;
            ss1_Sheet1.Cells[7, 12].Value = false;
            ss1_Sheet1.Cells[7, 15].Value = false;
            ss1_Sheet1.Cells[7, 18].Value = false;
            ss1_Sheet1.Cells[7, 20].Text = "";


            ss1_Sheet1.Cells[8, 1].Text = "";
            ss1_Sheet1.Cells[8, 15].Text = "";


            ss1_Sheet1.Cells[11, 1] .Value = false;
            ss1_Sheet1.Cells[11, 4] .Value = false;
            ss1_Sheet1.Cells[11, 8] .Value = false;
            ss1_Sheet1.Cells[11, 11].Value = false;
            ss1_Sheet1.Cells[11, 16].Value = false;
            ss1_Sheet1.Cells[11, 21].Value = false;

            ss1_Sheet1.Cells[12, 1].Value = false;
            ss1_Sheet1.Cells[12, 4].Value = false;
            ss1_Sheet1.Cells[12, 8].Value = false;
            ss1_Sheet1.Cells[12, 11].Value = false;
            ss1_Sheet1.Cells[12, 16].Value = false;
            ss1_Sheet1.Cells[12, 19].Value = false;

            ss1_Sheet1.Cells[13, 1].Value = false;
            ss1_Sheet1.Cells[13, 4].Value = false;
            ss1_Sheet1.Cells[13, 8].Value = false;
            ss1_Sheet1.Cells[13, 10].Text = "";

            ss1_Sheet1.Cells[14, 6].Text = "";
            ss1_Sheet1.Cells[14, 8].Text = "";

            ss1_Sheet1.Cells[15, 1].Text = "";
            ss1_Sheet1.Cells[16, 1].Text = "";
            ss1_Sheet1.Cells[17, 1].Text = "";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (strDepartment == "적정관리실")
            {
                ComFunc.MsgBox("관리프로그램으로 해당 보고서를 열었을 경우 신규 작성이 제한 됩니다.");
                return;
            }

            strRowid = "";
            SCREEN_CLEAR();

            ss1_Sheet1.Cells[4, 2].Text = clsType.User.UserName;
            ss1_Sheet1.Cells[4, 7].Text = clsType.User.Sabun;
            ss1_Sheet1.Cells[4, 13].Text = clsVbfunc.GetBASBuSe(clsDB.DbCon, clsType.User.BuseCode);

            ss1_Sheet1.Cells[4, 20].Text = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("yyyy-MM-dd HH:mm");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();

            ssList_Sheet1.RowCount = 0;

            if (strDepartment == "적정관리실")
            {
                READ_DATA(strRowid);
                return;
            }

            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT TO_CHAR(RDATE, 'YYYY-MM-DD HH24:MI') RDATE, ROWID ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_SAFETY_REPORT2 ";
                SQL += ComNum.VBLF + " WHERE WSABUN = " + clsType.User.Sabun;
                SQL += ComNum.VBLF + " ORDER BY WDATE DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Rowid"].ToString().Trim();
                }


                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void READ_DATA(string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SCREEN_CLEAR();

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT SEQNO, WDate, WSABUN, WBUSE,";
                SQL += ComNum.VBLF + "  TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') RDATE, TARGET1, TARGET2, TARGET3,";
                SQL += ComNum.VBLF + "  TARGET4, TARGET5, TARGET6, TARGET7,";
                SQL += ComNum.VBLF + "  TARGET8, TO_CHAR(BALDATE,'YYYY-MM-DD HH24:MI') BALDATE, BALPLACE, ACCI01,";
                SQL += ComNum.VBLF + "  ACCI02, ACCI03, ACCI04, ACCI05,";
                SQL += ComNum.VBLF + "  ACCI06, ACCI07, ACCI08, ACCI09,";
                SQL += ComNum.VBLF + "  ACCI10, ACCI11, ACCI12, ACCI13,";
                SQL += ComNum.VBLF + "  ACCI14, ACCI15, ACCI16, ACCI17,";
                SQL += ComNum.VBLF + "  RESULT1, RESULT2, REPORT1, REPORT2,";
                SQL += ComNum.VBLF + "  REPORT3, ANONY";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_SAFETY_REPORT2 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                string strWDATE = dt.Rows[0]["WDate"].ToString().Trim();
                string strWSABUN = dt.Rows[0]["WSABUN"].ToString().Trim();
                string strWBUSE = dt.Rows[0]["WBUSE"].ToString().Trim();
                string StrRDate = dt.Rows[0]["RDate"].ToString().Trim();
                string strTARGET1 = dt.Rows[0]["TARGET1"].ToString().Trim();
                string strTARGET2 = dt.Rows[0]["TARGET2"].ToString().Trim();
                string strTARGET3 = dt.Rows[0]["TARGET3"].ToString().Trim();
                string strTARGET4 = dt.Rows[0]["TARGET4"].ToString().Trim();
                string strTARGET5 = dt.Rows[0]["TARGET5"].ToString().Trim();
                string strTARGET6 = dt.Rows[0]["TARGET6"].ToString().Trim();
                string strTARGET7 = dt.Rows[0]["TARGET7"].ToString().Trim();
                string strTARGET8 = dt.Rows[0]["TARGET8"].ToString().Trim();
                string strBalDate = dt.Rows[0]["BALDATE"].ToString().Trim();
                string strBALPLACE = dt.Rows[0]["BALPLACE"].ToString().Trim();
                string strACCI01 = dt.Rows[0]["ACCI01"].ToString().Trim();
                string strACCI02 = dt.Rows[0]["ACCI02"].ToString().Trim();
                string strACCI03 = dt.Rows[0]["ACCI03"].ToString().Trim();
                string strACCI04 = dt.Rows[0]["ACCI04"].ToString().Trim();
                string strACCI05 = dt.Rows[0]["ACCI05"].ToString().Trim();
                string strACCI06 = dt.Rows[0]["ACCI06"].ToString().Trim();
                string strACCI07 = dt.Rows[0]["ACCI07"].ToString().Trim();
                string strACCI08 = dt.Rows[0]["ACCI08"].ToString().Trim();
                string strACCI09 = dt.Rows[0]["ACCI09"].ToString().Trim();
                string strACCI10 = dt.Rows[0]["ACCI10"].ToString().Trim();
                string strACCI11 = dt.Rows[0]["ACCI11"].ToString().Trim();
                string strACCI12 = dt.Rows[0]["ACCI12"].ToString().Trim();
                string strACCI13 = dt.Rows[0]["ACCI13"].ToString().Trim();
                string strACCI14 = dt.Rows[0]["ACCI14"].ToString().Trim();
                string strACCI15 = dt.Rows[0]["ACCI15"].ToString().Trim();
                string strACCI16 = dt.Rows[0]["ACCI16"].ToString().Trim();
                string strACCI17 = dt.Rows[0]["ACCI17"].ToString().Trim();
                string strResult1 = dt.Rows[0]["RESULT1"].ToString().Trim();
                string strResult2 = dt.Rows[0]["RESULT2"].ToString().Trim();
                string strREPORT1 = dt.Rows[0]["REPORT1"].ToString().Trim();
                string strREPORT2 = dt.Rows[0]["REPORT2"].ToString().Trim();
                string strREPORT3 = dt.Rows[0]["REPORT3"].ToString().Trim();
                string strAnony = dt.Rows[0]["ANONY"].ToString().Trim();

                dt.Dispose();
                dt = null;

                ss1_Sheet1.Cells[3, 4].Value = strAnony;

                FstrSabun = strWSABUN;
                FstrBuse = strWBUSE;

                if (strAnony == "1")
                {
                    ss1_Sheet1.Cells[4, 2].Text = "익명보고";
                    ss1_Sheet1.Cells[4, 7].Text = "*****";
                    ss1_Sheet1.Cells[4, 13].Text = "익명보고";
                }
                else
                {
                    ss1_Sheet1.Cells[4, 2].Text = clsVbfunc.GetInSaName(clsDB.DbCon, strWSABUN);
                    ss1_Sheet1.Cells[4, 7].Text = strWSABUN;
                    ss1_Sheet1.Cells[4, 13].Text = strWBUSE;
                }

                ss1_Sheet1.Cells[4, 20].Text = StrRDate;

                ss1_Sheet1.Cells[7, 1].Value = strTARGET1 == "1";    //환자
                ss1_Sheet1.Cells[7, 3].Value = strTARGET2 == "1";    //입원
                ss1_Sheet1.Cells[7, 5].Value = strTARGET3 == "1";    //외래
                ss1_Sheet1.Cells[7, 7].Value = strTARGET4 == "1";    //응급센터
                ss1_Sheet1.Cells[7, 12].Value = strTARGET5 == "1";   //보호자
                ss1_Sheet1.Cells[7, 15].Value = strTARGET6 == "1";   //방문객
                ss1_Sheet1.Cells[7, 18].Value = strTARGET7 == "1";   //기타
                ss1_Sheet1.Cells[7, 20].Text  = strTARGET8;   //기타(입력)

                ss1_Sheet1.Cells[8, 1].Text = strBalDate;        
                ss1_Sheet1.Cells[8, 15].Text = strBALPLACE;       


                ss1_Sheet1.Cells[11, 1].Value = strACCI01 == "1";   //수술
                ss1_Sheet1.Cells[11, 4].Value = strACCI02 == "1";   //마취진정
                ss1_Sheet1.Cells[11, 8].Value = strACCI03 == "1";   //수혈
                ss1_Sheet1.Cells[11, 11].Value = strACCI04 == "1";  //검사시술치료
                ss1_Sheet1.Cells[11, 16].Value = strACCI05 == "1";  //진단처치검사시술
                ss1_Sheet1.Cells[11, 21].Value = strACCI06 == "1";  //지연된치료

                ss1_Sheet1.Cells[12, 1].Value = strACCI07 == "1";   //감염 
                ss1_Sheet1.Cells[12, 4].Value = strACCI08 == "1";   //분만
                ss1_Sheet1.Cells[12, 8].Value = strACCI09 == "1";   //화상 
                ss1_Sheet1.Cells[12, 11].Value = strACCI10 == "1";  //의료기구
                ss1_Sheet1.Cells[12, 16].Value = strACCI11 == "1";  //자살자해
                ss1_Sheet1.Cells[12, 19].Value = strACCI12 == "1";  //탈원


                ss1_Sheet1.Cells[13, 1].Value = strACCI14 == "1";   //식사
                ss1_Sheet1.Cells[13, 4].Value = strACCI15 == "1";   //시설환경
                ss1_Sheet1.Cells[13, 8].Value = strACCI16 == "1";   //기타
                ss1_Sheet1.Cells[13, 10].Text = strACCI17;      //기타

                ss1_Sheet1.Cells[14, 6].Text = strResult1;
                ss1_Sheet1.Cells[14, 8].Text = strResult2;

                ss1_Sheet1.Cells[15, 1].Text = strREPORT1;
                ss1_Sheet1.Cells[16, 1].Text = strREPORT2;
                ss1_Sheet1.Cells[17, 1].Text = strREPORT3;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (Save_Data() == true)
            {
                if (strDepartment == "적정관리실")
                {
                    Close();
                }
                else
                {
                    //GetSearchData();
                    btnSearch.PerformClick();
                }
            }
        }

        bool Save_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strWSABUN = "";
            string strWBUSE = "";

            string StrRDate = ss1_Sheet1.Cells[4, 20].Text.Trim();
            string strTARGET1 = ss1_Sheet1.Cells[7, 1].Text.Trim()  == "True" ? "1" : "0"  ;
            string strTARGET2 = ss1_Sheet1.Cells[7, 3].Text.Trim()  == "True" ? "1" : "0"  ;
            string strTARGET3 = ss1_Sheet1.Cells[7, 5].Text.Trim()  == "True" ? "1" : "0"  ;
            string strTARGET4 = ss1_Sheet1.Cells[7, 7].Text.Trim()  == "True" ? "1" : "0";
            string strTARGET5 = ss1_Sheet1.Cells[7, 12].Text.Trim() == "True" ? "1" : "0" ;
            string strTARGET6 = ss1_Sheet1.Cells[7, 15].Text.Trim() == "True" ? "1" : "0" ;
            string strTARGET7 = ss1_Sheet1.Cells[7, 18].Text.Trim() == "True" ? "1" : "0" ;
            string strTARGET8 = ss1_Sheet1.Cells[7, 20].Text.Trim();

            string strBalDate  = ss1_Sheet1.Cells[8, 1].Text.Trim();
            string strBALPLACE = ss1_Sheet1.Cells[8, 15].Text.Trim();

            string strACCI01 = ss1_Sheet1.Cells[11, 1].Text.Trim() == "True" ? "1" : "0" ;
            string strACCI02 = ss1_Sheet1.Cells[11, 4].Text.Trim() == "True" ? "1" : "0" ;
            string strACCI03 = ss1_Sheet1.Cells[11, 8].Text.Trim() == "True" ? "1" : "0" ;
            string strACCI04 = ss1_Sheet1.Cells[11, 11].Text.Trim() == "True" ? "1" : "0" ;
            string strACCI05 = ss1_Sheet1.Cells[11, 16].Text.Trim() == "True" ? "1" : "0" ;
            string strACCI06 = ss1_Sheet1.Cells[11, 21].Text.Trim() == "True" ? "1" : "0";

            string strACCI07 = ss1_Sheet1.Cells[12, 1].Text.Trim() == "True" ? "1" : "0" ;
            string strACCI08 = ss1_Sheet1.Cells[12, 4].Text.Trim() == "True" ? "1" : "0" ;
            string strACCI09 = ss1_Sheet1.Cells[12, 8].Text.Trim() == "True" ? "1" : "0" ;
            string strACCI10 = ss1_Sheet1.Cells[12, 11].Text.Trim() == "True" ? "1" : "0" ;
            string strACCI11 = ss1_Sheet1.Cells[12, 16].Text.Trim() == "True" ? "1" : "0" ;
            string strACCI12 = ss1_Sheet1.Cells[12, 19].Text.Trim() == "True" ? "1" : "0";

            string strACCI13 = "";
            string strACCI14 = ss1_Sheet1.Cells[13, 1].Text.Trim() == "True" ? "1" : "0";
            string strACCI15 = ss1_Sheet1.Cells[13, 4].Text.Trim() == "True" ? "1" : "0";
            string strACCI16 = ss1_Sheet1.Cells[13, 8].Text.Trim() == "True" ? "1" : "0";
            string strACCI17 = ss1_Sheet1.Cells[13, 10].Text.Trim();

            string strResult1 = ss1_Sheet1.Cells[14, 6].Text.Trim() == "True" ? "1" : "0";
            string strResult2 = ss1_Sheet1.Cells[14, 8].Text.Trim() == "True" ? "1" : "0";

            string strREPORT1 = ss1_Sheet1.Cells[15, 1].Text.Trim();
            string strREPORT2 = ss1_Sheet1.Cells[16, 1].Text.Trim();
            string strREPORT3 = ss1_Sheet1.Cells[17, 1].Text.Trim();

            string strAnony = ss1_Sheet1.Cells[3, 4].Text.Trim() == "True" ? "1" : "0";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if(string.IsNullOrEmpty(FstrSabun) == false && strAnony.Equals("1"))
                {
                    strWSABUN = FstrSabun;
                    strWBUSE = FstrBuse;
                }
                else if(string.IsNullOrEmpty(FstrSabun) == false && strAnony.Equals("0") && string.IsNullOrEmpty(strRowid) == false)
                {
                    strWSABUN = FstrSabun;
                    strWBUSE = FstrBuse;
                }
                else
                {
                    strWSABUN =  ss1_Sheet1.Cells[4, 7].Text.Trim().Replace("*****", "");
                    strWBUSE = ss1_Sheet1.Cells[4, 13].Text.Trim();
                }

                if (!string.IsNullOrEmpty(strRowid))
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.ETC_SAFETY_REPORT2_HISTORY ";
                    SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.ETC_SAFETY_REPORT2 ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = " DELETE KOSMOS_PMPA.ETC_SAFETY_REPORT2";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                SQL = " INSERT INTO KOSMOS_PMPA.ETC_SAFETY_REPORT2(";
                SQL = SQL + "\r" + " WDATE, WSABUN, WBUSE, RDATE, ";
                SQL = SQL + "\r" + "  TARGET1, TARGET2, TARGET3, TARGET4, ";
                SQL = SQL + "\r" + "  TARGET5, TARGET6, TARGET7, TARGET8, ";
                SQL = SQL + "\r" + "  BALDATE, BALPLACE, ACCI01,";
                SQL = SQL + "\r" + "  ACCI02, ACCI03, ACCI04, ACCI05,";
                SQL = SQL + "\r" + "  ACCI06, ACCI07, ACCI08, ACCI09,";
                SQL = SQL + "\r" + "  ACCI10, ACCI11, ACCI12, ACCI13,";
                SQL = SQL + "\r" + "  ACCI14, ACCI15, ACCI16, ACCI17,";
                SQL = SQL + "\r" + "  RESULT1, RESULT2,     ";
                SQL = SQL + "\r" + "  REPORT1, REPORT2, REPORT3, ANONY) VALUES (";
                SQL = SQL + "\r" + " SYSDATE, '" + strWSABUN + "','" + strWBUSE + "',TO_DATE('" + StrRDate + "','YYYY-MM-DD HH24:MI'),";
                SQL = SQL + "\r" + "'" + strTARGET1 + "','" + strTARGET2 + "','" + strTARGET3 + "','" + strTARGET4 + "', ";
                SQL = SQL + "\r" + "'" + strTARGET5 + "','" + strTARGET6 + "','" + strTARGET7 + "','" + strTARGET8 + "', ";
                SQL = SQL + "\r" + "TO_DATE('" + strBalDate + "','YYYY-MM-DD HH24:MI'), '" + strBALPLACE + "','" + strACCI01 + "',";
                SQL = SQL + "\r" + "'" + strACCI02 + "','" + strACCI03 + "','" + strACCI04 + "','" + strACCI05 + "', ";
                SQL = SQL + "\r" + "'" + strACCI06 + "','" + strACCI07 + "','" + strACCI08 + "','" + strACCI09 + "', ";
                SQL = SQL + "\r" + "'" + strACCI10 + "','" + strACCI11 + "','" + strACCI12 + "','" + strACCI13 + "', ";
                SQL = SQL + "\r" + "'" + strACCI14 + "','" + strACCI15 + "','" + strACCI16 + "','" + strACCI17 + "', ";
                SQL = SQL + "\r" + "'" + strResult1 + "','" + strResult2 + "',     ";
                SQL = SQL + "\r" + "'" + strREPORT1 + "','" + strREPORT2 + "','" + strREPORT3 + "','" + strAnony + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("삭제 후 복구 불가능 합니다. 삭제하시겠습니까?") == DialogResult.No) return;

            if (Delete_Data() == false) return;

            //GetSearchData();
            btnSearch.PerformClick();
        }

        bool Delete_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " INSERT INTO KOSMOS_PMPA.ETC_SAFETY_REPORT2_HISTORY ";
                SQL += ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.ETC_SAFETY_REPORT2 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = " DELETE KOSMOS_PMPA.ETC_SAFETY_REPORT2";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ss1.PrintSheet(0);

            Save_Data();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0) return;
            strRowid = ssList_Sheet1.Cells[e.Row, 1].Text;
            READ_DATA(ssList_Sheet1.Cells[e.Row, 1].Text.Trim());
        }

        private void ss1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            FarPoint.Win.Spread.CellType.TextCellType tCellType = new FarPoint.Win.Spread.CellType.TextCellType();


            if (ss1_Sheet1.Cells[3, 4].Text == "True")
            {

                ss1_Sheet1.Cells[4, 2].Text = "";
                ss1_Sheet1.Cells[4, 7].Text = "";
                ss1_Sheet1.Cells[4, 13].Text = "";

                tCellType.Static = false;
                ss1_Sheet1.Cells[4, 2].CellType = tCellType;
                ss1_Sheet1.Cells[4, 7].CellType = tCellType;
                ss1_Sheet1.Cells[4, 13].CellType = tCellType;
            }
            else
            {
                ss1_Sheet1.Cells[4, 2].Text = clsType.User.UserName;
                ss1_Sheet1.Cells[4, 7].Text = clsType.User.Sabun;
                ss1_Sheet1.Cells[4, 13].Text = clsVbfunc.GetBASBuSe(clsDB.DbCon, clsType.User.BuseCode);

                tCellType.Static = true;
                ss1_Sheet1.Cells[4, 2].CellType = tCellType;
                ss1_Sheet1.Cells[4, 7].CellType = tCellType;
                ss1_Sheet1.Cells[4, 13].CellType = tCellType;

                ss1_Sheet1.Cells[4, 20].Text = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("yyyy-MM-dd HH:mm");
            }
        }
    }
}
