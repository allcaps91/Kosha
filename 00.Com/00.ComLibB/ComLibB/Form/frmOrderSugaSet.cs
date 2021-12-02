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
    /// File Name       : frmOrderSugaSet.cs
    /// Description     : 수가 및 오더제한코드 설정하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06
    /// Update History  : try-catch문 수정, GnJobSabun과 GstrHelpCode를 받아들이는 생성자 추가
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\Frm오더수가Set.frm(Frm오더수가Set) => frmOrderSugaSet.cs 으로 변경함
    /// ssView_Change 이벤트에서 사용 안하는 부분 주석처리
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\Frm오더수가Set.frm(Frm오더수가Set)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmOrderSugaSet : Form
    {
        string GstrSysDate = DateTime.Now.ToString("yyyy-MM-dd");
        //string GnJobSabun = "41827";
        //string GstrHelpCode = "S12";

        string mstrHelpCode = "";
        string mstrJobSabun = "";
        public frmOrderSugaSet()
        {
            InitializeComponent();
        }

        public frmOrderSugaSet(string strHelpCode, string strJobSabun)
        {
            InitializeComponent();
            mstrHelpCode = strHelpCode;
            mstrJobSabun = strJobSabun;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            btnView.Focus();
        }

        void frmOrderSugaSet_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.RowCount = 0;

            ssView_Sheet1.Columns[4].Visible = false;
            ssView_Sheet1.Columns[5].Visible = false;
            ssView_Sheet1.Columns[6].Visible = false;

            switch (mstrHelpCode)
            {
                case "1":
                    txtInfo.Text = "암표지자검사 수가";
                    break;
                case "S1":
                    txtInfo.Text = "소화성궤양용제_공격인자";
                    break;
                case "S12":
                    txtInfo.Text = "소화성궤양용제_공격인자2";
                    break;
                case "S2":
                    txtInfo.Text = "소화성궤양용제_방어인자";
                    break;
                case "S3":
                    txtInfo.Text = "비스테로이드성_소염진통제";
                    break;
                case "S4":
                    txtInfo.Text = "진정(ASA)수가";
                    break;
                case "S5":
                    txtInfo.Text = "ER 급여/비급여 관리 대상 수가";
                    break;
                default:
                    break;
            }
            ComboGubun_SET(mstrHelpCode);
            SCREEN_CLEAR();

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }
        
        void ComboGubun_SET(string ArgGubun)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            cboJong.Items.Clear();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    Code, Name";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun ='자료사전목록' ";
                switch (ArgGubun)
                {
                    case "1":
                        SQL = SQL + ComNum.VBLF + "AND Name IN ('OCS_암표지자수가') ";
                        break;
                    case "S1":
                        SQL = SQL + ComNum.VBLF + "AND Name IN ('소화성궤양용제_공격인자') ";
                        break;
                    case "S12":
                        SQL = SQL + ComNum.VBLF + "AND Name IN ('소화성궤양용제_공격인자2') ";
                        break;
                    case "S2":
                        SQL = SQL + ComNum.VBLF + "AND Name IN ('소화성궤양용제_방어인자')";
                        break;
                    case "S3":
                        SQL = SQL + ComNum.VBLF + "AND Name IN ('비스테로이드성_소염진통제')";
                        break;
                    case "S4":
                        SQL = SQL + ComNum.VBLF + "AND Name IN ('진정(ASA)수가') ";
                        break;
                    case "S5":
                        SQL = SQL + ComNum.VBLF + "AND Name IN ('급여선택_메시지창_적용코드') ";
                        break;

                    default:
                        ComFunc.MsgBox("쿼리 에러");
                        break;
                }
                SQL = SQL + ComNum.VBLF + "AND (DelDate IS NULL OR DelDate>TRUNC(SYSDATE)) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Code ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboJong.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                    }
                    cboJong.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;

                
                cboJong.Enabled = false;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        void SCREEN_CLEAR()
        {
            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            ssView_Sheet1.RowCount = 50;
            ss_Clear();
            ssView.Enabled = false;
        }

        void ss_Clear()
        {
            for(int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssView_Sheet1.ColumnCount; j++)
                {
                    ssView_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i = 0;
            string strGUBUN = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssView.Enabled = true;

            if(cboJong.Text == "")
            {
                ComFunc.MsgBox("구분이 공란입니다.", "오류");
                return;
            }

            strGUBUN = cboJong.SelectedItem.ToString();

            if(strGUBUN == "자료사전목록")
            {
                ssView_Sheet1.Columns[3].Visible = true;
                ssView_Sheet1.Columns[4].Visible = true;
            }

            //현재 자료를 READ
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    Code,Name,TO_CHAR(JDate,'YYYY-MM-DD') JDate, ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun ='" + strGUBUN + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Code ";
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
                    btnView.Enabled = false;
                    btnSave.Enabled = true;
                    btnExit.Enabled = true;
                    btnCancel.Enabled = true;
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count + 20;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = "";
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Name"].ToString().Trim();
                    if (strGUBUN != "자료사전목록")
                    {
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = "";
                }

                dt.Dispose();
                dt = null;
                
                btnView.Enabled = false;
                btnSave.Enabled = true;
                btnExit.Enabled = true;
                btnCancel.Enabled = true;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            SaveData();
            GetData();
        }

        void SaveData()
        {
            int i = 0;

            string SQL = "";
            string strDel = "";
            string strGUBUN = "";
            string strCODE = "";
            string strName = "";
            string strJDate = "";
            string strDeldate = "";
            string strROWID = "";
            string strChange = "";

            //확인해볼것
            strGUBUN = cboJong.SelectedItem.ToString().Trim();

            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strDel = ssView_Sheet1.Cells[i, 0].Text;
                    strCODE = ssView_Sheet1.Cells[i, 1].Text.ToUpper();
                    strName = ssView_Sheet1.Cells[i, 2].Text;
                    strJDate = ssView_Sheet1.Cells[i, 3].Text;
                    strDeldate = ssView_Sheet1.Cells[i, 4].Text;
                    strROWID = ssView_Sheet1.Cells[i, 5].Text;
                    strChange = ssView_Sheet1.Cells[i, 6].Text;

                    if (strDel == "True")
                    {
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " DELETE";
                            SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "BAS_BCODE";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
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

                    else if (strChange == "Y")
                    {
                        if (strROWID == "")
                        {
                            // 등록
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO";
                            SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "BAS_BCODE";
                            SQL = SQL + ComNum.VBLF + "(Gubun, Code, Name, JDate, DelDate, EntSabun, EntDate)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "(    ";
                            SQL = SQL + ComNum.VBLF + "'" + strGUBUN + "','" + strCODE + "','" + strName + "', ";
                            SQL = SQL + ComNum.VBLF + "     TO_DATE('" + GstrSysDate + "','YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + "     TO_DATE('" + strDeldate + "','YYYY-MM-DD'), '" + mstrJobSabun + "',  SYSDATE) ";
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

                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE ";

                            SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "BAS_BCODE SET";
                            SQL = SQL + ComNum.VBLF + "     Code = '" + strCODE + "' ,";
                            SQL = SQL + ComNum.VBLF + "     Name = '" + strName + "' ,";
                            SQL = SQL + ComNum.VBLF + "     JDate = TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                            SQL = SQL + ComNum.VBLF + "     DelDate = TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                            SQL = SQL + ComNum.VBLF + "     EntSabun = " + mstrJobSabun + ",";
                            SQL = SQL + ComNum.VBLF + "     EntDate = SYSDATE";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
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

                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        void cboJong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btnView.Focus();
            }
        }

        void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strData = "";
            string strChk = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            switch (txtInfo.Text.Trim())
            {
                case "소화성궤양용제_공격인자":
                    strChk = "OK";
                    break;
                case "소화성궤양용제_공격인자2":
                    strChk = "OK";
                    break;
                case "소화성궤양용제_방어인자":
                    strChk = "OK";
                    break;
                case "비스테로이드성_소염진통제":
                    strChk = "OK";
                    break;
                case "ER 급여/비급여 관리 대상 수가":
                    strChk = "OK";
                    break;
                default:
                    break;
            }

            ssView_Sheet1.Cells[e.Row, 6].Text = "Y";
            
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            if (e.Column == 1 && strChk == "OK")
            {
                strData = ssView_Sheet1.Cells[e.Row, e.Column].Text.ToUpper();

                try
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "   SuNameK";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN";
                    SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + strData + "'";

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

                        ssView_Sheet1.Cells[e.Row, 1].Text = "";
                        ssView_Sheet1.Cells[e.Row, 2].Text = "";
                        return;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[e.Row, 2].Text = dt.Rows[0]["SuNameK"].ToString().Trim();
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
}

