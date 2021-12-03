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
    public partial class frmOderSugaInjection : Form
    {
        ComFunc CF = new ComFunc();
        int mnJobSabun = 0;
        public frmOderSugaInjection()
        {
            InitializeComponent();
        }

        public frmOderSugaInjection(int GnJobSabun)
        {
            InitializeComponent();
            mnJobSabun = GnJobSabun;
        }

        void frmOderSugaInjection_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtDrcode.Text = "";
            txtDrname.Text = "";
            txtSabun.Text = "";

            txtSabun.Visible = false;

            txtDrname.Focus();
        }

        void btnPrint_Click(object sender, EventArgs e)
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

            ssView_Sheet1.RowCount = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            txtDrcode.Text = "";
            txtDrname.Text = "";

            // 진료과 의사명 등록일 등록자 의사사번 의사코드
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    CODE, NAME, ENTSABUN, TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "WHERE GUBUN = 'OCS_근막근통유발점주사_자격'";
                SQL = SQL + ComNum.VBLF + "ORDER BY NAME";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = READ_DOCTOR_DEPT(dt.Rows[i]["CODE"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 2].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[i]["NAME"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[i]["ENTSABUN"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
        }

        string READ_DOCTOR_DEPT(string ArgCode)
        {
            string rtnVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "WHERE DRCODE = '" + ArgCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if(dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                }
                return rtnVal;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }       

        void btnReg_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            SaveData();
        }

        void SaveData()
        {
            string strCODE = "";
            string strSabun = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            strCODE = txtDrcode.Text;
            strSabun = txtSabun.Text;

            if (strCODE == "" || strSabun == "")
            {
                ComFunc.MsgBox("의사명을 확인하시기 바랍니다. ");
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO  " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     (GUBUN, CODE, NAME, ENTDATE, ENTSABUN) ";
                SQL = SQL + ComNum.VBLF + "    VALUES ( 'OCS_근막근통유발점주사_자격','" + strCODE + "','" + strSabun + "', SYSDATE, " + mnJobSabun + ") ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if(SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("BAS_SUN UPDATE 도중 오류가 발생함.");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            DelData();
        }

        void DelData()
        {
            string strCODE = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strROWID = "";

            string strOK = "";

            int Result = 0;

            strCODE = txtDrcode.Text;
            strOK = "OK";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for(int i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strROWID = ssView_Sheet1.Cells[i, 7].Text;
                    if(strROWID != "" && ssView_Sheet1.Cells[i, 0].Text == "True")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "BAS_BCODE";
                        SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND GUBUN = 'OCS_근막근통유발점주사_자격' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if(SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if(Result != 0)
                        {
                            strOK = "NO";
                        }
                    }
                }

                if(strOK == "OK")
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                    Cursor.Current = Cursors.Default;
                    GetData();
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                }
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        void txtDrname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtDrname.Text != "")
                {
                    READ_DRCODE();
                }
            }
        }

        void READ_DRCODE()
        {
            txtDrcode.Text = "";
            txtSabun.Text = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    DRCODE, SABUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "WHERE DRNAME = '" + txtDrname.Text.ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " AND GBOUT = 'N'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    txtDrcode.Text = dt.Rows[0]["DRCODE"].ToString().Trim();
                    txtSabun.Text = dt.Rows[0]["SABUN"].ToString().Trim();
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
    }

}
