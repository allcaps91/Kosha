using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Drawing;
using FarPoint.Win.Spread;

namespace ComLibB
{  
     /// Class Name      : ComLibB.dll
     /// File Name       : FrmSugaSerch.cs
     /// Description     : 수가코드 찾기
     /// Author          : 김효성
     /// Create Date     : 2017-06-20
     /// Update History  : 
     /// </summary>
     /// <history>  
     /// VB\basic\busuga\BuSuga11.frm => FrmSugaSerch.cs 으로 변경함
     /// </history>
     /// <seealso> 
     /// VB\basic\busuga\BuSuga11.frm(FrmSugaSerch)
     /// </seealso>
     /// <vbp>
     /// default : VB\basic\busuga\bvsuga.vbp
     /// </vbp>

    public partial class FrmSugaSerch : Form
    {
        string  GHelpCode = "";

        //이벤트를 전달할 경우
        public delegate void SetHelpCode (string strCode);
        public event SetHelpCode rSetHelpCode;

        //폼이 Close될 경우
        public delegate void EventClosed ();
        public event EventClosed rEventClosed;

        public FrmSugaSerch()
        {
            InitializeComponent();
        }

        public FrmSugaSerch (string strHelpCode)
        {
            InitializeComponent ();

            GHelpCode = strHelpCode;
        }

        private void FrmSugaSerch_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            txtCode.Text = "";
            ssView_Sheet1.RowCount = 0;
            optActive.Checked = true;
            optName.Checked = true;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return; //권한 확인

            try
            {
                txtCode.Text = txtCode.Text.Trim();

                if (txtCode.Text == "")
                {
                    ComFunc.MsgBox("찾으실 자료가 공란입니다", "확인");
                    txtCode.Focus();
                    return;
                }

                SQL = "SELECT Bun,Nu,SuCode,SuNext,SuNameK,SuNameG,HCode,";
                SQL = SQL + ComNum.VBLF + "     IAmt,TAmt,BAmt, ";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(SuDate,'YYYY-MM-DD') SuDate,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,Bcode ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "VIEW_SUGA_CODE ";

                if (optName.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE UPPER(SuNameK) LIKE '%" + (VB.UCase(txtCode.Text).Trim()) + "%' ";
                    if (optActive.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND DELDATE IS NULL ";
                    }
                    if (optDelet.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND DELDATE IS NOT NULL ";
                    }
                    SQL = SQL + ComNum.VBLF + "ORDER BY SuNameK ";
                }
                else if (optSugaCode.Checked == true)
                {

                    SQL = SQL + ComNum.VBLF + "WHERE SuCode LIKE '%" + (VB.UCase(txtCode.Text).Trim()) + "%' ";
                    if (optActive.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND DELDATE IS NULL ";
                    }
                    if (optDelet.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND DELDATE IS NOT NULL ";
                    }
                    SQL = SQL + ComNum.VBLF + "ORDER BY SuCode ";
                }
                else if (optNameCode.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE SuNext LIKE '%" + (VB.UCase(txtCode.Text).Trim()) + "%' ";
                    if (optActive.Checked == true)
                    { 
                        SQL = SQL + ComNum.VBLF + " AND DELDATE IS NULL ";
                    }
                    if (optDelet.Checked == true) 
                    {
                        SQL = SQL + ComNum.VBLF + " AND DELDATE IS NOT NULL ";
                    }
                    SQL = SQL + ComNum.VBLF + "ORDER BY SuNext,SuCode ";
                }
                else if (optGanGaga.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE BCODE LIKE '%" + (txtCode.Text).Trim() + "%' ";
                    if (optActive.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND DELDATE IS NULL ";
                    }
                    if (optDelet.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND DELDATE IS NOT NULL ";
                    }
                    SQL = SQL + ComNum.VBLF + "ORDER BY HCode,SuCode,SuNext ";
                }

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Bun"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Nu"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SuNameG"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["HCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = VB.Format(dt.Rows[i]["BAmt"]).ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = VB.Format(dt.Rows[i]["TAmt"]).ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = VB.Format(dt.Rows[i]["IAmt"]).ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["BCode"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true) return;

            if (ssView_Sheet1.RowCount == 0) return;

            GHelpCode = (ssView_Sheet1.Cells[e.Row, 2]).Text.Trim();//ssView_Sheet1.ActiveRowIndex , 2].Text.Trim ());

            rSetHelpCode(GHelpCode);
            rEventClosed ();
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            strTitle = "수가 코드 찾기";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter); 

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;


            #endregion
        }
    }
}
