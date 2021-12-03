using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSCode
    /// File Name : frmSCode.cs
    /// Title or Description : 동일성분 조회
    /// Author : 박창욱
    /// Create Date : 2017-06-01
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    ///     06-15 박창욱 : 디자인 수정, GstrHelpCode 지역변수 -> 전역변수 변환
    /// </summary>
    /// <history>
    /// VB\Busuga09.frm(FrmSCode) -> frmSCode.cs로 변경
    /// </history>
    /// <seealso>
    /// VB\basic\busuga\Busuga09.frm(FrmSCode)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busuga\busuga.vbp
    /// </vbp>
    public partial class frmSCode : Form
    {
        string gstrHelpCode = "";

        public delegate void EventClose();
        public event EventClose rEventClose;

        public delegate void SendHelpCode(string strHelpCode);
        public event SendHelpCode rSendHelpCode;

        public frmSCode()
        {
            InitializeComponent();
        }

        public frmSCode(string strHelpCode)
        {
            InitializeComponent();

            gstrHelpCode = strHelpCode;
        }

        private void frmSCode_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            txtSCode.Text = "";
            readSugaGbsugbf("1");
        }

        private void readSugaGbsugbf(string str)
        {
            int i = 0;
            string strSCode = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            ssView_Sheet1.RowCount = 0;

            if (str == "1" && gstrHelpCode == "")
            {
                return;
            }
            else if (str == "2" && txtSCode.Text.Trim() == "")
            {
                return;
            }

            try
            {
                SQL = "";
                SQL = " SELECT  SUBSTR(C.SCODE, 1,4) || SUBSTR(C.SCODE, 7,1)  SCODE";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_SUT A,  ADMIN.BAS_SUN B,  ADMIN.EDI_SUGA C";
                SQL = SQL + ComNum.VBLF + "  WHERE A.BUN IN ('11','12','20')";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "   AND B.BCODE =  C.CODE";

                if (str == "1")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = '" + gstrHelpCode + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBSTR(C.SCODE, 1,4) || SUBSTR(C.SCODE, 7,1) = '" + txtSCode.Text + "'";
                }

                SQL = SQL + " ORDER BY 1";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                strSCode = dt.Rows[0]["SCODE"].ToString().Trim();
                
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT B.SUNEXT, B.SUNAMEK, C.SCODE, B.BCODE, A.DELDATE ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SUT A, " + ComNum.DB_PMPA + "BAS_SUN B, " + ComNum.DB_PMPA + "EDI_SUGA C";
                SQL = SQL + ComNum.VBLF + " WHERE A.BUN IN ('11','12','20')";
                SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "   AND B.BCODE =  C.CODE";
                SQL = SQL + ComNum.VBLF + "   AND  SUBSTR(C.SCODE, 1,4) || SUBSTR(C.SCODE, 7,1) = '" + strSCode + "'  ";
                SQL = SQL + ComNum.VBLF + " ORDER BY 1";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt == null)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.Rows.Count = 0;
                ssView_Sheet1.Rows.Count = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            readSugaGbsugbf("2");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            readSugaGbsugbf("1");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClose();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) { return; }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            if (e.Row == 0 || e.Column == 0)
            {
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            
            rSendHelpCode(ssView_Sheet1.Cells[e.Row, 0].Text);
            rEventClose();
        }
    }
}
