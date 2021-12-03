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
    public partial class frmOcsMessage : Form
    {
        ComFunc CF = new ComFunc();
        string FstrMsgROWID = "";

        string mstrJobSabun = "";
        string mstrIpAddress = "";
        string mstrJobPart = "";

        public frmOcsMessage()
        {
            InitializeComponent();
        }

        public frmOcsMessage(string GstrIpAddress, string GstrJobSabun, string GstrJobPart)
        {
            InitializeComponent();            
            this.mstrJobSabun = GstrJobSabun;
            this.mstrIpAddress = GstrIpAddress;
            this.mstrJobPart = GstrJobPart;
        }

        private void frmOcsMessage_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            CF.FormInfo_History(clsDB.DbCon, this.Name, this.Text, mstrIpAddress, mstrJobSabun, mstrJobPart);
            Init();
        }

        private void Init()
        {            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {                
                SQL = "SELECT ROWID,REMARK FROM ADMIN.OCS_DRUG_MESSAGE ";                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrMsgROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    txtInfo.Text = dt.Rows[0]["Remark"].ToString().Trim();
                    btnDelete.Enabled = true;
                }
                else
                {
                    FstrMsgROWID = "";
                    txtInfo.Text = "";
                    btnDelete.Enabled = false;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;


            if (VB.Len(txtInfo.Text) > 2000)
            {
                ComFunc.MsgBox("약국전달사항은 최대 2000Byte만 가능합니다.", "확인");
                txtInfo.Focus();
                return;
            }
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인


                txtInfo.Text = clsVbfunc.QuotationChange(txtInfo.Text);


                if (FstrMsgROWID != "")
                {
                    SQL = " INSERT INTO ADMIN.OCS_DRUG_MESSAGE_HIS";
                    SQL = SQL + ComNum.VBLF + " SELECT * FROM ADMIN.OCS_DRUG_MESSAGE";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrMsgROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = " UPDATE ADMIN.OCS_DRUG_MESSAGE SET INDATE = TRUNC(SYSDATE), ";
                    SQL = SQL + ComNum.VBLF + " REMARK = '" + VB.Trim(txtInfo.Text) + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrMsgROWID + "' ";                    
                }
                else
                {
                    SQL = " INSERT INTO ADMIN.OCS_DRUG_MESSAGE (INDATE,REMARK) VALUES (";
                    SQL = SQL + ComNum.VBLF + " TRUNC(SYSDATE),'" + VB.Trim(txtInfo.Text) + "') ";                    
                }
                
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
                //ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {            
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인

                SQL = " INSERT INTO ADMIN.OCS_DRUG_MESSAGE_HIS";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM ADMIN.OCS_DRUG_MESSAGE";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrMsgROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = " DELETE ADMIN.OCS_DRUG_MESSAGE ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrMsgROWID + "' ";

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
                //ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }            
        }

    }
}
