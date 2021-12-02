using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongMisuDept.cs
    /// Description     : 월말현재 진료과별 미수금 통계
    /// Author          : 박창욱
    /// Create Date     : 2017-08-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misup203.frm(FrmMisuDeptTong.frm) >> frmPmpaTongMisuDept.cs 폼이름 재정의" />	
    public partial class frmPmpaTongMisuDept : Form
    {
        double[] nTotCnt = new double[8];
        double[] nTotAmt = new double[8];

        public frmPmpaTongMisuDept()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 2;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead = "";
            string strHead1 = "";
            string strHead2 = "";
            string PrintDate = "";
            string JobMan = "";

            JobMan = clsType.User.JobMan;

            PrintDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");

            //PrintHead
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead = cboFYYMM.Text + "~" + cboTYYMM.Text + " 진료과별 미수통계";

            strHead1 = "/f1" + VB.Space(5) + strHead;
            strHead2 = "/l/f2" + "작 성 자 : " + JobMan;
            strHead2 = strHead2 + VB.Space(50) + "출력시간 : " + PrintDate;


            //PrintBody
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 0;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 5;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            int nRow = 0;
            string strFYYMM = "";
            string strTYYMM = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            for (i = 1; i < 8; i++)
            {
                nTotCnt[i] = 0;
                nTotAmt[i] = 0;
            }

            ssView_Sheet1.RowCount = 2;

            strFYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 6, 2);
            strTYYMM = VB.Left(cboTYYMM.Text, 4) + VB.Mid(cboTYYMM.Text, 6, 2);

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;

            try
            {
                SQL = "";
                SQL = " SELECT b.PrintRanking,a.DeptCode,b.DeptNameK,                     ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(IwolAmt,0,0,1)) IwolCnt,SUM(IwolAmt) IwolAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonMAmt,0,0,1)) MonMCnt,SUM(MonMAmt) MonMAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonIAmt,0,0,1)) MonICnt,SUM(MonIAmt) MonIAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonSAmt,0,0,1)) MonSCnt,SUM(MonSAmt) MonSAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonbAmt,0,0,1)) MonbCnt,SUM(MonbAmt) MonbAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(MonGAmt,0,0,1)) MonGCnt,SUM(MonGAmt) MonGAmt,   ";
                SQL = SQL + ComNum.VBLF + "        SUM(DeCode(JanAmt,0,0,1))  JanCnt, SUM(JanAmt)  JanAmt     ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINTONG a, " + ComNum.DB_PMPA + "BAS_ClinicDept b";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM >= '" + strFYYMM + "'                               ";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM <= '" + strTYYMM + "'                               ";
                if (rdoGbn1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun <> '11'             ";
                }
                if (rdoGbn2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Gubun =  '11'             ";
                }
                SQL = SQL + ComNum.VBLF + "    AND a.DeptCode = b.DeptCode(+)                                 ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY b.PrintRanking,a.DeptCode,b.DeptNameK                   ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY b.PrintRanking                                          ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");

                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    return;
                }

                nRow = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRow + 3;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRow; i++)
                {
                    nTotCnt[1] += VB.Val(dt.Rows[i]["IwolCnt"].ToString().Trim());
                    nTotCnt[2] += VB.Val(dt.Rows[i]["MonMCnt"].ToString().Trim());
                    nTotCnt[3] += VB.Val(dt.Rows[i]["MonICnt"].ToString().Trim());
                    nTotCnt[4] += VB.Val(dt.Rows[i]["MonBCnt"].ToString().Trim());
                    nTotCnt[5] += VB.Val(dt.Rows[i]["MonGCnt"].ToString().Trim());
                    nTotCnt[6] += VB.Val(dt.Rows[i]["MonSCnt"].ToString().Trim());
                    nTotCnt[7] += VB.Val(dt.Rows[i]["JanCnt"].ToString().Trim());

                    nTotAmt[1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim());
                    nTotAmt[2] += VB.Val(dt.Rows[i]["MonMAmt"].ToString().Trim());
                    nTotAmt[3] += VB.Val(dt.Rows[i]["MonIAmt"].ToString().Trim());
                    nTotAmt[4] += VB.Val(dt.Rows[i]["MonBAmt"].ToString().Trim());
                    nTotAmt[5] += VB.Val(dt.Rows[i]["MonGAmt"].ToString().Trim());
                    nTotAmt[6] += VB.Val(dt.Rows[i]["MonSAmt"].ToString().Trim());
                    nTotAmt[7] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());

                    if (dt.Rows[i]["DeptNameK"].ToString().Trim() == "")
                    {
                        ssView_Sheet1.Cells[i + 2, 0].Text = "진료과 오류";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i + 2, 0].Text = dt.Rows[i]["DeptNameK"].ToString().Trim();
                    }

                    ssView_Sheet1.Cells[i + 2, 1].Text = VB.Val(dt.Rows[i]["IwolCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 2].Text = VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 3].Text = VB.Val(dt.Rows[i]["MonMCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 4].Text = VB.Val(dt.Rows[i]["MonMAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 5].Text = VB.Val(dt.Rows[i]["MonICnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 6].Text = VB.Val(dt.Rows[i]["MonIAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 7].Text = VB.Val(dt.Rows[i]["MonbCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 8].Text = VB.Val(dt.Rows[i]["MonbAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 9].Text = VB.Val(dt.Rows[i]["MonGCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 10].Text = VB.Val(dt.Rows[i]["MonGAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 11].Text = VB.Val(dt.Rows[i]["MonSCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 12].Text = VB.Val(dt.Rows[i]["MonSAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 13].Text = VB.Val(dt.Rows[i]["JanCnt"].ToString().Trim()).ToString("###,##0 ");
                    ssView_Sheet1.Cells[i + 2, 14].Text = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                }



                dt.Dispose();
                dt = null;

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "** 합계 **";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = nTotCnt[1].ToString("###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = nTotAmt[1].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = nTotCnt[2].ToString("###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nTotAmt[2].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nTotCnt[3].ToString("###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = nTotAmt[3].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nTotCnt[4].ToString("###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nTotAmt[4].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nTotCnt[5].ToString("###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nTotAmt[5].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = nTotCnt[6].ToString("###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = nTotAmt[6].ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = nTotCnt[7].ToString("###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Text = nTotAmt[7].ToString("###,###,###,##0 ");

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);

                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;
            }
        }

        private void frmPmpaTongMisuDept_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFYYMM, 20, "", "0");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTYYMM, 20, "", "0");

            cboFYYMM.SelectedIndex = 0;
            cboTYYMM.SelectedIndex = 0;
        }

        private void rdoGbn0_Click(object sender, EventArgs e)
        {
            rdoGbn0.ForeColor = Color.FromArgb(0, 0, 255);
            rdoGbn1.ForeColor = Color.FromArgb(0, 0, 0);
            rdoGbn2.ForeColor = Color.FromArgb(0, 0, 0);
        }

        private void rdoGbn1_Click(object sender, EventArgs e)
        {
            rdoGbn0.ForeColor = Color.FromArgb(0, 0, 0);
            rdoGbn1.ForeColor = Color.FromArgb(0, 0, 255);
            rdoGbn2.ForeColor = Color.FromArgb(0, 0, 0);
        }

        private void rdoGbn2_Click(object sender, EventArgs e)
        {
            rdoGbn0.ForeColor = Color.FromArgb(0, 0, 0);
            rdoGbn1.ForeColor = Color.FromArgb(0, 0, 0);
            rdoGbn2.ForeColor = Color.FromArgb(0, 0, 255);
        }
    }
}
