using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComEmrBase;
using ComLibB;
//using ComSupLibB;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDiet
    /// File Name       : frmNSTView_New.cs
    /// Description     : NST_NEW
    /// Author          : 박창욱
    /// Create Date     : 2018-04-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\mtsEmr\CADEX\FrmNSTView_New.frm(FrmNSTView_New.frm) >> frmNSTView_New.cs 폼이름 재정의" />	
    public partial class frmNSTView_New : Form
    {
        string GstrHelpCode = "";

        string FstrCLEAR = "";

        public frmNSTView_New()
        {
            InitializeComponent();
        }

        public frmNSTView_New(string strHelpCode)
        {
            InitializeComponent();
            GstrHelpCode = strHelpCode;
        }

        private void rdo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo0.Checked == true)
            {
                pan0.Visible = true;
                pan1.Visible = false;
                pan2.Visible = false;
                pan3.Visible = false;
                pan4.Visible = false;
                pan5.Visible = false;
            }
            else if (rdo1.Checked == true)
            {
                pan0.Visible = false;
                pan1.Visible = true;
                pan2.Visible = false;
                pan3.Visible = false;
                pan4.Visible = false;
                pan5.Visible = false;
            }
            else if (rdo2.Checked == true)
            {
                pan0.Visible = false;
                pan1.Visible = false;
                pan2.Visible = true;
                pan3.Visible = false;
                pan4.Visible = false;
                pan5.Visible = false;
            }
            else if (rdo3.Checked == true)
            {
                pan0.Visible = false;
                pan1.Visible = false;
                pan2.Visible = false;
                pan3.Visible = true;
                pan4.Visible = false;
                pan5.Visible = false;
            }
            else if (rdo4.Checked == true)
            {
                pan0.Visible = false;
                pan1.Visible = false;
                pan2.Visible = false;
                pan3.Visible = false;
                pan4.Visible = true;
                pan5.Visible = false;
            }
            else if (rdo5.Checked == true)
            {
                pan0.Visible = false;
                pan1.Visible = false;
                pan2.Visible = false;
                pan3.Visible = false;
                pan4.Visible = false;
                pan5.Visible = true;
            }
        }

        private void frmNSTView_New_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            pan0.Dock = DockStyle.Fill;
            pan1.Dock = DockStyle.Fill;
            pan2.Dock = DockStyle.Fill;
            pan3.Dock = DockStyle.Fill;
            pan4.Dock = DockStyle.Fill;
            pan5.Dock = DockStyle.Fill;

            pan0.Visible = true;
            pan1.Visible = false;
            pan2.Visible = false;
            pan3.Visible = false;
            pan4.Visible = false;
            pan5.Visible = false;

            //NST담당자만 보이게
            btnPrintScan.Visible = clsType.User.JobGroup == "JOB018005";

            DrawJepcode(clsPat.PATi.WRTNO);
            Read_Init();
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            Add_Line();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComFunc.MsgBoxQ("삭제하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //====================================
                //DIET_NST_PROGRESS
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_PROGRESS_HISTORY(";
                SQL = SQL + ComNum.VBLF + "  WRTNO , IPDNO, PANO, SNAME,";
                SQL = SQL + ComNum.VBLF + "  SEX , AGE, INDATE, DEPTCODE,";
                SQL = SQL + ComNum.VBLF + "  DRCODE , WARDCODE, ROOMCODE, DIAGNOSIS,";
                SQL = SQL + ComNum.VBLF + "  DRSABUN , NRSABUN, PMSABUN, DTSABUN,";
                SQL = SQL + ComNum.VBLF + "  RDATE , BDATE, STATUS, TO_DRCODE, ";
                SQL = SQL + ComNum.VBLF + "  COMPLITE, ORDERNO, OK_SABUN, VIEW_SABUN, VIEW_WRITEDATE)";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "  WRTNO , IPDNO, PANO, SNAME,";
                SQL = SQL + ComNum.VBLF + "  SEX , AGE, INDATE, DEPTCODE,";
                SQL = SQL + ComNum.VBLF + "  DRCODE , WARDCODE, ROOMCODE, DIAGNOSIS,";
                SQL = SQL + ComNum.VBLF + "  DRSABUN , NRSABUN, PMSABUN, DTSABUN,";
                SQL = SQL + ComNum.VBLF + "  RDATE , BDATE, STATUS, TO_DRCODE,";
                SQL = SQL + ComNum.VBLF + "  COMPLITE, ORDERNO, OK_SABUN, VIEW_SABUN, VIEW_WRITEDATE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_PROGRESS ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_PROGRESS ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //====================================

                //====================================
                //DIET_NST
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_HISTORY( ";
                for (i = 1; i <= 43; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, WRTNO)  ";
                SQL = SQL + ComNum.VBLF + " SELECT         ";
                for (i = 1; i <= 43; i++)
                {
                    SQL = SQL + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " SYSDATE, '" + clsType.User.Sabun + "', WRTNO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //====================================

                //====================================
                //DIET_NST_M1
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M1_HISTORY( ";
                for (i = 1; i <= 97; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, WRTNO)  ";
                SQL = SQL + ComNum.VBLF + " SELECT         ";
                for (i = 1; i <= 97; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " SYSDATE, '" + clsType.User.Sabun + "', WRTNO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M1 ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_M1 ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //====================================

                //====================================
                //DIET_NST_M1_EN
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M1_EN_HISTORY( ";
                for (i = 1; i <= 70; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, WRTNO)  ";
                SQL = SQL + ComNum.VBLF + " SELECT         ";
                for (i = 1; i <= 70; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " SYSDATE, '" + clsType.User.Sabun + "', WRTNO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M1_EN ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_M1_EN ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //====================================

                //====================================
                //DIET_NST_M2
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M2_HISTORY( ";
                for (i = 1; i <= 29; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, WRTNO)  ";
                SQL = SQL + ComNum.VBLF + " SELECT         ";
                for (i = 1; i <= 29; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " SYSDATE, '" + clsType.User.Sabun + "', WRTNO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M2 ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_M2 ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //====================================

                //==================================== 
                //DIET_NST_M3
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M3_HISTORY( ";
                for (i = 1; i <= 322; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("000") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, WRTNO)  ";
                SQL = SQL + ComNum.VBLF + " SELECT         ";
                for (i = 1; i <= 322; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("000") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " SYSDATE, '" + clsType.User.Sabun + "', WRTNO";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M3 ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_M3 ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //====================================

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (GstrHelpCode == "SUB")
            {
                Read_Init();
            }
            else
            {
                this.Close();
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLab_Click(object sender, EventArgs e)
        {
            Read_ResultDate();
        }

        private void btnLabClose_Click(object sender, EventArgs e)
        {
            panLab.Visible = false;
        }

        private void btnLabDel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strCode = "";

            if (ssLab_Sheet1.RowCount == 0)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssLab_Sheet1.RowCount; i++)
                {
                    strCode = ssLab_Sheet1.Cells[i, 1].Text.Trim();

                    if (strCode != "" && Convert.ToBoolean(ssLab_Sheet1.Cells[i, 0].Value) == true)
                    {
                        SQL = "";
                        SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + clsPat.PATi.Pano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND INDATE = TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND SUBCODE = '" + strCode + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            Search_LabData();
        }

        private void btnLabReset_Click(object sender, EventArgs e)
        {
            Set_Lab_Patient();
        }

        private void btnLabUpdate_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strCode = "";

            if (ssLab_Sheet1.RowCount == 0)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssLab_Sheet1.RowCount; i++)
                {
                    SQL = "";
                    SQL = " SELECT ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.DIET_NST_LAB_PATIENT ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + clsPat.PATi.Pano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND INDATE = TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND SUBCODE = '" + strCode + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (strCode != "" && dt.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE, UDATE) VALUES (";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'), '" + clsPat.PATi.Pano + "','" + strCode + "', SYSDATE) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("등록하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            Search_LabData();
        }

        private void btnLabSearch_Click(object sender, EventArgs e)
        {
            Search_LabData();
        }

        void Search_LabData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssLab_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.SUBCODE, B.EXAMFNAME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT A, " + ComNum.DB_MED + "EXAM_MASTER B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + clsPat.PATi.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.INDATE = TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.SUBCODE = B.MASTERCODE ";
                SQL = SQL + ComNum.VBLF + "   GROUP BY A.SUBCODE, B.EXAMFNAME ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SUBCODE ASC";

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
                    return;
                }

                ssLab_Sheet1.RowCount = dt.Rows.Count + 4;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssLab_Sheet1.Cells[i, 0].Value = false;
                    ssLab_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUBCODE"].ToString().Trim();
                    ssLab_Sheet1.Cells[i, 2].Text = dt.Rows[i]["EXAMFNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Set_New();
        }

        void Set_New()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strWEIGHT = "";
            string strHEIGHT = "";
            string strBEE = "";
            string StrRDate = "";
            string strROWID = "";
            string strMemo = "";
            string strGUBUN1 = "";
            string strGUBUN2 = "";
            string strGUBUN3 = "";
            string strGUBUN4 = "";
            string strGUBUN5 = "";
            string strGUBUN6 = "";
            string strIBW = "";

            string strSysDate = "";
            string strSysTime = "";

            StrRDate = clsPat.PATi.RDate;
            strROWID = clsPat.PATi.ROWID;

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT REMARK, DRCODE, GUBUN1, GUBUN2, GUBUN3, GUBUN4, GUBUN5, GUBUN6, SAYU1, SAYU2, SAYU3, SAYU4";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_CONSULT_NST ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strGUBUN1 = dt.Rows[0]["GUBUN1"].ToString().Trim();

                    switch (strGUBUN1)
                    {
                        case "1":
                            strGUBUN1 = "EN";
                            break;
                        case "2":
                            strGUBUN1 = "TPN";
                            break;
                        case "3":
                            strGUBUN1 = "Oral";
                            break;
                    }

                    strGUBUN2 = dt.Rows[0]["GUBUN2"].ToString().Trim();

                    switch (strGUBUN2)
                    {
                        case "1":
                            strGUBUN2 = "영양평가";
                            break;
                        case "2":
                            strGUBUN2 = "영양교육";
                            break;
                    }

                    strGUBUN3 = dt.Rows[0]["GUBUN3"].ToString().Trim();

                    switch (strGUBUN3)
                    {
                        case "1":
                            strGUBUN3 = "EN";
                            break;
                        case "2":
                            strGUBUN3 = "EN+TPN";
                            break;
                        case "3":
                            strGUBUN3 = "EN+PNN";
                            break;
                        case "11":
                            strGUBUN3 = "TPN(Central)";
                            break;
                        case "12":
                            strGUBUN3 = "PPN(Perperal)";
                            break;
                    }

                    strGUBUN4 = dt.Rows[0]["GUBUN4"].ToString().Trim();

                    switch (strGUBUN4)
                    {
                        case "1":
                            strGUBUN4 = "1주이내";
                            break;
                        case "2":
                            strGUBUN4 = "1~2주";
                            break;
                        case "3":
                            strGUBUN4 = "2~4주";
                            break;
                        case "4":
                            strGUBUN4 = "4주이상";
                            break;
                    }

                    strGUBUN5 = dt.Rows[0]["GUBUN5"].ToString().Trim();

                    switch (strGUBUN5)
                    {
                        case "1":
                            strGUBUN5 = "NG";
                            break;
                        case "2":
                            strGUBUN5 = "NJ";
                            break;
                        case "3":
                            strGUBUN5 = "PEG";
                            break;
                        case "4":
                            strGUBUN5 = "Gastrotomy ";
                            break;
                        case "5":
                            strGUBUN5 = "Jejunostomy)";
                            break;
                    }

                    strGUBUN6 = dt.Rows[0]["GUBUN6"].ToString().Trim();

                    switch (strGUBUN6)
                    {
                        case "1":
                            strGUBUN6 = "유";
                            break;
                        case "2":
                            strGUBUN6 = "무";
                            break;
                    }

                    strMemo = "의뢰영역 -" + strGUBUN1 + "/" + "의뢰유형 -" + strGUBUN2 + "/" + "투여경로 -" + strGUBUN3 + "/" + "예상투여기간 -" + strGUBUN4 + "/" + "EN Tube 종류 -" + strGUBUN5 + "/" + "C-LINE -" + strGUBUN6;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT  IPDNO, PANO, SNAME, ";
                SQL = SQL + ComNum.VBLF + " SEX, AGE, TO_CHAR(INDATE, 'YYYY-MM-DD') INDATE, DEPTCODE, ";
                SQL = SQL + ComNum.VBLF + " DRCODE, WARDCODE, ROOMCODE, HEIGHT, WEIGHT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + clsPat.PATi.IPDNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    Var_Clear();
                    Screen_Clear();

                    clsPat.PATi.IPDNO = VB.Val(dt.Rows[0]["IPDNO"].ToString().Trim());
                    clsPat.PATi.Pano = dt.Rows[0]["PANO"].ToString().Trim();
                    clsPat.PATi.sName = dt.Rows[0]["SNAME"].ToString().Trim();
                    clsPat.PATi.Sex = dt.Rows[0]["SEX"].ToString().Trim();
                    clsPat.PATi.Age = dt.Rows[0]["AGE"].ToString().Trim();
                    clsPat.PATi.InDate = VB.Left(dt.Rows[0]["INDATE"].ToString().Trim(), 10);
                    clsPat.PATi.DeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    clsPat.PATi.DrCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                    clsPat.PATi.WardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    clsPat.PATi.RoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    clsPat.PATi.DIAGNOSIS = Read_Diagnosis(clsPat.PATi.IPDNO);
                    clsPat.PATi.ROWID = strROWID;

                    if (clsPat.PATi.RDate == "")
                    {
                        clsPat.PATi.RDate = StrRDate;
                    }

                    ssView_Sheet1.Cells[4, 9].Text = clsPat.PATi.Pano;  //등록번호
                    ssView_Sheet1.Cells[4, 22].Text = clsPat.PATi.Sex;  //성별
                    ssView_Sheet1.Cells[4, 26].Text = clsPat.PATi.Age.ToString();  //나이
                    ssView_Sheet1.Cells[4, 36].Text = clsPat.PATi.InDate;   //입원일자

                    ssView_Sheet1.Cells[5, 9].Text = clsPat.PATi.sName; //환자명
                    ssView_Sheet1.Cells[5, 22].Text = clsPat.PATi.DeptCode; //진료과
                    ssView_Sheet1.Cells[5, 26].Text = clsPat.PATi.RoomCode; //병실
                    ssView_Sheet1.Cells[5, 36].Text = clsPat.PATi.RDate;    //의뢰일자

                    ssView_Sheet1.Cells[6, 9].Text = clsPat.PATi.DIAGNOSIS; //진단명

                    ssView_Sheet1.Cells[9, 2].Text = strMemo;   //의뢰내역

                    ssView_Sheet1.Cells[14, 3].Text = dt.Rows[0]["WEIGHT"].ToString().Trim();
                    strWEIGHT = dt.Rows[0]["WEIGHT"].ToString().Trim();
                    ssView_Sheet1.Cells[14, 15].Text = dt.Rows[0]["HEIGHT"].ToString().Trim();
                    strHEIGHT = dt.Rows[0]["HEIGHT"].ToString().Trim();

                    if (strHEIGHT != "")
                    {
                        strIBW = ((VB.Val(strHEIGHT) - 100) * 0.9).ToString("0");
                    }
                    else
                    {
                        strIBW = "0";
                    }

                    if (strWEIGHT == "")
                    {
                        strWEIGHT = "0";
                    }

                    ssView_Sheet1.Cells[14, 2].Text = strIBW;
                    ssView_Sheet1.Cells[14, 23].Text = dt.Rows[0]["AGE"].ToString().Trim();

                    if (strWEIGHT != "" && strHEIGHT != "")
                    {
                        switch (clsPat.PATi.Sex)
                        {
                            case "F":
                                strBEE = (655 + (9.6 * VB.Val(strWEIGHT)) + (1.8 * VB.Val(strHEIGHT)) - (4.7 * VB.Val(clsPat.PATi.Age))).ToString();
                                break;
                            case "M":
                                strBEE = (66.4 + (13.7 * VB.Val(strWEIGHT)) + (5 * VB.Val(strHEIGHT)) - (6.8 * VB.Val(clsPat.PATi.Age))).ToString();
                                break;
                        }

                        ssView_Sheet1.Cells[14, 32].Text = strBEE;
                        ssView_Sheet1.Cells[23, 2].Text = strBEE;
                    }

                    ssView2_Sheet1.Cells[4, 9].Text = clsPat.PATi.Pano; //등록번호
                    ssView2_Sheet1.Cells[5, 9].Text = clsPat.PATi.sName;    //환자명
                    ssView2_Sheet1.Cells[6, 9].Text = clsPat.PATi.Sex;  //성별
                    ssView2_Sheet1.Cells[6, 13].Text = clsPat.PATi.Age.ToString();  //나이
                    ssView2_Sheet1.Cells[7, 9].Text = clsPat.PATi.DeptCode; //진료과
                    ssView2_Sheet1.Cells[7, 13].Text = clsPat.PATi.RoomCode;    //병실
                    ssView2_Sheet1.Cells[8, 8].Text = clsPat.PATi.InDate;   //입원일자
                    ssView2_Sheet1.Cells[8, 19].Text = clsPat.PATi.RDate;   //의뢰일자  
                    ssView2_Sheet1.Cells[8, 33].Text = strSysDate + " " + strSysTime;

                    ssView2_EN_Sheet1.Cells[4, 9].Text = clsPat.PATi.Pano;  //등록번호
                    ssView2_EN_Sheet1.Cells[5, 9].Text = clsPat.PATi.sName; //환자명
                    ssView2_EN_Sheet1.Cells[6, 9].Text = clsPat.PATi.Sex;   //성별
                    ssView2_EN_Sheet1.Cells[6, 13].Text = clsPat.PATi.Age.ToString();   //나이
                    ssView2_EN_Sheet1.Cells[7, 9].Text = clsPat.PATi.DeptCode;    //진료과
                    ssView2_EN_Sheet1.Cells[7, 13].Text = clsPat.PATi.RoomCode; //병실
                    ssView2_EN_Sheet1.Cells[8, 8].Text = clsPat.PATi.InDate;    //입원일자
                    ssView2_EN_Sheet1.Cells[8, 19].Text = clsPat.PATi.RDate;    //의뢰일자
                    ssView2_EN_Sheet1.Cells[8, 33].Text = strSysDate + " " + strSysTime;    //작성일자

                    ssView3_Sheet1.Cells[14, 9].Text = strIBW;
                    ssView3_Sheet1.Cells[14, 13].Text = (VB.Val(strIBW) * 1.5).ToString();
                    ssView3_Sheet1.Cells[15, 9].Text = (VB.Val(strIBW) * 3).ToString();
                    ssView3_Sheet1.Cells[15, 13].Text = (VB.Val(strIBW) * 5).ToString();
                    ssView3_Sheet1.Cells[16, 9].Text = strIBW;
                    ssView3_Sheet1.Cells[16, 13].Text = (VB.Val(strIBW) * 2).ToString();

                    if (VB.Val(clsPat.PATi.Age) < 60)
                    {
                        ssView3_Sheet1.Cells[17, 9].Text = (1500 + 20 * (VB.Val(strWEIGHT) - 20)).ToString();
                    }

                    if (VB.Val(clsPat.PATi.Age) >= 60)
                    {
                        ssView3_Sheet1.Cells[17, 9].Text = (1500 + 15 * (VB.Val(strWEIGHT) - 20)).ToString();
                    }

                    ssView4_Sheet1.Cells[4, 9].Text = clsPat.PATi.Pano; //등록번호
                    ssView4_Sheet1.Cells[5, 9].Text = clsPat.PATi.sName;    //환자명
                    ssView4_Sheet1.Cells[6, 9].Text = clsPat.PATi.Sex;  //성별
                    ssView4_Sheet1.Cells[6, 13].Text = clsPat.PATi.Age.ToString();  //나이
                    ssView4_Sheet1.Cells[7, 9].Text = clsPat.PATi.DeptCode; //진료과
                    ssView4_Sheet1.Cells[7, 13].Text = clsPat.PATi.RoomCode;    //병실
                    ssView4_Sheet1.Cells[8, 8].Text = clsPat.PATi.InDate;   //입원일자
                    ssView4_Sheet1.Cells[8, 19].Text = clsPat.PATi.RDate;   //의뢰일자
                    ssView4_Sheet1.Cells[8, 33].Text = strSysDate + " " + strSysTime;   //작성일자

                    Set_Lab_Patient();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            btnPrint2.Text = "출력";
            panPrint.Visible = true;
        }

        /// <summary>
        /// 스캔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintScan_Click(object sender, EventArgs e)
        {
            btnPrint2.Text = "스캔";
            panPrint.Visible = true;
        }

        private void btnPrintCancel_Click(object sender, EventArgs e)
        {
            panPrint.Visible = false;
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            string strOutDate = string.Empty;
            string strTreatNo = string.Empty;
            string strBDate = clsPat.PATi.RDate.Length > 0 ? clsPat.PATi.RDate.Substring(0, 10) : clsPat.PATi.RDate;
            if (btnPrint2.Text == "스캔")
            {
                if (string.IsNullOrWhiteSpace(strBDate))
                {
                    ComFunc.MsgBoxEx(this, "의뢰일이 있을경우에만 스캔이 가능합니다.");
                    return;
                }

                clsImgcvt.NEW_PohangTreatInterface(clsDB.DbCon, this, clsPat.PATi.Pano);
                clsImgcvt.GetPatIpdInfo(clsDB.DbCon, clsPat.PATi.Pano, "I", clsPat.PATi.InDate, clsPat.PATi.DeptCode, ref strTreatNo, ref strOutDate);
                clsImgcvt.CreateSaveFolder();
                clsScan.DeleteFoldAll(@"C:\HealthSoft\IMGCVT");

                if (clsImgcvt.IsNSTCvt(clsDB.DbCon, strTreatNo, strBDate))
                {
                    if (ComFunc.MsgBoxQEx(this, "해당 의뢰일자로 변환된 내역이 있습니다\r\n삭제후 다시 변환 하시겠습니까?") == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        clsImgcvt.DelNSTCvt(clsDB.DbCon, strTreatNo, strBDate);
                    }
                }
            }

            long PageNum = 3;
            if (chkPrt1.Checked == true)
            {
                if (btnPrint2.Text == "출력")
                {
                    ssView_Sheet1.PrintInfo.ZoomFactor = 1.1f;
                    ssView.PrintSheet(0);
                    ComFunc.Delay(2000);
                }
                else
                {
                    ssView_Sheet1.PrintInfo.ZoomFactor = 1.6f;
                    clsImgcvt.SpreadToImg(clsDB.DbCon, ssView, clsPat.PATi.Pano, clsPat.PATi.InDate, clsPat.PATi.DeptCode, "109", ref PageNum);
                }
            }

            if (chkPrt2.Checked == true)
            {
                if (btnPrint2.Text == "출력")
                {
                    ssView2_Sheet1.PrintInfo.ZoomFactor = 1.1f;
                    ssView2.PrintSheet(0);
                    ComFunc.Delay(2000);
                }
                else
                {
                    ssView2_Sheet1.PrintInfo.ZoomFactor = 1.5f;
                    clsImgcvt.SpreadToImg(clsDB.DbCon, ssView2, clsPat.PATi.Pano, clsPat.PATi.InDate, clsPat.PATi.DeptCode, "109", ref PageNum);
                }
            }

            if (chkPrt3.Checked == true)
            {
                if (btnPrint2.Text == "출력")
                {
                    ssView2_EN_Sheet1.PrintInfo.ZoomFactor = 1.1f;
                    ssView2_EN.PrintSheet(0);
                    ComFunc.Delay(2000);
                }
                else
                {
                    ssView2_EN_Sheet1.PrintInfo.ZoomFactor = 1.6f;
                    clsImgcvt.SpreadToImg(clsDB.DbCon, ssView2_EN, clsPat.PATi.Pano, clsPat.PATi.InDate, clsPat.PATi.DeptCode, "109", ref PageNum);
                }
            }


            if (chkPrt4.Checked == true)
            {
                if (btnPrint2.Text == "출력")
                {
                    ssView3_Sheet1.PrintInfo.ZoomFactor = 1.1f;
                    ssView3.PrintSheet(0);
                    ComFunc.Delay(2000);
                }
                else
                {
                    ssView3_Sheet1.PrintInfo.ZoomFactor = 1.6f;
                    clsImgcvt.SpreadToImg(clsDB.DbCon, ssView3, clsPat.PATi.Pano, clsPat.PATi.InDate, clsPat.PATi.DeptCode, "109", ref PageNum);
                }
            }

            if (chkPrt5.Checked == true)
            {
                if (btnPrint2.Text == "출력")
                {
                    ssView4_Sheet1.PrintInfo.ZoomFactor = 1.1f;
                    ssView4.PrintSheet(0);
                    ComFunc.Delay(2000);
                }
                else
                {
                    ssView4_Sheet1.PrintInfo.ZoomFactor = 1.6f;
                    clsImgcvt.SpreadToImg(clsDB.DbCon, ssView4, clsPat.PATi.Pano, clsPat.PATi.InDate, clsPat.PATi.DeptCode, "109", ref PageNum);
                }
            }

            if (chkPrt6.Checked == true)
            {
                if (btnPrint2.Text == "출력")
                {
                    ssView3_Progress_Sheet1.PrintInfo.ZoomFactor = 1.1f;
                    ssView3_Progress.PrintSheet(0);
                    ComFunc.Delay(2000);
                }
                else
                {
                    ssView3_Progress_Sheet1.PrintInfo.ZoomFactor = 1.6f;
                    clsImgcvt.SpreadToImg(clsDB.DbCon, ssView3_Progress, clsPat.PATi.Pano, clsPat.PATi.InDate, clsPat.PATi.DeptCode, "109", ref PageNum);
                }
            }

            if (btnPrint2.Text == "스캔")
            {
                clsImgcvt.NSTInfo.GBN = "협진";
                clsImgcvt.NSTInfo.REQUESTDATE = strBDate;
                clsImgcvt.NSTInfo.TREATNO = strTreatNo;

                string[] dirs = Directory.GetFiles(@"C:\HealthSoft\IMGCVT", "*.tif");
                if (dirs.Length > 0 && strTreatNo.Equals("0") == false)
                {
                    if (string.IsNullOrWhiteSpace(strOutDate))
                    {
                        strOutDate = clsPat.PATi.InDate.Replace("-", "");
                    }

                    if (clsImgcvt.ADO_LabUpload(clsDB.DbCon, clsImgcvt.gstrFormcode, strTreatNo, strOutDate, 99, dirs, false, true))
                    {
                        ComFunc.MsgBoxEx(this, "영양협진이 정상적으로 변환되었습니다. EMR뷰어를 켜서 꼭 재확인해주세요!!");
                    }
                    else
                    {
                        ComFunc.MsgBoxEx(this, "영양협진 변환 도중 오류가 발생했습니다 재변환해주세요.");
                    }
                }

                clsScan.DeleteFoldAll(@"C:\HealthSoft\IMGCVT");
            }

            Cursor.Current = Cursors.Default;
            panPrint.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (clsPat.PATi.WRTNO == 0)
            {
                DataTable dt = null;
                string SQL = "";    //Query문
                string SqlErr = ""; //에러문 받는 변수

                try
                {
                    SQL = "";
                    SQL = "SELECT " + ComNum.DB_PMPA + "SEQ_DIET_NST.NEXTVAL WRTNO";
                    SQL = SQL + ComNum.VBLF + "  FROM DUAL";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        clsPat.PATi.WRTNO = VB.Val(dt.Rows[0]["WRTNO"].ToString().Trim());
                    }

                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }

            if (Save_Progress(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("진행상황 저장에 실패했습니다.");
                return;
            }

            if (Save_ssView(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("Nutritional Chart 저장에 실패했습니다.");
                return;
            }

            if (Save_ssView2(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("Monitoring Page-1(PN) 저장에 실패하였습니다.");
                return;
            }

            if (Save_ssView2_EN(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("Monitoring Page-1(EN) 저장에 실패했습니다.");
                return;
            }

            if (Save_ssView3(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("Monitoring Page-2 저장에 실패하였습니다.");
                return;
            }

            if (Save_ssView3_Progress(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("추가 Recommended Page 저장에 실패하였습니다.");
                return;
            }

            if (Save_ssView4(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("Monitoring Page-3 저장에 실패하였습니다.");
                return;
            }

            Set_Clear();

            if (GstrHelpCode == "POPUP")
            {
                this.Close();
            }

            btnSave.Enabled = false;
            btnNew.Enabled = false;
            btnLab.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void ssView_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            string strAF = "";
            string strIF = "";
            string strBEE = "";

            if (FstrCLEAR != "")
            {
                return;
            }

            //AF
            if (e.Row == 16)
            {
                switch (e.Column)
                {
                    case 3:
                        strAF = "1.2";
                        break;
                    case 15:
                        strAF = "1.3";
                        break;
                    default:
                        strAF = "";
                        break;
                }

                if (Convert.ToBoolean(ssView_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView_Sheet1.Cells[23, 13].Text = strAF;
                }
                else
                {
                    ssView_Sheet1.Cells[23, 13].Text = "";
                }
            }
            //IF
            else if (e.Row == 19 || e.Row == 20 || e.Row == 21)
            {
                switch (e.Row + "," + e.Column)
                {
                    case "19,3":
                        strIF = "1.1";
                        break;
                    case "19,13":
                        strIF = "1.2";
                        break;
                    case "19,23":
                        strIF = "1.35";
                        break;
                    case "19,34":
                        strIF = "1.5";
                        break;
                    case "20,3":
                        strIF = "1.2";
                        break;
                    case "20,13":
                        strIF = "1.4";
                        break;
                    case "20,23":
                        strIF = "1.6";
                        break;
                    case "20,34":
                        strIF = "1.95";
                        break;
                    case "21,3":
                        strIF = "1.0";
                        break;
                    case "21,13":
                        strIF = "1.8";
                        break;
                    default:
                        strIF = "";
                        break;
                }

                if (Convert.ToBoolean(ssView_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView_Sheet1.Cells[23, 21].Text = strIF;
                }
                else
                {
                    ssView_Sheet1.Cells[23, 21].Text = "";
                }
            }

            strBEE = ssView_Sheet1.Cells[23, 2].Text.Trim();
            strAF = ssView_Sheet1.Cells[23, 13].Text.Trim();
            strIF = ssView_Sheet1.Cells[23, 21].Text.Trim();

            if (strBEE != "" && strAF != "" && strIF != "")
            {
                ssView_Sheet1.Cells[23, 30].Text = (VB.Val(strBEE) * VB.Val(strAF) * VB.Val(strIF)).ToString();
            }
            else
            {
                ssView_Sheet1.Cells[23, 30].Text = "";
            }
        }

        private void ssView_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            string strWEIGHT = "";
            string strHEIGHT = "";
            string strYear = "";

            string strBEE = "";

            //BEE
            if (FstrCLEAR != "")
            {
                return;
            }

            if (e.Row == 14)
            {
                strWEIGHT = ssView_Sheet1.Cells[14, 2].Text.Trim();
                strWEIGHT = ssView_Sheet1.Cells[14, 15].Text.Trim();
                strYear = ssView_Sheet1.Cells[14, 23].Text.Trim();

                if (strWEIGHT != "" && strHEIGHT != "" && strYear != "")
                {
                    switch (clsPat.PATi.Sex)
                    {
                        case "F":
                            strBEE = (655 + (9.6 * VB.Val(strWEIGHT)) + (1.8 * VB.Val(strHEIGHT)) - (4.7 * VB.Val(strYear))).ToString();
                            break;
                        case "M":
                            strBEE = (66.4 + (13.7 * VB.Val(strWEIGHT)) + (5 * VB.Val(strHEIGHT)) - (6.8 * VB.Val(strYear))).ToString();
                            break;
                    }

                    ssView_Sheet1.Cells[14, 32].Text = strBEE;
                    ssView_Sheet1.Cells[23, 2].Text = strBEE;
                }
            }

        }

        private void ssView2_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            int i = 0;
            string strB = "";
            string strC = "";
            string strF = "";
            string strG = "";
            string strI = "";
            string strValue = "";
            double dblTotal1 = 0;
            double dblTotal2 = 0;
            double dblTotal3 = 0;
            double dblTotal4 = 0;

            if (FstrCLEAR != "")
            {
                return;
            }

            if (e.Row >= 43)
            {
                if (e.Column == 31)
                {
                    strValue = ssView2_Sheet1.Cells[e.Row, 31].Text.Trim();
                    strC = ssView2_Sheet1.Cells[e.Row, 12].Text.Trim();
                    strG = ssView2_Sheet1.Cells[e.Row, 25].Text.Trim();
                    ssView2_Sheet1.Cells[e.Row, 35].Text = (VB.Val(strC) * VB.Val(strValue)).ToString();
                    ssView2_Sheet1.Cells[e.Row, 38].Text = (VB.Val(strG) * VB.Val(strValue)).ToString();

                    for (i = 43; i < ssView2_Sheet1.RowCount - 2; i++)
                    {
                        strValue = ssView2_Sheet1.Cells[i, 31].Text.Trim();
                        strB = ssView2_Sheet1.Cells[i, 8].Text.Trim();
                        dblTotal1 += (VB.Val(strB) * VB.Val(strValue));
                        dblTotal2 += VB.Val(ssView2_Sheet1.Cells[i, 35].Text);
                        strI = ssView2_Sheet1.Cells[i, 31].Text.Trim();
                        strG = ssView2_Sheet1.Cells[i, 22].Text.Trim();
                        strF = ssView2_Sheet1.Cells[i, 25].Text.Trim();
                        dblTotal3 += (VB.Val(strI) * VB.Val(strF));
                        dblTotal4 += (VB.Val(strI) * VB.Val(strG));
                    }

                    ssView2_Sheet1.Cells[39, 31].Text = dblTotal1.ToString();
                    ssView2_Sheet1.Cells[40, 31].Text = dblTotal2.ToString();

                    if (dblTotal4 > 0)
                    {
                        ssView2_Sheet1.Cells[41, 31].Text = (dblTotal3 / dblTotal4).ToString("0.00");
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[41, 31].Text = "";
                    }
                }
            }
        }

        public void Read_Init()
        {
            Screen_Clear();

            if (clsPat.PATi.IPDNO < 1)
            {
                return;
            }

            if (clsPat.PATi.WRTNO == 0) //신규작성일 경우
            {
                Set_New();
            }
            else
            {
                Var_Set();
                Set_Clear();
            }
        }

        void Var_Clear()
        {
            clsPat.PATi.WRTNO = 0;
            clsPat.PATi.IPDNO = 0;
            clsPat.PATi.Pano = "";
            clsPat.PATi.sName = "";
            clsPat.PATi.Sex = "";
            clsPat.PATi.Age = "";
            clsPat.PATi.InDate = "";
            clsPat.PATi.DeptCode = "";
            clsPat.PATi.DrCode = "";
            clsPat.PATi.WardCode = "";
            clsPat.PATi.RoomCode = "";
            clsPat.PATi.RDate = "";
            clsPat.PATi.DIAGNOSIS = "";
            clsPat.PATi.BDate = "";
            clsPat.PATi.DRSABUN = "";
            clsPat.PATi.NRSABUN = "";
            clsPat.PATi.PMSABUN = "";
            clsPat.PATi.DTSABUN = "";
            clsPat.PATi.ROWID = "";
        }

        void Clear_ssView()
        {
            ssView_Sheet1.Cells[4, 9].Text = "";    //등록번호
            ssView_Sheet1.Cells[4, 22].Text = "";   //성별
            ssView_Sheet1.Cells[4, 26].Text = "";   //나이
            ssView_Sheet1.Cells[4, 36].Text = "";   //입원일자

            ssView_Sheet1.Cells[5, 9].Text = "";    //환자명
            ssView_Sheet1.Cells[5, 22].Text = "";   //진료과
            ssView_Sheet1.Cells[5, 26].Text = "";   //병실
            ssView_Sheet1.Cells[5, 36].Text = "";   //의뢰일자

            ssView_Sheet1.Cells[6, 9].Text = "";    //진단명

            ssView_Sheet1.Cells[9, 2].Text = "";    //의뢰내역

            ssView_Sheet1.Cells[14, 2].Text = "";   //체중
            ssView_Sheet1.Cells[14, 15].Text = "";  //키
            ssView_Sheet1.Cells[14, 23].Text = "";  //나이
            ssView_Sheet1.Cells[14, 32].Text = "";  //BEE

            ssView_Sheet1.Cells[16, 3].Value = false;   //AF-1
            ssView_Sheet1.Cells[16, 15].Value = false;  //AF-2

            ssView_Sheet1.Cells[19, 3].Value = false;   //IF-1
            ssView_Sheet1.Cells[19, 13].Value = false;  //IF-1
            ssView_Sheet1.Cells[19, 23].Value = false;  //IF-1
            ssView_Sheet1.Cells[19, 34].Value = false;  //IF-1

            ssView_Sheet1.Cells[20, 3].Value = false;   //IF-1
            ssView_Sheet1.Cells[20, 13].Value = false;   //IF-1
            ssView_Sheet1.Cells[20, 23].Value = false;   //IF-1
            ssView_Sheet1.Cells[20, 34].Value = false;   //IF-1

            ssView_Sheet1.Cells[21, 13].Value = false;  //IF-1

            ssView_Sheet1.Cells[23, 2].Text = "";   //BEE
            ssView_Sheet1.Cells[23, 13].Text = "";  //AF
            ssView_Sheet1.Cells[23, 21].Text = "";  //IF
            ssView_Sheet1.Cells[23, 30].Text = "";  //1일 필요 칼로리

            ssView_Sheet1.Cells[26, 3].Value = false;   //GIT FUNCTION-1
            ssView_Sheet1.Cells[26, 13].Value = false;  //GIT FUNCTION-2
            ssView_Sheet1.Cells[26, 21].Text = "";      //GIT FUNCTION

            ssView_Sheet1.Cells[29, 8].Value = false;   //TPN-1
            ssView_Sheet1.Cells[29, 17].Value = false;  //TPN-2

            ssView_Sheet1.Cells[30, 6].Value = false;   //EN-1
            ssView_Sheet1.Cells[30, 10].Value = false;  //EN-2
            ssView_Sheet1.Cells[30, 13].Value = false;  //EN-3
            ssView_Sheet1.Cells[30, 21].Value = false;  //EN-4
            ssView_Sheet1.Cells[30, 26].Value = false;  //EN-5
            ssView_Sheet1.Cells[30, 34].Value = false;  //EN-6

            ssView_Sheet1.Cells[33, 2].Text = "";   //CONSULTING PLAN

            ssView_Sheet1.Cells[35, 31].Text = "";  //의뢰의사
        }

        void Clear_ssView2()
        {
            int i = 0;

            ssView2_Sheet1.Cells[4, 9].Text = "";   //등록번호

            ssView2_Sheet1.Cells[5, 9].Text = "";   //환자명

            ssView2_Sheet1.Cells[6, 9].Text = "";   //성별
            ssView2_Sheet1.Cells[6, 13].Text = "";   //나이

            ssView2_Sheet1.Cells[7, 9].Text = "";   //진료과
            ssView2_Sheet1.Cells[7, 13].Text = "";   //병실

            ssView2_Sheet1.Cells[8, 8].Text = "";   //입원일자
            ssView2_Sheet1.Cells[8, 19].Text = "";  //의뢰일자
            ssView2_Sheet1.Cells[8, 33].Text = "";   //작성일자

            ssView2_Sheet1.Cells[12, 21].Value = false; //Protein need
            ssView2_Sheet1.Cells[13, 21].Value = false;
            ssView2_Sheet1.Cells[14, 21].Value = false;
            ssView2_Sheet1.Cells[15, 21].Value = false;
            ssView2_Sheet1.Cells[16, 21].Value = false;

            ssView2_Sheet1.Cells[12, 41].Value = false; //NPC/N
            ssView2_Sheet1.Cells[13, 41].Value = false;
            ssView2_Sheet1.Cells[14, 41].Value = false;
            ssView2_Sheet1.Cells[15, 41].Value = false;

            ssView2_Sheet1.Cells[19, 6].Text = "";  //Lab Data
            ssView2_Sheet1.Cells[19, 14].Text = "";
            ssView2_Sheet1.Cells[19, 22].Text = "";
            ssView2_Sheet1.Cells[19, 30].Text = "";
            ssView2_Sheet1.Cells[19, 38].Text = "";

            ssView2_Sheet1.Cells[20, 6].Text = "";
            ssView2_Sheet1.Cells[20, 14].Text = "";
            ssView2_Sheet1.Cells[20, 22].Text = "";
            ssView2_Sheet1.Cells[20, 30].Text = "";
            ssView2_Sheet1.Cells[20, 38].Text = "";

            ssView2_Sheet1.Cells[21, 6].Text = "";
            ssView2_Sheet1.Cells[21, 14].Text = "";
            ssView2_Sheet1.Cells[21, 22].Text = "";
            ssView2_Sheet1.Cells[21, 30].Text = "";
            ssView2_Sheet1.Cells[21, 38].Text = "";

            ssView2_Sheet1.Cells[26, 9].Text = "";

            ssView2_Sheet1.Cells[27, 15].Value = false;
            ssView2_Sheet1.Cells[27, 22].Value = false;
            ssView2_Sheet1.Cells[27, 29].Value = false;
            ssView2_Sheet1.Cells[27, 36].Value = false;

            ssView2_Sheet1.Cells[28, 15].Value = false;
            ssView2_Sheet1.Cells[28, 22].Value = false;
            ssView2_Sheet1.Cells[28, 29].Value = false;
            ssView2_Sheet1.Cells[28, 36].Value = false;

            ssView2_Sheet1.Cells[29, 15].Value = false;
            ssView2_Sheet1.Cells[29, 22].Value = false;
            ssView2_Sheet1.Cells[29, 29].Value = false;
            ssView2_Sheet1.Cells[29, 36].Value = false;

            ssView2_Sheet1.Cells[30, 9].Text = "";
            ssView2_Sheet1.Cells[31, 9].Text = "";
            ssView2_Sheet1.Cells[32, 9].Text = "";
            ssView2_Sheet1.Cells[33, 9].Text = "";

            ssView2_Sheet1.Cells[34, 3].Value = false;
            ssView2_Sheet1.Cells[34, 17].Value = false;
            ssView2_Sheet1.Cells[34, 31].Value = false;

            ssView2_Sheet1.Cells[35, 3].Value = false;
            ssView2_Sheet1.Cells[35, 17].Value = false;
            ssView2_Sheet1.Cells[35, 31].Value = false;

            ssView2_Sheet1.Cells[39, 31].Text = ""; //Total Volume Intake
            ssView2_Sheet1.Cells[40, 31].Text = ""; //Total Calory Intake
            ssView2_Sheet1.Cells[41, 31].Text = ""; //NPC/N

            for (i = 43; i < 60; i++)
            {
                ssView2_Sheet1.Cells[i, 31].Text = "";
            }

            for (i = 43; i < 60; i++)
            {
                ssView2_Sheet1.Cells[i, 35].Text = "";
            }

            for (i = 43; i < 60; i++)
            {
                ssView2_Sheet1.Cells[i, 38].Text = "";
            }
        }

        void Clear_ssView2_EN()
        {
            ssView2_EN_Sheet1.Cells[4, 9].Text = "";    //등록번호

            ssView2_EN_Sheet1.Cells[5, 9].Text = "";    //환자명

            ssView2_EN_Sheet1.Cells[6, 9].Text = "";    //성별
            ssView2_EN_Sheet1.Cells[6, 13].Text = "";   //나이

            ssView2_EN_Sheet1.Cells[7, 9].Text = "";    //진료과
            ssView2_EN_Sheet1.Cells[7, 13].Text = "";   //병실

            ssView2_EN_Sheet1.Cells[8, 8].Text = "";    //입원일자
            ssView2_EN_Sheet1.Cells[8, 19].Text = "";    //의뢰일자
            ssView2_EN_Sheet1.Cells[8, 33].Text = "";    //작성일자

            ssView2_EN_Sheet1.Cells[12, 22].Value = false;  //Protein need
            ssView2_EN_Sheet1.Cells[13, 22].Value = false;
            ssView2_EN_Sheet1.Cells[14, 22].Value = false;
            ssView2_EN_Sheet1.Cells[15, 22].Value = false;
            ssView2_EN_Sheet1.Cells[16, 22].Value = false;

            ssView2_EN_Sheet1.Cells[12, 41].Value = false;  //NPC/N
            ssView2_EN_Sheet1.Cells[13, 41].Value = false;
            ssView2_EN_Sheet1.Cells[14, 41].Value = false;
            ssView2_EN_Sheet1.Cells[15, 41].Value = false;

            //하모닐란
            ssView2_EN_Sheet1.Cells[19, 33].Text = "";
            ssView2_EN_Sheet1.Cells[19, 37].Text = "";
            ssView2_EN_Sheet1.Cells[19, 40].Text = "";

            //경관급식
            ssView2_EN_Sheet1.Cells[20, 33].Text = "";
            ssView2_EN_Sheet1.Cells[20, 37].Text = "";
            ssView2_EN_Sheet1.Cells[20, 40].Text = "";

            //경관설사식
            ssView2_EN_Sheet1.Cells[21, 33].Text = "";
            ssView2_EN_Sheet1.Cells[21, 37].Text = "";
            ssView2_EN_Sheet1.Cells[21, 40].Text = "";

            //경관당뇨식
            ssView2_EN_Sheet1.Cells[22, 33].Text = "";
            ssView2_EN_Sheet1.Cells[22, 37].Text = "";
            ssView2_EN_Sheet1.Cells[22, 40].Text = "";
            //원복 2019-11-06
            ssView2_EN_Sheet1.Cells[23, 11].Text = "경관신부전식";
            ssView2_EN_Sheet1.Cells[23, 17].Text = "200";
            ssView2_EN_Sheet1.Cells[23, 21].Text = "400";
            ssView2_EN_Sheet1.Cells[23, 24].Text = "64";
            ssView2_EN_Sheet1.Cells[23, 29].Text = "17";
            ssView2_EN_Sheet1.Cells[23, 33].Text = "";
            ssView2_EN_Sheet1.Cells[23, 37].Text = "";

            //경관투석식
            ssView2_EN_Sheet1.Cells[24, 11].Text = "경관투석식";
            ssView2_EN_Sheet1.Cells[24, 17].Text = "200";
            ssView2_EN_Sheet1.Cells[24, 21].Text = "400";
            ssView2_EN_Sheet1.Cells[24, 24].Text = "49";
            ssView2_EN_Sheet1.Cells[24, 29].Text = "17";
            ssView2_EN_Sheet1.Cells[24, 33].Text = "";
            ssView2_EN_Sheet1.Cells[24, 37].Text = "";

            //경관저단백
            ssView2_EN_Sheet1.Cells[25, 11].Text = "경관저단백식";
            ssView2_EN_Sheet1.Cells[25, 17].Text = "100";
            ssView2_EN_Sheet1.Cells[25, 21].Text = "100";
            ssView2_EN_Sheet1.Cells[25, 24].Text = "17";
            ssView2_EN_Sheet1.Cells[25, 29].Text = "2.4";
            ssView2_EN_Sheet1.Cells[25, 33].Text = "";
            ssView2_EN_Sheet1.Cells[25, 37].Text = "";

            //경관당뇨 - 탄수화물(11) / 지방(4.8)
            ssView2_EN_Sheet1.Cells[26, 11].Text = "";
            ssView2_EN_Sheet1.Cells[26, 17].Text = "";
            ssView2_EN_Sheet1.Cells[26, 21].Text = "";
            ssView2_EN_Sheet1.Cells[26, 24].Text = "";
            ssView2_EN_Sheet1.Cells[26, 29].Text = "";
            ssView2_EN_Sheet1.Cells[26, 33].Text = "";
            ssView2_EN_Sheet1.Cells[26, 37].Text = "";

            //ssView2_EN_Sheet1.Cells[23, 11].Text = "경관고단백";
            //ssView2_EN_Sheet1.Cells[23, 17].Text = "68";
            //ssView2_EN_Sheet1.Cells[23, 21].Text = "100";
            //ssView2_EN_Sheet1.Cells[23, 24].Text = "20.5";
            //ssView2_EN_Sheet1.Cells[23, 29].Text = "5.5";
            //ssView2_EN_Sheet1.Cells[24, 11].Text = "경관급식";
            //ssView2_EN_Sheet1.Cells[24, 17].Text = "68";
            //ssView2_EN_Sheet1.Cells[24, 21].Text = "100";
            //ssView2_EN_Sheet1.Cells[24, 24].Text = "14.28";
            //ssView2_EN_Sheet1.Cells[24, 29].Text = "3";
            //ssView2_EN_Sheet1.Cells[25, 11].Text = "경관설사";
            //ssView2_EN_Sheet1.Cells[25, 17].Text = "68";
            //ssView2_EN_Sheet1.Cells[25, 21].Text = "100";
            //ssView2_EN_Sheet1.Cells[25, 24].Text = "15";
            //ssView2_EN_Sheet1.Cells[25, 29].Text = "3";
            //ssView2_EN_Sheet1.Cells[26, 11].Text = "경관당뇨";
            //ssView2_EN_Sheet1.Cells[26, 17].Text = "68";
            //ssView2_EN_Sheet1.Cells[26, 21].Text = "100";
            //ssView2_EN_Sheet1.Cells[26, 24].Text = "11";
            //ssView2_EN_Sheet1.Cells[26, 29].Text = "4.8";

            //경관신부적식
            ssView2_EN_Sheet1.Cells[23, 33].Text = "";
            ssView2_EN_Sheet1.Cells[23, 37].Text = "";
            ssView2_EN_Sheet1.Cells[23, 40].Text = "";

            //경관투석식
            ssView2_EN_Sheet1.Cells[24, 33].Text = "";
            ssView2_EN_Sheet1.Cells[24, 37].Text = "";
            ssView2_EN_Sheet1.Cells[24, 40].Text = "";

            //경관저단백식
            ssView2_EN_Sheet1.Cells[25, 33].Text = "";
            ssView2_EN_Sheet1.Cells[25, 37].Text = "";
            ssView2_EN_Sheet1.Cells[25, 40].Text = "";

            //경관저단백식
            ssView2_EN_Sheet1.Cells[26, 33].Text = "";
            ssView2_EN_Sheet1.Cells[26, 37].Text = "";
            ssView2_EN_Sheet1.Cells[26, 40].Text = "";

            ssView2_EN_Sheet1.Cells[32, 33].Text = "";  //Total Volume Intake
            ssView2_EN_Sheet1.Cells[33, 33].Text = "";  //Total Calory Intake
            ssView2_EN_Sheet1.Cells[34, 33].Text = "";  //NPC/N

            ssView2_EN_Sheet1.Cells[37, 6].Text = "";   //Lab Data
            ssView2_EN_Sheet1.Cells[37, 14].Text = "";
            ssView2_EN_Sheet1.Cells[37, 22].Text = "";
            ssView2_EN_Sheet1.Cells[37, 30].Text = "";
            ssView2_EN_Sheet1.Cells[37, 38].Text = "";

            ssView2_EN_Sheet1.Cells[38, 6].Text = "";
            ssView2_EN_Sheet1.Cells[38, 14].Text = "";
            ssView2_EN_Sheet1.Cells[38, 22].Text = "";
            ssView2_EN_Sheet1.Cells[38, 30].Text = "";
            ssView2_EN_Sheet1.Cells[38, 38].Text = "";

            ssView2_EN_Sheet1.Cells[39, 6].Text = "";
            ssView2_EN_Sheet1.Cells[39, 14].Text = "";

            ssView2_EN_Sheet1.Cells[44, 9].Text = "";   //영양상태평가

            ssView2_EN_Sheet1.Cells[45, 15].Value = false;
            ssView2_EN_Sheet1.Cells[45, 22].Value = false;
            ssView2_EN_Sheet1.Cells[45, 29].Value = false;
            ssView2_EN_Sheet1.Cells[45, 36].Value = false;

            ssView2_EN_Sheet1.Cells[46, 15].Value = false;
            ssView2_EN_Sheet1.Cells[46, 22].Value = false;
            ssView2_EN_Sheet1.Cells[46, 29].Value = false;
            ssView2_EN_Sheet1.Cells[46, 36].Value = false;

            ssView2_EN_Sheet1.Cells[47, 15].Value = false;
            ssView2_EN_Sheet1.Cells[47, 22].Value = false;
            ssView2_EN_Sheet1.Cells[47, 29].Value = false;
            ssView2_EN_Sheet1.Cells[47, 36].Value = false;

            ssView2_EN_Sheet1.Cells[48, 9].Text = "";
            ssView2_EN_Sheet1.Cells[49, 9].Text = "";
            ssView2_EN_Sheet1.Cells[50, 9].Text = "";
            ssView2_EN_Sheet1.Cells[51, 9].Text = "";

            ssView2_EN_Sheet1.Cells[52, 3].Value = false;
            ssView2_EN_Sheet1.Cells[52, 17].Value = false;
            ssView2_EN_Sheet1.Cells[52, 31].Value = false;

            ssView2_EN_Sheet1.Cells[53, 3].Value = false;
            ssView2_EN_Sheet1.Cells[53, 17].Value = false;
            ssView2_EN_Sheet1.Cells[53, 31].Value = false;

            ssView2_EN_Sheet1.Cells[54, 3].Value = false;
            ssView2_EN_Sheet1.Cells[54, 17].Value = false;
        }

        void Clear_ssView3()
        {
            ssView3_Sheet1.Cells[4, 7].Text = "";

            ssView3_Sheet1.Cells[7, 3].Value = false;
            ssView3_Sheet1.Cells[7, 11].Value = false;
            ssView3_Sheet1.Cells[7, 17].Value = false;
            ssView3_Sheet1.Cells[7, 23].Value = false;
            ssView3_Sheet1.Cells[7, 29].Value = false;
            ssView3_Sheet1.Cells[7, 36].Value = false;

            ssView3_Sheet1.Cells[10, 3].Text = "";
            ssView3_Sheet1.Cells[10, 23].Text = "";

            ssView3_Sheet1.Cells[13, 9].Text = "";  //Energy
            ssView3_Sheet1.Cells[13, 19].Text = "";
            ssView3_Sheet1.Cells[13, 23].Text = "";
            ssView3_Sheet1.Cells[13, 36].Text = ""; //NPC
            ssView3_Sheet1.Cells[13, 40].Text = "";

            ssView3_Sheet1.Cells[14, 9].Text = "";  //Protein
            ssView3_Sheet1.Cells[14, 19].Text = "";
            ssView3_Sheet1.Cells[14, 23].Text = "";

            ssView3_Sheet1.Cells[15, 9].Text = "";  //Glucose
            ssView3_Sheet1.Cells[15, 19].Text = "";
            ssView3_Sheet1.Cells[15, 23].Text = "";

            ssView3_Sheet1.Cells[16, 9].Text = "";  //Fat
            ssView3_Sheet1.Cells[16, 19].Text = "";
            ssView3_Sheet1.Cells[16, 23].Text = "";

            ssView3_Sheet1.Cells[17, 9].Text = "";  //Fluid

            ssView3_Sheet1.Cells[18, 9].Text = "";  //기타

            ssView3_Sheet1.Cells[21, 2].Text = "";  //recommend
            ssView3_Sheet1.Cells[24, 2].Text = "";  //Comment
            ssView3_Sheet1.Cells[27, 2].Text = "";  //Monitor
            ssView3_Sheet1.Cells[30, 2].Text = "";  //Monitor
        }

        void Clear_ssView4()
        {
            int i = 0;
            int j = 0;

            ssView4_Sheet1.Cells[4, 9].Text = "";   //등록번호

            ssView4_Sheet1.Cells[5, 9].Text = "";   //환자명

            ssView4_Sheet1.Cells[6, 9].Text = "";   //성별
            ssView4_Sheet1.Cells[6, 13].Text = "";  //나이

            ssView4_Sheet1.Cells[7, 9].Text = "";   //진료과
            ssView4_Sheet1.Cells[7, 9].Text = "";   //병실

            ssView4_Sheet1.Cells[8, 8].Text = "";   //입원일자
            ssView4_Sheet1.Cells[8, 19].Text = "";  //의뢰일자
            ssView4_Sheet1.Cells[8, 33].Text = "";  //작성일자

            ssView4_Sheet1.Cells[10, 3].Text = "";

            for (i = 10; i < 41; i++)
            {
                for (j = 14; j < 21; j++)
                {
                    ssView4_Sheet1.Cells[j, i].Text = "";
                }
            }

            for (i = 10; i < 41; i++)
            {
                for (j = 23; j < 36; j++)
                {
                    ssView4_Sheet1.Cells[j, i].Text = "";
                }
            }

            for (i = 2; i < 41; i++)
            {
                for (j = 39; j < 42; j++)
                {
                    ssView4_Sheet1.Cells[j, i].Text = "";
                }
            }
        }

        public void Screen_Clear()
        {
            lblDoing0.Text = "";
            lblDoing1.Text = "";
            lblDoing2.Text = "";
            lblDoing3.Text = "";
            lblDoing4.Text = "";
            txtComplete.Text = "";
            FstrCLEAR = "OK";
            Clear_ssView();
            Clear_ssView2();
            Clear_ssView2_EN();
            Clear_ssView3();
            ssView3_Progress_Sheet1.RowCount = 5;
            Clear_ssView4();
            FstrCLEAR = "";

            if (GstrHelpCode == "SUB")
            {
                btnNew.Enabled = true;
                btnLab.Enabled = true;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                btnExit.Enabled = false;
                ssView_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
                ssView2_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
                ssView2_EN_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
                ssView3_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
                ssView4_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
            }
            else if (GstrHelpCode == "POPUP")
            {
                btnNew.Enabled = true;
                btnLab.Enabled = true;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                btnExit.Enabled = true;
                ssView_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
                ssView2_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
                ssView2_EN_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
                ssView3_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
                ssView4_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
            }
            else
            {
                btnNew.Enabled = false;
                btnLab.Enabled = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                ssView_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
                ssView2_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
                ssView2_EN_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
                ssView3_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
                ssView4_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            }
        }

        void Read_ResultDate()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int j = 0;
            int k = 0;

            string[,,] strLab = new string[12, 5, 36];

            string strALB = "";
            string strHB = "";
            string strTLC = "";
            string strHCT = "";
            string strBun = "";
            string strCR = "";
            string strCA = "";
            string strP = "";

            string strMG = "";
            string strNa = "";
            string strK = "";
            string strCL = "";
            string strCHOL = "";
            string strTG = "";
            string strCRP = "";

            string strRESULTDATE = "";
            string strRESULTDATE_B = "";

            string strRDATE = "";

            ComFunc cf = new ComFunc();

            for (i = 0; i < 12; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    for (k = 0; k < 36; k++)
                    {
                        strLab[i, j, k] = "";
                    }
                }
            }

            strRDATE = VB.Left(ssView4_Sheet1.Cells[8, 33].Text.Trim(), 10);

            if (VB.IsDate(strRDATE) == false)
            {
                ComFunc.MsgBox("작성일자가 날짜형식이 아닙니다. (날짜형식:YYYY-MM-DD)");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.SPECNO, TO_CHAR(A.RESULTDATE, 'YYYY-MM-DD') RESULTDATE, B.SUBCODE, B.RESULT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_SPECMST A, " + ComNum.DB_MED + "EXAM_RESULTC B";
                SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + clsPat.PATi.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.SPECNO = B.SPECNO ";
                SQL = SQL + ComNum.VBLF + "   AND B.SUBCODE IN('CR32C','HR01C','HR01D','HR02Q','CR41A','CR42A','CR44B','CR45B','CR67A','CR51A','CR52A','CR53A','CR40A','CR39A','SE04B')";
                SQL = SQL + ComNum.VBLF + "   AND A.RESULTDATE >= TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strRDATE, -14) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND A.RESULTDATE < TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strRDATE, 1) + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " ORDER BY RESULTDATE DESC, SUBCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strALB = "";
                    strHB = "";
                    strHCT = "";
                    strTLC = "";
                    strBun = "";
                    strCR = "";
                    strCA = "";
                    strP = "";
                    strMG = "";
                    strNa = "";
                    strK = "";
                    strCL = "";
                    strCHOL = "";
                    strTG = "";
                    strCRP = "";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (string.Compare(strRESULTDATE_B, dt.Rows[i]["RESULTDATE"].ToString().Trim()) <= 0)
                        {
                            strRESULTDATE = dt.Rows[i]["RESULTDATE"].ToString().Trim();
                        }

                        switch (dt.Rows[i]["SUBCODE"].ToString().Trim())
                        {
                            case "CR32C":
                                if (strALB == "")
                                {
                                    strALB = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "HR01C":
                                if (strHB == "")
                                {
                                    strHB = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "HR01D":
                                if (strHCT == "")
                                {
                                    strHCT = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "HR02Q":
                                if (strTLC == "")
                                {
                                    strTLC = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "CR41A":
                                if (strBun == "")
                                {
                                    strBun = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;

                            case "CR42A":
                                if (strCR == "")
                                {
                                    strCR = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "CR44B":
                                if (strCA == "")
                                {
                                    strCA = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "CR45B":
                                if (strP == "")
                                {
                                    strP = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "CR67A":
                                if (strMG == "")
                                {
                                    strMG = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "CR51A":
                                if (strNa == "")
                                {
                                    strNa = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;

                            case "CR52A":
                                if (strK == "")
                                {
                                    strK = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "CR53A":
                                if (strCL == "")
                                {
                                    strCL = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "CR40A":
                                if (strCHOL == "")
                                {
                                    strCHOL = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "CR39A":
                                if (strTG == "")
                                {
                                    strTG = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                            case "SE04B":
                                if (strCRP == "")
                                {
                                    strCRP = dt.Rows[i]["RESULT"].ToString().Trim();
                                }
                                break;
                        }

                        strRESULTDATE_B = dt.Rows[i]["RESULTDATE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;


                ssView2_Sheet1.Cells[19, 6].Text = strALB;
                ssView2_Sheet1.Cells[19, 14].Text = strHB;
                ssView2_Sheet1.Cells[19, 22].Text = strHCT;
                ssView2_Sheet1.Cells[19, 30].Text = strTLC;
                ssView2_Sheet1.Cells[19, 38].Text = strBun;

                ssView2_Sheet1.Cells[20, 6].Text = strCR;
                ssView2_Sheet1.Cells[20, 14].Text = strCA;
                ssView2_Sheet1.Cells[20, 22].Text = strP;
                ssView2_Sheet1.Cells[20, 30].Text = strMG;
                ssView2_Sheet1.Cells[20, 38].Text = strNa;

                ssView2_Sheet1.Cells[21, 6].Text = strK;
                ssView2_Sheet1.Cells[21, 14].Text = strCL;
                ssView2_Sheet1.Cells[21, 22].Text = strCHOL;
                ssView2_Sheet1.Cells[21, 30].Text = strTG;
                ssView2_Sheet1.Cells[21, 38].Text = strCRP;

                ssView2_EN_Sheet1.Cells[37, 6].Text = strALB;
                ssView2_EN_Sheet1.Cells[37, 14].Text = strHB;
                ssView2_EN_Sheet1.Cells[37, 22].Text = strHCT;
                ssView2_EN_Sheet1.Cells[37, 30].Text = strTLC;
                ssView2_EN_Sheet1.Cells[37, 38].Text = strBun;

                ssView2_EN_Sheet1.Cells[38, 6].Text = strCR;
                ssView2_EN_Sheet1.Cells[38, 14].Text = strCA;
                ssView2_EN_Sheet1.Cells[38, 22].Text = strP;
                ssView2_EN_Sheet1.Cells[38, 30].Text = strMG;
                ssView2_EN_Sheet1.Cells[38, 38].Text = strNa;

                ssView2_EN_Sheet1.Cells[39, 6].Text = strK;
                ssView2_EN_Sheet1.Cells[39, 14].Text = strCL;
                ssView2_EN_Sheet1.Cells[39, 22].Text = strCHOL;
                ssView2_EN_Sheet1.Cells[39, 30].Text = strTG;
                ssView2_EN_Sheet1.Cells[39, 38].Text = strCRP;


                SQL = "";
                SQL = "SELECT B.BDATE, A.PANO, A.SUBCODE, C.EXAMFNAME, A.RESULT, A.STATUS, C.EXAMNAME";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_RESULTC A, " + ComNum.DB_MED + "EXAM_SPECMST B, " + ComNum.DB_MED + "EXAM_MASTER C";
                SQL = SQL + ComNum.VBLF + "WHERE A.SPECNO = B.SPECNO";
                SQL = SQL + ComNum.VBLF + "  AND A.SUBCODE = C.MASTERCODE";
                SQL = SQL + ComNum.VBLF + "   AND B.RESULTDATE >= TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strRDATE, -14) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND B.RESULTDATE < TO_DATE('" + cf.DATE_ADD(clsDB.DbCon, strRDATE, 1) + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND A.RESULT IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "   AND A.SUBCODE IN ('CR32C','HR01C','HR01D','HR02Q','CR41A','CR42A','CR44B','CR45B','CR67A','CR51A','CR52A','CR53A','CR40A','CR39A','SE40B')";
                SQL = SQL + ComNum.VBLF + "  AND A.PANO = '" + clsPat.PATi.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BDATE DESC, DECODE(A.SUBCODE,'CR32C',1,'HR01C',2,'HR01D',3,'HR02Q',4,'CR41A',5,'CR42A',6,'CR44B',7,'CR45B',8,'CR67A',9,'CR51A',10,'CR52A',11,'CR53A',12,'CR40A',13,'CR39A',14,'SE40B',15,16) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            j = 0;
                        }

                        if (strLab[j, 0, 0] != dt.Rows[i]["BDATE"].ToString().Trim())
                        {
                            if (i > 0)
                            {
                                j++;
                            }

                            k = 0;

                            if (j > 11)
                            {
                                break;
                            }

                            strLab[j, 0, k] = dt.Rows[i]["BDATE"].ToString().Trim();
                        }

                        strLab[j, 0, k] = dt.Rows[i]["BDATE"].ToString().Trim();
                        strLab[j, 1, k] = dt.Rows[i]["SUBCODE"].ToString().Trim();
                        strLab[j, 2, k] = dt.Rows[i]["RESULT"].ToString().Trim();
                        strLab[j, 3, k] = dt.Rows[i]["EXAMNAME"].ToString().Trim();
                        k++;
                    }
                }

                dt.Dispose();
                dt = null;

                for (i = 0; i < 11; i++)
                {
                    for (j = 0; j < 18; j++)
                    {
                        int nRow = 0;

                        if (strLab[i, 0, j] != "")
                        {
                            if (VB.IsDate(strLab[i, 0, j]) == true)
                            {
                                ssView4_Sheet1.Cells[23, 10 + (i * 3)].Text = Convert.ToDateTime(strLab[i, 0, j]).ToString("MM/dd");
                            }
                        }

                        switch (strLab[i, 1, j])
                        {
                            case "CR32C":    //ALB
                                nRow = 25;
                                break;
                            case "HR01C":    //HB
                                nRow = 26;
                                break;
                            case "HR01D":    //HCT
                                nRow = 27;
                                break;
                            case "HR02Q":     //TLC
                                nRow = 28;
                                break;
                            case "CR41A":     //BUN
                                nRow = 29;
                                break;
                            case "CR42A":     //CR
                                nRow = 30;
                                break;
                            case "CR44B":     //CA
                                nRow = 31;
                                break;
                            case "CR45B":     //P
                                nRow = 32;
                                break;
                            case "CR67A":     //MG
                                nRow = 33;
                                break;
                            case "CR51A":     //NA
                                nRow = 34;
                                break;
                            case "CR52A":     //K
                                nRow = 35;
                                break;
                            case "CR53A":    //CL
                                nRow = 36;
                                break;
                            case "CR40A":     //CHOL
                                nRow = 37;
                                break;
                            case "CR39A":    //TG
                                nRow = 38;
                                break;
                            case "SE04B":    //CRP
                                nRow = 39;
                                break;
                        }

                        if (strLab[i, 2, j] != "")
                        {
                            ssView4_Sheet1.Cells[nRow - 1, 10 + (i * 3)].Text = strLab[i, 2, j];
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        bool Save_Progress(double argWRTNO)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strBuse = "";
            string strPROGRESS = "";

            bool rtnVar = false;

            if (clsPat.PATi.RDate == "")
            {
                clsPat.PATi.RDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT BUSE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + clsType.User.Sabun + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    strBuse = dt.Rows[0]["BUSE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (clsPat.PATi.DRSABUN == "" && (VB.Left(strBuse, 2) == "01" || VB.Left(strBuse, 2) == "02"))
                {
                    clsPat.PATi.DRSABUN = clsType.User.Sabun;
                }

                if (clsPat.PATi.PMSABUN == "" && (strBuse == "044100" || strBuse == "044101"))
                {
                    clsPat.PATi.PMSABUN = clsType.User.Sabun;
                }

                if (clsPat.PATi.DTSABUN == "" && (strBuse == "044300" || strBuse == "044301"))
                {
                    clsPat.PATi.DTSABUN = clsType.User.Sabun;
                }

                if (clsPat.PATi.NRSABUN == "" && Cert_Nurse() == true)
                {
                    clsPat.PATi.NRSABUN = clsType.User.Sabun;
                }

                if (txtComplete.Text.Trim() != "")
                {
                    strPROGRESS = "C";
                }
                else
                {
                    if (clsPat.PATi.DRSABUN == "" && clsPat.PATi.PMSABUN == "" && clsPat.PATi.DTSABUN == "" && clsPat.PATi.NRSABUN == "")
                    {
                        strPROGRESS = "0";
                    }
                    else
                    {
                        strPROGRESS = "1";
                    }
                }


                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_PROGRESS_HISTORY(";
                SQL = SQL + ComNum.VBLF + "  WRTNO , IPDNO, PANO, SNAME,";
                SQL = SQL + ComNum.VBLF + "  SEX , AGE, INDATE, DEPTCODE,";
                SQL = SQL + ComNum.VBLF + "  DRCODE , WARDCODE, ROOMCODE, DIAGNOSIS,";
                SQL = SQL + ComNum.VBLF + "  DRSABUN , NRSABUN, PMSABUN, DTSABUN,";
                SQL = SQL + ComNum.VBLF + "  RDATE , BDATE, STATUS, TO_DRCODE, ";
                SQL = SQL + ComNum.VBLF + "  COMPLITE, ORDERNO, OK_SABUN, VIEW_SABUN, VIEW_WRITEDATE)";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "  WRTNO , IPDNO, PANO, SNAME,";
                SQL = SQL + ComNum.VBLF + "  SEX , AGE, INDATE, DEPTCODE,";
                SQL = SQL + ComNum.VBLF + "  DRCODE , WARDCODE, ROOMCODE, DIAGNOSIS,";
                SQL = SQL + ComNum.VBLF + "  DRSABUN , NRSABUN, PMSABUN, DTSABUN,";
                SQL = SQL + ComNum.VBLF + "  RDATE , BDATE, STATUS, TO_DRCODE, ";
                SQL = SQL + ComNum.VBLF + "  COMPLITE, ORDERNO, OK_SABUN, VIEW_SABUN, VIEW_WRITEDATE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_PROGRESS ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_PROGRESS ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_PROGRESS( ";
                SQL = SQL + ComNum.VBLF + "  WRTNO , IPDNO, PANO, SNAME, ";
                SQL = SQL + ComNum.VBLF + "  SEX , AGE, INDATE, DEPTCODE, ";
                SQL = SQL + ComNum.VBLF + "  DRCODE , WARDCODE, ROOMCODE, DIAGNOSIS, ";
                SQL = SQL + ComNum.VBLF + "  DRSABUN , NRSABUN, PMSABUN, DTSABUN, ";
                SQL = SQL + ComNum.VBLF + "  RDATE , BDATE, STATUS, TO_DRCODE, ";
                SQL = SQL + ComNum.VBLF + "  COMPLITE, ORDERNO) VALUES ( ";
                SQL = SQL + ComNum.VBLF + argWRTNO + ", " + clsPat.PATi.IPDNO + ", '" + clsPat.PATi.Pano + "','" + clsPat.PATi.sName + "', ";
                SQL = SQL + ComNum.VBLF + "'" + clsPat.PATi.Sex + "','" + clsPat.PATi.Age + "',TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.DeptCode + "', ";
                SQL = SQL + ComNum.VBLF + "'" + clsPat.PATi.DrCode + "','" + clsPat.PATi.WardCode + "','" + clsPat.PATi.RoomCode + "','" + VB.Left(clsPat.PATi.DIAGNOSIS, 50) + "', ";
                SQL = SQL + ComNum.VBLF + "'" + clsPat.PATi.DRSABUN + "','" + clsPat.PATi.NRSABUN + "','" + clsPat.PATi.PMSABUN + "','" + clsPat.PATi.DTSABUN + "', ";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + clsPat.PATi.RDate + "','YYYY-MM-DD HH24:MI'), TO_DATE('" + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + "','YYYY-MM-DD'),'" + strPROGRESS + "','', ";
                SQL = SQL + ComNum.VBLF + " TO_DATE('" + txtComplete.Text + "','YYYY-MM-DD'),'" + clsPat.PATi.OrderNo + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVar = true;

                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }
        }

        bool Save_ssView(double argWRTNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string[] strBun = new string[46];

            for (i = 0; i < strBun.Length; i++)
            {
                strBun[i] = "";
            }

            bool rtnVar = false;

            strBun[1] = ssView_Sheet1.Cells[4, 9].Text.Trim();  //등록번호
            strBun[2] = ssView_Sheet1.Cells[4, 22].Text.Trim();  //성별
            strBun[3] = ssView_Sheet1.Cells[4, 26].Text.Trim();  //나이
            strBun[4] = ssView_Sheet1.Cells[4, 36].Text.Trim();  //입원일자

            strBun[5] = ssView_Sheet1.Cells[5, 9].Text.Trim();  //환자명
            strBun[6] = ssView_Sheet1.Cells[5, 22].Text.Trim(); //진료과
            strBun[7] = ssView_Sheet1.Cells[5, 26].Text.Trim(); //병실
            strBun[8] = ssView_Sheet1.Cells[5, 36].Text.Trim(); //의뢰일자

            strBun[9] = ssView_Sheet1.Cells[6, 9].Text.Trim();  //진단명

            strBun[10] = ssView_Sheet1.Cells[9, 2].Text.Trim(); //의뢰내역

            strBun[11] = ssView_Sheet1.Cells[14, 2].Text.Trim(); //체중
            strBun[12] = ssView_Sheet1.Cells[14, 15].Text.Trim(); //키
            strBun[13] = ssView_Sheet1.Cells[14, 23].Text.Trim(); //나이
            strBun[14] = ssView_Sheet1.Cells[14, 32].Text.Trim(); //BEE

            strBun[15] = Convert.ToBoolean(ssView_Sheet1.Cells[16, 3].Value) == true ? "1" : "0"; //AF-1
            strBun[16] = Convert.ToBoolean(ssView_Sheet1.Cells[16, 15].Value) == true ? "1" : "0"; //AF-2

            strBun[17] = Convert.ToBoolean(ssView_Sheet1.Cells[19, 3].Value) == true ? "1" : "0"; //IF-1
            strBun[18] = Convert.ToBoolean(ssView_Sheet1.Cells[19, 13].Value) == true ? "1" : "0"; //IF-1
            strBun[19] = Convert.ToBoolean(ssView_Sheet1.Cells[19, 23].Value) == true ? "1" : "0"; //IF-1
            strBun[20] = Convert.ToBoolean(ssView_Sheet1.Cells[19, 34].Value) == true ? "1" : "0"; //IF-1

            strBun[21] = Convert.ToBoolean(ssView_Sheet1.Cells[20, 3].Value) == true ? "1" : "0"; //IF-1
            strBun[22] = Convert.ToBoolean(ssView_Sheet1.Cells[20, 13].Value) == true ? "1" : "0"; //IF-1
            strBun[23] = Convert.ToBoolean(ssView_Sheet1.Cells[20, 23].Value) == true ? "1" : "0"; //IF-1
            strBun[24] = Convert.ToBoolean(ssView_Sheet1.Cells[20, 34].Value) == true ? "1" : "0"; //IF-1

            strBun[25] = Convert.ToBoolean(ssView_Sheet1.Cells[21, 3].Value) == true ? "1" : "0"; //IF-1
            strBun[26] = Convert.ToBoolean(ssView_Sheet1.Cells[21, 13].Value) == true ? "1" : "0"; //IF-1

            strBun[27] = ssView_Sheet1.Cells[23, 2].Text.Trim(); //BEE
            strBun[28] = ssView_Sheet1.Cells[23, 13].Text.Trim(); //AF
            strBun[29] = ssView_Sheet1.Cells[23, 21].Text.Trim(); //IF
            strBun[30] = ssView_Sheet1.Cells[23, 30].Text.Trim(); //1일 필요 칼로리

            strBun[31] = Convert.ToBoolean(ssView_Sheet1.Cells[26, 3].Value) == true ? "1" : "0"; //GIT FUNCTION-1
            strBun[32] = Convert.ToBoolean(ssView_Sheet1.Cells[26, 13].Value) == true ? "1" : "0"; //GIT FUNCTION-2
            strBun[33] = ssView_Sheet1.Cells[26, 21].Text.Trim(); //GIT FUNCTION

            strBun[34] = Convert.ToBoolean(ssView_Sheet1.Cells[29, 8].Value) == true ? "1" : "0"; //TPN-1
            strBun[35] = Convert.ToBoolean(ssView_Sheet1.Cells[29, 17].Value) == true ? "1" : "0"; //TPN-2

            strBun[44] = Convert.ToBoolean(ssView_Sheet1.Cells[30, 7].Value) == true ? "1" : "0"; //EN-7 (oral)
            strBun[36] = Convert.ToBoolean(ssView_Sheet1.Cells[30, 10].Value) == true ? "1" : "0"; //EN-1
            strBun[37] = Convert.ToBoolean(ssView_Sheet1.Cells[30, 13].Value) == true ? "1" : "0"; //EN-2
            strBun[38] = Convert.ToBoolean(ssView_Sheet1.Cells[30, 16].Value) == true ? "1" : "0"; //EN-3
            strBun[39] = Convert.ToBoolean(ssView_Sheet1.Cells[30, 21].Value) == true ? "1" : "0"; //EN-4
            strBun[40] = Convert.ToBoolean(ssView_Sheet1.Cells[30, 26].Value) == true ? "1" : "0"; //EN-5
            strBun[41] = Convert.ToBoolean(ssView_Sheet1.Cells[30, 34].Value) == true ? "1" : "0"; //EN-6

            strBun[42] = ssView_Sheet1.Cells[33, 2].Text.Trim(); //CONSULTING PLAN

            strBun[43] = ssView_Sheet1.Cells[35, 31].Text.Trim(); //담당의사

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.DIET_NST ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_HISTORY( ";
                    for (i = 1; i < 45; i++)
                    {
                        SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                    }
                    SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, WRTNO)  ";
                    SQL = SQL + ComNum.VBLF + " SELECT";
                    for (i = 1; i < 45; i++)
                    {
                        SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                    }
                    SQL = SQL + ComNum.VBLF + " SYSDATE, '" + clsType.User.Sabun + "', WRTNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }


                    SQL = "";
                    SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST( ";
                for (i = 1; i < 45; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " RDATE, RSABUN, WRTNO) VALUES ( ";
                for (i = 1; i < 45; i++)
                {
                    SQL = SQL + ComNum.VBLF + "'" + strBun[i] + "', ";
                }
                SQL = SQL + ComNum.VBLF + " SYSDATE, '" + clsType.User.Sabun + "'," + argWRTNO + ")";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }
        }

        bool Save_ssView2(double argWRTNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strJEPCODE = "";
            string strBag = "";
            string strKcal = "";
            string strNPC = "";

            string[] strBun = new string[134];
            bool rtnVar = false;

            for (i = 0; i < strBun.Length; i++)
            {
                strBun[i] = "";
            }

            strBun[1] = ssView2_Sheet1.Cells[4, 9].Text.Trim(); //등록번호

            strBun[2] = ssView2_Sheet1.Cells[5, 9].Text.Trim(); //환자명

            strBun[3] = ssView2_Sheet1.Cells[6, 9].Text.Trim(); //성별
            strBun[4] = ssView2_Sheet1.Cells[6, 13].Text.Trim(); //나이

            strBun[5] = ssView2_Sheet1.Cells[7, 9].Text.Trim(); //진료과
            strBun[6] = ssView2_Sheet1.Cells[7, 13].Text.Trim(); //병실

            strBun[7] = ssView2_Sheet1.Cells[8, 8].Text.Trim(); //입원일자
            strBun[8] = ssView2_Sheet1.Cells[8, 19].Text.Trim();    //의뢰일자
            strBun[9] = ssView2_Sheet1.Cells[8, 33].Text.Trim();    //작성일자

            strBun[10] = Convert.ToBoolean(ssView2_Sheet1.Cells[12, 21].Value) == true ? "1" : "0";    //protein need
            strBun[11] = Convert.ToBoolean(ssView2_Sheet1.Cells[13, 21].Value) == true ? "1" : "0";
            strBun[12] = Convert.ToBoolean(ssView2_Sheet1.Cells[14, 21].Value) == true ? "1" : "0";
            strBun[13] = Convert.ToBoolean(ssView2_Sheet1.Cells[15, 21].Value) == true ? "1" : "0";
            strBun[14] = Convert.ToBoolean(ssView2_Sheet1.Cells[16, 21].Value) == true ? "1" : "0";

            strBun[15] = Convert.ToBoolean(ssView2_Sheet1.Cells[12, 40].Value) == true ? "1" : "0"; //NPC/N
            strBun[16] = Convert.ToBoolean(ssView2_Sheet1.Cells[13, 40].Value) == true ? "1" : "0";
            strBun[17] = Convert.ToBoolean(ssView2_Sheet1.Cells[14, 40].Value) == true ? "1" : "0";
            strBun[18] = Convert.ToBoolean(ssView2_Sheet1.Cells[15, 40].Value) == true ? "1" : "0";

            strBun[95] = ssView2_Sheet1.Cells[39, 31].Text.Trim();  //Total Volume Intake
            strBun[96] = ssView2_Sheet1.Cells[40, 31].Text.Trim();  //Total Calory Intake
            strBun[97] = ssView2_Sheet1.Cells[41, 31].Text.Trim();  //NPC/N

            strBun[19] = ssView2_Sheet1.Cells[19, 6].Text.Trim();   //Lab Data
            strBun[20] = ssView2_Sheet1.Cells[19, 14].Text.Trim();
            strBun[23] = ssView2_Sheet1.Cells[19, 22].Text.Trim();
            strBun[21] = ssView2_Sheet1.Cells[19, 30].Text.Trim();
            strBun[24] = ssView2_Sheet1.Cells[19, 38].Text.Trim();

            strBun[25] = ssView2_Sheet1.Cells[20, 6].Text.Trim();
            strBun[26] = ssView2_Sheet1.Cells[20, 14].Text.Trim();
            strBun[27] = ssView2_Sheet1.Cells[20, 22].Text.Trim();
            strBun[28] = ssView2_Sheet1.Cells[20, 30].Text.Trim();
            strBun[113] = ssView2_Sheet1.Cells[20, 38].Text.Trim();

            strBun[114] = ssView2_Sheet1.Cells[21, 6].Text.Trim();
            strBun[115] = ssView2_Sheet1.Cells[21, 14].Text.Trim();
            strBun[116] = ssView2_Sheet1.Cells[21, 22].Text.Trim();
            strBun[117] = ssView2_Sheet1.Cells[21, 30].Text.Trim();
            strBun[123] = ssView2_Sheet1.Cells[21, 38].Text.Trim();

            strBun[31] = ssView2_Sheet1.Cells[26, 9].Text.Trim();   //영양상태평가

            strBun[32] = Convert.ToBoolean(ssView2_Sheet1.Cells[27, 15].Value) == true ? "1" : "0";
            strBun[33] = Convert.ToBoolean(ssView2_Sheet1.Cells[27, 22].Value) == true ? "1" : "0";
            strBun[34] = Convert.ToBoolean(ssView2_Sheet1.Cells[27, 29].Value) == true ? "1" : "0";
            strBun[35] = Convert.ToBoolean(ssView2_Sheet1.Cells[27, 36].Value) == true ? "1" : "0";

            strBun[36] = Convert.ToBoolean(ssView2_Sheet1.Cells[28, 15].Value) == true ? "1" : "0";
            strBun[37] = Convert.ToBoolean(ssView2_Sheet1.Cells[28, 22].Value) == true ? "1" : "0";
            strBun[38] = Convert.ToBoolean(ssView2_Sheet1.Cells[28, 29].Value) == true ? "1" : "0";
            strBun[39] = Convert.ToBoolean(ssView2_Sheet1.Cells[28, 36].Value) == true ? "1" : "0";

            strBun[40] = Convert.ToBoolean(ssView2_Sheet1.Cells[29, 15].Value) == true ? "1" : "0";
            strBun[41] = Convert.ToBoolean(ssView2_Sheet1.Cells[29, 22].Value) == true ? "1" : "0";
            strBun[42] = Convert.ToBoolean(ssView2_Sheet1.Cells[29, 29].Value) == true ? "1" : "0";
            strBun[43] = Convert.ToBoolean(ssView2_Sheet1.Cells[29, 36].Value) == true ? "1" : "0";

            strBun[44] = ssView2_Sheet1.Cells[30, 9].Text.Trim();
            strBun[45] = ssView2_Sheet1.Cells[31, 9].Text.Trim();
            strBun[46] = ssView2_Sheet1.Cells[32, 9].Text.Trim();
            strBun[47] = ssView2_Sheet1.Cells[33, 9].Text.Trim();

            strBun[118] = Convert.ToBoolean(ssView2_Sheet1.Cells[34, 3].Value) == true ? "1" : "0";
            strBun[50] = Convert.ToBoolean(ssView2_Sheet1.Cells[34, 17].Value) == true ? "1" : "0";
            strBun[119] = Convert.ToBoolean(ssView2_Sheet1.Cells[34, 31].Value) == true ? "1" : "0";

            strBun[120] = Convert.ToBoolean(ssView2_Sheet1.Cells[35, 3].Value) == true ? "1" : "0";
            strBun[121] = Convert.ToBoolean(ssView2_Sheet1.Cells[35, 20].Value) == true ? "1" : "0";
            strBun[122] = Convert.ToBoolean(ssView2_Sheet1.Cells[35, 31].Value) == true ? "1" : "0";

            //strBun[54] = Convert.ToBoolean(ssView2_Sheet1.Cells[36, 3].Value) == true ? "1" : "0";
            //strBun[55] = Convert.ToBoolean(ssView2_Sheet1.Cells[36, 17].Value) == true ? "1" : "0";

            strBun[54] = "";
            strBun[55] = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M1 ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M1_HISTORY( ";
                    for (i = 1; i < 124; i++)
                    {
                        SQL = SQL + " BUN" + i.ToString("00") + ", ";
                    }
                    SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, WRTNO)  ";
                    SQL = SQL + ComNum.VBLF + " SELECT         ";
                    for (i = 1; i < 124; i++)
                    {
                        SQL = SQL + " BUN" + i.ToString("00") + ", ";
                    }
                    SQL = SQL + ComNum.VBLF + " SYSDATE, '" + clsType.User.Sabun + "', WRTNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M1 ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }

                    SQL = "";
                    SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_M1 ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }

                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M1( ";
                for (i = 1; i < 124; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " RDATE, RSABUN, WRTNO) VALUES ( ";
                for (i = 1; i < 124; i++)
                {
                    SQL = SQL + ComNum.VBLF + "'" + strBun[i] + "', ";
                }
                SQL = SQL + ComNum.VBLF + " SYSDATE, " + clsType.User.Sabun + "," + argWRTNO + ")";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                for (i = 43; i < ssView2_Sheet1.RowCount - 1; i++)
                {
                    strJEPCODE = ssView2_Sheet1.Cells[i, 2].Text.Trim();
                    strBag = ssView2_Sheet1.Cells[i, 31].Text.Trim();
                    strKcal = ssView2_Sheet1.Cells[i, 35].Text.Trim();
                    strNPC = ssView2_Sheet1.Cells[i, 38].Text.Trim();

                    SQL = "";
                    SQL = "DELETE " + ComNum.DB_PMPA + "DIET_NST_JEPLIST ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;
                    SQL = SQL + ComNum.VBLF + "    AND JEPCODE = '" + strJEPCODE + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }

                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_JEPLIST(";
                    SQL = SQL + ComNum.VBLF + " WRTNO, JEPCODE, BAG, KCAL, NPC) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + argWRTNO + ",'" + strJEPCODE + "','" + strBag + "','" + strKcal + "','" + strNPC + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }
        }

        bool Save_ssView2_EN(double argWRTNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string[] strBun = new string[94];
            bool rtnVar = false;

            strBun[1] = ssView2_EN_Sheet1.Cells[4, 9].Text.Trim();  //등록번호

            strBun[2] = ssView2_EN_Sheet1.Cells[5, 9].Text.Trim();  //환자명

            strBun[3] = ssView2_EN_Sheet1.Cells[6, 9].Text.Trim();  //성별
            strBun[4] = ssView2_EN_Sheet1.Cells[6, 13].Text.Trim(); //나이

            strBun[5] = ssView2_EN_Sheet1.Cells[7, 9].Text.Trim();  //진료과
            strBun[6] = ssView2_EN_Sheet1.Cells[7, 13].Text.Trim(); //병실

            strBun[7] = ssView2_EN_Sheet1.Cells[8, 8].Text.Trim();  //입원일자
            strBun[8] = ssView2_EN_Sheet1.Cells[8, 19].Text.Trim(); //의뢰일자
            strBun[9] = ssView2_EN_Sheet1.Cells[8, 33].Text.Trim(); //작성일자

            strBun[10] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[12, 21].Value) == true ? "1" : "0";  //protein need
            strBun[11] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[13, 21].Value) == true ? "1" : "0";
            strBun[12] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[14, 21].Value) == true ? "1" : "0";
            strBun[13] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[15, 21].Value) == true ? "1" : "0";
            strBun[14] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[16, 21].Value) == true ? "1" : "0";

            strBun[15] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[12, 40].Value) == true ? "1" : "0";  //NPC/N
            strBun[16] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[13, 40].Value) == true ? "1" : "0";
            strBun[17] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[14, 40].Value) == true ? "1" : "0";
            strBun[18] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[15, 40].Value) == true ? "1" : "0";

            //하모닐란
            strBun[71] = ssView2_EN_Sheet1.Cells[19, 33].Text.Trim();
            strBun[72] = ssView2_EN_Sheet1.Cells[19, 37].Text.Trim();

            //경관급식
            strBun[56] = ssView2_EN_Sheet1.Cells[20, 33].Text.Trim();
            strBun[57] = ssView2_EN_Sheet1.Cells[20, 37].Text.Trim();

            //경관설사식
            strBun[58] = ssView2_EN_Sheet1.Cells[21, 33].Text.Trim();
            strBun[59] = ssView2_EN_Sheet1.Cells[21, 37].Text.Trim();

            //경관당뇨식
            strBun[60] = ssView2_EN_Sheet1.Cells[22, 33].Text.Trim();
            strBun[61] = ssView2_EN_Sheet1.Cells[22, 37].Text.Trim();


            if (ssView2_EN_Sheet1.Cells[23, 11].Text == "경관신부전식")
            {
                //경관신부전식
                strBun[62] = ssView2_EN_Sheet1.Cells[23, 33].Text.Trim();
                strBun[63] = ssView2_EN_Sheet1.Cells[23, 37].Text.Trim();
            }
            else
            {
                //경관고단백
                strBun[86] = ssView2_EN_Sheet1.Cells[23, 33].Text.Trim();
                strBun[87] = ssView2_EN_Sheet1.Cells[23, 37].Text.Trim();
            }

            if (ssView2_EN_Sheet1.Cells[24, 11].Text == "경관투석식")
            {
                //경관투석식
                strBun[64] = ssView2_EN_Sheet1.Cells[24, 33].Text.Trim();
                strBun[65] = ssView2_EN_Sheet1.Cells[24, 37].Text.Trim();
            }
            else
            {
                //신부전식
                strBun[88] = ssView2_EN_Sheet1.Cells[24, 33].Text.Trim();
                strBun[89] = ssView2_EN_Sheet1.Cells[24, 37].Text.Trim();
            }

            if (ssView2_EN_Sheet1.Cells[25, 11].Text == "경관저단백식")
            {
                //경관저단백
                strBun[73] = ssView2_EN_Sheet1.Cells[25, 33].Text.Trim();
                strBun[74] = ssView2_EN_Sheet1.Cells[25, 37].Text.Trim();
            }
            else
            {   
                //투석식
                strBun[90] = ssView2_EN_Sheet1.Cells[25, 33].Text.Trim();
                strBun[91] = ssView2_EN_Sheet1.Cells[25, 37].Text.Trim();

            }


            if(ssView2_EN_Sheet1.Cells[26, 11].Text == "경관당뇨")  //2021-07-15 사용안함
            {
                //경관당뇨
                strBun[92] = ssView2_EN_Sheet1.Cells[26, 33].Text.Trim();
                strBun[93] = ssView2_EN_Sheet1.Cells[26, 37].Text.Trim();
            }


            strBun[68] = ssView2_EN_Sheet1.Cells[32, 33].Text.Trim();   //Total Volume Intake
            strBun[69] = ssView2_EN_Sheet1.Cells[33, 33].Text.Trim();   //Total Calory Intake
            strBun[70] = ssView2_EN_Sheet1.Cells[34, 33].Text.Trim();   //NPC/N

            strBun[19] = ssView2_EN_Sheet1.Cells[37, 6].Text.Trim();    //Lab Data
            strBun[20] = ssView2_EN_Sheet1.Cells[37, 14].Text.Trim();
            strBun[23] = ssView2_EN_Sheet1.Cells[37, 22].Text.Trim();
            strBun[21] = ssView2_EN_Sheet1.Cells[37, 30].Text.Trim();
            strBun[24] = ssView2_EN_Sheet1.Cells[37, 38].Text.Trim();

            strBun[25] = ssView2_EN_Sheet1.Cells[38, 6].Text.Trim();
            strBun[26] = ssView2_EN_Sheet1.Cells[38, 14].Text.Trim();
            strBun[27] = ssView2_EN_Sheet1.Cells[38, 22].Text.Trim();
            strBun[28] = ssView2_EN_Sheet1.Cells[38, 30].Text.Trim();
            strBun[75] = ssView2_EN_Sheet1.Cells[38, 38].Text.Trim();

            strBun[76] = ssView2_EN_Sheet1.Cells[39, 6].Text.Trim();
            strBun[77] = ssView2_EN_Sheet1.Cells[39, 14].Text.Trim();
            strBun[78] = ssView2_EN_Sheet1.Cells[39, 22].Text.Trim();
            strBun[79] = ssView2_EN_Sheet1.Cells[39, 30].Text.Trim();
            strBun[85] = ssView2_EN_Sheet1.Cells[39, 38].Text.Trim();

            strBun[31] = ssView2_EN_Sheet1.Cells[44, 9].Text.Trim();  //영양상태평가

            strBun[32] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[45, 15].Value) == true ? "1" : "0";
            strBun[33] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[45, 22].Value) == true ? "1" : "0";
            strBun[34] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[45, 29].Value) == true ? "1" : "0";
            strBun[35] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[45, 36].Value) == true ? "1" : "0";

            strBun[36] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[46, 15].Value) == true ? "1" : "0";
            strBun[37] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[46, 22].Value) == true ? "1" : "0";
            strBun[38] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[46, 29].Value) == true ? "1" : "0";
            strBun[39] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[46, 36].Value) == true ? "1" : "0";

            strBun[40] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[47, 15].Value) == true ? "1" : "0";
            strBun[41] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[47, 22].Value) == true ? "1" : "0";
            strBun[42] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[47, 29].Value) == true ? "1" : "0";
            strBun[43] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[47, 36].Value) == true ? "1" : "0";

            strBun[44] = ssView2_EN_Sheet1.Cells[48, 9].Text.Trim();
            strBun[45] = ssView2_EN_Sheet1.Cells[49, 9].Text.Trim();
            strBun[46] = ssView2_EN_Sheet1.Cells[50, 9].Text.Trim();
            strBun[47] = ssView2_EN_Sheet1.Cells[51, 9].Text.Trim();

            strBun[80] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[52, 3].Value) == true ? "1" : "0";
            strBun[50] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[52, 17].Value) == true ? "1" : "0";
            strBun[81] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[52, 31].Value) == true ? "1" : "0";

            strBun[82] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[53, 3].Value) == true ? "1" : "0";
            strBun[83] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[53, 20].Value) == true ? "1" : "0";
            strBun[84] = Convert.ToBoolean(ssView2_EN_Sheet1.Cells[53, 31].Value) == true ? "1" : "0";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M1_EN ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = " INSERT INTO  " + ComNum.DB_PMPA + "DIET_NST_M1_EN_HISTORY( ";
                    for (i = 1; i < 94; i++)
                    {
                        SQL = SQL + " BUN" + i.ToString("00") + ", ";
                    }
                    SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, WRTNO)  ";
                    SQL = SQL + ComNum.VBLF + " SELECT         ";
                    for (i = 1; i < 94; i++)
                    {
                        SQL = SQL + " BUN" + i.ToString("00") + ", ";
                    }
                    SQL = SQL + ComNum.VBLF + " SYSDATE, '" + clsType.User.Sabun + "', WRTNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M1_EN ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }

                    SQL = "";
                    SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_M1_EN ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M1_EN( ";
                for (i = 1; i < 94; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " RDATE, RSABUN, WRTNO) VALUES ( ";
                for (i = 1; i < 94; i++)
                {
                    SQL = SQL + ComNum.VBLF + "'" + strBun[i] + "', ";
                }
                SQL = SQL + ComNum.VBLF + " SYSDATE, " + clsType.User.Sabun + "," + argWRTNO + ")";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }
        }

        bool Save_ssView3(double argWRTNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string[] strBun = new string[38];
            bool rtnVar = false;

            strBun[22] = ssView3_Sheet1.Cells[4, 7].Text.Trim();

            strBun[23] = Convert.ToBoolean(ssView3_Sheet1.Cells[7, 3].Value) == true ? "1" : "0";
            strBun[24] = Convert.ToBoolean(ssView3_Sheet1.Cells[7, 11].Value) == true ? "1" : "0";
            strBun[25] = Convert.ToBoolean(ssView3_Sheet1.Cells[7, 17].Value) == true ? "1" : "0";
            strBun[26] = Convert.ToBoolean(ssView3_Sheet1.Cells[7, 23].Value) == true ? "1" : "0";
            strBun[27] = Convert.ToBoolean(ssView3_Sheet1.Cells[7, 29].Value) == true ? "1" : "0";
            strBun[28] = Convert.ToBoolean(ssView3_Sheet1.Cells[7, 36].Value) == true ? "1" : "0";

            strBun[29] = ssView3_Sheet1.Cells[10, 3].Text.Trim();
            strBun[31] = ssView3_Sheet1.Cells[10, 23].Text.Trim();

            strBun[1] = ssView3_Sheet1.Cells[13, 9].Text.Trim(); //Energy
            strBun[2] = ssView3_Sheet1.Cells[13, 19].Text.Trim();
            strBun[3] = ssView3_Sheet1.Cells[13, 23].Text.Trim();
            strBun[4] = ssView3_Sheet1.Cells[13, 36].Text.Trim();    //NPC
            strBun[5] = ssView3_Sheet1.Cells[13, 40].Text.Trim();

            strBun[32] = ssView3_Sheet1.Cells[14, 9].Text.Trim();   //Protein
            strBun[33] = ssView3_Sheet1.Cells[14, 13].Text.Trim();

            strBun[34] = ssView3_Sheet1.Cells[15, 9].Text.Trim();   //Glucose
            strBun[35] = ssView3_Sheet1.Cells[15, 13].Text.Trim();

            strBun[36] = ssView3_Sheet1.Cells[16, 9].Text.Trim();    //Fat
            strBun[37] = ssView3_Sheet1.Cells[16, 13].Text.Trim();

            strBun[15] = ssView3_Sheet1.Cells[17, 9].Text.Trim();    //Fluid

            strBun[16] = ssView3_Sheet1.Cells[18, 9].Text.Trim();   //기타

            strBun[17] = ssView3_Sheet1.Cells[21, 2].Text.Trim();    //recommend
            strBun[18] = ssView3_Sheet1.Cells[24, 2].Text.Trim();    //comment
            strBun[19] = ssView3_Sheet1.Cells[27, 2].Text.Trim();    //monitor
            strBun[30] = ssView3_Sheet1.Cells[30, 2].Text.Trim();    //monitor

            strBun[20] = ssView3_Sheet1.Cells[33, 29].Text.Trim();   //영양사
            strBun[21] = ssView3_Sheet1.Cells[33, 38].Text.Trim();   //약사

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M2 ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = " INSERT INTO  " + ComNum.DB_PMPA + "DIET_NST_M2_HISTORY( ";
                    for (i = 1; i < 38; i++)
                    {
                        SQL = SQL + " BUN" + i.ToString("00") + ", ";
                    }
                    SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, WRTNO)  ";
                    SQL = SQL + ComNum.VBLF + " SELECT         ";
                    for (i = 1; i < 38; i++)
                    {
                        SQL = SQL + " BUN" + i.ToString("00") + ", ";
                    }
                    SQL = SQL + ComNum.VBLF + " SYSDATE, '" + clsType.User.Sabun + "', WRTNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M2 ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }

                    SQL = "";
                    SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_M2 ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M2( ";
                for (i = 1; i < 38; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " RDATE, RSABUN, WRTNO) VALUES ( ";
                for (i = 1; i < 38; i++)
                {
                    SQL = SQL + ComNum.VBLF + "'" + strBun[i] + "', ";
                }
                SQL = SQL + ComNum.VBLF + " SYSDATE, " + clsType.User.Sabun + "," + argWRTNO + ")";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }


                if (strBun[17].Trim() != "" && clsPat.PATi.OrderNo != "")
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_MED + "OCS_ITRANSFER_HISTORY";
                    SQL = SQL + ComNum.VBLF + " ( PTNO, ENTDATE, BDATE, FRDEPTCODE, FRDRCODE, TODEPTCODE, TODRCODE,";
                    SQL = SQL + ComNum.VBLF + "  GBFLAG, GBCONFIRM, INPDATE, INPID, FRREMARK, TOREMARK, BINPID, GBPRINT,";
                    SQL = SQL + ComNum.VBLF + "  GBCONFIRM_OLD, SDATE, EDATE, ORDERNO, GBDEL, RETURN, SNAME, AGE, SEX,";
                    SQL = SQL + ComNum.VBLF + "  WARDCODE, ROOMCODE, IPDNO, CDATE, CSABUN, NURSEOK, GBNST,";
                    SQL = SQL + ComNum.VBLF + "  GBSEND, SMS_SEND, SMS_REQ)";
                    SQL = SQL + ComNum.VBLF + " SELECT  PTNO, ENTDATE, BDATE, FRDEPTCODE, FRDRCODE, TODEPTCODE, TODRCODE,";
                    SQL = SQL + ComNum.VBLF + "  GBFLAG, GBCONFIRM, INPDATE, INPID, FRREMARK, TOREMARK, BINPID, GBPRINT,";
                    SQL = SQL + ComNum.VBLF + "  GBCONFIRM_OLD, SDATE, EDATE, ORDERNO, GBDEL, RETURN, SNAME, AGE, SEX,";
                    SQL = SQL + ComNum.VBLF + "  WARDCODE, ROOMCODE, IPDNO, CDATE, CSABUN, NURSEOK, GBNST,";
                    SQL = SQL + ComNum.VBLF + "  GBSEND , SMS_SEND, SMS_REQ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_ITRANSFER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + clsPat.PATi.Pano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + clsPat.PATi.IPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + clsPat.PATi.OrderNo;
                    SQL = SQL + ComNum.VBLF + "   AND GBNST IS NOT NULL ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }


                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_MED + "OCS_ITRANSFER SET ";
                    SQL = SQL + ComNum.VBLF + " GBCONFIRM = '*',";
                    SQL = SQL + ComNum.VBLF + " INPDATE = TO_DATE('" + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " INPID = '" + clsPat.PATi.DRSABUN + "', ";
                    SQL = SQL + ComNum.VBLF + " TOREMARK = '" + strBun[17] + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + clsPat.PATi.Pano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + clsPat.PATi.IPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + clsPat.PATi.OrderNo;
                    SQL = SQL + ComNum.VBLF + "   AND GBNST IS NOT NULL ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }
        }

        bool Save_ssView3_Progress(double argWRTNO)
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strCHANGE = "";
            string strROWID = "";
            string strDate = "";
            string strCONTENTS = "";

            bool rtnVar = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 5; i < ssView3_Progress_Sheet1.RowCount; i++)
                {
                    strCHANGE = ssView3_Progress_Sheet1.Cells[i, 4].Text.Trim();
                    strROWID = ssView3_Progress_Sheet1.Cells[i, 5].Text.Trim();

                    if (strCHANGE == "Y")
                    {
                        strDate = ssView3_Progress_Sheet1.Cells[i, 2].Text.Trim();
                        strCONTENTS = ssView3_Progress_Sheet1.Cells[i, 6].Text.Trim();

                        if (strDate == "")
                        {
                            strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");
                        }

                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M2_PROGRESS_HIS ";
                            SQL = SQL + ComNum.VBLF + " SELECT * FROM " + ComNum.DB_PMPA + "DIET_NST_M2_PROGRESS ";
                            SQL = SQL + ComNum.VBLF + "   WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return rtnVar;
                            }

                            SQL = "";
                            SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_M2_PROGRESS";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return rtnVar;
                            }
                        }

                        SQL = "";
                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M2_PROGRESS (";
                        SQL = SQL + ComNum.VBLF + " WRTNO, WDATE, WSABUN, CONTENTS) VALUES ( ";
                        SQL = SQL + ComNum.VBLF + argWRTNO + ", TO_DATE('" + strDate + "','YYYY-MM-DD HH24:MI:SS'), " + clsType.User.Sabun + ", '" + strCONTENTS + "') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return rtnVar;
                        }
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }
        }

        bool Save_ssView4(double argWRTNO)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string[] strBun = new string[323];
            bool rtnVar = false;

            strBun[1] = ssView4_Sheet1.Cells[4, 9].Text.Trim();  //등록번호

            strBun[2] = ssView4_Sheet1.Cells[5, 9].Text.Trim();  //환자명


            strBun[3] = ssView4_Sheet1.Cells[6, 9].Text.Trim();    //성별
            strBun[4] = ssView4_Sheet1.Cells[6, 13].Text.Trim();   //나이

            strBun[5] = ssView4_Sheet1.Cells[7, 9].Text.Trim();  //진료과
            strBun[6] = ssView4_Sheet1.Cells[7, 13].Text.Trim(); //병실

            strBun[7] = ssView4_Sheet1.Cells[8, 8].Text.Trim();      //입원일자
            strBun[8] = ssView4_Sheet1.Cells[8, 19].Text.Trim();     //의뢰일자
            strBun[9] = ssView4_Sheet1.Cells[8, 33].Text.Trim();     //작성일자

            strBun[10] = ssView4_Sheet1.Cells[11, 10].Text.Trim();   //키
            strBun[11] = ssView4_Sheet1.Cells[11, 17].Text.Trim();   //몸무게
            strBun[12] = ssView4_Sheet1.Cells[11, 24].Text.Trim();   //UBW
            strBun[13] = ssView4_Sheet1.Cells[11, 31].Text.Trim();   //IBW
            strBun[14] = ssView4_Sheet1.Cells[11, 37].Text.Trim();   //IBW%

            k = 15;
            for (i = 11; i <= 41; i += 3)
            {
                for (j = 15; j <= 21; j++)
                {
                    strBun[k] = ssView4_Sheet1.Cells[j - 1, i - 1].Text.Trim();
                    k++;
                }
            }

            for (i = 24; i <= 42; i++)
            {
                strBun[k] = ssView4_Sheet1.Cells[i - 1, 2].Text.Trim();
                k++;
            }

            for (i = 11; i <= 41; i += 3)
            {
                for (j = 24; j <= 42; j++)
                {
                    strBun[k] = ssView4_Sheet1.Cells[j - 1, i - 1].Text.Trim();
                    k++;
                }
            }

            strBun[k] = ssView4_Sheet1.Cells[43, 10].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M3 ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = " INSERT INTO  " + ComNum.DB_PMPA + "DIET_NST_M3_HISTORY( ";
                    for (i = 1; i <= 322; i++)
                    {
                        SQL = SQL + " BUN" + i.ToString("000") + ", ";
                    }
                    SQL = SQL + ComNum.VBLF + " DELDATE, DELSABUN, WRTNO)  ";
                    SQL = SQL + ComNum.VBLF + " SELECT";
                    for (i = 1; i <= 322; i++)
                    {
                        SQL = SQL + " BUN" + i.ToString("000") + ", ";
                    }
                    SQL = SQL + ComNum.VBLF + " SYSDATE, '" + clsType.User.Sabun + "', WRTNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M3 ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }

                    SQL = "";
                    SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_M3 ";
                    SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVar;
                    }
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M3( ";
                for (i = 1; i <= 322; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("000") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " RDATE, RSABUN, WRTNO) VALUES ( ";
                for (i = 1; i <= 322; i++)
                {
                    SQL = SQL + ComNum.VBLF + "'" + strBun[i] + "', ";
                }
                SQL = SQL + ComNum.VBLF + " SYSDATE, " + clsType.User.Sabun + "," + argWRTNO + ")";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVar;
            }
        }

        bool Display_ssView(double argWRTNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            bool rtnVar = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT";
                for (i = 1; i <= 44; i++)
                {
                    SQL = SQL + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.Cells[4, 9].Text = dt.Rows[0]["BUN01"].ToString().Trim();    //등록번호
                    ssView_Sheet1.Cells[4, 22].Text = dt.Rows[0]["BUN02"].ToString().Trim();   //성별
                    ssView_Sheet1.Cells[4, 26].Text = dt.Rows[0]["BUN03"].ToString().Trim();   //나이
                    ssView_Sheet1.Cells[4, 36].Text = dt.Rows[0]["BUN04"].ToString().Trim();   //입원일자

                    ssView_Sheet1.Cells[5, 9].Text = dt.Rows[0]["BUN05"].ToString().Trim();    //환자명
                    ssView_Sheet1.Cells[5, 22].Text = dt.Rows[0]["BUN06"].ToString().Trim();   //진료과
                    ssView_Sheet1.Cells[5, 26].Text = dt.Rows[0]["BUN07"].ToString().Trim();   //병실
                    ssView_Sheet1.Cells[5, 36].Text = dt.Rows[0]["BUN08"].ToString().Trim();   //의뢰일자

                    ssView_Sheet1.Cells[6, 9].Text = dt.Rows[0]["BUN09"].ToString().Trim();    //진단명

                    ssView_Sheet1.Cells[9, 2].Text = dt.Rows[0]["BUN10"].ToString().Trim();    //의뢰내역

                    ssView_Sheet1.Cells[14, 2].Text = dt.Rows[0]["BUN11"].ToString().Trim();   //체중
                    ssView_Sheet1.Cells[14, 15].Text = dt.Rows[0]["BUN12"].ToString().Trim();  //키
                    ssView_Sheet1.Cells[14, 23].Text = dt.Rows[0]["BUN13"].ToString().Trim();  //나이
                    ssView_Sheet1.Cells[14, 32].Text = dt.Rows[0]["BUN14"].ToString().Trim();  //BEE

                    ssView_Sheet1.Cells[16, 3].Text = dt.Rows[0]["BUN15"].ToString().Trim();   //AF-1
                    ssView_Sheet1.Cells[16, 15].Text = dt.Rows[0]["BUN16"].ToString().Trim();  //AF-2

                    ssView_Sheet1.Cells[19, 3].Text = dt.Rows[0]["BUN17"].ToString().Trim();   //IF-1
                    ssView_Sheet1.Cells[19, 13].Text = dt.Rows[0]["BUN18"].ToString().Trim();  //IF-1
                    ssView_Sheet1.Cells[19, 23].Text = dt.Rows[0]["BUN19"].ToString().Trim();  //IF-1
                    ssView_Sheet1.Cells[19, 34].Text = dt.Rows[0]["BUN20"].ToString().Trim();  //IF-1

                    ssView_Sheet1.Cells[20, 3].Text = dt.Rows[0]["BUN21"].ToString().Trim();   //IF-1
                    ssView_Sheet1.Cells[20, 13].Text = dt.Rows[0]["BUN22"].ToString().Trim();  //IF-1
                    ssView_Sheet1.Cells[20, 23].Text = dt.Rows[0]["BUN23"].ToString().Trim();  //IF-1
                    ssView_Sheet1.Cells[20, 34].Text = dt.Rows[0]["BUN24"].ToString().Trim();  //IF-1

                    ssView_Sheet1.Cells[21, 13].Text = dt.Rows[0]["BUN26"].ToString().Trim();  //IF-1

                    ssView_Sheet1.Cells[23, 2].Text = dt.Rows[0]["BUN27"].ToString().Trim();   //BEE
                    ssView_Sheet1.Cells[23, 13].Text = dt.Rows[0]["BUN28"].ToString().Trim();  //AF
                    ssView_Sheet1.Cells[23, 21].Text = dt.Rows[0]["BUN29"].ToString().Trim();  //IF
                    ssView_Sheet1.Cells[23, 30].Text = dt.Rows[0]["BUN30"].ToString().Trim();  //1일 필요 칼로리

                    ssView_Sheet1.Cells[26, 3].Text = dt.Rows[0]["BUN31"].ToString().Trim();   //GIT FUNCTION-1
                    ssView_Sheet1.Cells[26, 13].Text = dt.Rows[0]["BUN32"].ToString().Trim();  //GIT FUNCTION-2
                    ssView_Sheet1.Cells[26, 21].Text = dt.Rows[0]["BUN33"].ToString().Trim();  //GIT FUNCTION

                    ssView_Sheet1.Cells[29, 8].Text = dt.Rows[0]["BUN34"].ToString().Trim();   //TPN-1
                    ssView_Sheet1.Cells[29, 17].Text = dt.Rows[0]["BUN35"].ToString().Trim();  //TPN-2

                    ssView_Sheet1.Cells[30, 7].Text = dt.Rows[0]["BUN44"].ToString().Trim();   //EN-7 (ORAL)
                    ssView_Sheet1.Cells[30, 10].Text = dt.Rows[0]["BUN36"].ToString().Trim();  //EN-1
                    ssView_Sheet1.Cells[30, 13].Text = dt.Rows[0]["BUN37"].ToString().Trim();  //EN-2
                    ssView_Sheet1.Cells[30, 16].Text = dt.Rows[0]["BUN38"].ToString().Trim();  //EN-3
                    ssView_Sheet1.Cells[30, 21].Text = dt.Rows[0]["BUN39"].ToString().Trim();  //EN-4
                    ssView_Sheet1.Cells[30, 26].Text = dt.Rows[0]["BUN40"].ToString().Trim();  //EN-5
                    ssView_Sheet1.Cells[30, 34].Text = dt.Rows[0]["BUN41"].ToString().Trim();  //EN-6

                    ssView_Sheet1.Cells[33, 2].Text = dt.Rows[0]["BUN42"].ToString().Trim();   //CONSULTING PLAN
                    ssView_Sheet1.SetRowHeight(33, Convert.ToInt32(ssView_Sheet1.GetPreferredRowHeight(33)) + 20);

                    ssView_Sheet1.Cells[35, 31].Text = dt.Rows[0]["BUN43"].ToString().Trim();  //담당의사

                    if (ssView_Sheet1.Cells[35, 31].Text == "")
                    {
                        ssView_Sheet1.Cells[35, 31].Text = lblDoing1.Text.Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        bool Display_ssView2(double argWRTNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            bool rtnVar = false;

            DrawJepcode(clsPat.PATi.WRTNO);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT";
                for (i = 1; i <= 123; i++)
                {
                    SQL = SQL + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M1 ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.Cells[4, 9].Text = dt.Rows[0]["BUN01"].ToString().Trim();    //등록번호

                    ssView2_Sheet1.Cells[5, 9].Text = dt.Rows[0]["BUN02"].ToString().Trim();   //환자명

                    ssView2_Sheet1.Cells[6, 9].Text = dt.Rows[0]["BUN03"].ToString().Trim();   //성별
                    ssView2_Sheet1.Cells[6, 13].Text = dt.Rows[0]["BUN04"].ToString().Trim();   //나이

                    ssView2_Sheet1.Cells[7, 9].Text = dt.Rows[0]["BUN05"].ToString().Trim();    //진료과
                    ssView2_Sheet1.Cells[7, 13].Text = dt.Rows[0]["BUN06"].ToString().Trim();   //병실

                    ssView2_Sheet1.Cells[8, 8].Text = dt.Rows[0]["BUN07"].ToString().Trim();   //입원일자
                    ssView2_Sheet1.Cells[8, 19].Text = dt.Rows[0]["BUN08"].ToString().Trim();   //의뢰일자
                    ssView2_Sheet1.Cells[8, 33].Text = dt.Rows[0]["BUN09"].ToString().Trim();    //작성일자

                    ssView2_Sheet1.Cells[12, 21].Text = dt.Rows[0]["BUN10"].ToString().Trim();   //protein need
                    ssView2_Sheet1.Cells[13, 21].Text = dt.Rows[0]["BUN11"].ToString().Trim();
                    ssView2_Sheet1.Cells[14, 21].Text = dt.Rows[0]["BUN12"].ToString().Trim();
                    ssView2_Sheet1.Cells[15, 21].Text = dt.Rows[0]["BUN13"].ToString().Trim();
                    ssView2_Sheet1.Cells[16, 21].Text = dt.Rows[0]["BUN14"].ToString().Trim();

                    ssView2_Sheet1.Cells[39, 31].Text = dt.Rows[0]["BUN95"].ToString().Trim();  //Total Volume Intake
                    ssView2_Sheet1.Cells[40, 31].Text = dt.Rows[0]["BUN96"].ToString().Trim();  //Total Calory Intake
                    ssView2_Sheet1.Cells[41, 31].Text = dt.Rows[0]["BUN97"].ToString().Trim();  //NPC/N

                    if (clsPat.PATi.WRTNO == 0)
                    {
                        Read_ResultDate();
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[19, 6].Text = dt.Rows[0]["BUN19"].ToString().Trim();    //Lab Data
                        ssView2_Sheet1.Cells[19, 14].Text = dt.Rows[0]["BUN20"].ToString().Trim();
                        ssView2_Sheet1.Cells[19, 22].Text = dt.Rows[0]["BUN23"].ToString().Trim();
                        ssView2_Sheet1.Cells[19, 30].Text = dt.Rows[0]["BUN21"].ToString().Trim();
                        ssView2_Sheet1.Cells[19, 38].Text = dt.Rows[0]["BUN24"].ToString().Trim();

                        ssView2_Sheet1.Cells[20, 6].Text = dt.Rows[0]["BUN25"].ToString().Trim();
                        ssView2_Sheet1.Cells[20, 14].Text = dt.Rows[0]["BUN26"].ToString().Trim();
                        ssView2_Sheet1.Cells[20, 22].Text = dt.Rows[0]["BUN27"].ToString().Trim();
                        ssView2_Sheet1.Cells[20, 30].Text = dt.Rows[0]["BUN28"].ToString().Trim();
                        ssView2_Sheet1.Cells[20, 38].Text = dt.Rows[0]["BUN113"].ToString().Trim();

                        ssView2_Sheet1.Cells[21, 6].Text = dt.Rows[0]["BUN114"].ToString().Trim();
                        ssView2_Sheet1.Cells[21, 14].Text = dt.Rows[0]["BUN115"].ToString().Trim();
                        ssView2_Sheet1.Cells[21, 22].Text = dt.Rows[0]["BUN116"].ToString().Trim();
                        ssView2_Sheet1.Cells[21, 30].Text = dt.Rows[0]["BUN117"].ToString().Trim();
                        ssView2_Sheet1.Cells[21, 38].Text = dt.Rows[0]["BUN123"].ToString().Trim();
                    }

                    ssView2_Sheet1.Cells[26, 9].Text = dt.Rows[0]["BUN31"].ToString().Trim();  //영양상태평가

                    ssView2_Sheet1.Cells[27, 15].Text = dt.Rows[0]["BUN32"].ToString().Trim();
                    ssView2_Sheet1.Cells[27, 22].Text = dt.Rows[0]["BUN33"].ToString().Trim();
                    ssView2_Sheet1.Cells[27, 29].Text = dt.Rows[0]["BUN34"].ToString().Trim();
                    ssView2_Sheet1.Cells[27, 36].Text = dt.Rows[0]["BUN35"].ToString().Trim();

                    ssView2_Sheet1.Cells[28, 15].Text = dt.Rows[0]["BUN36"].ToString().Trim();
                    ssView2_Sheet1.Cells[28, 22].Text = dt.Rows[0]["BUN37"].ToString().Trim();
                    ssView2_Sheet1.Cells[28, 29].Text = dt.Rows[0]["BUN38"].ToString().Trim();
                    ssView2_Sheet1.Cells[28, 36].Text = dt.Rows[0]["BUN39"].ToString().Trim();

                    ssView2_Sheet1.Cells[29, 15].Text = dt.Rows[0]["BUN40"].ToString().Trim();
                    ssView2_Sheet1.Cells[29, 22].Text = dt.Rows[0]["BUN41"].ToString().Trim();
                    ssView2_Sheet1.Cells[29, 29].Text = dt.Rows[0]["BUN42"].ToString().Trim();
                    ssView2_Sheet1.Cells[29, 36].Text = dt.Rows[0]["BUN43"].ToString().Trim();

                    ssView2_Sheet1.Cells[30, 9].Text = dt.Rows[0]["BUN44"].ToString().Trim();
                    ssView2_Sheet1.Cells[31, 9].Text = dt.Rows[0]["BUN45"].ToString().Trim();
                    ssView2_Sheet1.Cells[32, 9].Text = dt.Rows[0]["BUN46"].ToString().Trim();
                    ssView2_Sheet1.Cells[33, 9].Text = dt.Rows[0]["BUN47"].ToString().Trim();

                    ssView2_Sheet1.Cells[34, 3].Text = dt.Rows[0]["BUN118"].ToString().Trim();
                    ssView2_Sheet1.Cells[34, 17].Text = dt.Rows[0]["BUN50"].ToString().Trim();
                    ssView2_Sheet1.Cells[34, 31].Text = dt.Rows[0]["BUN119"].ToString().Trim();

                    ssView2_Sheet1.Cells[35, 3].Text = dt.Rows[0]["BUN120"].ToString().Trim();
                    ssView2_Sheet1.Cells[35, 17].Text = dt.Rows[0]["BUN121"].ToString().Trim();
                    ssView2_Sheet1.Cells[35, 31].Text = dt.Rows[0]["BUN122"].ToString().Trim();

                    ssView2_Sheet1.Cells[36, 3].Text = dt.Rows[0]["BUN54"].ToString().Trim();
                    ssView2_Sheet1.Cells[36, 17].Text = dt.Rows[0]["BUN55"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        bool Display_ssView2_EN(double argWRTNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strBUN62 = "";     //여기 값이 있으면 과거 챠트 표시
            string strBUN63 = "";
            string strBUN64 = "";
            string strBUN65 = "";
            string strBUN73 = "";
            string strBUN74 = "";

            bool rtnVar = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT";
                for (i = 1; i <= 93; i++)
                {
                    SQL = SQL + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M1_EN ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                } 
                if (dt.Rows.Count > 0) 
                {
                    ssView2_EN_Sheet1.Cells[4, 9].Text = dt.Rows[0]["BUN01"].ToString().Trim();    //등록번호

                    ssView2_EN_Sheet1.Cells[5, 9].Text = dt.Rows[0]["BUN02"].ToString().Trim();   //환자명

                    ssView2_EN_Sheet1.Cells[6, 9].Text = dt.Rows[0]["BUN03"].ToString().Trim();   //성별
                    ssView2_EN_Sheet1.Cells[6, 13].Text = dt.Rows[0]["BUN04"].ToString().Trim();   //나이

                    ssView2_EN_Sheet1.Cells[7, 9].Text = dt.Rows[0]["BUN05"].ToString().Trim();    //진료과
                    ssView2_EN_Sheet1.Cells[7, 13].Text = dt.Rows[0]["BUN06"].ToString().Trim();   //병실

                    ssView2_EN_Sheet1.Cells[8, 8].Text = dt.Rows[0]["BUN07"].ToString().Trim();   //입원일자
                    ssView2_EN_Sheet1.Cells[8, 19].Text = dt.Rows[0]["BUN08"].ToString().Trim();   //의뢰일자
                    ssView2_EN_Sheet1.Cells[8, 33].Text = dt.Rows[0]["BUN09"].ToString().Trim();    //작성일자

                    ssView2_EN_Sheet1.Cells[12, 21].Text = dt.Rows[0]["BUN10"].ToString().Trim();   //protein need
                    ssView2_EN_Sheet1.Cells[13, 21].Text = dt.Rows[0]["BUN11"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[14, 21].Text = dt.Rows[0]["BUN12"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[15, 21].Text = dt.Rows[0]["BUN13"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[16, 21].Text = dt.Rows[0]["BUN14"].ToString().Trim();

                    //경관급식
                    ssView2_EN_Sheet1.Cells[19, 33].Text = dt.Rows[0]["BUN71"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[19, 37].Text = dt.Rows[0]["BUN72"].ToString().Trim();

                    //경관급식
                    ssView2_EN_Sheet1.Cells[20, 33].Text = dt.Rows[0]["BUN56"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[20, 37].Text = dt.Rows[0]["BUN57"].ToString().Trim();

                    //경관설사식
                    ssView2_EN_Sheet1.Cells[21, 33].Text = dt.Rows[0]["BUN58"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[21, 37].Text = dt.Rows[0]["BUN59"].ToString().Trim();

                    //경관당뇨식
                    ssView2_EN_Sheet1.Cells[22, 33].Text = dt.Rows[0]["BUN60"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[22, 37].Text = dt.Rows[0]["BUN61"].ToString().Trim();


                
                    strBUN62 = dt.Rows[0]["BUN62"].ToString().Trim();
                    strBUN63 = dt.Rows[0]["BUN63"].ToString().Trim();
                    strBUN64 = dt.Rows[0]["BUN64"].ToString().Trim();
                    strBUN65 = dt.Rows[0]["BUN65"].ToString().Trim();
                    strBUN73 = dt.Rows[0]["BUN73"].ToString().Trim();
                    strBUN74 = dt.Rows[0]["BUN74"].ToString().Trim();

                    if (strBUN62 != "" || strBUN63 != "" || strBUN64 != "" || strBUN65 != "" || strBUN73 != "" || strBUN74 != "" )
                    {
                        //VOL KCAL    탄수화물 지방
                        //경관신부전식  200 400 64  17
                        //경관투석식   200 400 49  17
                        //경관저단백식  100 100 17  2.4
                        //[23, 17][23, 21][23, 24][23, 29]

                        //2019-11-04
                        //경관신부전식
                        ssView2_EN_Sheet1.Cells[23, 11].Text = "경관신부전식";
                        ssView2_EN_Sheet1.Cells[23, 17].Text = "200";
                        ssView2_EN_Sheet1.Cells[23, 21].Text = "400";
                        ssView2_EN_Sheet1.Cells[23, 24].Text = "64";
                        ssView2_EN_Sheet1.Cells[23, 29].Text = "17";
                        ssView2_EN_Sheet1.Cells[23, 33].Text = dt.Rows[0]["BUN62"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[23, 37].Text = dt.Rows[0]["BUN63"].ToString().Trim();

                        //경관투석식
                        ssView2_EN_Sheet1.Cells[24, 11].Text = "경관투석식";
                        ssView2_EN_Sheet1.Cells[24, 17].Text = "200";
                        ssView2_EN_Sheet1.Cells[24, 21].Text = "400";
                        ssView2_EN_Sheet1.Cells[24, 24].Text = "49";
                        ssView2_EN_Sheet1.Cells[24, 29].Text = "17";
                        ssView2_EN_Sheet1.Cells[24, 33].Text = dt.Rows[0]["BUN64"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[24, 37].Text = dt.Rows[0]["BUN65"].ToString().Trim();

                        //경관저단백
                        ssView2_EN_Sheet1.Cells[25, 11].Text = "경관저단백식";
                        ssView2_EN_Sheet1.Cells[25, 17].Text = "100";
                        ssView2_EN_Sheet1.Cells[25, 21].Text = "100";
                        ssView2_EN_Sheet1.Cells[25, 24].Text = "17";
                        ssView2_EN_Sheet1.Cells[25, 29].Text = "2.4";
                        ssView2_EN_Sheet1.Cells[25, 33].Text = dt.Rows[0]["BUN73"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[25, 37].Text = dt.Rows[0]["BUN74"].ToString().Trim();

                        //경관당뇨 - 탄수화물(11) / 지방(4.8)
                        ssView2_EN_Sheet1.Cells[26, 11].Text = "";
                        ssView2_EN_Sheet1.Cells[26, 17].Text = "";
                        ssView2_EN_Sheet1.Cells[26, 21].Text = "";
                        ssView2_EN_Sheet1.Cells[26, 24].Text = "";
                        ssView2_EN_Sheet1.Cells[26, 29].Text = "";
                        ssView2_EN_Sheet1.Cells[26, 33].Text = "";
                        ssView2_EN_Sheet1.Cells[26, 37].Text = "";
                    }
                    else
                    {
                        //경관고단백 / vol(ml) 68 / kcal(100) / 탄수화물(20.5) / 지방(5.5)
                        ssView2_EN_Sheet1.Cells[23, 11].Text = "경관고단백";
                        ssView2_EN_Sheet1.Cells[23, 17].Text = "67";
                        ssView2_EN_Sheet1.Cells[23, 21].Text = "100";
                        ssView2_EN_Sheet1.Cells[23, 24].Text = "13.8";
                        ssView2_EN_Sheet1.Cells[23, 29].Text = "3.3";
                        ssView2_EN_Sheet1.Cells[23, 33].Text = dt.Rows[0]["BUN86"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[23, 37].Text = dt.Rows[0]["BUN87"].ToString().Trim();

                        //경관신부전식 - 탄수화물(14.25) / 지방(3)
                        ssView2_EN_Sheet1.Cells[24, 11].Text = "경관신부전식";
                        ssView2_EN_Sheet1.Cells[24, 17].Text = "200";
                        ssView2_EN_Sheet1.Cells[24, 21].Text = "400";
                        ssView2_EN_Sheet1.Cells[24, 24].Text = "64";
                        ssView2_EN_Sheet1.Cells[24, 29].Text = "14";
                        ssView2_EN_Sheet1.Cells[24, 33].Text = dt.Rows[0]["BUN88"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[24, 37].Text = dt.Rows[0]["BUN89"].ToString().Trim();

                        //경관투석식 - 탄수화물(15) / 지방(3)
                        ssView2_EN_Sheet1.Cells[25, 11].Text = "경관투석식";
                        ssView2_EN_Sheet1.Cells[25, 17].Text = "200";
                        ssView2_EN_Sheet1.Cells[25, 21].Text = "400";
                        ssView2_EN_Sheet1.Cells[25, 24].Text = "53";
                        ssView2_EN_Sheet1.Cells[25, 29].Text = "15";
                        ssView2_EN_Sheet1.Cells[25, 33].Text = dt.Rows[0]["BUN90"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[25, 37].Text = dt.Rows[0]["BUN91"].ToString().Trim();

                        //사용안함
                        //ssView2_EN_Sheet1.Cells[26, 11].Text = "경관당뇨";
                        //ssView2_EN_Sheet1.Cells[26, 17].Text = "68";
                        //ssView2_EN_Sheet1.Cells[26, 21].Text = "100";
                        //ssView2_EN_Sheet1.Cells[26, 24].Text = "11";
                        //ssView2_EN_Sheet1.Cells[26, 29].Text = "4.8";
                        //ssView2_EN_Sheet1.Cells[26, 33].Text = dt.Rows[0]["BUN92"].ToString().Trim();
                        //ssView2_EN_Sheet1.Cells[26, 37].Text = dt.Rows[0]["BUN93"].ToString().Trim();
                    }


                    ssView2_EN_Sheet1.Cells[33, 32].Text = dt.Rows[0]["BUN68"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[33, 33].Text = dt.Rows[0]["BUN69"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[33, 34].Text = dt.Rows[0]["BUN70"].ToString().Trim();

                    if (clsPat.PATi.WRTNO == 0)
                    {
                        Read_ResultDate();
                    }
                    else
                    {
                        ssView2_EN_Sheet1.Cells[37, 6].Text = dt.Rows[0]["BUN19"].ToString().Trim();    //Lab Data
                        ssView2_EN_Sheet1.Cells[37, 14].Text = dt.Rows[0]["BUN20"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[37, 22].Text = dt.Rows[0]["BUN23"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[37, 30].Text = dt.Rows[0]["BUN21"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[37, 38].Text = dt.Rows[0]["BUN24"].ToString().Trim();

                        ssView2_EN_Sheet1.Cells[38, 6].Text = dt.Rows[0]["BUN25"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[38, 14].Text = dt.Rows[0]["BUN26"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[38, 22].Text = dt.Rows[0]["BUN27"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[38, 30].Text = dt.Rows[0]["BUN28"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[38, 38].Text = dt.Rows[0]["BUN75"].ToString().Trim();

                        ssView2_EN_Sheet1.Cells[39, 6].Text = dt.Rows[0]["BUN76"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[39, 14].Text = dt.Rows[0]["BUN77"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[39, 22].Text = dt.Rows[0]["BUN78"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[39, 30].Text = dt.Rows[0]["BUN79"].ToString().Trim();
                        ssView2_EN_Sheet1.Cells[39, 38].Text = dt.Rows[0]["BUN85"].ToString().Trim();
                    }

                    ssView2_EN_Sheet1.Cells[44, 9].Text = dt.Rows[0]["BUN31"].ToString().Trim();  //영양상태평가

                    ssView2_EN_Sheet1.Cells[45, 15].Text = dt.Rows[0]["BUN32"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[45, 22].Text = dt.Rows[0]["BUN33"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[45, 29].Text = dt.Rows[0]["BUN34"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[45, 36].Text = dt.Rows[0]["BUN35"].ToString().Trim();

                    ssView2_EN_Sheet1.Cells[46, 15].Text = dt.Rows[0]["BUN36"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[46, 22].Text = dt.Rows[0]["BUN37"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[46, 29].Text = dt.Rows[0]["BUN38"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[46, 36].Text = dt.Rows[0]["BUN39"].ToString().Trim();

                    ssView2_EN_Sheet1.Cells[47, 15].Text = dt.Rows[0]["BUN40"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[47, 22].Text = dt.Rows[0]["BUN41"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[47, 29].Text = dt.Rows[0]["BUN42"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[47, 36].Text = dt.Rows[0]["BUN43"].ToString().Trim();

                    ssView2_EN_Sheet1.Cells[48, 9].Text = dt.Rows[0]["BUN44"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[49, 9].Text = dt.Rows[0]["BUN45"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[50, 9].Text = dt.Rows[0]["BUN46"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[51, 9].Text = dt.Rows[0]["BUN47"].ToString().Trim();

                    ssView2_EN_Sheet1.Cells[52, 3].Text = dt.Rows[0]["BUN80"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[52, 17].Text = dt.Rows[0]["BUN50"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[52, 31].Text = dt.Rows[0]["BUN81"].ToString().Trim();

                    ssView2_EN_Sheet1.Cells[53, 3].Text = dt.Rows[0]["BUN82"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[53, 17].Text = dt.Rows[0]["BUN83"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[53, 31].Text = dt.Rows[0]["BUN84"].ToString().Trim();

                    ssView2_EN_Sheet1.Cells[36, 3].Text = dt.Rows[0]["BUN54"].ToString().Trim();
                    ssView2_EN_Sheet1.Cells[36, 17].Text = dt.Rows[0]["BUN55"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        bool Display_ssView3(double argWRTNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strDRSABUN = "";
            bool rtnVar = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT";
                for (i = 1; i <= 37; i++)
                {
                    SQL = SQL + " BUN" + i.ToString("00") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M2";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = dt.Rows[0]["BUN22"].ToString().Trim();

                    ssView3_Sheet1.Cells[7, 3].Value = dt.Rows[0]["BUN23"].ToString().Trim() == "1" ? true : false;
                    ssView3_Sheet1.Cells[7, 11].Value = dt.Rows[0]["BUN24"].ToString().Trim() == "1" ? true : false;
                    ssView3_Sheet1.Cells[7, 17].Value = dt.Rows[0]["BUN25"].ToString().Trim() == "1" ? true : false;
                    ssView3_Sheet1.Cells[7, 23].Value = dt.Rows[0]["BUN26"].ToString().Trim() == "1" ? true : false;
                    ssView3_Sheet1.Cells[7, 29].Value = dt.Rows[0]["BUN27"].ToString().Trim() == "1" ? true : false;
                    ssView3_Sheet1.Cells[7, 36].Value = dt.Rows[0]["BUN28"].ToString().Trim() == "1" ? true : false;

                    ssView3_Sheet1.Cells[10, 3].Text = dt.Rows[0]["BUN29"].ToString().Trim();
                    ssView3_Sheet1.Cells[10, 23].Text = dt.Rows[0]["BUN31"].ToString().Trim();

                    ssView3_Sheet1.Cells[13, 9].Text = dt.Rows[0]["BUN01"].ToString().Trim();   //Energy
                    ssView3_Sheet1.Cells[13, 19].Text = dt.Rows[0]["BUN02"].ToString().Trim();
                    ssView3_Sheet1.Cells[13, 23].Text = dt.Rows[0]["BUN03"].ToString().Trim();
                    ssView3_Sheet1.Cells[13, 36].Text = dt.Rows[0]["BUN04"].ToString().Trim();  //NPC
                    ssView3_Sheet1.Cells[13, 40].Text = dt.Rows[0]["BUN05"].ToString().Trim();

                    ssView3_Sheet1.Cells[14, 9].Text = dt.Rows[0]["BUN32"].ToString().Trim();   //Protein
                    ssView3_Sheet1.Cells[14, 13].Text = dt.Rows[0]["BUN33"].ToString().Trim();

                    ssView3_Sheet1.Cells[15, 9].Text = dt.Rows[0]["BUN34"].ToString().Trim();   //Glucose
                    ssView3_Sheet1.Cells[15, 13].Text = dt.Rows[0]["BUN35"].ToString().Trim();

                    ssView3_Sheet1.Cells[16, 9].Text = dt.Rows[0]["BUN36"].ToString().Trim();   //Fat
                    ssView3_Sheet1.Cells[16, 13].Text = dt.Rows[0]["BUN37"].ToString().Trim();

                    ssView3_Sheet1.Cells[17, 9].Text = dt.Rows[0]["BUN15"].ToString().Trim();   //Fluid

                    ssView3_Sheet1.Cells[18, 9].Text = dt.Rows[0]["BUN16"].ToString().Trim();   //기타

                    ssView3_Sheet1.Cells[21, 2].Text = dt.Rows[0]["BUN17"].ToString().Trim();   //recommend
                    ssView3_Sheet1.SetRowHeight(21, Convert.ToInt32(ssView3_Sheet1.GetPreferredRowHeight(21)) + 30);

                    ssView3_Sheet1.Cells[24, 2].Text = dt.Rows[0]["BUN18"].ToString().Trim();   //Comment
                    ssView3_Sheet1.SetRowHeight(24, Convert.ToInt32(ssView3_Sheet1.GetPreferredRowHeight(24)) + 5);

                    ssView3_Sheet1.Cells[27, 2].Text = dt.Rows[0]["BUN19"].ToString().Trim();   //Monitor
                    ssView3_Sheet1.SetRowHeight(27, Convert.ToInt32(ssView3_Sheet1.GetPreferredRowHeight(27)) + 5);

                    ssView3_Sheet1.Cells[30, 2].Text = dt.Rows[0]["BUN30"].ToString().Trim();   //Monitor
                    ssView3_Sheet1.SetRowHeight(30, Convert.ToInt32(ssView3_Sheet1.GetPreferredRowHeight(30)) + 5);

                    strDRSABUN = clsVbfunc.GetInSaName(clsDB.DbCon, clsPat.PATi.DRSABUN);
                    ssView3_Sheet1.Cells[33, 12].Text = strDRSABUN != "" ? "과장 " + clsVbfunc.GetInSaName(clsDB.DbCon, clsPat.PATi.DRSABUN) : "";    //의사
                    ssView3_Sheet1.Cells[33, 21].Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsPat.PATi.DTSABUN);    //영양사
                    ssView3_Sheet1.Cells[33, 29].Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsPat.PATi.PMSABUN);    //약사
                    ssView3_Sheet1.Cells[33, 32].Text = "간호사:";
                    ssView3_Sheet1.Cells[33, 37].Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsPat.PATi.NRSABUN);
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " SELECT BUN01,  BUN02, BUN03, BUN05, BUN06, BUN07, BUN30,";
                SQL = SQL + ComNum.VBLF + " ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView3_Sheet1.Cells[13, 9].Text = dt.Rows[0]["BUN30"].ToString().Trim();   //Energy

                    //등록번호/이름/성별/나이/진료과/병실
                    ssView3_Sheet1.Cells[37, 8].Text = dt.Rows[0]["BUN01"].ToString().Trim();
                    ssView3_Sheet1.Cells[37, 19].Text = dt.Rows[0]["BUN05"].ToString().Trim();
                    ssView3_Sheet1.Cells[37, 24].Text = dt.Rows[0]["BUN02"].ToString().Trim();
                    ssView3_Sheet1.Cells[37, 29].Text = dt.Rows[0]["BUN03"].ToString().Trim();
                    ssView3_Sheet1.Cells[37, 34].Text = dt.Rows[0]["BUN06"].ToString().Trim();
                    ssView3_Sheet1.Cells[37, 39].Text = dt.Rows[0]["BUN07"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        bool Display_ssView3_Progress(double argWRTNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string[] strbun = new string[8];
            bool rtnVar = false;

            for (i = 0; i < strbun.Length; i++)
            {
                strbun[i] = "";
            }

            ssView3_Progress_Sheet1.RowCount = 5;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(WDATE,'YYYY-MM-DD HH24:MI') WDATE, WSABUN, ROWID, CONTENTS ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M2_PROGRESS ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;
                SQL = SQL + ComNum.VBLF + " ORDER BY WDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        Add_Line();

                        ssView3_Progress_Sheet1.Cells[ssView3_Progress_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["WDATE"].ToString().Trim();
                        ssView3_Progress_Sheet1.Cells[ssView3_Progress_Sheet1.RowCount - 1, 3].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["WSABUN"].ToString().Trim());
                        ssView3_Progress_Sheet1.Cells[ssView3_Progress_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView3_Progress_Sheet1.Cells[ssView3_Progress_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["CONTENTS"].ToString().Trim();
                        ssView3_Progress_Sheet1.SetRowHeight(ssView3_Progress_Sheet1.RowCount - 1, Convert.ToInt32(ssView3_Progress_Sheet1.GetPreferredRowHeight(ssView3_Progress_Sheet1.RowCount - 1)) + 20);
                    }
                }

                dt.Dispose();
                dt = null;

                rtnVar = true;


                SQL = "";
                SQL = " SELECT BUN01,  BUN02, BUN03, BUN05, BUN06, BUN07,";
                SQL = SQL + ComNum.VBLF + " ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    rtnVar = false;
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    //등록번호/이름/성별/나이/진료과/병실
                    strbun[1] = dt.Rows[0]["BUN01"].ToString().Trim();
                    strbun[5] = dt.Rows[0]["BUN05"].ToString().Trim();
                    strbun[2] = dt.Rows[0]["BUN02"].ToString().Trim();
                    strbun[3] = dt.Rows[0]["BUN03"].ToString().Trim();
                    strbun[6] = dt.Rows[0]["BUN06"].ToString().Trim();
                    strbun[7] = dt.Rows[0]["BUN07"].ToString().Trim();

                    ssView3_Progress_Sheet1.Cells[1, 2].Text = "등록번호 : " + strbun[1] + "   이름 : " + strbun[5] + "   성별 : " + strbun[2] + "   나이 : " + strbun[3] + "   진료실 : " + strbun[6] + "   병실 : " + strbun[7];
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        bool Display_ssView4(double argWRTNO)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            bool rtnVar = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT";
                for (i = 1; i <= 322; i++)
                {
                    SQL = SQL + ComNum.VBLF + " BUN" + i.ToString("000") + ", ";
                }
                SQL = SQL + ComNum.VBLF + " ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_M3";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView4_Sheet1.Cells[4, 9].Text = dt.Rows[0]["BUN001"].ToString().Trim();   //등록번호

                    ssView4_Sheet1.Cells[5, 9].Text = dt.Rows[0]["BUN002"].ToString().Trim();   //환자명

                    ssView4_Sheet1.Cells[6, 9].Text = dt.Rows[0]["BUN003"].ToString().Trim();   //성별
                    ssView4_Sheet1.Cells[6, 13].Text = dt.Rows[0]["BUN004"].ToString().Trim();  //나이

                    ssView4_Sheet1.Cells[7, 9].Text = dt.Rows[0]["BUN005"].ToString().Trim();   //진료과
                    ssView4_Sheet1.Cells[7, 13].Text = dt.Rows[0]["BUN006"].ToString().Trim();  //병실

                    ssView4_Sheet1.Cells[8, 8].Text = dt.Rows[0]["BUN007"].ToString().Trim();   //입원일자
                    ssView4_Sheet1.Cells[8, 19].Text = dt.Rows[0]["BUN008"].ToString().Trim();  //의뢰일자
                    ssView4_Sheet1.Cells[8, 33].Text = dt.Rows[0]["BUN009"].ToString().Trim();  //작성일자

                    ssView4_Sheet1.Cells[11, 10].Text = dt.Rows[0]["BUN010"].ToString().Trim(); //키
                    ssView4_Sheet1.Cells[11, 17].Text = dt.Rows[0]["BUN011"].ToString().Trim(); //몸무게
                    ssView4_Sheet1.Cells[11, 24].Text = dt.Rows[0]["BUN012"].ToString().Trim(); //UBW
                    ssView4_Sheet1.Cells[11, 31].Text = dt.Rows[0]["BUN013"].ToString().Trim(); //IBW
                    ssView4_Sheet1.Cells[11, 37].Text = dt.Rows[0]["BUN014"].ToString().Trim(); //IBW%

                    k = 15;

                    for (i = 11; i <= 41; i += 3)
                    {
                        for (j = 15; j <= 21; j++)
                        {
                            ssView4_Sheet1.Cells[j - 1, i - 1].Text = dt.Rows[0]["BUN" + k.ToString("000")].ToString().Trim();
                            k++;
                        }
                    }

                    for (i = 24; i <= 42; i++)
                    {
                        ssView4_Sheet1.Cells[i - 1, 2].Text = dt.Rows[0]["BUN" + k.ToString("000")].ToString().Trim();
                        k++;
                    }

                    for (i = 11; i <= 41; i += 3)
                    {
                        for (j = 24; j <= 42; j++)
                        {
                            ssView4_Sheet1.Cells[j - 1, i - 1].Text = dt.Rows[0]["BUN" + k.ToString("000")].ToString().Trim();
                            k++;
                        }
                    }

                    ssView4_Sheet1.Cells[43, 10].Text = dt.Rows[0]["BUN" + k.ToString("000")].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                rtnVar = true;
                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        string Read_Diagnosis(double argIPDNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            try
            {
                SQL = "";
                SQL = " SELECT B.ILLNAMEK";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_IILLS A, ADMIN.BAS_ILLS B";
                SQL = SQL + ComNum.VBLF + " WHERE A.ILLCODE = B.ILLCODE ";
                SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND B.ILLCLASS = '1'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rtnVar += dt.Rows[i]["ILLNAMEK"].ToString().Trim() + ", ";
                }

                rtnVar = VB.Mid(rtnVar, 1, rtnVar.Length - 2);

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        //기본 변수 세팅
        void Var_Set()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT IPDNO, PANO, SNAME, SEX, ";
                SQL = SQL + ComNum.VBLF + " AGE, INDATE, DEPTCODE, DRCODE, ";
                SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, DIAGNOSIS, DRSABUN, ";
                SQL = SQL + ComNum.VBLF + " NRSABUN, PMSABUN, DTSABUN, RDATE, ";
                SQL = SQL + ComNum.VBLF + " BDATE, STATUS, COMPLITE, ORDERNO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_PROGRESS ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + clsPat.PATi.WRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                clsPat.PATi.IPDNO = VB.Val(dt.Rows[0]["IPDNO"].ToString().Trim());
                clsPat.PATi.Pano = dt.Rows[0]["PANO"].ToString().Trim();
                clsPat.PATi.sName = dt.Rows[0]["SNAME"].ToString().Trim();
                clsPat.PATi.Sex = dt.Rows[0]["SEX"].ToString().Trim();
                clsPat.PATi.Age = dt.Rows[0]["AGE"].ToString().Trim();
                clsPat.PATi.InDate = VB.Left(dt.Rows[0]["INDATE"].ToString().Trim(), 10);
                clsPat.PATi.DeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                clsPat.PATi.DrCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                clsPat.PATi.WardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                clsPat.PATi.RoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                clsPat.PATi.DIAGNOSIS = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                clsPat.PATi.DRSABUN = dt.Rows[0]["DRSABUN"].ToString().Trim();
                clsPat.PATi.NRSABUN = dt.Rows[0]["NRSABUN"].ToString().Trim();
                clsPat.PATi.PMSABUN = dt.Rows[0]["PMSABUN"].ToString().Trim();
                clsPat.PATi.DTSABUN = dt.Rows[0]["DTSABUN"].ToString().Trim();
                clsPat.PATi.OrderNo = dt.Rows[0]["ORDERNO"].ToString().Trim();
                txtComplete.Text = dt.Rows[0]["COMPLITE"].ToString().Trim();

                if (clsPat.PATi.DRSABUN != "")
                {
                    lblDoing1.Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsPat.PATi.DRSABUN);
                }
                if (clsPat.PATi.NRSABUN != "")
                {
                    lblDoing2.Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsPat.PATi.NRSABUN);
                }
                if (clsPat.PATi.PMSABUN != "")
                {
                    lblDoing3.Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsPat.PATi.PMSABUN);
                }
                if (clsPat.PATi.DTSABUN != "")
                {
                    lblDoing4.Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsPat.PATi.DTSABUN);
                }

                switch (dt.Rows[0]["STATUS"].ToString().Trim())
                {
                    case "0":
                        lblDoing0.Text = "접수";
                        break;
                    case "1":
                        lblDoing0.Text = "진행중";
                        break;
                    case "C":
                        lblDoing0.Text = "F/U종료";
                        break;
                    default:
                        lblDoing0.Text = "접수";
                        break;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Set_Clear()
        {
            FstrCLEAR = "OK";

            Var_Set();

            Clear_ssView();

             if (Display_ssView(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("Nutrition Support Chart를 읽어오는데 실패하였습니다.");
                return;
            }

            Clear_ssView2();

            if (Display_ssView2(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("Monitoring Page-1(PN) 를 읽어오는데 실패하였습니다.");
            }

            Clear_ssView2_EN();

            if (Display_ssView2_EN(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("Monitoring Page-1(EN) 를 읽어오는데 실패하였습니다.");
            }

            Clear_ssView3();

            if (Display_ssView3(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("Monitoring Page-2 를 읽어오는데 실패하였습니다.");
            }

            if (Display_ssView3_Progress(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("추가 Recommended Page를 읽어오는데 실패하였습니다.");
            }

            Clear_ssView4();

            if (Display_ssView4(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("Monitoring Page-2 를 읽어오는데 실패하였습니다.");
            }

            FstrCLEAR = "";
        }

        bool Cert_Nurse()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            bool rtnVar = false;

            try
            {
                SQL = "";
                SQL = " SELECT SABUN";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST";
                SQL = SQL + ComNum.VBLF + " WHERE JIK IN ('31','32','33','34')";
                SQL = SQL + ComNum.VBLF + "   AND SABUN = '" + clsType.User.Sabun + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                rtnVar = true;

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        private void ssView2_EN_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            int i = 0;
            double nValue = 0;
            double nTotal1 = 0;
            double nTotal2 = 0;
            string strM = "";
            string strS = "";
            string strValue = "";

            if (FstrCLEAR != "")
            {
                return;
            }

           if (e.Row >= 19 && e.Row <= 31)
            {
                if (e.Column == 33)
                {
                    strM = ssView2_EN_Sheet1.Cells[e.Row, 21].Text.Trim();
                    strS = ssView2_EN_Sheet1.Cells[e.Row, 17].Text.Trim();

                    if (strM != "" && strS != "")
                    {
                        nValue = VB.Val(strM) / VB.Val(strS);
                    }

                    strValue = ssView2_EN_Sheet1.Cells[e.Row, 33].Text.Trim();
                    ssView2_EN_Sheet1.Cells[e.Row, 37].Text = (nValue * VB.Val(strValue)).ToString("F2");

                    for (i = 20; i <= 32; i++)
                    {
                        nTotal1 += VB.Val(ssView2_EN_Sheet1.Cells[i - 1, 33].Text);
                        nTotal2 += VB.Val(ssView2_EN_Sheet1.Cells[i - 1, 37].Text);
                    }

                    ssView2_EN_Sheet1.Cells[32, 33].Text = nTotal1.ToString("F2");
                    ssView2_EN_Sheet1.Cells[33, 33].Text = nTotal2.ToString("F2");
                }
            }
        }

        private void ssView3_Progress_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strROWID = "";

            if (e.Row <= 4)
            {
                return;
            }

            if (e.Column == 2 || e.Column == 6)
            {
                strROWID = ssView3_Progress_Sheet1.Cells[e.Row, 5].Text.Trim();

                if (strROWID != "")
                {
                    try
                    {
                        SQL = "";
                        SQL = " SELECT * FROM " + ComNum.DB_PMPA + "DIET_NST_M2_PROGRESS ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND WSABUN = " + clsType.User.Sabun;

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("작성자 본인만 수정 가능합니다.");
                            return;
                        }

                        dt.Dispose();
                        dt = null;
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                }

                ssView3_Progress_Sheet1.Cells[e.Row, 4].Text = "Y";
            }

            if (e.Column == 6)
            {
                ssView3_Progress_Sheet1.SetRowHeight(e.Row, Convert.ToInt32(ssView3_Progress_Sheet1.GetPreferredRowHeight(e.Row)) + 4);
            }
        }

        private void ssView4_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 37 || e.Row != 22)
            {
                return;
            }

            panLab.Visible = true;

            Search_LabData();

            if (clsType.User.Sabun == "23515")
            {
                btnLabReset.Visible = true;
            }
        }

        void DrawJepcode(double argWRTNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strBDate = "";
            string strDeldate = "";

            ssView2_Sheet1.RowCount = 43;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.JEPCODE, A.VOL_BAG, A.KCAL_BAG, A.AA, A.GLUC, A.N, A.NPC, A.NPC_N, B.BAG, B.KCAL, B.NPC NPC2, TO_CHAR(A.DELDATE,'YYYY-MM-DD') DELDATE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_CODE A, " + ComNum.DB_PMPA + "DIET_NST_JEPLIST B";
                SQL = SQL + ComNum.VBLF + " WHERE A.JEPCODE = B.JEPCODE(+)";
                SQL = SQL + ComNum.VBLF + "  AND B.WRTNO(+) = " + argWRTNO;
                SQL = SQL + ComNum.VBLF + " ORDER BY RANKING ASC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strBDate = Read_WRTNO_Date(argWRTNO);
                        strDeldate = dt.Rows[i]["DELDATE"].ToString().Trim();

                        if (strDeldate == "" || (string.Compare(strBDate, strDeldate) < 0 && strBDate != ""))
                        {
                            ssView2_Sheet1.RowCount++;
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["VOL_BAG"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["KCAL_BAG"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 16].Text = dt.Rows[i]["AA"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 19].Text = dt.Rows[i]["GLUC"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 22].Text = dt.Rows[i]["N"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 25].Text = dt.Rows[i]["NPC"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 28].Text = dt.Rows[i]["NPC_N"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 31].Text = dt.Rows[i]["BAG"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 35].Text = dt.Rows[i]["KCAL"].ToString().Trim();
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 38].Text = dt.Rows[i]["NPC2"].ToString().Trim();
                            DrawRow(ssView2_Sheet1.RowCount - 1);
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                ssView2_Sheet1.RowCount++;
                DrawLine(ssView2_Sheet1.RowCount - 1);
                ssView2_Sheet1.RowCount++;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        string Read_WRTNO_Date(double argWRTNO)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT BDATE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_NST_PROGRESS ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + argWRTNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = dt.Rows[0]["BDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        void DrawLine(int argROW)
        {
            int i = 0;

            for (i = 2; i <= 44; i++)
            {
                ssView2_Sheet1.Cells[argROW, i - 1].BackColor = Color.Black;
            }

            //ssView2_Sheet1.SetRowHeight(argROW, 1#);
        }

        void DrawRow(int argROW)
        {
            int i = 0;

            ssView2_Sheet1.Cells[argROW, 1].ForeColor = Color.Black;
            ssView2_Sheet1.Cells[argROW, 1].BackColor = Color.Black;

            ssView2_Sheet1.Cells[argROW, 43].ForeColor = Color.Black;
            ssView2_Sheet1.Cells[argROW, 43].BackColor = Color.Black;

            ssView2_Sheet1.AddSpanCell(argROW, 2, 1, 6);
            ssView2_Sheet1.AddSpanCell(argROW, 8, 1, 4);
            ssView2_Sheet1.AddSpanCell(argROW, 12, 1, 4);
            ssView2_Sheet1.AddSpanCell(argROW, 16, 1, 3);
            ssView2_Sheet1.AddSpanCell(argROW, 19, 1, 3);
            ssView2_Sheet1.AddSpanCell(argROW, 22, 1, 3);
            ssView2_Sheet1.AddSpanCell(argROW, 25, 1, 3);
            ssView2_Sheet1.AddSpanCell(argROW, 28, 1, 3);
            ssView2_Sheet1.AddSpanCell(argROW, 31, 1, 4);
            ssView2_Sheet1.AddSpanCell(argROW, 35, 1, 3);
            ssView2_Sheet1.AddSpanCell(argROW, 38, 1, 5);

            for (i = 3; i <= 43; i++)
            {
                ssView2_Sheet1.Cells[argROW, i - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssView2_Sheet1.Cells[argROW, i - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                clsSpread.gSpreadLineBoder(ssView2, argROW, i - 1, argROW, i - 1, Color.Black, 1, true, true, true, true);

                //ssView2_Sheet1.SetRowHeight(argROW, 16);
            }

            ssView2_Sheet1.Cells[argROW, 31].BackColor = Color.FromArgb(255, 255, 223);
            ssView2_Sheet1.Cells[argROW, 31].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssView2_Sheet1.Cells[argROW, 31].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            ssView2_Sheet1.Cells[argROW, 35].BackColor = Color.FromArgb(255, 255, 223);
            ssView2_Sheet1.Cells[argROW, 35].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssView2_Sheet1.Cells[argROW, 35].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            ssView2_Sheet1.Cells[argROW, 38].BackColor = Color.FromArgb(255, 255, 223);
            ssView2_Sheet1.Cells[argROW, 38].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssView2_Sheet1.Cells[argROW, 38].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        }

        void Set_Lab_Patient()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT *";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE INDATE = TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + clsPat.PATi.Pano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;


                //ALB
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','CR32C') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //HB
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','HR01C') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //HCT
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','HR01D') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //TLC
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','HR02Q') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //BUN
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','CR41A') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //CR
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','CR42A') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //CA
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','CR44B') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //P
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','CR45B') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //MG
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','CR67A') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //NA
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','CR51A') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //K
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','CR52A') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //CL
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','CR53A') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //CHOL
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','CR40A') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //TG
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_LAB_PATIENT(INDATE, PANO, SUBCODE) VALUES ";
                SQL = SQL + ComNum.VBLF + " (TO_DATE('" + clsPat.PATi.InDate + "','YYYY-MM-DD'),'" + clsPat.PATi.Pano + "','CR39A') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void txtComplete_DoubleClick(object sender, EventArgs e)
        {
            frmCalendar frm = new frmCalendar();
            frm.ShowDialog();

            txtComplete.Text = clsPublic.GstrCalDate;
            clsPublic.GstrCalDate = "";
        }

        void Remove_Line()
        {
            int i = 0;
            int nRow = 0;

            FarPoint.Win.Spread.CellType.TextCellType tct = new FarPoint.Win.Spread.CellType.TextCellType();

            ssView3_Progress_Sheet1.RowCount++;

            nRow = ssView3_Progress_Sheet1.RowCount - 1;

            ssView3_Progress_Sheet1.SetRowHeight(nRow, 60);

            //ssView3_Progress_Sheet1.SetRowHeight(nRow, 40);

            for (i = 2; i <= 6; i++)
            {
                clsSpread.gSpreadLineBoder(ssView3_Progress, nRow, i, nRow, i, Color.Black, 1, true, true, true, true);
            }

            ssView3_Progress_Sheet1.Cells[nRow, 1].ForeColor = Color.Black;
            ssView3_Progress_Sheet1.Cells[nRow, 1].BackColor = Color.Black;

            ssView3_Progress_Sheet1.Cells[nRow, 7].ForeColor = Color.Black;
            ssView3_Progress_Sheet1.Cells[nRow, 7].BackColor = Color.Black;

            ssView3_Progress_Sheet1.Cells[nRow, 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssView3_Progress_Sheet1.Cells[nRow, 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            ssView3_Progress_Sheet1.Cells[nRow, 3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssView3_Progress_Sheet1.Cells[nRow, 3].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;


            ssView3_Progress_Sheet1.Cells[nRow, 6].CellType = tct;
            tct.Multiline = true;
            tct.MaxLength = 20000;
            ssView3_Progress_Sheet1.Cells[nRow, 6].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssView3_Progress_Sheet1.Cells[nRow, 6].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssView3_Progress_Sheet1.Cells[nRow, 6].ForeColor = Color.Black;
            ssView3_Progress_Sheet1.Cells[nRow, 6].BackColor = Color.FromArgb(255, 255, 223);
        }

        void Add_Line()
        {
            int i = 0;
            int nRow = 0;

            FarPoint.Win.Spread.CellType.TextCellType tct = new FarPoint.Win.Spread.CellType.TextCellType();

            ssView3_Progress_Sheet1.RowCount++;

            nRow = ssView3_Progress_Sheet1.RowCount - 1;

            ssView3_Progress_Sheet1.SetRowHeight(nRow, 60);

            //ssView3_Progress_Sheet1.SetRowHeight(nRow, 40);

            for (i = 2; i <= 6; i++)
            {
                clsSpread.gSpreadLineBoder(ssView3_Progress, nRow, i, nRow, i, Color.Black, 1, true, true, true, true);
            }

            ssView3_Progress_Sheet1.Cells[nRow, 1].ForeColor = Color.Black;
            ssView3_Progress_Sheet1.Cells[nRow, 1].BackColor = Color.Black;

            ssView3_Progress_Sheet1.Cells[nRow, 7].ForeColor = Color.Black;
            ssView3_Progress_Sheet1.Cells[nRow, 7].BackColor = Color.Black;

            ssView3_Progress_Sheet1.Cells[nRow, 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssView3_Progress_Sheet1.Cells[nRow, 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            ssView3_Progress_Sheet1.Cells[nRow, 3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssView3_Progress_Sheet1.Cells[nRow, 3].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;


            ssView3_Progress_Sheet1.Cells[nRow, 6].CellType = tct;
            tct.Multiline = true;
            tct.MaxLength = 20000;
            ssView3_Progress_Sheet1.Cells[nRow, 6].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssView3_Progress_Sheet1.Cells[nRow, 6].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssView3_Progress_Sheet1.Cells[nRow, 6].ForeColor = Color.Black;
            ssView3_Progress_Sheet1.Cells[nRow, 6].BackColor = Color.FromArgb(255, 255, 223);
        }

        private void ssView2_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Row == 34 && e.Column == 3)
            {
                if (Convert.ToBoolean(ssView2_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "No malnutrition present";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
            else if (e.Row == 34 && e.Column == 17)
            {
                if (Convert.ToBoolean(ssView2_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "Mild malnutrition";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
            else if (e.Row == 34 && e.Column == 31)
            {
                if (Convert.ToBoolean(ssView2_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "Moderate malnutrition";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
            else if (e.Row == 35 && e.Column == 3)
            {
                if (Convert.ToBoolean(ssView2_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "Severe Protein Energy malnutrition";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
            else if (e.Row == 35 && e.Column == 20)
            {
                if (Convert.ToBoolean(ssView2_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "Energy malnutrition";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
            else if (e.Row == 35 && e.Column == 31)
            {
                if (Convert.ToBoolean(ssView2_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "Protein malnutrition";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
        }

        private void ssView2_EN_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Row == 52 && e.Column == 3)
            {
                if (Convert.ToBoolean(ssView2_EN_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "No malnutrition present";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
            else if (e.Row == 52 && e.Column == 17)
            {
                if (Convert.ToBoolean(ssView2_EN_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "Mild malnutrition";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
            else if (e.Row == 52 && e.Column == 31)
            {
                if (Convert.ToBoolean(ssView2_EN_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "Moderate malnutrition";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
            else if (e.Row == 53 && e.Column == 3)
            {
                if (Convert.ToBoolean(ssView2_EN_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "Severe Protein Energy malnutrition";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
            else if (e.Row == 53 && e.Column == 20)
            {
                if (Convert.ToBoolean(ssView2_EN_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "Energy malnutrition";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
            else if (e.Row == 53 && e.Column == 31)
            {
                if (Convert.ToBoolean(ssView2_EN_Sheet1.Cells[e.Row, e.Column].Value) == true)
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "Protein malnutrition";
                }
                else
                {
                    ssView3_Sheet1.Cells[4, 7].Text = "";
                }
            }
        }

        private void btnProgress3Update_Click(object sender, EventArgs e)
        {
            if (clsPat.PATi.WRTNO == 0)
            {
                MessageBox.Show("신규 NST 결과입니다. 상단의 '저장' 버튼을 이용하시기 바랍니다.");
                return;
            }

            if (Save_ssView3_Progress(clsPat.PATi.WRTNO) == false)
            {
                ComFunc.MsgBox("추가 Recommended Page 저장에 실패하였습니다.");
                return;
            }
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            //int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strROWID = "";

            if (ComFunc.MsgBoxQ("삭제하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            strROWID = ssView3_Progress_Sheet1.Cells[Convert.ToInt32(ssView3_Progress_Sheet1.ActiveRowIndex), 5].Text.Trim();

            if (strROWID != "")
            {

                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "DIET_NST_M2_PROGRESS_HIS ";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM " + ComNum.DB_PMPA + "DIET_NST_M2_PROGRESS ";
                SQL = SQL + ComNum.VBLF + "   WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "DIET_NST_M2_PROGRESS";
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

                if (Display_ssView3_Progress(clsPat.PATi.WRTNO) == false)
                {
                    ComFunc.MsgBox("추가 Recommended Page를 읽어오는데 실패하였습니다.");
                }

            }
        } 
    }
}
