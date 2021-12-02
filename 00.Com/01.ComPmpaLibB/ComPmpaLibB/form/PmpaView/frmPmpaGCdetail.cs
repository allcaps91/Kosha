using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaGCdetail : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmpaGCdetail.cs
        /// Description     : 카드거래내역조회
        /// Author          : 김효성
        /// Create Date     : 2017-08-18
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "\IPD\iument\iument.vbp(GCdetail,GCDETAIL.frm) >> frmPmpaGCdetail.cs 폼이름 재정의" />	

        string[] GstrSETDeptCodes = new string[51];            //'과목코드       안내 Array

        public frmPmpaGCdetail(string[] strSETDeptCodes)
        {
            GstrSETDeptCodes = strSETDeptCodes;

            InitializeComponent();
        }

        public frmPmpaGCdetail()
        {
            InitializeComponent();
        }

        private void frmPmpaGCdetail_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            string strDTP = "";
            strDTP = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            cboGubun.Items.Add("매출일자");
            cboGubun.Items.Add("병록번호");
            cboGubun.Items.Add("카드번호");
            cboGubun.SelectedIndex = 0;

            CardCombo_SELECT();

            cboCompany.SelectedIndex = 0;

            dtpFDate.Value = Convert.ToDateTime(strDTP);
            dtpTDate.Value = Convert.ToDateTime(strDTP);

            cboDept.Items.Add("전체");

            for (i = 0; i <= 50; i++)
            {
                if (GstrSETDeptCodes[i] == "")
                {
                    cboDept.Items.Add(GstrSETDeptCodes[i]);
                }
            }
            cboDept.SelectedIndex = 0;
            txtPtno.Text = "";
        }

        private void CardCombo_SELECT()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.Columns[5].Visible = false;   //개인정보 카드번호 숨김

            cboCompany.Items.Clear();
            cboCompany.Items.Add("전체");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "	SELECT CodeName_Ex, Remark1, Min(Code) Code    ";
                SQL = SQL + ComNum.VBLF + "	    FROM BAS_BASECODE                       ";
                SQL = SQL + ComNum.VBLF + "	    WHERE BUSINESS = 'CARDCODE_NEW'                   ";
                SQL = SQL + ComNum.VBLF + "	     AND GbUse = '0' AND BUN = '01'                            ";
                SQL = SQL + ComNum.VBLF + "	    GROUP BY CodeName_Ex, Remark1                 ";
                SQL = SQL + ComNum.VBLF + "	    ORDER BY Remark1                              ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboCompany.Items.Add(dt.Rows[i]["CodeName_Ex"].ToString().Trim() + ComNum.VBLF + dt.Rows[i]["CODE"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string strDTP = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            dtpFDate.Value = Convert.ToDateTime(strDTP);
            dtpTDate.Value = Convert.ToDateTime(strDTP);

            if (cboGubun.SelectedIndex == 0)
            {
                txtPtno.Visible = false;
                pan4.Visible = true;
            }
            else
            {
                txtPtno.Visible = true;
                txtPtno.Text = "";
                txtPtno.Focus();
                pan4.Visible = false;
            }
            txtPart.Text = "";
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 30;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            string strCardNo = "";
            string strDept = "";
            double nTotCard = 0;
            double nCard = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strFullCardNo = "";

            Cursor.Current = Cursors.WaitCursor;

            strDept = VB.Left(cboDept.Text, 2);

            if (cboGubun.SelectedIndex == 1)
            {
                txtPtno.Text = VB.Val(txtPtno.Text).ToString("00000000");
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "	SELECT TO_CHAR(TranDate, 'YYYY-MM-DD') ActDate,GUBUN, Pano Pano ";
                SQL = SQL + ComNum.VBLF + "       ,DeptCode DeptCode, SName SName, AccepterName CompanyName ";
                SQL = SQL + ComNum.VBLF + "       ,TranHeader,CardNo,FullCardNo ";
                SQL = SQL + ComNum.VBLF + "       ,FiName,  Period, AccountNo,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(TranDate, 'YYMMDDHH24MI') TranDate1, TO_CHAR(OriginDate, 'YYMMDD') POriginDate,";
                SQL = SQL + ComNum.VBLF + "       case when TranHeader = '1' or TranHeader = '11' then DECODE(InstPeriod, '0', '일승', '할승') ";
                SQL = SQL + ComNum.VBLF + "       When TranHeader = '2' or TranHeader = '22' then DECODE(InstPeriod, '0', '일취', '할취') END  GbRecord,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(OriginDate, 'YYYY-MM-DD') OriginDate, ";
                SQL = SQL + ComNum.VBLF + "       InstPeriod InstPeriod, ";
                SQL = SQL + ComNum.VBLF + "       TradeAmt TradeAmt, ";
                SQL = SQL + ComNum.VBLF + "       OriginNo OriginNo, TO_CHAR(ENTERDATE,'YYYY-MM-DD HH24:MI') ENTERDATE ";
                

                if (OptJob1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1 ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "CARD_APPROV_CENTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1 ";
                }

                if (cboGubun.SelectedIndex == 0)
                {
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }

                else if (cboGubun.SelectedIndex == 1)
                {
                    SQL = SQL + ComNum.VBLF + "   AND Pano = '" + (txtPtno.Text.Trim()) + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND CardNo LIKE '%" + (txtPtno.Text.Trim()) + "%'";
                }


                if (VB.Right(cboCompany.Text, 2) != "전체")
                {
                    SQL = SQL + ComNum.VBLF + "   AND FICODE = '" + VB.Right(cboCompany.Text, 3) + "'";
                }

                if (txtPart.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND PART = '" + (txtPart.Text.Trim()) + "'";
                }

                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND GBIO  = 'O'";
                }
                else if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND GBIO  = 'I'";
                }

                if (rdoCard0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND (GUBUN = '1' OR GUBUN IS NULL) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND GUBUN  = '2' ";
                }

                if (OptJob1.Checked == true)
                {
                    if (rdoVan0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   AND ( PtGubun ='1' OR PtGubun IS NULL)                                          ";
                    }
                    else if (rdoVan1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   AND PtGubun ='2'                                                               ";
                    }
                    else if (rdoVan2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   AND PtGubun ='3'                                                               ";
                    }
                    else if (rdoVan3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   AND PtGubun ='4'                                                               ";
                    }
                }

                if (strDept != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND DEPTCODE = '" + strDept + "' ";
                }

                if (rdoBun1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND  INPUTMETHOD IN ('S','K') ";
                }
                else if (rdoBun2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND  INPUTMETHOD = 'T' ";// '단말기 승인
                }

                if (cboGubun.SelectedIndex == 0)
                {
                    if ((dtpFDate.Value) != (dtpTDate.Value))
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY ENTERDATE, TranDate, Pano, OriginNo ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY ENTERDATE, pano, OriginNo, TranDate DESC ";
                    }
                }
                else if (cboGubun.SelectedIndex == 1)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY pano, OriginNo, TranDate DESC";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY CardNo, OriginNo, TranDate DESC";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strCardNo = dt.Rows[i]["CARDNO"].ToString().Trim();
                    strFullCardNo = dt.Rows[i]["FULLCARDNO"].ToString().Trim();

                    ssView_Sheet1.Cells[(i + 1) - 1, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[(i + 1) - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[(i + 1) - 1, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[(i + 1) - 1, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[(i + 1) - 1, 4].Text = dt.Rows[i]["CompanyName"].ToString().Trim();

                    if (dt.Rows[i]["GUBUN"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[(i + 1) - 1, 5].Text = VB.Mid(strCardNo, 1, 4) + "-" + VB.Mid(strCardNo, 5, 4) + "-" + VB.Mid(strCardNo, 9, 4) + "-" + VB.Mid(strCardNo, 13, 4) + "XX";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[(i + 1) - 1, 5].Text = strCardNo;
                    }
                    ssView_Sheet1.Cells[(i + 1) - 1, 6].Text = dt.Rows[i]["GbRecord"].ToString().Trim();
                    ssView_Sheet1.Cells[(i + 1) - 1, 7].Text = dt.Rows[i]["InstPeriod"].ToString().Trim();
                    ssView_Sheet1.Cells[(i + 1) - 1, 8].Text = dt.Rows[i]["OriginDate"].ToString().Trim();

                    nCard = VB.Val(dt.Rows[i]["TradeAmt"].ToString().Trim());

                    if (dt.Rows[i]["GbRecord"].ToString().Trim() == "일취" || dt.Rows[i]["GbRecord"].ToString().Trim() == "할취")
                    {
                        nCard = nCard * -1;
                    }
                    ssView_Sheet1.Cells[(i + 1) - 1, 9].Text = nCard.ToString("###,###,###,###");
                    ssView_Sheet1.Cells[(i + 1) - 1, 10].Text = dt.Rows[i]["OriginNo"].ToString().Trim();
                    ssView_Sheet1.Cells[(i + 1) - 1, 11].Text = VB.Right(dt.Rows[i]["ENTERDATE"].ToString().Trim(), 5);
                    nTotCard = nTotCard + nCard;
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "합 계";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nTotCard.ToString("###,###,###,###");

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }


        private void txtPtno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cboGubun.SelectedIndex == 1)
                {
                    txtPtno.Text = VB.Format(txtPtno.Text, "00000000");
                }
                btnSearch.Focus();
            }
        }

        private void cboGubun_Click(object sender, EventArgs e)
        {
            if (cboGubun.SelectedIndex == 0)
            {
                grb1.Enabled = false;
                pan4.Enabled = true;
            }
            else
            {
                grb1.Enabled = true;
                pan4.Enabled = false;
                if (cboGubun.Text == "병록번호")
                {
                    txtPtno.MaxLength = 8;
                }
                else
                {
                    txtPtno.MaxLength = 8 * 2;
                }
                txtPtno.Text = "";
                txtPtno.Focus();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            btnPrint.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;


            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strBun = "";

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            if (rdoBun0.Checked == true || rdoVan1.Checked == true)
            {
                strBun = "(전체)";
            }
            else if (rdoBun1.Checked == true)
            {
                strBun = "(PC)";
            }
            else
            {
                strBun = "(단말)";
            }

            strFont1 = "/fn\"신명조\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"신명조\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";


            if (rdoVan2.Checked == true || rdoVan1.Checked == true)
            {
                strHead1 = "/n/f1/c" + "카드거래내력조회" + strBun;
            }
            else if (rdoVan0.Checked == true)
            {
                strHead1 = "/n/f1/c" + "카드거래내력조회[나이스]" + strBun;
            }

            strHead2 = "/n/l/f2" + "매출일자 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd") + "";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            //ssView_Sheet1 . PrintInfo . Orientation = FarPoint . Win . Spread . PrintOrientation . Landscape;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 40;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 60;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);
            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void cboCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPtno.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }
    }
}

