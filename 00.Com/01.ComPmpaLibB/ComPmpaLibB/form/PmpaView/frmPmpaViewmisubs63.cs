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
    /// Create Date     : 2017-10-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\misu\misubs\misubs.vbp\misubs63.frm(FrmReMirView3.frm) >> frmPmpaViewmisubs63.cs 폼이름 재정의" />

    public partial class frmPmpaViewmisubs63 : Form
    {
        string GstrSakYYMM = "";
        string GstrSakIO = "";
        string GstrSakGBN = "";
        string GstrSakJohap = "";
        string GstrMenu = "";
        string GstrSMenu = "";
        string GstrYYMM = "";

        double[,] FnTotAmt = new double[3, 9];

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewmisubs63()
        {
            InitializeComponent();

        }
        public frmPmpaViewmisubs63(string strstrSakYYMM, string strstrSakIO, string strstrSakJohap)
        {
            InitializeComponent();

            GstrSakYYMM = strstrSakYYMM;
            GstrSakIO = strstrSakIO;
            GstrSakJohap = strstrSakJohap;
        }

        public frmPmpaViewmisubs63(string strstrSakYYMM, string strstrSakIO, string strstrSakJohap, string strstrSakGBN, string strstrMenu, string strstrSMenu, string strstrYYMM)
        {
            InitializeComponent();

            GstrYYMM = strstrYYMM;
            GstrSakIO = strstrSakIO;
            GstrSakJohap = strstrSakJohap;
            GstrSMenu = strstrSMenu;
            GstrMenu = strstrMenu;
            GstrSakYYMM = strstrSakYYMM;
            GstrSakGBN = strstrSakGBN;
        }

        private void frmPmpaViewmisubs63_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            btnReSearch.Visible = true;

            clsVbfunc.SetCboDate(clsDB.DbCon, cboyyyy, 12, "", "1");

            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboyyyy.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

            cboJong.Items.Add("0.전체");
            cboJong.Items.Add("1.건강보험");
            cboJong.Items.Add("2.의료급여");
            cboJong.SelectedIndex = 0;

            Screen_Clear();

            //인수값 으로 자동 동조회
            if (GstrSakIO != "")
            {
                if (GstrSakGBN == "0")
                {
                    rdojob0.Checked = true;//외래
                }

                if (GstrSakGBN == "1")
                {
                    rdojob1.Checked = true;//외래
                }

                switch (GstrSakJohap)
                {
                    case "1":
                        cboJong.Text = "1.건강보험";
                        break;
                    case "5":
                        cboJong.Text = "2.의료급여";
                        break;
                    default:
                        cboJong.Text = "0.전체";
                        break;
                }

                cboyyyy.Text = VB.Left(GstrSakYYMM, 4) + " 년" + VB.Mid(GstrSakYYMM, 5, 2) + "월";

                Search();

            }
        }

        private void Screen_Clear()
        {
            btnSearch.Enabled = true;
            ssView_Sheet1.RowCount = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            Search();
        }

        private void btnReSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            ssView_Sheet1.Rows.Count = 0;
            //Search();

        }
        #region MyRegion    접수번호별
        private void Search()
        {
            int i = 0;
            int j = 0;
            int nRow = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTdate = "";
            string strJong = "";
            string strOldData = "";
            string strNewData = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            btnSearch.Enabled = false;

            Screen_Clear();

            strYYMM = VB.Left(cboyyyy.Text, 4) + VB.Mid(cboyyyy.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            strJong = VB.Left(cboJong.Text, 1);

            for (i = 1; i <= 2; i++)
            {
                for (j = 1; j <= 4; j++)
                {
                    FnTotAmt[i, j] = 0;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + " Johap,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,JepNo,IpdOpd,";
                SQL = SQL + ComNum.VBLF + " DTbun,YYMM,Week,MirGbn,SamQty,SamTAmt,SamJAmt,SamJangAmt, SAMGAMT,  ";
                SQL = SQL + ComNum.VBLF + " SamDaebul,MirNo,ROWID, SAKAMT, REMIR, RESULTAMT, SAKAMTOUT, REMIROUT, RESULTAMTOUT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_JEPSU ";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                if (rdojob0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND JepDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND JepDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND YYMM = '" + strYYMM + "' ";
                }

                SQL = SQL + ComNum.VBLF + " AND BanDate IS NULL ";
                SQL = SQL + ComNum.VBLF + " AND JepDate IS NOT NULL ";

                switch (strJong)
                {
                    case "1":
                        SQL = SQL + ComNum.VBLF + " AND Johap <> '5' ";
                        break;
                    case "2":
                        SQL = SQL + ComNum.VBLF + " AND Johap = '5' ";
                        break;
                }

                if (rdoIOI0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND IpdOpd='O' ";
                }
                else if (rdoIOI1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND IpdOpd='I' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND IpdOpd IN ('O','I') ";
                }

                if (strJong == "3")
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY JOHAP,JepDate,IpdOpd,JepNo ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY JOHAP, JepDate,IpdOpd,DTBun,JepNo ";
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
                    btnSearch.Enabled = true;
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                strOldData = "";
                nRow = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = dt.Rows[i]["Johap"].ToString().Trim();

                    if (strOldData != strNewData && strOldData != "")
                    {
                        CmdView_SubTotal(ref nRow);
                    }

                    nRow = nRow + 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    if (strOldData != strNewData)
                    {
                        switch (strNewData)
                        {
                            case "1":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "공단";
                                break;
                            case "2":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "직장";
                                break;
                            case "3":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "지역";
                                break;
                            case "5":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "보호";
                                break;
                            case "6":
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = "산재";
                                break;
                        }
                        strOldData = strNewData;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["JepDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["JepNo"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();

                    switch (dt.Rows[i]["DTbun"].ToString().Trim())
                    {
                        case "0":
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "의과";
                            break;
                        case "1":
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "내과";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "외과";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "산소";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "안이";
                            break;
                        case "5":
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "피비";
                            break;
                        case "6":
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "치과";
                            break;
                        case "7":
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "NP정액";
                            break;
                        default:
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "";
                            break;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["YYMM"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Week"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["MirGbn"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["SamQty"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = VB.Val(dt.Rows[i]["SamTAmt"].ToString().Trim()).ToString("###,###,##0");

                    ssView_Sheet1.Cells[nRow - 1, 10].Text = VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = VB.Val(dt.Rows[i]["REMIR"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = VB.Val(dt.Rows[i]["RESULTAMT"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = VB.Val(dt.Rows[i]["SAKAMTOUT"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = VB.Val(dt.Rows[i]["REMIROUT"].ToString().Trim()).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = VB.Val(dt.Rows[i]["RESULTAMTOUT"].ToString().Trim()).ToString("###,###,##0");

                    //'소계,합계에 ADD
                    FnTotAmt[1, 1] = FnTotAmt[1, 1] + VB.Val(dt.Rows[i]["SamQty"].ToString().Trim());
                    FnTotAmt[1, 2] = FnTotAmt[1, 2] + VB.Val(dt.Rows[i]["SamTAmt"].ToString().Trim());
                    FnTotAmt[1, 3] = FnTotAmt[1, 3] + VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim());
                    FnTotAmt[1, 4] = FnTotAmt[1, 4] + VB.Val(dt.Rows[i]["REMIR"].ToString().Trim());
                    FnTotAmt[1, 5] = FnTotAmt[1, 5] + VB.Val(dt.Rows[i]["RESULTAMT"].ToString().Trim());
                    FnTotAmt[1, 6] = FnTotAmt[1, 6] + VB.Val(dt.Rows[i]["SAKAMTOUT"].ToString().Trim());
                    FnTotAmt[1, 7] = FnTotAmt[1, 7] + VB.Val(dt.Rows[i]["REMIROUT"].ToString().Trim());
                    FnTotAmt[1, 8] = FnTotAmt[1, 8] + VB.Val(dt.Rows[i]["RESULTAMTOUT"].ToString().Trim());
                    FnTotAmt[2, 1] = FnTotAmt[2, 1] + VB.Val(dt.Rows[i]["SamQty"].ToString().Trim());
                    FnTotAmt[2, 2] = FnTotAmt[2, 2] + VB.Val(dt.Rows[i]["SamTAmt"].ToString().Trim());
                    FnTotAmt[2, 3] = FnTotAmt[2, 3] + VB.Val(dt.Rows[i]["SAKAMT"].ToString().Trim());
                    FnTotAmt[2, 4] = FnTotAmt[2, 4] + VB.Val(dt.Rows[i]["REMIR"].ToString().Trim());
                    FnTotAmt[2, 5] = FnTotAmt[2, 5] + VB.Val(dt.Rows[i]["RESULTAMT"].ToString().Trim());
                    FnTotAmt[2, 6] = FnTotAmt[2, 6] + VB.Val(dt.Rows[i]["SAKAMTOUT"].ToString().Trim());
                    FnTotAmt[2, 7] = FnTotAmt[2, 7] + VB.Val(dt.Rows[i]["REMIROUT"].ToString().Trim());
                    FnTotAmt[2, 8] = FnTotAmt[2, 8] + VB.Val(dt.Rows[i]["RESULTAMTOUT"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
                btnSearch.Enabled = true;
                Cursor.Current = Cursors.Default;

                CmdView_SubTotal(ref nRow);
                CmdView_Total(ref nRow);

                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.Columns[6].BackColor = Color.FromArgb(180, 255, 255);
                ssView_Sheet1.Columns[7].BackColor = Color.FromArgb(180, 255, 255);
                ssView_Sheet1.Columns[10].BackColor = Color.FromArgb(206, 251, 201);
                ssView_Sheet1.Columns[11].BackColor = Color.FromArgb(206, 251, 201);
                ssView_Sheet1.Columns[12].BackColor = Color.FromArgb(206, 251, 201);

                ssView_Sheet1.Columns[13].BackColor = Color.FromArgb(227, 196, 255);
                ssView_Sheet1.Columns[14].BackColor = Color.FromArgb(227, 196, 255);
                ssView_Sheet1.Columns[15].BackColor = Color.FromArgb(227, 196, 255);
          

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 소계
        /// </summary>
        private void CmdView_SubTotal(ref int nRow)
        {
            int j = 0;
            nRow = nRow + 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }
            ssView_Sheet1.Cells[nRow - 1, 4].Text = "**소계**";
            for (j = 1; j <= 8; j++)
            {
                ssView_Sheet1.Cells[nRow - 1, j + 7].Text = FnTotAmt[1, j].ToString("###,###,###,##0");
                FnTotAmt[1, j] = 0;
            }
        }

        /// <summary>
        /// 합계
        /// </summary>
        private void CmdView_Total(ref int nRow)
        {
            int j = 0;
            nRow = nRow + 1;

            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }
            ssView_Sheet1.Cells[nRow - 1, 4].Text = "**합계**";

            for (j = 1; j <= 8; j++)
            {
                ssView_Sheet1.Cells[nRow - 1, j + 7].Text = FnTotAmt[2, j].ToString("###,###,###,##0");
            }
        }

        #endregion


        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "청구 삭감액 이의신청 접수현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            if (rdoIOI0.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("(외래)", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else if (rdoIOI1.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("(입원)", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else
            {
                strHeader += CS.setSpdPrint_String("(외래 + 입원)", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            if (rdojob0.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("접수월 : " + cboyyyy.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else
            {
                strHeader += CS.setSpdPrint_String("진료월 : " + cboyyyy.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false,(float)0.9);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            if(e.Column == 6)
            {
                lblFoot.Text = "주별구분 ( 1-5 : 1주차~5주차 주별청구, 6.외래 및 퇴원청구, 7. 중간청구)";
            }
            
            if(e.Column == 7)
            {
                lblFoot.Text = "청구구분(0.일반청구 1.보완(재)청구 2.추가청구 4.NP정액)";
            }
        }
    }
}
