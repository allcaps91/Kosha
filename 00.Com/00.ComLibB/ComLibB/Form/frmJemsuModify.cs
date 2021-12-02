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
    public partial class frmJemsuModify : Form
    {
        public frmJemsuModify()
        {
            InitializeComponent();
        }
        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void frmJemsuModify_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssJemsuModify_Sheet1.Columns.Get(0).Locked = true;
            ssJemsuModify_Sheet1.Columns.Get(4).Locked = true;

            ssJemsuModify_Sheet1.Columns[5].Visible = false;
            txtCode.Text = "";
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            int i = 0;
            int strLength = 0;
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = "";

            ssJemsuModify_Sheet1.RowCount = 30;
            ssClear();

            if(txtCode.Text == "" && chkZero.Checked == false)
            {
                ComFunc.MsgBox("찾으실 코드가 공란입니다.");
                txtCode.Focus();
            }
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    BCode,TO_CHAR(JDate1,'YYYY-MM-DD') JDate1,JemSu1,Price1,Remark,ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUGAJEMSU";
                if(chkZero.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE (Jemsu1 = 0 Or Price1 = 0)";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE BCode LIKE '" + txtCode.Text.Trim().ToUpper() + "%' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY BCode";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("자료가 등록되지 않았습니다.");
                    return;
                }

                ssJemsuModify_Sheet1.RowCount = dt.Rows.Count;
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssJemsuModify_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BCode"].ToString().Trim();
                    ssJemsuModify_Sheet1.Cells[i, 1].Text = dt.Rows[i]["JDate1"].ToString().Trim();
                    ssJemsuModify_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Jemsu1"].ToString().Trim();
                    ssJemsuModify_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Price1"].ToString().Trim();
                    ssJemsuModify_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    ssJemsuModify_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    strLength = VB.Len(dt.Rows[i]["Remark"].ToString().Trim());
                    if(strLength <= 60)
                    {
                        ssJemsuModify_Sheet1.Cells[i, 4].Row.Height = 33;
                    }
                    else if(strLength <= 120)
                    {
                        ssJemsuModify_Sheet1.Cells[i, 4].Row.Height = 66;
                    }
                    else if(strLength <= 180)
                    {
                        ssJemsuModify_Sheet1.Cells[i, 4].Row.Height = 77;
                    }
                    else
                    {
                        ssJemsuModify_Sheet1.Cells[i, 4].Row.Height = 88;
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

        void ssClear()
        {
            for(int i = 0; i < ssJemsuModify_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssJemsuModify_Sheet1.ColumnCount; j++)
                {
                    ssJemsuModify_Sheet1.Cells[i, j].Text = "";
                }
            }
        }
        void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btnSearch.Focus();
            }
        }

        void ssJemsuModify_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            //ssJemsuModify_Sheet1.Cells[e.Row, 6].Text = "Y";

            string[] strData = new string[3];
            string strROWID = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            strData[0] = ssJemsuModify_Sheet1.Cells[e.Row, 1].Text;
            strData[1] = ssJemsuModify_Sheet1.Cells[e.Row, 2].Text;
            strData[2] = ssJemsuModify_Sheet1.Cells[e.Row, 3].Text;
            strROWID = ssJemsuModify_Sheet1.Cells[e.Row, 5].Text;

            //자료를 UPDATE
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_SUGAJEMSU SET";
                SQL = SQL + ComNum.VBLF + " JDate1=TO_DATE('" + strData[0] + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + " Jemsu1= '" + strData[1] + "', ";
                SQL = SQL + ComNum.VBLF + " Price1= '" + strData[2] + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
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
                //clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }
    }
}
