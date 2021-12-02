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
    /// File Name       : frmPTSuga.cs
    /// Description     : 재활치료수가 최초발병일 관리(조회)하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-05
    /// Update History  : try - catch문 수정
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\Frm재활치료발병일조회.frm(Frm재활치료발병일조회) => frmPTSuga.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\Frm재활치료발병일조회.frm(Frm재활치료발병일조회)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmPTSuga : Form
    {
        public frmPTSuga()
        {

            InitializeComponent();
        }

        void fmrPTSuga_Load(object sender, EventArgs e)
        {
            
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
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssPTSuga_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    a.PANO,b.SNAME,a.SUCODE,TO_CHAR(a.BDATE,'YYYY-MM-DD') BDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PT_BDATE A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "     AND a.Pano=b.Pano(+)  ";
                SQL = SQL + ComNum.VBLF + "     AND a.GUBUN IN ('01','02','03')  ";
                if (txtData.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND (a.Pano ='" + txtData.Text.Trim() + "' OR b.SName LIKE '%" + txtData.Text.Trim() + "%'  )  ";
                }
                SQL = SQL + ComNum.VBLF + "     ORDER BY a.BDate  ";
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

                ssPTSuga_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssPTSuga_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssPTSuga_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssPTSuga_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    ssPTSuga_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BDate"].ToString().Trim();
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

        void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btnView.Focus();
            }
        }

        private void frmPTSuga_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }
    }
}
