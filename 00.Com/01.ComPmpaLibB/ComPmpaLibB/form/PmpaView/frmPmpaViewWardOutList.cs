using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using DevComponents.DotNetBar;
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    public partial class frmPmpaViewWardOutList : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : 
        /// Description     : 
        /// Author          : 김효성
        /// Create Date     : 2017-10-27
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref=  d:\psmh\IPD\iument\iument.vbp\Frm퇴원예고예정자명단.frm" >> frmPmpaViewWardOutList.cs 폼이름 재정의" />

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();
        clsComPmpaSpd cCPSpd = new clsComPmpaSpd();
        clsPmpaPb cPb = new clsPmpaPb();

        string GstrJewon = "";
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        int FnMinIlsu = 0;
        int FnMaxIlsu = 0;
        string FstrPart = "";
        string FstrPanoList = "";
        string FstrJob = "";
        string FstrCaption = "";
        string FstrDeptCodes = "";
        string FstrDrCodes = "";
        string FstrWardCodes = "";

        public frmPmpaViewWardOutList()
        {
            InitializeComponent();
            SetEvent();
        }
        
        public frmPmpaViewWardOutList(string strGstrJewon)
        {
            InitializeComponent();
            SetEvent();
            GstrJewon = strGstrJewon;
        }

        public frmPmpaViewWardOutList(string strPart, bool bPart = false)
        {
            InitializeComponent();
            SetEvent();
            Load_Ward_퇴원예고용();
            FstrPart = strPart;
        }

        //MHMAIN 에서 호출할때 사용함
        public frmPmpaViewWardOutList(string strPart, string bPart)
        {
            InitializeComponent();
            SetEvent();
            Load_Ward_퇴원예고용();
            FstrPart = strPart;
        }


        void SetEvent()
        {
            superTabControl1.SelectedTabChanged += new EventHandler<SuperTabStripSelectedTabChangedEventArgs>(eChgSelTab);
            this.ssList.CellClick += new CellClickEventHandler(eSpdClick);
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            clsSpread cSpd = new clsSpread();

            if (sender == ssList)
            {
                if (e.ColumnHeader == true)
                {
                    cSpd.setSpdSort(ssList, e.Column, true);
                    return;
                }
            }
        }

        void eChgSelTab(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            if (superTabControl1.SelectedTabIndex == 0)
            {
                panel4.Visible = true;
                panHedden2.Visible = true;
            }
            else
            {
                panel4.Visible = false;
                panHedden2.Visible = false;
            }
            
        }

        private void frmPmpaViewWardOutList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cCPSpd.sSpd_enmROutList(ssList, cPb.sSpdROutList, cPb.nSpdROutList, 10, 0);

            panTong.Visible = false;
            SS2.Visible = false;
            SS1.Dock = DockStyle.Fill;
            SS1.Visible = true;
            //panhedden1.Visible = false;
            dtpOutDate.Value = Convert.ToDateTime(strDTP);
            dtpOutDate2.Value = Convert.ToDateTime(strDTP);
            panel15.Visible = false;

            Load_Dept_퇴원예고용();
            Load_Ward_퇴원예고용();
            Load_DrCode_퇴원예고용();

            cboDr.Items.Add("전체");
            cboDr.SelectedIndex = 0;

            SS2_Sheet1.Columns[19].Visible = false;
            SS2_Sheet1.Columns[20].Visible = false;


            if (GstrJewon != "")
            {
                FstrJob = (VB.Pstr(GstrJewon, "{@}", 1)).Trim();
                FnMinIlsu = Convert.ToInt32((VB.Val(VB.Pstr(GstrJewon, "{@}", 2))));
                FnMaxIlsu = Convert.ToInt32((VB.Val(VB.Pstr(GstrJewon, "{@}", 3))));
                FstrCaption = (VB.Pstr(GstrJewon, "{@}", 4)).ToString().Trim();
                GstrJewon = "";

                Load_Dept_퇴원예고용();
                Load_Ward_퇴원예고용();
                Load_DrCode_퇴원예고용();

                cboDr.Items.Add("전체");
                cboDr.SelectedIndex = 0;
            }

            if (chkoWard.Checked == false)
            {
                //panhedden1.Visible = false;
                SS1_Sheet1.Columns[9].Visible = false;
                SS1_Sheet1.Columns[14].Visible = false;

                SS2_Sheet1.Columns[19].Visible = false;
                SS2_Sheet1.Columns[20].Visible = false;
            }
            else
            {

            }

            if (FstrPart == "원무")
            {
                superTabControl1.SelectedTabIndex = 1;
            }
        }

        #region Load 함수 모음

        /// <summary>
        /// Load_Dept_퇴원예고용
        /// </summary>
        private void Load_Dept_퇴원예고용()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT * ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT  ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1  ";
                SQL = SQL + ComNum.VBLF + "     AND PrintRanking < 32       ";
                SQL = SQL + ComNum.VBLF + "     AND DeptCode NOT IN ('PT','LM')        ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking  ";

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

                FstrDeptCodes = "";
                cboDept.Items.Clear();
                cboDept.Items.Add("전체");

                for (i = 1; i <= dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i - 1]["DeptCode"].ToString().Trim());
                    FstrDeptCodes = FstrDeptCodes + dt.Rows[i - 1]["DeptCode"].ToString().Trim() + "^^";
                }

                cboDept.SelectedIndex = 0;

                dt.Dispose();
                dt = null;

                if (clsPublic.GstrPassProgramID == "IPDTN")
                {
                    cboDept.Items.Clear();
                    cboDept.Items.Add("MR");
                    cboDept.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// Load_Ward_퇴원예고용
        /// </summary>
        private void Load_Ward_퇴원예고용()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT * ";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WARDCODE ";

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

                FstrWardCodes = "";
                cboWard.Items.Clear();
                cboWard.Items.Add("전체");

                cboWardH.Items.Clear();
                cboWardH.Items.Add("전체");

                for (i = 1; i <= dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i - 1]["WardCode"].ToString().Trim());
                    cboWardH.Items.Add(dt.Rows[i - 1]["WardCode"].ToString().Trim());
                    FstrWardCodes = FstrWardCodes + dt.Rows[i - 1]["WardCode"].ToString().Trim() + "^^";
                }
                cboWard.SelectedIndex = 0;
                cboWardH.SelectedIndex = 0;

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// Load_DrCode_퇴원예고용
        /// </summary>
        private void Load_DrCode_퇴원예고용()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.DrCode ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR a, " + ComNum.DB_PMPA + "BAS_CLINICDEPT b";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND a.DRDept1 =b.DeptCode";
                SQL = SQL + ComNum.VBLF + "   AND  a.DRCODE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "ORDER BY b.PrintRanking";


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

                FstrDrCodes = "";
                for (i = 1; i <= dt.Rows.Count; i++)
                {
                    FstrDrCodes = FstrDrCodes + dt.Rows[i - 1]["DRCODE"].ToString().Trim() + "^^";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return;

            if (superTabControl1.SelectedTabIndex == 1)
            {
                ePrint_Won();
                return;
            }

            SS2_Sheet1.Columns[18].Visible = true;
            //SS2_Sheet1.Columns[23].Visible = true;
            //SS2_Sheet1.Columns[24].Visible = true;
            //SS2_Sheet1.Columns[25].Visible = true;
            //SS2_Sheet1.Columns[26].Visible = true;

            SS2_Sheet1.RowCount = SS1_Sheet1.RowCount;

            for (i = 1; i <= SS1_Sheet1.RowCount; i++)
            {
                for (j = 2; j <= SS1_Sheet1.ColumnCount; j = j - 2)
                {
                    SS2_Sheet1.Cells[i - 1, j - 2].Text = SS1_Sheet1.Cells[i - 1, j - 1].Text;
                }
            }

            if (ChkAmset5.Checked == true)
            {
                strTitle = "퇴원예고자";
            }
            else
            {
                strTitle = "당일퇴원자";
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);

            SS2_Sheet1.Columns[18].Visible = false;
            //SS2_Sheet1.Columns[23].Visible = false;
            //SS2_Sheet1.Columns[24].Visible = false;
            //SS2_Sheet1.Columns[25].Visible = false;
            //SS2_Sheet1.Columns[26].Visible = false;
        }

        private void ePrint_Won()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "퇴원예고자";
            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 100, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint10_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "퇴원예고자-과별";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS100, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnPrint11_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "퇴원예고자-진료과장별";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS101, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnPrint12_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "퇴원예고자-병동별";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS102, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            int nRow = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            //if (ComQuery.IsJobAuth (this , "P") == false) { return; }     //권한확인

            SS2_Sheet1.RowCount = SS1_Sheet1.RowCount;

            nRow = 0;
            for (i = 1; i <= SS1_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(SS1_Sheet1.Cells[i - 1, 0].Value) == true)
                {
                    nRow = nRow + 1;
                    if (nRow > SS2_Sheet1.RowCount)
                    {
                        SS2_Sheet1.RowCount = nRow;
                    }
                    //for (j = 2; j <= SS1_Sheet1.ColumnCount - 2; j++)
                    //{
                    //    SS2_Sheet1.Cells[nRow - 1, j - 2].Text = SS1_Sheet1.Cells[i - 1, j - 1].Text;
                    //}

                    for (j = 2; j <= SS2_Sheet1.ColumnCount - 2; j++)
                    {
                        SS2_Sheet1.Cells[nRow - 1, j - 2].Text = SS1_Sheet1.Cells[i - 1, j - 1].Text;
                    }
                }
            }

            strTitle = "퇴원 예고자 명단";
            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        /// <summary>
        /// 원무용 조회
        /// </summary>
        void eSearCh_Rec()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strFDate = dtpOutDate.Value.ToString("yyyy-MM-dd");

            int nRow = 0, nRead = 0, i = 0;

            CS.Spread_All_Clear(ssList);

            btnSearch.Enabled = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT   M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,";
            SQL += ComNum.VBLF + "          TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,M.IpdNo,M.GbSts,";
            SQL += ComNum.VBLF + "          TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,";
            SQL += ComNum.VBLF + "          TO_CHAR(M.ROutDate,'YYYY-MM-DD HH24:MI') MROUTDATE,";
            SQL += ComNum.VBLF + "          TO_CHAR(M.SimsaTime,'YYYY-MM-DD HH24:MI') SimsaTime,";
            SQL += ComNum.VBLF + "          TO_CHAR(E.ROutDate,'YYYY-MM-DD') ROutDate, ";
            SQL += ComNum.VBLF + "          TO_CHAR(E.ROutENTTIME,'YYYY-MM-DD HH24:MI') ROutENTTIME, ";
            SQL += ComNum.VBLF + "          E.InRoom, E.OutRoom, E.InDept, ";
            SQL += ComNum.VBLF + "          M.DeptCode,M.DrCode,D.DrName,M.AmSet1,M.AmSet6,M.AmSet7 ";
            SQL += ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M,";
            SQL += ComNum.VBLF + "          " + ComNum.DB_PMPA + "BAS_PATIENT P,    ";
            SQL += ComNum.VBLF + "          " + ComNum.DB_PMPA + "BAS_DOCTOR  D,    ";
            SQL += ComNum.VBLF + "          " + ComNum.DB_PMPA + "NUR_MASTER  E     ";
            switch (cboWardH.Text.Trim())
            {
                case "전체": SQL += ComNum.VBLF + " WHERE M.WardCode>' ' ";                            break;
                case "MICU": SQL += ComNum.VBLF + " WHERE M.RoomCode='234' ";                          break;
                case "SICU": SQL += ComNum.VBLF + " WHERE M.RoomCode='233' ";                          break;
                case "ND":   SQL += ComNum.VBLF + " WHERE M.WardCode IN ('ND','IQ') ";                 break;
                default:     SQL += ComNum.VBLF + " WHERE M.WardCode='" + cboWardH.Text.Trim() + "' "; break;
            }
            SQL += ComNum.VBLF + "  AND M.Pano<>'81000004' ";
            SQL += ComNum.VBLF + "  AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD')) ";
            SQL += ComNum.VBLF + "  AND M.IpwonTime < TO_DATE('" + Convert.ToDateTime(strFDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND M.Pano < '90000000' ";
            SQL += ComNum.VBLF + "  AND M.Pano <> '81000004' ";
            SQL += ComNum.VBLF + "  AND M.GbSTS <> '9' ";
            SQL += ComNum.VBLF + "  AND M.Pano=P.Pano(+) ";
            SQL += ComNum.VBLF + "  AND M.Ipdno=E.Ipdno(+) ";
            SQL += ComNum.VBLF + "  AND M.DrCode=D.DrCode(+) ";
            SQL += ComNum.VBLF + "  AND trunc(E.ROutDate) =TO_DATE('" + strFDate + "','YYYY-MM-DD') ";

            if (radioButton4.Checked == true) //17: 30 이전등록
               SQL += ComNum.VBLF + "  AND E.ROutEntTime < TO_DATE('" + strFDate + " 17:31','YYYY-MM-DD HH24:MI') ";
            else if (radioButton5.Checked == true) //18시이전등록
               SQL += ComNum.VBLF + "  AND E.ROutEntTime < TO_DATE('" + strFDate + " 18:01','YYYY-MM-DD HH24:MI') ";
            else if (radioButton3.Checked == true) //17시이전등록
               SQL += ComNum.VBLF + "  AND E.ROutEntTime < TO_DATE('" + strFDate + " 17:01','YYYY-MM-DD HH24:MI') ";
            else
               SQL += ComNum.VBLF + "  AND TRUNC(E.ROutEntTime) <= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            
            if (cboWardH.Text.Trim() != "전체")
            {
                if (cboWardH.Text.Trim() == "MICU")
                {
                    SQL += ComNum.VBLF + "   AND E.InWard ='IU'";
                    SQL += ComNum.VBLF + "   AND E.InRoom ='234'";
                }
                else if (cboWardH.Text.Trim() == "SICU")
                { 
                    SQL += ComNum.VBLF + "   AND E.InWard ='IU'";
                    SQL += ComNum.VBLF + "   AND E.InRoom ='233'";
                }
                else
                { 
                    SQL += ComNum.VBLF + "    AND E.InWard ='" + cboWardH.Text.Trim() + "' ";
                }
            }
            
            SQL += ComNum.VBLF + " ORDER BY E.ROUTENTTIME ASC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            btnSearch.Enabled = true;
            nRead = dt.Rows.Count;

            if (nRead > 0)
            {
                for (i = 0; i < nRead; i++)
                {
                    nRow += 1;
                    if (ssList.ActiveSheet.RowCount < nRow)
                    {
                        ssList.ActiveSheet.RowCount = nRow;
                    }

                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.ROutDate].Text    = dt.Rows[i]["ROutDate"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.ROUTENTTIME].Text = dt.Rows[i]["ROUTENTTIME"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.PANO].Text        = dt.Rows[i]["PANO"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.SNAME].Text       = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.BiName].Text      = clsVbfunc.GetBiName(dt.Rows[i]["Bi"].ToString().Trim());
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.Age].Text         = dt.Rows[i]["AGE"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.Sex].Text         = dt.Rows[i]["SEX"].ToString().Trim(); 
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.InRoom].Text      = dt.Rows[i]["InRoom"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.OutRoom].Text     = dt.Rows[i]["ROOMCODE"].ToString().Trim();     //현재 재원중인 호실로 표시
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.InDept].Text      = dt.Rows[i]["InDept"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.DRNAME].Text      = dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.OutDate].Text     = dt.Rows[i]["OutDate"].ToString().Trim();
                    ssList.ActiveSheet.Cells[nRow - 1, (int)clsPmpaPb.enmROutList.InDate].Text      = dt.Rows[i]["InDate"].ToString().Trim();
                    if (dt.Rows[i]["InDate"].ToString().Trim() == strFDate)
                    {
                        CS.setSpdCellColor(ssList, nRow - 1, 0, nRow - 1, ssList.ActiveSheet.ColumnCount - 1, Color.DarkSalmon);
                    }
                }
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 병동용 조회
        /// </summary>
        void eSearCh_Ward()
        {
            int i = 0;
            int k = 0;
            int q = 0;
            int M = 0;
            double nRead = 0;
            double nRow = 0;
            string strGbSTS = "";
            string strAmSet3 = "";
            string strOK = "";
            string strInDate = "";
            string strOutDate = "";
            string strOutDate2 = "";
            string strROutTime = "";
            string strSunapTime = "";
            string strNur_ROutDate = "";
            string strNur_RoutEntDate = "";
            string strROutFlag = "";
            //'통계관련
            double nn = 0;
            double[] nCntDept = new double[501];
            double[] nCntDept2 = new double[501];
            double[] nCntDept3 = new double[501];
            double[] nCntDept4 = new double[501];
            double[] nCntDrCode = new double[501];
            double[] nCntDrCode2 = new double[501];
            double[] nCntDrCode3 = new double[501];
            double[] nCntDrCode4 = new double[501];
            double[] nCntWardCode = new double[501];
            double[] nCntWardCode2 = new double[501];
            double[] nCntWardCode3 = new double[501];
            double[] nCntWardCode4 = new double[501];
            double nSumDept = 0;
            double nSumDept2 = 0;
            double nSumDept3 = 0;
            double nSumDept4 = 0;
            double nSumDrCode = 0;
            double nSumDrCode2 = 0;
            double nSumDrCode3 = 0;
            double nSumDrCode4 = 0;
            double nSumWardCode = 0;
            double nSumWardCode2 = 0;
            double nSumWardCode3 = 0;
            double nSumWardCode4 = 0;
            string strDeptCode = "";
            string strDrcode = "";
            string strWardcode = "";
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            nSumDept = 0;
            nSumDept2 = 0;
            nSumDept3 = 0;
            nSumDept4 = 0;
            nSumDrCode = 0;
            nSumDrCode2 = 0;
            nSumDrCode3 = 0;
            nSumDrCode4 = 0;
            nSumWardCode = 0;
            nSumWardCode2 = 0;
            nSumWardCode3 = 0;
            nSumWardCode4 = 0;

            for (i = 0; i <= 500; i++)
            {
                nCntDept[i] = 0;
                nCntDept2[i] = 0;
                nCntDept3[i] = 0;
                nCntDept4[i] = 0;
                nCntDrCode[i] = 0;
                nCntDrCode2[i] = 0;
                nCntDrCode3[i] = 0;
                nCntDrCode4[i] = 0;
                nCntWardCode[i] = 0;
                nCntWardCode2[i] = 0;
                nCntWardCode3[i] = 0;
                nCntWardCode4[i] = 0;
            }

            SS_Info_Sheet1.ColumnCount = 500;

            for (i = 2; i <= SS_Info_Sheet1.ColumnCount; i++)
            {
                SS_Info_Sheet1.Cells[0, i - 1].Text = "";
                SS_Info_Sheet1.Cells[1, i - 1].Text = "";
                SS_Info_Sheet1.Cells[2, i - 1].Text = "";
            }

            strOutDate = dtpOutDate.Value.ToString("yyyy-MM-dd").Trim();
            strOutDate2 = dtpOutDate2.Value.ToString("yyyy-MM-dd").Trim();

            SS1_Sheet1.RowCount = 0;

            nRow = 0;

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            //try
            //{
            //'IPD_TRANS 단위로 DISPLAY
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT B.IPDNO, B.Pano,a.Sname,a.RoomCode,TO_CHAR(B.InDate,'YYYY-MM-DD') InDate,  ";
            SQL = SQL + ComNum.VBLF + "        B.GbSTS,TO_CHAR(B.OutDate,'YYYY-MM-DD') OutDate, a.age, b.simsasabun,              ";
            SQL = SQL + ComNum.VBLF + "        a.DeptCode,a.DrCode,a.WardCode,c.rDRSabun,c.rNRSabun,a.GbSuDay,            ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(c.RDrTime,'MM/DD HH24:MI') RDrTime,                                 ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(c.RNrTime,'MM/DD HH24:MI') RNrTime,                                 ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(b.RoutDate,'YYYY-MM-DD HH24:MI') RoutDate,                          ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(b.SimsaTime,'YYYY-MM-DD HH24:MI') SimsaTime,                        ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(b.SunapTime,'YYYY-MM-DD HH24:MI') SunapTime,                        ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(c.ROutDate,'YYYY-MM-DD') ROutDate_NEW,                             ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(c.ROutEntTime,'YYYY-MM-DD HH24:MI') ROutEntDate_NEW,c.TRout_Gbn,   ";
            SQL = SQL + ComNum.VBLF + "        B.GbSTS, b.TRSNO,b.Bi,b.GbIPD,b.AmSet3, B.AMSET5                           ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "IPD_TRANS b, " + ComNum.DB_PMPA + "IPD_NEW_MASTER a ," + ComNum.DB_PMPA + "NUR_MASTER c   ";
            SQL = SQL + ComNum.VBLF + "   WHERE 1=1 ";
            SQL = SQL + ComNum.VBLF + "     AND b.OUTDATE >= TO_DATE('" + strOutDate + "','YYYY-MM-DD')                   ";
            SQL = SQL + ComNum.VBLF + "     AND b.OUTDATE <= TO_DATE('" + strOutDate2 + "','YYYY-MM-DD')                    ";

            if (ChkOut.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND b.GbSTS IN ('7')                                                            ";
                SQL = SQL + ComNum.VBLF + "    AND a.JDATE > TO_DATE('1900-01-01','YYYY-MM-DD') ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "    AND b.GbSTS IN ('1','2','3','4','5','6','7')                                   ";
                SQL = SQL + ComNum.VBLF + "    AND a.JDATE > TO_DATE('1900-01-01','YYYY-MM-DD') ";
            }

            SQL = SQL + ComNum.VBLF + "    AND b.IPDNO = a.IPDNO(+)                                                       ";
            SQL = SQL + ComNum.VBLF + "    AND b.IPDNO = c.IPDNO(+)                                                       ";
            SQL = SQL + ComNum.VBLF + "    AND b.GbIPD IN ('1')                                                       ";
            SQL = SQL + ComNum.VBLF + "    AND  (b.OUTDATE  >= TO_DATE('" + strOutDate + "','YYYY-MM-DD') AND b.OUTDATE  <= TO_DATE('" + strOutDate2 + "','YYYY-MM-DD') )    ";

            if (ChkAmset5.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "   AND B.AMSET5 = '1' ";
            }

            if (FstrJob == "01") //'재원일수 7일 이내 당일 퇴원예정  18세이상 65세 미만 내과계
            {
                SQL = SQL + ComNum.VBLF + " AND A.Age>=18 AND A.Age <= 64 ";
                SQL = SQL + ComNum.VBLF + " AND A.Ilsu <= 6 ";
                SQL = SQL + ComNum.VBLF + " AND A.DeptCode IN ('MD','PD','DM','NP','RM') ";
            }
            else if (FstrJob == "02") // '재원일수 7일 이내 당일 퇴원예정인 수술받은 만 18세 이상 65세민만 외과계
            {
                SQL = SQL + ComNum.VBLF + " AND A.Age>=18 AND A.Age <= 64 ";
                SQL = SQL + ComNum.VBLF + " AND A.Ilsu <= 6 ";
                SQL = SQL + ComNum.VBLF + " AND A.DeptCode NOT IN ('MD','PD','DM','NP','RM') ";
            }
            else if (FstrJob == "03") // '내과계 퇴원예정환자
            {
                SQL = SQL + ComNum.VBLF + " AND A.DeptCode IN ('MD','PD','DM','NP','RM') ";
            }
            if (cboDept.Text.Trim() != "" && cboDept.Text.Trim() != "전체")
            {
                SQL = SQL + ComNum.VBLF + "  AND a.DEPTCODE ='" + cboDept.Text.Trim() + "' ";
            }

            if (cboDr.Text.Trim() != "" && cboDr.Text.Trim() != "전체")
            {

                SQL = SQL + ComNum.VBLF + "  AND a.DRCODE ='" + VB.Pstr((cboDr.Text.Trim()), ".", 2) + "' ";
            }

            if (cboWard.Text.Trim() != "" && cboWard.Text.Trim() != "전체")
            {
                SQL = SQL + ComNum.VBLF + "  AND a.WARDCODE ='" + cboWard.Text.Trim() + "' ";
            }

            if (rdoOptSort0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY B.ROUTDATE ASC, B.PANO, B.OutDate,a.RoomCode,B.IPDNO,b.TRSNO                          ";
            }
            else if (rdoOptSort1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY B.DeptCode,b.DrCode,B.ROUTDATE ASC, B.OutDate,a.RoomCode,B.IPDNO,b.TRSNO              ";
            }

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

            nRead = dt.Rows.Count;
            //스프레드 출력문
            SS1_Sheet1.RowCount = dt.Rows.Count;
            SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strInDate = dt.Rows[i]["InDate"].ToString().Trim();
                strDrcode = dt.Rows[i]["DrCode"].ToString().Trim();
                strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                strWardcode = dt.Rows[i]["WardCode"].ToString().Trim();
                strSunapTime = dt.Rows[i]["SunapTime"].ToString().Trim();
                strNur_ROutDate = dt.Rows[i]["ROutDate_NEW"].ToString().Trim();
                strNur_RoutEntDate = dt.Rows[i]["ROutEntDate_NEW"].ToString().Trim();

                if (strDrcode == "2501")
                {
                    i = i;
                }

                strOK = "";

                if (chkROut.Checked == false)
                {
                    strOK = "Y";
                }

                // '2013-09-11
                if (ChkSuDay.Checked == true)
                {
                    if (dt.Rows[i]["GbSuDay"].ToString().Trim() == "Y")
                    {
                        strOK = "";
                    }
                }

                if (FstrJob == "02")
                {
                    strOK = "N";
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT COUNT(*) CNT ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "     AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "      AND OPDATE >=TO_DATE('" + dt.Rows[i]["INDATE"].ToString().Trim() + "','YYYY-MM-DD') ";

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
                        if (Convert.ToInt32(dt2.Rows[0]["CNT"].ToString().Trim()) > 0)
                        {
                            strOK = "Y";
                        }
                        dt2.Dispose();
                        dt2 = null;
                    }
                }

                if ("03433660" == dt.Rows[i]["PANO"].ToString().Trim())
                {
                    i = i;
                }

                strROutTime = VB.Right(dt.Rows[i]["ROutDate"].ToString().Trim(), 5);
                if (rdoOptTime1.Checked == true)
                {
                    if (string.Compare(strROutTime, txtTime.Text.Trim()) >= 0)
                    {
                        strOK = "N";
                    }
                }
                else if (rdoOptTime2.Checked == true)
                {
                    if (string.Compare(strROutTime, txtTime.Text.Trim()) < 0)
                    {
                        strOK = "N";
                    }
                }

                strROutFlag = "";

                if (strNur_ROutDate != "" && string.Compare(strNur_ROutDate, VB.Left(strNur_RoutEntDate, 10)) > 0)
                {
                    if (ChkSunap.Checked == true)
                    {
                        if (string.Compare(strNur_ROutDate, "2013-06-01") >= 0)
                        {
                            if (Convert.ToDateTime(strNur_ROutDate) < Convert.ToDateTime(strNur_ROutDate + " 17:31").AddDays(-1) && Convert.ToDateTime(strNur_ROutDate) == Convert.ToDateTime(VB.Left(strSunapTime, 10)) && string.Compare(VB.Mid(strSunapTime, 12, 5), "10:31") < 0)
                            {
                                if (chkROut.Checked == true)
                                {
                                    strOK = "Y";
                                }
                                strROutFlag = "Y";
                            }
                        }
                        else if (string.Compare(strNur_ROutDate, "2010-03-01") >= 0)
                        {
                            if ((string.Compare(VB.Mid(strNur_RoutEntDate, 12, 5), "18:01") < 0) && Convert.ToDateTime(strNur_ROutDate) == Convert.ToDateTime(VB.Left(strSunapTime, 10)) && string.Compare(VB.Mid(strSunapTime, 12, 5), "10:31") < 0)
                            {
                                if (chkROut.Checked == true)
                                {
                                    strOK = "Y";
                                }
                                strROutFlag = "Y";
                            }
                        }
                        else
                        {
                            if ((string.Compare(VB.Mid(strNur_RoutEntDate, 12, 5), "17:01") < 0) && Convert.ToDateTime(strNur_ROutDate) == Convert.ToDateTime(VB.Left(strSunapTime, 10)) && string.Compare(VB.Mid(strSunapTime, 12, 5), "10:31") < 0)
                            {
                                if (chkROut.Checked == true)
                                {
                                    strOK = "Y";
                                }
                                strROutFlag = "Y";
                            }
                        }
                    }
                    else
                    {
                        if (string.Compare(strNur_ROutDate, "2013-06-01") >= 0)
                        {
                            //if (Convert.ToDateTime(strNur_RoutEntDate) < Convert.ToDateTime(strNur_ROutDate + " 17:31").AddDays(-1))
                            if (strNur_RoutEntDate == "")
                            {
                                if (chkROut.Checked == true)
                                {
                                    strOK = "Y";
                                }
                                strROutFlag = "Y";
                            }
                            else if (Convert.ToDateTime(strNur_RoutEntDate) < Convert.ToDateTime(strNur_ROutDate + " 17:31").AddDays(-1))
                            {
                                if (chkROut.Checked == true)
                                {
                                    strOK = "Y";
                                }
                                strROutFlag = "Y";
                            }

                        }
                        else if (string.Compare(strNur_ROutDate, "2010-03-01") >= 0)
                        {
                            if (string.Compare(VB.Mid(strNur_RoutEntDate, 12, 5), "18:01") < 0)
                            {
                                if (chkROut.Checked == true)
                                {
                                    strOK = "Y";
                                }
                                strROutFlag = "Y";
                            }

                        }
                        else
                        {
                            if (string.Compare(VB.Mid(strNur_RoutEntDate, 12, 5), "17:01") < 0)
                            {
                                if (chkROut.Checked == true)
                                {
                                    strOK = "Y";
                                }
                                strROutFlag = "Y";
                            }
                        }
                    }
                }
                else
                {
                    if (chkROut.Checked == true)
                    {
                        strOK = "";
                    }
                    strROutFlag = "";
                }

                if (strOK == "Y")
                {
                    nRow = nRow + 1;
                    SS1_Sheet1.RowCount = (int)nRow;

                    strGbSTS = dt.Rows[i]["GbSTS"].ToString().Trim();

                    SS1_Sheet1.Cells[(int)nRow - 1, 0].Text = "";
                    SS1_Sheet1.Cells[(int)nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 2].Text = dt.Rows[i]["AGE"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 4].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 5].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 7].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                    SS1_Sheet1.Cells[(int)nRow - 1, 8].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 9].Text = "";

                    if (dt.Rows[i]["GBIPD"].ToString().Trim() == "9")
                    {
                        SS1_Sheet1.Cells[(int)nRow - 1, 9].Text = "지병";
                    }

                    SS1_Sheet1.Cells[(int)nRow - 1, 10].Text = dt.Rows[i]["ROutEntDate_NEW"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 11].Text = dt.Rows[i]["SimsaTime"].ToString().Trim();

                    if (dt.Rows[i]["ROutEntDate_NEW"].ToString().Trim() != "" && dt.Rows[i]["SimsaTime"].ToString().Trim() != "")
                    {
                        SS1_Sheet1.Cells[(int)nRow - 1, 12].Text = ComFunc.TimeDiffHourMin(dt.Rows[i]["ROutEntDate_NEW"].ToString().Trim(), dt.Rows[i]["SimsaTime"].ToString().Trim());
                    }

                    SS1_Sheet1.Cells[(int)nRow - 1, 13].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "IPD_입원상태", strGbSTS);
                    strAmSet3 = dt.Rows[i]["AmSet3"].ToString().Trim();


                    SS1_Sheet1.Cells[(int)nRow - 1, 14].Text = Convert.ToDouble(dt.Rows[i]["IPDNO"].ToString().Trim()).ToString();
                    SS1_Sheet1.Cells[(int)nRow - 1, 15].Text = Convert.ToDouble(dt.Rows[i]["TRSNO"].ToString().Trim()).ToString();
                    SS1_Sheet1.Cells[(int)nRow - 1, 16].Text = dt.Rows[i]["GbSTS"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 17].Text = dt.Rows[i]["AmSet3"].ToString().Trim();

                    switch (dt.Rows[i]["AMSET5"].ToString().Trim())
                    {

                        case "1":
                            SS1_Sheet1.Cells[(int)nRow - 1, 18].Text = "완쾌";
                            break;
                        case "2":
                            SS1_Sheet1.Cells[(int)nRow - 1, 18].Text = "자의";
                            break;
                        case "3":
                            SS1_Sheet1.Cells[(int)nRow - 1, 18].Text = "사망";
                            break;
                        case "4":
                            SS1_Sheet1.Cells[(int)nRow - 1, 18].Text = "전원";
                            break;
                        case "5":
                            SS1_Sheet1.Cells[(int)nRow - 1, 18].Text = "빈사";
                            break;
                        case "6":
                            SS1_Sheet1.Cells[(int)nRow - 1, 18].Text = "도주";
                            break;
                        case "7":
                            SS1_Sheet1.Cells[(int)nRow - 1, 18].Text = "보관금";
                            break;
                        case "8":
                            SS1_Sheet1.Cells[(int)nRow - 1, 18].Text = "구분변경";
                            break;
                        default:
                            SS1_Sheet1.Cells[(int)nRow - 1, 18].Text = "";
                            break;
                    }
                    SS1_Sheet1.Cells[(int)nRow - 1, 19].Text = strROutFlag;
                    SS1_Sheet1.Cells[(int)nRow - 1, 20].Text = dt.Rows[i]["SunapTime"].ToString().Trim();

                    //이름찾기 -- 사번만 가지고 가면 됨.
                    //SS1_Sheet1.Cells[(int)nRow - 1, 21].Text = CPF.READ_INSA_Name(clsDB.DbCon, dt.Rows[i]["rDRSabun"].ToString().Trim() +" "+ dt.Rows[i]["RDRTime"].ToString().Trim());
                    SS1_Sheet1.Cells[(int)nRow - 1, 21].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["rDRSabun"].ToString().Trim()) + " " + dt.Rows[i]["RDRTime"].ToString().Trim();

                    if (dt.Rows[i]["TRout_Gbn"].ToString().Trim() == "1")
                    {
                        SS1_Sheet1.Cells[(int)nRow - 1, 22].Text = "1.등록";
                    }
                    else if (dt.Rows[i]["TRout_Gbn"].ToString().Trim() == "2")
                    {
                        SS1_Sheet1.Cells[(int)nRow - 1, 22].Text = "2.해지";
                    }

                    SS1_Sheet1.Cells[(int)nRow - 1, 23].Text = dt.Rows[i]["routdate"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 24].Text = dt.Rows[i]["simsaTime"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 25].Text = dt.Rows[i]["SunapTime"].ToString().Trim();
                    SS1_Sheet1.Cells[(int)nRow - 1, 26].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["simsasabun"].ToString().Trim());

                    // '통계계산
                    if (strDeptCode != "")
                    {
                        nn = VB.I(VB.Pstr(FstrDeptCodes, strDeptCode, 1), "^^");
                        nCntDept[(int)nn] = nCntDept[(int)nn] + 1;
                        if (strROutFlag == "Y")
                        {
                            nCntDept2[(int)nn] = nCntDept2[(int)nn] + 1;
                            //'예고수납체크


                            if (strNur_ROutDate == VB.Left(strSunapTime, 10) && string.Compare(VB.Mid(strSunapTime, 12, 5), "10:31") < 0)
                            {

                                nCntDept3[(int)nn] = nCntDept3[(int)nn] + 1;
                            }
                        }
                        //'전체수납체크
                        if (string.Compare(VB.Mid(strSunapTime, 12, 5), "10:31") < 0)
                        {
                            nCntDept4[(int)nn] = nCntDept4[(int)nn] + 1;
                        }
                    }

                    if (strDrcode != "")
                    {
                        nn = VB.I(VB.Pstr(FstrDrCodes, strDrcode, 1), "^^");
                        nCntDrCode[(int)nn] = nCntDrCode[(int)nn] + 1;
                        if (strROutFlag == "Y")
                        {
                            nCntDrCode2[(int)nn] = nCntDrCode2[(int)nn] + 1;
                            //'예고수납체크
                            if (strNur_ROutDate == VB.Left(strSunapTime, 10) && string.Compare(VB.Mid(strSunapTime, 12, 5), "10:31") < 0)
                            {
                                nCntDrCode3[(int)nn] = nCntDrCode3[(int)nn] + 1;
                            }
                        }
                        //'전체수납체크
                        if (string.Compare(VB.Mid(strSunapTime, 12, 5), "10:31") < 0)
                        {
                            nCntDrCode4[(int)nn] = nCntDrCode4[(int)nn] + 1;
                        }
                    }

                    if (strWardcode != "")
                    {
                        nn = VB.I(VB.Pstr(FstrWardCodes, strWardcode, 1), "^^");
                        nCntWardCode[(int)nn] = nCntWardCode[(int)nn] + 1;
                        if (strROutFlag == "Y")
                        {
                            nCntWardCode2[(int)nn] = nCntWardCode2[(int)nn] + 1;
                            //'예고수납체크
                            if (strNur_ROutDate == VB.Left(strSunapTime, 10) && string.Compare(VB.Mid(strSunapTime, 12, 5), "10:31") < 0)
                            {
                                nCntWardCode3[(int)nn] = nCntWardCode3[(int)nn] + 1;
                            }
                        }
                        //'전체수납체크
                        if (string.Compare(VB.Mid(strSunapTime, 12, 5), "10:31") < 0)
                        {
                            nCntWardCode4[(int)nn] = nCntWardCode4[(int)nn] + 1;
                        }
                    }

                }
            }
            dt.Dispose();
            dt = null;

            //'통계

            k = 0;
            M = 0;
            q = 0;

            for (i = 1; i <= 500; i++)
            {
                nSumDept = nSumDept + nCntDept[i];
                nSumDept2 = nSumDept2 + nCntDept2[i];
                nSumDept3 = nSumDept3 + nCntDept3[i];
                nSumDept4 = nSumDept4 + nCntDept4[i];

                if (nCntDept[i] > 0)
                {

                    k = k + 1;

                    strDeptCode = VB.Pstr(FstrDeptCodes, "^^", i);
                    SS_Info_Sheet1.Cells[0, ((k - 1) * 2 + 3) - 1].Text = strDeptCode;
                    SS_Info_Sheet1.Cells[0, (((k - 1) * 2) + 1 + 3) - 1].Text = strDeptCode;
                }

                nSumDrCode = nSumDrCode + nCntDrCode[i];
                nSumDrCode2 = nSumDrCode2 + nCntDrCode2[i];
                nSumDrCode3 = nSumDrCode3 + nCntDrCode3[i];
                nSumDrCode4 = nSumDrCode4 + nCntDrCode4[i];

                if (nCntDrCode[i] > 0)
                {
                    M = M + 1;
                    strDrcode = VB.Pstr(FstrDrCodes, "^^", i);
                    SS_Info_Sheet1.Cells[1, ((M - 1) * 2 + 3) - 1].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, strDrcode);
                    SS_Info_Sheet1.Cells[1, (((M - 1) * 2) + 1 + 3) - 1].Text = nCntDrCode[i].ToString();
                }
                nSumWardCode = nSumWardCode + nCntWardCode[i];
                nSumWardCode2 = nSumWardCode2 + nCntWardCode2[i];
                nSumWardCode3 = nSumWardCode3 + nCntWardCode3[i];
                nSumWardCode4 = nSumWardCode4 + nCntWardCode4[i];

                if (nCntWardCode[i] > 0)
                {
                    q = q + 1;
                    strWardcode = VB.Pstr(FstrWardCodes, "^^", i);
                    SS_Info_Sheet1.Cells[2, ((q - 1) * 2 + 3) - 1].Text = strWardcode;
                    SS_Info_Sheet1.Cells[2, (((q - 1) * 2) + 1 + 3) - 1].Text = nCntWardCode[i].ToString();
                }
            }
            SS_Info_Sheet1.Cells[0, 1].Text = nSumDept.ToString();
            SS_Info_Sheet1.Cells[1, 1].Text = nSumDrCode.ToString();
            SS_Info_Sheet1.Cells[2, 1].Text = nSumWardCode.ToString();

            SS_Info_Sheet1.ColumnCount = SS_Info_Sheet1.GetLastNonEmptyColumn(NonEmptyItemFlag.Data) + 1;

            // '각항목별 표

            SS100_Sheet1.Cells[0, 0, SS100_Sheet1.RowCount - 1, SS100_Sheet1.ColumnCount - 1].Text = "";
            SS101_Sheet1.Cells[0, 0, SS101_Sheet1.RowCount - 1, SS101_Sheet1.ColumnCount - 1].Text = "";
            SS102_Sheet1.Cells[0, 0, SS102_Sheet1.RowCount - 1, SS102_Sheet1.ColumnCount - 1].Text = "";


            k = 0;  //'nSumDept = 0

            SS100_Sheet1.RowCount = 500;
            for (i = 1; i <= 500; i++)
            {
                //'nSumDept = nSumDept + nCntDept(i)
                if (nCntDept[i] > 0)
                {
                    k = k + 1;
                    strDeptCode = VB.Pstr(FstrDeptCodes, "^^", i);
                    //.Row = k
                    SS100_Sheet1.Cells[k - 1, 0].Text = strDeptCode;
                    SS100_Sheet1.Cells[k - 1, 1].Text = nCntDept[i].ToString();
                    SS100_Sheet1.Cells[k - 1, 2].Text = nCntDept2[i].ToString();

                    if (chkROut.Checked == false)
                    {
                        if (nCntDept2[i] == 0)
                        {
                            SS100_Sheet1.Cells[k - 1, 3].Text = "0";
                        }
                        else
                        {
                            SS100_Sheet1.Cells[k - 1, 3].Text = (nCntDept2[i] / nCntDept[i] * 100).ToString("###.0");
                        }
                    }
                    SS100_Sheet1.Cells[k - 1, 4].Text = nCntDept3[i].ToString();

                    if (chkROut.Checked == false)
                    {
                        if (nCntDept3[i] == 0)
                        {
                            SS100_Sheet1.Cells[k - 1, 5].Text = "";
                        }
                        else
                        {
                            SS100_Sheet1.Cells[k - 1, 5].Text = VB.Format((nCntDept3[i] / nCntDept2[i] * 100), "###.0");
                        }
                    }

                    SS100_Sheet1.Cells[k - 1, 6].Text = nCntDept4[i].ToString();

                    if (nCntDept4[i] == 0)
                    {
                        SS100_Sheet1.Cells[k - 1, 7].Text = "0";
                    }
                    else
                    {
                        SS100_Sheet1.Cells[k - 1, 7].Text = VB.Format((nCntDept4[i] / nCntDept[i] * 100), "###.0");
                    }
                }
            }
            SS100_Sheet1.RowCount = SS100_Sheet1.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 2;

            SS100_Sheet1.Cells[SS100_Sheet1.RowCount - 1, 0].Text = "합계";
            SS100_Sheet1.Cells[SS100_Sheet1.RowCount - 1, 1].Text = nSumDept.ToString();
            SS100_Sheet1.Cells[SS100_Sheet1.RowCount - 1, 2].Text = nSumDept2.ToString();

            if (chkROut.Checked == false)
            {
                if (nSumDept2 == 0)
                {
                    SS100_Sheet1.Cells[SS100_Sheet1.RowCount - 1, 3].Text = "0";
                }
                else
                {
                    SS100_Sheet1.Cells[SS100_Sheet1.RowCount - 1, 3].Text = VB.Format((nSumDept2 / nSumDept * 100), "###.0");
                }
            }
            SS100_Sheet1.Cells[SS100_Sheet1.RowCount - 1, 4].Text = nSumDept3.ToString();

            if (chkROut.Checked == false)
            {
                if (nSumDept3 == 0)
                {
                    SS100_Sheet1.Cells[SS100_Sheet1.RowCount - 1, 5].Text = "";
                }
                else
                {
                    SS100_Sheet1.Cells[SS100_Sheet1.RowCount - 1, 5].Text = VB.Format((nSumDept3 / nSumDept2 * 100), "###.0");
                }
            }

            SS100_Sheet1.Cells[SS100_Sheet1.RowCount - 1, 6].Text = nSumDept4.ToString();

            if (nSumDept4 == 0)
            {
                SS100_Sheet1.Cells[SS100_Sheet1.RowCount - 1, 7].Text = "0";
            }
            else
            {
                SS100_Sheet1.Cells[SS100_Sheet1.RowCount - 1, 7].Text = VB.Format((nSumDept4 / nSumDept * 100), "###.0");
            }

            //SS101
            k = 0;

            SS101_Sheet1.RowCount = 500;
            for (i = 1; i <= 500; i++)
            {
                //'nSumDept = nSumDept + nCntDept(i)
                if (nCntDrCode[i] > 0)
                {
                    k = k + 1;
                    strDrcode = VB.Pstr(FstrDrCodes, "^^", i);
                    //.Row = k
                    SS101_Sheet1.Cells[k - 1, 0].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, strDrcode);
                    SS101_Sheet1.Cells[k - 1, 1].Text = nCntDrCode[i].ToString();
                    SS101_Sheet1.Cells[k - 1, 2].Text = nCntDrCode2[i].ToString();

                    if (chkROut.Checked == false)
                    {
                        if (nCntDrCode2[i] == 0)
                        {
                            SS101_Sheet1.Cells[k - 1, 3].Text = "0";
                        }
                        else
                        {
                            SS101_Sheet1.Cells[k - 1, 3].Text = (nCntDrCode2[i] / nCntDrCode[i] * 100).ToString("###.0");
                        }
                    }
                    SS101_Sheet1.Cells[k - 1, 4].Text = nCntDrCode3[i].ToString();

                    if (chkROut.Checked == false)
                    {
                        if (nCntDrCode3[i] == 0)
                        {
                            SS101_Sheet1.Cells[k - 1, 5].Text = "";
                        }
                        else
                        {
                            SS101_Sheet1.Cells[k - 1, 5].Text = VB.Format((nCntDrCode3[i] / nCntDrCode2[i] * 100), "###.0");
                        }
                    }

                    SS101_Sheet1.Cells[k - 1, 6].Text = nCntDrCode4[i].ToString();

                    if (nCntDrCode4[i] == 0)
                    {
                        SS101_Sheet1.Cells[k - 1, 7].Text = "0";
                    }
                    else
                    {
                        SS101_Sheet1.Cells[k - 1, 7].Text = VB.Format((nCntDrCode4[i] / nCntDrCode[i] * 100), "###.0");
                    }
                }
            }
            SS101_Sheet1.RowCount = SS101_Sheet1.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 2;

            SS101_Sheet1.Cells[SS101_Sheet1.RowCount - 1, 0].Text = "합계";
            SS101_Sheet1.Cells[SS101_Sheet1.RowCount - 1, 1].Text = nSumDrCode.ToString();
            SS101_Sheet1.Cells[SS101_Sheet1.RowCount - 1, 2].Text = nSumDrCode2.ToString();

            if (chkROut.Checked == false)
            {
                if (nSumDrCode2 == 0)
                {
                    SS101_Sheet1.Cells[SS101_Sheet1.RowCount - 1, 3].Text = "0";
                }
                else
                {
                    SS101_Sheet1.Cells[SS101_Sheet1.RowCount - 1, 3].Text = VB.Format((nSumDrCode2 / nSumDrCode * 100), "###.0");
                }
            }
            SS101_Sheet1.Cells[SS101_Sheet1.RowCount - 1, 4].Text = nSumDrCode3.ToString();

            if (chkROut.Checked == false)
            {
                if (nSumDrCode3 == 0)
                {
                    SS101_Sheet1.Cells[SS101_Sheet1.RowCount - 1, 5].Text = "";
                }
                else
                {
                    SS101_Sheet1.Cells[SS101_Sheet1.RowCount - 1, 5].Text = VB.Format((nSumDrCode3 / nSumDrCode2 * 100), "###.0");
                }
            }

            SS101_Sheet1.Cells[SS101_Sheet1.RowCount - 1, 6].Text = nSumDrCode4.ToString();

            if (nSumDrCode4 == 0)
            {
                SS101_Sheet1.Cells[SS101_Sheet1.RowCount - 1, 7].Text = "0";
            }
            else
            {
                SS101_Sheet1.Cells[SS101_Sheet1.RowCount - 1, 7].Text = VB.Format((nSumDrCode4 / nSumDrCode * 100), "###.0");
            }

            //SS102
            k = 0;

            SS102_Sheet1.RowCount = 500;
            for (i = 1; i <= 500; i++)
            {
                //'nSumDept = nSumDept + nCntDept(i)
                if (nCntWardCode[i] > 0)
                {
                    k = k + 1;
                    strWardcode = VB.Pstr(FstrDeptCodes, "^^", i);
                    //.Row = k
                    SS102_Sheet1.Cells[k - 1, 0].Text = strWardcode;
                    SS102_Sheet1.Cells[k - 1, 1].Text = nCntWardCode[i].ToString();
                    SS102_Sheet1.Cells[k - 1, 2].Text = nCntWardCode2[i].ToString();

                    if (chkROut.Checked == false)
                    {
                        if (nCntWardCode2[i] == 0)
                        {
                            SS102_Sheet1.Cells[k - 1, 3].Text = "0";
                        }
                        else
                        {
                            SS102_Sheet1.Cells[k - 1, 3].Text = (nCntWardCode2[i] / nCntWardCode[i] * 100).ToString("###.0");
                        }
                    }
                    SS102_Sheet1.Cells[k - 1, 4].Text = nCntWardCode3[i].ToString();

                    if (chkROut.Checked == false)
                    {
                        if (nCntWardCode3[i] == 0)
                        {
                            SS102_Sheet1.Cells[k - 1, 5].Text = "";
                        }
                        else
                        {
                            SS102_Sheet1.Cells[k - 1, 5].Text = VB.Format((nCntWardCode3[i] / nCntWardCode2[i] * 100), "###.0");
                        }
                    }

                    SS102_Sheet1.Cells[k - 1, 6].Text = nCntWardCode4[i].ToString();

                    if (nCntWardCode4[i] == 0)
                    {
                        SS102_Sheet1.Cells[k - 1, 7].Text = "0";
                    }
                    else
                    {
                        SS102_Sheet1.Cells[k - 1, 7].Text = VB.Format((nCntWardCode4[i] / nCntWardCode[i] * 100), "###.0");
                    }
                }
            }
            SS102_Sheet1.RowCount = SS102_Sheet1.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 2;

            SS102_Sheet1.Cells[SS102_Sheet1.RowCount - 1, 0].Text = "합계";
            SS102_Sheet1.Cells[SS102_Sheet1.RowCount - 1, 1].Text = nSumWardCode.ToString();
            SS102_Sheet1.Cells[SS102_Sheet1.RowCount - 1, 2].Text = nSumWardCode2.ToString();

            if (chkROut.Checked == false)
            {
                if (nSumWardCode2 == 0)
                {
                    SS102_Sheet1.Cells[SS102_Sheet1.RowCount - 1, 3].Text = "0";
                }
                else
                {
                    SS102_Sheet1.Cells[SS102_Sheet1.RowCount - 1, 3].Text = VB.Format((nSumWardCode2 / nSumWardCode * 100), "###.0");
                }
            }
            SS102_Sheet1.Cells[SS102_Sheet1.RowCount - 1, 4].Text = nSumWardCode3.ToString();

            if (chkROut.Checked == false)
            {
                if (nSumWardCode3 == 0)
                {
                    SS102_Sheet1.Cells[SS102_Sheet1.RowCount - 1, 5].Text = "";
                }
                else
                {
                    SS102_Sheet1.Cells[SS102_Sheet1.RowCount - 1, 5].Text = VB.Format((nSumWardCode3 / nSumWardCode2 * 100), "###.0");
                }
            }

            SS102_Sheet1.Cells[SS102_Sheet1.RowCount - 1, 6].Text = nSumWardCode4.ToString();

            if (nSumWardCode4 == 0)
            {
                SS102_Sheet1.Cells[SS102_Sheet1.RowCount - 1, 7].Text = "0";
            }
            else
            {
                SS102_Sheet1.Cells[SS102_Sheet1.RowCount - 1, 7].Text = VB.Format((nSumWardCode4 / nSumWardCode * 100), "###.0");
            }

            Cursor.Current = Cursors.Default;
            btnSearch.Enabled = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (superTabControl1.SelectedTabIndex == 0)
            {
                eSearCh_Ward();
            }
            else
            {
                eSearCh_Rec();
            }
        }

        private void cboDept_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (cboDept.Text.Trim() != "" && cboDept.Text.Trim() != "전체")
            {
                try
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT DrCode ";
                    SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                    SQL = SQL + ComNum.VBLF + "      WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "          AND DrDept1 ='" + cboDept.Text + "' ";
                    SQL = SQL + ComNum.VBLF + "          AND DRCODE IS NOT NULL ";
                    SQL = SQL + ComNum.VBLF + "ORDER  BY DrCode ";

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

                    cboDr.Items.Clear();
                    cboDr.Items.Add("전체");
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDr.Items.Add(clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim()) + "." + dt.Rows[i]["DrCode"].ToString().Trim());
                    }
                    cboDr.SelectedIndex = 0;

                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }

                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }
        }

        private void btnTongView_Click(object sender, EventArgs e)
        {
            panTong.Visible = true;
        }

        private void btnsubClsose_Click(object sender, EventArgs e)
        {
            panTong.Visible = false;
        }
    }
}

