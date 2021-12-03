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
    /// File Name       : frmOrderSugaSetInject.cs
    /// Description     :근막근통유발점주사 처방 권한 의사 등록하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06
    /// Update History  : try-catch문 수정, 작업자사번을 받아오는 생성자 추가
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\Frm오더수가Set_근막근통주사자격.frm(Frm오더수가Set_근막근통주사자격) => frmOrderSugaSetInject.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\Frm오더수가Set_근막근통주사자격.frm(Frm오더수가Set_근막근통주사자격) 
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmOrderSugaSetInject : Form
    {
        string mJobSabun = "";
        ComFunc CF = new ComFunc();
        public frmOrderSugaSetInject()
        {
            InitializeComponent();
        }

        public frmOrderSugaSetInject(string GnJobSabun)
        {
            InitializeComponent();
            mJobSabun = GnJobSabun;
        }

        void frmOrderSugaSetInject_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtDrName.Text = "";
            txtDrCode.Text = "";
            txtSabun.Text = "";

            txtSabun.Visible = false;

            for (int i = 5; i < 8; i++)
            {
                ssInject_Sheet1.Columns[i].Visible = false;
            }

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i = 0;

            ssInject_Sheet1.RowCount = 0;

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = "";

            txtDrName.Text = "";
            txtDrCode.Text = "";
            txtSabun.Text = "";

            //진료과 의사명 등록일 등록자 의사사번 의사코드
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "   CODE, NAME, ENTSABUN, TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, ROWID  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun = 'OCS_근막근통유발점주사_자격' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY NAME ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssInject_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < ssInject_Sheet1.RowCount; i++)
                    {
                        ssInject_Sheet1.Cells[i, 1].Text = READ_DOCTOR_DEPT(dt.Rows[i]["CODE"].ToString().Trim());
                        ssInject_Sheet1.Cells[i, 2].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[i]["NAME"].ToString().Trim());
                        ssInject_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                        ssInject_Sheet1.Cells[i, 4].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim());
                        ssInject_Sheet1.Cells[i, 5].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssInject_Sheet1.Cells[i, 6].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssInject_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
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

        //진료과를 읽어 온다.
        string READ_DOCTOR_DEPT(string ArgCode)
        {
            string rtnVal = "";
            string SQL = string.Empty;
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "   DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "WHERE DRCODE ='" + ArgCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                }

                return rtnVal;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return "";
            }

           
        }      

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            SaveData();
        }

        void SaveData()
        {
            string strCODE = "";
            string strSabun = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            strCODE = txtDrCode.Text.Trim();
            strSabun = txtSabun.Text.Trim();

            if (strCODE == "" || strSabun == "")
            {
                ComFunc.MsgBox("의사명을 확인하시기 바랍니다.");
            }                     

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO";
                SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " (GUBUN, CODE, NAME, ENTDATE, ENTSABUN) ";
                SQL = SQL + ComNum.VBLF + "VALUES ";
                SQL = SQL + ComNum.VBLF + "(    ";
                SQL = SQL + ComNum.VBLF + "'OCS_근막근통유발점주사_자격',  ";
                SQL = SQL + ComNum.VBLF + "'" + strCODE + "',   ";
                SQL = SQL + ComNum.VBLF + "'" + strSabun + "',  ";
                SQL = SQL + ComNum.VBLF + "SYSDATE ,    ";
                SQL = SQL + ComNum.VBLF + "'" + mJobSabun + "'";
                SQL = SQL + ComNum.VBLF + ")";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("등록하였습니다.");
                Cursor.Current = Cursors.Default;

                GetData();
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

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            DeleteData();
        }

        void DeleteData()
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strROWID = "";

            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            for (int i = 0; i < ssInject_Sheet1.RowCount; i++)
            {
                strROWID = ssInject_Sheet1.Cells[i, 7].Text;

                if (strROWID != "" && ssInject_Sheet1.Cells[i, 0].Text == "True")
                {
                    try
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " DELETE";
                        SQL = SQL + ComNum.VBLF + " " + ComNum.DB_PMPA + "BAS_BCODE";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'  ";
                        SQL = SQL + ComNum.VBLF + "AND GUBUN = 'OCS_근막근통유발점주사_자격' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        ComFunc.MsgBox("삭제하였습니다.");
                        Cursor.Current = Cursors.Default;

                        GetData();
                    }

                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        void txtDrName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtDrName.Text != "")
            {
                READ_DRCODE();
            }

        }

        void txtDrName_Leave(object sender, EventArgs e)
        {
            if (txtDrName.Text != "")
            {
                READ_DRCODE();
            }
        }

        void READ_DRCODE()
        {
            txtDrCode.Text = "";
            txtSabun.Text = "";

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "   DRCODE, SABUN ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "WHERE DRNAME ='" + txtDrName.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "AND GBOUT = 'N' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt == null)
                {
                    ComFunc.MsgBox("조회 중 문제가 발생하였습니다. ");
                }

                if (dt.Rows.Count > 0)
                {
                    txtDrCode.Text = dt.Rows[0]["DRCODE"].ToString().Trim();
                    txtSabun.Text = dt.Rows[0]["SABUN"].ToString().Trim();
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
