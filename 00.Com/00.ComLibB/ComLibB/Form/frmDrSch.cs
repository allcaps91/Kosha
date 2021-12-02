using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmDrSch.cs
    /// Description     : 월별 전공의 스케줄 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-08
    /// Update History  : GnJobSabun을 받아들이는 생성자 추가     
    /// <history>       
    /// D:\타병원\PSMHH\basic\bucode\bucode29.frm(FrmDrSch) => frmDrSch.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\bucode\bucode29.frm(FrmDrSch)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\bucode\bucode.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmDrSch : Form
    {        
        string mJobSabun = "";
        public frmDrSch()
        {
            InitializeComponent();
        }

        public frmDrSch(string strJobSabun)
        {
            InitializeComponent();
            mJobSabun = strJobSabun;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmDrSch_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Screen_Clear();

            SetComboYYMM();

            SetComboDept();

            GetData();

            Rettel_Read();
        }

        void Screen_Clear()
        {
            for(int i = 0; i < ssDR_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssDR_Sheet1.ColumnCount; j++)
                {
                    ssDR_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

        void SetComboYYMM()
        {
            int i = 0;
            int ArgYY = 0;
            int ArgMM = 0;

            ArgYY = Convert.ToInt16(VB.Left(DateTime.Now.ToString("yyyy-MM-dd"), 4));
            ArgMM = Convert.ToInt16(VB.Mid(DateTime.Now.ToString("yyyy-MM-dd"), 6, 2));
            
            cboYYMM.Items.Clear();

            for(i = 0; i < 31; i++)
            {
                cboYYMM.Items.Add(ArgYY + "년 " + ArgMM + "월분");
                ArgMM = ArgMM - 1;
                if(ArgMM == 0)
                {
                    ArgMM = 12;
                    ArgYY = ArgYY - 1;
                }
            }
            cboYYMM.SelectedIndex = 0;
            
        }

        void SetComboDept()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";

            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DEPTCODE, DEPTNAMEK ";
            SQL += ComNum.VBLF + "FROM " +ComNum.DB_PMPA + "BAS_CLINICDEPT";
            SQL += ComNum.VBLF + "WHERE DEPTCODE IN ('MG','MC','MP','ME','MN','MR','MI','OS','PD','GS','RM','NS')";
            SQL += ComNum.VBLF + "ORDER BY PRINTRANKING";
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
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return;
            }

            cboDept.Items.Clear();

            for(i = 0; i < dt.Rows.Count; i++)
            {
                cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString() + "" + "." + dt.Rows[i]["DeptNameK"].ToString().Trim() + "");
            }

            dt.Dispose();
            dt = null;

            cboDept.SelectedIndex = 0;
        }

        void Rettel_Read()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "select";
            SQL += ComNum.VBLF + "  code, name";
            SQL += ComNum.VBLF + "from " + ComNum.DB_PMPA + "bas_bcode";
            SQL += ComNum.VBLF + "  where gubun = '개인회신번호설정'";
            SQL += ComNum.VBLF + "      and deldate is null";
            SQL += ComNum.VBLF + "      and code = '" + mJobSabun + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }        

            if (dt.Rows.Count > 0)
            {
                txtRettel.Text = dt.Rows[0]["Name"].ToString().Trim() + "";
            }

            dt.Dispose();
            dt = null;

        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i = 0;
            //int nREAD = 0;

            ss3_Sheet1.RowCount = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + " SABUN, DRNAME, DEPTCODE, DRCODE ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                SQL += ComNum.VBLF + "WHERE GBOUT <> 'Y'";
                if (VB.Left(cboDept.SelectedItem.ToString(), 2) != "**")
                {
                    SQL += ComNum.VBLF + "  AND DEPTCODE = '" + VB.Left(cboDept.SelectedItem.ToString(), 2) + "'";
                }
                SQL += ComNum.VBLF + "  AND GRADE ='1'";
                SQL += ComNum.VBLF + "ORDER BY DRNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ss1_Sheet1.RowCount = dt.Rows.Count;

                for(i = 0; i < ss1_Sheet1.RowCount; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SABUN"].ToString().Trim() + "";
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DrName"].ToString().Trim() + "";
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim() + "";
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DrCode"].ToString().Trim() + "";
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }


        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            DelData();
        }

        void DelData()
        {
            string strYYMM = "";
            string strSabun = "";
            string strSetSabun = "";
            string strDrname = "";
            string strROWID = "";
            string SQL = "";

            string cboMM = ComFunc.vbFormat(VB.Mid(cboYYMM.SelectedItem.ToString(), 7, 2), "00");

            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;


            strYYMM = VB.Left(cboYYMM.SelectedItem.ToString(), 4) + cboMM;

            strSabun = ssDR_Sheet1.Cells[0, 0].Text;

            strSetSabun = ss2_Sheet1.Cells[ss2_Sheet1.ActiveRow.Index, 0].Text;
            strDrname = ss2_Sheet1.Cells[ss2_Sheet1.ActiveRow.Index, 1].Text;
            strROWID = ss2_Sheet1.Cells[ss2_Sheet1.ActiveRow.Index, 2].Text;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "DELETE ";
                SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR_SCH";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;

                READ_OCS_DOCTOR_SCH(strSabun);
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void READ_OCS_DOCTOR_SCH(string argSABUN)
        {
            int i = 0;
            //string strROWID = "";
            string strYYMM = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            ss2_Sheet1.RowCount = 0;

            string cboMM = ComFunc.vbFormat(VB.Mid(cboYYMM.SelectedItem.ToString(), 7, 2), "00");

            strYYMM = VB.Left(cboYYMM.SelectedItem.ToString(), 4) + cboMM;

            //스케줄내역
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  A.SETSABUN, A.ROWID , B.DRNAME";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR_SCH A, " + ComNum.DB_MED + "OCS_DOCTOR B";
                SQL += ComNum.VBLF + "WHERE A.SABUN = '" + argSABUN + "'";
                SQL += ComNum.VBLF + "  AND A.YYMM = '" + strYYMM + "'";
                SQL += ComNum.VBLF + "  AND A.SETSABUN = B.SABUN (+)";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss2_Sheet1.RowCount = dt.Rows.Count;

                    for(i = 0; i < ss2_Sheet1.RowCount; i++)
                    {
                        ss2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SETSABUN"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }

        }

        void READ_OCS_DOCTOR(string ArgDeptCode)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //전공의 조회
            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  B.SABUN BSABUN, A.KORNAME AKORNAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_MED + "OCS_DOCTOR B";
            SQL += ComNum.VBLF + "WHERE A.SABUN = B.SABUN";
            SQL += ComNum.VBLF + "  AND A.TOIDAY IS NULL";
            SQL += ComNum.VBLF + "  AND B.GBOUT <>'Y'";
            SQL += ComNum.VBLF + "  AND B.GRADE <>'1'";

            switch (ArgDeptCode)
            {
                //내과의 경우 전담간호사도 포함
                case "MP":
                case "MG":
                case "MC":
                case "ME":
                case "MN":
                case "MR":
                case "MI":
                    SQL += ComNum.VBLF + "  AND DEPTCODE IN ('" + ArgDeptCode + "', 'MD')";
                    SQL += ComNum.VBLF + "UNION ALL";
                    SQL += ComNum.VBLF + "SELECT";
                    SQL += ComNum.VBLF + "  SABUN, SNAME";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_CHARGE_NURSE";
                    SQL += ComNum.VBLF + "WHERE DELDATE IS NULL";
                    SQL += ComNum.VBLF + "  AND DEPTCODE IN ('MP', 'MG', 'MC', 'ME', 'MN', 'MR', 'MI')";
                    break;            

                //소아청소년과의 경우 전담간호사도 포함.
                case "PD":
                    SQL += ComNum.VBLF + "  AND DEPTCODE IN ('" + ArgDeptCode + "', 'PD')";
                    SQL += ComNum.VBLF + "UNION ALL";
                    SQL += ComNum.VBLF + "SELECT";
                    SQL += ComNum.VBLF + "  SABUN, SNAME";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_CHARGE_NURSE";
                    SQL += ComNum.VBLF + "WHERE DELDATE IS NULL";
                    SQL += ComNum.VBLF + "  AND DEPTCODE IN ('PD')";
                    break;

                //외과의 경우 전담간호사도 포함.
                case "GS":
                    SQL += ComNum.VBLF + "  AND DEPTCODE IN ('" + ArgDeptCode + "', 'GS' ,'NS')";
                    SQL += ComNum.VBLF + "UNION ALL";
                    SQL += ComNum.VBLF + "SELECT";
                    SQL += ComNum.VBLF + "  SABUN, SNAME";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_CHARGE_NURSE";
                    SQL += ComNum.VBLF + "WHERE DELDATE IS NULL";
                    SQL += ComNum.VBLF + "  AND DEPTCODE IN ('GS','NS')";
                    break;

                //재활의학과의 경우 전담간호사도 포함.
                case "RM":
                    SQL += ComNum.VBLF + "  AND DEPTCODE IN ('" + ArgDeptCode + "', 'RM')";
                    SQL += ComNum.VBLF + "UNION ALL";
                    SQL += ComNum.VBLF + "SELECT";
                    SQL += ComNum.VBLF + "  SABUN, SNAME";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_CHARGE_NURSE";
                    SQL += ComNum.VBLF + "WHERE DELDATE IS NULL";
                    SQL += ComNum.VBLF + "  AND DEPTCODE IN ('RM')";
                    break;

                default:
                    SQL += ComNum.VBLF + "  AND B.DEPTCODE  = '" + ArgDeptCode + "'";
                    break;
            }
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            ss3_Sheet1.RowCount = dt.Rows.Count;

            for(i =0; i < ss3_Sheet1.RowCount; i++)
            {
                ss3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BSABUN"].ToString().Trim();
                ss3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["AKORNAME"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            SaveData();
        }

        void SaveData()
        {
            string strYYMM = "";
            string strSabun = "";
            string strSetSabun = "";
            string strDrname = "";

            string SQL = "";
            DataTable dt = null;

            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            

            string cboMM = ComFunc.vbFormat(VB.Mid(cboYYMM.SelectedItem.ToString(), 7, 2), "00");
            strYYMM = VB.Left(cboYYMM.SelectedItem.ToString(), 4) + cboMM;

            strSabun = ssDR_Sheet1.Cells[0, 0].Text;

            strSetSabun = ss2_Sheet1.Cells[ss3_Sheet1.ActiveRow.Index, 0].Text;
            strDrname = ss2_Sheet1.Cells[ss3_Sheet1.ActiveRow.Index, 1].Text;

            //기존에 등록된 것이 있는지 db 읽기

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  ROWID ";
                SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR_SCH";
                SQL += ComNum.VBLF + "WHERE YYMM = '" + strYYMM + "' ";
                SQL += ComNum.VBLF + "  AND SABUN  = '" + strSabun + "' ";
                SQL += ComNum.VBLF + "  AND SETSABUN = '" + strSetSabun + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    int ss2Row = dt.Rows.Count + 1;
                    ss2_Sheet1.Cells[ss2Row, 0].Text = strSetSabun;
                    ss2_Sheet1.Cells[ss2Row, 1].Text = strDrname;


                    SQL += ComNum.VBLF + "INSERT INTO ";
                    SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR_SCH";
                    SQL += ComNum.VBLF + "VALUES( ";
                    SQL += ComNum.VBLF + "'" + strYYMM + "',";
                    SQL += ComNum.VBLF + "'" + strSabun + "',";
                    SQL += ComNum.VBLF + "'" + strSetSabun + "',";
                    SQL += ComNum.VBLF + "SYSDATE ";
                    SQL += ComNum.VBLF + " ) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }

                    ComFunc.MsgBox("등록하였습니다.");
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ComFunc.MsgBox("이미 의사스케쥴 등록 되어있습니다.");
                }

                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                READ_OCS_DOCTOR_SCH(strSabun);
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }       

        void btnReg_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            RegData();
        }

        void RegData()
        {
            string strGubun = "";
            string strSabun = "";
            string strRetTel = "";
            string strJDate = "";
            string strEntSabun = "";
            string strEntDate = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            

            DataTable dt = null;

            strGubun = "개인회신번호설정";
            strSabun = mJobSabun;
            strSabun = "20012";
            strRetTel = txtRettel.Text.Trim();
            strJDate = DateTime.Now.ToString("yyyy-MM-dd");
            strEntSabun = mJobSabun;
            strEntDate = strJDate + " " + DateTime.Now.ToString("HH:mm:ss");

            SQL = "";
            SQL += ComNum.VBLF + "select ";
            SQL += ComNum.VBLF + "  gubun, code";
            SQL += ComNum.VBLF + "from " + ComNum.DB_PMPA + "bas_bcode";
            SQL += ComNum.VBLF + "where gubun = '" + strGubun + "'";
            SQL += ComNum.VBLF + "  and code = '" + strSabun + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }


            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                if (dt.Rows.Count == 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO ";
                    SQL += ComNum.VBLF + ComNum.DB_PMPA + "bas_bcode ";
                    SQL += ComNum.VBLF + "(gubun, code, name, jdate, entsabun, entdate)";
                    SQL += ComNum.VBLF + "VALUES( ";
                    SQL += ComNum.VBLF + "'" + strGubun + "', ";
                    SQL += ComNum.VBLF + "'" + strSabun + "', ";
                    SQL += ComNum.VBLF + "'" + strRetTel + "', ";
                    SQL += ComNum.VBLF + "to_date('" + strJDate + "', 'yyyy-mm-dd'),";
                    SQL += ComNum.VBLF + "'" + strEntSabun + "',  ";
                    SQL += ComNum.VBLF + "to_date('" + strEntDate + "', 'yyyy-mm-dd hh24:mi')";
                    SQL += ComNum.VBLF + ") ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }
                }
                else if (dt.Rows.Count == 1)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "update  ";
                    SQL += ComNum.VBLF + ComNum.DB_PMPA + "bas_bcode set";
                    SQL += ComNum.VBLF + "name = '" + strRetTel + "', ";
                    SQL += ComNum.VBLF + "entsabun = '" + strEntSabun + "', ";
                    SQL += ComNum.VBLF + "entdate = to_date('" + strEntDate + "', 'yyyy-mm-dd hh24:mi')";
                    SQL += ComNum.VBLF + "where gubun = '" + strGubun + "' ";
                    SQL += ComNum.VBLF + "  and code = '" + strSabun + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }
                }

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
            }
        }


        void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //int i = 0;

            string strSabun = "";
            string strDrname = "";
            string strDeptCode = "";
            string strDrCode = "";
            string strYYMM = "";

            string cboMM = ComFunc.vbFormat(VB.Mid(cboYYMM.SelectedItem.ToString(), 7, 2), "00");
            strYYMM = VB.Left(cboYYMM.SelectedItem.ToString(), 4) + cboMM;

            strSabun = ss1_Sheet1.Cells[e.Row, 0].Text;
            strDrname = ss1_Sheet1.Cells[e.Row, 1].Text;
            strDeptCode = ss1_Sheet1.Cells[e.Row, 2].Text;
            strDrCode = ss1_Sheet1.Cells[e.Row, 3].Text;

            ssDR_Sheet1.Cells[0, 0].Text = strSabun;
            ssDR_Sheet1.Cells[0, 1].Text = strDrname;
            ssDR_Sheet1.Cells[0, 2].Text = strDeptCode;
            ssDR_Sheet1.Cells[0, 3].Text = strDrCode;

            READ_OCS_DOCTOR_SCH(strSabun);
            READ_OCS_DOCTOR(strDeptCode);
        }

        void ss2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strSabun = "";
            string strSetSabun = "";
            string strDrname = "";
            string strYYMM = "";
            string strROWID = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            


            string cboMM = ComFunc.vbFormat(VB.Mid(cboYYMM.SelectedItem.ToString(), 7, 2), "00");
            strYYMM = VB.Left(cboYYMM.SelectedItem.ToString(), 4) + cboMM;

            strSabun = ssDR_Sheet1.Cells[0, 0].Text;

            strSetSabun = ss2_Sheet1.Cells[e.Row, 0].Text;
            strDrname = ss2_Sheet1.Cells[e.Row, 1].Text;
            strROWID = ss2_Sheet1.Cells[e.Row, 2].Text;

            //ss2.DeleteRows ss2.DataRowCnt + 1, 1           

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "DELETE ";
                SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR_SCH";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            READ_OCS_DOCTOR_SCH(strSabun);
        }

        void ss3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strSabun = "";
            string strSetSabun = "";
            string strDrname = "";
            string strYYMM = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            

            DataTable dt = null;

            string cboMM = ComFunc.vbFormat(VB.Mid(cboYYMM.SelectedItem.ToString(), 7, 2), "00");
            strYYMM = VB.Left(cboYYMM.SelectedItem.ToString(), 4) + cboMM;

            strSabun = ssDR_Sheet1.Cells[0, 0].Text;

            strSetSabun = ss3_Sheet1.Cells[e.Row, 0].Text;
            strDrname = ss3_Sheet1.Cells[e.Row, 1].Text;

            // 기존에 등록된 것이 있는지 db 읽기
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR_SCH";
            SQL += ComNum.VBLF + "WHERE YYMM = '" + strYYMM + "' ";
            SQL += ComNum.VBLF + "  AND SABUN  = '" + strSabun + "' ";
            SQL += ComNum.VBLF + "  AND SETSABUN = '" + strSetSabun + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count  == 0)
            {
                ss2_Sheet1.RowCount += 1;
                ss2_Sheet1.Cells[dt.Rows.Count, 0].Text = strSetSabun;
                ss2_Sheet1.Cells[dt.Rows.Count, 1].Text = strDrname;

                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO ";
                    SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR_SCH";
                    SQL += ComNum.VBLF + "( YYMM, SABUN, SETSABUN , ENTDATE)";
                    SQL += ComNum.VBLF + "VALUES(";
                    SQL += ComNum.VBLF + "'" + strYYMM + "', ";
                    SQL += ComNum.VBLF + "'" + strSabun + "',";
                    SQL += ComNum.VBLF + "'" + strSetSabun + "', ";
                    SQL += ComNum.VBLF + "SYSDATE";
                    SQL += ComNum.VBLF + ")";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("등록하였습니다.");
                    Cursor.Current = Cursors.Default;

                }

                catch(Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                ComFunc.MsgBox("이미 의사스케줄 등록 되어있습니다.");
            }

            dt.Dispose();
            dt = null;

            READ_OCS_DOCTOR_SCH(strSabun);
        }
    }
}
