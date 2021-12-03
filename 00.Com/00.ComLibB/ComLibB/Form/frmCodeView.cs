using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary> 물품코드조회 </summary>
    public partial class frmCodeView : Form
    {
        string GstrJepCode1 = "";

        /// <summary> 물품코드조회 </summary>
        public frmCodeView()
        {
            InitializeComponent();
        }

        void frmCodeView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            optGubun1.Checked = true;
            optGubun2.Checked = false;
            optGubun3.Checked = false;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                string strJepCode = "";
                string strCsrName = "";

                int i = 0;
               
                int intRow = 0;

                //'물품코드 조회
                SQL = "";
                SQL = SQL + " SELECT JepCode,JepName ";
                SQL = SQL + ComNum.VBLF + " From ADMIN.ORD_JEP ";
                SQL = SQL + ComNum.VBLF + " Where GbCSR Is Not Null ";
                SQL = SQL + ComNum.VBLF + " AND DelDate IS NULL ";

                if (optGubun2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND BUN ='0702' ";
                }
                else if (optGubun3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND BUN ='0703' ";
                }

                SQL = SQL + ComNum.VBLF + " ORDER by JepCode ";
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

                ssView_Sheet1.Rows.Count = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                intRow = ssView_Sheet1.RowCount - 1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strJepCode = dt.Rows[i]["JepCode"].ToString().Trim();
                    strCsrName = dt.Rows[i]["JepName"].ToString().Trim();

                    intRow = intRow + 1;

                    if (intRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = intRow;
                    }

                    ssView_Sheet1.Cells[intRow, 0].Text = strJepCode;
                    ssView_Sheet1.Cells[intRow, 1].Text = strCsrName;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GstrJepCode1 = ssView_Sheet1.Cells[e.Row, 0].Text;
            this.Hide();
        }
    }
}
