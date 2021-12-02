using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;


namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-10-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\misu\misubs\misubs.vbp\misubs52.frm" >> frmPmpaViewOpdMirCheck.cs 폼이름 재정의" />
    /// <seealso cref= D:\psmh\misu\misubs\misubs.vbp\misubs52_1.frm" >> frmPmpaViewOpdMirCheck.cs 폼이름 재정의" />

    public partial class frmPmpaViewOpdMirCheck : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        /// <summary>
        /// Flag = "1" (misubs52.frm)
        /// Flag = "2" (misubs52_1.frm)
        /// </summary>
        string Flag = "1";

        double[,] FnAmt = new double[8, 7];

        string GstrYYMM = "";
        string GstrMenu = "";
        string GstrSMenu = "";


        string strBiGbn = "";
        string strBi = "";
        int nBiNo = 0;
        double nAmt = 0;
        string strFDate = "";
        string strTdate = "";
        string strFYYMM = "";
        string strTYYMM = "";

        public frmPmpaViewOpdMirCheck()
        {
            InitializeComponent();
        }

        public frmPmpaViewOpdMirCheck(string strGstrYYMM, string strGstrMenu, string strGstrSMenu)
        {
            GstrYYMM = strGstrYYMM;
            GstrMenu = strGstrMenu;
            GstrSMenu = strGstrSMenu;

            InitializeComponent();
        }

        private void frmPmpaViewOpdMirCheck_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            panSS.Visible = false;

            btnChAmt.Enabled = false;
            btnRemark.Enabled = false;

            Flag = "2";

            if (Flag == "1")
            {
                lblTitle.Text = "월별 외래 청구액 점검표 (응급실 6시간, 낮 병동 명칭) >> 응급실 입원으로 변경";
                this.Size = new Size(1116, 500);
                panSS.Visible = false;
                panSSbtn.Visible = false;
            }
            else
            {
                lblTitle.Text = "월별 외래 청구액 점검표";
                this.Size = new Size(1116, 750);
                panSS.Visible = true;
                panSSbtn.Visible = true;
                ssView2_Sheet1.Visible = false;
                ssView2.Dock = DockStyle.None;
                SSView5_Sheet1.Visible = true;
                SSView5.Dock = DockStyle.Fill;
            }
            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 24, "", "1");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM2, 24, "", "1");

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    cboYYMM2.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

        }

        #region 함수모음
        private void SS_Set()
        {
            if (chkOpt0.Checked == false)// Then '건강보험
            {
                ssView_Sheet1.Rows[0].Visible = false;
                ssPrint_Sheet1.Rows[4].Visible = false;
            }
            else
            {
                ssView_Sheet1.Rows[0].Visible = true;
                ssPrint_Sheet1.Rows[4].Visible = true;
            }

            if (chkOpt1.Checked == false)// '의료급여
            {
                ssView_Sheet1.Rows[1].Visible = false;
                ssPrint_Sheet1.Rows[5].Visible = false;
            }
            else
            {
                ssView_Sheet1.Rows[1].Visible = true;
                ssPrint_Sheet1.Rows[5].Visible = true;
            }

            if (chkOpt0.Checked == true || chkOpt1.Checked == true)//Then '소계
            {
                ssView_Sheet1.Rows[2].Visible = true;
                ssPrint_Sheet1.Rows[6].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[2].Visible = false;
                ssPrint_Sheet1.Rows[6].Visible = false;
            }

            if (chkOpt3.Checked == false)//Then '산재
            {
                ssView_Sheet1.Rows[3].Visible = false;
                ssPrint_Sheet1.Rows[7].Visible = false;
            }
            else
            {
                ssView_Sheet1.Rows[3].Visible = true;
                ssPrint_Sheet1.Rows[7].Visible = true;
            }

            if (chkOpt2.Checked == false)//자보
            {
                ssView_Sheet1.Rows[4].Visible = false;
                ssPrint_Sheet1.Rows[8].Visible = false;
            }
            else
            {
                ssView_Sheet1.Rows[4].Visible = true;
                ssPrint_Sheet1.Rows[8].Visible = true;
            }

            if (chkOpt2.Checked == true || chkOpt3.Checked == true)// = 1 Then '소계
            {
                ssView_Sheet1.Rows[5].Visible = true;
                ssPrint_Sheet1.Rows[9].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[5].Visible = false;
                ssPrint_Sheet1.Rows[9].Visible = false;
            }

            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            ssPrint_Sheet1.Cells[4, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }
        #endregion

        private void btnCencel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount -1, ssView_Sheet1.ColumnCount-1].Text = "";
            //ssView_Sheet1.Cells[0, 0, ssView2_Sheet1.RowCount-1, ssView2_Sheet1.ColumnCount-1].Text = "";
            //ssView_Sheet1.Cells[0, ssView_Sheet1.Column - 1, 0, ssView_Sheet1.ColumnCount - 1].Text = "";
            //ssView2_Sheet1.Cells[0, ssView2_Sheet1.ColumnCount - 1, 0, ssView2_Sheet1.ColumnCount - 1].Text = "";

            ssView.Visible = true;
            ssView2.Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 차액 점검
        /// </summary>
        private void btnChAmt_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strYYMM = "";
            string strPano = "";
            string strSname = "";
            string strBi = "";
            string strFDate = "";
            string strTdate = "";
            double nEdiTAmt = 0;
            double nEdiJAmt = 0;
            double nEdiBamt = 0;
            double nTAmt = 0;
            double nBamt = 0;
            double nJAmt = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView2_Sheet1.ColumnCount = 10;
            ssView2_Sheet1.ColumnHeader.Cells[0, 0].Text = "청구년월";
            ssView2_Sheet1.ColumnHeader.Cells[0, 1].Text = "등록번호";
            ssView2_Sheet1.ColumnHeader.Cells[0, 2].Text = "성명";
            ssView2_Sheet1.ColumnHeader.Cells[0, 3].Text = "환자종류";
            ssView2_Sheet1.ColumnHeader.Cells[0, 4].Text = "총진료비";
            ssView2_Sheet1.ColumnHeader.Cells[0, 5].Text = "조합 부담금";
            ssView2_Sheet1.ColumnHeader.Cells[0, 6].Text = "본인 부담액";
            ssView2_Sheet1.ColumnHeader.Cells[0, 7].Text = "총 진료비";
            ssView2_Sheet1.ColumnHeader.Cells[0, 8].Text = "조합 부담금";
            ssView2_Sheet1.ColumnHeader.Cells[0, 9].Text = "본인 부담액";
            ssView2_Sheet1.ColumnHeader.Cells[0, 10].Text = "보합 부담금 차액";
            ssView2_Sheet1.Columns[0].Width = 80;
            ssView2_Sheet1.Columns[1].Width = 80;
            ssView2_Sheet1.Columns[2].Width = 80;
            ssView2_Sheet1.Columns[3].Width = 80;
            ssView2_Sheet1.Columns[4].Width = 90;
            ssView2_Sheet1.Columns[5].Width = 100;
            ssView2_Sheet1.Columns[6].Width = 100;
            ssView2_Sheet1.Columns[7].Width = 100;
            ssView2_Sheet1.Columns[8].Width = 100;
            ssView2_Sheet1.Columns[9].Width = 100;
            ssView2_Sheet1.Columns[10].Width = 110;
            //SS5
            panSS.Visible = true;
            SSView5_Sheet1.ColumnCount = 8;
            SSView5_Sheet1.ColumnHeader.Cells[0, 0].Text = "A";
            SSView5_Sheet1.ColumnHeader.Cells[0, 1].Text = "B";
            SSView5_Sheet1.ColumnHeader.Cells[0, 2].Text = "C";
            SSView5_Sheet1.ColumnHeader.Cells[0, 3].Text = "조합 미수";
            SSView5_Sheet1.ColumnHeader.Cells[0, 4].Text = "응급 6시간";
            SSView5_Sheet1.ColumnHeader.Cells[0, 5].Text = "조합 미수합";
            SSView5_Sheet1.ColumnHeader.Cells[0, 6].Text = "미수 발생";
            SSView5_Sheet1.ColumnHeader.Cells[0, 7].Text = "청구 차액";
            SSView5_Sheet1.ColumnHeader.Cells[0, 8].Text = "건수";
            SSView5_Sheet1.Columns[0].Width = 40;
            SSView5_Sheet1.Columns[1].Width = 40;
            SSView5_Sheet1.Columns[2].Width = 40;
            SSView5_Sheet1.Columns[3].Width = 120;
            SSView5_Sheet1.Columns[4].Width = 120;
            SSView5_Sheet1.Columns[5].Width = 120;
            SSView5_Sheet1.Columns[6].Width = 120;
            SSView5_Sheet1.Columns[7].Width = 120;
            SSView5_Sheet1.Columns[8].Width = 120;




            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + VB.Right(strYYMM, 2) + "-01";
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);


            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {
                SQL = "";
                SQL = " SELECT YYMM, PANO , SNAME, BI, SUM(EDITAMT) EDITAMT, SUM(EDIJAMT +EDIBOAMT) EDIJAMT, SUM(EDIBAMT) EDIBAMT";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSID";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "     AND YYMM ='" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "     AND IPDOPD='O'";
                SQL = SQL + ComNum.VBLF + "     AND UPCNT1 <> '9' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY YYMM, PANO ,SNAME, BI";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();
                    strSname = dt.Rows[i]["SNAME"].ToString().Trim();
                    strBi = dt.Rows[i]["BI"].ToString().Trim();

                    if (i >= ssView2_Sheet1.RowCount)
                    {
                        ssView2_Sheet1.RowCount = ssView2_Sheet1.RowCount + 20;
                    }

                    ssView2_Sheet1.Cells[i, 0].Text = strYYMM;
                    ssView2_Sheet1.Cells[i, 1].Text = strPano;
                    ssView2_Sheet1.Cells[i, 2].Text = strSname;
                    ssView2_Sheet1.Cells[i, 3].Text = strBi;
                    ssView2_Sheet1.Cells[i, 4].Text = Convert.ToDouble(dt.Rows[i]["EDITAMT"].ToString().Trim()).ToString("##,###,###,###,##0");
                    nEdiTAmt = nEdiTAmt + Convert.ToDouble(dt.Rows[i]["EDITAMT"].ToString().Trim());
                    ssView2_Sheet1.Cells[i, 5].Text = Convert.ToDouble(dt.Rows[i]["EDIJAMT"].ToString().Trim()).ToString("##,###,###,###,##0");
                    nEdiJAmt = nEdiJAmt + Convert.ToDouble(dt.Rows[i]["EDIJAMT"].ToString().Trim());
                    ssView2_Sheet1.Cells[i, 6].Text = Convert.ToDouble(dt.Rows[i]["EDIBAMT"].ToString().Trim()).ToString("##,###,###,###,##0");
                    nEdiBamt = nEdiBamt + Convert.ToDouble(dt.Rows[i]["EDIBAMT"].ToString().Trim());

                    SQL = "";
                    SQL = " SELECT SUM(DECODE(Bun,'92',0,'96',0,'98',0,'99',0,Amt1+Amt2)) TAmt,";
                    SQL = SQL + ComNum.VBLF + " SUM(DECODE(Bun,'92',Amt1+Amt2,0)) Bun92,";
                    SQL = SQL + ComNum.VBLF + " SUM(DECODE(Bun,'96',Amt1+Amt2,0)) Bun96,";
                    SQL = SQL + ComNum.VBLF + " SUM(DECODE(Bun,'98',Amt1+Amt2,0)) Bun98,";
                    SQL = SQL + ComNum.VBLF + " SUM(DECODE(Bun,'99',Amt1+Amt2,0)) Bun99";
                    SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP";
                    SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND PANO ='" + strPano + "'";
                    SQL = SQL + ComNum.VBLF + "   AND BI = '" + strBi + "'";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        ssView2_Sheet1.Cells[i, 8].Text = Convert.ToDouble(dt2.Rows[0]["TAMT"].ToString().Trim()).ToString("##,###,###,###,##0");
                        ssView2_Sheet1.Cells[i, 9].Text = Convert.ToDouble(dt2.Rows[0]["BUN98"].ToString().Trim()).ToString("##,###,###,###,##0");
                        ssView2_Sheet1.Cells[i, 10].Text = Convert.ToDouble(dt2.Rows[0]["BUN99"].ToString().Trim()).ToString("##,###,###,###,##0");
                        ssView2_Sheet1.Cells[i, 11].Text = (Convert.ToDouble(dt.Rows[i]["EDIJAMT"].ToString().Trim()) - Convert.ToDouble(dt2.Rows[0]["BUN98"].ToString().Trim())).ToString("##,###,###,###,##0");
                    }
                    dt2.Dispose();
                    dt2 = null;

                }
                ssView2_Sheet1.Cells[i, 0].Text = "합 계";
                ssView2_Sheet1.Cells[i, 4].Text = nEdiTAmt.ToString("##,###,###,###,##0");
                ssView2_Sheet1.Cells[i, 5].Text = nEdiJAmt.ToString("##,###,###,###,##0");
                ssView2_Sheet1.Cells[i, 6].Text = nEdiBamt.ToString("##,###,###,###,##0");
                ssView2_Sheet1.Cells[i, 8].Text = nTAmt.ToString("##,###,###,###,##0");
                ssView2_Sheet1.Cells[i, 9].Text = nJAmt.ToString("##,###,###,###,##0");
                ssView2_Sheet1.Cells[i, 10].Text = nBamt.ToString("##,###,###,###,##0");
                ssView2_Sheet1.Cells[i, 11].Text = (nEdiJAmt - nJAmt).ToString("##,###,###,###,##0");

                ssView2_Sheet1.RowCount = i + 1;

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

                ssView_Sheet1.Visible = false;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnChPrint_Click(object sender, EventArgs e)
        {

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "월별 외래차액 점검표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작 업 월: " + cboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                                          
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = ""; 
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            if(chkOpt0.Checked == true)
            {
                ssPrint_Sheet1.Cells[4, 1].Text = ssView_Sheet1.Cells[0, 1].Text;
                ssPrint_Sheet1.Cells[4, 2].Text = ssView_Sheet1.Cells[0, 2].Text;
                ssPrint_Sheet1.Cells[4, 3].Text = ssView_Sheet1.Cells[0, 3].Text;
                ssPrint_Sheet1.Cells[4, 5].Text = ssView_Sheet1.Cells[0, 4].Text;
                ssPrint_Sheet1.Cells[4, 7].Text = ssView_Sheet1.Cells[0, 5].Text;
                ssPrint_Sheet1.Cells[4, 9].Text = ssView_Sheet1.Cells[0, 7].Text;
            }

            if(chkOpt1.Checked == true)
            {
                ssPrint_Sheet1.Cells[5, 1].Text = ssView_Sheet1.Cells[1, 1].Text;
                ssPrint_Sheet1.Cells[5, 2].Text = ssView_Sheet1.Cells[1, 2].Text;
                ssPrint_Sheet1.Cells[5, 3].Text = ssView_Sheet1.Cells[1, 3].Text;
                ssPrint_Sheet1.Cells[5, 5].Text = ssView_Sheet1.Cells[1, 4].Text;
                ssPrint_Sheet1.Cells[5, 7].Text = ssView_Sheet1.Cells[1, 5].Text;
                ssPrint_Sheet1.Cells[5, 9].Text = ssView_Sheet1.Cells[1, 7].Text;
            }

            if(chkOpt0.Checked ==true || chkOpt1.Checked == true)
            {
                ssPrint_Sheet1.Cells[6, 1].Text = ssView_Sheet1.Cells[2, 1].Text;
                ssPrint_Sheet1.Cells[6, 2].Text = ssView_Sheet1.Cells[2, 2].Text;
                ssPrint_Sheet1.Cells[6, 3].Text = ssView_Sheet1.Cells[2, 3].Text;
                ssPrint_Sheet1.Cells[6, 5].Text = ssView_Sheet1.Cells[2, 4].Text;
                ssPrint_Sheet1.Cells[6, 7].Text = ssView_Sheet1.Cells[2, 5].Text;
                ssPrint_Sheet1.Cells[6, 9].Text = ssView_Sheet1.Cells[2, 7].Text;
            }

            if(chkOpt3.Checked == true)
            {
                ssPrint_Sheet1.Cells[7, 1].Text = ssView_Sheet1.Cells[3, 1].Text;
                ssPrint_Sheet1.Cells[7, 2].Text = ssView_Sheet1.Cells[3, 2].Text;
                ssPrint_Sheet1.Cells[7, 3].Text = ssView_Sheet1.Cells[3, 3].Text;
                ssPrint_Sheet1.Cells[7, 5].Text = ssView_Sheet1.Cells[3, 4].Text;
                ssPrint_Sheet1.Cells[7, 7].Text = ssView_Sheet1.Cells[3, 5].Text;
                ssPrint_Sheet1.Cells[7, 9].Text = ssView_Sheet1.Cells[3, 7].Text;
            }

            if (chkOpt2.Checked == true)
            {
                ssPrint_Sheet1.Cells[8, 1].Text = ssView_Sheet1.Cells[4, 1].Text;
                ssPrint_Sheet1.Cells[8, 2].Text = ssView_Sheet1.Cells[4, 2].Text;
                ssPrint_Sheet1.Cells[8, 3].Text = ssView_Sheet1.Cells[4, 3].Text;
                ssPrint_Sheet1.Cells[8, 5].Text = ssView_Sheet1.Cells[4, 4].Text;
                ssPrint_Sheet1.Cells[8, 7].Text = ssView_Sheet1.Cells[4, 5].Text;
                ssPrint_Sheet1.Cells[8, 9].Text = ssView_Sheet1.Cells[4, 7].Text;
            }

            if(chkOpt2.Checked == true || chkOpt3.Checked == true)
            {
                ssPrint_Sheet1.Cells[9, 1].Text = ssView_Sheet1.Cells[5, 1].Text;
                ssPrint_Sheet1.Cells[9, 2].Text = ssView_Sheet1.Cells[5, 2].Text;
                ssPrint_Sheet1.Cells[9, 3].Text = ssView_Sheet1.Cells[5, 3].Text;
                ssPrint_Sheet1.Cells[9, 5].Text = ssView_Sheet1.Cells[5, 4].Text;
                ssPrint_Sheet1.Cells[9, 7].Text = ssView_Sheet1.Cells[5, 5].Text;
                ssPrint_Sheet1.Cells[9, 9].Text = ssView_Sheet1.Cells[5, 7].Text;
            }

            ssPrint_Sheet1.Cells[10, 1].Text = ssView_Sheet1.Cells[6, 1].Text;
            ssPrint_Sheet1.Cells[10, 2].Text = ssView_Sheet1.Cells[6, 2].Text;
            ssPrint_Sheet1.Cells[10, 3].Text = ssView_Sheet1.Cells[6, 3].Text;
            ssPrint_Sheet1.Cells[10, 5].Text = ssView_Sheet1.Cells[6, 4].Text;
            ssPrint_Sheet1.Cells[10, 7].Text = ssView_Sheet1.Cells[6, 5].Text;
            ssPrint_Sheet1.Cells[10, 9].Text = ssView_Sheet1.Cells[6, 7].Text;
            ssPrint_Sheet1.Cells[1, 0].Text = "출력일자 :" + VB.Now().ToString();

            strTitle = "[" + cboYYMM.Text + "~" + cboYYMM2.Text + "]"+ "월별 외래청구액 점검표";
            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 18, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += "\n\n\n";
            //strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 11, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            //strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 11), clsSpread.enmSpdHAlign.Right, false, true);
            //strHeader += CS.setSpdPrint_String("작 업 월: " + cboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            //strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter = "";
            

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, false, false, false, false,(float)1.1f);

            if(chkGel.Checked == true)
            {
                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else
            {
                CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            string SQL = "";
            string strYYMM = "";

            for (i = 0; i <= 7; i++)
            {
                for (j = 1; j <= 6; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strTYYMM = VB.Left(cboYYMM2.Text, 4) + VB.Mid(cboYYMM2.Text, 7, 2);

            strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";

            strTdate = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(strTYYMM, 4) + "-" + VB.Right(strTYYMM, 2) + "-01");

            GstrYYMM = strFYYMM;
            GstrMenu = "4";
            GstrSMenu = "2";

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            try
            {

                CmdView_Slip_ADD();
                CmdView_Em6TimveOver_ADD();
                CmdView_Misu_ADD();
                CmdView_EdiJepsu_ADD();

                for (i = 1; i <= 7; i++)
                {
                    //'조합미수합계 = 당월조합미수 - 응급6시간이상
                    FnAmt[i, 3] = FnAmt[i, 1] - FnAmt[i, 2];
                    // '청구차액 = (응급6시간 + 당월분청구) - 당월조합미수
                    FnAmt[i, 5] = FnAmt[i, 4] - FnAmt[i, 3];

                    if (FnAmt[i, 5] != 0)
                    {
                        btnChAmt.Enabled = true;
                        btnChPrint.Enabled = true;

                        if (Flag == "1")
                        {
                            this.Size = new Size(1116, 750);
                        }


                    }

                    for (j = 1; j <= 6; j++)
                    {
                        ssView_Sheet1.Cells[i - 1, j].Text = FnAmt[i, j].ToString("###,###,###,##0 ");
                    }
                }

                ssView_Sheet1.Cells[0, 7].Text = " ";
                btnRemark.Enabled = true;

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void CmdView_Slip_ADD()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            int j = 0;

            Cursor.Current = Cursors.WaitCursor;

            strBiGbn = "''";
            if (chkOpt0.Checked == true)
            {
                strBiGbn = strBiGbn + ",'1','5'";
            }
            if (chkOpt1.Checked == true)
            {
                strBiGbn = strBiGbn + ",'2'";
            }
            if (chkOpt3.Checked == true)
            {
                strBiGbn = strBiGbn + ",'3'";
            }
            if (chkOpt2.Checked == true)
            {
                strBiGbn = strBiGbn + ",'4'";
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT BiGbn,SUM(Amt33) Amt ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "  AND  ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND IpdOpd='O' ";

            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + " AND BIGBN IN (" + strBiGbn + " ) ";
            }


            SQL = SQL + ComNum.VBLF + "GROUP BY BiGbn ";


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
                //ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;

            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nBiNo = (int)VB.Val(dt.Rows[i]["BiGbn"].ToString().Trim());
                nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                if (nAmt != 0)
                {
                    switch (nBiNo)
                    {
                        case 1:
                            j = 1;  //보험
                            break;
                        case 2:
                            j = 2;  //보호
                            break;
                        case 3:
                            j = 4;  //산재
                            break;
                        case 4:
                            j = 5;  //자보
                            break;
                        default:
                            j = 1;  //기타는 보험으로
                            break;
                    }

                    FnAmt[j, 1] = FnAmt[j, 1] + nAmt;
                    if (j <= 2)
                        FnAmt[3, 1] = FnAmt[3, 1] + nAmt;
                    else
                        FnAmt[6, 1] = FnAmt[6, 1] + nAmt;
                    FnAmt[7, 1] = FnAmt[7, 1] + nAmt;
                }
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void CmdView_Em6TimveOver_ADD()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            int j = 0;

            Cursor.Current = Cursors.WaitCursor;

            strBiGbn = "''";
            if (chkOpt0.Checked == true)
            {
                strBiGbn = strBiGbn + ",'1','5'";
            }
            if (chkOpt1.Checked == true)
            {
                strBiGbn = strBiGbn + ",'2'";
            }
            if (chkOpt3.Checked == true)
            {
                strBiGbn = strBiGbn + ",'3'";
            }
            if (chkOpt2.Checked == true)
            {
                strBiGbn = strBiGbn + ",'4'";
            }

            //'응급실6시간이상,NP낮병동 조합부담액 ADD
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT SuBi,SUM(Johap) Amt ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
            SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
            SQL = SQL + ComNum.VBLF + "   AND ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "   AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "   AND Gubun='3'";  //응급6시간초과
            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + "   AND SUBI IN (" + strBiGbn + " ) ";
            }
            SQL = SQL + ComNum.VBLF + " GROUP BY SuBi ";

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
                //ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;

            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nBiNo = (int)VB.Val(dt.Rows[i]["SuBi"].ToString().Trim());
                nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                if (nAmt != 0)
                {
                    switch (nBiNo)
                    {
                        case 1:
                            j = 1;  //보험
                            break;
                        case 2:
                            j = 2;  //보호
                            break;
                        case 3:
                            j = 4;  //산재
                            break;
                        case 4:
                            j = 5;  //자보
                            break;
                        default:
                            j = 1;  //기타는 보험으로
                            break;
                    }

                    FnAmt[j, 2] = FnAmt[j, 2] + nAmt;
                    if (j <= 2)
                        FnAmt[3, 2] = FnAmt[3, 2] + nAmt;
                    else
                        FnAmt[6, 2] = FnAmt[6, 2] + nAmt;
                    FnAmt[7, 2] = FnAmt[7, 2] + nAmt;
                }
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void CmdView_Misu_ADD()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            int j = 0;

            Cursor.Current = Cursors.WaitCursor;

            strBiGbn = "''";
            if (chkOpt0.Checked == true)
            {
                strBiGbn = strBiGbn + ",'01','02','03'";
            }
            if (chkOpt1.Checked == true)
            {
                strBiGbn = strBiGbn + ",'04'";
            }
            if (chkOpt3.Checked == true)
            {
                strBiGbn = strBiGbn + ",'05'";
            }
            if (chkOpt2.Checked == true)
            {
                strBiGbn = strBiGbn + ",'07'";
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT Class,SUM(Amt2) Amt ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
            SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
            SQL = SQL + ComNum.VBLF + "   AND MirYYMM >= '" + strFYYMM + "' ";
            SQL = SQL + ComNum.VBLF + "   AND MirYYMM <= '" + strTYYMM + "' ";
            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + " AND CLASS IN (" + strBiGbn + " ) ";
            }
            SQL = SQL + ComNum.VBLF + "   AND TongGbn IN ('1','2') ";
            SQL = SQL + ComNum.VBLF + "   AND IpdOpd='O' ";
            SQL = SQL + ComNum.VBLF + "GROUP BY Class ";

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
                //ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;

            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strBi = dt.Rows[i]["Class"].ToString().Trim();
                nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                if (nAmt != 0)
                {
                    switch (strBi)
                    {
                        case "01":
                        case "02":
                        case "03":
                            j = 1;  //보험
                            break;
                        case "04":
                            j = 2;  //보호
                            break;
                        case "05":
                            j = 4;  //산재
                            break;
                        case "07":
                            j = 5;  //자보
                            break;
                        default:
                            j = 1;  //기타는 보험으로
                            break;
                    }

                    FnAmt[j, 4] = FnAmt[j, 4] + nAmt;
                    if (j <= 2)
                        FnAmt[3, 4] = FnAmt[3, 4] + nAmt;
                    else
                        FnAmt[6, 4] = FnAmt[6, 4] + nAmt;
                    FnAmt[7, 4] = FnAmt[7, 4] + nAmt;
                }
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void CmdView_EdiJepsu_ADD()
        {
            DataTable dt = null;
            DataTable dtAdores = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            int j = 0;

            Cursor.Current = Cursors.WaitCursor;

            strBiGbn = "''";
            if (chkOpt0.Checked == true)
            {
                strBiGbn = strBiGbn + ",'1','2'";
            }
            if (chkOpt1.Checked == true)
            {
                strBiGbn = strBiGbn + ",'5'";
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT Johap,SUM(SamJAmt+SamJangAmt+SamDaebul) Amt ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "EDI_JEPSU ";
            SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
            SQL = SQL + ComNum.VBLF + "   AND YYMM >= '" + strFYYMM + "' ";// '진료월
            SQL = SQL + ComNum.VBLF + "   AND YYMM <= '" + strTYYMM + "' ";// '진료월
            SQL = SQL + ComNum.VBLF + "   AND IpdOpd='O' ";
            SQL = SQL + ComNum.VBLF + "   AND BanDate IS NULL ";//  '반송분은 제외
            SQL = SQL + ComNum.VBLF + "   AND MirGbn IN ('4','0') ";     //'추가청구,재청구는 제외

            if (strBiGbn != "")
            {
                SQL = SQL + ComNum.VBLF + "   AND JOHAP IN (" + strBiGbn + " ) ";
            }
            SQL = SQL + ComNum.VBLF + "GROUP BY Johap ";

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
                //ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;

            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strBi = dt.Rows[i]["Johap"].ToString().Trim();
                nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                if (nAmt != 0)
                {
                    switch (strBi)
                    {
                        case "1":
                        case "2":
                            j = 1;  //보험
                            break;
                        case "5":
                            j = 2;  //자보
                            break;
                        default:
                            j = 1;  //기타는 보험으로
                            break;
                    }

                    FnAmt[j, 6] = FnAmt[j, 6] + nAmt;
                    if (j <= 2)
                        FnAmt[3, 6] = FnAmt[3, 6] + nAmt;
                    else
                        FnAmt[6, 6] = FnAmt[6, 6] + nAmt;
                    FnAmt[7, 6] = FnAmt[7, 6] + nAmt;
                }
            }

            dt.Dispose();
            dt = null;

            // ' 산재 당월 진료분 EDI접수액 합산(재청구,추가청구 제외)
            if (chkOpt2.Checked == true)
            {
                SQL = "";
                SQL = "SELECT SUM(JepAmt) Amt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_SANJEPSU ";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1"; //'진료월
                SQL = SQL + ComNum.VBLF + "  AND YYMM >= '" + strFYYMM + "' "; //'진료월
                SQL = SQL + ComNum.VBLF + "  AND YYMM <= '" + strTYYMM + "' ";// '진료월
                SQL = SQL + ComNum.VBLF + "  AND IpdOpd='O' ";
                SQL = SQL + ComNum.VBLF + "  AND BanDate IS NULL ";// '반송분은 제외

                SqlErr = clsDB.GetDataTable(ref dtAdores, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (i = 0; i < dtAdores.Rows.Count; i++)
                {
                    if(dtAdores.Rows[i]["Amt"].ToString().Trim() == "")
                    {
                        nAmt = 0;
                    }
                    else
                    {
                        nAmt = Convert.ToDouble(dtAdores.Rows[i]["Amt"].ToString().Trim());
                    }
                    

                    if (nAmt != 0)
                    {
                        j = 4;
                        FnAmt[j, 6] = FnAmt[j, 6] + nAmt;
                        if (j <= 2)
                        {
                            FnAmt[3, 6] = FnAmt[3, 6] + nAmt;
                        }
                        else
                        {
                            FnAmt[6, 6] = FnAmt[6, 6] + nAmt;
                        }

                        FnAmt[7, 6] = FnAmt[7, 6] + nAmt;
                    }
                }
                dtAdores.Dispose();
                dtAdores = null;
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            frmPmpaViewBalRemark frm = new frmPmpaViewBalRemark();
            frm.ShowDialog();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            pnaHelp.Visible = true;
        }

        private void btnHelpClsoe_Click(object sender, EventArgs e)
        {
            pnaHelp.Visible = false;
        }

        private void btnSSfpSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            string strYYMM = "";
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTdate = "";
            int nRow = 0;
            double nSumTot1 = 0;
            double nSumTot2 = 0; //'퇴원합
            double nSumTot3 = 0;
            double nSumTot4 = 0; //'중간합
            //double nSumTot5 = 0; //'응급
            //double nSumTot6 = 0;
            //double nSumTot7 = 0;
            //double nSumTot8 = 0;
            //double nSubSum1 = 0;
            double nTempMisuAmt = 0;
            double nTempCnt = 0;
            double nTempAmt = 0;
            double nTempAmt2 = 0;
            double nTempAmt3 = 0;
            //double nTempAmt4 = 0;
            double nTempAmt5 = 0;
            //double nTempAmt6 = 0;
            //double nTempAmt7 = 0;
            //double nTempAmt8 = 0;
            string strBiGbn = "";
            double[,] nSumDept = new double[51, 201];
            double[,] nSumCnt = new double[51, 201];
            string strDeptRow = "";
            string strDept = "";
            int nDept = 0;
            string strOK = "";
            string strJungFDate = "";
            string strJungTDate = "";
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {

                SSView5_Sheet1.RowCount = 0;
                //SSView5_Sheet1.Cells [0 , SSView5_Sheet1.RowCount - 1 , 0 , SSView5_Sheet1.ColumnCount - 1].Text = "";

                for (i = 1; i <= 7; i++)
                {
                    for (j = 1; j <= 6; j++)
                    {
                        FnAmt[i, j] = 0;
                    }
                }

                strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

                strFYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
                strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";

                strTYYMM = VB.Left(cboYYMM2.Text, 4) + VB.Mid(cboYYMM2.Text, 7, 2);
                strTdate = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(strTYYMM, 4) + "-" + VB.Right(strTYYMM, 2) + "-01");

                // 'jjy(2003-01-13) '통계 remark 등록 공용변수
                GstrYYMM = strFYYMM;
                GstrMenu = "4";
                GstrSMenu = "1";

                strJungFDate = Convert.ToDateTime(CF.READ_LASTDAY(clsDB.DbCon, strFDate)).AddDays(1).ToString("yyyy-MM-dd");
                strJungTDate = Convert.ToDateTime(CF.READ_LASTDAY(clsDB.DbCon, strTdate)).AddDays(1).ToString("yyyy-MM-dd");
                strJungTDate = Convert.ToDateTime(CF.READ_LASTDAY(clsDB.DbCon, strJungTDate)).AddDays(1).ToString("yyyy-MM-dd");

                nSumTot1 = 0;
                nSumTot2 = 0;
                nSumTot3 = 0;
                nSumTot4 = 0;
                //nSumTot5 = 0;
                //nSumTot6 = 0;
                //nSumTot7 = 0;
                //nSumTot8 = 0;
                nTempAmt = 0;
                nTempMisuAmt = 0;
                //nSubSum1 = 0;

                for (i = 0; i <= 50; i++)
                {
                    for (j = 0; j <= 200; j++)
                    {
                        nSumDept[i, j] = 0;
                        nSumCnt[i, j] = 0;
                    }
                }

                //'과배열 세팅
                SQL = SQL + ComNum.VBLF + " SELECT DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL = SQL + ComNum.VBLF + "   WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "      AND DEPTCODE  NOT IN ('II','PT','AN','HC','OM','LM')";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                strDeptRow = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strDeptRow = strDeptRow + dt.Rows[i]["DeptCode"].ToString().Trim() + "," + (i + 1) + ";";
                }

                dt.Dispose();
                dt = null;

                strDept = "";
                nDept = 0;
                nTempMisuAmt = 0;
                nTempCnt = 0;

                strBiGbn = "''";
                if (chkOpt0.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'01','02','03'";
                }
                if (chkOpt1.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'04'";
                }
                if (chkOpt3.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'05'";
                }
                if (chkOpt2.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'07'";
                }

                //'미수발생
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT class,ipdopd,deptcode,edimirno,sum(amt2) misuamt,sum(qty1) cnt1  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "  AND MirYYMM >= '" + strFYYMM + "'   ";
                SQL = SQL + ComNum.VBLF + "  AND MirYYMM <= '" + strTYYMM + "' ";

                if (strBiGbn != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND CLASS IN (" + strBiGbn + " ) ";
                }

                SQL = SQL + ComNum.VBLF + "   AND TONGGBN IN ('1','2') ";
                SQL = SQL + ComNum.VBLF + "  AND IPDOPD='O'";
                SQL = SQL + ComNum.VBLF + "gROUP BY CLASS,IPDOPD,DEPTCODE,EDIMIRNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["class"].ToString().Trim())
                        {
                            case "07":
                                strDept = "";
                                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nDept = 0;
                                nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                if (strDept != "" && nDept > 0)
                                {
                                    nSumDept[nDept, 5] = nSumDept[nDept, 5] + Convert.ToDouble(dt.Rows[i]["misuamt"].ToString().Trim());
                                    nSumCnt[nDept, 5] = nSumCnt[nDept, 5] + Convert.ToDouble(dt.Rows[i]["cnt1"].ToString().Trim());
                                    nTempCnt = nTempCnt + Convert.ToDouble(dt.Rows[i]["cnt1"].ToString().Trim());

                                    nTempMisuAmt = nTempMisuAmt + Convert.ToDouble(dt.Rows[i]["misuamt"].ToString().Trim());
                                }
                                break;

                            case "05":
                                strDept = "";
                                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nDept = 0;
                                nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                if (strDept != "" && nDept > 0)
                                {
                                    nSumDept[nDept, 3] = nSumDept[nDept, 3] + Convert.ToDouble(dt.Rows[i]["misuamt"].ToString().Trim());
                                    nSumCnt[nDept, 3] = nSumCnt[nDept, 3] + Convert.ToDouble(dt.Rows[i]["cnt1"].ToString().Trim());
                                    nTempCnt = nTempCnt + Convert.ToDouble(dt.Rows[i]["cnt1"].ToString().Trim());

                                    nTempMisuAmt = nTempMisuAmt + Convert.ToDouble(dt.Rows[i]["misuamt"].ToString().Trim());
                                }
                                break;

                            case "04":
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + " select decode(DeptCode1,'RD','R6',deptcode1) deptcode1,sum(edijamt + nvl(ediuamt100,0) + nvl(edigamt,0) + ediboamt) jamt,count(wrtno) cnt1 ";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID";
                                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                                SQL = SQL + ComNum.VBLF + "  AND EDIMIRNO= '" + dt.Rows[i]["EDIMIRNO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "  AND IPDOPD ='O' ";
                                SQL = SQL + ComNum.VBLF + "  AND JOHAP ='5' ";
                                SQL = SQL + ComNum.VBLF + "  GROUP BY DECODE(DEPTCODE1,'RD','R6',DEPTCODE1) ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    btnSearch.Enabled = true;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt2.Rows.Count == 0)
                                {

                                    dt.Dispose();
                                    dt = null;
                                    btnSearch.Enabled = true;
                                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if (dt2.Rows.Count > 0)
                                {
                                    for (j = 0; j < dt2.Rows.Count; j++)
                                    {
                                        strDept = "";
                                        strDept = dt2.Rows[j]["DeptCode1"].ToString().Trim();
                                        nDept = 0;
                                        nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                        if (strDept != "" && nDept > 0)
                                        {
                                            nSumDept[nDept, 2] = nSumDept[nDept, 2] + Convert.ToDouble(dt2.Rows[j]["Jamt"].ToString().Trim());
                                            nSumCnt[nDept, 2] = nSumCnt[nDept, 2] + Convert.ToDouble(dt2.Rows[j]["cnt1"].ToString().Trim());
                                            nTempCnt = nTempCnt + Convert.ToDouble(dt2.Rows[j]["cnt1"].ToString().Trim());
                                            nTempMisuAmt = nTempMisuAmt + Convert.ToDouble(dt2.Rows[j]["Jamt"].ToString().Trim());
                                        }
                                    }
                                }
                                break;

                            case "01":
                            case "02":
                            case "03":
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + " select decode(DeptCode1,'RD','R6',deptcode1) deptcode1,sum(edijamt + nvl(ediuamt100,0) + nvl(edigamt,0) + ediboamt) jamt,count(wrtno) cnt1 ";
                                SQL = SQL + ComNum.VBLF + "from " + ComNum.DB_PMPA + "mir_insid";
                                SQL = SQL + ComNum.VBLF + " where edimirno= '" + dt.Rows[i]["edimirno"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "  and ipdopd ='O' ";
                                SQL = SQL + ComNum.VBLF + "  and johap <>'5' ";
                                SQL = SQL + ComNum.VBLF + "  group by decode(DeptCode1,'RD','R6',deptcode1)  ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    btnSearch.Enabled = true;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt2.Rows.Count == 0)
                                {

                                    dt2.Dispose();
                                    dt2 = null;
                                    btnSearch.Enabled = true;
                                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if (dt2.Rows.Count > 0)
                                {
                                    for (j = 0; j < dt2.Rows.Count; j++)
                                    {
                                        strDept = "";
                                        strDept = dt2.Rows[j]["DeptCode1"].ToString().Trim();
                                        nDept = 0;
                                        nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));



                                        if (strDept != "" && nDept > 0)
                                        {
                                            nSumDept[nDept, 1] = nSumDept[nDept, 1] + Convert.ToDouble(dt2.Rows[j]["Jamt"].ToString().Trim());
                                            nSumCnt[nDept, 1] = nSumCnt[nDept, 1] + Convert.ToDouble(dt2.Rows[j]["cnt1"].ToString().Trim());
                                            nTempCnt = nTempCnt + Convert.ToDouble(dt2.Rows[j]["cnt1"].ToString().Trim());
                                            nTempMisuAmt = nTempMisuAmt + Convert.ToDouble(dt2.Rows[j]["Jamt"].ToString().Trim());
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                ///발생금액
                SQL = "";
                SQL = SQL + ComNum.VBLF + " select DeptCode From bas_clinicdept";
                SQL = SQL + ComNum.VBLF + "   where deptcode  not in ('II','PT','AN','RD','HC','OM','LM')";
                SQL = SQL + ComNum.VBLF + " order by printranking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SSView5_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        for (j = 1; j <= 5; j++)
                        {
                            strOK = "";

                            if (chkOpt0.Checked == true && j == 1)
                            {
                                strBiGbn = strOK = "OK";
                            }
                            if (chkOpt1.Checked == true && j == 2)
                            {
                                strBiGbn = strOK = "OK";
                            }
                            if (chkOpt3.Checked == true && j == 3)
                            {
                                strBiGbn = strOK = "OK";
                            }
                            if (chkOpt2.Checked == true && j == 5)
                            {
                                strBiGbn = strOK = "OK";
                            }


                            if (j == 4)
                            {
                                strOK = "";
                            }

                            if (strOK == "OK")
                            {
                                nRow = nRow + 1;
                                SSView5_Sheet1.RowCount = nRow;

                                switch (j)
                                {
                                    case 1:
                                        SSView5_Sheet1.Cells[nRow - 1, 1].Text = "건보";
                                        break;
                                    case 2:
                                        SSView5_Sheet1.Cells[nRow - 1, 1].Text = "의급";
                                        break;
                                    case 3:
                                        SSView5_Sheet1.Cells[nRow - 1, 1].Text = "산재";
                                        break;
                                    case 5:
                                        SSView5_Sheet1.Cells[nRow - 1, 1].Text = "자보";
                                        break;
                                }

                                SSView5_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nTempAmt = 0;
                                nTempAmt2 = 0;
                                nTempAmt3 = 0;
                                //nTempAmt4 = 0;
                                nTempAmt5 = 0;
                                //nTempAmt6 = 0;
                                //nTempAmt7 = 0;
                                //nTempAmt8 = 0;

                                //'발생
                                if (Check1.Checked == false)
                                {
                                    SQL = "";
                                    SQL = SQL + ComNum.VBLF + " select substr(trim(bi),1,1) bi,deptcode,SUM(Amt1) TotAmt , SUM(Amt2) Amt2, ";
                                    SQL = SQL + ComNum.VBLF + " SUM( DECODE(RTRIM(SUNEXT) ,'Y98D', AMT1 + AMT2 , 0 )) AMT3,";
                                    SQL = SQL + ComNum.VBLF + " SUM( DECODE(RTRIM(SUNEXT) ,'DRG001', AMT1 ,'DRG002', AMT1, 0 )) AMT5 ,";
                                    SQL = SQL + ComNum.VBLF + " SUM( DECODE(RTRIM(SUNEXT) ,'Y97', AMT4, 0 )) AMT6,";
                                    SQL = SQL + ComNum.VBLF + " SUM( DECODE(RTRIM(SUNEXT) ,'Y97', DanAmt, 0 )) Damt";
                                    SQL = SQL + ComNum.VBLF + " From " + ComNum.DB_PMPA + "OPD_SLIP ";
                                    SQL = SQL + ComNum.VBLF + " where actdate >=to_date('" + strFDate + "','yyyy-mm-dd')";
                                    SQL = SQL + ComNum.VBLF + " and actdate <=to_date('" + strTdate + "','yyyy-mm-dd')";
                                    SQL = SQL + ComNum.VBLF + " and bun ='98'   ";
                                    SQL = SQL + ComNum.VBLF + "   and substr(trim(bi),1,1) ='" + j + "' ";
                                    SQL = SQL + ComNum.VBLF + "   and deptcode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + " group by substr(trim(bi),1,1),deptcode ";

                                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        btnSearch.Enabled = true;
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }

                                    if (dt2.Rows.Count > 0)
                                    {
                                        SSView5_Sheet1.Cells[nRow - 1, 3].Text = Convert.ToDouble(dt2.Rows[0]["TotAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                                        nTempAmt = Convert.ToDouble(dt2.Rows[0]["TotAmt"].ToString().Trim());
                                        nSumTot1 = nSumTot1 + Convert.ToDouble(dt2.Rows[0]["TotAmt"].ToString().Trim());
                                    }
                                    if (dt2 == null)
                                    {
                                        dt2.Dispose();
                                        dt2 = null;
                                    }
                                }
                                //'응급실 6시간
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + " select substr(trim(bi),1,1) bi,deptcode,SUM(TotAmt) TotAmt,SUM(Junggan) Junggan,";
                                SQL = SQL + ComNum.VBLF + " SUM(Johap) Johap,SUM(Halin) Halin,SUM(Bojung) Bojung,";
                                SQL = SQL + ComNum.VBLF + " SUM(EtcMisu) EtcMisu,SUM(SuNap) SuNap,SUM(Dansu) Dansu, SUM(DRUGAMT) DRUGAMT ,SUM(Johap+junggan) totamt,SUM(JepJAmt) JepJAmt ";
                                SQL = SQL + ComNum.VBLF + " From MISU_BALTEWON";
                                SQL = SQL + ComNum.VBLF + " where actdate >=to_date('" + strFDate + "','yyyy-mm-dd')";
                                SQL = SQL + ComNum.VBLF + " and actdate <=to_date('" + strTdate + "','yyyy-mm-dd')";
                                SQL = SQL + ComNum.VBLF + " and gubun in ('3')  ";
                                SQL = SQL + ComNum.VBLF + "   and substr(trim(bi),1,1) ='" + j + "' ";
                                SQL = SQL + ComNum.VBLF + "   and deptcode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + " group by substr(trim(bi),1,1),deptcode ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    btnSearch.Enabled = true;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }


                                if (dt2.Rows.Count > 0)
                                {
                                    nTempAmt2 = Convert.ToDouble(dt2.Rows[0]["Johap"].ToString().Trim());
                                    nSumTot2 = nSumTot2 + Convert.ToDouble(dt2.Rows[0]["Johap"].ToString().Trim());
                                    SSView5_Sheet1.Cells[nRow - 1, 4].Text = Convert.ToDouble(dt2.Rows[0]["Johap"].ToString().Trim()).ToString("###,###,###,##0 ");
                                }
                                if (dt2 == null)
                                {
                                    dt2.Dispose();
                                    dt2 = null;
                                }

                                nTempAmt3 = nTempAmt - nTempAmt2;
                                nSumTot3 = nSumTot3 + nTempAmt3;

                                SSView5_Sheet1.Cells[nRow - 1, 5].Text = (nTempAmt3).ToString("###,###,###,##0 ");//  '외래조합-낮병동


                                strDept = "";
                                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nDept = 0;
                                nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                if (strDept == "R6")
                                {
                                    i = i;
                                }

                                if (nDept > 0)
                                {
                                    SSView5_Sheet1.Cells[nRow - 1, 6].Text = nSumDept[nDept, j].ToString("###,###,###,##0 ");
                                    nTempAmt5 = nSumDept[nDept, j] - nTempAmt3;
                                    nSumTot4 = nSumTot4 + nTempAmt5;
                                    SSView5_Sheet1.Cells[nRow - 1, 7].Text = nTempAmt5.ToString("###,###,###,##0 ");
                                    SSView5_Sheet1.Cells[nRow - 1, 8].Text = nSumCnt[nDept, j].ToString("###,###,###,##0 ");
                                }
                                else
                                {
                                    nTempAmt5 = nSumDept[nDept, j] - nTempAmt3;
                                    nSumTot4 = nSumTot4 + nTempAmt5;
                                    SSView5_Sheet1.Cells[nRow - 1, 7].Text = nTempAmt5.ToString("###,###,###,##0 ");
                                }
                                Application.DoEvents();
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                SSView5_Sheet1.RowCount = nRow + 1;
                SSView5_Sheet1.Cells[nRow, 3].Text = nSumTot1.ToString("###,###,###,##0 ");
                SSView5_Sheet1.Cells[nRow, 4].Text = nSumTot2.ToString("###,###,###,##0 ");
                SSView5_Sheet1.Cells[nRow, 5].Text = nSumTot3.ToString("###,###,###,##0 ");
                SSView5_Sheet1.Cells[nRow, 6].Text = nTempMisuAmt.ToString("###,###,###,##0 ");
                SSView5_Sheet1.Cells[nRow, 7].Text = nSumTot4.ToString("###,###,###,##0 ");
                SSView5_Sheet1.Cells[nRow, 8].Text = nTempCnt.ToString("###,###,###,##0 ");

                Cursor.Current = Cursors.Default;
            }

            catch (Exception ex)
            {


                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExell_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            int mm = 0;
            string strYYMM = "";
            string strFYYMM = "";
            string strTYYMM = "";
            string strFDate = "";
            string strTdate = "";
            int nRow = 0;
            double nSumTot1 = 0;
            double nSumTot2 = 0; //'퇴원합
            double nSumTot3 = 0;
            double nSumTot4 = 0; //'중간합
            //double nSumTot5 = 0; //'응급
            //double nSumTot6 = 0;
            //double nSumTot7 = 0;
            //double nSumTot8 = 0;
            //double nSubSum1 = 0;
            double nTempMisuAmt = 0;
            double nTempCnt = 0;
            double nTempAmt = 0;
            double nTempAmt2 = 0;
            double nTempAmt3 = 0;
            //double nTempAmt4 = 0;
            double nTempAmt5 = 0;
            //double nTempAmt6 = 0;
            //double nTempAmt7 = 0;
            //double nTempAmt8 = 0;
            string strBiGbn = "";
            double[,] nSumDept = new double[51, 201];
            double[,] nSumCnt = new double[51, 201];
            string strDeptRow = "";
            string strDept = "";
            int nDept = 0;
            string strOK = "";
            string strJungFDate = "";
            string strJungTDate = "";
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            bool x = false;

            Cursor.Current = Cursors.WaitCursor;


            try
            {




                switch (mm)
                {
                    case 1:
                        cboYYMM.Text = "2013년 01월분";
                        cboYYMM2.Text = "2013년 01월분";
                        break;
                    case 2:
                        cboYYMM.Text = "2013년 02월분";
                        cboYYMM2.Text = "2013년 02월분";
                        break;
                    case 3:
                        cboYYMM.Text = "2013년 03월분";
                        cboYYMM2.Text = "2013년 03월분";
                        break;
                    case 4:
                        cboYYMM.Text = "2013년 04월분";
                        cboYYMM2.Text = "2013년 04월분";
                        break;
                    case 5:
                        cboYYMM.Text = "2013년 05월분";
                        cboYYMM2.Text = "2013년 05월분";
                        break;
                    case 6:
                        cboYYMM.Text = "2013년 06월분";
                        cboYYMM2.Text = "2013년 06월분";
                        break;
                    case 7:
                        cboYYMM.Text = "2013년 07월분";
                        cboYYMM2.Text = "2013년 07월분";
                        break;
                    case 8:
                        cboYYMM.Text = "2013년 08월분";
                        cboYYMM2.Text = "2013년 08월분";
                        break;
                    case 9:
                        cboYYMM.Text = "2013년 09월분";
                        cboYYMM2.Text = "2013년 09월분";
                        break;
                    case 10:
                        cboYYMM.Text = "2013년 10월분";
                        cboYYMM2.Text = "2013년 10월분";
                        break;
                    case 11:
                        cboYYMM.Text = "2013년 11월분";
                        cboYYMM2.Text = "2013년 11월분";
                        break;
                    case 12:
                        cboYYMM.Text = "2013년 12월분";
                        cboYYMM2.Text = "2013년 12월분";
                        break;



                    case 13:
                        cboYYMM.Text = "2014년 01월분";
                        cboYYMM2.Text = "2014년 01월분";
                        break;
                    case 14:
                        cboYYMM.Text = "2014년 02월분";
                        cboYYMM2.Text = "2014년 02월분";
                        break;
                    case 15:
                        cboYYMM.Text = "2014년 03월분";
                        cboYYMM2.Text = "2014년 03월분";
                        break;
                    case 16:
                        cboYYMM.Text = "2014년 04월분";
                        cboYYMM2.Text = "2014년 04월분";
                        break;
                    case 17:
                        cboYYMM.Text = "2014년 05월분";
                        cboYYMM2.Text = "2014년 05월분";
                        break;
                    case 18:
                        cboYYMM.Text = "2014년 06월분";
                        cboYYMM2.Text = "2014년 06월분";
                        break;
                    case 19:
                        cboYYMM.Text = "2014년 07월분";
                        cboYYMM2.Text = "2014년 07월분";
                        break;
                    case 20:
                        cboYYMM.Text = "2014년 08월분";
                        cboYYMM2.Text = "2014년 08월분";
                        break;
                    case 21:
                        cboYYMM.Text = "2014년 09월분";
                        cboYYMM2.Text = "2014년 09월분";
                        break;
                    case 22:
                        cboYYMM.Text = "2014년 10월분";
                        cboYYMM2.Text = "2014년 10월분";
                        break;
                    case 23:
                        cboYYMM.Text = "2014년 11월분";
                        cboYYMM2.Text = "2014년 11월분";
                        break;
                    case 24:
                        cboYYMM.Text = "2014년 12월분";
                        cboYYMM2.Text = "2014년 12월분";
                        break;


                    case 25:
                        cboYYMM.Text = "2015년 01월분";
                        cboYYMM2.Text = "2015년 01월분";
                        break;
                    case 26:
                        cboYYMM.Text = "2015년 02월분";
                        cboYYMM2.Text = "2015년 02월분";
                        break;
                    case 27:
                        cboYYMM.Text = "2015년 03월분";
                        cboYYMM2.Text = "2015년 03월분";
                        break;
                    case 28:
                        cboYYMM.Text = "2015년 04월분";
                        cboYYMM2.Text = "2015년 04월분";
                        break;
                    case 29:
                        cboYYMM.Text = "2015년 05월분";
                        cboYYMM2.Text = "2015년 05월분";
                        break;
                    case 30:
                        cboYYMM.Text = "2015년 06월분";
                        cboYYMM2.Text = "2015년 06월분";
                        break;
                    case 31:
                        cboYYMM.Text = "2015년 07월분";
                        cboYYMM2.Text = "2015년 07월분";
                        break;
                    case 32:
                        cboYYMM.Text = "2015년 08월분";
                        cboYYMM2.Text = "2015년 08월분";
                        break;
                    case 33:
                        cboYYMM.Text = "2015년 09월분";
                        cboYYMM2.Text = "2015년 09월분";
                        break;
                    case 34:
                        cboYYMM.Text = "2015년 10월분";
                        cboYYMM2.Text = "2015년 10월분";
                        break;
                    case 35:
                        cboYYMM.Text = "2015년 11월분";
                        cboYYMM2.Text = "2015년 11월분";
                        break;
                    case 36:
                        cboYYMM.Text = "2015년 12월분";
                        cboYYMM2.Text = "2015년 12월분";
                        break;
                }

                SSView5_Sheet1.RowCount = 0;
                //SSView5_Sheet1.Cells [0 , SSView5_Sheet1.RowCount - 1 , 0 , SSView5_Sheet1.ColumnCount - 1].Text = "";

                for (i = 1; i <= 7; i++)
                {
                    for (j = 1; j <= 6; j++)
                    {
                        FnAmt[i, j] = 0;
                    }
                }

                strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);

                strFYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
                strFDate = VB.Left(strFYYMM, 4) + "-" + VB.Right(strFYYMM, 2) + "-01";

                strTYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM2.Text, 7, 2);
                strTdate = CF.READ_LASTDAY(clsDB.DbCon, VB.Left(strTYYMM, 4) + "-" + VB.Right(strTYYMM, 2) + "-01");

                // 'jjy(2003-01-13) '통계 remark 등록 공용변수
                GstrYYMM = strFYYMM;
                GstrMenu = "4";
                GstrSMenu = "1";

                strJungFDate = Convert.ToDateTime(CF.READ_LASTDAY(clsDB.DbCon, strFDate)).AddDays(1).ToString("yyyy-MM-dd");
                strJungTDate = Convert.ToDateTime(CF.READ_LASTDAY(clsDB.DbCon, strTdate)).AddDays(1).ToString("yyyy-MM-dd");
                strJungTDate = Convert.ToDateTime(CF.READ_LASTDAY(clsDB.DbCon, strJungTDate)).AddDays(1).ToString("yyyy-MM-dd");

                nSumTot1 = 0;
                nSumTot2 = 0;
                nSumTot3 = 0;
                nSumTot4 = 0;
                //nSumTot5 = 0;
                //nSumTot6 = 0;
                //nSumTot7 = 0;
                //nSumTot8 = 0;
                nTempAmt = 0;
                nTempMisuAmt = 0;
                //nSubSum1 = 0;

                for (i = 0; i <= 50; i++)
                {
                    for (j = 0; j <= 200; j++)
                    {
                        nSumDept[i, j] = 0;
                        nSumCnt[i, j] = 0;
                    }
                }

                //'과배열 세팅
                SQL = SQL + ComNum.VBLF + " SELECT DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL = SQL + ComNum.VBLF + "   WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "      AND DEPTCODE  NOT IN ('II','PT','AN','HC','OM','LM')";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                strDeptRow = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strDeptRow = strDeptRow + dt.Rows[i]["DeptCode"].ToString().Trim() + "," + (i + 1) + ";";
                }

                dt.Dispose();
                dt = null;

                strDept = "";
                nDept = 0;
                nTempMisuAmt = 0;
                nTempCnt = 0;

                strBiGbn = "''";
                if (chkOpt0.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'01','02','03'";
                }
                if (chkOpt1.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'04'";
                }
                if (chkOpt3.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'05'";
                }
                if (chkOpt2.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'07'";
                }

                //'미수발생
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT class,ipdopd,deptcode,edimirno,sum(amt2) misuamt,sum(qty1) cnt1  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "  AND MirYYMM >= '" + strFYYMM + "'   ";
                SQL = SQL + ComNum.VBLF + "  AND MirYYMM <= '" + strTYYMM + "' ";

                if (strBiGbn != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND CLASS IN (" + strBiGbn + " ) ";
                }

                SQL = SQL + ComNum.VBLF + "   AND TONGGBN IN ('1','2') ";
                SQL = SQL + ComNum.VBLF + "  AND IPDOPD='O'";
                SQL = SQL + ComNum.VBLF + "gROUP BY CLASS,IPDOPD,DEPTCODE,EDIMIRNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["class"].ToString().Trim())
                        {
                            case "07":
                                strDept = "";
                                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nDept = 0;
                                nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                if (strDept != "" && nDept > 0)
                                {
                                    nSumDept[nDept, 5] = nSumDept[nDept, 5] + Convert.ToDouble(dt.Rows[i]["misuamt"].ToString().Trim());
                                    nSumCnt[nDept, 5] = nSumCnt[nDept, 5] + Convert.ToDouble(dt.Rows[i]["cnt1"].ToString().Trim());
                                    nTempCnt = nTempCnt + Convert.ToDouble(dt.Rows[i]["cnt1"].ToString().Trim());

                                    nTempMisuAmt = nTempMisuAmt + Convert.ToDouble(dt.Rows[i]["misuamt"].ToString().Trim());
                                }
                                break;

                            case "05":
                                strDept = "";
                                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nDept = 0;
                                nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                if (strDept != "" && nDept > 0)
                                {
                                    nSumDept[nDept, 3] = nSumDept[nDept, 3] + Convert.ToDouble(dt.Rows[i]["misuamt"].ToString().Trim());
                                    nSumCnt[nDept, 3] = nSumCnt[nDept, 3] + Convert.ToDouble(dt.Rows[i]["cnt1"].ToString().Trim());
                                    nTempCnt = nTempCnt + Convert.ToDouble(dt.Rows[i]["cnt1"].ToString().Trim());

                                    nTempMisuAmt = nTempMisuAmt + Convert.ToDouble(dt.Rows[i]["misuamt"].ToString().Trim());
                                }
                                break;

                            case "04":
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + " select decode(DeptCode1,'RD','R6',deptcode1) deptcode1,sum(edijamt + nvl(ediuamt100,0) + nvl(edigamt,0) + ediboamt) jamt,count(wrtno) cnt1 ";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID";
                                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                                SQL = SQL + ComNum.VBLF + "  AND EDIMIRNO= '" + dt.Rows[i]["EDIMIRNO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "  AND IPDOPD ='O' ";
                                SQL = SQL + ComNum.VBLF + "  AND JOHAP ='5' ";
                                SQL = SQL + ComNum.VBLF + "  GROUP BY DECODE(DEPTCODE1,'RD','R6',DEPTCODE1) ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    btnSearch.Enabled = true;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt2.Rows.Count == 0)
                                {

                                    dt.Dispose();
                                    dt = null;
                                    btnSearch.Enabled = true;
                                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if (dt2.Rows.Count > 0)
                                {
                                    for (j = 0; j < dt2.Rows.Count; j++)
                                    {
                                        strDept = "";
                                        strDept = dt2.Rows[j]["DeptCode1"].ToString().Trim();
                                        nDept = 0;
                                        nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                        if (strDept != "" && nDept > 0)
                                        {
                                            nSumDept[nDept, 2] = nSumDept[nDept, 2] + Convert.ToDouble(dt2.Rows[j]["Jamt"].ToString().Trim());
                                            nSumCnt[nDept, 2] = nSumCnt[nDept, 2] + Convert.ToDouble(dt2.Rows[j]["cnt1"].ToString().Trim());
                                            nTempCnt = nTempCnt + Convert.ToDouble(dt2.Rows[j]["cnt1"].ToString().Trim());
                                            nTempMisuAmt = nTempMisuAmt + Convert.ToDouble(dt2.Rows[j]["Jamt"].ToString().Trim());
                                        }
                                    }
                                }
                                break;

                            case "01":
                            case "02":
                            case "03":
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + " select decode(DeptCode1,'RD','R6',deptcode1) deptcode1,sum(edijamt + nvl(ediuamt100,0) + nvl(edigamt,0) + ediboamt) jamt,count(wrtno) cnt1 ";
                                SQL = SQL + ComNum.VBLF + "from " + ComNum.DB_PMPA + "mir_insid";
                                SQL = SQL + ComNum.VBLF + " where edimirno= '" + dt.Rows[i]["edimirno"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "  and ipdopd ='O' ";
                                SQL = SQL + ComNum.VBLF + "  and johap <>'5' ";
                                SQL = SQL + ComNum.VBLF + "  group by decode(DeptCode1,'RD','R6',deptcode1)  ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    btnSearch.Enabled = true;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt2.Rows.Count == 0)
                                {

                                    dt2.Dispose();
                                    dt2 = null;
                                    btnSearch.Enabled = true;
                                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if (dt2.Rows.Count > 0)
                                {
                                    for (j = 0; j < dt2.Rows.Count; j++)
                                    {
                                        strDept = "";
                                        strDept = dt2.Rows[j]["DeptCode1"].ToString().Trim();
                                        nDept = 0;
                                        nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));



                                        if (strDept != "" && nDept > 0)
                                        {
                                            nSumDept[nDept, 1] = nSumDept[nDept, 1] + Convert.ToDouble(dt2.Rows[j]["Jamt"].ToString().Trim());
                                            nSumCnt[nDept, 1] = nSumCnt[nDept, 1] + Convert.ToDouble(dt2.Rows[j]["cnt1"].ToString().Trim());
                                            nTempCnt = nTempCnt + Convert.ToDouble(dt2.Rows[j]["cnt1"].ToString().Trim());
                                            nTempMisuAmt = nTempMisuAmt + Convert.ToDouble(dt2.Rows[j]["Jamt"].ToString().Trim());
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;


                ///발생금액
                SQL = "";
                SQL = SQL + ComNum.VBLF + " select DeptCode From bas_clinicdept";
                SQL = SQL + ComNum.VBLF + "   where deptcode  not in ('II','PT','AN','RD','HC','OM','LM')";
                SQL = SQL + ComNum.VBLF + " order by printranking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SSView5_Sheet1.RowCount = 0;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        for (j = 1; j <= 5; j++)
                        {
                            strOK = "";

                            if (chkOpt0.Checked == true && j == 1)
                            {
                                strBiGbn = strOK = "OK";
                            }
                            if (chkOpt1.Checked == true && j == 2)
                            {
                                strBiGbn = strOK = "OK";
                            }
                            if (chkOpt3.Checked == true && j == 3)
                            {
                                strBiGbn = strOK = "OK";
                            }
                            if (chkOpt2.Checked == true && j == 5)
                            {
                                strBiGbn = strOK = "OK";
                            }


                            if (j == 4)
                            {
                                strOK = "";
                            }

                            if (strOK == "OK")
                            {
                                nRow = nRow + 1;
                                SSView5_Sheet1.RowCount = nRow;

                                switch (j)
                                {
                                    case 1:
                                        SSView5_Sheet1.Cells[nRow - 1, 1].Text = "건보";
                                        break;
                                    case 2:
                                        SSView5_Sheet1.Cells[nRow - 1, 1].Text = "의급";
                                        break;
                                    case 3:
                                        SSView5_Sheet1.Cells[nRow - 1, 1].Text = "산재";
                                        break;
                                    case 5:
                                        SSView5_Sheet1.Cells[nRow - 1, 1].Text = "자보";
                                        break;
                                }

                                SSView5_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nTempAmt = 0;
                                nTempAmt2 = 0;
                                nTempAmt3 = 0;
                                //nTempAmt4 = 0;
                                nTempAmt5 = 0;
                                //nTempAmt6 = 0;
                                //nTempAmt7 = 0;
                                //nTempAmt8 = 0;

                                //'발생
                                if (Check1.Checked == false)
                                {
                                    SQL = "";
                                    SQL = SQL + ComNum.VBLF + " select substr(trim(bi),1,1) bi,deptcode,SUM(Amt1) TotAmt , SUM(Amt2) Amt2, ";
                                    SQL = SQL + ComNum.VBLF + " SUM( DECODE(RTRIM(SUNEXT) ,'Y98D', AMT1 + AMT2 , 0 )) AMT3,";
                                    SQL = SQL + ComNum.VBLF + " SUM( DECODE(RTRIM(SUNEXT) ,'DRG001', AMT1 ,'DRG002', AMT1, 0 )) AMT5 ,";
                                    SQL = SQL + ComNum.VBLF + " SUM( DECODE(RTRIM(SUNEXT) ,'Y97', AMT4, 0 )) AMT6,";
                                    SQL = SQL + ComNum.VBLF + " SUM( DECODE(RTRIM(SUNEXT) ,'Y97', DanAmt, 0 )) Damt";
                                    SQL = SQL + ComNum.VBLF + " From " + ComNum.DB_PMPA + "OPD_SLIP ";
                                    SQL = SQL + ComNum.VBLF + " where actdate >=to_date('" + strFDate + "','yyyy-mm-dd')";
                                    SQL = SQL + ComNum.VBLF + " and actdate <=to_date('" + strTdate + "','yyyy-mm-dd')";
                                    SQL = SQL + ComNum.VBLF + " and bun ='98'   ";
                                    SQL = SQL + ComNum.VBLF + "   and substr(trim(bi),1,1) ='" + j + "' ";
                                    SQL = SQL + ComNum.VBLF + "   and deptcode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + " group by substr(trim(bi),1,1),deptcode ";

                                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        btnSearch.Enabled = true;
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }

                                    if (dt2.Rows.Count > 0)
                                    {
                                        SSView5_Sheet1.Cells[nRow - 1, 3].Text = Convert.ToDouble(dt2.Rows[0]["TotAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                                        nTempAmt = Convert.ToDouble(dt2.Rows[0]["TotAmt"].ToString().Trim());
                                        nSumTot1 = nSumTot1 + Convert.ToDouble(dt2.Rows[0]["TotAmt"].ToString().Trim());
                                    }
                                    if (dt2 == null)
                                    {
                                        dt2.Dispose();
                                        dt2 = null;
                                    }
                                }
                                //'응급실 6시간
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + " select substr(trim(bi),1,1) bi,deptcode,SUM(TotAmt) TotAmt,SUM(Junggan) Junggan,";
                                SQL = SQL + ComNum.VBLF + " SUM(Johap) Johap,SUM(Halin) Halin,SUM(Bojung) Bojung,";
                                SQL = SQL + ComNum.VBLF + " SUM(EtcMisu) EtcMisu,SUM(SuNap) SuNap,SUM(Dansu) Dansu, SUM(DRUGAMT) DRUGAMT ,SUM(Johap+junggan) totamt,SUM(JepJAmt) JepJAmt ";
                                SQL = SQL + ComNum.VBLF + " From MISU_BALTEWON";
                                SQL = SQL + ComNum.VBLF + " where actdate >=to_date('" + strFDate + "','yyyy-mm-dd')";
                                SQL = SQL + ComNum.VBLF + " and actdate <=to_date('" + strTdate + "','yyyy-mm-dd')";
                                SQL = SQL + ComNum.VBLF + " and gubun in ('3')  ";
                                SQL = SQL + ComNum.VBLF + "   and substr(trim(bi),1,1) ='" + j + "' ";
                                SQL = SQL + ComNum.VBLF + "   and deptcode ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + " group by substr(trim(bi),1,1),deptcode ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    btnSearch.Enabled = true;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }


                                if (dt2.Rows.Count > 0)
                                {
                                    nTempAmt2 = Convert.ToDouble(dt2.Rows[0]["Johap"].ToString().Trim());
                                    nSumTot2 = nSumTot2 + Convert.ToDouble(dt2.Rows[0]["Johap"].ToString().Trim());
                                    SSView5_Sheet1.Cells[nRow - 1, 4].Text = Convert.ToDouble(dt2.Rows[0]["Johap"].ToString().Trim()).ToString("###,###,###,##0 ");
                                }
                                if (dt2 == null)
                                {
                                    dt2.Dispose();
                                    dt2 = null;
                                }

                                nTempAmt3 = nTempAmt - nTempAmt2;
                                nSumTot3 = nSumTot3 + nTempAmt3;

                                SSView5_Sheet1.Cells[nRow - 1, 5].Text = (nTempAmt3).ToString("###,###,###,##0 ");//  '외래조합-낮병동


                                strDept = "";
                                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nDept = 0;
                                nDept = (int)VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(strDeptRow, strDept, 2), ";", 1), ",", 2));

                                if (strDept == "R6")
                                {
                                    //i = i;
                                }

                                if (nDept > 0)
                                {
                                    SSView5_Sheet1.Cells[nRow - 1, 6].Text = nSumDept[nDept, j].ToString("###,###,###,##0 ");
                                    nTempAmt5 = nSumDept[nDept, j] - nTempAmt3;
                                    nSumTot4 = nSumTot4 + nTempAmt5;
                                    SSView5_Sheet1.Cells[nRow - 1, 7].Text = nTempAmt5.ToString("###,###,###,##0 ");
                                    SSView5_Sheet1.Cells[nRow - 1, 8].Text = nSumCnt[nDept, j].ToString("###,###,###,##0 ");
                                }
                                else
                                {
                                    nTempAmt5 = nSumDept[nDept, j] - nTempAmt3;
                                    nSumTot4 = nSumTot4 + nTempAmt5;
                                    SSView5_Sheet1.Cells[nRow - 1, 7].Text = nTempAmt5.ToString("###,###,###,##0 ");
                                }
                                Application.DoEvents();
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                SSView5_Sheet1.RowCount = nRow + 1;
                SSView5_Sheet1.Cells[SSView5_Sheet1.RowCount - 1, 3].Text = nSumTot1.ToString("###,###,###,##0 ");
                SSView5_Sheet1.Cells[SSView5_Sheet1.RowCount - 1, 4].Text = nSumTot2.ToString("###,###,###,##0 ");
                SSView5_Sheet1.Cells[SSView5_Sheet1.RowCount - 1, 5].Text = nSumTot3.ToString("###,###,###,##0 ");
                SSView5_Sheet1.Cells[SSView5_Sheet1.RowCount - 1, 6].Text = nTempMisuAmt.ToString("###,###,###,##0 ");
                SSView5_Sheet1.Cells[SSView5_Sheet1.RowCount - 1, 7].Text = nSumTot4.ToString("###,###,###,##0 ");
                SSView5_Sheet1.Cells[SSView5_Sheet1.RowCount - 1, 8].Text = nTempCnt.ToString("###,###,###,##0 ");

                Cursor.Current = Cursors.Default;



                if (ComFunc.MsgBoxQ("파일로 만드시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    return;
                Cursor.Current = Cursors.WaitCursor;
                x = SSView5.SaveExcel("C:\\" + lblTitle.Text + ".xlsx", FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);
                {
                    if (x == true)
                        ComFunc.MsgBox("엑셀파일이 생성이 되었습니다.", "확인");
                    else
                        ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
                }

                Cursor.Current = Cursors.Default;
            }

            catch (Exception ex)
            {

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void chkOpt0_CheckedChanged(object sender, EventArgs e)
        {
            SS_Set();
        }

        private void chkOpt1_CheckedChanged(object sender, EventArgs e)
        {
            SS_Set();
        }

        private void chkOpt2_CheckedChanged(object sender, EventArgs e)
        {
            SS_Set();
        }

        private void chkOpt3_CheckedChanged(object sender, EventArgs e)
        {
            SS_Set();
        }
    }
}
