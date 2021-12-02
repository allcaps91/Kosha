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
    /// Class Name      : PmpaMagam
    /// File Name       : frmPmpaMagamMisubs34.cs
    /// Description     : 조합미수청구액조회및자료변경
    /// Author          : 김해수
    /// Create Date     : 2018-09-03   
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs34.frm(FrmMisuView) >> frmPmpaMagamMisuView.cs 폼이름 재정의" />
    public partial class frmPmpaMagamMisuView : Form
    {
        #region 클래스 선언 및 etc....

        clsSpread methodSpd = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();
        clsComPmpaSQL cSQL = new clsComPmpaSQL();
        clsPmpaMisu cPM = new clsPmpaMisu();
        
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

        public frmPmpaMagamMisuView()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaMagamMisuView(MainFormMessage pform)
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
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnReView.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.optD.CheckedChanged += new EventHandler(eRdoChecked);
            this.optM.CheckedChanged += new EventHandler(eRdoChecked);
            //this.btnSearch.Click += new EventHandler(eBtnSearch);
            //this.btnSearch2.Click += new EventHandler(eBtnSearch);
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
            //this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            ////this.ssList.MouseUp += new MouseEventHandler(eMouseEvent);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            //this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            //this.ssList.EditModeOff += new EventHandler(eSpreadEditOff);

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
            CF.ComboMonth_Set(cboYYMM, 12);
            cboJong.Items.Add("0.전체");
            cboJong.Items.Add("1.건강보험");
            cboJong.Items.Add("2.의료급여");
            cboJong.Items.Add("3.산재");
            cboJong.Items.Add("4.자보");
            cboJong.SelectedIndex = 1;
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
                //툴팁
                setTxtTip();

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();
            }
        }

        void eRdoChecked(object sender, EventArgs e)
        {
            if(sender == this.optD)
            {
                gbYYMM.Text = "미수발생월";
            }else if(sender == this.optM)
            {
                gbYYMM.Text = "통계월별";
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

        void eBtnClick(object sender, EventArgs e) 
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if(sender == this.btnReView)
            {
                screen_clear("Clear");
                cboYYMM.Focus();
            }else if(sender == this.btnView)
            {
                screen_display();
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            //if (sender == this.btnSearch)
            //{
            
            //}
        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            if (Job == "")
            {

            }
            else if (Job == "Clear") 
            {
                btnView.Enabled = true;
                gbIO.Enabled = true;
                gbYYMM.Enabled = true;
                gbJong.Enabled = true;
                ssList.ActiveSheet.RowCount = 0;
                ssList.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssList.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;  
                //
            }
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList);
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {

            DataTable dt = null;
            DataTable dt1 = null;
            FarPoint.Win.LineBorder lb = new FarPoint.Win.LineBorder(Color.White, 1, true, false, false, true);

            int i = 0, j = 0, nRow = 0, nAmt = 0 ;
            string strYYMM = "", strFDate = "", strTdate = "", strJong = "", strOldData = "", strNewData = "";
            long[,] FnTotAmt = new long[2, 6];
            string strGubun = "-1", strIOGubun = "-1",strSort ="-1";

            btnView.Enabled = false;
            gbIO.Enabled = false;
            gbYYMM.Enabled = false;
            gbJong.Enabled = false;
            Spd.Enabled = true;

            //sheet Clear
            Spd.ActiveSheet.RowCount = 30;

            strYYMM = ComFunc.LeftH(cboYYMM.Text, 4) + ComFunc.MidH(cboYYMM.Text, 8, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);
            strJong = ComFunc.LeftH(cboJong.Text, 1);

            //Sort 구분
            if (optSort1.Checked == true)
            {
                strSort = "0";
            }
            else if (optSort2.Checked == true)
            {
                strSort = "1";
            }

            //작업 구분 
            if (optD.Checked == true)
            {
                strGubun = "0";
            }
            else if (optM.Checked == true)
            {
                strGubun = "1";
            }

            //외래, 입원 구분
            if(optI.Checked == true)
            {
                strIOGubun = "0";
            }else if(optO.Checked == true)
            {
                strIOGubun = "1";
            }else if(optAll.Checked == true)
            {
                strIOGubun = "-1";
            }
            

            //누적할 배열을 Clear
            for (i = 0; i < 2; i++)
            {
                for (j = 0; j < 6; j++)
                {
                    FnTotAmt[i, j] = 0;
                }
            }

            //자료조회
            dt = cSQL.sel_VIEW04_Check1(pDbCon, strFDate, strTdate, strYYMM, strJong, strSort, strGubun, strIOGubun);

            Spd.ActiveSheet.RowCount = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strNewData = dt.Rows[i]["Class"].ToString().Trim();

                if (String.Compare(strNewData, "03") <= 0)
                {
                    strNewData = "01";
                }
                if (strOldData != strNewData && strOldData != "")
                {
                    #region 소계부분
                    nRow += 1;
                    if (nRow > Spd.ActiveSheet.RowCount)
                    {
                        Spd.ActiveSheet.RowCount = nRow;
                    }
                    Spd.ActiveSheet.Cells[nRow - 1, 1].Text = "**소계**";

                    for (j = 0; j < 6; j++)
                    {

                        //Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, j + 7].Text = VB.Format(FnTotAmt[0, j], "###,###,###,##0");
                        Spd.ActiveSheet.Cells[nRow - 1, j + 7].Text = VB.Format(FnTotAmt[0, j], "###,###,###,##0");
                        FnTotAmt[0, j] = 0;
                    }                    
                    #endregion
                }

                nRow += 1;
                if (nRow > Spd.ActiveSheet.RowCount)
                {
                    Spd.ActiveSheet.RowCount = nRow;
                }

                if (strOldData != strNewData)
                {
                    switch (strNewData)
                    {
                        case "01":
                            Spd.ActiveSheet.Cells[nRow - 1, 0].Text = "보험";
                            break;
                        case "04":
                            Spd.ActiveSheet.Cells[nRow - 1, 0].Text = "보호";
                            break;
                        case "05":
                            Spd.ActiveSheet.Cells[nRow - 1, 0].Text = "산재";
                            break;
                        case "07":
                            Spd.ActiveSheet.Cells[nRow - 1, 0].Text = "자보";
                            break;
                    }
                    strOldData = strNewData;
                }

                Spd.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["BDate"].ToString().Trim();
                Spd.ActiveSheet.Cells[nRow - 1, 2].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                Spd.ActiveSheet.Cells[nRow - 1, 3].Text = dt.Rows[i]["IpdOpd"].ToString().Trim(); 
                Spd.ActiveSheet.Cells[nRow - 1, 4].Text = cPM.READ_MisuBunName(dt.Rows[i]["Bun"].ToString().Trim());
                
                Spd.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["MirYYMM"].ToString().Trim();
                Spd.ActiveSheet.Cells[nRow - 1, 6].Text = dt.Rows[i]["TongGbn"].ToString().Trim();
                nAmt = Convert.ToInt32(VB.Val(dt.Rows[i]["NAmt"].ToString().Trim()));

                dt1 = cSQL.sel_VIEW04_Check3(pDbCon, dt.Rows[i]["edimirno"].ToString().Trim());

                if (dt1.Rows.Count > 0)
                {
                    if(dt1.Rows[0]["DTHU"].ToString().Trim() == "Y")
                    {
                        Spd.ActiveSheet.Cells[nRow - 1, 4].Text = "HU분야";
                    }
                    
                }
                dt1.Dispose();
                dt1 = null;

                switch (dt.Rows[i]["TongGbn"].ToString().Trim())
                {
                    case "1":
                        Spd.ActiveSheet.Cells[nRow - 1, 7].Text = VB.Format(nAmt, "###,###,###,##0");//처음청구
                        //ComFunc.MsgBox(nAmt.ToString() + " " + nRow);
                        FnTotAmt[0, 0] += nAmt;
                        FnTotAmt[1, 0] += nAmt;
                        break;
                    case "2":
                        Spd.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Format(nAmt, "###,###,###,##0");//재청구
                        FnTotAmt[0, 1] = FnTotAmt[0, 1] + nAmt;
                        FnTotAmt[1, 1] = FnTotAmt[1, 1] + nAmt;
                        break;
                    case "3":
                        Spd.ActiveSheet.Cells[nRow - 1, 9].Text = VB.Format(nAmt, "###,###,###,##0");///추가청구
                        FnTotAmt[0, 2] = FnTotAmt[0, 2] + nAmt;
                        FnTotAmt[1, 2] = FnTotAmt[1, 2] + nAmt;
                        break;
                    case "4":
                        Spd.ActiveSheet.Cells[nRow - 1, 10].Text = VB.Format(nAmt, "###,###,###,##0");//이의신청
                        FnTotAmt[0, 3] = FnTotAmt[0, 3] + nAmt;
                        FnTotAmt[1, 3] = FnTotAmt[1, 3] + nAmt;
                        break;
                    case "5":
                        Spd.ActiveSheet.Cells[nRow - 1, 11].Text = VB.Format(nAmt, "###,###,###,##0");//이의신청
                        FnTotAmt[0, 4] = FnTotAmt[0, 4] + nAmt;
                        FnTotAmt[1, 4] = FnTotAmt[1, 4] + nAmt;
                        break;
                    default:
                        Spd.ActiveSheet.Cells[nRow - 1, 12].Text = VB.Format(nAmt, "###,###,###,##0");//기타창구
                        FnTotAmt[0, 5] = FnTotAmt[0, 5] + nAmt;
                        FnTotAmt[1, 5] = FnTotAmt[1, 5] + nAmt;
                        break;
                }

                Spd.ActiveSheet.Cells[nRow - 1, 13].Text = dt.Rows[i]["Remark"].ToString().Trim();

                //2016-05-03 응급실 입원건 체크
                if (strJong == "1" || strJong == "2")
                {
                    if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                    {
                        dt1 = cSQL.sel_VIEW04_Check2(pDbCon, dt.Rows[i]["edimirno"].ToString().Trim());

                        if (dt1.Rows.Count > 0)
                        {
                            for(j = 1; j < Spd.ActiveSheet.ColumnCount - 1; j++)
                            {
                                Spd.ActiveSheet.Cells[nRow - 1, j].ForeColor = Color.Blue;
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                }
                Spd.ActiveSheet.Cells[nRow - 1 , 0].Border = lb;
                //nRow++;
            }
            dt.Dispose();
            dt = null;

            #region 소계부분
            nRow += 1;

            if (nRow > Spd.ActiveSheet.RowCount)
            {
                Spd.ActiveSheet.RowCount = nRow;
            }
            Spd.ActiveSheet.Cells[nRow - 1, 1].Text = "**소계**";
           
            for (j = 0; j < 6; j++)
            {
                
                Spd.ActiveSheet.Cells[nRow - 1, j + 7].Text = VB.Format(FnTotAmt[0, j], "###,###,###,##0");
                FnTotAmt[0, j] = 0;
            }
            #endregion

            #region 합계
            nRow += 1;
            if (nRow > Spd.ActiveSheet.RowCount)
            {
                Spd.ActiveSheet.RowCount = nRow;
            }

            Spd.ActiveSheet.Cells[nRow - 1, 1].Text = "**합계**";
            
            for (j = 0; j < 6; j++)
            {
                Spd.ActiveSheet.Cells[nRow - 1, j + 7].Text = VB.Format(FnTotAmt[1, j], "###,###,###,##0");
                FnTotAmt[1, j] = 0;
            }
            #endregion        

            Spd.ActiveSheet.RowCount = nRow;
     
            //CS.setColMerge(Spd, 0);

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}
                ePrint();
            }
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
            strTitle = "월 별  조 합 미 수  청 구 액";

            //strSubTitle += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
            if(optD.Checked == true)
            {
                strSubTitle += SPR.setSpdPrint_String("발생월:" + cboYYMM.Text.Trim(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
            }
            else
            {
                strSubTitle += SPR.setSpdPrint_String("진료월:" + cboYYMM.Text.Trim(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
            }
            
            strSubTitle += SPR.setSpdPrint_String("       인쇄 일자 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);

            //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            //strFooter = "";

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 100, 0, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;


            #endregion
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
            }
        }
    }       
}
