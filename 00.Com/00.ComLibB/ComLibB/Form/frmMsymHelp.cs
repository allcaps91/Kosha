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
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmMsymHelp.cs
    /// Description     : 표준코드 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-05
    /// Update History  : try-catch문 수정, 권한 확인하는 부분 추가
    /// <history>       
    /// D:\타병원\PSMHH\mir\edi\miredi48.frm(FrmMsymHelp) => frmMsymHelp.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\mir\edi\miredi48.frm(FrmMsymHelp)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\mir\edi\miredi.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    /// </summary>
    public partial class frmMsymHelp : Form
    {
        public frmMsymHelp()
        {
            InitializeComponent();
        }

        void frmMsymHelp_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            btnSearch.Enabled = true;
            btnCancel.Enabled = false;
            SCREEN_CLEAR();
            optJong.Checked = true;
        }

        void SCREEN_CLEAR()
        {
            txtData.Text = "";
            btnSearch.Enabled = true;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;

            for(int i = 0; i < ssSCode_Sheet1.Rows.Count; i++)
            {
                for(int j = 0; j < ssSCode_Sheet1.Columns.Count; j++)
                {
                    ssSCode_Sheet1.Cells[i, j].Text = "";
                }
            }
         
            ssSCode.Enabled = false;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtData.Focus();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            if (txtData.Text == "")
            {
                ComFunc.MsgBox("찾으실 코드가 공란입니다.");
                txtData.Focus();
            }

            int i = 0;          

            string SQL = string.Empty;
            string SqlErr = "";

            DataTable dt = null;

            ssSCode.Enabled = true;
            btnCancel.Enabled = true;

            ssSCode_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    Code, Name";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "EDI_MSYM";

                if(optJong.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE Name LIKE '%" + txtData.Text.Trim().ToUpper() + "%' ";                  
                }

                else if(optCode.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE Code LIKE '%" + txtData.Text.Trim().ToUpper() + "%' ";                    
                }

                SQL = SQL + ComNum.VBLF + "AND ROWNUM < '200'";
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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssSCode_Sheet1.RowCount = dt.Rows.Count;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssSCode_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ssSCode_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                }

                btnSearch.Enabled = false;
                btnCancel.Enabled = true;

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

        void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btnSearch.Focus();
            }
        }
    }
}
