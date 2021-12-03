using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComBase;

namespace ComLibB
{

    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmChulgo.cs
    /// Description     : 월별 재원미수금 집계표
    /// Author          : 최익준
    /// Create Date     : 2017-10-17
    /// <seealso>  
    /// PSMH\Ocd\ipdoxd\nurview\Nview16.frm
    /// </seealso>
    /// <history>
    /// </history>
    /// </summary>
    /// 
    public partial class frmChulgo : Form
    {
        string GstrWardCode = "";
        string GstrICUWard = "";

        public frmChulgo()
        {
            InitializeComponent();
        }

        public frmChulgo(string strWardCode, string strIUCWard)
        {
            InitializeComponent();

            GstrWardCode = strWardCode;
            GstrICUWard = strIUCWard;
        }

        private void frmChulgo_Load(object sender, EventArgs e)
        {

            if (ComQuery.isFormAuth(clsDB.DbCon,this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssRelease_Sheet1.Visible = false;
            rdoSupply.Checked = true;
            rdoMichul.Checked = true;
            rdoCode.Checked = true;
            dtpStart.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpEnd.Value = dtpStart.Value;
            btnPrint.Enabled = false;
            ssView.Dock = DockStyle.Fill;

            if (GstrWardCode == "")
            {
                GstrWardCode = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");
            }

            if (GstrICUWard == "")
            {
                GstrICUWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssRelease.Visible = false;
            ssView.Visible = true;
            ssView_Sheet1.RowCount = 0;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "P",clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strHead3 = "";

            if (ComFunc.MsgBoxQ("자료를 출력하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            btnPrint.Enabled = false;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/c/f1 OCS전달 물품" + "/n/n";

            strHead3 = "/n/l/f2" + " 조회일자 : " + dtpStart.Text + "부터  " + dtpEnd.Text + "까지" +
            VB.Space(5) + " 출력시간 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon,"D"), "D", "-") 
            + " / " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon,"T"), "T", ":") + "/r" + "Page : " + "/p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2 + strHead3 + "/n";
            ssView_Sheet1.PrintInfo.Centering = Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 40;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = PrintType.All;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;
        }

        private void btnChulgo_Click(object sender, EventArgs e)
        {
            int i = 0;
            int nRow = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strOldData = "";
            string strNewData = "";

            Cursor.Current = Cursors.WaitCursor;

            ssView.Visible = false;
            ssRelease.Visible = true;

            ssRelease_Sheet1.RowCount = 0;
            ssRelease_Sheet1.RowCount = 50;

            //출고 마감한 자료를 SHEET에 Display

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate,ORDERCODE,";
                SQL = SQL + ComNum.VBLF + "       ORDERNAME,GbInfo,SUM(QTY) QTY ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_GUMESEND ";
                SQL = SQL + ComNum.VBLF + " WHERE CDate >= TO_DATE('" + dtpStart.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND CDate <= TO_DATE('" + dtpEnd.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND GBIO ='I' ";

                if (GstrWardCode == "IU")
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='" + GstrICUWard + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND WARDCODE ='" + GstrWardCode + "'";
                }

                if (rdoDept.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBBUSE ='1'";       //관리과
                }

                if (rdoSupply.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBBUSE ='2'";       //공급실
                }

                SQL = SQL + ComNum.VBLF + " GROUP BY EntDate,ORDERCODE,OrderName,GbInfo ";
                SQL = SQL + ComNum.VBLF + " ORDER BY EntDate DESC,ORDERCODE,GbInfo ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("인쇄할 자료가 없습니다.", "확인");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssRelease_Sheet1.Visible = true;

                nRow = 0;
                strOldData = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (VB.Val(dt.Rows[i]["Qty"].ToString()) != 0)
                    {
                        strNewData = dt.Rows[i]["EntDate"].ToString();
                        nRow = nRow + 1;
                        if (nRow > ssRelease_Sheet1.RowCount)
                        {
                            ssRelease_Sheet1.RowCount = nRow;
                        }

                        if (strOldData != strNewData)
                        {
                            ssRelease_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["EntDate"].ToString();
                            ssRelease_Sheet1.Cells[nRow - 1, 1].Text = GstrWardCode;

                            //Sheet줄그리기
                            //SS2.Row = nRow:    SS2.Row2 = nRow;
                            //SS2.Col = 1:       SS2.Col2 = SS2.MaxCols;
                            //SS2.BlockMode = True;
                            //Determines the section of the cell border displayed around the entire spreadsheet
                            //SS2.CellBorderType = SS_BORDER_TYPE_LEFT Or SS_BORDER_TYPE_RIGHT Or SS_BORDER_TYPE_TOP;
                            //SS2.CellBorderStyle = SS_BORDER_STYLE_SOLID;
                            //SS2.Action = SS_ACTION_SET_CELL_BORDER;
                            //SS2.BlockMode = False;
                            
                            strOldData = strNewData;
                        }

                        ssRelease_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["OrderCode"].ToString().Trim();
                        ssRelease_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Qty"].ToString();
                        ssRelease_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["OrderName"].ToString().Trim() + " " + dt.Rows[i]["GbInfo"].ToString();

                    }

                }

                //Sheet줄그리기
                //SS2.Row = nRow:    SS2.Row2 = nRow;
                //SS2.Col = 1:       SS2.Col2 = SS2.MaxCols;
                //SS2.BlockMode = True;
                //Determines the section of the cell border displayed around the entire spreadsheet
                //SS2.CellBorderType = SS_BORDER_TYPE_LEFT Or SS_BORDER_TYPE_RIGHT Or SS_BORDER_TYPE_BOTTOM;
                //SS2.CellBorderStyle = SS_BORDER_STYLE_SOLID;
                //SS2.Action = SS_ACTION_SET_CELL_BORDER;
                //SS2.BlockMode = False;
                //ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                ssRelease_Sheet1.RowCount = nRow;

                dt.Dispose();
                dt = null;

                btnCancel.Enabled = true;

                Cursor.Current = Cursors.Default;

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
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string strSDate = "";
            string strEDate = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView.Visible = true;
            ssRelease.Visible = false;
            strSDate = dtpStart.Value.ToString("yyyy-MM-dd");
            strEDate = dtpEnd.Value.ToString("yyyy-MM-dd");
            btnPrint.Enabled = false;
            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, GBIO, PANO, SNAME, ORDERCODE, ORDERNAME,GbInfo,";
                SQL = SQL + ComNum.VBLF + "                     SUCODE, QTY, ORDERNO, CDATE, ENTDATE, ENTSABUN ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_GUMESEND ";

                if (rdoMichul.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE BDATE >=TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND BDATE <=TO_DATE('" + strEDate + "','YYYY-MM-DD')";
                }

                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE CDATE >=TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND CDATE <=TO_DATE('" + strEDate + "','YYYY-MM-DD')";
                }

                if (GstrWardCode == "IU")
                {
                    SQL = SQL + ComNum.VBLF + "     AND WARDCODE ='" + GstrICUWard + "'";
                }

                else
                {
                    if (GstrWardCode == "ND" || GstrWardCode == "IQ" || GstrWardCode == "DR" || GstrWardCode == "ND")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND  WardCode IN ('ND','DR','IQ','NR')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND WARDCODE ='" + GstrWardCode + "'";
                    }
                }

                if (rdoMichul.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND CDATE IS NULL";      //미출고
                }
                if (rdoChul.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND CDATE IS NOT NULL";       //출고
                }

                if (rdoDept.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND GBBUSE ='1'";        //관리과
                }
                if (rdoSupply.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     AND GBBUSE ='2'";        //공급실
                }

                if (rdoNum.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY PANO,BDate,ORDERCODE,GbInfo";
                }
                if (rdoCode.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY OrderCode,GbInfo,Pano,BDate";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["QTY"].ToString();

                    if (VB.Val(ssView_Sheet1.Cells[i, 2].Text) < 0)
                    {
                        ssView_Sheet1.Rows[i].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                        ssView_Sheet1.Rows[i + 1].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    }

                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BDATE"].ToString();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ORDERCODE"].ToString();
                    ssView_Sheet1.Cells[i, 5].Text = " " + dt.Rows[i]["ORDERNAME"].ToString() + " " + dt.Rows[i]["GbInfo"];
                }

                ssView_Sheet1.RowCount = i;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (ssView_Sheet1.RowCount >= 0)
                {
                    btnPrint.Enabled = true;
                }
                else
                {
                    btnPrint.Enabled = false;
                }

                dt.Dispose();
                dt = null;

                //VB

                //If UCase(App.EXEName) = "ENDRES" Then
                //   cmdPrint.Enabled = True
                //Else
                //   cmdPrint.Enabled = False
                //End If

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
                Cursor.Current = Cursors.Default;
            }
        }

        private void dtpStart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtpEnd.Focus();
            }
        }

        private void dtpEnd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }
    }
}
