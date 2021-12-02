using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaMagamDailyCheck.cs
    /// Description     : 일일점검
    /// Author          : 김해수
    /// Create Date     : 2018-09-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "\IPD\iument\Frm일일점검.frm(Frm일일점검) >> frmPmpaMagamDailyCheck.cs 폼이름 재정의" />
    public partial class frmPmpaMagamDailyCheck : Form
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();
        clsComPmpaSQL cSQL = new clsComPmpaSQL();
        clsComPmpaSpd MagamSpd = new clsComPmpaSpd();
        clsPmpaMisu cPM = new clsPmpaMisu();
        clsPmpaFunc cPF = new clsPmpaFunc();
       
        

        #endregion

        #region //MainFormMessage
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion
        public frmPmpaMagamDailyCheck()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaMagamDailyCheck(MainFormMessage pform) 
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        void setCtrlData()
        {
            setCombo();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            ////탭버튼클릭 이벤트
            //this.tab1.Click += new EventHandler(eTabClick);
            //this.tab2.Click += new EventHandler(eTabClick);
            //this.tab3.Click += new EventHandler(eTabClick);
            //this.btnView.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            //this.btnReView.Click += new EventHandler(eBtnClick);
            //this.btnHelp.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnPrint);
            

            //this.optD.CheckedChanged += new EventHandler(eRdoChecked);
            //this.optM.CheckedChanged += new EventHandler(eRdoChecked);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnSearch2.Click += new EventHandler(eBtnSearch);
            //this.btnSearch5.Click += new EventHandler(eBtnSearch);
            //this.btnSearch6.Click += new EventHandler(eBtnSearch);
            //this.btnSearch8.Click += new EventHandler(eBtnSearch);
            //this.btnSearch9.Click += new EventHandler(eBtnSearch);

            //this.btnSave1.Click += new EventHandler(eBtnSave);
            //this.btnSave2.Click += new EventHandler(eBtnSave);
            //this.btnSave3.Click += new EventHandler(eBtnSave);
            //this.btnSave5.Click += new EventHandler(eBtnSave);
            //this.btnSave_Session.Click += new EventHandler(eBtnSave);
            //this.btnMemo.Click += new EventHandler(eBtnSave);

            //this.btnPrint.Click += new EventHandler(eBtnPrint);  //스프레드 방식 출력
            //this.btnPrint2.Click += new EventHandler(eBtnPrint);
            //this.btnPrint3.Click += new EventHandler(eBtnPrint); //Print.print 출력방식


            ////명단 더블클릭 이벤트
            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            ////this.ssList.MouseUp += new MouseEventHandler(eMouseEvent);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            //this.ssList.EditChange += new EditorNotifyEventHandler(eSpreadSelEditChanged);
            //this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            //this.ssList.EditModeOff += new EventHandler(eSpreadEditOff);
            //this.ssList.LostFocus += new EventHandler(eSelLsostFocus);
            this.ssList.EditModeOff += new EventHandler(eSpdChange);

            //this.cboGubun.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboPart.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboAmPm.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboIO.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboSleep.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            //this.txtSearch.KeyDown += new KeyEventHandler(eTxtKeyDown);
            //this.dtpDate.ValueChanged += new EventHandler(eDtpValueChanged);
        }

        void setCombo()
        {

        }

        void setTxtTip()
        {

        }

        void setCtrlInit()
        {
            //clsCompuInfo.SetComputerInfo();
            //DataTable dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프로그램위치", "접수프로그램위치_내시경실");

            //if (ComFunc.isDataTableNull(dt) == false)
            //{
            //    //설정세팅
            //}
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {

                MagamSpd.sSpd_enmDailyCheck_sel(ssList, MagamSpd.senmDailyCheck_sel, MagamSpd.nenmDailyCheck_sel, 1, 0);
                //툴팁
                setTxtTip();

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();
            }
        }

        void eFormResize(object sender, EventArgs e)
        {
            //setCtrlProgress();
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eSpdChange(object sender, EventArgs e)
        {
           
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                screen_display();
            }
            else if (sender == this.btnSearch2)
            {
                screen_display("sunsunap");
            }
            else if (sender == this.btnTest)
            {
          
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            if (Job == "")
            {
                dtpDate.Text = cpublic.strSysDate;
                btnTest.Visible = false;
                ssListChk1.ActiveSheet.Columns[3].Visible = false;
                ssListBun.Visible = false;
            }
            else if (Job == "Clear")
            {
                //cboYYMM.Focus();
                //btnView.Enabled = true;
                ////gbIO.Enabled = true;
                //gbYYMM.Enabled = true;
                //gbJong.Enabled = true;
                //ssList.ActiveSheet.RowCount = 0;
                //ssList.ActiveSheet.RowCount = 1;
                ////// 화면상의 정렬표시 Clear ( Sort 정렬 아이콘 클리어)
                //ssList.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssList.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }
        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display(string Gubun ="")
        {
            if (Gubun == "")
            {
                GetData(clsDB.DbCon, ssList);
            }else if(Gubun == "sunsunap")
            {
                ssList.ActiveSheet.RowCount = 0;
                SunSuNap_II(clsDB.DbCon, ssList);
                ComFunc.MsgBox("점검 작업완료!!");
            }
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = null;
            DataTable rs = null;
            DataTable rs1 = null;
            string strDate = "", strPano = "", strDept="", strDrcode="", strPart="";
            string strOK = "", strBun = "", strNu = "", strTemp = "";
            string strCHK = "", strInDate = "", strOutDate = "";
            int i = 0, j = 0, nCNT = 0, OAmt = 0, CAmt = 0;
            long nAmtsum = 0, nIPDNO = 0, nAmt1 = 0, nAmt2 = 0, nTRSNo = 0;
            double nQty = 0;

            int intRowAffected = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            strDate = dtpDate.Text.Trim();
            Spd.ActiveSheet.RowCount = 0;

            //Cursor.Current = Cursors.WaitCursor;

            if (tabControl1.SelectedTab == SSTab1)
            {
                #region 점검 
                #region 점검 1 전체 부분
                ssListChk1.ActiveSheet.Cells[0, 0].Text = "";
                lblSearch.Text = "[ 점검 1 ] * 점검중 *";
                Application.DoEvents();
                #region 점검1               

                dt = cSQL.sel_DailyCheck_JumGum1(pDbCon);//점검1쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();

                    rs = cSQL.sel_DailyCheck_JumGum1_1(pDbCon, dt.Rows[i]["IPDNO"].ToString().Trim(), dt.Rows[i]["TRSNO"].ToString().Trim());//점검1_1쿼리
                    switch (rs.Rows.Count)
                    {
                        case 0:
                            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검1 : IPD_NEW_MASTER에 자료 없습니다.";
                            //ROW 높이 설정
                            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                            break;
                        case 1:
                            if (rs.Rows[0]["GBSTS"].ToString().Trim() != "0")
                            {
                                Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                                Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = rs.Rows[0]["PANO"].ToString().Trim();
                                Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = rs.Rows[0]["SNAME"].ToString().Trim();
                                Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = rs.Rows[0]["INDATE"].ToString().Trim();
                                Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검1:TRANS(재원)  MASTER(퇴원) ";
                                //ROW 높이 설정
                                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                            }
                            break;
                    }

                    rs.Dispose();
                    rs = null;
                }

                dt.Dispose();
                dt = null;

                dt = cSQL.sel_DailyCheck_JumGum1_2(pDbCon, strDate);//점검1_2쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검1 : IPD_TRANS 지병 퇴원날짜공란";
                }

                dt.Dispose();
                dt = null;

                //IPD_NEW_MASTER OUTDATE 없는데 GBSTS 값이 0 아니면 
                dt = cSQL.sel_DailyCheck_JumGum1_3(pDbCon); //점검1_3쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검1:MASTER(퇴원중) - 퇴원일자없음 ";
                    //ROW 높이 설정
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                }

                dt.Dispose();
                dt = null;

                //IPD_NEW_MASTER OUTDATE 없는데 GBSTS 값이 0 아니면
                dt = cSQL.sel_DailyCheck_JumGum1_4(pDbCon); //점검1_4쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검1 : MASTER(퇴원) - 퇴원일자없음 ";
                    //ROW 높이 설정
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                }

                dt.Dispose();
                dt = null;



                #endregion
                ssListChk1.ActiveSheet.Cells[0, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 2 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 2 전체 부분
                ssListChk1.ActiveSheet.Cells[1, 0].Text = "";
                #region 점검2
                strCHK = "OK";

                dt = cSQL.sel_DailyCheck_JumGum2(pDbCon, strDate);//점검2쿼리

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rs = cSQL.sel_DailyCheck_JumGum2_1(pDbCon, dt.Rows[i]["IPDNO"].ToString().Trim());//점검2_1쿼리

                        for (j = 0; j < rs.Rows.Count; j++)
                        {
                            if (rs.Rows[j]["GBSTS"].ToString().Trim() != "7")
                            {
                                strCHK = "NO";
                            }
                        }

                        rs.Dispose();
                        rs = null;

                        if (strCHK == "NO")
                        {
                            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검2 : MASTER(퇴원) TRANS(퇴원아님)";
                            //ROW 높이 설정
                            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //2015-01-26
                dt = cSQL.sel_DailyCheck_JumGum2_2(pDbCon);//점검2_2쿼리

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검2 : 퇴원상태인데, 퇴원일자가 없음";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[1, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 3 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 3 전체 부분            
                ssListChk1.ActiveSheet.Cells[2, 0].Text = "";
                #region 점검3
                int nY85 = 0, nY87 = 0, nY88 = 0;

                dt = cSQL.sel_DailyCheck_JumGum3(pDbCon, strDate);//점검3쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum3_1(pDbCon, dt.Rows[i]["IPDNO"].ToString().Trim());//점검3_1쿼리

                    for (j = 0; j < rs.Rows.Count; j++)
                    {
                        switch (rs.Rows[j]["BUN"].ToString().Trim())
                        {
                            case "85":
                                nY85 = Convert.ToInt32(VB.Val(rs.Rows[j]["AMT"].ToString().Trim()));
                                break;
                            case "87":
                                nY87 = Convert.ToInt32(VB.Val(rs.Rows[j]["AMT"].ToString().Trim()));
                                break;
                            case "88":
                                nY88 = Convert.ToInt32(VB.Val(rs.Rows[j]["AMT"].ToString().Trim()));
                                break;
                        }
                    }

                    rs.Dispose();
                    rs = null;

                    if (nY85 + nY87 - nY88 != 0)
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검3: 중간납+보증금 대체금액 다름.";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    nY85 = 0; nY87 = 0; nY88 = 0;
                }

                dt.Dispose();
                dt = null;
                #endregion
                ssListChk1.ActiveSheet.Cells[2, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 4 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 4 전체 부분
                ssListChk1.ActiveSheet.Cells[3, 0].Text = "";
                #region 점검4

                dt = cSQL.sel_DailyCheck_JumGum4(pDbCon, strDate);//점검4쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum4_1(pDbCon, dt.Rows[i]["TRSNO"].ToString().Trim());//점검4_1쿼리

                    if (dt.Rows[i]["AMT50"].ToString().Trim() != rs.Rows[0]["AMT"].ToString().Trim())
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검4 : 계정금액과 SLIP금액 차액";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[3, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 5 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 5 전체 부분
                ssListChk1.ActiveSheet.Cells[4, 0].Text = "";
                #region 점검5
                dt = cSQL.sel_DailyCheck_JumGum5(pDbCon, strDate);//점검5쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum5_1(pDbCon, dt.Rows[i]["TRSNO"].ToString().Trim());//점검5_1쿼리

                    if (rs.Rows.Count == 0)
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검5: Y88 계정 발생함";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[4, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 6 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 6 전체 부분
                ssListChk1.ActiveSheet.Cells[5, 0].Text = "";
                #region 점검6

                dt = cSQL.sel_DailyCheck_JumGum6(pDbCon, strDate);//점검6쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["OUTDATE"].ToString().Trim() == "")
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검6 : 퇴원일자 없습니다.";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[5, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 7 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 7 전체 부분
                ssListChk1.ActiveSheet.Cells[6, 0].Text = "";
                #region 점검7
                dt = cSQL.sel_DailyCheck_JumGum7(pDbCon, strDate);//점검7쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum7_1(pDbCon, dt.Rows[i]["LASTTRS"].ToString().Trim());//점검7_1쿼리

                    if (dt.Rows[i]["DRCODE"].ToString().Trim() != rs.Rows[0]["DRCODE"].ToString().Trim())
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검7  : 진료의사명이 다름. ";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;

                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[6, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 8 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 8 전체 부분
                ssListChk1.ActiveSheet.Cells[7, 0].Text = "";
                #region 점검8

                dt = cSQL.sel_DailyCheck_JumGum8(pDbCon, strDate);//점검8쿼리

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rs = cSQL.sel_DailyCheck_JumGum8_1(pDbCon, dt.Rows[i]["TRSNO"].ToString().Trim());//점검8_1쿼리

                        if (rs.Rows.Count > 0)
                        {
                            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검8 : 일반가격임 조합부담금 발생함";
                            //ROW 높이 설정
                            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                        }

                        rs.Dispose();
                        rs = null;

                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[7, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 9 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 9 전체 부분
                ssListChk1.ActiveSheet.Cells[8, 0].Text = "";
                #region 점검9
                dt = cSQL.sel_DailyCheck_JumGum9(pDbCon, strDate);//점검9쿼리
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rs = cSQL.sel_DailyCheck_JumGum9_1(pDbCon, dt.Rows[i]["TRSNO"].ToString().Trim());//점검9_1쿼리

                        if (rs.Rows.Count > 0)
                        {
                            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검9 : 퇴원일자가 없음";
                            //ROW 높이 설정
                            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                        }

                        rs.Dispose();
                        rs = null;

                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[8, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 10 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 10 전체 부분
                ssListChk1.ActiveSheet.Cells[9, 0].Text = "";
                #region 점검 10
                dt = cSQL.sel_DailyCheck_JumGum10(pDbCon, strDate);//점검10쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum10_1(pDbCon, dt.Rows[i]["PANO"].ToString().Trim(), strDate, CF.DATE_ADD(clsDB.DbCon, strDate, 1));//점검10_1쿼리

                    for (j = 0; j < rs.Rows.Count; j++)
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = rs.Rows[j]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = rs.Rows[j]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "예약일자 : " + rs.Rows[j]["YDATE"].ToString().Trim() + " " + rs.Rows[j]["YTIME"].ToString().Trim() + " 예약변경";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;

                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[9, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 11 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 11 전체 부분
                ssListChk1.ActiveSheet.Cells[10, 0].Text = "";
                #region 점검 11

                dt = cSQL.sel_DailyCheck_JumGum11(pDbCon, strDate);//점검11쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검11 : 누적코드에 공백이 발생함.";
                    //ROW 높이 설정
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[10, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 12 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 12 전체 부분
                ssListChk1.ActiveSheet.Cells[11, 0].Text = "";
                #region 점검 12

                dt = cSQL.sel_DailyCheck_JumGum12(pDbCon, strDate);//점검12쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["pano"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검12 : 상한금액이 있습니다.. 확인!";
                    //ROW 높이 설정
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[11, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 13 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 13 전체 부분
                ssListChk1.ActiveSheet.Cells[12, 0].Text = "";
                #region 점검 13
                dt = cSQL.sel_DailyCheck_JumGum13(pDbCon, strDate);//점검13쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum13_1(pDbCon, dt.Rows[i]["Trsno"].ToString().Trim(), dt.Rows[i]["BI"].ToString().Trim());//점검13_1쿼리

                    if (rs.Rows.Count == 0)
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                       // Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "발생일자 : " + strDate + " " + dt.Rows[i]["TRSNO"].ToString().Trim() + " 자른자격과 NEW_SLIP의 자격이 틀림.";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;

                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[12, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 14 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 14 전체 부분
                ssListChk1.ActiveSheet.Cells[13, 0].Text = "";
                #region 점검 14
                dt = cSQL.sel_DailyCheck_JumGum14(pDbCon, strDate);//점검14쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum14_1(pDbCon, dt.Rows[i]["Trsno"].ToString().Trim(), dt.Rows[i]["BI"].ToString().Trim(), strDate);//점검14_1쿼리

                    if (rs.Rows.Count > 0)
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.14>== 발생일자 : " + strDate + " " + dt.Rows[i]["TRSNO"].ToString().Trim() + " 퇴원일보다 큰 입원 SLIP이 있습니다...";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;

                }

                dt.Dispose();
                dt = null;

                dt = cSQL.sel_DailyCheck_JumGum14_2(pDbCon, cpublic.strSysDate);//점검14_2쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum14_3(pDbCon, dt.Rows[i]["SNAME"].ToString().Trim(), cpublic.strSysDate);//점검14_3쿼리

                    if (rs.Rows[0]["TAmt"].ToString().Trim() != "0")
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.14B>== 발생일자 :" + strDate + " " + dt.Rows[i]["TRSNO"].ToString().Trim() + " 당일보다 큰 입원 SLIP이 있습니다...";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[13, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 15 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 15 전체 부분
                ssListChk1.ActiveSheet.Cells[14, 0].Text = "";
                #region 점검 15

                dt = cSQL.sel_DailyCheck_JumGum15(pDbCon, strDate);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strInDate = dt.Rows[i]["InDate"].ToString().Trim();
                    strOutDate = dt.Rows[i]["OutDate"].ToString().Trim();

                    rs = cSQL.sel_DailyCheck_JumGum15_1(pDbCon, dt.Rows[i]["Trsno"].ToString().Trim(), dt.Rows[i]["IPDNO"].ToString().Trim(), strInDate, strOutDate);

                    if (VB.Val(rs.Rows[0]["TAMT"].ToString().Trim()) != 0)
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.15>== 발생일자 " + strDate + " " + dt.Rows[i]["TRSNO"].ToString().Trim() + " 자격기간외에 금액 발생함...";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;

                }

                dt.Dispose();
                dt = null;

                //당일 - 처방 발생체크 및 - 금액체크

                dt = cSQL.sel_DailyCheck_JumGum15_2(pDbCon, strDate);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = "";
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.15>== 발생일자 : " + strDate + " " + dt.Rows[i]["TRSNO"].ToString().Trim() + " 처방기준 - 금액발생!!..";
                    //ROW 높이 설정
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[14, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 16 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 16 전체 부분
                ssListChk1.ActiveSheet.Cells[15, 0].Text = "";
                #region 점검 16

                dt = cSQL.sel_DailyCheck_JumGum16(pDbCon);//점검16쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = "";
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.16>== " + dt.Rows[i]["Pano"].ToString().Trim() + " 퇴원처리되었는데 퇴원일자 공란 발생함...";
                    //ROW 높이 설정
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                }

                dt.Dispose();
                dt = null;

                //입원 slip 당일전송중 Bdate, ActDate 점검

                dt = cSQL.sel_DailyCheck_JumGum16_1(pDbCon, strDate);//점검16_1쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum16_2(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["IPDNO"].ToString().Trim(), strDate);//점검16_2쿼리

                    if (rs.Rows.Count > 0)
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = "";
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.16>== " + dt.Rows[i]["Pano"].ToString().Trim() + " 발생일자와 회꼐일자가 이상합니다..";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;
                }

                dt.Dispose();
                dt = null;

                //퇴원일자 점검 -IPD_NEW_MASTER, IPD_TRANS
                dt = cSQL.sel_DailyCheck_JumGum16_3(pDbCon);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = "";
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.16>== " + dt.Rows[i]["Pano"].ToString().Trim() + " 마스타, 트랜스의 퇴원일자가 이상합니다..확인요망";
                    //ROW 높이 설정
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[15, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 17 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 17 전체 부분
                ssListChk1.ActiveSheet.Cells[16, 0].Text = "";
                #region 점검17
                dt = cSQL.sel_DailyCheck_JumGum17(pDbCon, strDate);//점검17쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum17_1(pDbCon, strDate, dt.Rows[i]["PANO"].ToString().Trim());//점검17_1쿼리

                    if (rs.Rows.Count == 0)
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.17>== " + dt.Rows[i]["Pano"].ToString().Trim() + " 당일 입원자 OPD_MASTER REP=# 누락건..확인요망";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[16, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 18 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 18 전체 부분
                ssListChk1.ActiveSheet.Cells[17, 0].Text = "";
                #region 점검18
                dt = cSQL.sel_DailyCheck_JumGum18(pDbCon, strDate);//점검18쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = strDate;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.18>== " + dt.Rows[i]["Pano"].ToString().Trim() + " " + dt.Rows[i]["Sunext"].ToString().Trim() + " 계약처환자 - 감액환인건!!!";
                    //ROW 높이 설정
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[17, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 19 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 19 전체 부분
                ssListChk1.ActiveSheet.Cells[18, 0].Text = "";
                #region 점검19
                //현금영수 30만 이상 의무사항 점검
                dt = cSQL.sel_DailyCheck_JumGum19(pDbCon, strDate);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    OAmt = VB.Fix(Convert.ToInt32(VB.Val(dt.Rows[i]["AMT"].ToString().Trim())) / 10) * 10;

                    rs = cSQL.sel_DailyCheck_JumGum19_1(pDbCon, strDate, dt.Rows[i]["PANO"].ToString().Trim());

                    CAmt = Convert.ToInt32(VB.Val(rs.Rows[0]["CardAMT"].ToString().Trim()));

                    rs.Dispose();
                    rs = null;

                    if (OAmt > CAmt)
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = "";
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = "";
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.19>== 입원발생금 : " + OAmt + " 카드+현금승인 :" + CAmt;
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[18, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 20 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 20 전체 부분
                ssListChk1.ActiveSheet.Cells[19, 0].Text = "";
                #region 점검20
                dt = cSQL.sel_DailyCheck_JumGum20(pDbCon, strDate, CF.DATE_ADD(clsDB.DbCon, strDate, -2));

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    strNu = dt.Rows[i]["Nu"].ToString().Trim();

                    strOK = "";

                    for (j = 0; j < ssListBun_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); j++)
                    {
                        strTemp = "";
                        strTemp = ssListBun.ActiveSheet.Cells[j, 0].Text.Trim();
                        strTemp = strTemp + ssListBun.ActiveSheet.Cells[j, 1].Text.Trim();

                        if (strBun + strNu == strTemp)
                        {
                            strOK = "OK";
                            break;
                        }
                    }

                    if (VB.Len(strBun + strNu) != 4)
                    {
                        strOK = "";
                    }

                    if (strOK == "")
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = "";
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = "";
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = strDate;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = "==<점검.20>== 입원 slip 의 분류(" + strBun + ") 와 누적(" + strNu + ")을 확인바랍니다.";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[19, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 21 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 21 전체 부분
                ssListChk1.ActiveSheet.Cells[20, 0].Text = "";
                #region 점검21
                dt = cSQL.sel_DailyCheck_JumGum21(pDbCon, CF.DATE_ADD(clsDB.DbCon, strDate, -1), CF.DATE_ADD(clsDB.DbCon, strDate, 1));//점검21쿼리

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = "";
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = "";
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = strDate;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.21>== 외국인 일반수가 대상구분인데 보험종류가 51이 아닙니다..";
                    //ROW 높이 설정
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                }

                dt.Dispose();
                dt = null;
                #endregion
                ssListChk1.ActiveSheet.Cells[20, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 22 ] * 점검중 *";
                Application.DoEvents();
                #endregion
           
                #region 점검 22 전체 부분
                ssListChk1.ActiveSheet.Cells[21, 0].Text = "";
                #region 점검22
                //2013-04-24 타과전문의 진료시 진찰료 점검추가 해야함..
                //ER에서 입원시 21종 급여나 차상위 E,F 자격도 진찰료 점검추가 해야함...

                dt = cSQL.sel_DailyCheck_JumGum22(pDbCon, strDate, CF.DATE_ADD(clsDB.DbCon, strDate, -1));

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if(dt.Rows[i]["Pano"].ToString().Trim() == "06622008")
                    {
                        i = i;
                    }

                    strInDate = dt.Rows[i]["InDate"].ToString().Trim();

                    rs = cSQL.sel_DailyCheck_JumGum22_1(pDbCon, strInDate, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["DeptCode"].ToString().Trim());

                    strOK = "";
                    
                    if(rs.Rows.Count > 0)
                    {
                        strOK = "OK";
                    }

                    rs.Dispose();
                    rs = null;

                    if (strOK == "OK")
                    {
                        rs = cSQL.sel_DailyCheck_JumGum22_2(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), strInDate, strDate, dt.Rows[i]["DeptCode"].ToString().Trim());

                        nCNT = Convert.ToInt32(VB.Val(rs.Rows[0]["Cnt"].ToString().Trim()));
                        nAmtsum = Convert.ToInt32(VB.Val(rs.Rows[0]["Amt"].ToString().Trim()));

                        rs.Dispose();
                        rs = null;

                        if(dt.Rows[i]["DeptCode"].ToString().Trim() == "NP" && dt.Rows[i]["Bi"].ToString().Trim() == "21")
                        {
                            nAmtsum = 1;
                        }

                        if(nCNT == 0)
                        {
                            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = strInDate;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.22>== 입원후 진찰코드 발생안됨..";
                            //ROW 높이 설정
                            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                        }else if(nCNT >= 1 && nAmtsum == 0)
                        {
                            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = strInDate;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.22>== 입원후 진찰금액이 0 입니다..";
                            //ROW 높이 설정
                            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                        }
                    }    
                }

                dt.Dispose();
                dt = null;

                //ER내원후 입원환자 진찰료 발생 점검
                dt = cSQL.sel_DailyCheck_JumGum22_3(pDbCon, strDate);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum22_4(pDbCon, CF.DATE_ADD(clsDB.DbCon, strDate, -1), strDate, dt.Rows[i]["Pano"].ToString().Trim());

                    if(rs.Rows.Count >0)
                    {
                        strInDate = rs.Rows[0]["InDate"].ToString().Trim();

                        rs1 = cSQL.sel_DailyCheck_JumGum22_5(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), strDate, strInDate);

                        nCNT = Convert.ToInt32(VB.Val(rs1.Rows[0]["Cnt"].ToString().Trim()));
                        nAmtsum = Convert.ToInt32(VB.Val(rs1.Rows[0]["Amt"].ToString().Trim()));

                        rs1.Dispose();
                        rs1 = null;

                        if(dt.Rows[i]["DeptCode"].ToString().Trim() =="NP" && dt.Rows[i]["Bi"].ToString().Trim() == "21")
                        {
                            nAmtsum = 1;
                        }

                        if(nCNT == 0)
                        {
                            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = strInDate;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.22>== 입원후 진찰코드 발생안됨..";
                            //ROW 높이 설정
                            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                        }
                        else if(nCNT >= 1&& nAmtsum == 0)
                        {
                            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = strInDate;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.22>== 입원후 진찰금액이 0입니다..";
                            //ROW 높이 설정
                            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                        }
                    }

                    rs.Dispose();
                    rs = null; 

                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[21, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 23 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 23 전체 부분
                ssListChk1.ActiveSheet.Cells[22, 0].Text = "";
                #region 점검23
                dt = cSQL.sel_DailyCheck_JumGum23(pDbCon, strDate);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검23 : 약제상환 차액 - 금액발생 점검";
                    //ROW 높이 설정
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                }

                dt.Dispose();
                dt = null;

                dt = cSQL.sel_DailyCheck_JumGum23_1(pDbCon, strDate);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum23_2(pDbCon, dt.Rows[i]["TRSNO"].ToString().Trim());

                    if (dt.Rows[i]["Amt64"].ToString().Trim() != rs.Rows[0]["AMT"].ToString().Trim())
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검23 : 약제상환 차액 점검";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;

                }

                dt.Dispose();
                dt = null;

                dt = cSQL.sel_DailyCheck_JumGum23_3(pDbCon, strDate);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum23_4(pDbCon, dt.Rows[i]["TRSNO"].ToString().Trim());

                    if (dt.Rows[i]["AMT64"].ToString().Trim() != rs.Rows[0]["AMT"].ToString().Trim())
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검23 : 약제상환 차액 점검";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;

                }

                dt.Dispose();
                dt = null;

                dt = cSQL.sel_DailyCheck_JumGum23_5(pDbCon, strDate);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    rs = cSQL.sel_DailyCheck_JumGum23_6(pDbCon, dt.Rows[i]["TRSNO"].ToString().Trim(), 0);

                    if (dt.Rows[i]["AMT"].ToString().Trim() != rs.Rows[0]["AMT64"].ToString().Trim() && rs.Rows[0]["GBIPD"].ToString().Trim() == "0")
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검23 : 약제상환 차액 점검";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs = cSQL.sel_DailyCheck_JumGum23_6(pDbCon, dt.Rows[i]["TRSNO"].ToString().Trim(), 1);

                    if (dt.Rows[i]["AMT"].ToString().Trim() != rs.Rows[0]["AMT_SLIP"].ToString().Trim())
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "점검23 : 약제상환 차액 점검";
                        //ROW 높이 설정
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                    }

                    rs.Dispose();
                    rs = null;

                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[22, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 24 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 24 전체 부분
                ssListChk1.ActiveSheet.Cells[23, 0].Text = "";
                #region 점검24
                dt = cSQL.sel_DailyCheck_JumGum24(pDbCon, strDate, CF.DATE_ADD(clsDB.DbCon, strDate, 1));

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (String.Compare(dt.Rows[i]["DATE1"].ToString().Trim(), VB.Left(strDate, 4) + "-01-01") < 0)
                        {
                            rs = cSQL.sel_DailyCheck_JumGum24_1(pDbCon, dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["DeptCode"].ToString().Trim(), strDate);

                            if (rs.Rows[0]["NAL"].ToString().Trim() == "0")
                            {
                                rs1 = cSQL.sel_DailyCheck_JumGum24_2(pDbCon, dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["DeptCode"].ToString().Trim(), strDate);

                                if (rs1.Rows[0]["NAL"].ToString().Trim() == "0")
                                {
                                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<점검.24>== 발생일자 : " + strDate + " " + dt.Rows[i]["PANO"].ToString().Trim() + " 입원환자-진찰료차액 확인요망!!";
                                    //ROW 높이 설정
                                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                                }

                                rs1.Dispose();
                                rs1 = null;

                            }

                            rs.Dispose();
                            rs = null;

                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[23, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 25 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 25 전체 부분
                ssListChk1.ActiveSheet.Cells[24, 0].Text = "";
                #region 점검25
                //선택진료관련 점검

                //입원마스터 기준 선택진료점검
                dt = cSQL.sel_DailyCheck_JumGum25(pDbCon, strDate);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strDrcode = dt.Rows[i]["DrCode"].ToString().Trim();
                        nIPDNO = Convert.ToInt32(VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim()));

                        if (ComFunc.Read_Pano_SELECT_MST(clsDB.DbCon, strPano, "I", strDrcode, strDate, nIPDNO) == "OK" && VB.Left(dt.Rows[i]["Bi"].ToString().Trim(), 1) != "2")
                        {
                            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = strPano;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<25.점검>== " + strDept + "(" + strDrcode + ") " + "선택의사인데 입원MST 는 선택아님...";
                            //ROW 높이 설정
                            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                        }
                        Cursor.Current = Cursors.WaitCursor;
                    }
                }

                dt.Dispose();
                dt = null;

                //TRAMS 기준 선택진료 점검 
                dt = cSQL.sel_DailyCheck_JumGum25_1(pDbCon, strDate);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strDrcode = dt.Rows[i]["DrCode"].ToString().Trim();
                        nIPDNO = Convert.ToInt32(VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim()));

                        if (ComFunc.Read_Pano_SELECT_MST(clsDB.DbCon, strPano, "I", strDrcode, strDate, nIPDNO) == "OK" && VB.Left(dt.Rows[i]["Bi"].ToString().Trim(), 1) != "2")
                        {
                            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = strPano;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<25.점검>== " + strDept + "(" + strDrcode + ") " + "선택의사인데 입원TRANS 는 선택아님...";
                            //ROW 높이 설정
                            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                        }
                        Cursor.Current = Cursors.WaitCursor;
                    }
                }

                dt.Dispose();
                dt = null;

                strTemp = "";
                //점검일 전날 선택 퇴원명단
                dt = cSQL.sel_DailyCheck_JumGum25_2(pDbCon, CF.DATE_ADD(clsDB.DbCon, strDate, -1));

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strDrcode = dt.Rows[i]["DrCode"].ToString().Trim();
                        nIPDNO = Convert.ToInt32(VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim()));

                        strTemp = "Y";
                    }
                }

                dt.Dispose();
                dt = null;

                if (strTemp == "Y")
                {
                    Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = "입원환자";
                    Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<25.점검>== 어제일자 선택대상 퇴원 - 해지확인!!";
                    //ROW 높이 설정
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                }
                #endregion
                ssListChk1.ActiveSheet.Cells[24, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 26 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 26 전체 부분
                ssListChk1.ActiveSheet.Cells[25, 0].Text = "";
                #region 점검26
                //입원마스터 기준 선택진료점검
                dt = cSQL.sel_DailyCheck_JumGum26(pDbCon, strDate);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        nAmt1 = Convert.ToInt32(VB.Val(dt.Rows[i]["amt"].ToString().Trim()));
                        nIPDNO = Convert.ToInt32(VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim()));

                        rs = cSQL.sel_DailyCheck_JumGum26_1(pDbCon, strPano, strDate);

                        nAmt2 = 0;
                        if (rs.Rows.Count > 0)
                        {
                            nAmt2 = Convert.ToInt32(VB.Val(rs.Rows[0]["misuamt"].ToString().Trim()));
                        }

                        if (nAmt1 != nAmt2)
                        {
                            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = strPano;
                            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<26.점검>==" + "입원미수:" + nAmt1 + "원 , 발생미수:" + nAmt2 + "원 미수발생점검";
                            //ROW 높이 설정
                            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
                ssListChk1.ActiveSheet.Cells[25, 0].Text = "TRUE";
                lblSearch.Text = "[ 점검 27 ] * 점검중 *";
                Application.DoEvents();
                #endregion

                #region 점검 27 전체 부분
                ssListChk1.ActiveSheet.Cells[26, 0].Text = "";
                #region 점검27
                dt = cSQL.sel_DailyCheck_JumGum27(pDbCon, strDate);

                if(dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();

                        nIPDNO = Convert.ToInt32(VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim()));
                        nTRSNo = Convert.ToInt32(VB.Val(dt.Rows[i]["TRSNO"].ToString().Trim()));

                        rs = cSQL.sel_DailyCheck_JumGum27_1(pDbCon, strPano, strDate, nIPDNO.ToString(), nTRSNo.ToString());

                        if(rs.Rows.Count > 0)
                        {
                            nQty = Convert.ToInt32(VB.Val(rs.Rows[0]["NQTY"].ToString().Trim()));

                            if(nQty > 0)
                            {
                                Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                                Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = strPano;
                                Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<27.점검>==" + "DRG 입원 SLIP 차액발생 심사완료 취소요망.";
                                //ROW 높이 설정
                                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
                            }
                        }

                        rs.Dispose();
                        rs = null;
                    }
                }

                dt.Dispose();
                dt = null;
                #endregion
                ssListChk1.ActiveSheet.Cells[26, 0].Text = "TRUE";
                lblSearch.Text = "* 점검완료 *";
                Application.DoEvents();
                #endregion
                #endregion
            }
            else if (tabControl1.SelectedTab == SSTab2)
            {
                #region 개인점검1
                ssListChk2.ActiveSheet.Cells[0, 0].Text = "";
                i = 0;
                strPano = "";
                strDept = "";
                strPart = "";
                OAmt = 0;
                CAmt = 0;

                strDate = dtpDate.Text.Trim();
                //strPart = "45316";
                strPart = clsType.User.IdNumber.Trim();

                //현금영수 30만 이상 의무사항 점검
                dt = cSQL.sel_DailyCheck_Search1(pDbCon, strDate, strPart);

                for(i=0; i> dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();
                    strDept = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    OAmt = VB.Fix(Convert.ToInt32(VB.Val(dt.Rows[i]["AMT"].ToString().Trim())) / 10) * 10;

                    rs1 = cSQL.sel_DailyCheck_Search1_1(pDbCon, strDate, strPart, strPano, strDept);

                    CAmt =  Convert.ToInt32(VB.Val(rs1.Rows[0]["CardAMT"].ToString().Trim()));

                    rs1.Dispose();
                    rs1 = null;

                    if(OAmt > CAmt)
                    {
                        Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = strPano;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = "";
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = strDept;
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "==<개인점검.1>== " + "*입원발생금 : " + OAmt +" *카드+현금승인 : " + CAmt;

                        //Row 높이 설정 
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);

                    }
                }

                dt.Dispose();
                dt = null;

                ssListChk2.ActiveSheet.Cells[0, 0].Text = "TRUE";
                #endregion
            }            

            #region 17:00이후 점검 인설트 
            if (String.Compare(cpublic.strSysTime, "17:00") >= 0)
            {
                clsDB.setBeginTran(pDbCon);

                try
                {
                    SqlErr = cSQL.ins_DailyCheck_Insert(pDbCon, clsType.User.IdNumber, ref intRowAffected);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }
                    if (SqlErr == "")
                    {
                        clsDB.setCommitTran(pDbCon);
                    }
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ex.Message);
                }
            }
            #endregion

            ComFunc.MsgBox("오늘 하루도 대단히 수고하셨습니다.."+ ComNum.VBLF +"행복한 하루 되십시오..^^" + ComNum.VBLF +"입원 점검을 완료하였습니다..");

            Cursor.Current = Cursors.Default;
        }

        void SunSuNap_II(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {
            int i = 0;
            string strDate = "";
            DataTable dt = null;
            
            strDate = dtpDate.Text.Trim();

            dt = cSQL.sel_DailyCheck_SunsuNap(pDbCon, strDate);

            if(dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = Spd.ActiveSheet.RowCount + 1;
                Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.Pano].Text = dt.Rows[i]["PtNO"].ToString().Trim();
                Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.OUTDATE].Text = dt.Rows[i]["BDate"].ToString().Trim();
                Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, (int)clsComPmpaSpd.enmDailyCheck_sel.bigo].Text = "입원 선수납 처리 대상 있음!!";
                //ROW 높이 설정
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 2);
            }

            dt.Dispose();
            dt = null;
        }

        void eSave(PsmhDb pDbCon, int argRow, int argCol)
        {
            //int intRowAffected = 0;

            //string strROWID = "", strRemark = "", strOldRemark = "", strGubun = "0";

            //string SQL = "";
            //string SqlErr = ""; //에러문 받는 변수

            //if (argCol == 7)
            //{
            //    strRemark = ssList.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.Remark].Text;
            //    strROWID = ssList.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.ROWID].Text;
            //    strOldRemark = ssList.ActiveSheet.Cells[argRow, (int)clsComPmpaSpd.enmIpdMirCheckUpdate.OldReMark].Text;

            //    if (strRemark != strOldRemark)
            //    {
            //        clsDB.setBeginTran(pDbCon);

            //        if (opt2.Checked == true)
            //        {
            //            strGubun = "2";
            //        }

            //        try
            //        {
            //            SqlErr = cSQL.udt_IpdMirCheckUpdate2_update1(pDbCon, strRemark, strROWID, clsType.User.IdNumber, strGubun, ref intRowAffected);

            //            if (SqlErr != "")
            //            {
            //                clsDB.setRollbackTran(pDbCon);
            //                ComFunc.MsgBox(SqlErr);
            //                ComFunc.MsgBox("MISU_BALCHECK_PANO에 자료를 등록중 오류발생", "RollBack");
            //                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
            //                return;
            //            }
            //            if (SqlErr == "")
            //            {
            //                clsDB.setCommitTran(pDbCon);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            clsDB.setRollbackTran(pDbCon);
            //            ComFunc.MsgBox(ex.Message);
            //        }
            //    }
            //}
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            // strTitle = "내시경 예약자 명단 " + "(" + dtpFDate.Text.Trim() + ")";
            strTitle = "입원 일일점검 내역";

            //strSubTitle += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);

  //        strSubTitle += SPR.setSpdPrint_String("작업월:" + cboYYMM.Text.Trim(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);

            strSubTitle += SPR.setSpdPrint_String("       출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
            strSubTitle += " PAGE:" + "/p";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);

            //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            //strFooter = "";

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 100, 0, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, true, false, (float)1);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;


            #endregion
        }
    }
}
