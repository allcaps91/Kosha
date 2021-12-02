using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSanOpdPatient
    /// File Name : frmSanOpdPatient.cs
    /// Title or Description : 수가변경내역 조회
    /// Author : 박창욱
    /// Create Date : 2017-06-02
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    /// </summary>
    /// <history>  
    /// VB\BuSuga21.frm(FrmChangeView) -> frmChangeView.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busuga\FrmBuSuga21.frm(FrmChangeView)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busuga\\busuga.vbp
    /// </vbp>
    public partial class frmChangeView : Form
    {
        public frmChangeView()
        {
            InitializeComponent();
        }

        private void screen_Clear()
        {
            txtSuCode.Text = "";
            rdoTerm.Enabled = true;
            rdoCode.Enabled = true;
            dtpFDate.Enabled = true;
            dtpTDate.Enabled = true;
            txtSuCode.Enabled = true;
            btnSearch.Enabled = true;
            btnCancel.Enabled = true;
            btnPrint.Enabled = true;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            screen_Clear();
            btnSearch.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            ssView_Sheet1.Columns[20].Visible = false;
            ssView_Sheet1.Columns[21].Visible = false;
            ssView_Sheet1.Columns[22].Visible = false;
            ssView_Sheet1.Columns[23].Visible = false;

            Cursor.Current = Cursors.WaitCursor;

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fs2";
            strHead1 = "/l/f1" + VB.Space(40) + "수가코드 변경 내역" + "/n";
            strHead2 = "/l/f2" + "인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            strHead2 = strHead2 + VB.Space(130) + "PAGE : /p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 5;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            Cursor.Current = Cursors.Default;

            ssView_Sheet1.Columns[20].Visible = true;
            ssView_Sheet1.Columns[21].Visible = true;
            ssView_Sheet1.Columns[22].Visible = true;
            ssView_Sheet1.Columns[23].Visible = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            btnSearch.Enabled = false;
            rdoTerm.Enabled = false;
            rdoCode.Enabled = false;
            dtpFDate.Enabled = false;
            dtpTDate.Enabled = false;
            txtSuCode.Enabled = false;

            if (searchData() == true)
            {
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
                ssView.Focus();
            }
        }

        private bool searchData()
        {
            int i = 0;
            int nRow = 0;
            int nRead = 0;
            string strOldData1 = "";
            string strNewData1 = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(JobDate,'YYYY-MM-DD') JobDate,JobSabun,JobGbn, ";
                SQL = SQL + ComNum.VBLF + " CASE WHEN B.USERNAME IS NULL THEN '*****' ";
                SQL = SQL + ComNum.VBLF + " ELSE B.USERNAME ";
                SQL = SQL + ComNum.VBLF + " END AS USERNAME, ";
                SQL = SQL + ComNum.VBLF + " SuCode, Bun,Nu,SugbA,SugbB,SugbC,SugbD,SugbE,SugbF,SugbG,SugbH,SugbI,";
                SQL = SQL + ComNum.VBLF + " SugbJ,SugbK,SugbL,SugbM, SugbN, SugbO, SugbP, SugbQ, SugbR, ";
                SQL = SQL + ComNum.VBLF + " SugbS, SugbT, SugbU, SugbSS,SugbBi,SuQty,IAmt,TAmt,BAmt,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SuDate,'YY-MM-DD') SuDate,OldIAmt,OldTAmt,OldBAmt,";
                SQL = SQL + ComNum.VBLF + " SuNext,TO_CHAR(DelDate,'YY-MM-DD') DelDate,SuNameK,Unit,";
                SQL = SQL + ComNum.VBLF + " DaiCode,HCode,BCode,TableGbn ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_SUGAHIS A";
                SQL = SQL + ComNum.VBLF + "   LEFT OUTER JOIN  KOSMOS_PMPA.BAS_USER B";
                SQL = SQL + ComNum.VBLF + "     ON B.IDNUMBER = A.JOBSABUN";

                if (rdoTerm.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE JobDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND JobDate <  TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') + 1 ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE SuCode = '" + txtSuCode.Text.Trim().ToUpper() + "' ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY JobDate DESC,SuCode,JobGbn,TableGbn DESC,JobSabun,SuNext ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                nRead = dt.Rows.Count;
                strOldData1 = "";
                nRow = 0;

                //조회용 Sheet에 Display
                for (i = 0; i < nRead; i++, nRow++)
                {
                    strNewData1 = dt.Rows[i]["JobDate"].ToString().Trim() + VB.Left(dt.Rows[i]["SuCode"].ToString().Trim() + VB.Space(8), 8);
                    strNewData1 = strNewData1 + VB.Format(Convert.ToInt32(dt.Rows[i]["JobSabun"].ToString().Trim()), "####0") + dt.Rows[i]["JobGbn"].ToString().Trim();

                    if (nRow >= ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow + 1;
                    }

                    //작업자 성명을 READ
                    if (strNewData1 != strOldData1)
                    {
                        ssView_Sheet1.Cells[nRow, 0].Text = dt.Rows[i]["JobDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow, 1].Text = dt.Rows[i]["USERNAME"].ToString().Trim();

                        switch (dt.Rows[i]["JobGbn"].ToString().Trim())
                        {
                            case "1":
                                ssView_Sheet1.Cells[nRow, 2].Text = "신규";
                                break;
                            case "2":
                                ssView_Sheet1.Cells[nRow, 2].Text = "전";
                                break;
                            case "3":
                                ssView_Sheet1.Cells[nRow, 2].Text = "후";
                                break;
                            case "4":
                                ssView_Sheet1.Cells[nRow, 2].Text = "삭제";
                                break;
                        }
                        ssView_Sheet1.Cells[nRow, 3].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                        strOldData1 = strNewData1;
                    }

                    ssView_Sheet1.Cells[nRow, 4].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow, 5].Text = dt.Rows[i]["TableGbn"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow, 6].Text = dt.Rows[i]["Bun"].ToString().Trim() + "," + dt.Rows[i]["Nu"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow, 7].Text =  VB.Val(dt.Rows[i]["SugbA"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 8].Text =  VB.Val(dt.Rows[i]["SugbB"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 9].Text =  VB.Val(dt.Rows[i]["SugbC"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 10].Text = VB.Val(dt.Rows[i]["SugbD"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 11].Text = VB.Val(dt.Rows[i]["SugbE"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 12].Text = VB.Val(dt.Rows[i]["SugbF"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 13].Text = VB.Val(dt.Rows[i]["SugbG"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 14].Text = VB.Val(dt.Rows[i]["SugbH"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 15].Text = VB.Val(dt.Rows[i]["SugbI"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 16].Text = VB.Val(dt.Rows[i]["SugbJ"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 17].Text = VB.Val(dt.Rows[i]["SugbK"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 18].Text = VB.Val(dt.Rows[i]["SugbL"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 19].Text = VB.Val(dt.Rows[i]["SugbM"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 20].Text = VB.Val(dt.Rows[i]["SugbN"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 21].Text = VB.Val(dt.Rows[i]["SugbO"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 22].Text = VB.Val(dt.Rows[i]["SugbP"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 23].Text = VB.Val(dt.Rows[i]["SugbQ"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 24].Text = VB.Val(dt.Rows[i]["SugbR"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 25].Text = VB.Val(dt.Rows[i]["SugbS"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 26].Text = VB.Val(dt.Rows[i]["SugbT"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 27].Text = VB.Val(dt.Rows[i]["SugbU"].ToString().Trim()).ToString("0");
                    ssView_Sheet1.Cells[nRow, 28].Text = "";
                    ssView_Sheet1.Cells[nRow, 29].Text = "";
                    ssView_Sheet1.Cells[nRow, 30].Text = "";
                    ssView_Sheet1.Cells[nRow, 31].Text = "";
                    ssView_Sheet1.Cells[nRow, 32].Text = "";
                    ssView_Sheet1.Cells[nRow, 33].Text = dt.Rows[i]["SugbSS"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow, 34].Text = dt.Rows[i]["SugbBi"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow, 35].Text = dt.Rows[i]["SuQty"].ToString().Trim(); //VB - AdoGetNumber임.
                    ssView_Sheet1.Cells[nRow, 36].Text = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow, 37].Text = VB.Val(dt.Rows[i]["TAmt"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow, 38].Text = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow, 39].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow, 40].Text = VB.Val(dt.Rows[i]["OldIAmt"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow, 41].Text = VB.Val(dt.Rows[i]["OldTAmt"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow, 42].Text = VB.Val(dt.Rows[i]["OldBAmt"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow, 43].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow, 44].Text = dt.Rows[i]["Unit"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow, 45].Text = dt.Rows[i]["DaiCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow, 46].Text = dt.Rows[i]["HCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow, 47].Text = dt.Rows[i]["BCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow, 48].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                }
                ssView_Sheet1.RowCount = nRow;

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                return true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private void frmChangeView_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ComFunc.ReadSysDate(clsDB.DbCon);

            txtSuCode.Visible = false;
            lblCode.Visible = false;
            txtSuCode.Top = dtpFDate.Top;
            lblCode.Top = lblTerm.Top;
            dtpFDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);  //VB - DATE_ADD(GstrSysDate, -7)
            dtpTDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            screen_Clear();

            ssView_Sheet1.Columns[28].Visible = false;
            ssView_Sheet1.Columns[29].Visible = false;
            ssView_Sheet1.Columns[30].Visible = false;
            ssView_Sheet1.Columns[31].Visible = false;
            ssView_Sheet1.Columns[32].Visible = false;
        }

        private void rdoTerm_Click(object sender, EventArgs e)
        {
            if (rdoTerm.Checked == true)
            {
                //FrameData.Caption = "조회하실 기간은?"

                txtSuCode.Visible = false;
                lblCode.Visible = false;
                dtpFDate.Visible = true;
                dtpTDate.Visible = true;
                lblTerm.Visible = true;
            }
        }

        private void rdoCode_Click(object sender, EventArgs e)
        {
            if (rdoCode.Checked == true)
            {
                //FrameData.Caption = "조회하실 수가코드는?"

                txtSuCode.Visible = true;
                lblCode.Visible = true;
                dtpFDate.Visible = false;
                dtpTDate.Visible = false;
                lblTerm.Visible = false;
            }
        }

        private void dtpFDate_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void dtpTDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtSuCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

    }
}
