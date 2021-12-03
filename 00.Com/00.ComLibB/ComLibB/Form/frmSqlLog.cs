using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSqlLog.cs
    /// Description     : 접속이력 조회
    /// Author          : 박창욱
    /// Create Date     : 2019-02-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\basic\bucode\BuCode32.frm(FrmSqlLog) >> frmSqlLog.cs 폼이름 재정의" />	
    public partial class frmSqlLog : Form
    {
        public frmSqlLog()
        {
            InitializeComponent();
        }

        private void frmSqlLog_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            txtIP.Text = "";
            txtExeCode.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ACTDATE, EXECODE, JOBSABUN, IP, GUBUN, SQL, ENTDATE ";
                SQL += ComNum.VBLF + " FROM ADMIN.ETC_SQLLOG_" + dtpFDate.Value.ToString("yyyyMMdd");
                SQL += ComNum.VBLF + "  WHERE  rownum < 100 ";
                if (txtIP.Text != "")
                {
                    SQL += ComNum.VBLF + "    AND IP = '" + txtIP.Text + "'  ";
                }
                if (txtExeCode.Text != "")
                {
                    SQL += ComNum.VBLF + "    AND EXECODE = '" + txtExeCode.Text + "'  ";
                }
                SQL += ComNum.VBLF + "   AND JOBSABUN IN  ( ";
                SQL += ComNum.VBLF + "                       SELECT SABUN FROM ADMIN.INSA_MST ";
                SQL += ComNum.VBLF + "                        WHERE (TOIDAY IS NOT NULL  or TOIDAY <= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ) ";
                SQL += ComNum.VBLF + "                          AND JOBSABUN NOT IN ('18319') ) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["EXECODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JOBSABUN"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IP"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SQL"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            if (chkOut.Checked)
            {
                strTitle = "정보시스템 비인가 접속이력";
            }
            else
            {
                strTitle = "정보시스템 접속이력";
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회일자 : " + dtpFDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("IP : " + txtIP.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("프로그램 : " + txtExeCode.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);

            //VB -> C# : 사용하지 않는 기능 주석처리
            //Call SQL_LOG("", SS1.PrintHeader)
        }
    }
}
