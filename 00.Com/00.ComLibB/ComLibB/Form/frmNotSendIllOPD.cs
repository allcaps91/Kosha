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
    /// File Name       : frmNotSendIllOPD.cs
    /// Description     : 외래에서 주진단으로 사용 불가능한 상병코드 설정하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06
    /// Update History  : try - catch문 수정, GnJobSabun을 읽어오는 생성자, JobPart, IpAdd 읽어오는 생성자 추가
    /// <history>       
    /// FormInfo_History(Me, Name, Me.Caption) 구현 필요
    /// D:\타병원\PSMHH\basic\busuga\FrmNotSendIllOPD.frm(Frm주진단사용불가상병설정) => frmNotSendIllOPD.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\FrmNotSendIllOPD.frm(Frm주진단사용불가상병설정)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>    
    public partial class frmNotSendIllOPD : Form
    {
        ComFunc CF = new ComFunc();
        string mstrJobSabun = "";
        string mstrIpAddress = "";
        string mstrJobPart = "";

        public frmNotSendIllOPD()
        {
            InitializeComponent();
        }

        public frmNotSendIllOPD(string GnJobSabun)
        {
            InitializeComponent();
            mstrJobSabun = GnJobSabun;
        }

        public frmNotSendIllOPD(string GstrIpAddress, string GstrJobSabun, string GstrJobPart)
        {
            mstrIpAddress = GstrIpAddress;
            mstrJobSabun = GstrJobSabun;
            mstrJobPart = GstrJobPart;
        }

        void frmNotSendIllOPD_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            CF.FormInfo_History(clsDB.DbCon, this.Name, this.Text, mstrIpAddress, mstrJobSabun, mstrJobPart);

            int i = 0; 

            for (i = 5; i < 8; i++)
            {
                ssView_Sheet1.Columns[i].Visible = false;

            }

            for (i = 0; i < ssView_Sheet1.ColumnCount; i++)
            {
                ssView_Sheet1.Columns.Get(i).Locked = true;

                if (i == 1 || i == 0)
                {
                    ssView_Sheet1.Columns.Get(i).Locked = false;
                }

            }

            ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            cboTitle.SelectedIndex = 0;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    CODE, NAME, TO_CHAR(ENTDATE, 'YYYY-MM-DD HH24:MI') ENTDATE, ENTSABUN,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(DELDATE, 'YYYY-MM-DD') DELDATE, ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = '" + cboTitle.SelectedItem.ToString().Trim() + "'";
                SQL = SQL + ComNum.VBLF + "AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "ORDER BY CODE, ENTDATE ";

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

                ssView_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount += 10;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = "";
                    }
                }
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            DelData();
        }

        void DelData()
        {
            int i = 0;

            string strROWID = "";
            string strOK = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strROWID = ssView_Sheet1.Cells[i, 6].Text;

                    if (ssView_Sheet1.Cells[i, 0].Text == "True")
                    {

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "BAS_BCODE";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                        SQL = SQL + ComNum.VBLF + "AND GUBUN = '" + cboTitle.Text.ToString().Trim() + "' ";
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

                if (strOK != "OK")
                {
                    ComFunc.MsgBox("BAS_BCODE DELETE 도중 오류가 발생함.");
                    clsDB.setRollbackTran(clsDB.DbCon);
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                    Cursor.Current = Cursors.Default;
                }

                GetData();
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            SaveData();
        }

        void SaveData()
        {
            int i = 0;

            string strCODE = "";
            string strName = "";

            string strROWID = "";
            string strChange = "";

            string strOK = "";

            strOK = "OK";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strCODE = ssView_Sheet1.Cells[i, 1].Text;
                    strName = ssView_Sheet1.Cells[i, 2].Text;
                    strROWID = ssView_Sheet1.Cells[i, 6].Text;
                    strChange = ssView_Sheet1.Cells[i, 7].Text;

                    if (strChange != "" && strCODE != "")
                    {
                        if (strROWID == "")
                        {

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_BCODE(";
                            SQL = SQL + ComNum.VBLF + " GUBUN, CODE, NAME, ENTSABUN,";
                            SQL = SQL + ComNum.VBLF + " ENTDATE) VALUES (";
                            SQL = SQL + ComNum.VBLF + " '" + cboTitle.SelectedItem.ToString().Trim() + "','" + strCODE + "','" + strName + "'," + mstrJobSabun + ", ";
                            SQL = SQL + ComNum.VBLF + " SYSDATE) ";

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

                if (strOK != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);

                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                    Cursor.Current = Cursors.Default;
                }

                GetData();
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strCODE = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                if (e.Column == 1)
                {
                    strCODE = ssView_Sheet1.Cells[e.Row, 1].Text.ToUpper();
                    ssView_Sheet1.Cells[e.Row, 2].Text = "";

                    ssView_Sheet1.Cells[e.Row, 7].Text = "Y";


                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     ILLNAMEK";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ILLS";
                    SQL = SQL + ComNum.VBLF + "WHERE ILLCODE = '" + strCODE + "' ";
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

                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[e.Row, 1].Text = dt.Rows[0]["ILLNAMEK"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
            
    }
}
